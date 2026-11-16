namespace GbxCLI.Tests;

/// <summary>
/// A temporary folder that is deleted with everything in it.
/// </summary>
internal sealed class TempDirectory : IDisposable
{
    public string Path { get; } = Directory.CreateTempSubdirectory("gbx-cli-").FullName;

    public string Combine(params string[] parts) => System.IO.Path.Combine([Path, .. parts]);

    public string Write(string name, byte[] contents)
    {
        var path = Combine(name);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, contents);
        return path;
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, recursive: true);
        }
    }
}
