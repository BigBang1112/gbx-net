using System.Collections.Generic;
using System.Linq;

namespace GBX.NET
{
    public class GameBoxRefTable
    {
        public Folder RootFolder { get; }
        public ExternalNode[] ExternalNodes { get; }

        public GameBoxRefTable(Folder rootFolder, params ExternalNode[] externalNodes)
        {
            RootFolder = rootFolder;
            ExternalNodes = externalNodes;
        }

        public void Write(GameBoxWriter w)
        {
            w.Write(ExternalNodes.Length);
            w.Write(RootFolder.Folders.Count);

            // ...
        }

        public class ControlEntry
        {
            public int Time { get; }
            public byte ControlNameIndex { get; }
            public bool Enable { get; }

            public ControlEntry(int time, byte controlNameIndex, bool enable)
            {
                Time = time;
                ControlNameIndex = controlNameIndex;
                Enable = enable;
            }
        }

        public class ExternalNode
        {
            public int Flags { get; }
            public string FileName { get; }
            public int? ResourceIndex { get; }
            public int NodeIndex { get; }
            public bool? UseFile { get; }
            public int? FolderIndex { get; }

            public ExternalNode(int flags, string fileName, int? resourceIndex, int nodeIndex, bool? useFile, int? folderIndex)
            {
                Flags = flags;
                FileName = fileName;
                ResourceIndex = resourceIndex;
                NodeIndex = nodeIndex;
                UseFile = useFile;
                FolderIndex = folderIndex;
            }
        }

        public class Folder
        {
            public string Name { get; }
            public Folder Parent { get; }
            public List<Folder> Folders { get; }

            public Folder(string name, Folder parent, params Folder[] folders)
            {
                Name = name;
                Parent = parent;
                Folders = folders.ToList();
            }

            public Folder(string name, Folder parent) : this(name, parent, new Folder[0])
            {

            }

            public Folder(string name) : this(name, null)
            {

            }
        }
    }
}