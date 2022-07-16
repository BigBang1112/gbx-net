namespace GBX.NET.Engines.Game;

[Node(0x03192000)]
public class CGameCtnBlockInfoMobilLink : CMwNod
{
    public Id SocketId { get; set; }
    public CGameObjectModel? Model { get; set; }
    
    public int U01 { get; set; }
    public int U02 { get; set; }
    public int U03 { get; set; }
    public int? U04 { get; set; }

    protected CGameCtnBlockInfoMobilLink()
    {
        
    }

    public CGameCtnBlockInfoMobilLink(Id socketId, CGameObjectModel? model)
    {
        SocketId = socketId;
        Model = model;
    }
}
