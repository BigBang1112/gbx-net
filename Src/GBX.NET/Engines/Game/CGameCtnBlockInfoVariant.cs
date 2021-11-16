using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game;

[Node(0x0315B000)]
public class CGameCtnBlockInfoVariant : CMwNod
{
    private CGameCtnBlockInfoMobil?[]? mobils;

    public string? Name { get; set; }

    public CGameCtnBlockInfoMobil?[]? Mobils
    {
        get => mobils;
        set => mobils = value;
    }

    public CGameCtnBlockUnitInfo?[]? BlockUnitInfos { get; set; }
    public bool HasManualSymmetryH { get; set; }
    public float SpawnTransX { get; set; }
    public float SpawnTransY { get; set; }
    public float SpawnTransZ { get; set; }

    protected CGameCtnBlockInfoVariant()
    {

    }

    [Chunk(0x0315B002)]
    public class Chunk0315B002 : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0315B003)]
    public class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;
        public short U03;
        public byte U04;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int16(ref U03);
            rw.Byte(ref U04);
        }
    }

    [Chunk(0x0315B004)]
    public class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
    {
        public short U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
        }
    }

    [Chunk(0x0315B005)]
    public class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;
        public int U02;
        public int U03;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
            rw.ArrayNode(ref n.mobils);

            if (Version >= 2)
            {
                rw.Int32(ref U02);
                rw.Int32(ref U03);
            }
        }
    }

    [Chunk(0x0315B006)]
    public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;
        public int U09;
        public int U10;
        public int U11;
        public int U12;
        public int U13;
        public int U14;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);

            if (Version >= 10)
            {
                rw.Int32(ref U11);
                rw.Int32(ref U12);

                if (Version >= 11)
                {
                    rw.Int32(ref U13);
                    rw.Int32(ref U14);
                }
            }
        }
    }

    [Chunk(0x0315B007)]
    public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    [Chunk(0x0315B008)]
    public class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public Vec3 U06;
        public int U07;
        public int U08;
        public int U09;
        public string? U10;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            n.BlockUnitInfos = rw.Array(n.BlockUnitInfos,
                r => r.ReadNodeRef<CGameCtnBlockUnitInfo>(),
                (x, w) => w.Write(x));

            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Vec3(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);

            if (Version >= 2)
                rw.Int32(ref U09);

            rw.String(ref U10);
        }
    }

    [Chunk(0x0315B009)]
    public class Chunk0315B009 : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;

        public override void Read(CGameCtnBlockInfoVariant n, GameBoxReader r)
        {
            U01 = r.ReadInt32();
            r.ReadArray(r => r.ReadArray<int>(5));
            U02 = r.ReadInt32();
        }
    }

    [Chunk(0x0315B00A)]
    public class Chunk0315B00A : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;
        public float U03;
        public int U04;
        public int U05;
        public int U06;
        public float U07;
        public int U08;
        public int U09;
        public int U10;
        public float U11;
        public int U12;
        public int U13;
        public int U14;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);
            rw.Single(ref U11);
            rw.Int32(ref U12);
            rw.Int32(ref U13);
            rw.Int32(ref U14);
        }
    }

    [Chunk(0x0315B00B)]
    public class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    [Chunk(0x0315B00C)]
    public class Chunk0315B00C : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }
}
