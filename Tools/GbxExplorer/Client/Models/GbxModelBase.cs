namespace GbxExplorer.Client.Models;

public abstract class GbxModelBase
{
    public string FileName { get; }
    public byte[] PureData { get; }
    public DateTimeOffset LastModified { get; }
    public string Sha256 { get; }
    
    public bool Selected { get; set; }

    public GbxModelBase(string fileName, DateTimeOffset lastModified, byte[] pureData, string sha256)
    {
        FileName = fileName;
        PureData = pureData;
        LastModified = lastModified;
        Sha256 = sha256;
    }
}