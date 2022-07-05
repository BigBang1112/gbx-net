namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07002000</remarks>
[Node(0x07002000)]
public class CControlContainer : CControlBase
{
    #region Fields

    private bool acceptOwnControls;
    private bool useScript;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    public bool AcceptOwnControls { get => acceptOwnControls; set => acceptOwnControls = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool UseScript { get => useScript; set => useScript = value; }

    #endregion

    #region Constructors

    protected CControlContainer()
    {

    }

    #endregion

    #region Chunks

    #region 0x005 chunk

    [Chunk(0x07002005)]
    public class Chunk07002005 : Chunk<CControlContainer>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CControlContainer n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Boolean(ref n.acceptOwnControls);
            rw.Boolean(ref n.useScript);

            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);

            if (!n.acceptOwnControls)
            {
                // CControlContainer::ArchiveChilds
                
            }
        }
    }

    #endregion

    #endregion
}
