namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public class CrystalElemPool : IReadable
    {
        // magic number for "null" or "end of list"
        private const uint InvalidIndex = 0x7FFFFFFF;

        public List<Handle> RawBuffer { get; } = [];
        public uint FirstFreeHandleIndex { get; private set; }
        public uint FirstUsedHandleIndex { get; private set; }

        public void Read(GbxReader r, int v = 0)
        {
            RawBuffer.Clear();

            var handleCount = r.ReadInt32();
            for (int i = 0; i < handleCount; i++)
            {
                var handle = new Handle
                {
                    IsFree = r.ReadBoolean(),
                    Version = r.ReadInt32() & 0x3FF, // Masked to 10 bits just like the C++ code
                    NextIndex = r.ReadUInt32()
                };

                RawBuffer.Add(handle);
            }

            // Read the linked list heads
            FirstFreeHandleIndex = r.ReadUInt32();
            FirstUsedHandleIndex = r.ReadUInt32();

            // Reconstruct the Doubly-Linked Lists (Prev Pointers)
            // Rebuild the 'Free' and 'Used' lists (this loops twice like the C++ `do...while (uVar10 < 2)`)
            uint[] headIndices = [FirstFreeHandleIndex, FirstUsedHandleIndex];

            foreach (uint headIndex in headIndices)
            {
                uint prevIndex = 0x7FFFFFFF; // Magic number for "Invalid/Null Index"
                uint currentIndex = headIndex;

                while (currentIndex != 0x7FFFFFFF)
                {
                    var handle = RawBuffer[(int)currentIndex];
                    handle.PrevIndex = prevIndex; // Reconstruct the backward pointer

                    prevIndex = currentIndex;
                    currentIndex = handle.NextIndex; // Move to next
                }
            }
        }

        public IEnumerable<Handle> GetActiveHandles()
        {
            uint currentIndex = FirstUsedHandleIndex;

            while (currentIndex != InvalidIndex && currentIndex < RawBuffer.Count)
            {
                var handle = RawBuffer[(int)currentIndex];

                // Replicating the C++ assert: "'!Handle.IsFree' failed."
                // If it's in the Used list, it MUST NOT be marked as free.
                if (handle.IsFree)
                {
                    throw new InvalidDataException(
                        $"Corrupted archive: Handle at pool index {currentIndex} is in the Used list but marked as Free.");
                }

                // Yield the handle to the caller, then jump to the next one in the chain
                yield return handle;

                currentIndex = handle.NextIndex;
            }
        }

        public class Handle
        {
            public bool IsFree { get; set; }
            public int Version { get; set; }
            public uint NextIndex { get; set; }
            public uint PrevIndex { get; set; }
        }
    }
}
