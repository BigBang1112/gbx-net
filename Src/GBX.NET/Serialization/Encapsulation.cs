namespace GBX.NET.Serialization;

internal sealed class Encapsulation : IDisposable
{
    private readonly GbxReader? reader;
    private readonly GbxWriter? writer;

    internal int? IdVersion { get; set; }

    private Dictionary<uint, string>? idReadDict;
    internal Dictionary<uint, string> IdReadDict => idReadDict ??= [];

    private Dictionary<string, int>? idWriteDict;
    internal Dictionary<string, int> IdWriteDict => idWriteDict ??= [];

    public Encapsulation(GbxReader reader)
    {
        this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        EncapsulateReader(reader);
    }

    public Encapsulation(GbxWriter writer)
    {
        this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        EncapsulateWriter(writer);
    }

    public Encapsulation(GbxReaderWriter rw)
    {
        _ = rw ?? throw new ArgumentNullException(nameof(rw));

        if (rw.Reader?.Encapsulation is not null)
        {
            throw new InvalidOperationException("Reader already has an encapsulation.");
        }

        if (rw.Writer?.Encapsulation is not null)
        {
            throw new InvalidOperationException("Writer already has an encapsulation.");
        }

        reader = rw.Reader;
        writer = rw.Writer;

        if (reader is not null)
        {
            EncapsulateReader(reader);
        }

        if (writer is not null)
        {
            EncapsulateWriter(writer);
        }
    }

    private void EncapsulateReader(GbxReader r)
    {
        if (r.Encapsulation is not null)
        {
            throw new InvalidOperationException("Reader already has an encapsulation.");
        }

        r.Encapsulation = this;
    }

    private void EncapsulateWriter(GbxWriter w)
    {
        if (w.Encapsulation is not null)
        {
            throw new InvalidOperationException("Writer already has an encapsulation.");
        }

        w.Encapsulation = this;
    }

    public void Dispose()
    {
        if (reader?.Encapsulation == this)
        {
            reader.Encapsulation = null;
        }

        if (writer?.Encapsulation == this)
        {
            writer.Encapsulation = null;
        }
    }
}
