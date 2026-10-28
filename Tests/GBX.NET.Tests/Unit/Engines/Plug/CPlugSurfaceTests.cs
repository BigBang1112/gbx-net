using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSurfaceTests
{
    [Test]
    [Arguments(11u)]
    [Arguments(uint.MaxValue)]
    public async Task ReadMultiSphere_CountAboveNativeLimit_ThrowsInvalidDataException(uint count)
    {
        // Arrange
        using var input = new MemoryStream(BitConverter.GetBytes(count));
        using var reader = new GbxReader(input);
        var surface = new CPlugSurface.MultiSphere();

        // Act
        var error = Assert.Throws<InvalidDataException>(() => surface.Read(reader));

        // Assert
        await Assert.That(error.Message).Contains("at most 10");
        await Assert.That(input.Position).IsEqualTo((long)sizeof(uint));
    }

    [Test]
    [Arguments(11)]
    [Arguments(100)]
    public async Task WriteMultiSphere_CountAboveNativeLimit_ThrowsBeforeWritingPayload(int count)
    {
        // Arrange
        var surface = CreateMultiSphere(count);
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);

        // Act
        var error = Assert.Throws<InvalidDataException>(() => surface.Write(writer));

        // Assert
        await Assert.That(error.Message).Contains("at most 10");
        await Assert.That(output.Length).IsEqualTo(0L);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(10)]
    public async Task ReadMultiSphere_CountWithinNativeLimit_ReadsSpheresAndSurfaceIndex(int count)
    {
        // Arrange
        using var input = new MemoryStream(CreatePayload(count));
        using var reader = new GbxReader(input);
        var surface = new CPlugSurface.MultiSphere();

        // Act
        surface.Read(reader);

        // Assert
        await GbxAssert.AreDeeplyEqual(CreateMultiSphere(count), surface);
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(10)]
    public async Task WriteMultiSphere_CountWithinNativeLimit_WritesNativePayload(int count)
    {
        // Arrange
        var surface = CreateMultiSphere(count);
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);

        // Act
        surface.Write(writer);

        // Assert
        await Assert.That(output.ToArray()).IsEquivalentTo(CreatePayload(count), CollectionOrdering.Matching);
    }

    private static CPlugSurface.MultiSphere CreateMultiSphere(int count) => new()
    {
        Spheres = Enumerable.Range(0, count)
            .Select(i => new CPlugSurface.MultiSphere.LocatedSphere(i + 0.5f, new Vec3(i, i + 1, i + 2)))
            .ToArray(),
        SurfaceIndex = 7
    };

    private static byte[] CreatePayload(int count)
    {
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output);
        writer.Write((uint)count);
        for (var i = 0; i < count; i++)
        {
            writer.Write(i + 0.5f);
            writer.Write((float)i);
            writer.Write((float)(i + 1));
            writer.Write((float)(i + 2));
        }
        writer.Write((short)7);
        return output.ToArray();
    }
}
