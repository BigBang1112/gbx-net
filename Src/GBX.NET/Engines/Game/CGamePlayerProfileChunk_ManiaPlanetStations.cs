namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_ManiaPlanetStations
{
    private GameStationDesc[]? stations;
    public partial GameStationDesc[]? Stations { get => stations; set => stations = value; }

    private bool hasSeenWelcomePage;
    public partial bool HasSeenWelcomePage { get => hasSeenWelcomePage; set => hasSeenWelcomePage = value; }

    public partial class Chunk03180002 : IVersionable
    {
        public int Version { get; set; }

        public bool U04;

        public override void ReadWrite(CGamePlayerProfileChunk_ManiaPlanetStations n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<GameStationDesc>(ref n.stations!, version: Version);
            rw.Boolean(ref n.isFirstLaunch);
            rw.Int32(ref n.installStationClickCount);

            if (Version >= 2)
            {
                rw.Boolean(ref n.isManualLayout);

                foreach (var desc in n.stations!)
                {
                    desc.AutoLayoutTimestamp = rw.Int32(desc.AutoLayoutTimestamp);
                    desc.Order = rw.Int32(desc.Order);
                    if (Version >= 3)
                    {
                        desc.LoadWithParentalLock = rw.Boolean(desc.LoadWithParentalLock);
                        if (Version >= 5)
                        {
                            desc.DownloadUrl = rw.String(desc.DownloadUrl);
                        }
                    }
                }

                if (Version >= 4)
                {
                    rw.Boolean(ref n.hasSeenWelcomePage);
                }
            }
        }
    }

    public sealed partial class GameStationDesc
    {
        /// <summary>
        /// Unix timestamp used to order stations in the automatic layout.
        /// </summary>
        public int? AutoLayoutTimestamp { get; set; }
        public int? Order { get; set; }
        public bool? LoadWithParentalLock { get; set; }
        public string? DownloadUrl { get; set; }
    }
}
