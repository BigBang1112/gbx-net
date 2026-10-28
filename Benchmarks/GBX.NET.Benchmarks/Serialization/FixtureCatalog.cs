namespace GBX.NET.Benchmarks.Serialization;

internal static class FixtureCatalog
{
    public static IReadOnlyList<GbxFixture> All { get; } = Array.AsReadOnly<GbxFixture>(
    [
        new("TMU map", "CGameCtnChallenge/CGameCtnChallenge TMU 001.Challenge.Gbx"),
        new("TM2020 map", "CGameCtnChallenge/CGameCtnChallenge TM2020 001.Map.Gbx"),
        new("MP4 ghost", "CGameCtnGhost/CGameCtnGhost MP4 001.Ghost.Gbx"),
        new("MP4 crystal", "CPlugCrystal/CPlugCrystal MP4 001.Crystal.Gbx")
    ]);
}
