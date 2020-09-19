using System;
using System.Collections.Generic;
using System.IO;

namespace GBX.NET
{
    public static class Log
    {
        public delegate void OnLog(string text, ConsoleColor color);

        public static event OnLog OnLogEvent;

        public static StringWriter MainLog { get; }
        public static Dictionary<string, StringWriter> AlternativeLogs { get; }

        static Log()
        {
            MainLog = new StringWriter();
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
            MainLog.WriteLine(text);

            foreach (var log in AlternativeLogs)
                log.Value.WriteLine(text);

            OnLogEvent?.Invoke(text, color);
        }

        public static void Write()
        {
            Write("");
        }
    }
}
