using System.Drawing;

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

    private string? materialName;
    private string? model;
    private string? baseTexture;
    private string? link;
    private int[]? color;
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

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>]
    public string? MaterialName { get => materialName; set => materialName = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD000>]
    public string? Model { get => model; set => model = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD000>]
    public string? BaseTexture { get => baseTexture; set => baseTexture = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD000>]
    public string? Link { get => link; set => link = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 2)]
    public Cst[]? Csts { get => csts; set => csts = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 2)]
    public int[]? Color { get => color; set => color = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 3)]
    public UvAnim[]? UvAnims { get => uvAnims; set => uvAnims = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 7)]
    public string? HidingGroup { get => hidingGroup; set => hidingGroup = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD001>(sinceVersion: 3)]
    public ETexAddress TilingU { get => tilingU; set => tilingU = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD001>(sinceVersion: 3)]
    public ETexAddress TilingV { get => tilingV; set => tilingV = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD001>(sinceVersion: 3)]
    public float TextureSizeInMeters { get => textureSizeInMeters; set => textureSizeInMeters = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD001>(sinceVersion: 5)]
    public bool IsNatural { get => isNatural; set => isNatural = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 6)]
    public UserTexture[]? UserTextures { get => userTextures; set => userTextures = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 10)]
    public byte SurfaceGameplayId { get => surfaceGameplayId; set => surfaceGameplayId = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk090FD000>]
    public byte SurfacePhysicId { get => surfacePhysicId; set => surfacePhysicId = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090FD000>(sinceVersion: 11)]
    public bool IsUsingGameMaterial { get => isUsingGameMaterial; set => isUsingGameMaterial = value; }

    #endregion

    #region Constructors

    internal CPlugMaterialUserInst()
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
        
        public string[]? U07;

        /// <summary>
        /// Version 10: TM®, version 9: ManiaPlanet 2019.11.19.1850
        /// </summary>
        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 11)
            {
                rw.Boolean(ref n.isUsingGameMaterial, asByte: true);
            }

            rw.Id(ref n.materialName);
            rw.Id(ref n.model);
            rw.String(ref n.baseTexture);

            rw.Byte(ref n.surfacePhysicId);

            if (version >= 10)
            {
                rw.Byte(ref n.surfaceGameplayId);
            }

            if (version >= 1)
            {
                if ((version >= 9 && version < 11) || n.isUsingGameMaterial) // Guessed
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
                    rw.ArrayArchive<Cst>(ref n.csts);
                    rw.Array<int>(ref n.color);

                    if (version >= 3)
                    {
                        rw.ArrayArchive<UvAnim>(ref n.uvAnims, version);

                        if (version >= 4)
                        {
                            rw.ArrayId(ref U07);

                            if (version >= 6)
                            {
                                rw.ArrayArchive<UserTexture>(ref n.userTextures); // UserTextures

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

        public int Version { get => version; set => version = value; }

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

    public class Cst : IReadableWritable
    {
        private string u01 = "";
        private string u02 = "";
        private int u03;

        public string U01 { get => u01; set => u01 = value; }
        public string U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!);
            rw.Id(ref u02!);
            rw.Int32(ref u03);
        }
    }
    
    public class UvAnim : IReadableWritable
    {
        private string u01 = "";
        private string u02 = "";
        private float u03;
        private ulong u04;
        private string? u05;

        public string U01 { get => u01; set => u01 = value; }
        public string U02 { get => u02; set => u02 = value; }
        public float U03 { get => u03; set => u03 = value; }
        public ulong U04 { get => u04; set => u04 = value; }
        public string? U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!);
            rw.Id(ref u02!);
            rw.Single(ref u03);
            rw.UInt64(ref u04);

            if (version >= 5)
            {
                rw.Id(ref u05);
            }
        }
    }

    public class UserTexture : IReadableWritable
    {
        private int u01;
        private string texture = "";
        
        public int U01 { get => u01; set => u01 = value; }
        public string Texture { get => texture; set => texture = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.String(ref texture!);
        }
    }

    #endregion
}