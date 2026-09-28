namespace GBX.NET.Engines.Input;

/// <remarks>ID: 0x13006000</remarks>
[Class(0x13006000)]
public partial class CInputBindingsConfig : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x13006000;




    private Binding[]? bindings;
    [AppliedWithChunk<Chunk13006000>]
    [AppliedWithChunk<Chunk13006003>]
    public Binding[]? Bindings { get => bindings; set => bindings = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CInputBindingsConfig"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CInputBindingsConfig() { }


    /// <summary>
    /// CInputBindingsConfig 0x000 chunk
    /// </summary>
    [Chunk(0x13006000)]
    public partial class Chunk13006000 : Chunk<CInputBindingsConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x13006000;

        public string? U01;

        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.ArrayReadableWritable<Binding>(ref n.bindings!);
        }
    }

    /// <summary>
    /// CInputBindingsConfig 0x001 chunk
    /// </summary>
    [Chunk(0x13006001)]
    public partial class Chunk13006001 : Chunk<CInputBindingsConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x13006001;

        public string[]? U01;

        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            rw.ArrayId(ref U01!);
        }
    }

    /// <summary>
    /// CInputBindingsConfig 0x002 skippable chunk
    /// </summary>
    [Chunk(0x13006002)]
    public partial class Chunk13006002 : SkippableChunk<CInputBindingsConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x13006002;

        public string? U01;
        public int U02;

        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CInputBindingsConfig 0x003 chunk
    /// </summary>
    [Chunk(0x13006003)]
    public partial class Chunk13006003 : Chunk<CInputBindingsConfig>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x13006003;

        public int Version { get; set; }


        public override void ReadWrite(CInputBindingsConfig n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Binding>(ref n.bindings!, version: Version + 1);
        }
    }


    public sealed partial class Binding : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? deviceId;
        public string? DeviceId { get => deviceId; set => deviceId = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int isAnalog;
        public int IsAnalog { get => isAnalog; set => isAnalog = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private string? name;
        public string? Name { get => name; set => name = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v == 0)
            {
                rw.Int32(ref u01);
            }
            rw.Id(ref deviceId);
            if (v >= 1)
            {
                rw.Int32(ref u02);
                rw.Int32(ref u01);
            }
            rw.Int32(ref isAnalog);
            rw.Int32(ref u03);
            rw.String(ref name);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x13006000 => new Chunk13006000(),
        0x13006001 => new Chunk13006001(),
        0x13006002 => new Chunk13006002(),
        0x13006003 => new Chunk13006003(),
        _ => base.NewChunk(chunkId),
    };
}
