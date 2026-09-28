namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A083000</remarks>
[Class(0x0A083000)]
public partial class CSceneVehicleCarMarksSamples : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A083000;




    private uint[]? stops;
    [AppliedWithChunk<Chunk0A083002>]
    public uint[]? Stops { get => stops; set => stops = value; }

    private string? name;
    [AppliedWithChunk<Chunk0A083003>]
    public string? Name { get => name; set => name = value; }

    private bool disabled;
    [AppliedWithChunk<Chunk0A083004>]
    public bool Disabled { get => disabled; set => disabled = value; }

    private Sample[]? samples;
    [AppliedWithChunk<Chunk0A083006>]
    public Sample[]? Samples { get => samples; set => samples = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneVehicleCarMarksSamples"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneVehicleCarMarksSamples() { }


    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x002 chunk
    /// </summary>
    [Chunk(0x0A083002)]
    public partial class Chunk0A083002 : Chunk<CSceneVehicleCarMarksSamples>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A083002;


        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GbxReaderWriter rw)
        {
            rw.Array<uint>(ref n.stops!);
        }
    }

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x003 chunk
    /// </summary>
    [Chunk(0x0A083003)]
    public partial class Chunk0A083003 : Chunk<CSceneVehicleCarMarksSamples>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A083003;


        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x004 chunk
    /// </summary>
    [Chunk(0x0A083004)]
    public partial class Chunk0A083004 : Chunk<CSceneVehicleCarMarksSamples>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A083004;


        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.disabled);
        }
    }

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x006 chunk
    /// </summary>
    [Chunk(0x0A083006)]
    public partial class Chunk0A083006 : Chunk<CSceneVehicleCarMarksSamples>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0A083006;

        public int Version { get; set; }


        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                throw new ("");
            }
            rw.ArrayReadableWritable<Sample>(ref n.samples!);
        }
    }


    public sealed partial class Sample : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

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

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Int32(ref u10);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A083002 => new Chunk0A083002(),
        0x0A083003 => new Chunk0A083003(),
        0x0A083004 => new Chunk0A083004(),
        0x0A083006 => new Chunk0A083006(),
        _ => base.NewChunk(chunkId),
    };
}
