using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.PAK
{
    public class NadeoPakFolder
    {
        public string Name { get; }
        public List<NadeoPakFolder> Folders { get; }
        public List<NadeoPakFile> Files { get; }

        public NadeoPakFolder(string name)
        {
            Name = name;
            Folders = new List<NadeoPakFolder>();
            Files = new List<NadeoPakFile>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
