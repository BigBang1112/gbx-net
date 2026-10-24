using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCustomBeamModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version15_KeepsNativeOrderAndInlineCurve()
    {
        var beam = new CPlugCustomBeamModel();

        await RoundTrip(BeamPayload(15), rw => beam.ReadWrite(rw, 15));

        await Assert.That(beam.BulletName).IsEqualTo("Laser");
        await Assert.That(beam.FireSound).IsEqualTo("Fire");
        await Assert.That(beam.ReloadingSound).IsEqualTo("Reload");
        await Assert.That(beam.ExplosionSound).IsEqualTo("Boom");
        await Assert.That(beam.FireSoundVolume).IsEqualTo(0.5f);
        await Assert.That(beam.ReloadingSoundVolume).IsEqualTo(0.6f);
        await Assert.That(beam.ExplosionSoundVolume).IsEqualTo(0.7f);
        await Assert.That(beam.TriLaserRadiusAtOneMeter_Zoom0).IsEqualTo(0.01f);
        await Assert.That(beam.TriLaserRadiusAtOneMeter_Zoom1).IsEqualTo(0.02f);
        await Assert.That(beam.LaserDamage).IsEqualTo(100);
        await Assert.That(beam.BeamType).IsEqualTo(CPlugCustomBeamModel.ECustomBeamType.Slash);
        await Assert.That(beam.DamageAttenuationFromDist!.Values).IsEquivalentTo(new[] { new Vec2(0, 1), new Vec2(10, 0.5f) }, CollectionOrdering.Matching);
        await Assert.That(beam.MaxDistance).IsEqualTo(80f);
        await Assert.That(beam.LaserRadius).IsEqualTo(0.3f);
        await Assert.That(beam.BlowRadius).IsEqualTo(4f);
        await Assert.That(beam.BlowValue).IsEqualTo(5f);
        await Assert.That(beam.LaserShowAdvancedCrosshair).IsFalse();
    }

    [Test]
    public async Task ReadWrite_Version14_ReadsLegacyCurveReference()
    {
        var beam = new CPlugCustomBeamModel();

        await RoundTrip(BeamPayload(14), rw => beam.ReadWrite(rw, 14));

        await Assert.That(beam.DamageAttenuationFromDist!.Values ?? []).IsEmpty(); // A null legacy reference reads as an empty curve.
        await Assert.That(beam.BlowValue).IsEqualTo(5f);
        await Assert.That(beam.LaserShowAdvancedCrosshair).IsFalse();
    }

    [Test]
    public async Task ReadWrite_Version0_KeepsDefaultsForLaterFields()
    {
        var beam = new CPlugCustomBeamModel();

        await RoundTrip(BeamPayload(0), rw => beam.ReadWrite(rw, 0));

        await Assert.That(beam.ExplosionSound).IsEqualTo("Boom");
        await Assert.That(beam.FireSoundVolume).IsEqualTo(-1f);
        await Assert.That(beam.ShowPlayerExplosion).IsTrue();
        await Assert.That(beam.MaxDistance).IsEqualTo(50f);
        await Assert.That(beam.LaserShowAdvancedCrosshair).IsTrue();
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var beam = new CPlugCustomBeamModel();

        await Assert.That(beam.FireSoundVolume).IsEqualTo(-1f);
        await Assert.That(beam.ReloadingSoundVolume).IsEqualTo(-1f);
        await Assert.That(beam.ExplosionSoundVolume).IsEqualTo(-1f);
        await Assert.That(beam.ShowPlayerExplosion).IsTrue();
        await Assert.That(beam.U10).IsTrue();
        await Assert.That(beam.ModifyFOV).IsTrue();
        await Assert.That(beam.BeamType).IsEqualTo(CPlugCustomBeamModel.ECustomBeamType.Laser);
        await Assert.That(beam.MaxDistance).IsEqualTo(50f);
        await Assert.That(beam.LaserShowAdvancedCrosshair).IsTrue();
    }

    internal static void WriteBeam(BinaryWriter w, int version)
    {
        String(w, "Laser");
        w.Write(10); // Deprecated beam emitter list.
        w.Write(0);
        w.Write(10); // Deprecated particle emitter list.
        w.Write(0);
        w.Write(3); // Id table version.
        NewId(w, "Fire");
        NewId(w, "Reload");
        NewId(w, "Boom");
        if (version < 1) return;
        w.Write(0.5f);
        w.Write(0.6f);
        w.Write(0.7f);
        if (version < 2) return;
        w.Write(1);
        w.Write(0);
        if (version < 3) return;
        w.Write(1);
        if (version < 4) return;
        w.Write(0.01f);
        w.Write(0.02f);
        if (version < 5) return;
        w.Write(100);
        if (version < 6) return;
        w.Write(2f);
        if (version < 7) return;
        w.Write(0.1f);
        if (version < 8) return;
        w.Write(1f);
        w.Write(2f);
        w.Write(3f);
        w.Write((byte)2); // Slash
        if (version < 9) return;
        w.Write(1.5f);
        if (version < 10) return;
        w.Write(1);
        w.Write(0f);
        w.Write(0);
        if (version < 11) return;
        if (version < 15)
        {
            w.Write(-1); // Legacy curve reference.
        }
        else
        {
            w.Write(3); // Inline curve version.
            w.Write(2);
            w.Write((ushort)0);
            w.Write(0f);
            w.Write(1f);
            w.Write(10f);
            w.Write(0.5f);
        }
        if (version < 12) return;
        w.Write(80f);
        w.Write((byte)1);
        w.Write(0.3f);
        if (version < 13) return;
        w.Write(4f);
        w.Write(5f);
        if (version < 14) return;
        w.Write(0);
    }

    private static byte[] BeamPayload(int version) => Payload(w => WriteBeam(w, version));

    internal static void NewId(BinaryWriter w, string value)
    {
        w.Write(0x40000000u);
        String(w, value);
    }

    internal static void String(BinaryWriter w, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        w.Write(bytes.Length);
        w.Write(bytes);
    }

    internal static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    internal static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(0xDEADBEEFu);
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
