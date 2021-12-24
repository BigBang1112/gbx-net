namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Entity (0x0329F000)
/// </summary>
[Node(0x0329F000)]
public sealed class CGameCtnMediaBlockEntity : CGameCtnMediaBlock
{
    private CPlugEntRecordData recordData;

    public CPlugEntRecordData RecordData
    {
        get => recordData;
        set => recordData = value;
    }

    private CGameCtnMediaBlockEntity()
    {
        recordData = null!;
    }

    [Chunk(0x0329F000)]
    public class Chunk0329F000 : Chunk<CGameCtnMediaBlockEntity>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockEntity n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugEntRecordData>(ref n.recordData!);

            rw.UntilFacade(Unknown);
        }
    }
}
