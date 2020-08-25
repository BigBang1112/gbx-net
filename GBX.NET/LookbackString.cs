using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GBX.NET
{
    public class LookbackString
    {
        public ILookbackable Owner { get; }
        public string String { get; set; }

        public int Index
        {
            get => Owner.LookbackStrings.IndexOf(String);
        }

        public static Dictionary<int, string> CollectionIDs { get; }

        public LookbackString(string str, ILookbackable lookbackable)
        {
            Owner = lookbackable;
            String = str;
        }

        public static implicit operator string(LookbackString s)
        {
            if (s == null) return "";
            return s.ToString();
        }

        public override string ToString()
        {
            return String;
        }

        static LookbackString()
        {
            CollectionIDs = new Dictionary<int, string>();

            var startTimestamp = DateTime.Now;

            using (StringReader reader = new StringReader(Resources.CollectionID))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var keys = line.Split(' ');
                    if (keys.Length >= 2)
                        CollectionIDs[Convert.ToInt32(keys[0])] = keys[0];
                }
            }

            Debug.WriteLine("Collection IDs named in " + (DateTime.Now - startTimestamp).TotalMilliseconds + "ms");
        }
    }
}
