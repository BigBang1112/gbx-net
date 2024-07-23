using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.LZO;
using GBX.NET.ZLib;

Gbx.LZO = new Lzo();
Gbx.ZLib = new ZLib();

var gbxMap = Gbx.Parse<CGameCtnChallenge>(args[0]);
var ghost = Gbx.ParseNode<CGameCtnGhost>(args[1]);

if (ghost.RecordData is null) return;

var envs = new string[] { "Stadium", "Snow", "Rally", "Desert" };
var envIndex = 0;

var samples = ghost.RecordData
    .EntList.SelectMany(x => x.Samples)
    .OfType<CSceneVehicleVis.EntRecordDelta>();

foreach (var sample in samples)
{
    var pos = sample.Position;
    var rot = sample.PitchYawRoll;

    gbxMap.Node.PlaceAnchoredObject(
        ($"GateGameplay{envs[envIndex]}32m", new(26), "Nadeo"),
        pos,
        (rot.Y, rot.X, rot.Z));

    envIndex = (envIndex + 1) % envs.Length;
}

gbxMap.Save(gbxMap.GetFileNameWithoutExtension() + "-blik.Map.Gbx");
