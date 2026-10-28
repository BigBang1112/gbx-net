
namespace GBX.NET.Engines.MetaNotPersistent;

public partial class NPlugItem_SVariant
{
    public Dictionary<string, string> Tags { get; set; } = [];

    public CMwNod? EntityModel { get => entityModelRef?.Resolve(); set => NodeRef.SetEmbedded(ref entityModelRef, value); }

    private bool hiddenInManualCycle;
    public bool HiddenInManualCycle { get => hiddenInManualCycle; set => hiddenInManualCycle = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (rw.Reader is not null)
        {
            var count = rw.Reader.ReadInt32();
            Tags = new Dictionary<string, string>(count);

            for (var i = 0; i < count; i++)
            {
                var key = rw.Reader.ReadString();
                var value = rw.Reader.ReadString();
                Tags[key] = value;
            }
        }

        if (rw.Writer is not null)
        {
            rw.Writer.Write(Tags.Count);

            foreach (var pair in Tags)
            {
                rw.Writer.Write(pair.Key);
                rw.Writer.Write(pair.Value);
            }
        }

        rw.NodeRef(ref entityModelRef);

        if (v >= 1)
        {
            rw.Boolean(ref hiddenInManualCycle);
        }
    }
}
