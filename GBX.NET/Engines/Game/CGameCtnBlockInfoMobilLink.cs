using GBX.NET.Engines.GameData;

namespace GBX.NET.Engines.Game
{
    public class CGameCtnBlockInfoMobilLink
    {
        public string SocketID { get; set; }
        public CGameObjectModel Model { get; set; }

        public CGameCtnBlockInfoMobilLink(string socketID, CGameObjectModel model)
        {
            SocketID = socketID;
            Model = model;
        }
    }
}
