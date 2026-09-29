using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit.Components;

public class GbxHeaderTests
{
    [Test]
    public async Task Constructor_AssignsBasicProperty()
    {
        var basic = new GbxHeaderBasic();
        var header = new MockGbxHeader(basic);

        await Assert.That(header.Basic).IsEqualTo(basic);
    }
}
