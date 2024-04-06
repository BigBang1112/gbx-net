
namespace GBX.NET.Engines.Plug;

public partial class CPlugPrefab : IVersionable
{
    private DateTime fileWriteTime;
    private string url = "";
    private int u01;
    private int u02;
    private EntRef[] ents = [];

    public int Version { get; set; }

    public DateTime FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }
    public string Url { get => url; set => url = value; }
    public int U01 { get => u01; set => u01 = value; }
    public int U02 { get => u02; set => u02 = value; }
    public EntRef[] Ents { get => ents; set => ents = value; }

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.FileTime(ref fileWriteTime);
        rw.String(ref url);
        rw.Int32(ref u01);
        var entsLength = rw.Int32(ents.Length);
        rw.Int32(ref u02);
        rw.ArrayReadableWritable<EntRef>(ref ents, entsLength);
    }
}
