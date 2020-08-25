using System;
using GBX.NET;
using GBX.NET.Engines.Game;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace IslandConverter
{
    public enum MapSize
    {
        X31WithSmallBorder,
        X32WithBigBorder,
        X45WithSmallBorder
    }

    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        [STAThread]
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            bool wizard = false;
            int? seed = default;
            MapSize? size = default;
            bool? cutoff = default;
            bool? noask = default;
            bool? ignoreMediaTracker = default;

            string[] fileNames = new string[0];

            if (args.Length == 0)
            {
                Console.Write("Starting the Island Converter! ");

                Thread.Sleep(500);

                FreeConsole();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ConverterForm());

                return;
            }
            else
            {
                Log.OnLogEvent += LogConsoleMode;

                Console.Title = $"Island Converter {version}";

                if (args.Length == 1 && (args[0] == "-console" || args[0] == "-wizard"))
                {
                    wizard = true;

                    Console.WriteLine($"Island Converter {version} WIZARD");
                    Console.WriteLine();
                    Console.Write("Please select the map you want to convert to TM2 Island.");

                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "GBX (*.Gbx)|*.Gbx|All files (*.*)|*.*";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine(ofd.FileName);
                        Console.Write($"Are you sure you want to convert this file? (Y/N) ");
                        if (Console.ReadKey().Key != ConsoleKey.Y)
                            return;
                        fileNames = new string[] { ofd.FileName };
                    }
                    else return;
                }
                else
                {
                    var arguments = args.Skip(1);

                    Console.WriteLine();

                    if (arguments.Count() > 0)
                    {
                        Console.WriteLine($"Specified optional arguments: {string.Join(' ', arguments)}");

                        var enumerator = arguments.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            switch (enumerator.Current)
                            {
                                case "-seed":
                                    if (enumerator.MoveNext())
                                    {
                                        var seedParam = enumerator.Current;
                                        if (int.TryParse(seedParam, out int num))
                                            seed = num;
                                        else seed = seedParam.GetHashCode();

                                        Console.WriteLine($"- Seed: {seedParam} {(Convert.ToInt32(seedParam) != seed ? $"({seed})" : "")}");
                                    }
                                    break;
                                case "-size":
                                    if (enumerator.MoveNext())
                                    {
                                        var sizeParam = enumerator.Current;
                                        switch (sizeParam)
                                        {
                                            case "x31":
                                                size = MapSize.X31WithSmallBorder;
                                                break;
                                            case "x32":
                                                size = MapSize.X32WithBigBorder;
                                                break;
                                            case "x45":
                                                size = MapSize.X45WithSmallBorder;
                                                break;
                                        }

                                        Console.WriteLine($"- Size: {size}");
                                    }
                                    break;
                                case "-cutoff":
                                    cutoff = true;
                                    Console.WriteLine($"- Cutoff: {cutoff}");
                                    break;
                                case "-noask":
                                    noask = true;
                                    Console.WriteLine($"- No ask: {noask}");
                                    break;
                                case "-ignoremt":
                                    ignoreMediaTracker = true;
                                    Console.WriteLine($"- Ignore MediaTracker: {ignoreMediaTracker}");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No specified optional arguments.");
                    }

                    if (!seed.HasValue)
                    {
                        seed = Environment.TickCount;
                        Console.WriteLine($"- Seed: {seed}");
                    }

                    if (!size.HasValue)
                    {
                        size = MapSize.X45WithSmallBorder;
                        Console.Write($"- Size: {size} ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("(will require OpenPlanet)");
                        Console.ResetColor();
                    }

                    if (!cutoff.HasValue)
                    {
                        cutoff = false;
                        Console.WriteLine($"- Cutoff: {cutoff}");
                    }

                    if (!noask.HasValue)
                    {
                        noask = false;
                        Console.WriteLine($"- No ask: {noask}");
                    }

                    if (!ignoreMediaTracker.HasValue)
                    {
                        ignoreMediaTracker = false;
                        Console.WriteLine($"- Ignore MediaTracker: {ignoreMediaTracker}");
                    }

                    Console.WriteLine();

                    var fileOrDirectory = args[0];

                    if (File.Exists(fileOrDirectory))
                    {
                        fileNames = new string[] { fileOrDirectory };
                        if (!noask.Value) Console.Write($"Are you sure you want to convert {fileOrDirectory}? (Y/N) ");
                    }
                    else if (Directory.Exists(fileOrDirectory))
                    {
                        fileNames = Directory.GetFiles(fileOrDirectory, "*", SearchOption.AllDirectories);
                        if (!noask.Value) Console.Write($"Are you sure you want to convert everything in the directory {fileOrDirectory}? (Y/N) ");
                    }
                    else
                    {
                        Console.Write("File or directory not found.");
                        Console.ReadKey();

                        return;
                    }

                    if (!noask.Value && Console.ReadKey().Key != ConsoleKey.Y)
                        return;
                }
            }

            Console.Clear();

            Random random = new Random(seed.GetValueOrDefault());

            foreach (var f in fileNames)
            {
                Log.Write(f);

                var gbx = IslandConverter.LoadGBX(f, out TimeSpan? finishMapLoad);
                if (gbx == null) continue;

                var map = gbx.MainNode;

                var blocks = map.Blocks.ToArray();

                Int3 mapRange = IslandConverter.DefineMapRange(blocks, out Int3? minCoord);

                if (wizard)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Selected valid map:\n{Path.GetFileName(f)} (loaded in {finishMapLoad.Value.TotalMilliseconds}ms)\n");

                        Console.Write($"- Name: ");
                        Console.WriteLine(Formatter.Deformat(map.MapName));

                        Console.WriteLine($"- UID: {map.MapUid}");
                        Console.WriteLine($"- Author: {map.AuthorLogin}");

                        Console.Write($"- Size: {map.Size} ");
                        if (map.Size == (45, 36, 45)) Console.WriteLine("(made in TMUF)");
                        else if (map.Size == (36, 36, 36)) Console.WriteLine("(made in Sunrise or TMU)");
                        else Console.WriteLine("(made in China)");

                        Console.WriteLine($"- Decoration: {map.DecorationName}");

                        Console.WriteLine($"- Medals:");
                        Console.WriteLine($" - Author: {map.AuthorTime}");
                        Console.WriteLine($" - Gold: {map.GoldTime}");
                        Console.WriteLine($" - Silver: {map.SilverTime}");
                        Console.WriteLine($" - Bronze: {map.BronzeTime}");

                        Console.WriteLine($"- Map type: {map.Type}");

                        Console.Write($"- Block range: ");

                        if (mapRange.X <= 32 && mapRange.Z <= 32)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(mapRange);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(mapRange);
                        }

                        Console.WriteLine();
                        if (mapRange.X < 32 && mapRange.Z < 32)
                            Console.WriteLine("This map fits the default Stadium64x64 base! You can choose your map base size just fine without issues:");
                        else
                            Console.WriteLine("This map is bigger than the default Stadium64x64 base! Choose one of the 3 options below:");

                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("  1) 31x31 area with small border and no Island background, doesn't require OpenPlanet to work");
                        Console.Write("  2) 32x32 area with normal border with Island background, doesn't require OpenPlanet to work ");
                        if (mapRange.X < 32 && mapRange.Z < 32)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("(recommended)");
                            Console.ResetColor();
                        }
                        else Console.WriteLine();
                        Console.Write("  3) 45x45 area with small border with Island background, ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("requires OpenPlanet to work ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (mapRange.X >= 32 && mapRange.Z >= 32)
                            Console.WriteLine("(recommended)");
                        else
                            Console.WriteLine("(also recommended)");
                        Console.ResetColor();
                        Console.WriteLine();

                        if (mapRange.X >= 32 && mapRange.Z >= 32)
                        {
                            Console.WriteLine("Choosing either 31x31 or 32x32 will cause blocks to appear outside the border which can break gameplay in some cases.");
                            Console.WriteLine();
                        }

                        Console.Write("Choose your option: ");

                        string option = Console.ReadLine();

                        if (int.TryParse(option, out int num) && num >= 1 && num <= 3)
                        {
                            switch(num)
                            {
                                case 1:
                                    size = MapSize.X31WithSmallBorder;
                                    break;
                                case 2:
                                    size = MapSize.X32WithBigBorder;
                                    break;
                                case 3:
                                    size = MapSize.X45WithSmallBorder;
                                    break;
                                default:
                                    continue;
                            }

                            break;
                        }
                        else
                            continue;
                    }
                }

                IslandConverter.ConvertToTM2Island(gbx, finishMapLoad.Value, f, size.GetValueOrDefault(), mapRange, minCoord.GetValueOrDefault(), random, cutoff.GetValueOrDefault(), ignoreMediaTracker.GetValueOrDefault());
            }

            Log.Write();
            Log.Write("All complete!");
        }

        private static void LogConsoleMode(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
