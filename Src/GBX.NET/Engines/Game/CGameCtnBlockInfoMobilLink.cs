namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobilLink : IVersionable
{
    private int version;
    private string socketId;
    private CGameObjectModel? model;
    private CMwNod? u01;

    public int Version { get => version; set => version = value; }
    public string SocketId { get => socketId; set => socketId = value; }
    public CGameObjectModel? Model { get => model; set => model = value; }
    public CMwNod? U01 { get => u01; set => u01 = value; }

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.IdAsString(ref socketId);
        rw.NodeRef<CGameObjectModel>(ref model);

        if (Version == 0) // May still not be perfect
        {
            rw.NodeRef<CMwNod>(ref u01);
        }
    }
}
