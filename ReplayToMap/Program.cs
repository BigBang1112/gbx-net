using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Linq;

namespace ReplayToMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args.FirstOrDefault();
            if (fileName == null) return;

            Log.OnLogEvent += Log_OnLogEvent;

            var gbx = GameBox.Parse(fileName);

            if (gbx is GameBox<CGameCtnReplayRecord> gbxReplay)
            {
                var track = gbxReplay.MainNode.Track.Result;
                track.Save(Formatter.Deformat(track.MainNode.MapName + ".Map.Gbx"));
            }
        }

        private static void Log_OnLogEvent(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}