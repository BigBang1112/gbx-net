using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Hms;
using GBX.NET.Engines.Plug;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

#pragma warning disable CS0618 // Obsolete members are the subject of these tests.

[Category("Unit")]
public class CompatibilityAliasTests
{
    [Test]
    public async Task TriggerActionComponentAliases_UpdateVectors()
    {
        var node = new CPlugTriggerAction();

        node.U04 = 2f;
        node.U13 = 3f;
        node.U08 = (int)CPlugTriggerAction.EType.Mesh;

        await Assert.That(node.EndPos).IsEqualTo(new Vec3(0, 2, 0));
        await Assert.That(node.StartPos).IsEqualTo(new Vec3(0, 0, 3));
        await Assert.That(node.Type).IsEqualTo(CPlugTriggerAction.EType.Mesh);
    }

    [Test]
    public async Task LightMapFrameAliases_MapRenumberedFields()
    {
        var frame = new CHmsLightMapCache.SFrame();

        frame.U13 = 1.5f;
        frame.U21 = 2.5f;
        frame.U10 = 1;

        await Assert.That(frame.U01).IsEqualTo(1.5f);
        await Assert.That(frame.MaxHDR_HBasisScaled234.Y).IsEqualTo(2.5f);
        await Assert.That(frame.StoreLAmbient).IsTrue();
    }

    [Test]
    public async Task GhostSettingsScreenAlias_SplitsInt3()
    {
        var settings = new CGameCtnGhost.SettingsInfos { U10 = new Int3(1920, 1080, 5000) };

        await Assert.That(settings.ScreenWidth).IsEqualTo(1920);
        await Assert.That(settings.ScreenHeight).IsEqualTo(1080);
        await Assert.That(settings.Time).IsEqualTo(TimeInt32.FromMilliseconds(5000));
        await Assert.That(settings.U10).IsEqualTo(new Int3(1920, 1080, 5000));
    }

    [Test]
    public async Task MacroblockBlockSpawnShortAlias_PacksColorAndLightmapQuality()
    {
        var spawn = new CGameCtnMacroBlockInfo.BlockSpawn { U02 = 0x0203 };

        await Assert.That(spawn.Color).IsEqualTo((byte)3);
        await Assert.That(spawn.LightmapQuality).IsEqualTo((byte)2);
        await Assert.That(spawn.U02).IsEqualTo((short)0x0203);
    }

    [Test]
    public async Task HeaderUnknownByteAlias_MatchesMode()
    {
        var header = GbxHeaderBasic.Create(mode: GbxMode.Editor);

        await Assert.That(header.UnknownByte).IsEqualTo(GbxUnknownByte.E);
        await Assert.That((header with { UnknownByte = GbxUnknownByte.R }).Mode).IsEqualTo(GbxMode.Release);
    }
}
