
namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClipGroup
{
    public readonly record struct ClipTrigger(CGameCtnMediaClip Clip, Trigger Trigger) : IDeepCloneable
    {
        private readonly NodeRef<CGameCtnMediaClip>? clipRef = CreateRef(Clip);

        public CGameCtnMediaClip Clip
        {
            get => clipRef?.Resolve()!;
            init => clipRef = CreateRef(value);
        }

        public NodeRefView References => new(this);

        private ClipTrigger(NodeRef<CGameCtnMediaClip>? reference, Trigger trigger) : this((CGameCtnMediaClip)null!, trigger)
        {
            clipRef = reference;
        }

        private static NodeRef<CGameCtnMediaClip> CreateRef(CGameCtnMediaClip clip)
        {
            return new NodeRef<CGameCtnMediaClip>
            {
                Node = clip
            };
        }

        public readonly struct NodeRefView
        {
            private readonly ClipTrigger owner;
            internal NodeRefView(ClipTrigger owner) => this.owner = owner;
            public NodeRef<CGameCtnMediaClip>? Clip => owner.clipRef;
        }

        internal static ClipTrigger FromReference(NodeRef<CGameCtnMediaClip> reference, Trigger trigger) => new(reference, trigger);

        object IDeepCloneable.DeepClone(DeepCloneContext context) => new ClipTrigger(context.Clone(clipRef), context.Clone(Trigger)!);
    }

    private List<ClipTrigger>? clips;
    public List<ClipTrigger> Clips
    {
        get => clips ??= [];
        set => clips = value;
    }

    private static void ReadClips(CGameCtnMediaClipGroup n, GbxReader r, int version)
    {
        var clipBuffer = r.ReadListNodeRef_deprec<CGameCtnMediaClip>();
        Span<Trigger> triggerBuffer = r.ReadArrayReadable<Trigger>(version: version);

        n.Clips = new List<ClipTrigger>(clipBuffer.Count);

        for (var i = 0; i < clipBuffer.Count; i++)
        {
            n.Clips.Add(ClipTrigger.FromReference(clipBuffer.Get(i), triggerBuffer[i]));
        }
    }

    private static void WriteClips(CGameCtnMediaClipGroup n, GbxWriter w, int version)
    {
        w.WriteDeprecVersion();
        w.Write(n.Clips.Count);
        foreach (var pair in n.Clips)
        {
            w.WriteNodeRef(pair.References.Clip);
        }
        w.WriteListWritable(n.Clips.Select(x => x.Trigger).ToList(), version: version);
    }

    public override IEnumerable<KeyValuePair<string, NodeRef>> EnumerateReferences()
    {
        foreach (var reference in base.EnumerateReferences()) yield return reference;
        for (var i = 0; i < Clips.Count; i++)
        {
            if (Clips[i].References.Clip is { } reference)
                yield return new($"Clips[{i}].Clip", reference);
        }
    }

    public partial class Chunk0307A000
    {
        public override void Read(CGameCtnMediaClipGroup n, GbxReader r)
        {
            ReadClips(n, r, version: 0);
        }

        public override void Write(CGameCtnMediaClipGroup n, GbxWriter w)
        {
            WriteClips(n, w, version: 0);
        }
    }

    public partial class Chunk0307A001
    {
        public override void Read(CGameCtnMediaClipGroup n, GbxReader r)
        {
            ReadClips(n, r, version: 1);
        }

        public override void Write(CGameCtnMediaClipGroup n, GbxWriter w)
        {
            WriteClips(n, w, version: 1);
        }
    }

    public partial class Chunk0307A002
    {
        public override void Read(CGameCtnMediaClipGroup n, GbxReader r)
        {
            ReadClips(n, r, version: 2);
        }

        public override void Write(CGameCtnMediaClipGroup n, GbxWriter w)
        {
            WriteClips(n, w, version: 2);
        }
    }

    public partial class Chunk0307A003
    {
        public override void Read(CGameCtnMediaClipGroup n, GbxReader r)
        {
            ReadClips(n, r, version: 3);
        }

        public override void Write(CGameCtnMediaClipGroup n, GbxWriter w)
        {
            WriteClips(n, w, version: 3);
        }
    }

    public partial class Trigger
    {
        [Obsolete("Use RefCoord.X instead.")]
        public int U01 { get => RefCoord.X; set => RefCoord = RefCoord with { X = value }; }

        [Obsolete("Use RefCoord.Y instead.")]
        public int U02 { get => RefCoord.Y; set => RefCoord = RefCoord with { Y = value }; }

        [Obsolete("Use RefCoord.Z instead.")]
        public int U03 { get => RefCoord.Z; set => RefCoord = RefCoord with { Z = value }; }

        [Obsolete("Use RefDir instead.")]
        public int U04 { get => (int)RefDir; set => RefDir = (Direction)value; }

        public override string ToString()
        {
            return $"Trigger: {Coords?.Count ?? 0} coords";
        }
    }
}
