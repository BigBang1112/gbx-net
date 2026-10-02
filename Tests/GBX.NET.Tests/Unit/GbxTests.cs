using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Tests.Unit;

public class GbxTests
{
    [Test]
    public async Task Magic_ReturnsGBX()
    {
        await Assert.That(Gbx.Magic).IsEqualTo("GBX");
    }
}
