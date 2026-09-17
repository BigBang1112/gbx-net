using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CGameGhostDataTests
{
    [Fact]
    public void Read_Version13VariableTimeStep_DoesNotReadExtraValueAfterStateTimes()
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

        Assert.Equal(13, data.Version);
        Assert.False(data.IsFixedTimeStep);
        Assert.Single(data.Samples);
        Assert.Equal(stream.Length, stream.Position);
    }

    [Fact]
    public void Read_UniformStateSize_ReadsFinalSampleAsStateBufferRemainder()
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

        Assert.Equal(73, normalSampleData.Length);
        Assert.Equal(92, finalSampleData.Length);

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

        var parsedFinalSample = Assert.IsType<CSceneVehicleCar.Sample>(data.Samples[1]);
        Assert.Single(parsedFinalSample.U35_1!);
        Assert.Equal(stream.Length, stream.Position);
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
