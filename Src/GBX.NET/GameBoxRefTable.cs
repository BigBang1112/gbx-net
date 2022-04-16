using System.Text;

namespace GBX.NET;

public partial class GameBoxRefTable
{
    /// <summary>
    /// How many folder levels to go up in the .pak folder hierarchy to reach the base folder from which files will be referenced.
    /// </summary>
    public int AncestorLevel { get; init; }
    public IList<Folder> Folders { get; init; }
    public IList<File> Files { get; init; }

    public GameBoxRefTable(int ancestorLevel, IList<Folder> folders, IList<File> files)
    {
        AncestorLevel = ancestorLevel;
        Folders = folders;
        Files = files;
    }

    /// <summary>
    /// Parses the reference table.
    /// </summary>
    /// <param name="header">Header used to read ref. table compression and version from.</param>
    /// <param name="r">Reader.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>Reference table.</returns>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBoxRefTable? Parse(GameBoxHeader header, GameBoxReader r, ILogger? logger = null)
    {
        if (header.CompressionOfRefTable != GameBoxCompression.Uncompressed)
        {
            throw new CompressedRefTableException();
        }

        var numFiles = r.ReadInt32(); // With this, number of files value can be optimized

        switch (numFiles)
        {
            case 0:
                logger?.LogDebug("No external nodes found, reference table completed.");
                return null;
            case < 0:
                throw new InvalidDataException();
        }

        var ancestorLevel = r.ReadInt32();
        var numFolders = r.ReadInt32();

        var allFolders = new List<Folder>();

        var indexCounter = 0;
        var folders = ReadRefTableFolders(numFolders, ref indexCounter, asParentFolder: null);

        Folder[] ReadRefTableFolders(int n, ref int indexCounter, Folder? asParentFolder)
        {
            var folders = new Folder[n];

            for (var i = 0; i < n; i++)
            {
                var name = r.ReadString();
                var numSubFolders = r.ReadInt32();

                var folder = new Folder(name, indexCounter, asParentFolder);
                allFolders.Add(folder);

                indexCounter++;

                foreach (var subFolder in ReadRefTableFolders(numSubFolders, ref indexCounter, folder))
                {
                    folder.Folders.Add(subFolder);
                }

                folders[i] = folder;
            }

            return folders;
        }

        var files = new List<File>();

        for (var i = 0; i < numFiles; i++)
        {
            string? fileName = null;
            int? resourceIndex = null;
            bool? useFile = null;
            int? folderIndex = null;

            var flags = r.ReadInt32();

            if ((flags & 4) == 0)
                fileName = r.ReadString();
            else
                resourceIndex = r.ReadInt32();

            var nodeIndex = r.ReadInt32() - 1;

            if (header.Version >= 5)
                useFile = r.ReadBoolean();

            if ((flags & 4) == 0)
                folderIndex = r.ReadInt32() - 1;

            var file = new File(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);

            if (!folderIndex.HasValue)
                continue;

            if (folderIndex.Value < 0)
                files.Add(file);
            else
                allFolders[folderIndex.Value].Files.Add(file);
        }

        return new GameBoxRefTable(ancestorLevel, folders, files);
    }

    public void Write(GameBoxHeader header, GameBoxWriter w)
    {
        Write(header.Version, w);
    }

    public void Write(int version, GameBoxWriter w)
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

            if (version >= 5)
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

        if (file.FolderIndex == -1)
        {
            return "";
        }

        var folder = GetAllFolders().First(x => x.Index == file.FolderIndex);

        var parentBuilder = new StringBuilder(folder.Name);

        var parentFolder = folder.ParentFolder;

        while (parentFolder is not null)
        {
            parentBuilder.Insert(0, '/');
            parentBuilder.Insert(0, parentFolder.Name);

            parentFolder = parentFolder.ParentFolder;
        }

        mainBuilder.Append(parentBuilder);

        return mainBuilder.ToString();
    }

    public Node? GetNode(Node? nodeAtTheMoment, int? nodeIndex, string? fileName)
    {
        if (nodeAtTheMoment is not null || nodeIndex is null)
            return nodeAtTheMoment;

        if (Folders.Count == 0 && Files.Count == 0)
            return nodeAtTheMoment;

        var currentGbxFolderPath = Path.GetDirectoryName(fileName);

        if (currentGbxFolderPath is null)
            return nodeAtTheMoment;

        var refTableNode = GetAllFiles().FirstOrDefault(x => x.NodeIndex == nodeIndex);

        if (refTableNode is null)
            return nodeAtTheMoment;

        var folderPath = GetRelativeFolderPathToFile(refTableNode);
        var finalFileName = Path.Combine(currentGbxFolderPath, folderPath, refTableNode.FileName ?? "");

        return GameBox.ParseNode(finalFileName);
    }

    private static string Repeat(string value, int count)
    {
        return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
    }
}
