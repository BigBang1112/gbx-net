namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03053000</remarks>
[Node(0x03053000)]
[NodeExtension("TMEDClip")]
[NodeExtension("EDClip")]
public class CGameCtnBlockInfoClip : CGameCtnBlockInfo
{
    private string? aSymmetricalClipId;

    [NodeMember(ExactlyNamed = true)]
    public string? ASymmetricalClipId { get => aSymmetricalClipId; set => aSymmetricalClipId = value; }

    protected CGameCtnBlockInfoClip()
    {

    }

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x002 chunk
    /// </summary>
    [Chunk(0x03053002)]
    public class Chunk03053002 : Chunk<CGameCtnBlockInfoClip>
    {
        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.aSymmetricalClipId);
        }
    }

    #endregion
}
