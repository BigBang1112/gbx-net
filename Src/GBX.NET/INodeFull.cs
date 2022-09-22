namespace GBX.NET;

public interface INodeFull
{
    uint Id { get; }
    ChunkSet Chunks { get; }
    
    /// <summary>
    /// Gets the <see cref="GameBox"/> object holding the main node.
    /// </summary>
    /// <returns>The holding <see cref="GameBox"/> object, if THIS node is the main node, otherwise null.</returns>
    GameBox? GetGbx();
    
    /// <summary>
    /// Saves the serialized node on a disk in a Gbx form.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value from <see cref="GBX"/> object instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    void Save(string? fileName = default, IDRemap remap = default, ILogger? logger = null);

    /// <summary>
    /// Saves the serialized node to a stream in a Gbx form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    void Save(Stream stream, IDRemap remap = default, ILogger? logger = null);

    /// <summary>
    /// Saves the serialized node on a disk in a Gbx form.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value from <see cref="GBX"/> object instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveAsync(string? fileName = default,
                   IDRemap remap = default,
                   ILogger? logger = null,
                   GameBoxAsyncWriteAction? asyncAction = null,
                   CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the serialized node to a stream in a Gbx form.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveAsync(Stream stream,
                   IDRemap remap = default,
                   ILogger? logger = null,
                   GameBoxAsyncWriteAction? asyncAction = null,
                   CancellationToken cancellationToken = default);
}
