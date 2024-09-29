namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    private Data? sampleData;

    public byte[]? RawData { get; set; }
    public CompressedData? CompressedData { get; set; }

    [AppliedWithChunk<Chunk0303F003>]
    [AppliedWithChunk<Chunk0303F005>]
    [AppliedWithChunk<Chunk0303F006>]
    public Data? SampleData
    {
        get
        {
            if (sampleData is null)
            {
                return null;
            }

            sampleData.Parse();

            return sampleData;
        }
    }

    public partial class Chunk0303F003
    {
        public int[]? Times;

        public override void Read(CGameGhost n, GbxReader r)
        {
            n.RawData = r.ReadData();
            n.sampleData = new Data(n.RawData)
            {
                Offsets = r.ReadArray<int>()
            };
            Times = r.ReadArray<int>();
            n.sampleData.IsFixedTimeStep = r.ReadBoolean();
            n.sampleData.SamplePeriod = r.ReadTimeInt32();
            n.sampleData.Version = r.ReadInt32();
        }

        public override void Write(CGameGhost n, GbxWriter w)
        {
            w.Write(n.RawData);
            w.WriteArray(n.sampleData?.Offsets);
            w.WriteArray(Times);
            w.Write(n.sampleData?.IsFixedTimeStep ?? false);
            w.Write(n.sampleData?.SamplePeriod ?? TimeInt32.FromMilliseconds(100));
            w.Write(n.sampleData?.Version ?? 0);
        }
    }

    public partial class Chunk0303F005
    {
        public override void Read(CGameGhost n, GbxReader r)
        {
            var uncompressedSize = r.ReadInt32();
            var data = r.ReadData();
            n.CompressedData = new(uncompressedSize, data);

            n.sampleData = new Data(n.CompressedData);
        }

        public override void Write(CGameGhost n, GbxWriter w)
        {
            w.Write(n.CompressedData?.UncompressedSize ?? 0);
            w.WriteData(n.CompressedData?.Data);
        }
    }
}
