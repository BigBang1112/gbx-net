using Microsoft.Extensions.Logging;
#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.Components;

public sealed class GbxRefTable
{
    public int AncestorLevel { get; set; }

    /// <summary>
    /// Files in the reference table, available for header-only preview and minimal reference table modifications. This won't be serialized back to Gbx, except if <see cref="GbxBody.RawData"/> is set (for example when <see cref="GbxReadSettings.ReadRawBody"/> was enabled).
    /// </summary>
    public IReadOnlyCollection<UnlinkedGbxRefTableFile> Files { get; internal set; } = [];

    /// <summary>
    /// Resources in the reference table, available for header-only preview and minimal reference table modifications. This won't be serialized back to Gbx, except if <see cref="GbxBody.RawData"/> is set (for example when <see cref="GbxReadSettings.ReadRawBody"/> was enabled).
    /// </summary>
    public IReadOnlyCollection<UnlinkedGbxRefTableResource> Resources { get; internal set; } = [];

    public Dictionary<string, Func<Gbx>> ExternalNodes { get; set; } = [];

    /// <summary>
    /// Directory path of the Gbx file used to resolve external files via the file system. File system is not used if this is null, instead, <see cref="ExternalNodes"/> is used.
    /// </summary>
    public string? FileSystemPath { get; init; }

    private string GetFilePath(string filePath)
    {
        var ancestor = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, AncestorLevel));
        
        return string.IsNullOrEmpty(FileSystemPath)
            ? Path.Combine(ancestor, filePath)
            : Path.Combine(FileSystemPath, ancestor, filePath);
    }

    private string GetFilePath(GbxRefTableFile file) => GetFilePath(file.FilePath);
    private string GetFilePath(UnlinkedGbxRefTableFile file) => GetFilePath(file.FilePath);

    public string GetFullFilePath(GbxRefTableFile file) => Path.GetFullPath(GetFilePath(file));
    public string GetFullFilePath(UnlinkedGbxRefTableFile file) => Path.GetFullPath(GetFilePath(file));

    // TODO: should also have async variant in the future
    public CMwNod? LoadNode(GbxRefTableFile file, GbxReadSettings settings = default, bool exceptions = false)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        var logger = settings.Logger;

        if (FileSystemPath is not null)
        {
            var filePath = GetFilePath(file);

            if (!File.Exists(filePath))
            {
                logger?.LogWarning("File not found: {FilePath}", filePath);
                return default;
            }

            logger?.LogDebug("Loading node from file: {FilePath}", filePath);

            var scope = logger?.BeginScope("{FilePath}", Path.GetFileName(file.FilePath));

            try
            {
                var gbx = Gbx.Parse(filePath, settings);

                scope?.Dispose();

                if (gbx.Node is null)
                {
                    logger?.LogWarning("Failed to load node from file: {FilePath} ({Gbx})", filePath, gbx);
                }

                return gbx.Node;
            }
            catch (Exception ex)
            {
                scope?.Dispose();
                logger?.LogError(ex, "Failed to load node from file: {FilePath}", filePath);

                if (exceptions)
                {
                    throw;
                }

                return default;
            }
        }

        if (ExternalNodes?.TryGetValue(file.FilePath, out var nodFunc) == true)
        {
            return nodFunc();
        }

        return default;
    }

    public T? LoadNode<T>(GbxRefTableFile file, GbxReadSettings settings = default, bool exceptions = false) where T : CMwNod
    {
        return LoadNode(file, settings, exceptions) as T;
    }

    internal static GbxRefTable? Parse(GbxReader reader, GbxHeader header, string? fileSystemPath)
    {
        return new GbxRefTableReader(reader, header, fileSystemPath).Parse();
    }

    internal bool Write(GbxWriter writer, GbxHeader header, bool rawBody)
    {
        return new GbxRefTableWriter(this, header, writer).Write(rawBody);
    }

#if NET8_0_OR_GREATER
    [Experimental("GBXNET10001")]
#endif
    public GbxRefTable DeepClone()
    {
        var refTable = new GbxRefTable
        {
            AncestorLevel = AncestorLevel,
            Files = Files.Select(x => new UnlinkedGbxRefTableFile(x.Flags, x.UseFile, x.NodeIndex, x.FilePath)).ToList(),
            Resources = Resources.Select(x => new UnlinkedGbxRefTableResource(x.Flags, x.UseFile, x.NodeIndex, x.ResourceIndex)).ToList(),
            FileSystemPath = FileSystemPath
        };

        foreach (var pair in ExternalNodes)
        {
            refTable.ExternalNodes.Add(pair.Key, pair.Value);
        }

        return refTable;
    }
}