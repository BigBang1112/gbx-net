using GBX.NET.Engines.GameData;
using GBX.NET.Tests.Unit.Engines.Plug;

namespace GBX.NET.Tests.Unit.Engines.GameData;

[Category("Unit")]
public class CGameActionModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version16_ReadsBeamsAsVersion1()
    {
        var node = new CGameActionModel();
        var chunk = new CGameActionModel.Chunk2E008000();
        var payload = CPlugCustomBeamModelLayoutTests.Payload(w =>
        {
            w.Write(16);
            w.Write(5); // BeamVersion, ignored by native readers before chunk version 18.
            w.Write(1);
            CPlugCustomBeamModelLayoutTests.WriteBeam(w, version: 1);
            w.Write(1); // IsCustom
            CPlugCustomBeamModelLayoutTests.NewId(w, "Shoot");
            w.Write(2); // ActionType
            CPlugCustomBeamModelLayoutTests.NewId(w, "Gauge");
            w.Write(0); // ScriptParamsVersion
            w.Write(0); // ActionScriptEffectVersion
            w.Write(0); // ActionScriptEffects
            w.Write(0); // CustomBulletVersion
            w.Write(0); // Projectiles
            w.Write(0); // SoundBlockVersion
            w.Write(0); // ParticleBlockVersion
            w.Write(0); // LegacyText
            w.Write(-1); // Anim
            w.Write(-1); // Script
            w.Write(0); // IconRef
            w.Write(-1); // Icon
            w.Write(0); // CrossHairRef
            w.Write(-1); // CrossHair
            w.Write(0); // ParticleBlocks
            w.Write(0); // SoundBlocks
        });

        await CPlugCustomBeamModelLayoutTests.RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        var beam = node.Beams!.Single();
        await Assert.That(node.BeamVersion).IsEqualTo(5);
        await Assert.That(beam.FireSoundVolume).IsEqualTo(0.5f);
        await Assert.That(beam.ShowPlayerExplosion).IsTrue();
        await Assert.That(node.ActionType).IsEqualTo(2);
    }
}
