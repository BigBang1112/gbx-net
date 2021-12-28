using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Linq;
using TmEssentials;

namespace ReplayToMap;

class Program
{
    static void Main(string[] args)
    {
        var fileName = args.FirstOrDefault();
        if (fileName == null) return;

        var node = GameBox.ParseNode(fileName);

        if (node is CGameCtnReplayRecord replay)
        {
            var map = replay.Challenge;
            map.Save(Formatter.Deformat(map.MapName + ".Map.Gbx"));
        }
    }
}
