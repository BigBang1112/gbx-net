using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.IO;
using System.Linq;

namespace PasswordCracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args.FirstOrDefault();
            if (fileName == null) return;

            Log.OnLogEvent += Log_OnLogEvent;

            var gbx = GameBox.Parse(fileName);

            if (gbx is GameBox<CGameCtnChallenge> gbxMap)
            {
                gbxMap.MainNode.Password = null;
                gbxMap.MainNode.CrackPassword();
                gbxMap.Save(Path.GetFileName(fileName));
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
