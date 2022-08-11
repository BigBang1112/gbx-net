namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    public class PlayerInputData : IReadableWritable
    {
        public int U01; // 8 in shootmania, 12 in tm2020
        public int U02;
        public int U03 = -1;
        public int U04;
        public byte[]? U05;
        
        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref U01); // 8 in shootmania, 12 in tm2020
            rw.Int32(ref U02);
            
            if (version >= 4)
            {
                rw.Int32(ref U03);
            }
            
            rw.Int32(ref U04);
            rw.Bytes(ref U05);
        }
    }
}
