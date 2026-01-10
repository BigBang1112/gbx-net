using GBX.NET.Components;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobilLink : IVersionable
{
    private int version;
    private string socketId = string.Empty;
    private CGameObjectModel? model;
    private CMwNod? u01;
    private GbxRefTableFile? u01File;

    public int Version { get => version; set => version = value; }
    public string SocketId { get => socketId; set => socketId = value; }
    public CGameObjectModel? Model { get => model; set => model = value; }
    public CMwNod? U01 { get => u01File?.GetNode(ref u01) ?? u01; set => u01 = value; }
    public CMwNod? GetU01(GbxReadSettings settings = default, bool exceptions = false) => u01File?.GetNode(ref u01, settings, exceptions) ?? u01;

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.IdAsString(ref socketId);
        rw.NodeRef<CGameObjectModel>(ref model);

        if (Version == 0) // May still not be perfect
        {
            rw.NodeRef<CMwNod>(ref u01, ref u01File);
        }
    }
}
