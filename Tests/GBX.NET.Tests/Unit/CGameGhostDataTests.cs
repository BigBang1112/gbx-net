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
}
