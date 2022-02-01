using GBX.NET;
using GBX.NET.Engines.Game;
using System.Linq;
using TmEssentials;

var fileName = args.FirstOrDefault();
if (fileName is null) return;

var node = GameBox.ParseNode(fileName);

if (node is CGameCtnReplayRecord replay)
{
    var map = replay.Challenge;
    map.Save(Formatter.Deformat(map.MapName + ".Map.Gbx"));
}
