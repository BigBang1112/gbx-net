
namespace GBX.NET.Engines.Plug;

public partial class CPlugTreeVisualMip
{
    private List<Level> levels = [];
    /// <remarks>Applied with: 0x002 body chunk</remarks>
    [AppliedWithChunk<Chunk09015002>]
    public List<Level> Levels { get => levels; set => levels = value; }

    public partial class Chunk09015002
    {
        public override void Read(CPlugTreeVisualMip n, GbxReader r)
        {
            var length = r.ReadInt32();

            n.Levels.Clear();

            for (var i = 0; i < length; i++)
            {
                var farZ = r.ReadSingle();
                var level = new Level(farZ, null);
                r.ReadNodeRef(level.References.Tree);
                n.Levels.Add(level);
            }
        }

        public override void Write(CPlugTreeVisualMip n, GbxWriter w)
        {
            w.Write(n.Levels.Count);

            foreach (var pair in n.Levels)
            {
                w.Write(pair.FarZ);
                w.WriteNodeRef(pair.References.Tree);
            }
        }
    }

    public sealed record Level : IDeepCloneable
    {
        private NodeRef<CPlugTree>? treeRef;
        public float FarZ { get; init; }
        public CPlugTree? Tree { get => treeRef?.Resolve(); init => NodeRef.SetEmbedded(ref treeRef, value); }
        public NodeRefView References => new(this);

        public Level(float farZ, CPlugTree? tree)
        {
            FarZ = farZ;
            Tree = tree;
        }

        public readonly struct NodeRefView
        {
            private readonly Level owner;
            internal NodeRefView(Level owner) => this.owner = owner;
            public NodeRef<CPlugTree> Tree => NodeRef.GetOrCreate(ref owner.treeRef);
        }

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = new Level(FarZ, null);
            context.Register(this, clone);
            clone.treeRef = context.Clone(treeRef)!;
            return clone;
        }
    }
}
