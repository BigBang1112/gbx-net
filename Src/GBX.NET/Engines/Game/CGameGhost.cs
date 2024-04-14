namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    private Data? sampleData;

    public byte[]? RawData { get; set; }
    public CompressedData? CompressedData { get; set; }

    public partial class Chunk0303F003
    {
        public int[]? Times;

        public override void Read(CGameGhost n, GbxReader r)
        {
            n.RawData = r.ReadData();
            n.sampleData = new Data(n.RawData)
            {
                Offsets = r.ReadArray<int>(),
                IsFixedTimeStep = r.ReadBoolean(),
                SamplePeriod = r.ReadTimeInt32(),
                Version = r.ReadInt32()
            };
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
