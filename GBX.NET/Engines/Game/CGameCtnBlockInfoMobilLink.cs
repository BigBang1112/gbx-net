using GBX.NET.Engines.GameData;
using System;
using System.Collections.Generic;
using System.Text;

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
