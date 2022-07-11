namespace GBX.NET.Engines.Plug;

/// <summary>
/// Material referenced in user creation.
/// </summary>
/// <remarks>ID: 0x090FD000</remarks>
[Node(0x090FD000)]
[NodeExtension("Mat")]
public class CPlugMaterialUserInst : CMwNod
{
    #region Enums

    public enum ETexAddress
    {
        Wrap,
        Mirror,
        Clamp,
        Border
    }

    #endregion

    #region Fields

    private string? model;
    private string? baseTexture;
    private string? link;
    private Cst[]? csts;
    private UvAnim[]? uvAnims;
    private string? hidingGroup;
    private ETexAddress tilingU;
    private ETexAddress tilingV;
    private float textureSizeInMeters;
    private bool isNatural;
    private UserTexture[]? userTextures;
    private byte surfaceGameplayId;
    private byte surfacePhysicId;
    private bool isUsingGameMaterial;

    #endregion

    #region Properties

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
    public ETexAddress TilingU { get => tilingU; set => tilingU = value; }

    [NodeMember(ExactlyNamed = true)]
    public ETexAddress TilingV { get => tilingV; set => tilingV = value; }

    [NodeMember(ExactlyNamed = true)]
    public float TextureSizeInMeters { get => textureSizeInMeters; set => textureSizeInMeters = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsNatural { get => isNatural; set => isNatural = value; }

    [NodeMember]
    public UserTexture[]? UserTextures { get => userTextures; set => userTextures = value; }

    [NodeMember]
    public byte SurfaceGameplayId { get => surfaceGameplayId; set => surfaceGameplayId = value; }
    
    [NodeMember]
    public byte SurfacePhysicId { get => surfacePhysicId; set => surfacePhysicId = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public bool IsUsingGameMaterial { get => isUsingGameMaterial; set => isUsingGameMaterial = value; }

    #endregion

    #region Constructors

    protected CPlugMaterialUserInst()
    {

    }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{base.ToString()} {{ {Link ?? "No material file"} }}";
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CPlugMaterialUserInst 0x000 chunk
    /// </summary>
    [Chunk(0x090FD000)]
    public class Chunk090FD000 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public string? U01;
        public int[]? U06;
        public string[]? U07;

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
                rw.Boolean(ref n.isUsingGameMaterial, asByte: true);
            }

            rw.Id(ref U01);
            rw.Id(ref n.model);
            rw.String(ref n.baseTexture);

            rw.Byte(ref n.surfacePhysicId);

            if (version >= 10)
            {
                rw.Byte(ref n.surfaceGameplayId);
            }

            if (version >= 1)
            {
                if (n.isUsingGameMaterial)
                {
                    rw.String(ref n.link);
                }
                else
                {
                    rw.Id(ref n.link);
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

                    rw.Array<int>(ref U06);

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
                                rw.Array<UserTexture>(ref n.userTextures, // UserTextures
                                    r => new(r.ReadInt32(), r.ReadString()),
                                    (x, w) => { w.Write(x.U01); w.Write(x.Texture); });

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

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CPlugMaterialUserInst 0x001 chunk
    /// </summary>
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

            if (version == 2)
            {
                throw new ChunkVersionNotSupportedException(version);
            }

            if (version >= 3)
            {
                rw.EnumInt32<ETexAddress>(ref n.tilingU);
                rw.EnumInt32<ETexAddress>(ref n.tilingV);
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

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CPlugMaterialUserInst 0x002 chunk
    /// </summary>
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

    #endregion

    #endregion

    #region Other classes

    public record Cst(string U01, string U02, int U03);
    public record UvAnim(string U01, string U02, float U03, ulong U04);
    public record UserTexture(int U01, string Texture);

    #endregion
}