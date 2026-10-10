using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class OptimizedArrayTests
{
    public static IEnumerable<(int DetermineFrom, int Width, bool HasLengthPrefix)> Widths()
    {
        foreach (var (determineFrom, width) in new[] { (254, 1), (255, 2), (65534, 2), (65535, 4), (65536, 4) })
        {
            yield return (determineFrom, width, false);
            yield return (determineFrom, width, true);
        }
    }

    [Test]
    [MethodDataSource(nameof(Widths))]
    public async Task IntegersUseTheExpectedWidthAndPreserveTheNextField(int determineFrom, int width, bool hasLengthPrefix)
    {
        int[] values = [0, 1, determineFrom - 1];
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteArrayOptimizedInt(values, determineFrom, hasLengthPrefix);
            writer.Write(0x12345678);
        }

        await Assert.That(stream.Length).IsEqualTo((long)((hasLengthPrefix ? 4 : 0) + values.Length * width + 4));
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var actual = hasLengthPrefix
            ? reader.ReadArrayOptimizedInt(determineFrom: determineFrom)
            : reader.ReadArrayOptimizedInt(values.Length, determineFrom);

        await Assert.That(actual).IsEquivalentTo(values, CollectionOrdering.Matching);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [MethodDataSource(nameof(Widths))]
    public async Task PairsUseTheExpectedWidthAndPreserveTheNextField(int determineFrom, int width, bool hasLengthPrefix)
    {
        Int2[] values = [new(0, determineFrom - 1), new(determineFrom - 1, 1)];
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteArrayOptimizedInt2(values, determineFrom, hasLengthPrefix);
            writer.Write(0x12345678);
        }

        await Assert.That(stream.Length).IsEqualTo((long)((hasLengthPrefix ? 4 : 0) + values.Length * width * 2 + 4));
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var actual = hasLengthPrefix
            ? reader.ReadArrayOptimizedInt2(determineFrom: determineFrom)
            : reader.ReadArrayOptimizedInt2(values.Length, determineFrom);

        await Assert.That(actual).IsEquivalentTo(values, CollectionOrdering.Matching);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task NullAndEmptyArraysHaveTheSameEncoding(bool hasLengthPrefix)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.WriteArrayOptimizedInt(null, hasLengthPrefix: hasLengthPrefix);
        writer.WriteArrayOptimizedInt([], hasLengthPrefix: hasLengthPrefix);
        writer.WriteArrayOptimizedInt2(null, hasLengthPrefix: hasLengthPrefix);
        writer.WriteArrayOptimizedInt2([], hasLengthPrefix: hasLengthPrefix);

        await Assert.That(stream.ToArray()).IsEquivalentTo(new byte[hasLengthPrefix ? 16 : 0], CollectionOrdering.Matching);
    }
}
