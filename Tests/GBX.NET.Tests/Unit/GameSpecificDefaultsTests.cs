using GBX.NET.Attributes;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class GameSpecificDefaultsTests
{
    [Test]
    public async Task LayoutDefaultsAreAvailableAsPropertyMetadata()
    {
        var expected = new (Type Type, string Member, GameVersion Game, object Value)[]
        {
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllBronzeValue), GameVersion.TM10, 100),
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllBronzeValue), GameVersion.TMPU, 100),
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllSilverValue), GameVersion.TM10, 200),
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllSilverValue), GameVersion.TMPU, 200),
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllGoldValue), GameVersion.TM10, 300),
            (typeof(CGameCtnChallengeGroup), nameof(CGameCtnChallengeGroup.AllGoldValue), GameVersion.TMPU, 300),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.ImageRadius), GameVersion.TMF, 0.024f),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.ImageRadius), GameVersion.MP3, 0.024f),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.ImageRadius), GameVersion.MP4, 0.024f),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.BlurPower), GameVersion.TMF, 3f),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.BlurPower), GameVersion.MP3, 3f),
            (typeof(CHmsAmbientOcc), nameof(CHmsAmbientOcc.BlurPower), GameVersion.MP4, 3f),
            (typeof(CHmsItem), nameof(CHmsItem.VisibleId), GameVersion.TMF, 0),
            (typeof(CHmsItem), nameof(CHmsItem.FlagsItem), GameVersion.TMF, 0xFFF1C00019800000UL),
            (typeof(CGameCtnChallengeGroup.Chunk0308F00B), nameof(IVersionable.Version), GameVersion.MP3, 0),
            (typeof(CGameCtnChallengeGroup.Chunk0308F00B), nameof(IVersionable.Version), GameVersion.TMT, 0),
            (typeof(CGameCtnMediaClip.Chunk0307900D), nameof(IVersionable.Version), GameVersion.MP4, 0)
        };

        foreach (var (type, member, game, value) in expected)
        {
            var attribute = type.GetProperty(member)!.GetCustomAttributes<GameVersionDefaultAttribute>().Single(x => x.Game == game);
            await Assert.That(attribute.DefaultValue).IsEqualTo(value);
            await Assert.That(attribute.DefaultExpression).IsNull();
        }

        foreach (var group in expected.GroupBy(x => (x.Type, x.Member)))
        {
            await Assert.That(group.Key.Type.GetProperty(group.Key.Member)!.GetCustomAttributes<GameVersionDefaultAttribute>().Count())
                .IsEqualTo(group.Count());
        }
        await Assert.That(typeof(CGameCtnChallengeGroup.Chunk0308F00B).GetCustomAttributes<GameVersionDefaultAttribute>()).IsEmpty();
    }

    [Test]
    [Arguments(GameVersion.TM10, 100)]
    [Arguments(GameVersion.TMPU, 100)]
    [Arguments(GameVersion.TM2020, 0)]
    [Arguments(GameVersion.Unspecified, 0)]
    [Arguments(GameVersion.TM10 | GameVersion.TMPU, 0)]
    public async Task MedalDefaultsRequireAnExactGameContext(GameVersion gameVersion, int bronze)
    {
        var node = new CGameCtnChallengeGroup(gameVersion);
        await Assert.That(node.AllBronzeValue).IsEqualTo(bronze);
        await Assert.That(node.AllSilverValue).IsEqualTo(bronze * 2);
        await Assert.That(node.AllGoldValue).IsEqualTo(bronze * 3);
        await Assert.That(new CGameCtnChallengeGroup().AllBronzeValue).IsEqualTo(0);
    }

    [Test]
    [Arguments(GameVersion.TMF, 0.024f, 3f)]
    [Arguments(GameVersion.MP3, 0.024f, 3f)]
    [Arguments(GameVersion.MP4, 0.024f, 3f)]
    [Arguments(GameVersion.TM2020, 0.1f, 1.5f)]
    [Arguments(GameVersion.Unspecified, 0.1f, 1.5f)]
    public async Task AmbientOcclusionDefaultsPreserveTheCurrentFallback(GameVersion gameVersion, float radius, float blur)
    {
        var node = new CHmsAmbientOcc(gameVersion);
        await Assert.That(node.ImageRadius).IsEqualTo(radius);
        await Assert.That(node.BlurPower).IsEqualTo(blur);
        await Assert.That(node.BlurTexelCount).IsEqualTo(15u);
        await Assert.That(new CHmsAmbientOcc().ImageRadius).IsEqualTo(0.1f);
    }

    [Test]
    [Arguments(GameVersion.TMF, 0xFFF1C00019800000UL, (ushort)0)]
    [Arguments(GameVersion.TM2020, 0xFFF1C00018800000UL, (ushort)1)]
    [Arguments(GameVersion.Unspecified, 0xFFF1C00018800000UL, (ushort)1)]
    public async Task ItemDefaultsSelectTheLegacyFlagsAndVisibleId(GameVersion gameVersion, ulong flags, ushort visibleId)
    {
        var node = new CHmsItem(gameVersion);
        await Assert.That(node.FlagsItem).IsEqualTo(flags);
        await Assert.That(node.VisibleId).IsEqualTo(visibleId);
        await Assert.That(new CHmsItem().FlagsItem).IsEqualTo(0xFFF1C00018800000UL);
    }

    [Test]
    [Arguments(GameVersion.MP3, 0)]
    [Arguments(GameVersion.TMT, 0)]
    [Arguments(GameVersion.MP4, 1)]
    [Arguments(GameVersion.TM2020, 1)]
    [Arguments(GameVersion.Unspecified, 1)]
    public async Task MapInfoChunkWritesTheSelectedVersion(GameVersion gameVersion, int version)
    {
        var chunk = new CGameCtnChallengeGroup.Chunk0308F00B(gameVersion);
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
            chunk.ReadWrite(new CGameCtnChallengeGroup(), rw);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadInt32()).IsEqualTo(version);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        await Assert.That(new CGameCtnChallengeGroup.Chunk0308F00B().Version).IsEqualTo(1);
    }

    [Test]
    public async Task ReadingAChunkReplacesItsGameSpecificVersionDefault()
    {
        var chunk = new CGameCtnChallengeGroup.Chunk0308F00B(GameVersion.MP3);
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(1);
            writer.Write(0);
            writer.Write(0xDEADBEEFu);
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        chunk.ReadWrite(new CGameCtnChallengeGroup(), rw);
        await Assert.That(chunk.Version).IsEqualTo(1);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
    }

    [Test]
    public async Task ClipVersionDefaultsKeepParameterlessConstructionCompatible()
    {
        await Assert.That(new CGameCtnMediaClip.Chunk0307900D(GameVersion.MP4).Version).IsEqualTo(0);
        await Assert.That(new CGameCtnMediaClip.Chunk0307900D(GameVersion.TM2020).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnMediaClip.Chunk0307900D().Version).IsEqualTo(1);
    }
}
