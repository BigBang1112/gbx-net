using System;
using System.Collections.Generic;
using System.IO;

namespace GBX.NET
{
    public static class Log
    {
        public delegate void OnLog(string text, ConsoleColor color);
        public delegate void OnPush(int amount);

        public static event OnLog? OnLogEvent;
        public static event OnPush? OnPushEvent;

        public static List<string> MainLog { get; }
        public static Dictionary<string, StringWriter> AlternativeLogs { get; }

        static Log()
        {
            MainLog = new List<string>();
            AlternativeLogs = new Dictionary<string, StringWriter>();
        }

        public static bool Start(string fileName)
        {
            if (AlternativeLogs.ContainsKey(fileName)) return false;
            AlternativeLogs[fileName] = new StringWriter();
            return true;
        }

        public static void End(string fileName)
        {
            if (AlternativeLogs.TryGetValue(fileName, out StringWriter writer))
            {
                File.WriteAllText(fileName, writer.ToString());
                AlternativeLogs.Remove(fileName);
            }
        }

        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            MainLog.Add(text);

            foreach (var log in AlternativeLogs)
                log.Value.WriteLine(text);

            OnLogEvent?.Invoke(text, color);
        }

        public static void Write()
        {
            Write("");
        }

        public static void Push(int amount)
        {
            OnPushEvent?.Invoke(amount);
        }

        public static void Push()
        {
            Push(1);
        }
    }
}
