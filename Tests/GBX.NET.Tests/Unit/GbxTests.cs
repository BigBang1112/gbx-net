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

    [Fact]
    public void FileName_Throws()
    {
        Assert.Throws<NotImplementedException>(() => new Gbx(new GbxHeaderUnknown(new(), 0)).FileName);
    }

    [Fact]
    public void Clone_Throws()
    {
        Assert.Throws<NotImplementedException>(() => new Gbx(new GbxHeaderUnknown(new(), 0)).Clone());
    }

    [Fact]
    public void Save_Stream_Throws()
    {
        Assert.Throws<NotImplementedException>(() => new Gbx(new GbxHeaderUnknown(new(), 0)).Save(string.Empty));
    }

    [Fact]
    public void Save_FileName_Throws()
    {
        Assert.Throws<NotImplementedException>(() => new Gbx(new GbxHeaderUnknown(new(), 0)).Save(ms));
    }
}
