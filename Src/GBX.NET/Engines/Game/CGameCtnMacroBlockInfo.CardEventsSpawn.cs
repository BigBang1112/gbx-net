namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public class CardEventsSpawn : IReadableWritable, IVersionable
    {
        private int ver;

        public Ident[]? U01;
        public Int3 U02;

        public int Version { get => ver; set => ver = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref ver);
            rw.Array(ref U01, r => r.ReadIdent(), (x, w) => w.Write(x));
            rw.Int3(ref U02);
        }
    }
}
