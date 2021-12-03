using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk<T> : Chunk, IChunk where T : CMwNod
{
    [IgnoreDataMember]
    public new T Node
    {
        get => (T)base.Node;
        internal set => base.Node = value;
    }

    public bool IsHeader => this is IHeaderChunk;
    public bool IsBody => !IsHeader;

    protected Chunk() : base(null!)
    {

    }

    protected Chunk(T node) : base(node)
    {

    }

    void IChunk.Read(CMwNod n, GameBoxReader r)
    {
        Read((T)n, r);
    }

    void IChunk.Write(CMwNod n, GameBoxWriter w)
    {
        Write((T)n, w);
    }

    void IChunk.ReadWrite(CMwNod n, GameBoxReaderWriter rw)
    {
        ReadWrite((T)n, rw);
    }

    public virtual void Read(T n, GameBoxReader r)
    {
        throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Read.");
    }

    public virtual void Write(T n, GameBoxWriter w)
    {
        throw new NotImplementedException($"Chunk 0x{ID & 0xFFF:x3} from class {Node.ClassName} doesn't support Write.");
    }

    public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
    {
        if (rw.Reader != null)
            Read(n, rw.Reader);
        else if (rw.Writer != null)
            Write(n, rw.Writer);
    }

    public byte[] ToByteArray()
    {
        if (this is ILookbackable l)
        {
            l.IdWritten = false;
            l.IdStrings.Clear();
        }

        using var ms = new MemoryStream();
        using var w = CreateWriter(ms);
        var rw = new GameBoxReaderWriter(w);
        ReadWrite(Node, rw);
        return ms.ToArray();
    }

    public override string ToString()
    {
        var desc = GetType().GetCustomAttribute<ChunkAttribute>()?.Description;
        var version = (this as IVersionable)?.Version;
        return $"{typeof(T).Name} chunk 0x{ID:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(version is null ? "" : $" [v{version}]")}";
    }
}

