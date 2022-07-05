namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910C000</remarks>
[Node(0x0910C000)]
public abstract class CPlugCamControlModel : CMwNod
{
    #region Fields

    private CPlugCamShakeModel? shake;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    public CPlugCamShakeModel? Shake { get => shake; set => shake = value; }

    #endregion

    #region Constructors

    protected CPlugCamControlModel()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CPlugCamControlModel 0x000 chunk
    /// </summary>
    [Chunk(0x0910C000)]
    public class Chunk0910C000 : Chunk<CPlugCamControlModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugCamControlModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugCamShakeModel>(ref n.shake);
        }
    }

    #endregion

    #endregion
}