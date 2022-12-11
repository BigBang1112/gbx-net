namespace GBX.NET.Engines.Game;

/// <summary>
/// Ghost data.
/// </summary>
/// <remarks>ID: 0x0303F000</remarks>
[Node(0x0303F000)]
[NodeExtension("Ghost")]
public partial class CGameGhost : CMwNod
{
    private readonly Action<Task<Data?>> dataExceptionHandle;
    private bool isReplaying;
    private Task<Data?> sampleData;

    [NodeMember]
    [AppliedWithChunk<Chunk0303F006>]
    public bool IsReplaying { get => isReplaying; set => isReplaying = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0303F003>]
    [AppliedWithChunk<Chunk0303F005>]
    [AppliedWithChunk<Chunk0303F006>]
    public Data? SampleData => sampleData.Result;

    internal CGameGhost()
    {
        sampleData = Task.FromResult(default(Data));

        dataExceptionHandle = task =>
        {
            if (!task.IsFaulted)
                return;

            var exception = task.Exception;

            if (exception is null)
            {
                //Log.Write("Ghost data faulted without an exception", ConsoleColor.Yellow);
                return;
            }

            //Log.Write($"\nExceptions while reading ghost data: ({exception.InnerExceptions.Count})", ConsoleColor.Yellow);

            //foreach (var ex in exception.InnerExceptions)
            //    Log.Write(ex.ToString());
        };
    }

    public async Task<Data?> GetSampleDataAsync()
    {
        if (sampleData is null)
            return null;
        return await sampleData;
    }

    #region Chunks

    #region 0x003 chunk

    /// <summary>
    /// CGameGhost 0x003 chunk
    /// </summary>
    [Chunk(0x0303F003)]
    public class Chunk0303F003 : Chunk<CGameGhost>
    {
        public int[]? U01;
        public bool U02;
        public int U03;

        public byte[] Data { get; set; } = Array.Empty<byte>();
        public int[]? Samples { get; set; }
        public int SamplePeriod { get; set; }

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            Data = rw.Bytes(Data)!;
            Samples = rw.Array(Samples);

            rw.Array(ref U01);
            rw.Boolean(ref U02);
            SamplePeriod = rw.Int32(SamplePeriod);
            rw.Int32(ref U03);

            n.sampleData = Task.Run(() =>
            {
                var ghostData = new Data
                {
                    SamplePeriod = TimeInt32.FromMilliseconds(SamplePeriod)
                };

                using var ms = new MemoryStream(Data);

                ghostData.ReadSamples(ms, numSamples: Samples?.Length ?? 0, sizePerSample: 56);

                if (ghostData.NodeID == uint.MaxValue)
                    return null;

                return ghostData;
            });

            n.sampleData.ContinueWith(n.dataExceptionHandle);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameGhost 0x004 chunk
    /// </summary>
    [Chunk(0x0303F004)]
    public class Chunk0303F004 : Chunk<CGameGhost>
    {
        public int U01;

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // 0x0A103000
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameGhost 0x005 chunk
    /// </summary>
    [Chunk(0x0303F005)]
    public class Chunk0303F005 : Chunk<CGameGhost>
    {
        public int UncompressedSize { get; set; }
        public int CompressedSize { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            UncompressedSize = rw.Int32(UncompressedSize);
            CompressedSize = rw.Int32(CompressedSize);
            Data = rw.Bytes(Data, CompressedSize)!;

            if (rw.Reader is not null)
            {
                n.sampleData = Task.Run(() =>
                {
                    var ghostData = new Data();

                    using var ms = new MemoryStream(Data);
                    ghostData.Read(ms, compressed: true);

                    if (ghostData.NodeID == uint.MaxValue)
                        return null;

                    return ghostData;
                });

                n.sampleData.ContinueWith(n.dataExceptionHandle);
            }
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameGhost 0x006 chunk
    /// </summary>
    [Chunk(0x0303F006)]
    public class Chunk0303F006 : Chunk0303F005
    {
        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isReplaying);
            base.ReadWrite(n, rw);
        }
    }

    #endregion

    #region 0x007 skippable chunk

    /// <summary>
    /// CGameGhost 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0303F007), IgnoreChunk]
    public class Chunk0303F007 : SkippableChunk<CGameGhost>
    {
        
    }

    #endregion

    #endregion
}
