namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x0702B000</remarks>
[Class(0x0702B000)]
public partial class CControlIconIndex : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0702B000;




    private string? name;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public string? Name { get => name; set => name = value; }

    private int indexOff;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOff { get => indexOff; set => indexOff = value; }

    private int indexOffFocused;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOffFocused { get => indexOffFocused; set => indexOffFocused = value; }

    private int indexOffGrayed;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOffGrayed { get => indexOffGrayed; set => indexOffGrayed = value; }

    private int indexOn;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOn { get => indexOn; set => indexOn = value; }

    private int indexOnFocused;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOnFocused { get => indexOnFocused; set => indexOnFocused = value; }

    private int indexOnGrayed;
    [AppliedWithChunk<Chunk0702B000>]
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int IndexOnGrayed { get => indexOnGrayed; set => indexOnGrayed = value; }

    private int marginPercentU;
    [AppliedWithChunk<Chunk0702B001>]
    [AppliedWithChunk<Chunk0702B002>]
    public int MarginPercentU { get => marginPercentU; set => marginPercentU = value; }

    private int marginPercentV;
    [AppliedWithChunk<Chunk0702B002>]
    public int MarginPercentV { get => marginPercentV; set => marginPercentV = value; }

    private Vec2 marginSize;
    [AppliedWithChunk<Chunk0702B002>]
    public Vec2 MarginSize { get => marginSize; set => marginSize = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CControlIconIndex"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlIconIndex() { }


    /// <summary>
    /// CControlIconIndex 0x000 chunk
    /// </summary>
    [Chunk(0x0702B000)]
    public partial class Chunk0702B000 : Chunk<CControlIconIndex>
    {
        /// <inheritdoc />
        public override uint Id => 0x0702B000;


        public override void ReadWrite(CControlIconIndex n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
            rw.Int32(ref n.indexOff);
            rw.Int32(ref n.indexOffFocused);
            rw.Int32(ref n.indexOffGrayed);
            rw.Int32(ref n.indexOn);
            rw.Int32(ref n.indexOnFocused);
            rw.Int32(ref n.indexOnGrayed);
        }
    }

    /// <summary>
    /// CControlIconIndex 0x001 chunk
    /// </summary>
    [Chunk(0x0702B001)]
    public partial class Chunk0702B001 : Chunk0702B000
    {
        /// <inheritdoc />
        public override uint Id => 0x0702B001;


        public override void ReadWrite(CControlIconIndex n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref n.marginPercentU);
        }
    }

    /// <summary>
    /// CControlIconIndex 0x002 chunk
    /// </summary>
    [Chunk(0x0702B002)]
    public partial class Chunk0702B002 : Chunk0702B001
    {
        /// <inheritdoc />
        public override uint Id => 0x0702B002;


        public override void ReadWrite(CControlIconIndex n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref n.marginPercentV);
            rw.Vec2(ref n.marginSize);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0702B000 => new Chunk0702B000(),
        0x0702B001 => new Chunk0702B001(),
        0x0702B002 => new Chunk0702B002(),
        _ => base.NewChunk(chunkId),
    };
}
