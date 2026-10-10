using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class EncapsulationTests : IDisposable
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

    public void Dispose()
    {
        readerWriter.Dispose();
        reader.Dispose();
        writer.Dispose();
        ms.Dispose();
    }

    [Test]
    public async Task Constructor_WithNonNullReader_SetsEncapsulation()
    {
        var encapsulation = new Encapsulation(reader);
        await Assert.That(reader.Encapsulation).IsEqualTo(encapsulation);
    }

    [Test]
    public async Task Constructor_WithNonNullWriter_SetsEncapsulation()
    {
        var encapsulation = new Encapsulation(writer);
        await Assert.That(writer.Encapsulation).IsEqualTo(encapsulation);
    }

    [Test]
    public async Task Constructor_WithNonNullReaderWriter_SetsEncapsulations()
    {
        var encapsulation = new Encapsulation(readerWriter);
        await Assert.That(reader.Encapsulation).IsEqualTo(encapsulation);
        await Assert.That(writer.Encapsulation).IsEqualTo(encapsulation);
    }

    [Test]
    public void Constructor_WithNullReader_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxReader)!));
    }

    [Test]
    public void Constructor_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxWriter)!));
    }

    [Test]
    public void Constructor_WithNullReaderWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Encapsulation(default(GbxReaderWriter)!));
    }

    [Test]
    public async Task Dispose_SetsReaderEncapsulationToNull()
    {
        var encapsulation = new Encapsulation(reader);
        encapsulation.Dispose();
        await Assert.That(reader.Encapsulation).IsNull();
    }

    [Test]
    public async Task Dispose_SetsWriterEncapsulationToNull()
    {
        var encapsulation = new Encapsulation(writer);
        encapsulation.Dispose();
        await Assert.That(writer.Encapsulation).IsNull();
    }

    [Test]
    public async Task Dispose_ReleasesBothSidesAndAllowsAnotherScope()
    {
        var encapsulation = new Encapsulation(readerWriter);
        encapsulation.Dispose();
        encapsulation.Dispose();

        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(writer.Encapsulation).IsNull();

        using var nextScope = new Encapsulation(readerWriter);
        await Assert.That(reader.Encapsulation).IsEqualTo(nextScope);
        await Assert.That(writer.Encapsulation).IsEqualTo(nextScope);
    }

    [Test]
    public async Task FailedConstructionDoesNotAttachTheOtherSide()
    {
        using var existingScope = new Encapsulation(writer);

        Assert.Throws<InvalidOperationException>(() => new Encapsulation(readerWriter));

        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(writer.Encapsulation).IsEqualTo(existingScope);
    }

    [Test]
    public async Task ExceptionInsideScopeRestoresBothSides()
    {
        Assert.Throws<IOException>(() =>
        {
            using var scope = new Encapsulation(readerWriter);
            throw new IOException("Interrupted payload");
        });

        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(writer.Encapsulation).IsNull();
    }
}
