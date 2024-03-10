
namespace GBX.NET.Engines.Plug;

public partial class CPlugTreeVisualMip
{
    public IDictionary<float, CPlugTree> Levels { get; set; } = new Dictionary<float, CPlugTree>();

    public partial class Chunk09015002
    {
        public override void Read(CPlugTreeVisualMip n, GbxReader r)
        {
            var length = r.ReadInt32();

            n.Levels.Clear();

            for (var i = 0; i < length; i++)
            {
                var key = r.ReadSingle();
                var value = r.ReadNodeRef<CPlugTree>()!;
                n.Levels.Add(key, value);
            }
        }

        public override void Write(CPlugTreeVisualMip n, GbxWriter w)
        {
            w.Write(n.Levels.Count);

            foreach (var pair in n.Levels)
            {
                w.Write(pair.Key);
                w.WriteNodeRef(pair.Value);
            }
        }
    }
}
