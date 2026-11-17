using GBX.NET;
using GBX.NET.Engines.Hms;
using System.Globalization;

namespace GbxCLI.Tests;

public class ValueFormatterTests
{
    [Test]
    public async Task Describe_ClassWithoutToString_UsesReadableTypeName()
    {
        await Assert.That(ValueFormatter.Describe(new CHmsLightMapCache.Small())).IsEqualTo("CHmsLightMapCache.Small");
    }

    [Test]
    public async Task Describe_Vector_UsesInvariantCulture()
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");

        try
        {
            await Assert.That(ValueFormatter.Describe(new Vec3(0.5f, 1, 2.25f))).IsEqualTo("<0.5, 1, 2.25>");
            await Assert.That(CultureInfo.CurrentCulture.Name).IsEqualTo("cs-CZ");
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    [Test]
    public async Task Describe_EmptyValues_AreVisible()
    {
        await Assert.That(ValueFormatter.Describe(new Id(""))).IsEqualTo("\"\"");
        await Assert.That(ValueFormatter.Describe(Array.Empty<byte>())).IsEqualTo("0 bytes");
    }
}
