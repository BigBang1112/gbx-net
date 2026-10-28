using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDayTimeLayoutTests
{
    [Test]
    public async Task ReadWrite_SunAndAtmosphereChunks_KeepNativeOrder()
    {
        var node = new CPlugDayTime();
        var powers = new CPlugDayTime.Chunk09181000();
        var colors = new CPlugDayTime.Chunk09181001();

        await RoundTrip(Payload(w =>
        {
            foreach (var value in new[] { 20000f, 40f, 0.5f, 4f, 0.25f }) w.Write(value);
        }), rw => powers.ReadWrite(node, rw));
        await RoundTrip(Payload(w =>
        {
            w.Write(1);
            foreach (var value in new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f }) w.Write(value);
        }), rw => colors.ReadWrite(node, rw));

        await Assert.That(node.HdrSunPower).IsEqualTo(20000f);
        await Assert.That(node.Atmo1Power).IsEqualTo(40f);
        await Assert.That(node.Atmo1ColorScale).IsEqualTo(0.5f);
        await Assert.That(node.Atmo2Power).IsEqualTo(4f);
        await Assert.That(node.Atmo2ColorScale).IsEqualTo(0.25f);
        await Assert.That(node.Atmo1Color).IsEqualTo(new Vec3(0.1f, 0.2f, 0.3f));
        await Assert.That(node.Atmo2Color).IsEqualTo(new Vec3(0.4f, 0.5f, 0.6f));
    }

    [Test]
    public async Task ReadWrite_CloudsX2Chunk_ReadsColorsBeforeScales()
    {
        var node = new CPlugDayTime();
        var chunk = new CPlugDayTime.Chunk09181004();

        await RoundTrip(Payload(w =>
        {
            foreach (var value in new[] { 0.1f, 0.2f, 0.3f, 0.7f, 0.8f, 0.9f, 2f, 3f }) w.Write(value);
        }), rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.CloudsX2MinRgb).IsEqualTo(new Vec3(0.1f, 0.2f, 0.3f));
        await Assert.That(node.CloudsX2MaxRgb).IsEqualTo(new Vec3(0.7f, 0.8f, 0.9f));
        await Assert.That(node.CloudsX2MinRgbX).IsEqualTo(2f);
        await Assert.That(node.CloudsX2MaxRgbX).IsEqualTo(3f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_CloudsChunk_ReadsVersionedFields(int version)
    {
        var node = new CPlugDayTime();
        var chunk = new CPlugDayTime.Chunk09181007();
        var payload = Payload(w =>
        {
            w.Write(version);
            foreach (var value in new[] { 0.1f, 0.2f, 0.3f, 0.7f, 0.8f, 0.9f }) w.Write(value);
            if (version >= 1)
                foreach (var value in new[] { 1f, 2f, 3f, 4f, 5f, 6f }) w.Write(value); // Power and scale per edge-light layer.
            if (version >= 2) w.Write(0.5f);
            if (version >= 3)
                foreach (var value in new[] { 0.01f, 0.02f, 0.03f, 0.04f, 0.05f, 0.06f, 7f, 8f }) w.Write(value);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.CloudsVortexMinRgb).IsEqualTo(new Vec3(0.1f, 0.2f, 0.3f));
        await Assert.That(node.CloudsVortexMaxRgb).IsEqualTo(new Vec3(0.7f, 0.8f, 0.9f));
        await Assert.That(node.CloudsEdgeLight0Power).IsEqualTo(version >= 1 ? 1f : 0f);
        await Assert.That(node.CloudsEdgeLight0Scale).IsEqualTo(version >= 1 ? 2f : 0f);
        await Assert.That(node.CloudsEdgeLight2Scale).IsEqualTo(version >= 1 ? 6f : 0f);
        await Assert.That(node.CloudsGodLightIntens).IsEqualTo(version >= 2 ? 0.5f : 0f);
        await Assert.That(node.CloudsMinRgb).IsEqualTo(version >= 3 ? new Vec3(0.01f, 0.02f, 0.03f) : default);
        await Assert.That(node.CloudsMaxRgb).IsEqualTo(version >= 3 ? new Vec3(0.04f, 0.05f, 0.06f) : new Vec3(1, 1, 1));
        await Assert.That(node.CloudsMinRgbX).IsEqualTo(version >= 3 ? 7f : 1f);
        await Assert.That(node.CloudsMaxRgbX).IsEqualTo(version >= 3 ? 8f : 1f);
    }

    [Test]
    public async Task ReadWrite_FogMatters_ReadIdDensityAndColor()
    {
        var node = new CPlugDayTime();
        var chunk = new CPlugDayTime.Chunk09181006();

        await RoundTrip(Payload(w =>
        {
            w.Write(1);
            w.Write(3); // Id table version.
            w.Write(0x40000000u); // New string ID.
            String(w, "Dust");
            w.Write(0.002f);
            w.Write(0.5f);
            w.Write(0.6f);
            w.Write(0.7f);
        }), rw => chunk.ReadWrite(node, rw));

        var matter = node.FogMatters!.Single();
        await Assert.That(matter.Id).IsEqualTo(new Id("Dust"));
        await Assert.That(matter.Density).IsEqualTo(0.002f);
        await Assert.That(matter.Color).IsEqualTo(new Vec3(0.5f, 0.6f, 0.7f));
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugDayTime();
        var matter = new CPlugDayTime.FogMatterId();

        await Assert.That(node.HdrSunPower).IsEqualTo(25000f);
        await Assert.That(node.Atmo1Power).IsEqualTo(50f);
        await Assert.That(node.Atmo1ColorScale).IsEqualTo(0.25f);
        await Assert.That(node.Atmo1Color).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.Atmo2Power).IsEqualTo(5f);
        await Assert.That(node.Atmo2ColorScale).IsEqualTo(0.125f);
        await Assert.That(node.Atmo2Color).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.SunThroughIntens).IsEqualTo(0.75f);
        await Assert.That(node.CloudsX2MaxRgb).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.CloudsX2MinRgbX).IsEqualTo(1f);
        await Assert.That(node.CloudsX2MaxRgbX).IsEqualTo(1f);
        await Assert.That(node.FxSmSelfIllumScaleGamePlay).IsEqualTo(1f);
        await Assert.That(node.CloudsVortexMaxRgb).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.CloudsMaxRgb).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.MoonHdrScale).IsEqualTo(1f);
        await Assert.That(matter.Density).IsEqualTo(0.001f);
        await Assert.That(matter.Color).IsEqualTo(new Vec3(1, 1, 1));
    }

    private static void String(BinaryWriter w, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        w.Write(bytes.Length);
        w.Write(bytes);
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
