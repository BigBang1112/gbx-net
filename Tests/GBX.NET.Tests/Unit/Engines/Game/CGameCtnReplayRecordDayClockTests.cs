using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnReplayRecordDayClockTests
{
    [Test]
    public async Task Constructor_DayClock_UsesNativeDefaults()
    {
        var replay = new CGameCtnReplayRecord();

        await Assert.That(replay.TimeOfDay).IsEqualTo(-1f);
        await Assert.That(replay.DayPeriod).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    [Arguments(-1f, 0)]
    [Arguments(0.25f, 120000)]
    [Arguments(0.75f, 0)]
    public async Task Read_DayClock_ReadsTheStartTimeAndCyclePeriod(float timeOfDay, int dayPeriod)
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(0);
            writer.Write(timeOfDay);
            writer.Write(dayPeriod);
            writer.Write(0x12345678);
        }
        var bytes = input.ToArray();
        using var payload = new MemoryStream(bytes);
        var replay = new CGameCtnReplayRecord();
        var chunk = new CGameCtnReplayRecord.Chunk03093025();
        using (var reader = new GbxReader(payload))
        {
            chunk.Read(replay, reader);

            await Assert.That(replay.TimeOfDay).IsEqualTo(timeOfDay);
            await Assert.That(replay.DayPeriod).IsEqualTo(new TimeInt32(dayPeriod));
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }
    }

    [Test]
    [Arguments(1)]
    [Arguments(int.MaxValue)]
    public void Read_DayClock_RejectsUnsupportedVersionsBeforeReadingFields(int version)
    {
        using var input = new MemoryStream(BitConverter.GetBytes(version));
        using var reader = new GbxReader(input);
        var replay = new CGameCtnReplayRecord();
        var chunk = new CGameCtnReplayRecord.Chunk03093025();

        Assert.Throws<ChunkVersionNotSupportedException>(() => chunk.Read(replay, reader));
    }
}
