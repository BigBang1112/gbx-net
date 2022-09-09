namespace GbxExplorer.Client.Models;

public class FaultyGbxModel : GbxModelBase
{
    public Exception Exception { get; }
    
    public bool Hovered { get; set; }

    public FaultyGbxModel(string fileName, DateTimeOffset lastModified, byte[] pureData, string sha256, Exception exception)
        : base(fileName, lastModified, pureData, sha256)
    {
        Exception = exception;
    }
}
