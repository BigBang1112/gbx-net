namespace GBX.NET.Engines.Game;

[Node(0x03192000)]
public class CGameCtnBlockInfoMobilLink : CMwNod
{
    [NodeMember(ExactlyNamed = true)]
    public Id SocketId { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public CGameObjectModel? Model { get; set; }

    [NodeMember]
    public int Version { get; set; }
    
    [NodeMember]
    public Node? U04 { get; set; }
    
    [NodeMember]
    public GameBoxRefTable.File? U04File { get; set; }

    internal CGameCtnBlockInfoMobilLink()
    {

    }

    [Obsolete]
    public CGameCtnBlockInfoMobilLink(Id socketId, CGameObjectModel? model)
    {
        SocketId = socketId;
        Model = model;
    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        Version = r.ReadInt32();
        SocketId = r.ReadId();
        Model = r.ReadNodeRef<CGameObjectModel>();

        if (Version == 0) // May still not be perfect
        {
            U04 = r.ReadNodeRef(out var file);
            U04File = file;
        }
    }

    protected override async Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        Version = r.ReadInt32();
        SocketId = r.ReadId();
        Model = await r.ReadNodeRefAsync<CGameObjectModel>();

        if (Version == 0) // May still not be perfect
        {
            U04 = r.ReadNodeRef(out var file); // async wouldn't track it
            U04File = file;
        }
    }

    protected override void WriteChunkData(GameBoxWriter w)
    {
        w.Write(Version);
        w.WriteId(SocketId);
        w.Write(Model);

        if (Version == 0) // May still not be perfect
        {
            w.Write(U04, U04File);
        }
    }

    protected override Task WriteChunkDataAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        WriteChunkData(w);
        return Task.CompletedTask;
    }
}
