namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    public class PlayerInputData : IReadableWritable
    {
        private int u01; // 8 in shootmania, 12 in tm2020
        private int u02;
        private int u03 = -1;
        private int u04;
        private byte[]? u05;

        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public byte[]? U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01); // 8 in shootmania, 12 in tm2020
            rw.Int32(ref u02);
            
            if (version >= 4)
            {
                rw.Int32(ref u03);
            }
            
            rw.Int32(ref u04);
            rw.Bytes(ref u05);
        }
    }
}
