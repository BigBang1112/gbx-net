using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CGameGhostDataTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ReadWrite_MissingMobilClass_EndsArchiveOnlyForNonzeroChunkVersion(int archiveVersion)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(uint.MaxValue);
            if (archiveVersion == 0)
            {
                writer.Write(true);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.WriteData([]);
                writer.Write(0);
            }
        }
        var payload = stream.ToArray();
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();

        data.ReadNew(reader, archiveVersion);

        Assert.Equal(uint.MaxValue, data.SavedMobilClassId);
        Assert.Empty(data.Samples);
        Assert.Equal(stream.Length, stream.Position);
        Assert.Equal(archiveVersion == 0 ? 28 : 4, stream.Length);
        using var output = new MemoryStream();
        using var outputWriter = new GbxWriter(output);
        data.WriteNew(outputWriter, archiveVersion);
        Assert.Equal(payload, output.ToArray());
    }

    [Theory]
    [InlineData(0, 16)]
    [InlineData(1, 0)]
    [InlineData(1, 16)]
    [InlineData(2, 0)]
    public void ReadWrite_EmptyFixedTimeStep_UsesChunkVersionForFirstStateTime(int archiveVersion, int stateVersion)
    {
        // Archive version 1 and state version 0 reproduce the validation replay's 32-byte stream.
        // Legacy archives omit the final uint32 even when the vehicle-state version is 16.
        var payload = WriteArchive(true, stateVersion, [], [], archiveVersion != 0 ? 0 : null);
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();

        data.ReadNew(reader, archiveVersion);

        Assert.Empty(data.Samples);
        Assert.Equal(stateVersion, data.Version);
        Assert.Equal(archiveVersion != 0 ? (TimeInt32?)TimeInt32.Zero : null, data.FirstSampleTime);
        Assert.Equal(archiveVersion != 0 ? 32 : 28, stream.Length);
        Assert.Equal(stream.Length, stream.Position);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        data.WriteNew(writer, archiveVersion);
        Assert.Equal(payload, output.ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ReadWrite_VariableTimeStep_UsesAbsoluteTimesForLegacyAndDeltasForVersionedArchives(int archiveVersion)
    {
        var sample = WriteSample(new CSceneVehicleCar.Sample(TimeInt32.Zero, []), version: 13);
        int[] encodedTimes = archiveVersion != 0 ? [1200, 50, 1150] : [1200, 1250, 2400];
        var payload = WriteArchive(false, 13, [sample, sample, sample], encodedTimes, null);
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();

        data.ReadNew(reader, archiveVersion);

        Assert.Equal([1200, 1250, 2400], data.Samples.Select(x => x.Time.TotalMilliseconds));
        Assert.Equal(stream.Length, stream.Position);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        data.WriteNew(writer, archiveVersion);
        Assert.Equal(payload, output.ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Read_FixedTimeStep_AddsArchivedFirstStateTime(int archiveVersion)
    {
        var sample = WriteSample(new CSceneVehicleCar.Sample(TimeInt32.Zero, []), version: 13);
        var payload = WriteArchive(true, 13, [sample, sample], [], archiveVersion != 0 ? 1500 : null);
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var data = new CGameGhost.Data();

        data.ReadNew(reader, archiveVersion);

        Assert.Equal(archiveVersion != 0 ? [1500, 1600] : new[] { 0, 100 },
            data.Samples.Select(x => x.Time.TotalMilliseconds));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Write_VariableTimeStep_UsesCurrentSampleTimes(int archiveVersion)
    {
        var data = new CGameGhost.Data
        {
            SavedMobilClassId = 0x0A02B000,
            SamplePeriod = new TimeInt32(100),
            Version = 13,
            Samples = [
                new CSceneVehicleCar.Sample(new TimeInt32(1200), []),
                new CSceneVehicleCar.Sample(new TimeInt32(1250), []),
                new CSceneVehicleCar.Sample(new TimeInt32(2400), [])
            ]
        };
        var sample = WriteSample(data.Samples[0], version: 13);
        int[] encodedTimes = archiveVersion != 0 ? [1200, 50, 1150] : [1200, 1250, 2400];
        var expected = WriteArchive(false, 13, [sample, sample, sample], encodedTimes, null);
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);

        data.WriteNew(writer, archiveVersion);

        Assert.Equal(expected, stream.ToArray());
    }

    [Fact]
    public void GetSampleLerp_FirstSampleTimeIsNonzero_InterpolatesWithinTheSampleTimeRange()
    {
        var data = new CGameGhost.Data
        {
            IsFixedTimeStep = true,
            SamplePeriod = new TimeInt32(100),
            Samples = [
                new CSceneVehicleCar.Sample(new TimeInt32(1500), []) { Position = new Vec3(0, 0, 0) },
                new CSceneVehicleCar.Sample(new TimeInt32(1600), []) { Position = new Vec3(10, 0, 0) }
            ]
        };

        var interpolated = data.GetSampleLerp(TimeSingle.FromMilliseconds(1550));

        Assert.NotNull(interpolated);
        Assert.Equal(new Vec3(5, 0, 0), interpolated.Position);
        Assert.Same(data.Samples[0], data.GetSampleLerp(TimeSingle.FromMilliseconds(1500)));
        Assert.Null(data.GetSampleLerp(TimeSingle.FromMilliseconds(1499)));
    }

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
