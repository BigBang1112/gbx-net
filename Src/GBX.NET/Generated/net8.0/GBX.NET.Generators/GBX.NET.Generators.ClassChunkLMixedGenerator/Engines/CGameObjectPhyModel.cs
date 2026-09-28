namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E006000</remarks>
[Class(0x2E006000)]
public partial class CGameObjectPhyModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E006000;





    /// <summary>
    /// CGameObjectPhyModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E006001)]
    public partial class Chunk2E006001 : Chunk<CGameObjectPhyModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E006001;

    }

    /// <summary>
    /// CGameObjectPhyModel 0x003 chunk
    /// </summary>
    [Chunk(0x2E006003)]
    public partial class Chunk2E006003 : Chunk<CGameObjectPhyModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E006003;

        public bool U01;

        public override void ReadWrite(CGameObjectPhyModel n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            if (U01)
            {
                throw new ("");
            }
        }
    }



    public enum EPersistence
    {
        OnPlayerUnspawn,
        OnPlayerRemoved,
        NeverRemove,
    }

    public enum EProgram
    {
        None,
        Target,
        Turret,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E006001 => new Chunk2E006001(),
        0x2E006003 => new Chunk2E006003(),
        _ => base.NewChunk(chunkId),
    };
}
