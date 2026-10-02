namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    public partial class Chunk0304304E
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Encapsulated(rw =>
            {
                rw.NodeRef(ref n.challengeAnimation, ref n.challengeAnimationFile);
                U01 = rw.Int32(n.animationTriggers?.Length ?? 0);
                rw.Int32(ref U02);
                rw.ArrayReadableWritable(ref n.animationTriggers, U01, version: U02);
            });
        }
    }
}
