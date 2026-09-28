namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090EB000</remarks>
[Class(0x090EB000)]
public partial class CPlugVehiclePhyTuning : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090EB000;




    private string name = string.Empty;
    [AppliedWithChunk<Chunk090EB000>]
    public string Name { get => name; set => name = value; }


    /// <summary>
    /// CPlugVehiclePhyTuning 0x000 chunk
    /// </summary>
    [Chunk(0x090EB000)]
    public partial class Chunk090EB000 : Chunk<CPlugVehiclePhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090EB000;

        public CFuncKeysReal? U01;
        public float U02;
    }

    /// <summary>
    /// CPlugVehiclePhyTuning 0x001 chunk
    /// </summary>
    [Chunk(0x090EB001)]
    public partial class Chunk090EB001 : Chunk<CPlugVehiclePhyTuning>
    {
        /// <inheritdoc />
        public override uint Id => 0x090EB001;

        public bool U01;

        public override void ReadWrite(CPlugVehiclePhyTuning n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090EB000 => new Chunk090EB000(),
        0x090EB001 => new Chunk090EB001(),
        _ => base.NewChunk(chunkId),
    };
}
