using System.Diagnostics;

namespace GBX.NET.PAK;

public class NadeoPakFolder
{
    public string Name { get; }
    public NadeoPakFolder? Parent { get; }
    public List<NadeoPakFolder> Folders { get; }
    public List<NadeoPakFile> Files { get; }

    public NadeoPakFolder(string name, NadeoPakFolder? parent)
    {
        Name = name;
        Parent = parent;
        Folders = new List<NadeoPakFolder>();
        Files = new List<NadeoPakFile>();
    }

    public override string ToString()
    {
        return Name;
    }

    public void CastFoldersInFileNames(Dictionary<string, string> hashes)
    {
        var newFiles = Files.Where(x => GetBruteforcedNameOrDefault(x.Name, hashes).Contains('\\')).ToList();

        var removedFiles = Files.RemoveAll(x => GetBruteforcedNameOrDefault(x.Name, hashes).Contains('\\'));

        Debug.Assert(newFiles.Count == removedFiles);

        foreach (var file in newFiles)
        {
            var pathSplit = GetBruteforcedNameOrDefault(file.Name, hashes).Split('\\');

            if (pathSplit.Length != 2)
            {
                continue; // shouldn't went there
            }

            var finalFolder = RecurseFolderCreation(this, pathSplit);

            file.Name = pathSplit[pathSplit.Length - 1];
            file.Folder = finalFolder;

            finalFolder.Files.Add(file);
        }
    }

    private static string GetBruteforcedNameOrDefault(string n, Dictionary<string, string> hashes)
    {
        if (!hashes.TryGetValue(n, out string? name))
        {
            name = n;
        }

        return name;
    }

    private NadeoPakFolder RecurseFolderCreation(NadeoPakFolder currentFolder, string[] pathSplit, int index = 0)
    {
        var folderName = $"{pathSplit[index]}\\";

        var folderToUse = currentFolder.Folders.FirstOrDefault(x => x.Name == folderName);

        if (folderToUse is null)
        {
            folderToUse = new NadeoPakFolder(folderName, currentFolder);
            currentFolder.Folders.Add(folderToUse);
        }

        var nextIndex = index + 1;

        if (nextIndex != pathSplit.Length - 1) // Not gonna be last
        {
            return RecurseFolderCreation(folderToUse, pathSplit, nextIndex);
        }

        return folderToUse;
    }
}
