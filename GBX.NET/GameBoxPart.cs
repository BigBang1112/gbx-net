using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GBX.NET
{
    public abstract class GameBoxPart : ILookbackable
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public GameBox GBX { get; }

        public GameBoxPart(GameBox gbx) => GBX = gbx;
    }
}
