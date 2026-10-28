using GBX.NET.Serialization;

namespace GBX.NET.Engines.Input;

public partial class CInputBindingsConfig
{
    public partial class Binding
    {
        [Obsolete("Use ObjectIndex instead.")]
        public int U01 { get => ObjectIndex; set => ObjectIndex = value; }

        [Obsolete("Use SubDeviceIndex instead.")]
        public int U02 { get => SubDeviceIndex; set => SubDeviceIndex = value; }

        [Obsolete("Use PlayerNumber instead.")]
        public int U03 { get => PlayerNumber; set => PlayerNumber = value; }

        [Obsolete("Use ActionType instead. This value is an action type, not a boolean.")]
        public int IsAnalog { get => ActionType; set => ActionType = value; }
    }

    public partial class Chunk13006004
    {
        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            WithRestoredIdCount(rw, () => rw.ArrayId(ref n.configuredDeviceModels!));
        }
    }

    public partial class Chunk13006005
    {
        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            WithRestoredIdCount(rw, () => rw.ArrayReadableWritable<Binding>(ref n.bindings!, version: 3));
        }
    }

    private static void WithRestoredIdCount(GbxReaderWriter rw, Action archive)
    {
        var readCount = rw.Reader?.IdDict.Count ?? 0;
        var writeCount = rw.Writer?.IdDict.Count ?? 0;

        try
        {
            archive();
        }
        finally
        {
            // These chunks may refer to earlier IDs, but their new IDs must not escape the chunk.
            // Keep the ID version: unlike an encapsulation, this does not start a new ID stream.
            if (rw.Reader is { } reader)
            {
                foreach (var key in reader.IdDict.Keys.Where(key => (key & 0x3FFFFFFF) > readCount).ToArray())
                {
                    reader.IdDict.Remove(key);
                }
            }

            if (rw.Writer is { } writer)
            {
                foreach (var key in writer.IdDict.Where(pair => pair.Value >= writeCount).Select(pair => pair.Key).ToArray())
                {
                    writer.IdDict.Remove(key);
                }
            }
        }
    }
}
