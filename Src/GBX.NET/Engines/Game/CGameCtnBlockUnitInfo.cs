namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockUnitInfo
{
    public External<CGameCtnBlockInfoClip>[]? ClipsNorth { get; set; }
    public External<CGameCtnBlockInfoClip>[]? ClipsEast { get; set; }
    public External<CGameCtnBlockInfoClip>[]? ClipsSouth { get; set; }
    public External<CGameCtnBlockInfoClip>[]? ClipsWest { get; set; }
    public External<CGameCtnBlockInfoClip>[]? ClipsTop { get; set; }
    public External<CGameCtnBlockInfoClip>[]? ClipsBottom { get; set; }

    public partial class Chunk0303600C : IVersionable
    {
        public int Version { get; set; }

        public short? U01;
        public short? U02;
        public int? U03;
        public int? U04;

        public override void Read(CGameCtnBlockUnitInfo n, GbxReader r)
        {
            Version = r.ReadInt32();

            // Version 0 packs six 2-bit counts into a UInt16; later versions use 3-bit counts.
            var bitsPerClipCount = Version == 0 ? 2 : 3;
            var clipCountMask = (1 << bitsPerClipCount) - 1;
            var clipCountBits = Version == 0 ? r.ReadUInt16() : r.ReadInt32();

            var clipCountNorth = clipCountBits & clipCountMask;
            var clipCountEast = clipCountBits >> bitsPerClipCount & clipCountMask;
            var clipCountSouth = clipCountBits >> (bitsPerClipCount * 2) & clipCountMask;
            var clipCountWest = clipCountBits >> (bitsPerClipCount * 3) & clipCountMask;
            var clipCountTop = clipCountBits >> (bitsPerClipCount * 4) & clipCountMask;
            var clipCountBottom = clipCountBits >> (bitsPerClipCount * 5) & clipCountMask;

            n.ClipsNorth = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountNorth)!;
            n.ClipsEast = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountEast)!;
            n.ClipsSouth = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountSouth)!;
            n.ClipsWest = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountWest)!;
            n.ClipsTop = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountTop)!;
            n.ClipsBottom = r.ReadArrayExternalNodeRef<CGameCtnBlockInfoClip>(clipCountBottom)!;

            if (Version >= 2)
            {
                U01 = r.ReadInt16();
                U02 = r.ReadInt16();
            }
            else
            {
                U03 = r.ReadInt32();
                U04 = r.ReadInt32();
            }
        }

        public override void Write(CGameCtnBlockUnitInfo n, GbxWriter w)
        {
            if (Version == 0 && (n.ClipsNorth?.Length > 3
                || n.ClipsEast?.Length > 3
                || n.ClipsSouth?.Length > 3
                || n.ClipsWest?.Length > 3
                || n.ClipsTop?.Length > 3
                || n.ClipsBottom?.Length > 3))
            {
                throw new InvalidOperationException("Version 0 supports at most 3 clips per direction.");
            }

            w.Write(Version);

            var bitsPerClipCount = Version == 0 ? 2 : 3;
            var clipCountBits = (n.ClipsNorth?.Length ?? 0)
                | (n.ClipsEast?.Length ?? 0) << bitsPerClipCount
                | (n.ClipsSouth?.Length ?? 0) << (bitsPerClipCount * 2)
                | (n.ClipsWest?.Length ?? 0) << (bitsPerClipCount * 3)
                | (n.ClipsTop?.Length ?? 0) << (bitsPerClipCount * 4)
                | (n.ClipsBottom?.Length ?? 0) << (bitsPerClipCount * 5);

            if (Version == 0)
            {
                w.Write((ushort)clipCountBits);
            }
            else
            {
                w.Write(clipCountBits);
            }

            foreach (var clip in n.ClipsNorth ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            foreach (var clip in n.ClipsEast ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            foreach (var clip in n.ClipsSouth ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            foreach (var clip in n.ClipsWest ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            foreach (var clip in n.ClipsTop ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            foreach (var clip in n.ClipsBottom ?? [])
            {
                w.WriteNodeRef(clip.Node, clip.File);
            }

            if (Version >= 2)
            {
                w.Write(U01.GetValueOrDefault());
                w.Write(U02.GetValueOrDefault());
            }
            else
            {
                w.Write(U03.GetValueOrDefault());
                w.Write(U04.GetValueOrDefault());
            }
        }
    }

}
