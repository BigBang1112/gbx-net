using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class EncapsulationTests
{
    private readonly MemoryStream ms;
    private readonly GbxReader reader;
    private readonly GbxWriter writer;
    private readonly GbxReaderWriter readerWriter;

    public EncapsulationTests()
    {
        ms = new MemoryStream();
        reader = new GbxReader(ms);
        writer = new GbxWriter(ms);
        readerWriter = new GbxReaderWriter(reader, writer);
    }

    [Fact]
    public void Constructor_WithNonNullReader_SetsEncapsulation()
    {
        var encapsulation = new Encapsulation(reader);
        Assert.Equal(encapsulation, reader.Encapsulation);
    }

    [Fact]
    public void Constructor_WithNonNullWriter_SetsEncapsulation()
    {
        var encapsulation = new Encapsulation(writer);
        Assert.Equal(encapsulation, writer.Encapsulation);
    }

    [Fact]
    public void Constructor_WithNonNullReaderWriter_SetsEncapsulations()
    {
        var encapsulation = new Encapsulation(readerWriter);
        Assert.Equal(encapsulation, reader.Encapsulation);
        Assert.Equal(encapsulation, writer.Encapsulation);
    }

    [Fact]
    public void Constructor_WithNullReader_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxReader)!));
    }

    [Fact]
    public void Constructor_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxWriter)!));
    }

    [Fact]
    public void Constructor_WithNullReaderWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxReaderWriter)!));
    }

    [Fact]
    public void Dispose_SetsReaderEncapsulationToNull()
    {
        var encapsulation = new Encapsulation(reader);
        encapsulation.Dispose();
        Assert.Null(reader.Encapsulation);
    }

    [Fact]
    public void Dispose_SetsWriterEncapsulationToNull()
    {
        var encapsulation = new Encapsulation(writer);
        encapsulation.Dispose();
        Assert.Null(writer.Encapsulation);
    }
}
