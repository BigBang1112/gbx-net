namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05018000</remarks>
[Class(0x05018000)]
public abstract partial class CFuncLight : CFuncPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x05018000;




    private EFctType fctType;
    [AppliedWithChunk<Chunk05018000>]
    public EFctType FctType { get => fctType; set => fctType = value; }

    private float flickPeriod;
    [AppliedWithChunk<Chunk05018000>]
    public float FlickPeriod { get => flickPeriod; set => flickPeriod = value; }

    private int flickCount;
    [AppliedWithChunk<Chunk05018000>]
    public int FlickCount { get => flickCount; set => flickCount = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncLight"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncLight() { }


    /// <summary>
    /// CFuncLight 0x000 chunk
    /// </summary>
    [Chunk(0x05018000)]
    public partial class Chunk05018000 : Chunk<CFuncLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x05018000;


        public override void ReadWrite(CFuncLight n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EFctType>(ref n.fctType);
            rw.Single(ref n.flickPeriod);
            rw.Int32(ref n.flickCount);
        }
    }



    public enum EFctType
    {
        Sinus,
        Flick,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05018000 => new Chunk05018000(),
        _ => base.NewChunk(chunkId),
    };
}
