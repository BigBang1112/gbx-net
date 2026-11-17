using GBX.NET.Engines.Game;

namespace GbxCLI.Tests;

public class ValueInspectorTests
{
    private const string Computed = "(computed; inspect with gbx eval)";

    [Test]
    public async Task GetMembers_OnlyMarkedTextIsFormatted()
    {
        var map = new CGameCtnChallenge { MapName = "$f00Red", MapUid = "$uid" };

        var members = ValueInspector.GetMembers(map).ToDictionary(member => member.Name);

        await Assert.That(members["MapName"].IsFormatted).IsTrue();
        await Assert.That(members["MapUid"].IsFormatted).IsFalse();
    }

    [Test]
    public async Task GetMembers_WithExternalReferences_SkipsComputedProperties()
    {
        var members = ValueInspector.GetMembers(new CGameCtnChallenge()).ToDictionary(member => member.Name);

        await Assert.That(members["NbBlocks"].Value).IsEqualTo(Computed);
    }

    [Test]
    public async Task GetMembers_WithoutExternalReferences_ReadsComputedProperties()
    {
        var map = new CGameCtnChallenge { Blocks = [new CGameCtnBlock()] };

        var members = ValueInspector.GetMembers(map, hasExternalReferences: false).ToDictionary(member => member.Name);

        await Assert.That(members["NbBlocks"].Value).IsEqualTo(1);
    }

    [Test]
    public async Task GetMembers_WithExternalReferences_ReadsAutoProperties()
    {
        var item = new CGameCtnAnchoredObject { SnappedOnItem = new CGameCtnAnchoredObject() };

        var members = ValueInspector.GetMembers(item).ToDictionary(member => member.Name);

        await Assert.That(members["SnappedOnItem"].Value).IsSameReferenceAs(item.SnappedOnItem);
    }
}
