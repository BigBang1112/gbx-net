namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03146000</remarks>
[Class(0x03146000)]
public partial class CGamePlayerProfileChunk_GameScores : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03146000;




    private STrainingMedalsScores[]? trainingMedalsScores;
    [AppliedWithChunk<Chunk03146000>]
    public STrainingMedalsScores[]? TrainingMedalsScores { get => trainingMedalsScores; set => trainingMedalsScores = value; }

    private LadderMatchResult[]? ladderMatchResults;
    [AppliedWithChunk<Chunk03146005>]
    public LadderMatchResult[]? LadderMatchResults { get => ladderMatchResults; set => ladderMatchResults = value; }


    /// <summary>
    /// CGamePlayerProfileChunk_GameScores 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03146000)]
    public partial class Chunk03146000 : SkippableChunk<CGamePlayerProfileChunk_GameScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03146000;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_GameScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<STrainingMedalsScores>(ref n.trainingMedalsScores!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameScores 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03146001)]
    public partial class Chunk03146001 : SkippableChunk<CGamePlayerProfileChunk_GameScores>
    {
        /// <inheritdoc />
        public override uint Id => 0x03146001;

    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameScores 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03146004)]
    public partial class Chunk03146004 : SkippableChunk<CGamePlayerProfileChunk_GameScores>
    {
        /// <inheritdoc />
        public override uint Id => 0x03146004;

    }

    /// <summary>
    /// CGamePlayerProfileChunk_GameScores 0x005 skippable chunk
    /// </summary>
    [Chunk(0x03146005)]
    public partial class Chunk03146005 : SkippableChunk<CGamePlayerProfileChunk_GameScores>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03146005;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_GameScores n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<LadderMatchResult>(ref n.ladderMatchResults!);
        }
    }


    public sealed partial class STrainingMedalsScores : IReadableWritable
    {
    }

    public sealed partial class LadderMatchResult : IReadableWritable
    {

        private DateTime? u01;
        public DateTime? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.SystemTime(ref u01);
            rw.Single(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03146000 => new Chunk03146000(),
        0x03146001 => new Chunk03146001(),
        0x03146004 => new Chunk03146004(),
        0x03146005 => new Chunk03146005(),
        _ => base.NewChunk(chunkId),
    };
}
