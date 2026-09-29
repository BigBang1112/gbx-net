using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CGameGhostDataTests
{
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

    private static byte[] WriteSample(CSceneVehicleCar.Sample sample, int version)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            sample.Write(writer, version);
        }

        return stream.ToArray();
    }
}
