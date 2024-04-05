using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace GBX.NET.Components;

public sealed class GbxRefTable
{
    public int AncestorLevel { get; set; }

    /// <summary>
    /// Directories in the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableDirectory> Directories { get; internal set; } = ImmutableList<ImmutableGbxRefTableDirectory>.Empty;

    /// <summary>
    /// Files in the root directory of the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableFile> Files { get; internal set; } = ImmutableList<ImmutableGbxRefTableFile>.Empty;

    /// <summary>
    /// Resources in the reference table at point when the Gbx was read. This could be different from reality if the nodes were modified.
    /// </summary>
    public ImmutableList<ImmutableGbxRefTableResource> Resources { get; internal set; } = ImmutableList<ImmutableGbxRefTableResource>.Empty;

    public Dictionary<GbxRefTableFile, byte[]> ExternalData { get; } = [];

    /// <summary>
    /// Directory path of the Gbx file used to resolve external files via the file system. File system is not used if this is null, instead, <see cref="ExternalData"/> is used.
    /// </summary>
    public string? FileSystemPath { get; init; }

    private string GetFilePath(GbxRefTableFile file)
    {
        var ancestor = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, AncestorLevel));
        return Path.Combine(FileSystemPath, ancestor, file.FilePath);
    }

    public string GetFullFilePath(GbxRefTableFile file)
    {
        return Path.GetFullPath(GetFilePath(file));
    }

    public CMwNod? LoadNode(GbxRefTableFile file, GbxReadSettings settings = default)
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
                return default;
            }
        }

        return default;
    }

    public T? LoadNode<T>(GbxRefTableFile file, GbxReadSettings settings = default) where T : CMwNod
    {
        return LoadNode(file, settings) as T;
    }

    internal static GbxRefTable? Parse(GbxReader reader, GbxHeader header, string? fileSystemPath, GbxReadSettings settings)
    {
        return new GbxRefTableReader(reader, header, fileSystemPath, settings).Parse();
    }

    internal bool Write(GbxWriter writer, GbxHeader header, GbxWriteSettings settings)
    {
        return new GbxRefTableWriter(this, header, writer, settings).Write();
    }

    public GbxRefTable DeepClone()
    {
        return new GbxRefTable(); // WRONG, TODO
    }
}