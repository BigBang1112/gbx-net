using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Hashing;
using GBX.NET.LZO;
using System.Text;
using TmEssentials;

Gbx.LZO = new MiniLZO();
Gbx.CRC32 = new CRC32();

var cars = new[] { "CarSport", "CarSnow", "CarRally", "CarDesert", "CharacterPilot" };
var envs = new[] { "Stadium", "Snow", "Rally", "Desert" };

var includeCarSport = true;
var includeCarSnow = true;
var includeCarRally = true;
var includeCarDesert = true;
var includeCharacterPilot = false;
var generateDefaultCarVariant = true;
var mapFormat = "$<{0}$> - {1}";

foreach (var arg in args)
{
    var map = Gbx.ParseNode<CGameCtnChallenge>(arg);
    
    var includes = new[]
    {
        includeCarSport, includeCarSnow, includeCarRally, includeCarDesert, includeCharacterPilot
    };

    var defaultCar = map.PlayerModel?.Id;
    if (string.IsNullOrEmpty(defaultCar))
    {
        defaultCar = "CarSport";
    }

    var defaultMapUid = map.MapUid;
    var defaultMapName = map.MapName;

    for (int i = 0; i < cars.Length; i++)
    {
        if (i > 0)
        {
            map = Gbx.ParseNode<CGameCtnChallenge>(arg);
        }

        var car = cars[i];
        var env = envs.Length > i ? envs[i] : null;
        var include = includes[i];

        if (!include)
        {
            continue;
        }

        if (!generateDefaultCarVariant && car == defaultCar)
        {
            continue;
        }

        map.PlayerModel = (car, 10003, "");
        map.MapUid = $"{Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString()))[..10]}{defaultMapUid.Substring(9, 10)}ENVIMIX";
        map.MapName = string.Format(mapFormat, defaultMapName, car);

        if (env is not null)
        {
            ChangeGates(map, env);
        }

        map.TMObjective_AuthorTime = TimeInt32.MaxValue;
        map.TMObjective_GoldTime = TimeInt32.MaxValue;
        map.TMObjective_SilverTime = TimeInt32.MaxValue;
        map.TMObjective_BronzeTime = TimeInt32.MaxValue;

        if (map.ChallengeParameters is not null)
        {
            map.ChallengeParameters.AuthorTime = TimeInt32.MaxValue;
            map.ChallengeParameters.GoldTime = TimeInt32.MaxValue;
            map.ChallengeParameters.SilverTime = TimeInt32.MaxValue;
            map.ChallengeParameters.BronzeTime = TimeInt32.MaxValue;
        }

        map.CrackPassword();

        Directory.CreateDirectory("output");

        var pureFileName = $"{TextFormatter.Deformat(map.MapName)}.Map.Gbx";
        var validFileName = string.Join("_", pureFileName.Split(Path.GetInvalidFileNameChars()));

        map.Save(Path.Combine("output", validFileName), 0x03043000);
    }
}

void ChangeGates(CGameCtnChallenge map, string envimixEnvironment)
{
    var gatesToRemove = new List<CGameCtnAnchoredObject>();

    foreach (var block in map.GetBlocks().Where(block => block.Name.Contains("Gameplay")))
    {
        for (int i = 0; i < envs.Length; i++)
        {
            var env = envs[i];

            if (block.Name.Contains($"Gameplay{env}"))
            {
                block.Name = block.Name.Replace(env, envimixEnvironment);
            }
        }
    }

    foreach (var item in map.GetAnchoredObjects().Where(item => item.ItemModel.Id.Contains("Gameplay")))
    {
        for (int i = 0; i < envs.Length; i++)
        {
            var env = envs[i];

            if (item.ItemModel.Id.Contains($"Gameplay{env}"))
            {
                item.ItemModel = item.ItemModel with { Id = item.ItemModel.Id.Replace(env, envimixEnvironment) };
            }

            if (item.ItemModel.Id.Contains($"{env}GateGameplay"))
            {
                if (envimixEnvironment == "Stadium")
                {
                    gatesToRemove.Add(item);
                }
                else
                {
                    item.ItemModel = item.ItemModel with { Id = item.ItemModel.Id.Replace(env, envimixEnvironment) };
                }
            }
        }
    }

    foreach (var gate in gatesToRemove)
    {
        map.AnchoredObjects!.Remove(gate);
    }
}