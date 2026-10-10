namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobilLink : IVersionable
{
    private int version = 1;
    private string socketId = string.Empty;
    private CGameObjectModel? model;

    public int Version { get => version; set => version = value; }
    public string SocketId { get => socketId; set => socketId = value; }
    public CGameObjectModel? Model { get => model; set => model = value; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.IdAsString(ref socketId);

        if (Version == 0)
        {
            // Version 0 stores the model's two references without a CGameObjectModel node.
            model ??= new CGameObjectModel();
            model.Phy = rw.NodeRef(model.Phy);
            var visFile = model.VisFile;
            model.Vis = rw.NodeRef(model.Vis, ref visFile);
            model.VisFile = visFile;
        }
        else
        {
            rw.NodeRef<CGameObjectModel>(ref model);
        }
    }
}
