using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Components;

public class GbxHeaderBasicTests
{
    [Test]
    public async Task Constructor_Parameters()
    {
        // Arrange & Act
        var basic = new GbxHeaderBasic(6, GbxFormat.Binary, GbxCompression.Uncompressed, GbxCompression.Compressed, GbxMode.Release);
        
        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)6);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.Mode).IsEqualTo(GbxMode.Release);
    }

    [Test]
    public async Task Create_UsesDefaults()
    {
        // Arrange & Act
        var basic = GbxHeaderBasic.Create();

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)6);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.Mode).IsEqualTo(GbxMode.Release);
    }

    [Test]
    public async Task Parse_Stream_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);

        // Act
        var basic = GbxHeaderBasic.Parse(ms);

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)4);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Text);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.Mode).IsEqualTo(GbxMode.Editor);
        await Assert.That(ms.Position).IsEqualTo(9);
    }

    [Test]
    public async Task Parse_GbxReader_V4_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);
        using var reader = new GbxReader(ms);

        // Act
        var basic = GbxHeaderBasic.Parse(reader);

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)4);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Text);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.Mode).IsEqualTo(GbxMode.Editor);
    }

    [Test]
    public void Parse_GbxReader_WrongGbxMagic_Throws()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', 4, 0, (byte)'T', (byte)'C', (byte)'U', 69]);
        using var reader = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<NotAGbxException>(() => GbxHeaderBasic.Parse(reader));
    }

    [Test]
    public async Task Write_Successful()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new GbxWriter(ms);

        var basic = GbxHeaderBasic.Create(4, GbxFormat.Text, GbxCompression.Compressed, GbxCompression.Uncompressed, GbxMode.Editor);

        // Act
        var result = basic.Write(writer);

        // Assert
        await Assert.That(ms.ToArray()).IsEquivalentTo((byte[])[(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E'], CollectionOrdering.Matching);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Version3UsesImplicitEditorModeWithoutConsumingAByte()
    {
        using var stream = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 3, 0, (byte)'B', (byte)'U', (byte)'U', 69]);
        using var reader = new GbxReader(stream);

        var basic = GbxHeaderBasic.Parse(reader);

        await Assert.That(basic.Mode).IsEqualTo(GbxMode.Editor);
        await Assert.That(reader.IsRelease).IsFalse();
        await Assert.That(stream.Position).IsEqualTo(8L);
        await Assert.That(reader.ReadByte()).IsEqualTo((byte)69);
    }

    [Test]
    [Arguments(GbxMode.Release)]
    [Arguments(GbxMode.Editor)]
    [Arguments(GbxMode.Unspecified)]
    public async Task WritingVersion3UsesEditorModeRegardlessOfTheUnencodedValue(GbxMode mode)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        var basic = GbxHeaderBasic.Create(version: 3, mode: mode);

        basic.Write(writer);

        await Assert.That(writer.IsRelease).IsFalse();
        await Assert.That(stream.ToArray()).IsEquivalentTo(
            (byte[])[(byte)'G', (byte)'B', (byte)'X', 3, 0, (byte)'B', (byte)'U', (byte)'C'],
            CollectionOrdering.Matching);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(GbxHeaderBasic.Parse(reader).Mode).IsEqualTo(GbxMode.Editor);
        await Assert.That(reader.IsRelease).IsFalse();
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task OnlyEditorAndReleaseBytesAreAccepted(int version)
    {
        for (var value = 0; value <= byte.MaxValue; value++)
        {
            var mode = (GbxMode)value;
            var isValid = mode is GbxMode.Editor or GbxMode.Release;
            using var input = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', (byte)version, 0, (byte)'B', (byte)'U', (byte)'U', (byte)mode, 69]);
            using var reader = new GbxReader(input);
            using var output = new MemoryStream();
            using var writer = new GbxWriter(output);
            var basic = GbxHeaderBasic.Create(version: (ushort)version, mode: mode);

            if (isValid)
            {
                await Assert.That(GbxHeaderBasic.Parse(reader).Mode).IsEqualTo(mode);
                await Assert.That(reader.IsRelease).IsEqualTo(mode == GbxMode.Release);
                basic.Write(writer);
                await Assert.That(writer.IsRelease).IsEqualTo(mode == GbxMode.Release);
                await Assert.That(output.ToArray()[8]).IsEqualTo((byte)mode);
            }
            else
            {
                Assert.Throws<InvalidDataException>(() => GbxHeaderBasic.Parse(reader));
                Assert.Throws<InvalidDataException>(() => basic.Write(writer));
                await Assert.That(output.Length).IsEqualTo(0L);
            }

            await Assert.That(input.Position).IsEqualTo(9L);
            await Assert.That(reader.ReadByte()).IsEqualTo((byte)69);
        }
    }

    [Test]
    [Arguments(GbxMode.Editor)]
    [Arguments(GbxMode.Release)]
    public async Task NestedReadersAndWritersPreserveHeaderMode(GbxMode mode)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        GbxHeaderBasic.Create(mode: mode).Write(writer);

        using var nestedOutput = new MemoryStream();
        using var nestedWriter = new GbxWriter(nestedOutput);
        nestedWriter.LoadFrom(writer);
        await Assert.That(nestedWriter.IsRelease).IsEqualTo(mode == GbxMode.Release);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        GbxHeaderBasic.Parse(reader);
        using var nestedInput = new MemoryStream();
        using var nestedReader = new GbxReader(nestedInput);
        nestedReader.LoadFrom(reader);
        await Assert.That(nestedReader.IsRelease).IsEqualTo(mode == GbxMode.Release);
    }
}
