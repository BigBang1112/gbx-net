using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace IslandConverter
{
    public static class IslandConverter
    {
        internal static GameBox<CGameCtnChallenge> LoadGBX(string fileName, out TimeSpan? time)
        {
            time = null;

            Log.Write("Validating the GBX...");

            Log.Write();

            bool isMap = false;
            using (var fs = File.OpenRead(fileName))
                isMap = GameBox.GetGameBoxType(fs) == typeof(GameBox<CGameCtnChallenge>);

            Log.Write();

            if (!isMap)
            {
                Log.Write("Not a map GBX!", ConsoleColor.Red);
                return null;
            }

            Log.Write("This GBX is a map!", ConsoleColor.Green);

            Log.Write();

            var startMapLoad = DateTime.Now;

            var gbx = GameBox.Parse<CGameCtnChallenge>(fileName);

            if (gbx == null)
            {
                Log.Write("GBX failed to load!", ConsoleColor.Red);
                return null;
            }

            time = DateTime.Now - startMapLoad;

            if (gbx.MainNode.Collection != "Island")
            {
                Log.Write("Not an Island map!", ConsoleColor.Red);
                return null;
            }

            return gbx;
        }

        internal static Int3 DefineMapRange(CGameCtnBlock[] blocks, out Int3? minCoord)
        {
            minCoord = default;
            Int3? maxCoord = default;

            foreach (var b in blocks)
            {
                if (b.Coord != (0, 0, 0))
                {
                    if (minCoord.HasValue)
                    {
                        if (b.Coord.X < minCoord.Value.X) minCoord = (b.Coord.X, minCoord.Value.Y, minCoord.Value.Z);
                        if (b.Coord.Y < minCoord.Value.Y) minCoord = (minCoord.Value.X, b.Coord.Y, minCoord.Value.Z);
                        if (b.Coord.Z < minCoord.Value.Z) minCoord = (minCoord.Value.X, minCoord.Value.Y, b.Coord.Z);
                    }
                    else
                        minCoord = b.Coord;

                    if (maxCoord.HasValue)
                    {
                        if (b.Coord.X > maxCoord.Value.X) maxCoord = (b.Coord.X, maxCoord.Value.Y, maxCoord.Value.Z);
                        if (b.Coord.Y > maxCoord.Value.Y) maxCoord = (maxCoord.Value.X, b.Coord.Y, maxCoord.Value.Z);
                        if (b.Coord.Z > maxCoord.Value.Z) maxCoord = (maxCoord.Value.X, maxCoord.Value.Y, b.Coord.Z);
                    }
                    else
                        maxCoord = b.Coord;
                }
            }

            Int3 mapRange = default;
            if (minCoord.HasValue && maxCoord.HasValue)
                mapRange = maxCoord.Value - (minCoord.Value - (1, 1, 1));

            return mapRange;
        }

        internal static List<CGameCtnBlock> CreateWaterBlocks(Int3 mapSize, List<CGameCtnBlock> islandBlocks, int yOffset, Dictionary<string, BlockConversion[]> conversionInfo, int xzOffset)
        {
            var islandBlockDictionary = islandBlocks.GroupBy(
                x => new Int3(x.Coord.X, 0, x.Coord.Z))
                .Where(x => x.Count(y =>
                {
                    if (conversionInfo.TryGetValue(y.Name, out BlockConversion[] c)
                    && c.Length > y.Variant
                    && c[y.Variant.Value] != null
                    && c[y.Variant.Value].KeepWater)
                        return false;
                    else if (y.IsGround && !y.IsClip)
                        return true;
                    return false;
                }) > 0).ToDictionary(x => x.Key, x => x.First());

            var blocks = new List<CGameCtnBlock>();

            for (int x = 0; x < mapSize.X; x++)
            {
                for (int z = 0; z < mapSize.Z; z++)
                {
                    if (x == 0 || z == 0 || x == mapSize.X - 1 || z == mapSize.Z - 1)
                    {
                        var flag = 135168;
                        var dir = Direction.North;

                        if (x == 0 && z == 0)
                        {
                            flag = 135177;
                            dir = Direction.East;
                        }
                        else if (x == 0 && z == mapSize.Z - 1)
                        {
                            flag = 135177;
                        }
                        else if (x == mapSize.X - 1 && z == 0)
                        {
                            flag = 135177;
                            dir = Direction.South;
                        }
                        else if (x == mapSize.X - 1 && z == mapSize.Z - 1)
                        {
                            flag = 135177;
                            dir = Direction.West;
                        }
                        else if (x == 0)
                        {
                            flag = 135173;
                            dir = Direction.East;
                        }
                        else if (z == 0)
                        {
                            flag = 135173;
                            dir = Direction.South;
                        }
                        else if (x == mapSize.X - 1)
                        {
                            flag = 135173;
                            dir = Direction.West;
                        }
                        else if (z == mapSize.Z - 1)
                        {
                            flag = 135173;
                        }

                        blocks.Add(new CGameCtnBlock("StadiumPool2", dir, (x, yOffset, z), flag, null, null, null));
                    }
                    else if (islandBlockDictionary.TryGetValue(new Int3(Convert.ToInt32(Math.Floor((x + xzOffset) / 2f) - 2), 0, Convert.ToInt32(Math.Floor((z + xzOffset) / 2f) - 2)), out CGameCtnBlock bl))
                    {
                        blocks.Add(new CGameCtnBlock("RemoveGrass.Block.Gbx_CustomBlock", Direction.North, (x, yOffset, z), 135168, null, null, null));
                    }
                    else
                    {
                        blocks.Add(new CGameCtnBlock("StadiumWater2", Direction.North, (x, yOffset, z), 135168, null, null, null));
                    }

                    blocks.Add(CGameCtnBlock.Unassigned1);
                }
            }

            return blocks;
        }

        internal static void ConvertToTM2Island(GameBox<CGameCtnChallenge> gbx, TimeSpan? mapLoadTime, string fileName, string outputFolder, MapSize size, Int3 mapRange, Int3 minCoord, Random randomizer, bool cutoff, bool ignoreMediaTracker)
        {
            var outputFileBase = $"{outputFolder}/{Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName))}.Map.Gbx";
            var logFileBase = $"log/{Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName))}.Map.Gbx";

            Log.Start(logFileBase + ".log");

            var map = gbx.MainNode;

            Log.Write("Converting decoration...");
            map.Decoration.ID = "64x64" + map.Decoration.ID.TrimEnd();

            Log.Write("Converting environment...");
            map.Collection = "Stadium";

            Log.Write("Setting player model...");

            var carTranslations = new Dictionary<string, string>();
            carTranslations[""] = "IslandCar";
            carTranslations["American"] = "DesertCar";
            carTranslations["DesertCar"] = "DesertCar";
            carTranslations["Rally"] = "RallyCar";
            carTranslations["RallyCar"] = "RallyCar";
            carTranslations["SnowCar"] = "SnowCar";
            carTranslations["SportCar"] = "IslandCar";
            carTranslations["IslandCar"] = "IslandCar";
            carTranslations["CoastCar"] = "CoastCar";
            carTranslations["BayCar"] = "BayCar";
            carTranslations["StadiumCar"] = "";

            var chunk00D = map.GetChunk<CGameCtnChallenge.Chunk0304300D>();

            var beforeCar = map.PlayerModel.ID;
            map.PlayerModel = new Ident("IslandCar.Item.Gbx", "Stadium", "adamkooo");

            Log.Write("Applying texture mod...");
            map.ModPackDesc = new FileRef(3, Convert.FromBase64String("AgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="), @"Skins\Stadium\Mod\IslandTM2U.zip", "");

            Bitmap thumbnail;
            if (map.Thumbnail == null)
                thumbnail = new Bitmap(512, 512);
            else
                thumbnail = new Bitmap(map.Thumbnail.Result, 512, 512);

            using (var g = Graphics.FromImage(thumbnail))
            {
                g.DrawImage(Resources.Overlay, 0, 0);
                if (size == MapSize.X45WithSmallBorder)
                    g.DrawImage(Resources.OverlayOpenplanet, 0, 0);
            }

            map.Thumbnail = Task.FromResult(thumbnail);

            map.GetChunk<CGameCtnChallenge.Chunk0304301F>().Version = 6;

            if (size == MapSize.X32WithBigBorder)
            {
                Log.Write("Importing chunk 0x03043043 for water on ground...");
                var chunk = gbx.CreateBodyChunk<CGameCtnChallenge.Chunk03043043>(File.ReadAllBytes("0x03043043.dat"));
            }

            Log.Write("Cracking the map password if presented...");
            map.CrackPassword();

            var startConvert = DateTime.Now;

            var blocks = map.Blocks.ToArray();

            Dictionary<string, BlockConversion[]> conversionInfo;
            Dictionary<string, Dictionary<string, string>> skinInfo;

            Log.Write("Reading IslandConverter.yaml...");

            using (var r = new StreamReader("IslandConverter.yaml"))
            {
                Deserializer yaml = new Deserializer();
                conversionInfo = yaml.Deserialize<Dictionary<string, BlockConversion[]>>(r);
            }

            Log.Write("Reading IslandConverterSkin.yaml...");

            using (var r = new StreamReader("IslandConverterSkin.yaml"))
            {
                Deserializer yaml = new Deserializer();
                skinInfo = yaml.Deserialize<Dictionary<string, Dictionary<string, string>>>(r);
            }

            Log.Write("Preparing to remove additional terrain blocks to avoid Z-fighting...");

            blocks = blocks.Where(x =>
            {
                if (x.Name == "IslandGrass" || x.Name == "IslandBeach" || x.Name == "IslandHills6")
                {
                    foreach (var block in blocks)
                    {
                        if (x != block && conversionInfo.TryGetValue(block.Name, out BlockConversion[] conversion))
                        {
                            if (conversion != null)
                            {
                                var variant = conversion.ElementAtOrDefault(block.Variant.Value);
                                if (variant != null)
                                {
                                    if (variant.Ground != null)
                                    {
                                        if (variant.Ground.Directions != null)
                                        {
                                            var direction = variant.Ground.Directions.ElementAtOrDefault((int)block.Direction);
                                            if (direction != null)
                                                foreach (var unit in direction.Units)
                                                    if (!RemoveGroundBlocks(block, x, variant.Ground, unit)) return false;
                                        }

                                        if (variant.Ground.Units != null)
                                            foreach (var unit in variant.Ground.Units)
                                                if (!RemoveGroundBlocks(block, x, variant.Ground, unit)) return false;
                                    }
                                    else if (variant.Air != null && variant.Air.OffsetYFromTerrain != null)
                                    {
                                        foreach (var unit in variant.Air.Units)
                                            if (!RemoveGroundBlocks(block, x, variant.Air, unit)) return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }).ToArray();

            Log.Write("All additional terrain blocks removed!");

            bool RemoveGroundBlocks(CGameCtnBlock block, CGameCtnBlock block2, BlockConversion variantGround, int[] unit)
            {
                if (unit.Length < 3)
                    throw new FormatException();

                Vec3 centerFromCoord = default;
                if (variantGround.CenterFromCoord != null)
                    if (variantGround.CenterFromCoord.Length >= 3)
                        centerFromCoord = new Vec3(variantGround.CenterFromCoord[0], variantGround.CenterFromCoord[1], variantGround.CenterFromCoord[2]);

                var radians = (((int)block.Direction + variantGround.OffsetDirection.GetValueOrDefault()) % 4) * ((float)Math.PI / 2);

                var unit2 = (Convert.ToInt32(Math.Cos(radians) * (unit[0] - centerFromCoord.X) -
                Math.Sin(radians) * (unit[2] - centerFromCoord.Z) + centerFromCoord.X),
                unit[1], // not supported yet
                Convert.ToInt32(Math.Sin(radians) * (unit[0] - centerFromCoord.X) +
                Math.Cos(radians) * (unit[2] - centerFromCoord.Z) + centerFromCoord.Z));

                if (block.Coord + unit2 - new Int3(0, variantGround.OffsetYFromTerrain.GetValueOrDefault(), 0) == block2.Coord)
                {
                    Log.Write($"Removing {block2.Name} away from {block.Name} to avoid Z-fighting.");
                    return false;
                }

                return true;
            }

            var offsetHeight = 12;

            Int3 offset = new Int3(32, 32, 32) - mapRange;
            offset = new Int3(
                Convert.ToInt32(offset.X / 2f) - 2,
                0,
                Convert.ToInt32(offset.Z / 2f) - 2);

            switch (size)
            {
                case MapSize.X31WithSmallBorder:
                    Log.Write("Centering the map...");

                    map.Size = (62, 64, 62);

                    foreach (var b in blocks)
                        b.Coord += offset - minCoord.XZ;

                    Log.Write("Adding the pool...");

                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), blocks.ToList(), -1, conversionInfo, 0);

                    map.Size = (
                        map.Size.GetValueOrDefault().X + 2,
                        map.Size.GetValueOrDefault().Y,
                        map.Size.GetValueOrDefault().Z + 2);

                    foreach (var b in map.Blocks)
                        b.Coord = new Int3(b.Coord.X + 1, b.Coord.Y, b.Coord.Z + 1);

                    break;
                case MapSize.X32WithBigBorder:
                    Log.Write("Centering the map...");

                    offsetHeight = 20;

                    map.Size = (64, 64, 64);

                    foreach (var b in blocks)
                        b.Coord += offset - minCoord.XZ;

                    Log.Write("Adding the pool...");

                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), blocks.ToList(), -1, conversionInfo, 0);

                    break;
                case MapSize.X45WithSmallBorder:
                    Log.Write("Adding the pool...");

                    map.Size = (92, 255, 92);
                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), map.Blocks, 0, conversionInfo, 1);

                    map.Size = (
                        map.Size.GetValueOrDefault().X + 2,
                        map.Size.GetValueOrDefault().Y,
                        map.Size.GetValueOrDefault().Z + 2);

                    foreach (var b in map.Blocks)
                        b.Coord = new Int3(b.Coord.X + 1, b.Coord.Y, b.Coord.Z + 1);
                    break;
            }

            if (cutoff)
            {
                switch (size)
                {
                    case MapSize.X31WithSmallBorder:
                        Log.Write("Cutting off blocks outside 31x31 from center...");
                        blocks = blocks.Where(x => x.Coord.X > -1 && x.Coord.X < 30 && x.Coord.Z > -1 && x.Coord.Z < 30).ToArray();
                        break;
                    case MapSize.X32WithBigBorder:
                        Log.Write("Cutting off blocks outside 32x32 from center...");
                        blocks = blocks.Where(x => x.Coord.X > -2 && x.Coord.X < 31 && x.Coord.Z > -2 && x.Coord.Z < 31).ToArray();
                        break;
                    case MapSize.X45WithSmallBorder:
                        Log.Write("No cutoff happening, not supported by 45x45 conversion.");
                        break;
                }
            }

            Log.Write("Fixing MediaTracker...");

            if (ignoreMediaTracker)
            {
                gbx.RemoveBodyChunk<CGameCtnChallenge.Chunk03043021>();
            }
            else
            {
                map.TransferMediaTrackerTo049(6);

                var xzCameraOffset = new Vec3();
                var xzTriggerOffset = new Int3();

                if (size == MapSize.X45WithSmallBorder)
                {
                    xzTriggerOffset = (6, -2, 6);
                }
                else
                {
                    xzCameraOffset = new Vec3((offset.X - minCoord.X) * 64, 0, (offset.Z - minCoord.Z) * 64);
                    xzTriggerOffset = new Int3((offset.X - minCoord.X + 1) * 6, -3, (offset.Z - minCoord.Z + 1) * 6);
                    if (size == MapSize.X31WithSmallBorder)
                        xzTriggerOffset += (0, 1, 0);
                }

                map.OffsetMediaTrackerTriggers(xzTriggerOffset);

                map.OffsetMediaTrackerCameras(new Vec3(64, -6 - offsetHeight, 64) + xzCameraOffset);
            }

            var chunk003 = gbx.Header.GetChunk<CGameCtnChallenge.Chunk03043003>();
            chunk003.Version = 6;

            gbx.CreateBodyChunk<CGameCtnChallenge.Chunk03043044>();

            map.ScriptMetadata.Declare("MapVehicle", carTranslations[beforeCar]);

            if (map.Mode == CGameCtnChallenge.PlayMode.Stunts)
            {
                map.Mode = CGameCtnChallenge.PlayMode.Script;

                var challParams = map.ChallengeParameters;

                var chunk00E = challParams.CreateChunk<CGameCtnChallengeParameters.Chunk0305B00E>();
                challParams.MapType = "Stunts";

                var authorScore = challParams.AuthorScore;
                var goldScore = challParams.GoldTime.GetValueOrDefault().ToMilliseconds();
                var silverScore = challParams.SilverTime.GetValueOrDefault().ToMilliseconds();
                var bronzeScore = challParams.BronzeTime.GetValueOrDefault().ToMilliseconds();
                var timeLimit = challParams.TimeLimit.ToMilliseconds();

                var mapStyle = $"{authorScore}|{goldScore}|{silverScore}|{bronzeScore}";
                challParams.MapStyle = mapStyle;

                map.ScriptMetadata.Declare("MapTimeLimit", timeLimit);
                map.ScriptMetadata.Declare("ObjectiveAuthor", authorScore);
                map.ScriptMetadata.Declare("ObjectiveGold", goldScore);
                map.ScriptMetadata.Declare("ObjectiveSilver", silverScore);
                map.ScriptMetadata.Declare("ObjectiveBronze", bronzeScore);

                map.ScriptMetadata.Declare("GameMode", "Stunts");
            }
            else
                map.ScriptMetadata.Declare("GameMode", "Race");

            map.ScriptMetadata.Declare("MadeByConverter", true);
            map.ScriptMetadata.Declare("RequiresOpenPlanet", size == MapSize.X45WithSmallBorder);
            map.ScriptMetadata.Declare("OriginalAuthorLogin", map.AuthorLogin);

            switch (size)
            {
                case MapSize.X32WithBigBorder:
                    Log.Write("Placing the background item...");
                    map.PlaceAnchoredObject(("Island\\8Terrain\\7BackGround\\Background.Item.gbx", 10003, "adamkooo"), new Vec3(), new Vec3(), new Vec3(1024, 9, 1024));
                    break;
                case MapSize.X45WithSmallBorder:
                    Log.Write("Placing the background item...");
                    map.PlaceAnchoredObject(("Island\\8Terrain\\6BigBackground\\Background_45x45.Item.gbx", 10003, "adamkooo"), new Vec3(), new Vec3(), new Vec3(1504, 17, 1504));
                    break;
                default:
                    Log.Write($"Island background is not supported for {size}.", ConsoleColor.Yellow);
                    break;
            }

            var placed = 0;
            var faultyBlocks = new List<CGameCtnBlock>();

            Log.Write("Starting the conversion!");

            foreach (var block in blocks)
            {
                if (block.IsClip) continue;

                if (conversionInfo.TryGetValue(block.Name, out BlockConversion[] conversion))
                {
                    if (conversion != null)
                    {
                        var variant = conversion.ElementAtOrDefault(block.Variant.Value);

                        if (variant != null)
                        {
                            if (variant.ItemModel == null)
                            {
                                if (block.IsGround)
                                {
                                    if (variant.Ground != null && variant.Ground.ItemModels != null)
                                    {
                                        foreach (var item in variant.Ground.ItemModels)
                                            if (item != null)
                                                DoConversion(item, true);
                                    }
                                    else if (variant.Ground != null)
                                        DoConversion(variant.Ground, true);
                                    else
                                    {
                                        Log.Write($"Missing ground variant of {block.Name}! (Variant {block.Variant})", ConsoleColor.Yellow);
                                        faultyBlocks.Add(block);
                                    }
                                }
                                else
                                {
                                    if (variant.Air != null && variant.Air.ItemModels != null)
                                    {
                                        foreach (var item in variant.Air.ItemModels)
                                            if (item != null)
                                                DoConversion(item, false);
                                    }
                                    else if (variant.Air != null)
                                        DoConversion(variant.Air, false);
                                    else
                                    {
                                        Log.Write($"Missing air variant of {block.Name}! (Variant {block.Variant})", ConsoleColor.Yellow);
                                        faultyBlocks.Add(block);
                                    }
                                }
                            }
                            else
                                DoConversion(variant);

                            void DoConversion(BlockConversion conv, bool? isItemGround = null)
                            {
                                if (conv.ItemModel.Length > 0 && !string.IsNullOrEmpty(conv.ItemModel[0]))
                                {
                                    var modelToChoose = 0;
                                    if (conv.Clip != null)
                                    {

                                    }
                                    else
                                        modelToChoose = randomizer.Next(0, conv.ItemModel.Length);

                                    var itemModel = conv.ItemModel[modelToChoose];
                                    var itemModelSplit = itemModel.Split(' ');

                                    var meta = new Ident("Island\\" + itemModelSplit[0], itemModelSplit.Length > 1 ? new Collection(itemModelSplit[1]) : 10003, itemModelSplit.Length > 2 ? itemModelSplit[2] : "adamkooo");

                                    Vec3 offsetAbsolutePosition = new Vec3((block.Coord.X+1) * 64 + 96, (block.Coord.Y+1) * 8 - offsetHeight, (block.Coord.Z+1) * 64 + 96);
                                    if (conv.OffsetAbsolutePosition != null)
                                    {
                                        if (conv.OffsetAbsolutePosition.Length >= 3)
                                            offsetAbsolutePosition += new Vec3(
                                                conv.OffsetAbsolutePosition[0],
                                                conv.OffsetAbsolutePosition[1],
                                                conv.OffsetAbsolutePosition[2]);
                                        else if (conv.OffsetAbsolutePosition.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetAbsolutePosition: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conv.OffsetAbsolutePosition)}]");
                                    }

                                    Vec3 offsetPivot = new Vec3(0, 2, 0);
                                    if (conv.OffsetPivot != null)
                                    {
                                        if (conv.OffsetPivot.Length >= 3)
                                            offsetPivot += new Vec3(
                                                conv.OffsetPivot[0],
                                                conv.OffsetPivot[1],
                                                conv.OffsetPivot[2]);
                                        else if (conv.OffsetPivot.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetPivot: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conv.OffsetPivot)}]");
                                    }

                                    var offsetDirection = 0;
                                    if (conv.OffsetDirection.HasValue)
                                    {
                                        if (conv.OffsetDirection.Value >= 0 && conv.OffsetDirection.Value < 4)
                                            offsetDirection = conv.OffsetDirection.Value;
                                        else
                                            throw new FormatException($"Wrong format of OffsetDirection: {block.Name} -> index {block.Variant} -> {conv.OffsetDirection}");
                                    }

                                    var dir = 3 - (((int)block.Direction + offsetDirection) % 4);
                                    if (conv.InverseDirection.GetValueOrDefault() == true)
                                        dir = 3 - dir;

                                    if (conv.Directions != null)
                                    {
                                        for (var i = 0; i < conv.Directions.Length; i++)
                                        {
                                            var direction = conv.Directions[i];
                                            if (direction != null)
                                            {
                                                if (i == dir && direction.OffsetAbsolutePosition != null && direction.OffsetAbsolutePosition.Length >= 3)
                                                    offsetAbsolutePosition += new Vec3(
                                                        direction.OffsetAbsolutePosition[0],
                                                        direction.OffsetAbsolutePosition[1],
                                                        direction.OffsetAbsolutePosition[2]);
                                                if (i == dir && direction.OffsetPivot != null && direction.OffsetPivot.Length >= 3)
                                                    offsetPivot += new Vec3(
                                                        direction.OffsetPivot[0],
                                                        direction.OffsetPivot[1],
                                                        direction.OffsetPivot[2]);
                                            }
                                        }
                                    }

                                    Vec3 offsetPitchYawRoll = new Vec3(dir * 90f / 180 * (float)Math.PI, 0, 0);
                                    if (conv.OffsetPitchYawRoll != null)
                                    {
                                        if (conv.OffsetPitchYawRoll.Length >= 3)
                                            offsetPitchYawRoll = new Vec3(
                                                conv.OffsetPitchYawRoll[0] / 180 * (float)Math.PI,
                                                conv.OffsetPitchYawRoll[1] / 180 * (float)Math.PI,
                                                conv.OffsetPitchYawRoll[2] / 180 * (float)Math.PI);
                                        else if (conv.OffsetPitchYawRoll.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetPitchYawRoll: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conv.OffsetPitchYawRoll)}]");
                                    }

                                    map.PlaceAnchoredObject(meta, offsetAbsolutePosition, offsetPitchYawRoll, offsetPivot);

                                    Vec3 skinPosOffset = default;
                                    if (conv.SkinPositionOffset != null)
                                        if (conv.SkinPositionOffset.Length >= 3)
                                            skinPosOffset = new Vec3(conv.SkinPositionOffset[0], conv.SkinPositionOffset[1], conv.SkinPositionOffset[2]);

                                    if (conv.SkinSignSet != null && block.Skin != null && block.Skin != null && block.Skin.PackDesc != null)
                                    {
                                        if (skinInfo.TryGetValue(conv.SkinSignSet, out Dictionary<string, string> skinDic))
                                        {
                                            if (skinDic.TryGetValue(block.Skin.PackDesc.FilePath, out string itemFile))
                                                map.PlaceAnchoredObject(new Ident("Island\\" + itemFile, "10003", "adamkooo"),
                                                    offsetAbsolutePosition + skinPosOffset,
                                                    offsetPitchYawRoll + new Vec3(-conv.SkinDirectionOffset % 4 * 90f / 180 * (float)Math.PI, 0, 0), offsetPivot);
                                            else
                                            {
                                                Log.Write($"Could not find item alternative for {block.Skin.PackDesc.FilePath}. Default sign will be used instead.", ConsoleColor.DarkYellow);
                                            }
                                        }
                                    }

                                    Log.Write($"{block.Name} ({(block.IsGround ? "Ground" : "Air")}) -> {meta.ID} ({(isItemGround.HasValue ? (isItemGround.Value ? "Ground" : "Air") : "Undefined")})");

                                    placed++;
                                }
                                else
                                {
                                    Log.Write($"Missing item model definition! ({block.Name} variant {block.Variant})", ConsoleColor.DarkYellow);
                                    faultyBlocks.Add(block);
                                }
                            }
                        }
                        else
                        {
                            Log.Write($"Variant {block.Variant} not available for {block.Name}!", ConsoleColor.Yellow);
                            faultyBlocks.Add(block);
                        }
                    }
                    else
                    {
                        Log.Write($"No info specified at {block.Name}!", ConsoleColor.Red);
                        faultyBlocks.Add(block);
                    }
                }
                else
                {
                    Log.Write($"Item not available for {block.Name}! ({(block.IsGround ? "Ground" : "Air")})", ConsoleColor.Yellow);
                    faultyBlocks.Add(block);
                }
            }

            var finishConversion = DateTime.Now - startConvert;

            Log.Write();

            if(mapLoadTime.HasValue)
                Log.Write($"Conversion completed in {finishConversion.TotalSeconds} seconds. ({(mapLoadTime.Value + finishConversion).TotalSeconds} seconds including map load)");
            else
                Log.Write($"Conversion completed in {finishConversion.TotalSeconds} seconds.");

            Log.Write();
            Log.Write("Faulty conversions: ");

            if (faultyBlocks.Count == 0)
                Log.Write("- None!", ConsoleColor.Green);

            foreach (var b in faultyBlocks.GroupBy(x => new { x.Name, x.Variant, x.IsGround }).Select(y => y.First()))
                Log.Write($"- {b.Name}, variant {b.Variant}, ({(b.IsGround ? "Ground" : "Air")})", ConsoleColor.Yellow);

            Log.Write();

            Log.Write($"Saving map to {outputFileBase}");

            var startMapSave = DateTime.Now;

            Directory.CreateDirectory(outputFolder);
            gbx.Save(outputFileBase);

            Log.Write($"Saving log to {logFileBase}.log");
            
            Log.Write($"Map saved! (in {(DateTime.Now - startMapSave).TotalMilliseconds}ms)");

            Directory.CreateDirectory("log");
            Log.End(logFileBase + ".log");
        }
    }
}
