namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x031CC000</remarks>
[Class(0x031CC000)]
public partial class CGameUserProfile : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x031CC000;




    private Sticker[]? stickers;
    [AppliedWithChunk<Chunk031CC000>]
    public Sticker[]? Stickers { get => stickers; set => stickers = value; }

    private string[]? layers;
    [AppliedWithChunk<Chunk031CC000>]
    public string[]? Layers { get => layers; set => layers = value; }

    private string? clubTag;
    [AppliedWithChunk<Chunk031CC000>]
    public string? ClubTag { get => clubTag; set => clubTag = value; }

    private ContextTime[]? contextTimes;
    [AppliedWithChunk<Chunk031CC004>]
    public ContextTime[]? ContextTimes { get => contextTimes; set => contextTimes = value; }

    private ContextMapRecordForProfile[]? contextMapRecordsForProfile;
    [AppliedWithChunk<Chunk031CC006>]
    public ContextMapRecordForProfile[]? ContextMapRecordsForProfile { get => contextMapRecordsForProfile; set => contextMapRecordsForProfile = value; }

    private bool editor_ShowHelp;
    [AppliedWithChunk<Chunk031CC00B>]
    public bool Editor_ShowHelp { get => editor_ShowHelp; set => editor_ShowHelp = value; }

    private DeviceSettings[]? vehicleSettings;
    [AppliedWithChunk<Chunk031CC00C>]
    public DeviceSettings[]? VehicleSettings { get => vehicleSettings; set => vehicleSettings = value; }


    /// <summary>
    /// CGameUserProfile 0x000 skippable chunk
    /// </summary>
    [Chunk(0x031CC000)]
    public partial class Chunk031CC000 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC000;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public int U05;
        public string? U06;
        public string? U07;
        public string? U08;
        public bool U09;
        public string? U10;
        public string? U11;
        public string? U12;
        public byte U13;
        public string? U14;
        public string? U15;
        public float U16;
        public string? U17;
        public string? U18;
        public float U19;
        public int U20;
        public int U21;
        public int U22;
        public int U23;
        public byte U24;
        public float U25;
        public float U26;
        public int U27;
        public int U28;
        public string? U29;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            if (Version == 0)
            {
                rw.Single(ref U04);
            }
            if (Version <= 3)
            {
                rw.Int32(ref U05);
                rw.String(ref U06);
            }
            if (Version <= 2)
            {
                rw.String(ref U07);
                if (Version == 2)
                {
                    rw.String(ref U08);
                }
            }
            if (Version >= 3)
            {
                rw.ArrayReadableWritable<Sticker>(ref n.stickers!);
                rw.ArrayString(ref n.layers!);
                if (Version >= 4)
                {
                    rw.Boolean(ref U09);
                    rw.String(ref U10);
                }
                if (Version >= 5)
                {
                    rw.String(ref U11);
                    rw.String(ref U12);
                }
                if (Version >= 6)
                {
                    rw.Byte(ref U13);
                }
                if (Version >= 7)
                {
                    rw.String(ref n.clubTag);
                }
                if (Version >= 8)
                {
                    rw.String(ref U14);
                    rw.String(ref U15);
                    rw.Single(ref U16);
                    rw.String(ref U17);
                    rw.String(ref U18);
                    rw.Single(ref U19);
                    rw.Int32(ref U20);
                    rw.Int32(ref U21);
                }
                if (Version >= 10)
                {
                    rw.Int32(ref U22);
                }
                if (Version >= 11)
                {
                    rw.Int32(ref U23);
                }
                if (Version >= 12)
                {
                    rw.Byte(ref U24);
                }
                if (Version >= 13)
                {
                    rw.Single(ref U25);
                    rw.Single(ref U26);
                }
                if (Version >= 14)
                {
                    rw.Int32(ref U27);
                }
                if (Version >= 15)
                {
                    rw.Int32(ref U28);
                }
                if (Version >= 16)
                {
                    rw.String(ref U29);
                }
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x001 skippable chunk
    /// </summary>
    [Chunk(0x031CC001)]
    public partial class Chunk031CC001 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC001;

    }

    /// <summary>
    /// CGameUserProfile 0x002 skippable chunk
    /// </summary>
    [Chunk(0x031CC002)]
    public partial class Chunk031CC002 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC002;

        public int Version { get; set; }

        public byte[]? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Data(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x003 skippable chunk
    /// </summary>
    [Chunk(0x031CC003)]
    public partial class Chunk031CC003 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC003;

        public int Version { get; set; }

        public byte[]? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Data(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x004 skippable chunk
    /// </summary>
    [Chunk(0x031CC004)]
    public partial class Chunk031CC004 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC004;

        public int Version { get; set; }


        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<ContextTime>(ref n.contextTimes!);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x005 skippable chunk
    /// </summary>
    [Chunk(0x031CC005)]
    public partial class Chunk031CC005 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC005;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x006 skippable chunk
    /// </summary>
    [Chunk(0x031CC006)]
    public partial class Chunk031CC006 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC006;

        public int Version { get; set; }


        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<ContextMapRecordForProfile>(ref n.contextMapRecordsForProfile!, version: Version);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x007 skippable chunk
    /// </summary>
    [Chunk(0x031CC007)]
    public partial class Chunk031CC007 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC007;

        public int Version { get; set; }

        public CInputBindingsConfig? U01;
        public CInputBindingsConfig[]? U02;
        public float U03;
        public float U04;
        public bool U05;
        public int U06;
        public bool U07;
        public float U08;
        public float U09;
        public float U10;
        public bool U11;
        public float U12;
        public float U13;
        public Unknown[]? U14;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 1)
            {
                rw.NodeRef<CInputBindingsConfig>(ref U01);
            }
            if (Version >= 2)
            {
                rw.ArrayNodeRef<CInputBindingsConfig>(ref U02!);
            }
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Boolean(ref U05);
            rw.Int32(ref U06);
            rw.Boolean(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Boolean(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            if (Version <= 2)
            {
                rw.ArrayReadableWritable<Unknown>(ref U14!);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x008 skippable chunk
    /// </summary>
    [Chunk(0x031CC008)]
    public partial class Chunk031CC008 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC008;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x009 skippable chunk
    /// </summary>
    [Chunk(0x031CC009)]
    public partial class Chunk031CC009 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC009;

    }

    /// <summary>
    /// CGameUserProfile 0x00A skippable chunk
    /// </summary>
    [Chunk(0x031CC00A)]
    public partial class Chunk031CC00A : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00A;

        public int Version { get; set; }

        public Unknown2[]? U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Unknown2>(ref U01!);
            if (Version >= 2)
            {
                rw.Int32(ref U02);
                rw.Int32(ref U03);
                rw.Int32(ref U04);
                rw.Int32(ref U05);
                rw.Int32(ref U06);
            }
            if (Version >= 3)
            {
                rw.Int32(ref U07);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x00B skippable chunk
    /// </summary>
    [Chunk(0x031CC00B)]
    public partial class Chunk031CC00B : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00B;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;
        public string? U06;
        public Int3 U07;
        public Int3 U08;
        public Int3 U09;
        public Int3 U10;
        public Int3 U11;
        public Int3 U12;
        public Int3 U13;
        public Int3 U14;
        public Int3 U15;
        public Int3 U16;
        public Int3 U17;
        public Unknown4[]? U18;
        public int U19;
        public int U20;
        public int U21;
        public int U22;
        public byte U23;
        public int U24;
        public int U25;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.editor_ShowHelp);
            if (Version >= 1)
            {
                rw.String(ref U01);
                rw.String(ref U02);
                rw.String(ref U03);
                rw.String(ref U04);
                rw.String(ref U05);
                rw.String(ref U06);
            }
            if (Version >= 2)
            {
                if (Version <= 3)
                {
                    rw.Int3(ref U07);
                    rw.Int3(ref U08);
                    rw.Int3(ref U09);
                    rw.Int3(ref U10);
                    rw.Int3(ref U11);
                }
            }
            if (Version >= 4)
            {
                rw.Int3(ref U12);
                rw.Int3(ref U13);
                rw.Int3(ref U14);
                rw.Int3(ref U15);
                rw.Int3(ref U16);
                rw.Int3(ref U17);
            }
            if (Version >= 5)
            {
                rw.ArrayReadableWritable<Unknown4>(ref U18!);
                rw.Int32(ref U19);
            }
            if (Version >= 6)
            {
                rw.Int32(ref U20);
                rw.Int32(ref U21);
                if (Version == 6)
                {
                    rw.Int32(ref U22);
                }
                if (Version >= 7)
                {
                    rw.Byte(ref U23);
                }
                rw.Int32(ref U24);
            }
            if (Version >= 8)
            {
                rw.Int32(ref U25);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x00C skippable chunk
    /// </summary>
    [Chunk(0x031CC00C)]
    public partial class Chunk031CC00C : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00C;

        public int Version { get; set; }


        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<DeviceSettings>(ref n.vehicleSettings!);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x00D skippable chunk
    /// </summary>
    [Chunk(0x031CC00D)]
    public partial class Chunk031CC00D : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00D;

        public int Version { get; set; }

        public int U01;
        public int U02;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (Version >= 1)
            {
                rw.Int32(ref U02);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x00E skippable chunk
    /// </summary>
    [Chunk(0x031CC00E)]
    public partial class Chunk031CC00E : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00E;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CGameUserProfile 0x00F skippable chunk
    /// </summary>
    [Chunk(0x031CC00F)]
    public partial class Chunk031CC00F : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC00F;

        public int Version { get; set; }

        public Unknown6? U01;
        public byte U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Unknown6>(ref U01);
            if (Version >= 2)
            {
                rw.Byte(ref U02);
            }
            if (Version >= 3)
            {
                rw.Int32(ref U03);
            }
            if (Version >= 4)
            {
                rw.Int32(ref U04);
                rw.Int32(ref U05);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x010 skippable chunk
    /// </summary>
    [Chunk(0x031CC010)]
    public partial class Chunk031CC010 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC010;

        public int Version { get; set; }

        public Unknown7[]? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Unknown7>(ref U01!);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x011 skippable chunk
    /// </summary>
    [Chunk(0x031CC011)]
    public partial class Chunk031CC011 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC011;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public ulong U03;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.UInt64(ref U03);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x012 skippable chunk
    /// </summary>
    [Chunk(0x031CC012)]
    public partial class Chunk031CC012 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC012;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x014 skippable chunk
    /// </summary>
    [Chunk(0x031CC014)]
    public partial class Chunk031CC014 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC014;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x015 skippable chunk
    /// </summary>
    [Chunk(0x031CC015)]
    public partial class Chunk031CC015 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC015;

        public int Version { get; set; }

        public Unknown8? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Unknown8>(ref U01, version: Version);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x016 skippable chunk
    /// </summary>
    [Chunk(0x031CC016)]
    public partial class Chunk031CC016 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC016;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x017 skippable chunk
    /// </summary>
    [Chunk(0x031CC017)]
    public partial class Chunk031CC017 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC017;

        public int Version { get; set; }

        public Unknown8? U01;
        public ulong U02;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Unknown8>(ref U01, version: 5);
            if (Version >= 6)
            {
                rw.UInt64(ref U02);
            }
        }
    }

    /// <summary>
    /// CGameUserProfile 0x018 skippable chunk
    /// </summary>
    [Chunk(0x031CC018)]
    public partial class Chunk031CC018 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC018;

        public int Version { get; set; }

        public byte U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x019 skippable chunk
    /// </summary>
    [Chunk(0x031CC019)]
    public partial class Chunk031CC019 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC019;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x01A skippable chunk
    /// </summary>
    [Chunk(0x031CC01A)]
    public partial class Chunk031CC01A : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01A;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x01B skippable chunk
    /// </summary>
    [Chunk(0x031CC01B)]
    public partial class Chunk031CC01B : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01B;

    }

    /// <summary>
    /// CGameUserProfile 0x01C skippable chunk
    /// </summary>
    [Chunk(0x031CC01C)]
    public partial class Chunk031CC01C : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01C;

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x01D skippable chunk
    /// </summary>
    [Chunk(0x031CC01D)]
    public partial class Chunk031CC01D : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01D;

        public int Version { get; set; }

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

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
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
            rw.Int32(ref U11);
            rw.Int32(ref U12);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x01E skippable chunk
    /// </summary>
    [Chunk(0x031CC01E)]
    public partial class Chunk031CC01E : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01E;

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x01F skippable chunk
    /// </summary>
    [Chunk(0x031CC01F)]
    public partial class Chunk031CC01F : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC01F;

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x020 skippable chunk
    /// </summary>
    [Chunk(0x031CC020)]
    public partial class Chunk031CC020 : SkippableChunk<CGameUserProfile>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC020;

        public int Version { get; set; }

        public Unknown8? U01;
        public ulong U02;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<Unknown8>(ref U01, version: Version);
            rw.UInt64(ref U02);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x021 skippable chunk
    /// </summary>
    [Chunk(0x031CC021)]
    public partial class Chunk031CC021 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC021;

    }

    /// <summary>
    /// CGameUserProfile 0x022 skippable chunk
    /// </summary>
    [Chunk(0x031CC022)]
    public partial class Chunk031CC022 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC022;

        public byte[]? U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Data(ref U01!, 20);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x023 skippable chunk
    /// </summary>
    [Chunk(0x031CC023)]
    public partial class Chunk031CC023 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC023;

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameUserProfile 0x024 skippable chunk
    /// </summary>
    [Chunk(0x031CC024)]
    public partial class Chunk031CC024 : SkippableChunk<CGameUserProfile>
    {
        /// <inheritdoc />
        public override uint Id => 0x031CC024;

        public int U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }


    public sealed partial class Unknown6 : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private Quat[]? u02;
        public Quat[]? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            if (Version>=1)
            {
                rw.Int32(ref u01);
                rw.Array<Quat>(ref u02!);
            }
        }
    }

    public sealed partial class ContextTime : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
        }
    }

    public sealed partial class Unknown2 : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private Unknown3[]? innerData;
        public Unknown3[]? InnerData { get => innerData; set => innerData = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.String(ref u02);
            rw.String(ref u03);
            rw.String(ref u04);
            rw.ArrayReadableWritable<Unknown3>(ref innerData!);
        }
    }

    public sealed partial class Unknown4 : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.String(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
        }
    }

    public sealed partial class Unknown7 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private string? u05;
        public string? U05 { get => u05; set => u05 = value; }

        private string? u06;
        public string? U06 { get => u06; set => u06 = value; }

        private byte[]? u07;
        public byte[]? U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.String(ref u02);
            rw.String(ref u03);
            rw.String(ref u04);
            rw.String(ref u05);
            rw.String(ref u06);
            rw.Data(ref u07!, 32);
            rw.Int32(ref u08);
        }
    }

    public sealed partial class DeviceSettings : IReadableWritable
    {

        private Ident vehicle = Ident.Empty;
        public Ident Vehicle { get => vehicle; set => vehicle = value; }

        private float analogSensitivity = 1;
        public float AnalogSensitivity { get => analogSensitivity; set => analogSensitivity = value; }

        private float analogDeadZone = 0.1f;
        public float AnalogDeadZone { get => analogDeadZone; set => analogDeadZone = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private float vibrationIntensity = 1;
        public float VibrationIntensity { get => vibrationIntensity; set => vibrationIntensity = value; }

        private float centerSpringIntensity;
        public float CenterSpringIntensity { get => centerSpringIntensity; set => centerSpringIntensity = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref vehicle);
            rw.Single(ref analogSensitivity);
            rw.Single(ref analogDeadZone);
            rw.Int32(ref u01);
            rw.Single(ref vibrationIntensity);
            rw.Single(ref centerSpringIntensity);
            rw.Int32(ref u02);
            rw.String(ref u03);
            rw.String(ref u04);
            rw.Int32(ref u05);
        }
    }

    public sealed partial class ContextMapRecordForProfile : IReadableWritable
    {

        private string? titleId;
        public string? TitleId { get => titleId; set => titleId = value; }

        private string? mapUid;
        public string? MapUid { get => mapUid; set => mapUid = value; }

        private int personalBest;
        public int PersonalBest { get => personalBest; set => personalBest = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v <= 2)
            {
                rw.Id(ref titleId);
            }
            rw.Id(ref mapUid);
            rw.Int32(ref personalBest);
            if (v >= 2)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                if (v >= 3)
                {
                    rw.String(ref u03);
                }
            }
        }
    }

    public sealed partial class Unknown8 : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private byte u05;
        public byte U05 { get => u05; set => u05 = value; }

        private string? u06;
        public string? U06 { get => u06; set => u06 = value; }

        private string? u07;
        public string? U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v >= 1)
            {
                if (v <= 4)
                {
                    rw.Int32(ref u01);
                }
                if (v >= 5)
                {
                    rw.Int32(ref u02);
                }
                rw.Int32(ref u03);
            }
            if (v >= 2)
            {
                if (v <= 5)
                {
                    rw.Int32(ref u04);
                }
                if (v <= 3)
                {
                    rw.Byte(ref u05);
                }
                if (v == 2)
                {
                    rw.String(ref u06);
                    rw.String(ref u07);
                }
            }
            if (v >= 7)
            {
                rw.Int32(ref u08);
            }
            if (v >= 8)
            {
                rw.Int32(ref u09);
            }
        }
    }

    public sealed partial class Unknown3 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Boolean(ref u04);
        }
    }

    public sealed partial class Sticker : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref u01);
            rw.String(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x031CC000 => new Chunk031CC000(),
        0x031CC001 => new Chunk031CC001(),
        0x031CC002 => new Chunk031CC002(),
        0x031CC003 => new Chunk031CC003(),
        0x031CC004 => new Chunk031CC004(),
        0x031CC005 => new Chunk031CC005(),
        0x031CC006 => new Chunk031CC006(),
        0x031CC007 => new Chunk031CC007(),
        0x031CC008 => new Chunk031CC008(),
        0x031CC009 => new Chunk031CC009(),
        0x031CC00A => new Chunk031CC00A(),
        0x031CC00B => new Chunk031CC00B(),
        0x031CC00C => new Chunk031CC00C(),
        0x031CC00D => new Chunk031CC00D(),
        0x031CC00E => new Chunk031CC00E(),
        0x031CC00F => new Chunk031CC00F(),
        0x031CC010 => new Chunk031CC010(),
        0x031CC011 => new Chunk031CC011(),
        0x031CC012 => new Chunk031CC012(),
        0x031CC014 => new Chunk031CC014(),
        0x031CC015 => new Chunk031CC015(),
        0x031CC016 => new Chunk031CC016(),
        0x031CC017 => new Chunk031CC017(),
        0x031CC018 => new Chunk031CC018(),
        0x031CC019 => new Chunk031CC019(),
        0x031CC01A => new Chunk031CC01A(),
        0x031CC01B => new Chunk031CC01B(),
        0x031CC01C => new Chunk031CC01C(),
        0x031CC01D => new Chunk031CC01D(),
        0x031CC01E => new Chunk031CC01E(),
        0x031CC01F => new Chunk031CC01F(),
        0x031CC020 => new Chunk031CC020(),
        0x031CC021 => new Chunk031CC021(),
        0x031CC022 => new Chunk031CC022(),
        0x031CC023 => new Chunk031CC023(),
        0x031CC024 => new Chunk031CC024(),
        _ => base.NewChunk(chunkId),
    };
}
