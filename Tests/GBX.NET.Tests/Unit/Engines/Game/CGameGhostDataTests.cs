using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameGhostDataTests
{
    [Test]
    [Arguments(0, true)]
    [Arguments(0, false)]
    [Arguments(1, true)]
    [Arguments(1, false)]
    public async Task ReadWrite_Version6Samples_PreservesNetworkStatesAndTiming(int archiveVersion, bool fixedTimeStep)
    {
        byte[] first = [0x80, 0, 0, 0x80, 0, 0, 0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0x25, 0x81];
        byte[] second = [.. first[..15], 0xDA, 0xFE];
        var bytes = WriteArchive(fixedTimeStep, 6, [first, second], [100, 200],
            fixedTimeStep && archiveVersion >= 1 ? 50 : null);
        using var input = new MemoryStream(bytes);
        using var reader = new GbxReader(input);
        var data = new CGameGhost.Data();

        data.ReadNew(reader, archiveVersion);

        await Assert.That(data.Samples.Count).IsEqualTo(2);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        var firstSample = (CSceneVehicleCar.Sample)data.Samples[0];
        var secondSample = (CSceneVehicleCar.Sample)data.Samples[1];
        await Assert.That(firstSample.Horn).IsEqualTo((byte)2);
        await Assert.That(firstSample.FLIsSliding).IsTrue();
        await Assert.That(firstSample.RPM).IsBetween(8362f, 8363f);
        await Assert.That(secondSample.Horn).IsEqualTo((byte)1);
        await Assert.That(secondSample.FRIsSliding).IsTrue();
        await Assert.That(secondSample.RPM).IsBetween(10999f, 11001f);
        await Assert.That(data.Samples[0].Time.TotalMilliseconds).IsEqualTo(fixedTimeStep ? (archiveVersion >= 1 ? 50 : 0) : 100);
        await Assert.That(data.Samples[1].Time.TotalMilliseconds).IsEqualTo(fixedTimeStep ? (archiveVersion >= 1 ? 150 : 100) : (archiveVersion >= 1 ? 300 : 200));

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            data.WriteNew(writer, archiveVersion);
        }
        await Assert.That(output.ToArray()).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Read_Version13VariableTimeStep_DoesNotReadExtraValueAfterStateTimes()
    {
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, []);
        byte[] sampleData;

        using (var sampleStream = new MemoryStream())
        {
            using var sampleWriter = new GbxWriter(sampleStream);
            sample.Write(sampleWriter, version: 13);
            sampleData = sampleStream.ToArray();
        }

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(0x0A02B000u);
            writer.Write(false);
            writer.Write(0);
            writer.Write(new TimeInt32(100));
            writer.Write(13);
            writer.WriteData(sampleData);
            writer.Write(1);
            writer.Write(0);
            writer.WriteArray([100]);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();
        data.Read(reader, v: 1);

        await Assert.That(data.Version).IsEqualTo(13);
        await Assert.That(data.IsFixedTimeStep).IsFalse();
        await Assert.That(data.Samples).HasSingleItem();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task Read_UniformStateSize_ReadsFinalSampleAsStateBufferRemainder()
    {
        var normalSample = new CSceneVehicleCar.Sample(TimeInt32.Zero, []);
        var finalSample = new CSceneVehicleCar.Sample(TimeInt32.Zero, [])
        {
            U35_1 = [(new Vec3(1, 2, 3), new Quat(0, 0, 0, 1), 0xFF)]
        };
        var normalSampleData = WriteSample(normalSample, version: 16);
        var finalSampleData = WriteSample(finalSample, version: 16);
        var stateBuffer = new byte[normalSampleData.Length + finalSampleData.Length];

        normalSampleData.CopyTo(stateBuffer, 0);
        finalSampleData.CopyTo(stateBuffer, normalSampleData.Length);

        await Assert.That(normalSampleData.Length).IsEqualTo(73);
        await Assert.That(finalSampleData.Length).IsEqualTo(92);

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(0x0A02B000u);
            writer.Write(true);
            writer.Write(0);
            writer.Write(new TimeInt32(100));
            writer.Write(16);
            writer.WriteData(stateBuffer);
            writer.Write(2);
            writer.Write(0);
            writer.Write(normalSampleData.Length);
            writer.Write(0);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();
        data.Read(reader, v: 1);

        var parsedFinalSample = (await Assert.That(data.Samples[1]).IsTypeOf<CSceneVehicleCar.Sample>())!;
        await Assert.That(parsedFinalSample.U35_1!).HasSingleItem();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    private static byte[] WriteSample(CGameGhost.Data.Sample sample, int version)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            sample.Write(writer, version);
        }

        return stream.ToArray();
    }

    private static byte[] WriteArchive(bool fixedTimeStep, int stateVersion, byte[][] samples, int[] times, int? firstTime)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write(samples.Length == 0 ? 0u : 0x0A02B000u);
        writer.Write(fixedTimeStep);
        writer.Write(0);
        writer.Write(samples.Length == 0 ? 0 : 100);
        writer.Write(stateVersion);
        writer.WriteData(samples.SelectMany(x => x).ToArray());
        writer.Write(samples.Length);
        if (samples.Length > 0)
        {
            writer.Write(0);
            if (samples.Length > 1)
            {
                writer.Write(samples[0].Length);
            }
        }
        if (!fixedTimeStep)
        {
            writer.WriteArray(times);
        }
        if (firstTime is not null)
        {
            writer.Write(firstTime.Value);
        }
        return stream.ToArray();
    }
}
