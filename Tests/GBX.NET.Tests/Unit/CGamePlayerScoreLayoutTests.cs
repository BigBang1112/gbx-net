using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CGamePlayerScoreLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    public async Task ScoreVersionsKeepNativeFieldOrderAndUnsignedStatistics(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(3); // ID table version.
            w.Write(uint.MaxValue);
            w.Write(uint.MaxValue);
            w.Write(uint.MaxValue);
            if (version == 0)
            {
                w.Write(0xFEDCBA98u);
            }
            w.Write(12345);
            if (version <= 4)
            {
                if (version >= 1)
                {
                    w.Write(11u);
                    w.Write(1); // Four-byte boolean.
                }
                if (version >= 2)
                {
                    w.Write(12u);
                    w.Write(13u);
                    w.Write(14u);
                }
                if (version >= 3)
                {
                    w.Write(15u);
                }
                if (version == 4)
                {
                    w.Write(16u);
                }
            }
            else
            {
                w.Write(0xFEDCBA98u);
                w.Write(0x87654321u);
                w.Write(0x80000001u);
                w.Write((ushort)0xFEDC);
                w.Write((ushort)0x8765);
            }
            if (version >= 6) w.Write(999u);
            if (version >= 7) w.Write(27u);
            if (version >= 8) w.Write(123456u);
            if (version >= 9)
            {
                w.Write(0UL);
                w.Write(0UL);
                if (version <= 15)
                {
                    w.Write(1); // Deprecated league-score count.
                    w.Write(5); // Each league score has its own version.
                    w.Write(-1);
                    w.Write(-1);
                }
            }
            if (version >= 10) w.Write(30u);
            if (version >= 11) w.Write(31u);
            if (version >= 12) w.Write(4u);
            if (version >= 13)
            {
                w.Write((byte)2); // Platform in the master-server play-mode enum.
                String(w, "Platform map");
            }
            if (version >= 14) w.Write(32u);
            if (version >= 15) w.Write(33u);
            if (version >= 17)
            {
                w.Write(34u);
                w.Write(35u);
                w.Write(36u);
                w.Write(37u);
            }
            if (version >= 18)
            {
                w.Write(101u);
                w.Write(102u);
                w.Write(103u);
                w.Write((ushort)0x8001);
                w.Write((ushort)0xFFFE);
            }
        });
        var score = new CGamePlayerScore.Score();

        await RoundTrip(payload, rw => score.ReadWrite(rw, version));
        await Assert.That(score.PersonalBest.TotalMilliseconds).IsEqualTo(12345);
        if (version >= 5)
        {
            await Assert.That(score.EditPlayTimeSeconds).IsEqualTo(0xFEDCBA98u);
            await Assert.That(score.RacePlayTimeSeconds).IsEqualTo(0x87654321u);
            await Assert.That(score.NetPlayTimeSeconds).IsEqualTo(0x80000001u);
            await Assert.That(score.ResetCount).IsEqualTo((ushort)0xFEDC);
            await Assert.That(score.FinishCount).IsEqualTo((ushort)0x8765);
        }
        if (version >= 13)
        {
            await Assert.That(score.PlayMode).IsEqualTo(CGamePlayerScore.EChallengePlayModeMS.Platform);
        }
        if (version is >= 9 and <= 15)
        {
            await Assert.That(score.DeprecatedChallengeLeagueScores!.Length).IsEqualTo(1);
        }
        if (version >= 18)
        {
            await Assert.That(score.SubmittedEditPlayTimeSeconds).IsEqualTo(101u);
            await Assert.That(score.SubmittedResetCount).IsEqualTo((ushort)0x8001);
            await Assert.That(score.SubmittedFinishCount).IsEqualTo((ushort)0xFFFE);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task DeprecatedLeagueScoresKeepArrayVersionMarkers(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 4)
            {
                w.Write(-1);
                w.Write(-1);
            }
            else
            {
                w.Write(0xFEDCBA98u);
                w.Write(0x87654321u);
                String(w, "league");
            }
            if (version < 5)
            {
                w.Write(10); // Deprecated array version before its count.
                w.Write(2);
                w.Write(-1);
                w.Write(-1);
                if (version == 3)
                {
                    String(w, "legacy");
                }
            }
        });
        var score = new CGamePlayerScore.DeprecatedChallengeLeagueScore();

        await RoundTrip(payload, rw => score.ReadWrite(rw));
        await Assert.That(score.Version).IsEqualTo(version);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task DeprecatedCampaignSkillsKeepIndependentFilteredScoreVersions(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0xFEDCBA98u);
            String(w, "campaign");
            if (version == 1)
            {
                w.Write(123u);
            }
            else
            {
                if (version != 2) w.Write(124u);
                w.Write(1); // First filtered-score array uses version 1.
                w.Write(1);
                String(w, "filter");
                w.Write(125u);
            }
            if (version >= 4)
            {
                w.Write(2); // Medal-score array independently uses version 2.
                w.Write(1);
                String(w, "medals");
                w.Write(126u);
                w.Write(127u);
            }
        });
        var node = new CGamePlayerScore();
        var chunk = new CGamePlayerScore.Chunk0308D007();

        await RoundTrip(Payload(w =>
        {
            w.Write(1);
            w.Write(payload);
        }), rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U01![0].Version).IsEqualTo(version);
        if (version >= 2)
        {
            await Assert.That(chunk.U01[0].FilteredScores![0].U02).IsEqualTo(125u);
        }
        if (version >= 4)
        {
            await Assert.That(chunk.U01[0].FilteredMedalsScores![0].U03).IsEqualTo(127u);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task LadderResultsOnlyStoreTheThreeBytesInTheNewChunk(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(0UL);
            w.Write(123.5f);
            if (version != 0)
            {
                w.Write((byte)0xFE);
                w.Write((byte)0x87);
                w.Write((byte)0x65);
            }
        });
        var node = new CGamePlayerScore();

        await RoundTrip(payload, rw =>
        {
            if (version == 0) new CGamePlayerScore.Chunk0308D00D().ReadWrite(node, rw);
            else new CGamePlayerScore.Chunk0308D012().ReadWrite(node, rw);
        });
        await Assert.That(node.LadderMatchResults![0].U02).IsEqualTo(123.5f);
        await Assert.That(node.LadderMatchResults[0].U03).IsEqualTo(version == 0 ? (byte)0 : (byte)0xFE);
    }

    [Test]
    public async Task MasterServerStatisticsKeepCurrentAndSubmittedValuesInterleaved()
    {
        var payload = Payload(w =>
        {
            for (var i = 0; i < 17; i++)
            {
                w.Write(0x80000000u + (uint)i);
                w.Write(0xFEDCBA00u + (uint)i);
            }
        });
        var node = new CGamePlayerScore();
        var chunk = new CGamePlayerScore.Chunk0308D011();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SoloRaceTimeSeconds).IsEqualTo(0x80000000u);
        await Assert.That(node.SubmittedSoloRaceTimeSeconds).IsEqualTo(0xFEDCBA00u);
        await Assert.That(node.MapEditorTimeSeconds).IsEqualTo(0x8000000Au);
        await Assert.That(node.SubmittedMapEditorTimeSeconds).IsEqualTo(0xFEDCBA0Au);
        await Assert.That(node.ResetCount).IsEqualTo(0x8000000Fu);
        await Assert.That(node.SubmittedFinishCount).IsEqualTo(0xFEDCBA10u);
    }

    [Test]
    [Arguments(0x00C, 1)]
    [Arguments(0x00E, 2)]
    [Arguments(0x00F, 0)]
    public async Task LegacyOfficialScoresKeepTheirTrailingTimestamps(int chunkId, int timestamps)
    {
        var payload = Payload(w =>
        {
            w.Write(-1); // Official scores node reference.
            w.Write(0); // Training medals.
            w.Write(10); // Campaign scores deprecated-array version.
            w.Write(0); // Campaign scores count.
            for (var i = 0; i < timestamps; i++) w.Write(0UL);
        });
        var node = new CGamePlayerScore();
        var chunk = chunkId switch
        {
            0x00C => (GBX.NET.Serialization.Chunking.Chunk<CGamePlayerScore>)new CGamePlayerScore.Chunk0308D00C(),
            0x00E => new CGamePlayerScore.Chunk0308D00E(),
            _ => new CGamePlayerScore.Chunk0308D00F()
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.GameVersion).IsEqualTo(chunkId == 0x00F ? GameVersion.TMF : GameVersion.Unspecified);
    }

    [Test]
    public async Task NativeWriterDefaultsAndMetadataAreApplied()
    {
        var node = new CGamePlayerScore();
        var score = new CGamePlayerScore.Score();

        await Assert.That(node.ScoresVersion).IsEqualTo(18);
        await Assert.That(node.SurvivalScoresVersion).IsEqualTo(2);
        await Assert.That(node.CampaignRecordsStateVersion).IsEqualTo((byte)1);
        await Assert.That(score.PersonalBest.TotalMilliseconds).IsEqualTo(-1);
        await Assert.That(score.PlatformBestResetCount).IsEqualTo(999u);
        await Assert.That(score.PlayMode).IsEqualTo(CGamePlayerScore.EChallengePlayModeMS.Unknown);
        await Assert.That(score.OfficialBestRecord).IsEqualTo(uint.MaxValue);
        await Assert.That(new CGamePlayerScore.Chunk0308D003().GameVersion).IsEqualTo(GameVersion.Unspecified);
        await Assert.That(new CGamePlayerScore.Chunk0308D004().GameVersion).IsEqualTo(GameVersion.TMF);
        await Assert.That(new CGamePlayerScore.Chunk0308D006().GameVersion).IsEqualTo(GameVersion.TMF);
        await Assert.That(new CGamePlayerScore.Chunk0308D010().GameVersion).IsEqualTo(GameVersion.TMF);
        await Assert.That(new CGamePlayerScore.Chunk0308D011().GameVersion).IsEqualTo(GameVersion.TMF);
        await Assert.That(new CGamePlayerScore.Chunk0308D012().GameVersion).IsEqualTo(GameVersion.TMF);
    }

    private static void String(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
