using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit.Components;

public class GbxHeaderTests
{
    [Fact]
    public void Constructor_AssignsBasicProperty()
    {
        var basic = new GbxHeaderBasic();
        var header = new MockGbxHeader(basic);

        Assert.Equal(basic, header.Basic);
    }
}
