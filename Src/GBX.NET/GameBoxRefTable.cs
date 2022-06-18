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
        
        var folders = ReadRefTableFolders(r, numFolders, parentFolder: null).ToList();

        static IEnumerable<Folder> ReadRefTableFolders(GameBoxReader r, int numFolders, Folder? parentFolder)
        {
            for (var i = 0; i < numFolders; i++)
            {
                var name = r.ReadString();

                var folder = new Folder(name, parentFolder);

                yield return folder;

                var numSubFolders = r.ReadInt32();

                foreach (var subFolder in ReadRefTableFolders(r, numSubFolders, folder))
                {
                    yield return subFolder;
                }
            }
        }

        var files = new List<File>();

        for (var i = 0; i < numFiles; i++)
        {
            var fileName = default(string?);
            var resourceIndex = default(int?);
            var useFile = default(bool?);
            var folderIndex = -1;

            var flags = r.ReadInt32();

            if ((flags & 4) == 0)
            {
                fileName = r.ReadString();
            }
            else
            {
                resourceIndex = r.ReadInt32();
            }

            var nodeIndex = r.ReadInt32() - 1;

            if (header.Version >= 5)
            {
                useFile = r.ReadBoolean();
            }

            if ((flags & 4) == 0)
            {
                folderIndex = r.ReadInt32() - 1;
            }

            files.Add(new File(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex));
        }

        return new GameBoxRefTable(ancestorLevel, folders, files);
    }

    public void Write(GameBoxHeader header, GameBoxWriter w)
    {
        Write(header.Version, w);
    }

    public void Write(int version, GameBoxWriter w)
    {
        w.Write(Files.Count);

        if (Files.Count <= 0)
        {
            return;
        }

        w.Write(AncestorLevel);

        w.Write(Folders.Count(x => x.ParentFolder is null));

        WriteFolders(Folders);

        void WriteFolders(IEnumerable<Folder> folders)
        {
            foreach (var folder in folders)
            {
                w.Write(folder.Name);

                var subFolders = Folders.Where(x => x.ParentFolder == folder);
                w.Write(subFolders.Count());

                WriteFolders(subFolders);
            }
        }

        foreach (var file in Files)
        {
            w.Write(file.Flags);

            if ((file.Flags & 4) == 0)
            {
                w.Write(file.FileName);
            }
            else
            {
                w.Write(file.ResourceIndex.GetValueOrDefault());
            }

            w.Write(file.NodeIndex + 1);

            if (version >= 5)
            {
                w.Write(file.UseFile.GetValueOrDefault());
            }

            if ((file.Flags & 4) == 0)
            {
                w.Write(file.FolderIndex + 1);
            }
        }
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

        var folder = Folders[file.FolderIndex];

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

        var refTableNode = Files.FirstOrDefault(x => x.NodeIndex == nodeIndex);

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
