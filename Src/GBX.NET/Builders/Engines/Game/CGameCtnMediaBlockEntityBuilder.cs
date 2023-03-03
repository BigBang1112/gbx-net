namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockEntityBuilder : Builder
{
    public CPlugEntRecordData RecordData { get; set; }
    public TimeSingle StartOffset { get; set; }
    public int[]? NoticeRecords { get; set; }

    public CGameCtnMediaBlockEntityBuilder(CPlugEntRecordData recordData)
    {
        RecordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
    }

    public CGameCtnMediaBlockEntityBuilder WithStartOffset(TimeSingle startOffset)
    {
        StartOffset = startOffset;
        return this;
    }

    public CGameCtnMediaBlockEntityBuilder WithNoticeRecords(params int[] noticeRecords)
    {
        NoticeRecords = noticeRecords;
        return this;
    }

    public TM2020 ForTM2020() => new(this, NewNode());

    internal CGameCtnMediaBlockEntity NewNode()
    {
        var node = new CGameCtnMediaBlockEntity()
        {
            RecordData = RecordData,
            StartOffset = StartOffset,
            NoticeRecords = NoticeRecords ?? Array.Empty<int>(),
        };
        
        node.CreateChunk<CGameCtnMediaBlockEntity.Chunk0329F000>();

        return node;
    }
}