namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BB000</remarks>
[Node(0x090BB000)]
public class CPlugSolid2Model : CMwNod
{
    protected CPlugSolid2Model()
    {
        
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CPlugSolid2Model 0x000 chunk
    /// </summary>
    [Chunk(0x090BB000), IgnoreChunk]
    public class Chunk090BB000 : Chunk<CPlugSolid2Model>, IVersionable
    {
        private int version;

        public string? U01;
        public int U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSolid2Model n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref U01);
            rw.Int32(ref U02); // SShadedGeom array
            // ...
        }
    }

    #endregion

    #endregion
}
