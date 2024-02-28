using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Tests.Unit;

public class GbxTests
{
    private readonly MemoryStream ms;

    public GbxTests()
    {
        ms = new MemoryStream();
    }

    [Fact]
    public void Magic_ReturnsGBX()
    {
        Assert.Equal(expected: "GBX", actual: Gbx.Magic);
    }
}
