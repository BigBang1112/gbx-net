namespace GBX.NET;

public static class GbxPath
{
    public static string GetFileNameWithoutExtension(string path)
    {
        return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
    }
}
