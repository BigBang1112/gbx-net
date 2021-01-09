using System.Collections.Generic;
using GBX.NET.Engines.GameData;

namespace GBX.NET.Engines.Game
{
    public class CGameScriptMapBotPath
    {
        public int Clan { get; set; }
        public List<Vec3> Path { get; set; }
        public bool IsFlying { get; set; }
        public CGameWaypointSpecialProperty WaypointSpecialProperty { get; set; }
        public bool IsAutonomous { get; set; }
    }
}
