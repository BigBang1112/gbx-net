namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03130000</remarks>
[Class(0x03130000)]
public partial class CGamePlayerProfileChunk_VehiclesSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03130000;




    private VehicleProfile[]? vehicleProfiles;
    [AppliedWithChunk<Chunk03130002>]
    public VehicleProfile[]? VehicleProfiles { get => vehicleProfiles; set => vehicleProfiles = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_VehiclesSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_VehiclesSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_VehiclesSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03130000)]
    public partial class Chunk03130000 : SkippableChunk<CGamePlayerProfileChunk_VehiclesSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03130000;

        public int Version { get; set; }

        public string? U01;
        public byte[]? U02;

        public override void ReadWrite(CGamePlayerProfileChunk_VehiclesSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.Data(ref U02!, 32);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_VehiclesSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03130001)]
    public partial class Chunk03130001 : SkippableChunk<CGamePlayerProfileChunk_VehiclesSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03130001;

        public int Version { get; set; }

        public Vec3 U01;
        public float U02 = 1;

        public override void ReadWrite(CGamePlayerProfileChunk_VehiclesSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref U01);
            if (Version >= 2)
            {
                rw.Single(ref U02);
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_VehiclesSettings 0x002 skippable chunk (VehicleProfiles)
    /// </summary>
    [Chunk(0x03130002, "VehicleProfiles")]
    public partial class Chunk03130002 : SkippableChunk<CGamePlayerProfileChunk_VehiclesSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03130002;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_VehiclesSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<VehicleProfile>(ref n.vehicleProfiles!, version: Version);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_VehiclesSettings 0x003 skippable chunk
    /// </summary>
    [Chunk(0x03130003)]
    public partial class Chunk03130003 : SkippableChunk<CGamePlayerProfileChunk_VehiclesSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03130003;

        public int Version { get; set; }

        public TransQuat[]? U01;
        public byte[]? U02;
        public byte[]? U03;
        public TransQuat[]? U04;
        public byte[]? U05;
        public byte[]? U06;
        public UInt128 U07;
        public UInt128 U08;
        public ushort[]? U09;
        public ushort[]? U10;
        public TransQuat[]? U11;
        public TransQuat[]? U12;

        public override void ReadWrite(CGamePlayerProfileChunk_VehiclesSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<TransQuat>(ref U01!);
            if (Version >= 2)
            {
                if (Version <= 4)
                {
                    rw.Data(ref U02!, 32);
                    rw.Data(ref U03!, 32);
                }
                if (Version >= 3)
                {
                    rw.Array<TransQuat>(ref U04!);
                    if (Version == 4)
                    {
                        rw.Data(ref U05!, 32);
                        rw.Data(ref U06!, 32);
                    }
                    if (Version >= 5)
                    {
                        if (Version == 5)
                        {
                            rw.UInt128(ref U07);
                            rw.UInt128(ref U08);
                        }
                        if (Version == 6)
                        {
                            rw.Array<ushort>(ref U09!);
                            rw.Array<ushort>(ref U10!);
                            rw.Array<TransQuat>(ref U11!);
                            rw.Array<TransQuat>(ref U12!);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_VehiclesSettings 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03130004)]
    public partial class Chunk03130004 : SkippableChunk<CGamePlayerProfileChunk_VehiclesSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03130004;

        public int Version { get; set; }

        public Unknown[]? U01;

        public override void ReadWrite(CGamePlayerProfileChunk_VehiclesSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Unknown>(ref U01!);
        }
    }


    public sealed partial class VehicleProfile : IReadableWritable
    {

        private Ident? playerModel;
        public Ident? PlayerModel { get => playerModel; set => playerModel = value; }

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private byte[]? u03;
        public byte[]? U03 { get => u03; set => u03 = value; }

        private int gameCamera;
        public int GameCamera { get => gameCamera; set => gameCamera = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        private bool u07;
        public bool U07 { get => u07; set => u07 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref playerModel);
            rw.String(ref u01);
            rw.String(ref u02);
            rw.Data(ref u03!, 32);
            rw.Int32(ref gameCamera);
            rw.Single(ref u04);
            rw.Single(ref u05);
            if (v >= 2)
            {
                rw.Boolean(ref u06);
                if (v >= 3)
                {
                    rw.Boolean(ref u07);
                }
            }
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private Ident? u01;
        public Ident? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private byte[]? u04;
        public byte[]? U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private bool u09;
        public bool U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref u01);
            rw.String(ref u02);
            rw.String(ref u03);
            rw.Data(ref u04!, 32);
            rw.Boolean(ref u05);
            rw.Int32(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Boolean(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03130000 => new Chunk03130000(),
        0x03130001 => new Chunk03130001(),
        0x03130002 => new Chunk03130002(),
        0x03130003 => new Chunk03130003(),
        0x03130004 => new Chunk03130004(),
        _ => base.NewChunk(chunkId),
    };
}
