namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03147000</remarks>
[Class(0x03147000)]
public partial class CGamePlayerProfileChunk_GameStats : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03147000;




    private int totalTimePlay;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimePlay { get => totalTimePlay; set => totalTimePlay = value; }

    private int totalTimeInSolo;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSolo { get => totalTimeInSolo; set => totalTimeInSolo = value; }

    private int totalTimeInSoloRace;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSoloRace { get => totalTimeInSoloRace; set => totalTimeInSoloRace = value; }

    private int totalTimeInSoloPuzzle;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSoloPuzzle { get => totalTimeInSoloPuzzle; set => totalTimeInSoloPuzzle = value; }

    private int totalTimeInSoloPlatform;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSoloPlatform { get => totalTimeInSoloPlatform; set => totalTimeInSoloPlatform = value; }

    private int totalTimeInSoloScript;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSoloScript { get => totalTimeInSoloScript; set => totalTimeInSoloScript = value; }

    private int totalTimeInSplitScreen;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInSplitScreen { get => totalTimeInSplitScreen; set => totalTimeInSplitScreen = value; }

    private int totalTimeInHotSeat;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInHotSeat { get => totalTimeInHotSeat; set => totalTimeInHotSeat = value; }

    private int totalTimeInNetwork;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetwork { get => totalTimeInNetwork; set => totalTimeInNetwork = value; }

    private int totalTimeInNetworkTimeAttack;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkTimeAttack { get => totalTimeInNetworkTimeAttack; set => totalTimeInNetworkTimeAttack = value; }

    private int totalTimeInNetworkRounds;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkRounds { get => totalTimeInNetworkRounds; set => totalTimeInNetworkRounds = value; }

    private int totalTimeInNetworkLaps;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkLaps { get => totalTimeInNetworkLaps; set => totalTimeInNetworkLaps = value; }

    private int totalTimeInNetworkStunts;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkStunts { get => totalTimeInNetworkStunts; set => totalTimeInNetworkStunts = value; }

    private int totalTimeInNetworkCup;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkCup { get => totalTimeInNetworkCup; set => totalTimeInNetworkCup = value; }

    private int totalTimeInNetworkScript;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInNetworkScript { get => totalTimeInNetworkScript; set => totalTimeInNetworkScript = value; }

    private int totalTimeInEditChallenge;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInEditChallenge { get => totalTimeInEditChallenge; set => totalTimeInEditChallenge = value; }

    private int totalTimeInEditReplay;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInEditReplay { get => totalTimeInEditReplay; set => totalTimeInEditReplay = value; }

    private int totalTimeInEditSkin;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInEditSkin { get => totalTimeInEditSkin; set => totalTimeInEditSkin = value; }

    private int totalTimeInManiaLink;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalTimeInManiaLink { get => totalTimeInManiaLink; set => totalTimeInManiaLink = value; }

    private int totalNbReset;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalNbReset { get => totalNbReset; set => totalNbReset = value; }

    private int totalNbFinish;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalNbFinish { get => totalNbFinish; set => totalNbFinish = value; }

    private int totalNbChallenges;
    [AppliedWithChunk<Chunk03147000>]
    public int TotalNbChallenges { get => totalNbChallenges; set => totalNbChallenges = value; }

    private int averageTimePlay;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimePlay { get => averageTimePlay; set => averageTimePlay = value; }

    private int averageTimeInSolo;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSolo { get => averageTimeInSolo; set => averageTimeInSolo = value; }

    private int averageTimeInSoloRace;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSoloRace { get => averageTimeInSoloRace; set => averageTimeInSoloRace = value; }

    private int averageTimeInSoloPuzzle;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSoloPuzzle { get => averageTimeInSoloPuzzle; set => averageTimeInSoloPuzzle = value; }

    private int averageTimeInSoloPlatform;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSoloPlatform { get => averageTimeInSoloPlatform; set => averageTimeInSoloPlatform = value; }

    private int averageTimeInSoloScript;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSoloScript { get => averageTimeInSoloScript; set => averageTimeInSoloScript = value; }

    private int averageTimeInSplitScreen;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInSplitScreen { get => averageTimeInSplitScreen; set => averageTimeInSplitScreen = value; }

    private int averageTimeInHotSeat;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInHotSeat { get => averageTimeInHotSeat; set => averageTimeInHotSeat = value; }

    private int averageTimeInNetwork;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetwork { get => averageTimeInNetwork; set => averageTimeInNetwork = value; }

    private int averageTimeInNetworkTimeAttack;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkTimeAttack { get => averageTimeInNetworkTimeAttack; set => averageTimeInNetworkTimeAttack = value; }

    private int averageTimeInNetworkRounds;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkRounds { get => averageTimeInNetworkRounds; set => averageTimeInNetworkRounds = value; }

    private int averageTimeInNetworkLaps;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkLaps { get => averageTimeInNetworkLaps; set => averageTimeInNetworkLaps = value; }

    private int averageTimeInNetworkStunts;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkStunts { get => averageTimeInNetworkStunts; set => averageTimeInNetworkStunts = value; }

    private int averageTimeInNetworkCup;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkCup { get => averageTimeInNetworkCup; set => averageTimeInNetworkCup = value; }

    private int averageTimeInNetworkScript;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInNetworkScript { get => averageTimeInNetworkScript; set => averageTimeInNetworkScript = value; }

    private int averageTimeInEditChallenge;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageTimeInEditChallenge { get => averageTimeInEditChallenge; set => averageTimeInEditChallenge = value; }

    private int averageNbReset;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageNbReset { get => averageNbReset; set => averageNbReset = value; }

    private int averageNbFinish;
    [AppliedWithChunk<Chunk03147000>]
    public int AverageNbFinish { get => averageNbFinish; set => averageNbFinish = value; }

    private int maxTimePlay;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimePlay { get => maxTimePlay; set => maxTimePlay = value; }

    private int maxTimeInSolo;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSolo { get => maxTimeInSolo; set => maxTimeInSolo = value; }

    private int maxTimeInSoloRace;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSoloRace { get => maxTimeInSoloRace; set => maxTimeInSoloRace = value; }

    private int maxTimeInSoloPuzzle;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSoloPuzzle { get => maxTimeInSoloPuzzle; set => maxTimeInSoloPuzzle = value; }

    private int maxTimeInSoloPlatform;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSoloPlatform { get => maxTimeInSoloPlatform; set => maxTimeInSoloPlatform = value; }

    private int maxTimeInSoloScript;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSoloScript { get => maxTimeInSoloScript; set => maxTimeInSoloScript = value; }

    private int maxTimeInSplitScreen;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInSplitScreen { get => maxTimeInSplitScreen; set => maxTimeInSplitScreen = value; }

    private int maxTimeInHotSeat;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInHotSeat { get => maxTimeInHotSeat; set => maxTimeInHotSeat = value; }

    private int maxTimeInNetwork;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetwork { get => maxTimeInNetwork; set => maxTimeInNetwork = value; }

    private int maxTimeInNetworkTimeAttack;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkTimeAttack { get => maxTimeInNetworkTimeAttack; set => maxTimeInNetworkTimeAttack = value; }

    private int maxTimeInNetworkRounds;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkRounds { get => maxTimeInNetworkRounds; set => maxTimeInNetworkRounds = value; }

    private int maxTimeInNetworkLaps;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkLaps { get => maxTimeInNetworkLaps; set => maxTimeInNetworkLaps = value; }

    private int maxTimeInNetworkStunts;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkStunts { get => maxTimeInNetworkStunts; set => maxTimeInNetworkStunts = value; }

    private int maxTimeInNetworkCup;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkCup { get => maxTimeInNetworkCup; set => maxTimeInNetworkCup = value; }

    private int maxTimeInNetworkScript;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInNetworkScript { get => maxTimeInNetworkScript; set => maxTimeInNetworkScript = value; }

    private int maxTimeInEditChallenge;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxTimeInEditChallenge { get => maxTimeInEditChallenge; set => maxTimeInEditChallenge = value; }

    private int maxNbReset;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxNbReset { get => maxNbReset; set => maxNbReset = value; }

    private int maxNbFinish;
    [AppliedWithChunk<Chunk03147000>]
    public int MaxNbFinish { get => maxNbFinish; set => maxNbFinish = value; }

    private string? mostPlayed;
    [AppliedWithChunk<Chunk03147000>]
    public string? MostPlayed { get => mostPlayed; set => mostPlayed = value; }

    private string? mostRaced;
    [AppliedWithChunk<Chunk03147000>]
    public string? MostRaced { get => mostRaced; set => mostRaced = value; }

    private string? mostEdited;
    [AppliedWithChunk<Chunk03147000>]
    public string? MostEdited { get => mostEdited; set => mostEdited = value; }

    private string? mostNetted;
    [AppliedWithChunk<Chunk03147000>]
    public string? MostNetted { get => mostNetted; set => mostNetted = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_GameStats"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_GameStats() { }


    /// <summary>
    /// CGamePlayerProfileChunk_GameStats 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03147000)]
    public partial class Chunk03147000 : SkippableChunk<CGamePlayerProfileChunk_GameStats>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03147000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGamePlayerProfileChunk_GameStats n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.totalTimePlay);
            rw.Int32(ref n.totalTimeInSolo);
            rw.Int32(ref n.totalTimeInSoloRace);
            rw.Int32(ref n.totalTimeInSoloPuzzle);
            rw.Int32(ref n.totalTimeInSoloPlatform);
            rw.Int32(ref n.totalTimeInSoloScript);
            rw.Int32(ref n.totalTimeInSplitScreen);
            rw.Int32(ref n.totalTimeInHotSeat);
            rw.Int32(ref n.totalTimeInNetwork);
            rw.Int32(ref n.totalTimeInNetworkTimeAttack);
            rw.Int32(ref n.totalTimeInNetworkRounds);
            rw.Int32(ref n.totalTimeInNetworkLaps);
            rw.Int32(ref n.totalTimeInNetworkStunts);
            rw.Int32(ref n.totalTimeInNetworkCup);
            rw.Int32(ref n.totalTimeInNetworkScript);
            rw.Int32(ref n.totalTimeInEditChallenge);
            rw.Int32(ref n.totalTimeInEditReplay);
            rw.Int32(ref n.totalTimeInEditSkin);
            rw.Int32(ref n.totalTimeInManiaLink);
            rw.Int32(ref n.totalNbReset);
            rw.Int32(ref n.totalNbFinish);
            rw.Int32(ref n.totalNbChallenges);
            if (Version >= 2)
            {
                rw.Int32(ref U01);
            }
            rw.Int32(ref n.averageTimePlay);
            rw.Int32(ref n.averageTimeInSolo);
            rw.Int32(ref n.averageTimeInSoloRace);
            rw.Int32(ref n.averageTimeInSoloPuzzle);
            rw.Int32(ref n.averageTimeInSoloPlatform);
            rw.Int32(ref n.averageTimeInSoloScript);
            rw.Int32(ref n.averageTimeInSplitScreen);
            rw.Int32(ref n.averageTimeInHotSeat);
            rw.Int32(ref n.averageTimeInNetwork);
            rw.Int32(ref n.averageTimeInNetworkTimeAttack);
            rw.Int32(ref n.averageTimeInNetworkRounds);
            rw.Int32(ref n.averageTimeInNetworkLaps);
            rw.Int32(ref n.averageTimeInNetworkStunts);
            rw.Int32(ref n.averageTimeInNetworkCup);
            rw.Int32(ref n.averageTimeInNetworkScript);
            rw.Int32(ref n.averageTimeInEditChallenge);
            rw.Int32(ref n.averageNbReset);
            rw.Int32(ref n.averageNbFinish);
            rw.Int32(ref n.maxTimePlay);
            rw.Int32(ref n.maxTimeInSolo);
            rw.Int32(ref n.maxTimeInSoloRace);
            rw.Int32(ref n.maxTimeInSoloPuzzle);
            rw.Int32(ref n.maxTimeInSoloPlatform);
            rw.Int32(ref n.maxTimeInSoloScript);
            rw.Int32(ref n.maxTimeInSplitScreen);
            rw.Int32(ref n.maxTimeInHotSeat);
            rw.Int32(ref n.maxTimeInNetwork);
            rw.Int32(ref n.maxTimeInNetworkTimeAttack);
            rw.Int32(ref n.maxTimeInNetworkRounds);
            rw.Int32(ref n.maxTimeInNetworkLaps);
            rw.Int32(ref n.maxTimeInNetworkStunts);
            rw.Int32(ref n.maxTimeInNetworkCup);
            rw.Int32(ref n.maxTimeInNetworkScript);
            rw.Int32(ref n.maxTimeInEditChallenge);
            rw.Int32(ref n.maxNbReset);
            rw.Int32(ref n.maxNbFinish);
            rw.String(ref n.mostPlayed);
            rw.String(ref n.mostRaced);
            rw.String(ref n.mostEdited);
            rw.String(ref n.mostNetted);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03147000 => new Chunk03147000(),
        _ => base.NewChunk(chunkId),
    };
}
