namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallengeParameters
{
    public partial class Chunk0305B00F
    {
        public int U01;

        public override void Read(CGameCtnChallengeParameters n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);
            
            n.raceValidateGhost = r.ReadNodeRef<CGameCtnGhost>();
        }

        public override void Write(CGameCtnChallengeParameters n, GbxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.WriteNodeRef(n.raceValidateGhost);

            w.Write((int)ms.Length);
            ms.WriteTo(w.BaseStream);
        }
    }
}
