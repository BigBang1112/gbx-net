
namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClipGroup
{
    public readonly record struct ClipTrigger(CGameCtnMediaClip Clip, Trigger Trigger);

    private IList<ClipTrigger> clips = new List<ClipTrigger>();
    public IList<ClipTrigger> Clips { get => clips; set => clips = value; }

    private static void ReadClips(CGameCtnMediaClipGroup n, GbxReader r, int version)
    {
        Span<CGameCtnMediaClip?> clipBuffer = r.ReadArrayNodeRef_deprec<CGameCtnMediaClip>();
        Span<Trigger> triggerBuffer = r.ReadArrayReadable<Trigger>(version);

        n.Clips = new List<ClipTrigger>(clipBuffer.Length);

        for (var i = 0; i < clipBuffer.Length; i++)
        {
            var clip = clipBuffer[i];

            if (clip is null)
            {
                // log warning
            }

            n.Clips.Add(new ClipTrigger(clip!, triggerBuffer[i]));
        }
    }

    private static void WriteClips(CGameCtnMediaClipGroup n, GbxWriter w, int version)
    {
        w.WriteListNodeRef_deprec(n.Clips.Select(x => x.Clip).ToList()!);
        w.WriteListWritable(n.Clips.Select(x => x.Trigger).ToList(), version);
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

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Trigger;
}
