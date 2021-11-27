using System;
using System.Linq;
using GBX.NET;
using GBX.NET.PAK;
using GBX.NET.Engines.Game;
using System.Collections.Generic;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.OnLogEvent += Log_OnLogEvent;

            //var mem = GC.GetTotalMemory(false);

            var r = GameBox.ParseNode(@"E:\ManiaPlanetUserData\Replays\$fff$sD07_riolu_(0'50''23).Replay.Gbx");
            
            //var replay = GameBox.ParseNode(@"E:\Games\TrackMania\GameData\Replays\RaceA1.Replay.Gbx");
            //var gbx = GameBox.ParseNode(@"C:\Users\petrp\Openplanet4\Extract\GameData\Stadium\Media\Solid\Inflatable\Cactus.Solid.Gbx");
            
            /*gbx.AnchoredObjects.First(x => x.ItemModel.ID == "Screen2x1").Scale = 1.5f;

            gbx.Save();

            //var gbx2 = GameBox.Parse(@"C:\Users\petrp\Documents\TrackMania\Tracks\Replays\A03-Race_eL33T(00'18''15).Replay.Gbx");

            while (true)
            {
                var bruh = GameBox.Parse<CGameCtnChallenge>(@"D:\GBX\gbx-net\GBX.NET.Tests\Files\Summer 2021 - 25.Map.Gbx");
                bruh.Node.AnchoredObjects.Any();
            }
            var cars = new string[] { "BayCar", "SportCar", "CoastCar" };
            foreach (var fileName in Directory.GetFiles(@"E:\Downloads\TMSBases"))
            {
                var node = GameBox.ParseNode<CGameCtnChallenge>(fileName);
                var originaMapName = node.MapName;
                foreach (var car in cars)
                {
                    node.PlayerModel = new Ident(car, "Vehicles", node.PlayerModel.Author);
                    node.MapName = originaMapName + "-" + car;
                    node.Save(@$"E:\Downloads\TMSBases\{node.MapName}.Challenge.Gbx", IDRemap.TrackMania2006);
                }
            }*/

            //var meme = GC.GetTotalMemory(false) - mem;

            //var node = GameBox.ParseNode(@"E:\Downloads\TMO_Puzzle_A5_TASowanie000189.Replay.Gbx");
            var pakList = NadeoPakList.Parse(@"E:\Games\TmUnitedForever\Packs\packlist.dat");
            foreach (var item in pakList)
            {
                Console.WriteLine(item.Name + ": " + BitConverter.ToString(item.Key).Replace("-", ""));
            }
            
            using (var pak = NadeoPak.Parse(@"E:\Games\TmUnitedForever\Packs\Island.pak", pakList.First(x => x.Name == "island").Key))
            {
                //var solid = pak.Folders[4].Folders[0].Folders[1].Files[15];


                var list = new List<NadeoPakFile>();

                foreach (var file in pak.GetFiles())
                {
                    if(file.Data == null)
                    {

                    }
                    /*if (file.Data != null)
                    {
                        var fullFileName = Path.Combine("IslandPak", file.GetFullFileName());
                        var dir = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(dir);

                        if (file.IsHashed)
                        {
                            var fileName = $"{file.GetClassNameWithoutNamespace()}_{file.Name}";
                            if (NodeCacheManager.Extensions.TryGetValue(file.ClassID, out string ext))
                                fileName += ext;
                            else
                                fileName += ".Gbx";
                            File.WriteAllBytes(Path.Combine(dir, fileName), file.Data);
                        }
                        else
                            File.WriteAllBytes(fullFileName, file.Data);
                    }*/
                }

                /*Directory.CreateDirectory("solids");

                foreach(var gbx in list)
                {
                    File.WriteAllBytes(Path.Combine("solids", gbx.Name + ".Solid.Gbx"), gbx.Data);
                }*/
            }
        }

        private static void Log_OnLogEvent(string text, ConsoleColor color)
        {
            Console.WriteLine(text);
        }
    }
}
