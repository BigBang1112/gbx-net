using KellermanSoftware.CompareNetObjects;

namespace GBX.NET.Tests.Infrastructure;

internal static class GbxAssert
{
    public static async Task AreDeeplyEqual(object? expected, object? actual)
    {
        var comparison = new CompareLogic(new ComparisonConfig
        {
            MaxDifferences = 20,
            IgnoreCollectionOrder = false
        }).Compare(expected, actual);

        await Assert.That(comparison.AreEqual).IsTrue().Because(comparison.DifferencesString);
    }

    public static Task HaveEqualSerializedData(Gbx expected, Gbx actual) => AreDeeplyEqual(
        new { expected.Header, expected.RefTable, expected.Node },
        new { actual.Header, actual.RefTable, actual.Node });
}
