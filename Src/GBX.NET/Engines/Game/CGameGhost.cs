namespace GBX.NET.Engines.Game;

public partial class CGameGhost
{
    private Data? sampleData;
    private int sampleDataVersion;

    public RawData? RawData { get; set; }
    public ZlibData? CompressedData { get; set; }

#if NET9_0_OR_GREATER
    private readonly Lock SampleDataLock = new();
#else
    private readonly object SampleDataLock = new();
#endif

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    [AppliedWithChunk<Chunk0303F003>]
    [AppliedWithChunk<Chunk0303F005>]
    [AppliedWithChunk<Chunk0303F006>]
    public Data SampleData
    {
        get
        {
            // old format which precreated the sampleData instance
            if (sampleData is not null)
            {
                if (RawData is null or { Parsed: true })
                {
                    return sampleData;
                }

                lock (SampleDataLock)
                {
                    if (RawData is null or { Parsed: true }) return sampleData;
                    try
                    {
                        using var ms = new MemoryStream(RawData.Data);
                        using var reader = new GbxReader(ms);
                        sampleData.SavedMobilClassId = savedMobilClassId.GetValueOrDefault();
                        sampleData.ReadSamplesOld(reader);
                        RawData.Parsed = true;
                        return sampleData;
                    }
                    catch (Exception ex)
                    {
                        RawData.Exception = ex;
                        throw;
                    }
                }
            }

            if (CompressedData is null) throw new InvalidOperationException("CompressedData not available");

            lock (SampleDataLock)
            {
                if (sampleData is not null) return sampleData;

                try
                {
                    using var reader = CompressedData.OpenDecompressedReader();
                    var data = new Data();
                    data.ReadNew(reader, sampleDataVersion);
                    CompressedData.Parsed = true;
                    CompressedData.Exception = null;
                    sampleData = data;
                    return data;
                }
                catch (Exception ex)
                {
                    CompressedData.Exception = ex;
                    throw;
                }
            }
        }
    }

    public partial class Chunk0303F003
    {
        public override void Read(CGameGhost n, GbxReader r)
        {
            n.RawData = new RawData(r.ReadData(), exception: null);
            n.sampleData = r.ReadReadable<Data>(version: 0);
        }

        public override void Write(CGameGhost n, GbxWriter w)
        {
            w.WriteData(n.RawData?.Data);
            w.WriteWritable<Data>(n.sampleData, version: 0);
        }
    }

    public partial class Chunk0303F005
    {
        public override void Read(CGameGhost n, GbxReader r)
        {
            n.sampleDataVersion = (this as IVersionable)?.Version ?? 0;
            n.sampleData = null;
            n.CompressedData = r.ReadZlibData();
        }

        public override void Write(CGameGhost n, GbxWriter w)
        {
            var version = (this as IVersionable)?.Version ?? 0;
            w.WriteZlibData(n.CompressedData, writer => n.SampleData.WriteNew(writer, version));
        }
    }
}
