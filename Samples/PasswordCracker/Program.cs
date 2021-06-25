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

            var node = GameBox.ParseNode(fileName);

            if (node is CGameCtnChallenge map)
            {
                map.CrackPassword();

                var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(fileName));
                map.Save(savePath);
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
