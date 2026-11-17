using GBX.NET.Engines.Game;

namespace GbxCLI.Tests;

public class ValueInspectorTests
{
    [Test]
    public async Task GetMembers_OnlyMarkedTextIsFormatted()
    {
        var map = new CGameCtnChallenge { MapName = "$f00Red", MapUid = "$uid" };

        var members = ValueInspector.GetMembers(map).ToDictionary(member => member.Name);

        await Assert.That(members["MapName"].IsFormatted).IsTrue();
        await Assert.That(members["MapUid"].IsFormatted).IsFalse();
    }
}
