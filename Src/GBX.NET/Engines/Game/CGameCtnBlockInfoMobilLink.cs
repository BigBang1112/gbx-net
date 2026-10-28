namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobilLink : IVersionable
{
    private int version = 1;
    private string socketId = string.Empty;
    private NodeRef<CGameObjectModel>? modelRef;

    public int Version { get => version; set => version = value; }
    public string SocketId { get => socketId; set => socketId = value; }
    public CGameObjectModel? Model { get => modelRef?.Resolve(); set => NodeRef.SetEmbedded(ref modelRef, value); }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.IdAsString(ref socketId);

        if (Version == 0)
        {
            // Version 0 stores the model's two references without a CGameObjectModel node.
            if (modelRef?.Descriptor is not null) throw new NotSupportedException("Version 0 stores the model inline.");
            var model = modelRef?.CachedNode ?? new CGameObjectModel();
            NodeRef.SetEmbedded(ref modelRef, model);
            rw.NodeRef(model.References.Phy);
            rw.NodeRef(model.References.Vis);
        }
        else
        {
            rw.NodeRef(ref modelRef);
        }
    }
}
