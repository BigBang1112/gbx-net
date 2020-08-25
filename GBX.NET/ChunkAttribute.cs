using System;

namespace GBX.NET
{
    public class ChunkAttribute : Attribute
    {
        public uint ID { get; }
        public uint ClassID => ID & 0xFFFFF000;
        public uint ChunkID => ID & 0xFFF;
        public bool ProcessAsync { get; }

        public ChunkAttribute(uint classID, uint chunkID, bool processAsync = false)
        {
            ID = classID + chunkID;
            ProcessAsync = processAsync;
        }

        public ChunkAttribute(uint classID, uint chunkID) : this(classID, chunkID, false)
        {

        }

        public ChunkAttribute(uint chunkID, bool processAsync = false) : this(chunkID & 0xFFFFF000, chunkID & 0xFFF, processAsync)
        {

        }

        public ChunkAttribute(uint chunkID) : this(chunkID, false)
        {

        }
    }
}