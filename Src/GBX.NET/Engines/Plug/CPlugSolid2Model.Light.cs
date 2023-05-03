namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    public class Light : IReadableWritable
    {
        private string u01 = "";
        private bool u02 = true;
        private CPlugLight? u03;
        private string? u04;
        private Iso4 u05;
        private int u06;
        private int u07;
        private int u08;
        private int u09;
        private int u10;
        private int u11;
        private int u12;
        private int u13;
        private int u14;
        private bool u15;
        private float u16;
        private float u17;
        private float u18;
        private GameBoxRefTable.File? u03File;

        public string U01 { get => u01; set => u01 = value; }
        public bool U02 { get => u02; set => u02 = value; }
        public CPlugLight? U03 { get => u03; set => u03 = value; }
        public GameBoxRefTable.File? U03File { get => u03File; set => u03File = value; }
        public string? U04 { get => u04; set => u04 = value; }
        public Iso4 U05 { get => u05; set => u05 = value; }
        public int U06 { get => u06; set => u06 = value; }
        public int U07 { get => u07; set => u07 = value; }
        public int U08 { get => u08; set => u08 = value; }
        public int U09 { get => u09; set => u09 = value; }
        public int U10 { get => u10; set => u10 = value; }
        public int U11 { get => u11; set => u11 = value; }
        public int U12 { get => u12; set => u12 = value; }
        public int U13 { get => u13; set => u13 = value; }
        public int U14 { get => u14; set => u14 = value; }
        public bool U15 { get => u15; set => u15 = value; }
        public float U16 { get => u16; set => u16 = value; }
        public float U17 { get => u17; set => u17 = value; }
        public float U18 { get => u18; set => u18 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!); // id of the light
            rw.Boolean(ref u02); // defaults to true

            if (u02)
            {
                rw.NodeRef<CPlugLight>(ref u03, ref u03File); // CPlugLight external
            }
            else
            {
                rw.String(ref u04);
            }

            rw.Iso4(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Int32(ref u10);
            rw.Int32(ref u11);

            if (version >= 26)
            {
                rw.Int32(ref u12);
                rw.Int32(ref u13);
                rw.Int32(ref u14);
            }
            else
            {
                // u12 and u13 is 0
            }

            rw.Boolean(ref u15);
            
            if (u15)
            {
                rw.Single(ref u16);
                rw.Single(ref u17);
                rw.Single(ref u18);
            }
        }
    }
}
