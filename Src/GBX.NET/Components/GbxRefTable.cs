using Microsoft.Extensions.Logging;

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

    public Dictionary<GbxRefTableFile, byte[]> ExternalData { get; } = [];

    /// <summary>
    /// Directory path of the Gbx file used to resolve external files via the file system. File system is not used if this is null, instead, <see cref="ExternalData"/> is used.
    /// </summary>
    public string? FileSystemPath { get; init; }

    private string GetFilePath(GbxRefTableFile file)
    {
        var ancestor = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, AncestorLevel));
        
        return string.IsNullOrEmpty(FileSystemPath)
            ? Path.Combine(ancestor, file.FilePath)
            : Path.Combine(FileSystemPath, ancestor, file.FilePath);
    }

    public string GetFullFilePath(GbxRefTableFile file)
    {
        return Path.GetFullPath(GetFilePath(file));
    }

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

        if (ExternalData.TryGetValue(file, out var data))
        {
            using var ms = new MemoryStream(data);

            var scope = logger?.BeginScope("External [{Length} bytes]", data.Length);

            try
            {
                var gbx = Gbx.Parse(ms, settings);

                scope?.Dispose();

                if (gbx.Node is null)
                {
                    logger?.LogWarning("Failed to load node from External [{Length} bytes] ({Gbx})", data.Length, gbx);
                }

                return gbx.Node;
            }
            catch (Exception ex)
            {
                scope?.Dispose();
                logger?.LogError(ex, "Failed to load node from file: {Length} bytes", data.Length);

                if (exceptions)
                {
                    throw;
                }

                return default;
            }
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

    public GbxRefTable DeepClone()
    {
        var refTable = new GbxRefTable
        {
            AncestorLevel = AncestorLevel,
            Files = Files.Select(x => new UnlinkedGbxRefTableFile(x.Flags, x.UseFile, x.NodeIndex, x.FilePath)).ToList(),
            Resources = Resources.Select(x => new UnlinkedGbxRefTableResource(x.Flags, x.UseFile, x.NodeIndex, x.ResourceIndex)).ToList(),
            FileSystemPath = FileSystemPath
        };

        foreach (var pair in ExternalData)
        {
            // TODO: Deep clone file or not?
            var file = new GbxRefTableFile(refTable, pair.Key.Flags, pair.Key.UseFile, pair.Key.FilePath);
            refTable.ExternalData.Add(file, pair.Value.ToArray());
        }

        return refTable;
    }
}