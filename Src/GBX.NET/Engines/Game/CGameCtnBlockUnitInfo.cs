using GBX.NET.Components;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockUnitInfo
{
    public CGameCtnBlockInfoClip[]? ClipsNorth { get; set; }
    public CGameCtnBlockInfoClip[]? ClipsEast { get; set; }
    public CGameCtnBlockInfoClip[]? ClipsSouth { get; set; }
    public CGameCtnBlockInfoClip[]? ClipsWest { get; set; }
    public CGameCtnBlockInfoClip[]? ClipsTop { get; set; }
    public CGameCtnBlockInfoClip[]? ClipsBottom { get; set; }

    public partial class Chunk03036006
    {
        public bool U01;
        public (bool, int) U02;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            for (var i = 0; i < 4; i++)
            {
                rw.Boolean(ref U02.Item1);
                rw.Int32(ref U02.Item2);
            }
        }
    }

    public partial class Chunk03036007
    {
        public External<CMwNod>[]? U01;

        public override void Read(CGameCtnBlockUnitInfo n, GbxReader r)
        {
            U01 = new External<CMwNod>[4]; // or pylons?

            for (var i = 0; i < U01.Length; i++)
            {
                var node = r.ReadNodeRef<CMwNod>(out GbxRefTableFile? file);
                U01[i] = new(node, file);
            }
        }

        public override void Write(CGameCtnBlockUnitInfo n, GbxWriter w)
        {
            foreach (var e in U01!)
            {
                w.WriteNodeRef(e.Node, e.File);
            }
        }
    }

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

            if (Version == 0)
            {
                //rw.Int16();
                throw new ChunkVersionNotSupportedException(Version);
            }

            var clipCountBits = r.ReadInt32();

            var clipCountNorth = clipCountBits & 7;
            var clipCountEast = clipCountBits >> 3 & 7;
            var clipCountSouth = clipCountBits >> 6 & 7;
            var clipCountWest = clipCountBits >> 9 & 7;
            var clipCountTop = clipCountBits >> 12 & 7;
            var clipCountBottom = clipCountBits >> 15 & 7;

            n.ClipsNorth = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountNorth)!;
            n.ClipsEast = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountEast)!;
            n.ClipsSouth = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountSouth)!;
            n.ClipsWest = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountWest)!;
            n.ClipsTop = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountTop)!;
            n.ClipsBottom = r.ReadArrayNodeRef<CGameCtnBlockInfoClip>(clipCountBottom)!;

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
            w.Write(Version);

            var clipCountBits = (n.ClipsNorth?.Length ?? 0)
                | (n.ClipsEast?.Length ?? 0) << 3
                | (n.ClipsSouth?.Length ?? 0) << 6
                | (n.ClipsWest?.Length ?? 0) << 9
                | (n.ClipsTop?.Length ?? 0) << 12
                | (n.ClipsBottom?.Length ?? 0) << 15;
            w.Write(clipCountBits);

            foreach (var clip in n.ClipsNorth ?? [])
            {
                w.WriteNodeRef(clip);
            }

            foreach (var clip in n.ClipsEast ?? [])
            {
                w.WriteNodeRef(clip);
            }

            foreach (var clip in n.ClipsSouth ?? [])
            {
                w.WriteNodeRef(clip);
            }

            foreach (var clip in n.ClipsWest ?? [])
            {
                w.WriteNodeRef(clip);
            }

            foreach (var clip in n.ClipsTop ?? [])
            {
                w.WriteNodeRef(clip);
            }

            foreach (var clip in n.ClipsBottom ?? [])
            {
                w.WriteNodeRef(clip);
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
