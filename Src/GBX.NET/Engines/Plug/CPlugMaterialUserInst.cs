namespace GBX.NET.Engines.Plug;

[Node(0x090FD000)]
[NodeExtension("Mat")]
public class CPlugMaterialUserInst : CMwNod
{
    private string? model;
    private string? baseTexture;
    private string? link;
    private Cst[]? csts;
    private UvAnim[]? uvAnims;
    private string? hidingGroup;
    private int tilingU;
    private int tilingV;
    private float textureSizeInMeters;
    private bool isNatural;

    [NodeMember(ExactlyNamed = true)]
    public string? Model { get => model; set => model = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? BaseTexture { get => baseTexture; set => baseTexture = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? Link { get => link; set => link = value; }

    public Cst[]? Csts { get => csts; set => csts = value; }
    public UvAnim[]? UvAnims { get => uvAnims; set => uvAnims = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? HidingGroup { get => hidingGroup; set => hidingGroup = value; }

    [NodeMember(ExactlyNamed = true)]
    public int TilingU { get => tilingU; set => tilingU = value; }

    [NodeMember(ExactlyNamed = true)]
    public int TilingV { get => tilingV; set => tilingV = value; }

    [NodeMember(ExactlyNamed = true)]
    public float TextureSizeInMeters { get => textureSizeInMeters; set => textureSizeInMeters = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsNatural { get => isNatural; set => isNatural = value; }

    protected CPlugMaterialUserInst()
    {

    }

    public override string ToString()
    {
        return $"{base.ToString()} {{ {Link ?? "No material file"} }}";
    }

    [Chunk(0x090FD000)]
    public class Chunk090FD000 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public string? U01;
        public string? U02;
        public string? U03;
        public byte U04;
        public byte? U05;
        public ulong[]? U06;
        public string[]? U07;
        public string? U08;
        public byte? U09;

        /// <summary>
        /// Version 10: TM®, version 9: ManiaPlanet 2019.11.19.1850
        /// </summary>
        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 11)
            {
                rw.Byte(ref U09);
            }

            rw.Id(ref U01);
            rw.Id(ref n.model);
            rw.String(ref n.baseTexture);
            rw.Byte(ref U04);

            if (version >= 10)
            {
                rw.Byte(ref U05);
            }

            if (version >= 1)
            {
                if (version < 9)
                {
                    rw.Id(ref n.link);
                }
                else
                {
                    rw.String(ref n.link);
                }

                if (version >= 11)
                {
                    // Something
                }

                if (version >= 2)
                {
                    rw.Array<Cst>(ref n.csts, r => new(
                        U01: r.ReadId(),
                        U02: r.ReadId(),
                        U03: r.ReadInt32()
                    ), (x, w) =>
                    {
                        w.WriteId(x.U01);
                        w.WriteId(x.U02);
                        w.Write(x.U03);
                    });

                    rw.Array<ulong>(ref U06);

                    if (version >= 3)
                    {
                        rw.Array<UvAnim>(ref n.uvAnims, r => new(
                            U01: r.ReadId(),
                            U02: r.ReadId(),
                            U03: r.ReadSingle(),
                            U04: r.ReadUInt64()
                        ), (x, w) =>
                        {
                            w.WriteId(x.U01);
                            w.WriteId(x.U02);
                            w.Write(x.U03);
                            w.Write(x.U04);
                        });

                        if (version >= 4)
                        {
                            rw.ArrayId(ref U07);

                            if (version >= 6)
                            {
                                // UserTextures
                                rw.Int32();

                                if (version >= 7)
                                {
                                    rw.Id(ref n.hidingGroup);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [Chunk(0x090FD001)]
    public class Chunk090FD001 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public int? U02;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01);

            if (version >= 3)
            {
                rw.Int32(ref n.tilingU);
                rw.Int32(ref n.tilingV);
                rw.Single(ref n.textureSizeInMeters);

                if (version >= 4)
                {
                    rw.Int32(ref U02);

                    if (version >= 5)
                    {
                        rw.Boolean(ref n.isNatural);
                    }
                }
            }
        }
    }

    [Chunk(0x090FD002)]
    public class Chunk090FD002 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
        }
    }

    public record Cst(string U01, string U02, int U03);
    public record UvAnim(string U01, string U02, float U03, ulong U04);
}