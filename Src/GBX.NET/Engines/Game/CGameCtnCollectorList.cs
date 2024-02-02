
namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollectorList
{
    public partial class Chunk0301B000
    {
        public override void Read(CGameCtnCollectorList n, GbxReader r)
        {
            var count = r.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                r.ReadIdent();
                r.ReadInt32();
            }
        }

        public override void Write(CGameCtnCollectorList n, GbxWriter w)
        {
            w.Write(0);
        }
    }
}
