
namespace GBX.NET.Engines.MetaNotPersistent;

public partial class NPlugItemPlacement_SPlacementOption
{
    public Dictionary<string, string> RequiredTags { get; set; } = [];

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (rw.Reader is not null)
        {
            var count = rw.Reader.ReadInt32();
            RequiredTags = new Dictionary<string, string>(count);

            for (var i = 0; i < count; i++)
            {
                var key = rw.Reader.ReadString();
                var value = rw.Reader.ReadString();
                RequiredTags[key] = value;
            }
        }

        if (rw.Writer is not null)
        {
            rw.Writer.Write(RequiredTags.Count);

            foreach (var pair in RequiredTags)
            {
                rw.Writer.Write(pair.Key);
                rw.Writer.Write(pair.Value);
            }
        }
    }
}
