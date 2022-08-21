namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    public class BotPath
    {
        private int clan;
        private IList<Vec3>? path;
        private bool isFlying;
        private CGameWaypointSpecialProperty? waypointSpecialProperty;
        private bool isAutonomous;

        public int Clan { get => clan; set => clan = value; }
        public IList<Vec3>? Path { get => path; set => path = value; }
        public bool IsFlying { get => isFlying; set => isFlying = value; }
        public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }
        public bool IsAutonomous { get => isAutonomous; set => isAutonomous = value; }

        internal void ReadWrite(GameBoxReaderWriter rw)
        {
            rw.Int32(ref clan);
            rw.List<Vec3>(ref path, r => r.ReadVec3(), (x, w) => w.Write(x));
            rw.Boolean(ref isFlying);
            rw.NodeRef<CGameWaypointSpecialProperty>(ref waypointSpecialProperty);
            rw.Boolean(ref isAutonomous);
        }
    }
}
