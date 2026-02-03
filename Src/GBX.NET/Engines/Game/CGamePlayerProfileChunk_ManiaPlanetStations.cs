namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_ManiaPlanetStations
{
    private GameStationDesc[]? stations;
    [AppliedWithChunk<Chunk03180002>]
    public GameStationDesc[]? Stations { get => stations; set => stations = value; }

    private bool hasSeenWelcomePage;
    public bool HasSeenWelcomePage { get => hasSeenWelcomePage; set => hasSeenWelcomePage = value; }

    public partial class Chunk03180002 : IVersionable
    {
        public int Version { get; set; }

        public bool U01;
        public int U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CGamePlayerProfileChunk_ManiaPlanetStations n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<GameStationDesc>(ref n.stations!, version: Version);
            rw.Boolean(ref U01);
            rw.Int32(ref U02);

            if (Version >= 2)
            {
                rw.Boolean(ref U03);

                foreach (var desc in n.stations!)
                {
                    // Placeholder for additional data per GameStationDesc in version 2
                    desc.U07 = rw.Int32(desc.U07);
                    desc.U08 = rw.Int32(desc.U08);
                    if (Version >= 3)
                    {
                        desc.U09 = rw.Boolean(desc.U09);
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
        public int? U07 { get; set; }
        public int? U08 { get; set; }
        public bool? U09 { get; set; }
        public string? DownloadUrl { get; set; }
    }
}
