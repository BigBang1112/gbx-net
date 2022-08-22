namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x0700F000</remarks>
[Node(0x0700F000), WritingNotSupported]
public class CControlList : CControlContainer
{
    #region Constructors
    
    protected CControlList()
    {

    }

    #endregion

    #region Chunks

    #region 0x007 chunk

    /// <summary>
    /// CControlList 0x007 chunk
    /// </summary>
    [Chunk(0x0700F007)]
    public class Chunk07002007 : Chunk<CControlList>
    {
        private int U01;
        private int U02;
        private int U03;
        private int U04;

        public override void ReadWrite(CControlList n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);

            rw.Int32(ref U04);
            rw.Int32(ref U04);
            rw.Int32(ref U04);
            rw.Int32(ref U04);
            rw.Int32(ref U04);

            for (var i = 0; i < U04; i++)
            {
                var str = rw.String();
                var flo = rw.Single();
                var id = rw.Id();
                var boo = rw.Boolean();
                var randomNumber = rw.Int32();
            }
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CControlList 0x00A chunk
    /// </summary>
    [Chunk(0x0700F00A)]
    public class Chunk0700200A : Chunk<CControlList>
    {
        private float U01;
        private float U02;
        private float U03;
        private int U04;
        private bool U05;
        private string? U06;
        private Node? U07;
        private float U08;
        private float U09;
        private bool U10;

        public override void ReadWrite(CControlList n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Boolean(ref U05);

            // CControlBase::CStyleSheetElem::Archive
            rw.Id(ref U06);
            if (U06 == "")
            {
                rw.NodeRef(ref U07);
            }

            rw.Single(ref U08);

            rw.Single(ref U09);
            rw.Boolean(ref U10);
        }
    }

    #endregion

    #endregion
}
