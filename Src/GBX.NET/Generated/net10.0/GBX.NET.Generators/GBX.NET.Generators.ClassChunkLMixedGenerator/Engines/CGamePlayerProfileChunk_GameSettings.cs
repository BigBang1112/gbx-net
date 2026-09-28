namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0312D000</remarks>
[Class(0x0312D000)]
public partial class CGamePlayerProfileChunk_GameSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x0312D000;




    private int opponentsVersionLast;
    [AppliedWithChunk<Chunk0312D000>]
    public int OpponentsVersionLast { get => opponentsVersionLast; set => opponentsVersionLast = value; }

    private SChallengeOpponents[]? challengeOpponents;
    [AppliedWithChunk<Chunk0312D000>]
    public SChallengeOpponents[]? ChallengeOpponents { get => challengeOpponents; set => challengeOpponents = value; }

    private string? serverName;
    [AppliedWithChunk<Chunk0312D002>]
    public string? ServerName { get => serverName; set => serverName = value; }

    private string? serverComment;
    [AppliedWithChunk<Chunk0312D002>]
    public string? ServerComment { get => serverComment; set => serverComment = value; }

    private int networkGameMode;
    [AppliedWithChunk<Chunk0312D002>]
    public int NetworkGameMode { get => networkGameMode; set => networkGameMode = value; }

    private byte maxSpectatorCount;
    [AppliedWithChunk<Chunk0312D002>]
    public byte MaxSpectatorCount { get => maxSpectatorCount; set => maxSpectatorCount = value; }

    private byte opponentVisibility;
    [AppliedWithChunk<Chunk0312D002>]
    public byte OpponentVisibility { get => opponentVisibility; set => opponentVisibility = value; }

    private bool editorHelp;
    [AppliedWithChunk<Chunk0312D003>]
    public bool EditorHelp { get => editorHelp; set => editorHelp = value; }

    private float mouseSensitivity_Default;
    [AppliedWithChunk<Chunk0312D004>]
    [AppliedWithChunk<Chunk0312D004>]
    public float MouseSensitivity_Default { get => mouseSensitivity_Default; set => mouseSensitivity_Default = value; }

    private float mouseAccelQuantity;
    [AppliedWithChunk<Chunk0312D004>]
    public float MouseAccelQuantity { get => mouseAccelQuantity; set => mouseAccelQuantity = value; }

    private float stereoscopyStrength01;
    [AppliedWithChunk<Chunk0312D005>]
    public float StereoscopyStrength01 { get => stereoscopyStrength01; set => stereoscopyStrength01 = value; }

    private float stereoscopyAdvancedScreenDist;
    [AppliedWithChunk<Chunk0312D005>]
    public float StereoscopyAdvancedScreenDist { get => stereoscopyAdvancedScreenDist; set => stereoscopyAdvancedScreenDist = value; }

    private CGameCtnMediaShootParams? shootParamsVideo;
    [AppliedWithChunk<Chunk0312D005>]
    public CGameCtnMediaShootParams? ShootParamsVideo { get => shootParamsVideo; set => shootParamsVideo = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_GameSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_GameSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x0312D000)]
    public partial class Chunk0312D000 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D000;

        public int Version { get; set; }

        public bool U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.opponentsVersionLast);
            rw.ArrayReadableWritable<SChallengeOpponents>(ref n.challengeOpponents!, version: n.OpponentsVersionLast);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x0312D001)]
    public partial class Chunk0312D001 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D001;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;
        public string? U06;
        public string? U07;
        public string? U08;
        public string? U09;
        public string? U10;
        public string? U11;
        public string? U12;
        public string? U13;
        public string? U14;
        public string? U15;
        public string? U16;
        public byte U17;
        public byte U18;
        public byte U19;
        public byte U20;
        public byte U21;
        public int U22;
        public int U23;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.String(ref U04);
            rw.String(ref U05);
            rw.String(ref U06);
            rw.String(ref U07);
            rw.String(ref U08);
            rw.String(ref U09);
            rw.String(ref U10);
            rw.String(ref U11);
            rw.String(ref U12);
            rw.String(ref U13);
            rw.String(ref U14);
            rw.String(ref U15);
            rw.String(ref U16);
            rw.Byte(ref U17);
            rw.Byte(ref U18);
            rw.Byte(ref U19);
            rw.Byte(ref U20);
            rw.Byte(ref U21);
            rw.Int32(ref U22);
            rw.Int32(ref U23);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x0312D002)]
    public partial class Chunk0312D002 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D002;

        public int Version { get; set; }

        public byte U01;
        public byte U02;
        public byte U03;
        public byte U04;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.serverName);
            rw.String(ref n.serverComment);
            rw.Int32(ref n.networkGameMode);
            rw.Byte(ref n.maxSpectatorCount);
            rw.Byte(ref U01);
            rw.Byte(ref U02);
            rw.Byte(ref U03);
            rw.Byte(ref U04);
            if (Version >= 3)
            {
                rw.Byte(ref n.opponentVisibility);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0312D003)]
    public partial class Chunk0312D003 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D003;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
            if (Version >= 2)
            {
                rw.Boolean(ref n.editorHelp);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0312D004)]
    public partial class Chunk0312D004 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D004;

        public int Version { get; set; }

        public float U01;
        public bool U02;
        public bool U03;
        public float U04;
        public bool U05;
        public float U06;
        public float U07;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Boolean(ref U02);
            if (Version <= 2)
            {
                rw.Single(ref n.mouseSensitivity_Default);
            }
            rw.Boolean(ref U03);
            rw.Single(ref n.mouseAccelQuantity);
            if (Version >= 2)
            {
                rw.Single(ref U04);
                if (Version >= 3)
                {
                    rw.Boolean(ref U05);
                    rw.Single(ref n.mouseSensitivity_Default);
                    rw.Single(ref U06);
                    if (Version >= 4)
                    {
                        rw.Single(ref U07);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0312D005)]
    public partial class Chunk0312D005 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D005;

        public int Version { get; set; }

        public float U01;
        public CGameCtnMediaShootParams? U02;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.stereoscopyStrength01);
            rw.Single(ref U01);
            rw.Single(ref n.stereoscopyAdvancedScreenDist);
            rw.Node<CGameCtnMediaShootParams>(ref n.shootParamsVideo);
            rw.Node<CGameCtnMediaShootParams>(ref U02);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x006 skippable chunk
    /// </summary>
    [Chunk(0x0312D006)]
    public partial class Chunk0312D006 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D006;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0312D007)]
    public partial class Chunk0312D007 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D007;

        public int Version { get; set; }

        public bool U01;
        public float U02;
        public float U03;
        public float U04;
        public string? U05;
        public bool U06;
        public float U07;
        public bool U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            if (Version >= 1)
            {
                rw.String(ref U05);
                if (Version >= 2)
                {
                    rw.Boolean(ref U06);
                    rw.Single(ref U07);
                    rw.Boolean(ref U08);
                    rw.Single(ref U09);
                    if (Version >= 3)
                    {
                        rw.Single(ref U10);
                        if (Version >= 4)
                        {
                            rw.Single(ref U11);
                            rw.Single(ref U12);
                            if (Version >= 5)
                            {
                                rw.Single(ref U13);
                                if (Version >= 6)
                                {
                                    rw.Single(ref U14);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x008 skippable chunk
    /// </summary>
    [Chunk(0x0312D008)]
    public partial class Chunk0312D008 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D008;

        public int Version { get; set; }

        public int U01;
        public float U02;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x009 skippable chunk
    /// </summary>
    [Chunk(0x0312D009)]
    public partial class Chunk0312D009 : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D009;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x00A skippable chunk
    /// </summary>
    [Chunk(0x0312D00A)]
    public partial class Chunk0312D00A : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D00A;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0312D00B)]
    public partial class Chunk0312D00B : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D00B;

        public int Version { get; set; }

        public int U01;
        public Unknown[]? U02;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ArrayReadableWritable<Unknown>(ref U02!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x00C skippable chunk
    /// </summary>
    [Chunk(0x0312D00C)]
    public partial class Chunk0312D00C : SkippableChunk<CGamePlayerProfileChunk_GameSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D00C;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameSettings 0x00D skippable chunk
    /// </summary>
    [Chunk(0x0312D00D)]
    public partial class Chunk0312D00D : SkippableChunk<CGamePlayerProfileChunk_GameSettings>
    {
        /// <inheritdoc />
        public override uint Id => 0x0312D00D;

        public uint U01;
        public int U02;
        public string? U03;

        public override void ReadWrite(CGamePlayerProfileChunk_GameSettings n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.Int32(ref U02);
            rw.String(ref U03);
        }
    }


    public sealed partial class SChallengeOpponents : IReadableWritable
    {

        private Ident? u01;
        public Ident? U01 { get => u01; set => u01 = value; }

        private string[]? u02;
        public string[]? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref u01);
            if (v == 0)
            {
                rw.ArrayId(ref u02!);
            }
            rw.Int32(ref u03);
            rw.String(ref u04);
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0312D000 => new Chunk0312D000(),
        0x0312D001 => new Chunk0312D001(),
        0x0312D002 => new Chunk0312D002(),
        0x0312D003 => new Chunk0312D003(),
        0x0312D004 => new Chunk0312D004(),
        0x0312D005 => new Chunk0312D005(),
        0x0312D006 => new Chunk0312D006(),
        0x0312D007 => new Chunk0312D007(),
        0x0312D008 => new Chunk0312D008(),
        0x0312D009 => new Chunk0312D009(),
        0x0312D00A => new Chunk0312D00A(),
        0x0312D00B => new Chunk0312D00B(),
        0x0312D00C => new Chunk0312D00C(),
        0x0312D00D => new Chunk0312D00D(),
        _ => base.NewChunk(chunkId),
    };
}
