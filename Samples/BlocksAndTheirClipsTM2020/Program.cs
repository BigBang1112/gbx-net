using GBX.NET;
using GBX.NET.BlockInfo;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using System;
using System.Linq;
using System.Text.Json;

if (args.Length == 0)
{
    return;
}

var blockFolder = args[0];

GameBox.OpenPlanetHookExtractMode = true;

var dict = new Dictionary<string, BlockModel>();

foreach (var blockInfoFile in Directory.EnumerateFiles(blockFolder, "*.*", SearchOption.AllDirectories))
{
    try
    {
        if (GameBox.ParseNode(blockInfoFile) is not CGameCtnBlockInfoClassic blockInfo)
        {
            continue;
        }

        var block = new BlockModel
        {
            Air = CreateVariant(blockInfo.VariantBaseAir),
            Ground = CreateVariant(blockInfo.VariantBaseGround)
        };

        dict.Add(blockInfo.Author.Id, block);
    }
    catch (NotAGbxException)
    {
        
    }
}

File.WriteAllText("Clips.json", JsonSerializer.Serialize(dict, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull}));

BlockInfoWriter.Write("Clips.bi", dict);
var nice = BlockInfoReader.Read("Clips.bi");
BlockInfoWriter.Write("Clips.bi", nice);
var doubleNice = BlockInfoReader.Read("Clips.bi");

Console.WriteLine();

static BlockUnit[]? CreateVariant(CGameCtnBlockInfoVariant? blockInfoVariant)
{
    if (blockInfoVariant?.BlockUnitModels is null)
    {
        return null;
    }

    return blockInfoVariant
        .BlockUnitModels
        .OfType<CGameCtnBlockUnitInfo>()
        .Select(x => new BlockUnit
        {
            Coord = x.RelativeOffset,
            NorthClips = GetClipInfo(x.ClipsNorth),
            EastClips = GetClipInfo(x.ClipsEast),
            SouthClips = GetClipInfo(x.ClipsSouth),
            WestClips = GetClipInfo(x.ClipsWest),
            TopClips = GetClipInfo(x.ClipsTop),
            BottomClips = GetClipInfo(x.ClipsBottom)
        }).ToArray();
}

static string[]? GetClipInfo(ExternalNode<CGameCtnBlockInfoClip>[]? clips)
{
    return clips?.Length > 0
        ? clips.Select(x => Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(x.File?.FileName)))
        .OfType<string>()
        .ToArray() : null;
}