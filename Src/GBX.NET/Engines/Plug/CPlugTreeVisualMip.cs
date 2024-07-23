
namespace GBX.NET.Engines.Plug;

public partial class CPlugTreeVisualMip
{
    private IList<Level> levels = new List<Level>();
    [AppliedWithChunk<Chunk09015002>]
    public IList<Level> Levels { get => levels; set => levels = value; }

    public partial class Chunk09015002
    {
        public override void Read(CPlugTreeVisualMip n, GbxReader r)
        {
            var length = r.ReadInt32();

            n.Levels.Clear();

            for (var i = 0; i < length; i++)
            {
                var farZ = r.ReadSingle();
                var tree = r.ReadNodeRef<CPlugTree>()!;
                n.Levels.Add(new(farZ, tree));
            }
        }

        public override void Write(CPlugTreeVisualMip n, GbxWriter w)
        {
            w.Write(n.Levels.Count);

            foreach (var pair in n.Levels)
            {
                w.Write(pair.FarZ);
                w.WriteNodeRef(pair.Tree);
            }
        }
    }

    public sealed record Level(float FarZ, CPlugTree Tree);
}
