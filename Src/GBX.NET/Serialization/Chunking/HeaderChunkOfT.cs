using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A data chunk located in the header part. Associated with <typeparamref name="T"/>, supports separate reading and writing.
/// </summary>
public interface IHeaderChunk<T> : IReadableChunk<T>, IWritableChunk<T>, IHeaderChunk where T : IClass;

/// <summary>
/// A data chunk located in the header part. Associated with <typeparamref name="T"/>, supports separate and joined reading and writing.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HeaderChunk<T> : Chunk<T>, IHeaderChunk<T> where T : IClass
{
    /// <inheritdoc />
    public bool IsHeavy { get; set; }

    /// <inheritdoc />
    public override void Read(T n, GbxReader r)
    {
        throw new NotImplementedException($"Header chunk 0x{Id:X8} ({ClassManager.GetName(Id & 0xFFFFF000)}, Read) is not implemented.");
    }

    /// <inheritdoc />
    public override void Write(T n, GbxWriter w)
    {
        throw new NotImplementedException($"Header chunk 0x{Id:X8} ({ClassManager.GetName(Id & 0xFFFFF000)}, Write) is not implemented.");
    }

    public override string ToString() => $"{ClassManager.GetName(Id & 0xFFFFF000)} header chunk 0x{Id:X8}{(this is IVersionable v ? $" [v{v.Version}]" : "")}";
}