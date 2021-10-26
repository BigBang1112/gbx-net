using GBX.NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GBX.NET
{
    public class GameBoxRefTable
    {
        public GameBoxHeaderInfo Header { get; }

        /// <summary>
        /// How many folder levels to go up in the .pak folder hierarchy to reach the base folder from which files will be referenced.
        /// </summary>
        public int AncestorLevel { get; private set; }
        public IList<Folder> Folders { get; private set; }
        public IList<File> Files { get; private set; }

        public GameBoxRefTable(GameBoxHeaderInfo header)
        {
            Header = header;

            Folders = new List<Folder>();
            Files = new List<File>();
        }

        internal void Read(GameBoxReader reader)
        {
            var numFiles = reader.ReadInt32(); // With this, number of files value can be optimized

            if (numFiles <= 0)
            {
                Log.Write("No external nodes found, reference table completed.", ConsoleColor.Green);
                return;
            }

            AncestorLevel = reader.ReadInt32();
            var numFolders = reader.ReadInt32();

            var allFolders = new List<Folder>();

            Folders = ReadRefTableFolders(numFolders);

            Folder[] ReadRefTableFolders(int n)
            {
                var folders = new Folder[n];

                for (var i = 0; i < n; i++)
                {
                    var name = reader.ReadString();
                    var numSubFolders = reader.ReadInt32();

                    var folder = new Folder(name);
                    allFolders.Add(folder);
                    foreach (var subFolder in ReadRefTableFolders(numSubFolders))
                        folder.Folders.Add(subFolder);

                    folders[i] = folder;
                }

                return folders;
            }

            Files = new List<File>();

            for (var i = 0; i < numFiles; i++)
            {
                string? fileName = null;
                int? resourceIndex = null;
                bool? useFile = null;
                int? folderIndex = null;

                var flags = reader.ReadInt32();

                if ((flags & 4) == 0)
                    fileName = reader.ReadString();
                else
                    resourceIndex = reader.ReadInt32();

                var nodeIndex = reader.ReadInt32();

                if (Header.Version >= 5)
                    useFile = reader.ReadBoolean();

                if ((flags & 4) == 0)
                    folderIndex = reader.ReadInt32();

                var file = new File(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);

                if (folderIndex.HasValue)
                {
                    if (folderIndex.Value - 1 < 0)
                        Files.Add(file);
                    else
                        allFolders[folderIndex.Value - 1].Files.Add(file);
                }
            }
        }

        internal void Write(GameBoxWriter w)
        {
            var allFiles = GetAllFiles();
            var numFiles = allFiles.Count();

            w.Write(numFiles);

            if (numFiles <= 0)
            {
                return;
            }

            w.Write(AncestorLevel);
            w.Write(Folders.Count);

            WriteFolders(Folders);

            void WriteFolders(IEnumerable<Folder> folders)
            {
                if (folders == null) return;

                foreach (var folder in folders)
                {
                    w.Write(folder.Name);
                    w.Write(folder.Folders.Count);

                    WriteFolders(folder.Folders);
                }
            }

            foreach (var file in allFiles)
            {
                w.Write(file.Flags);

                if ((file.Flags & 4) == 0)
                    w.Write(file.FileName);
                else
                    w.Write(file.ResourceIndex.GetValueOrDefault());

                w.Write(file.NodeIndex);

                if (Header.Version >= 5)
                    w.Write(file.UseFile.GetValueOrDefault());

                if ((file.Flags & 4) == 0)
                    w.Write(file.FolderIndex.GetValueOrDefault());
            }
        }

        public IEnumerable<File> GetAllFiles()
        {
            return Folders.Flatten(x => x.Folders).SelectMany(x => x.Files);
        }

        public class File
        {
            public int Flags { get; }
            public string? FileName { get; }
            public int? ResourceIndex { get; }
            public int NodeIndex { get; }
            public bool? UseFile { get; }
            public int? FolderIndex { get; }

            public File(int flags, string? fileName, int? resourceIndex, int nodeIndex, bool? useFile, int? folderIndex)
            {
                Flags = flags;
                FileName = fileName;
                ResourceIndex = resourceIndex;
                NodeIndex = nodeIndex;
                UseFile = useFile;
                FolderIndex = folderIndex;
            }

            public override string ToString()
            {
                return FileName ?? string.Empty;
            }
        }

        public class Folder
        {
            public string Name { get; }
            public IList<Folder> Folders { get; }
            public IList<File> Files { get; }

            public Folder(string name)
            {
                Name = name;
                Folders = new List<Folder>();
                Files = new List<File>();
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}