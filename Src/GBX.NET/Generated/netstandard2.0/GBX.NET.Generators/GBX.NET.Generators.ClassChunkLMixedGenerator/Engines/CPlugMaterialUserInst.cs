namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090FD000</remarks>
[Class(0x090FD000)]
public partial class CPlugMaterialUserInst : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090FD000;




    private bool isUsingGameMaterial;
    [AppliedWithChunk<Chunk090FD000>]
    public bool IsUsingGameMaterial { get => isUsingGameMaterial; set => isUsingGameMaterial = value; }

    private string? materialName;
    [AppliedWithChunk<Chunk090FD000>]
    public string? MaterialName { get => materialName; set => materialName = value; }

    private string? model;
    [AppliedWithChunk<Chunk090FD000>]
    public string? Model { get => model; set => model = value; }

    private string? baseTexture;
    [AppliedWithChunk<Chunk090FD000>]
    public string? BaseTexture { get => baseTexture; set => baseTexture = value; }

    private CPlugSurface.MaterialId surfacePhysicId;
    [AppliedWithChunk<Chunk090FD000>]
    public CPlugSurface.MaterialId SurfacePhysicId { get => surfacePhysicId; set => surfacePhysicId = value; }

    private CPlugSurface.GameplayId surfaceGameplayId;
    [AppliedWithChunk<Chunk090FD000>]
    public CPlugSurface.GameplayId SurfaceGameplayId { get => surfaceGameplayId; set => surfaceGameplayId = value; }

    private string? link;
    [AppliedWithChunk<Chunk090FD000>]
    [AppliedWithChunk<Chunk090FD000>]
    [AppliedWithChunk<Chunk090FD000>]
    [AppliedWithChunk<Chunk090FD000>]
    public string? Link { get => link; set => link = value; }

    private Cst[]? csts;
    [AppliedWithChunk<Chunk090FD000>]
    public Cst[]? Csts { get => csts; set => csts = value; }

    private int[]? color;
    [AppliedWithChunk<Chunk090FD000>]
    public int[]? Color { get => color; set => color = value; }

    private UvAnim[]? uvAnims;
    [AppliedWithChunk<Chunk090FD000>]
    public UvAnim[]? UvAnims { get => uvAnims; set => uvAnims = value; }

    private UserTexture[]? userTextures;
    [AppliedWithChunk<Chunk090FD000>]
    public UserTexture[]? UserTextures { get => userTextures; set => userTextures = value; }

    private string? hidingGroup;
    [AppliedWithChunk<Chunk090FD000>]
    public string? HidingGroup { get => hidingGroup; set => hidingGroup = value; }

    private ETexAddress tilingU;
    [AppliedWithChunk<Chunk090FD001>]
    public ETexAddress TilingU { get => tilingU; set => tilingU = value; }

    private ETexAddress tilingV;
    [AppliedWithChunk<Chunk090FD001>]
    public ETexAddress TilingV { get => tilingV; set => tilingV = value; }

    private float textureSizeInMeters;
    [AppliedWithChunk<Chunk090FD001>]
    public float TextureSizeInMeters { get => textureSizeInMeters; set => textureSizeInMeters = value; }

    private bool isNatural;
    [AppliedWithChunk<Chunk090FD001>]
    public bool IsNatural { get => isNatural; set => isNatural = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugMaterialUserInst"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugMaterialUserInst() { }


    /// <summary>
    /// CPlugMaterialUserInst 0x000 chunk
    /// </summary>
    [Chunk(0x090FD000)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 9, 11)]
    public partial class Chunk090FD000 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090FD000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public string[]? U01;

        public override void ReadWrite(CPlugMaterialUserInst n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 11)
            {
                rw.Boolean(ref n.isUsingGameMaterial, asByte: true);
            }
            rw.Id(ref n.materialName);
            rw.Id(ref n.model);
            rw.String(ref n.baseTexture);
            rw.EnumByte<CPlugSurface.MaterialId>(ref n.surfacePhysicId);
            if (Version >= 10)
            {
                rw.EnumByte<CPlugSurface.GameplayId>(ref n.surfaceGameplayId);
            }
            if (Version >= 1)
            {
                if (n.IsUsingGameMaterial)
                {
                    rw.String(ref n.link);
                }
                if (!n.IsUsingGameMaterial)
                {
                    if (Version >= 9)
                    {
                        if (Version <= 10)
                        {
                            rw.String(ref n.link);
                        }
                        if (Version >= 11)
                        {
                            rw.Id(ref n.link);
                        }
                    }
                    if (Version <= 8)
                    {
                        rw.Id(ref n.link);
                    }
                }
                if (Version >= 2)
                {
                    rw.ArrayReadableWritable<Cst>(ref n.csts!);
                    rw.Array<int>(ref n.color!);
                    if (Version >= 3)
                    {
                        rw.ArrayReadableWritable<UvAnim>(ref n.uvAnims!, version: Version);
                        if (Version >= 4)
                        {
                            rw.ArrayId(ref U01!);
                            if (Version >= 6)
                            {
                                rw.ArrayReadableWritable<UserTexture>(ref n.userTextures!);
                                if (Version >= 7)
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

    /// <summary>
    /// CPlugMaterialUserInst 0x001 chunk
    /// </summary>
    [Chunk(0x090FD001)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk090FD001 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090FD001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; } = 5;

        public CPlugBitmapAtlas? U01;
        public int? U02;

        public override void ReadWrite(CPlugMaterialUserInst n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugBitmapAtlas>(ref U01);
            if (Version == 2)
            {
                throw new ("");
            }
            if (Version >= 3)
            {
                rw.EnumInt32<ETexAddress>(ref n.tilingU);
                rw.EnumInt32<ETexAddress>(ref n.tilingV);
                rw.Single(ref n.textureSizeInMeters);
                if (Version >= 4)
                {
                    rw.Int32(ref U02);
                    if (Version >= 5)
                    {
                        rw.Boolean(ref n.isNatural);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugMaterialUserInst 0x002 chunk
    /// </summary>
    [Chunk(0x090FD002)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk090FD002 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090FD002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugMaterialUserInst n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }


    public sealed partial class UvAnim : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private ulong u04;
        public ulong U04 { get => u04; set => u04 = value; }

        private string? u05;
        public string? U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Id(ref u02);
            rw.Single(ref u03);
            rw.UInt64(ref u04);
            if (v >= 5)
            {
                rw.Id(ref u05);
            }
        }
    }

    public sealed partial class UserTexture : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? texture;
        public string? Texture { get => texture; set => texture = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.String(ref texture);
        }
    }

    public sealed partial class Cst : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Id(ref u02);
            rw.Int32(ref u03);
        }
    }


    public enum ETexAddress
    {
        Wrap,
        Mirror,
        Clamp,
        Border,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090FD000 => new Chunk090FD000(),
        0x090FD001 => new Chunk090FD001(),
        0x090FD002 => new Chunk090FD002(),
        _ => base.NewChunk(chunkId),
    };
}
