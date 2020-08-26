using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
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

            GameBox<CGameCtnChallenge> gbx = new GameBox<CGameCtnChallenge>();

            var startMapLoad = DateTime.Now;

            if (!gbx.Load(fileName))
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

        internal static Int3 DefineMapRange(Block[] blocks, out Int3? minCoord)
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

        internal static List<Block> CreateWaterBlocks(Int3 mapSize, List<Block> islandBlocks, byte yOffset)
        {
            var islandBlockDictionary = islandBlocks.Where(x => x.Name == "IslandGrass" || x.Name == "IslandHills6").ToDictionary(x => new Int3(x.Coord.X, 0, x.Coord.Z));

            var blocks = new List<Block>();

            for (byte x = 1; x <= mapSize.X; x++)
            {
                for (byte z = 1; z <= mapSize.Z; z++)
                {
                    if (x == 1 || z == 1 || x == mapSize.X || z == mapSize.Z)
                    {
                        var flag = 135168;
                        var dir = Direction.North;

                        if (x == 1 && z == 1)
                        {
                            flag = 135177;
                            dir = Direction.East;
                        }
                        else if (x == 1 && z == mapSize.Z)
                        {
                            flag = 135177;
                        }
                        else if (x == mapSize.X && z == 1)
                        {
                            flag = 135177;
                            dir = Direction.South;
                        }
                        else if (x == mapSize.X && z == mapSize.Z)
                        {
                            flag = 135177;
                            dir = Direction.West;
                        }
                        else if (x == 1)
                        {
                            flag = 135173;
                            dir = Direction.East;
                        }
                        else if (z == 1)
                        {
                            flag = 135173;
                            dir = Direction.South;
                        }
                        else if (x == mapSize.X)
                        {
                            flag = 135173;
                            dir = Direction.West;
                        }
                        else if (z == mapSize.Z)
                        {
                            flag = 135173;
                        }

                        blocks.Add(new Block("StadiumPool2", dir, (x, yOffset, z), flag, null, null, null));
                    }
                    else if (islandBlockDictionary.TryGetValue(new Int3(Convert.ToInt32(MathF.Floor(x / 2f) - 1), 0, Convert.ToInt32(MathF.Floor(z / 2f) - 1)), out Block bl))
                    {
                        blocks.Add(new Block("RemoveGrass.Block.Gbx_CustomBlock", Direction.North, (x, yOffset, z), 135168, null, null, null));
                    }
                    else
                    {
                        blocks.Add(new Block("StadiumWater2", Direction.North, (x, yOffset, z), 135168, null, null, null));
                    }

                    blocks.Add(new Block("Unassigned1", Direction.East, (0, 0, 0), -1, null, null, null));
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
            map.DecorationName = "64x64" + map.DecorationName.TrimEnd();

            Log.Write("Converting environment...");
            map.Collection = "Stadium";

            Log.Write("Setting player model...");
            map.PlayerModelID = "IslandCar.Item.Gbx";
            map.GetChunk<CGameCtnChallenge.Chunk00D>().Vehicle.Collection = "Stadium";
            map.GetChunk<CGameCtnChallenge.Chunk00D>().Vehicle.Author = "adamkooo";

            Log.Write("Applying texture mod...");
            map.ModPackDesc = new FileRef(3, Convert.FromBase64String("AgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="), @"Skins\Stadium\Mod\IslandTM2U.zip", "");

            Bitmap thumbnail;
            if (map.Thumbnail == null)
                thumbnail = new Bitmap(512, 512);
            else
                thumbnail = new Bitmap(map.Thumbnail, 512, 512);

            using (var g = Graphics.FromImage(thumbnail))
            {
                g.DrawImage(Resources.Overlay, 0, 0);
                if (size == MapSize.X45WithSmallBorder)
                    g.DrawImage(Resources.OverlayOpenplanet, 0, 0);
            }

            map.Thumbnail = thumbnail;

            map.GetChunk<CGameCtnChallenge.Chunk01F>().Version = 6;

            if (size == MapSize.X32WithBigBorder)
            {
                Log.Write("Importing chunk 0x03043043 for water on ground...");
                var chunk = gbx.Body.MainNode.CreateChunk<CGameCtnChallenge.Chunk043>(File.ReadAllBytes("0x03043043.dat"));
            }

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
                                var variant = conversion.ElementAtOrDefault(block.Variant);
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

            static bool RemoveGroundBlocks(Block block, Block block2, BlockConversion variantGround, int[] unit)
            {
                if (unit.Length < 3)
                    throw new FormatException();

                Vector3 centerFromCoord = default;
                if (variantGround.CenterFromCoord != null)
                    if (variantGround.CenterFromCoord.Length >= 3)
                        centerFromCoord = new Vector3(variantGround.CenterFromCoord[0], variantGround.CenterFromCoord[1], variantGround.CenterFromCoord[2]);

                var radians = (((int)block.Direction + variantGround.OffsetDirection.GetValueOrDefault()) % 4) * (MathF.PI / 2);

                var unit2 = (Convert.ToInt32(MathF.Cos(radians) * (unit[0] - centerFromCoord.X) -
                MathF.Sin(radians) * (unit[2] - centerFromCoord.Z) + centerFromCoord.X),
                unit[1], // not supported yet
                Convert.ToInt32(MathF.Sin(radians) * (unit[0] - centerFromCoord.X) +
                MathF.Cos(radians) * (unit[2] - centerFromCoord.Z) + centerFromCoord.Z));

                if (block.Name == "IslandHills6DecoRock" && block2.Name == "IslandHills6")
                {

                }

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
                Convert.ToInt32(offset.X / 2f) - 1,
                0,
                Convert.ToInt32(offset.Z / 2f) - 1);

            switch (size)
            {
                case MapSize.X31WithSmallBorder:
                    Log.Write("Centering the map...");

                    map.Size = (62, 64, 62);

                    foreach (var b in blocks)
                        b.Coord += offset - minCoord.XZ;

                    Log.Write("Adding the pool...");

                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), blocks.ToList(), 1);

                    map.Size = (
                        map.Size.GetValueOrDefault().X + 2,
                        map.Size.GetValueOrDefault().Y,
                        map.Size.GetValueOrDefault().Z + 2);

                    foreach (var b in map.Blocks)
                        b.Coord = new Int3(b.Coord.X + 1, b.Coord.Y, b.Coord.Z + 1);

                    break;
                case MapSize.X32WithBigBorder:
                    Log.Write("Centering the map...");

                    offsetHeight = 16;

                    map.Size = (64, 64, 64);

                    foreach (var b in blocks)
                        b.Coord += offset - minCoord.XZ;

                    Log.Write("Adding the pool...");

                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), blocks.ToList(), 0);

                    break;
                case MapSize.X45WithSmallBorder:
                    Log.Write("Adding the pool...");

                    map.Size = (92, 255, 92);
                    map.Blocks = CreateWaterBlocks(map.Size.GetValueOrDefault(), map.Blocks, 1);

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
                gbx.RemoveBodyChunk<CGameCtnChallenge.Chunk021>();
            }
            else
            { 
                gbx.Body.MainNode.TransferMediaTrackerTo049(6);

                var xzCameraOffset = new Vector3();
                var xzTriggerOffset = new Int3();

                if (size == MapSize.X45WithSmallBorder)
                {
                    xzTriggerOffset = (6, -2, 6);
                }
                else
                {
                    xzCameraOffset = new Vector3(offset.X * 64 - 128, offset.Y * 64, offset.Z * 64 - 192);
                    xzTriggerOffset = (6 + offset.X * 6, -3, 6 + offset.X * 6);
                    if (size == MapSize.X31WithSmallBorder)
                        xzTriggerOffset += (0, 1, 0);
                }

                gbx.Body.MainNode.OffsetMediaTrackerTriggers(xzTriggerOffset);

                gbx.Body.MainNode.OffsetMediaTrackerCameras(new Vector3(64, -6 - offsetHeight, 64) + xzCameraOffset);
            }

            var chunk003 = map.GetChunk<CGameCtnChallenge.Chunk003>();
            chunk003.Version = 6;

            gbx.CreateBodyChunk<CGameCtnChallenge.Chunk044>();
            map = gbx.MainNode; // Due to current solution this must be presented

            map.MetadataTraits.Declare("MapVehicle", "IslandCar");

            if (map.Type == CGameCtnChallenge.TrackType.Stunts)
            {
                map.MapType = "Stunts";
                map.Type = CGameCtnChallenge.TrackType.Script;

                var challParams = map.ChallengeParameters;

                var chunk00E = challParams.CreateChunk<CGameCtnChallengeParameters.Chunk00E>();
                chunk00E.MapType = "Stunts";

                var authorScore = challParams.AuthorScore.GetValueOrDefault();
                var goldScore = challParams.GoldTime.GetValueOrDefault().ToMilliseconds();
                var silverScore = challParams.SilverTime.GetValueOrDefault().ToMilliseconds();
                var bronzeScore = challParams.BronzeTime.GetValueOrDefault().ToMilliseconds();
                var timeLimit = challParams.TimeLimit.GetValueOrDefault().ToMilliseconds();

                var mapStyle = $"{authorScore}|{goldScore}|{silverScore}|{bronzeScore}";
                map.MapStyle = mapStyle;
                chunk00E.MapStyle = mapStyle;

                map.MetadataTraits.Declare("MapTimeLimit", timeLimit);
                map.MetadataTraits.Declare("ObjectiveAuthor", authorScore);
                map.MetadataTraits.Declare("ObjectiveGold", goldScore);
                map.MetadataTraits.Declare("ObjectiveSilver", silverScore);
                map.MetadataTraits.Declare("ObjectiveBronze", bronzeScore);

                map.MetadataTraits.Declare("GameMode", "Stunts");
            }
            else
                map.MetadataTraits.Declare("GameMode", "Race");

            map.MetadataTraits.Declare("MadeByConverter", true);
            map.MetadataTraits.Declare("RequiresOpenPlanet", size == MapSize.X45WithSmallBorder);

            switch (size)
            {
                case MapSize.X32WithBigBorder:
                    Log.Write("Placing the background item...");
                    gbx.Body.MainNode.PlaceItem(("Island\\8Terrain\\7BackGround\\Background.Item.gbx", "10003", "adamkooo"), new Vector3(), new Vector3(), new Byte3(), new Vector3(1024, 9, 1024));
                    break;
                case MapSize.X45WithSmallBorder:
                    Log.Write("Placing the background item...");
                    gbx.Body.MainNode.PlaceItem(("Island\\8Terrain\\6BigBackground\\Background_45x45.Item.gbx", "10003", "adamkooo"), new Vector3(), new Vector3(), new Byte3(), new Vector3(1504, 17, 1504));
                    break;
                default:
                    Log.Write($"Island background is not supported for {size}.", ConsoleColor.Yellow);
                    break;
            }

            var placed = 0;
            var faultyBlocks = new List<Block>();

            Log.Write("Starting the conversion!");

            foreach (var block in blocks)
            {
                if (block.IsClip) continue;

                if (conversionInfo.TryGetValue(block.Name, out BlockConversion[] conversion))
                {
                    if (conversion != null)
                    {
                        var variant = conversion.ElementAtOrDefault(block.Variant);

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

                            void DoConversion(BlockConversion conversion, bool? isItemGround = null)
                            {
                                if (conversion.ItemModel.Length > 0 && !string.IsNullOrEmpty(conversion.ItemModel[0]))
                                {
                                    var modelToChoose = 0;
                                    if (conversion.Clip != null)
                                    {

                                    }
                                    else
                                        modelToChoose = randomizer.Next(0, conversion.ItemModel.Length);

                                    var itemModel = conversion.ItemModel[modelToChoose];
                                    var itemModelSplit = itemModel.Split(' ');

                                    var meta = new Meta("Island\\" + itemModelSplit[0], itemModelSplit.Length > 1 ? itemModelSplit[1] : "10003", itemModelSplit.Length > 2 ? itemModelSplit[2] : "adamkooo");

                                    Vector3 offsetAbsolutePosition = new Vector3(block.Coord.X * 64 + 96, block.Coord.Y * 8 - offsetHeight, block.Coord.Z * 64 + 96);
                                    if (conversion.OffsetAbsolutePosition != null)
                                    {
                                        if (conversion.OffsetAbsolutePosition.Length >= 3)
                                            offsetAbsolutePosition += new Vector3(
                                                conversion.OffsetAbsolutePosition[0],
                                                conversion.OffsetAbsolutePosition[1],
                                                conversion.OffsetAbsolutePosition[2]);
                                        else if (conversion.OffsetAbsolutePosition.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetAbsolutePosition: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conversion.OffsetAbsolutePosition)}]");
                                    }

                                    Vector3 offsetPivot = new Vector3(0, 2, 0);
                                    if (conversion.OffsetPivot != null)
                                    {
                                        if (conversion.OffsetPivot.Length >= 3)
                                            offsetPivot += new Vector3(
                                                conversion.OffsetPivot[0],
                                                conversion.OffsetPivot[1],
                                                conversion.OffsetPivot[2]);
                                        else if (conversion.OffsetPivot.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetPivot: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conversion.OffsetPivot)}]");
                                    }

                                    var offsetDirection = 0;
                                    if (conversion.OffsetDirection.HasValue)
                                    {
                                        if (conversion.OffsetDirection.Value >= 0 && conversion.OffsetDirection.Value < 4)
                                            offsetDirection = conversion.OffsetDirection.Value;
                                        else
                                            throw new FormatException($"Wrong format of OffsetDirection: {block.Name} -> index {block.Variant} -> {conversion.OffsetDirection}");
                                    }

                                    var dir = 3 - (((int)block.Direction + offsetDirection) % 4);
                                    if (conversion.InverseDirection.GetValueOrDefault() == true)
                                        dir = 3 - dir;

                                    if (conversion.Directions != null)
                                    {
                                        for (var i = 0; i < conversion.Directions.Length; i++)
                                        {
                                            var direction = conversion.Directions[i];
                                            if (direction != null)
                                            {
                                                if (i == dir && direction.OffsetAbsolutePosition != null && direction.OffsetAbsolutePosition.Length >= 3)
                                                    offsetAbsolutePosition += new Vector3(
                                                        direction.OffsetAbsolutePosition[0],
                                                        direction.OffsetAbsolutePosition[1],
                                                        direction.OffsetAbsolutePosition[2]);
                                                if (i == dir && direction.OffsetPivot != null && direction.OffsetPivot.Length >= 3)
                                                    offsetPivot += new Vector3(
                                                        direction.OffsetPivot[0],
                                                        direction.OffsetPivot[1],
                                                        direction.OffsetPivot[2]);
                                            }
                                        }
                                    }

                                    Vector3 offsetPitchYawRoll = new Vector3(dir * 90f / 180 * MathF.PI, 0, 0);
                                    if (conversion.OffsetPitchYawRoll != null)
                                    {
                                        if (conversion.OffsetPitchYawRoll.Length >= 3)
                                            offsetPitchYawRoll = new Vector3(
                                                conversion.OffsetPitchYawRoll[0] / 180 * MathF.PI,
                                                conversion.OffsetPitchYawRoll[1] / 180 * MathF.PI,
                                                conversion.OffsetPitchYawRoll[2] / 180 * MathF.PI);
                                        else if (conversion.OffsetPitchYawRoll.Length != 0)
                                            throw new FormatException($"Wrong format of OffsetPitchYawRoll: {block.Name} -> index {block.Variant} -> [{string.Join(", ", conversion.OffsetPitchYawRoll)}]");
                                    }

                                    gbx.Body.MainNode.PlaceItem(meta, offsetAbsolutePosition, offsetPitchYawRoll, (0, 0, 0), offsetPivot);

                                    Vector3 skinPosOffset = default;
                                    if (conversion.SkinPositionOffset != null)
                                        if (conversion.SkinPositionOffset.Length >= 3)
                                            skinPosOffset = new Vector3(conversion.SkinPositionOffset[0], conversion.SkinPositionOffset[1], conversion.SkinPositionOffset[2]);

                                    if (conversion.SkinSignSet != null && block.Skin != null && block.Skin != null && block.Skin.PackDesc != null)
                                    {
                                        if (skinInfo.TryGetValue(conversion.SkinSignSet, out Dictionary<string, string> skinDic))
                                        {
                                            if (skinDic.TryGetValue(block.Skin.PackDesc.FilePath, out string itemFile))
                                                gbx.Body.MainNode.PlaceItem(new Meta("Island\\" + itemFile, "10003", "adamkooo"),
                                                    offsetAbsolutePosition + skinPosOffset,
                                                    offsetPitchYawRoll + new Vector3(-conversion.SkinDirectionOffset % 4 * 90f / 180 * MathF.PI, 0, 0), (0, 0, 0), offsetPivot);
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
