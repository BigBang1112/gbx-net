using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnDecorationMoodTests
{
    [Test]
    [Arguments(0f, false)]
    [Arguments(0.25f, false)]
    [Arguments(0.5f, true)]
    [Arguments(0.75f, false)]
    [Arguments(1f, false)]
    public async Task IsNight_WithoutExplicitValue_UsesDayTimeUntilAssigned(float dayTime, bool expected)
    {
        var mood = new CGameCtnDecorationMood { RemappedStartDayTime = dayTime };

        await Assert.That(mood.IsNight).IsEqualTo(expected);

        mood.IsNight = !expected;

        await Assert.That(mood.IsNight).IsEqualTo(!expected);
    }
}
