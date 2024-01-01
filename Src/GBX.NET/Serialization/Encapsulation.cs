namespace GBX.NET.Serialization;

internal sealed class Encapsulation : IDisposable
{
    private readonly GbxReader? reader;
    private readonly GbxWriter? writer;

    internal int? IdVersion { get; set; }

    private Dictionary<int, string>? idReadDict;
    internal Dictionary<int, string> IdReadDict => idReadDict ??= [];

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

        if (rw.Reader is not null)
        {
            EncapsulateReader(rw.Reader);
        }

        if (rw.Writer is not null)
        {
            EncapsulateWriter(rw.Writer);
        }
    }

    private void EncapsulateReader(GbxReader reader)
    {
        if (reader.Encapsulation is not null)
        {
            throw new InvalidOperationException("Reader already has an encapsulation.");
        }

        reader.Encapsulation = this;
    }

    private void EncapsulateWriter(GbxWriter writer)
    {
        if (writer.Encapsulation is not null)
        {
            throw new InvalidOperationException("Writer already has an encapsulation.");
        }

        writer.Encapsulation = this;
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
