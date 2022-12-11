namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910B000</remarks>
[Node(0x0910B000)]
public class CPlugCamShakeModel : CMwNod
{
    internal CPlugCamShakeModel()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugCamShakeModel 0x000 chunk
    /// </summary>
    [Chunk(0x0910B000)]
    public class Chunk0910B000 : Chunk<CPlugCamShakeModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public int U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float? U07;
        private int? U08;
        private int? U09;
        private float? U10;
        private float? U11;
        private float? U12;
        private float? U13;
        private float? U14;

        public override void ReadWrite(CPlugCamShakeModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);

            if (version >= 2)
            {
                rw.Single(ref U07);

                if (version >= 3)
                {
                    rw.Int32(ref U08);
                    rw.Int32(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);
                    rw.Single(ref U13);
                    rw.Single(ref U14);
                }
            }
        }
    }

    #endregion
}
