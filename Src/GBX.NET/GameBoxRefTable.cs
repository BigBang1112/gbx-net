using GBX.NET.Extensions;
using System.Text;

namespace GBX.NET;

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

        var indexCounter = 0;
        Folders = ReadRefTableFolders(numFolders, ref indexCounter, asParentFolder: null);

        Folder[] ReadRefTableFolders(int n, ref int indexCounter, Folder? asParentFolder)
        {
            var folders = new Folder[n];

            for (var i = 0; i < n; i++)
            {
                var name = reader.ReadString();
                var numSubFolders = reader.ReadInt32();

                var folder = new Folder(name, indexCounter, asParentFolder);
                allFolders.Add(folder);

                indexCounter++;

                foreach (var subFolder in ReadRefTableFolders(numSubFolders, ref indexCounter, folder))
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

            var nodeIndex = reader.ReadInt32() - 1;

            if (Header.Version >= 5)
                useFile = reader.ReadBoolean();

            if ((flags & 4) == 0)
                folderIndex = reader.ReadInt32() - 1;

            var file = new File(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);

            if (!folderIndex.HasValue)
                continue;

            if (folderIndex.Value - 1 < 0)
                Files.Add(file);
            else
                allFolders[folderIndex.Value - 1].Files.Add(file);
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

            w.Write(file.NodeIndex + 1);

            if (Header.Version >= 5)
                w.Write(file.UseFile.GetValueOrDefault());

            if ((file.Flags & 4) == 0)
                w.Write(file.FolderIndex.GetValueOrDefault() + 1);
        }
    }

    public IEnumerable<Folder> GetAllFolders()
    {
        return Folders.Flatten(x => x.Folders);
    }

    public IEnumerable<File> GetAllFiles()
    {
        foreach (var file in GetAllFolders().SelectMany(x => x.Files))
            yield return file;
        
        foreach (var file in Files)
            yield return file;
    }

    public string GetRelativeFolderPathToFile(File file)
    {
        // could be normal file not just gbx
        //var folder = GetAllFolders().FirstOrDefault(x => x.Index == file.FolderIndex);
        //var bruh = folders.ElementAt(file.FolderIndex.GetValueOrDefault());
        var mainBuilder = new StringBuilder()
            .Insert(0, "../", count: AncestorLevel);

        var folder = GetAllFolders().First(x => x.Index == file.FolderIndex);

        var parentBuilder = new StringBuilder(folder.Name);

        var parentFolder = folder.ParentFolder;

        while (parentFolder is not null)
        {
            parentBuilder.Insert(0, '/');
            parentBuilder.Insert(0, parentFolder.Name);

            parentFolder = parentFolder.ParentFolder;
        }

        mainBuilder.Append(parentBuilder.ToString());

        return mainBuilder.ToString();
    }

    private static string Repeat(string value, int count)
    {
        return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
    }

    public record File
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
        public int Index { get; }
        public Folder? ParentFolder { get; }
        public IList<Folder> Folders { get; }
        public IList<File> Files { get; }

        public Folder(string name, int index, Folder? parentFolder = null)
        {
            Name = name;
            Index = index;
            ParentFolder = parentFolder;
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
