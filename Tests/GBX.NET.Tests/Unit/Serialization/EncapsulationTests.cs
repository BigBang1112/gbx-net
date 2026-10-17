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
    public async Task Dispose_ReaderEncapsulation_ClearsReaderScope()
    {
        var encapsulation = new Encapsulation(reader);
        encapsulation.Dispose();
        await Assert.That(reader.Encapsulation).IsNull();
    }

    [Test]
    public async Task Dispose_WriterEncapsulation_ClearsWriterScope()
    {
        var encapsulation = new Encapsulation(writer);
        encapsulation.Dispose();
        await Assert.That(writer.Encapsulation).IsNull();
    }

    [Test]
    public async Task Dispose_ReaderAndWriter_ReleasesBothSidesAndAllowsAnotherScope()
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
    public async Task Constructor_OtherSideAlreadyEncapsulated_DoesNotAttachFreeSide()
    {
        using var existingScope = new Encapsulation(writer);

        Assert.Throws<InvalidOperationException>(() => new Encapsulation(readerWriter));

        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(writer.Encapsulation).IsEqualTo(existingScope);
    }

    [Test]
    public async Task Dispose_ExceptionInsideScope_RestoresBothSides()
    {
        Assert.Throws<IOException>(() =>
        {
            using var scope = new Encapsulation(readerWriter);
            throw new IOException("Interrupted payload");
        });

        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(writer.Encapsulation).IsNull();
    }

    [Test]
    public async Task ReadEncapsulated_NestedPayload_RestoresParentLookbackStrings()
    {
        writer.WriteIdAsString("Root");
        writer.WriteEncapsulated(outer =>
        {
            outer.WriteIdAsString("Outer");
            outer.WriteEncapsulated(inner =>
            {
                inner.WriteIdAsString("Inner");
                inner.WriteIdAsString("Inner");
            });
            outer.WriteIdAsString("Outer");
        });
        writer.WriteIdAsString("Root");
        ms.Position = 0;

        var strings = new List<string> { reader.ReadIdAsString() };
        reader.ReadEncapsulated(outer =>
        {
            strings.Add(outer.ReadIdAsString());
            outer.ReadEncapsulated(inner =>
            {
                strings.Add(inner.ReadIdAsString());
                strings.Add(inner.ReadIdAsString());
            });
            strings.Add(outer.ReadIdAsString());
        });
        strings.Add(reader.ReadIdAsString());

        await Assert.That(strings).IsEquivalentTo(new[] { "Root", "Outer", "Inner", "Inner", "Outer", "Root" }, CollectionOrdering.Matching);
        await Assert.That(reader.Encapsulation).IsNull();
        await Assert.That(ms.Position).IsEqualTo(ms.Length);
    }

    [Test]
    public async Task Dispose_NestedReaderException_RestoresParentScope()
    {
        using var parent = new Encapsulation(reader);
        parent.IdVersion = 3;
        parent.IdReadDict.Add(0, "Parent");

        Assert.Throws<IOException>(() =>
        {
            using var nested = new Encapsulation(reader, allowNested: true);
            nested.IdVersion = 2;
            nested.IdReadDict.Add(0, "Child");
            throw new IOException("Interrupted nested payload");
        });

        await Assert.That(reader.Encapsulation).IsSameReferenceAs(parent);
        await Assert.That(parent.IdVersion).IsEqualTo(3);
        await Assert.That(parent.IdReadDict[0]).IsEqualTo("Parent");
    }
}
