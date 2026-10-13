using Microsoft.Extensions.Logging;
#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
#endif

namespace GBX.NET.Components;

public sealed class GbxRefTable : IDeepCloneable
{
    private GbxReadSettings readSettings;
    public GbxReadSettings ReadSettings { get => readSettings; init => readSettings = value; }
    internal NodeRefLoadContext? LoadContext { get; set; }
    internal bool InferAncestorLevelOnWrite { get; set; }
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
    public Dictionary<string, Func<CancellationToken, Task<Gbx>>> ExternalNodesAsync { get; set; } = [];

    internal CMwNod? ResolveNode(GbxRefTableFile file)
    {
        if (FileSystemPath is null)
        {
            if (!ExternalNodes.ContainsKey(file.FilePath) && ExternalNodesAsync.ContainsKey(file.FilePath))
            {
                throw new NotSupportedException("This reference has an asynchronous resolver. Use ResolveAsync to load it.");
            }

            return ExternalNodes.TryGetValue(file.FilePath, out var resolve) ? LoadedNode(resolve()) : null;
        }

        var path = FindFilePath(file);

        var ancestor = LoadContext?.FindAncestor(path);
        if (ancestor is not null)
        {
            return ancestor;
        }

        using var contextScope = LoadContext?.Enter();
        using var stream = OpenReference(path, useAsync: false);

        return stream is null ? null : LoadedNode(Gbx.Parse(stream, ReadSettings));
    }

    internal async Task<CMwNod?> ResolveNodeAsync(GbxRefTableFile file)
    {
        if (FileSystemPath is null)
        {
            if (ExternalNodesAsync.TryGetValue(file.FilePath, out var resolve))
            {
                return LoadedNode(await resolve(CancellationToken.None).ConfigureAwait(false));
            }

            if (ExternalNodes.ContainsKey(file.FilePath))
            {
                throw new NotSupportedException("This reference has a synchronous resolver. Configure ExternalNodesAsync for asynchronous access.");
            }

            return null;
        }

        var path = FindFilePath(file);

        var ancestor = LoadContext?.FindAncestor(path);
        if (ancestor is not null)
        {
            return ancestor;
        }

        using var contextScope = LoadContext?.Enter();
        using var stream = OpenReference(path, useAsync: true);

        return stream is null ? null : LoadedNode(await Gbx.ParseAsync(stream, ReadSettings).ConfigureAwait(false));
    }

    private static FileStream? OpenReference(string path, bool useAsync)
    {
        try
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }
    }

    private CMwNod LoadedNode(Gbx gbx)
    {
        var node = gbx.Node ?? throw new NotSupportedException($"Cannot resolve Gbx class 0x{gbx.Header.ClassId:X8}.");
        
        if (gbx.RefTable is not null)
        {
            gbx.RefTable.readSettings = ReadSettings;

            if (gbx.RefTable.FileSystemPath is null)
            {
                gbx.RefTable.ExternalNodes = ExternalNodes;
                gbx.RefTable.ExternalNodesAsync = ExternalNodesAsync;
            }
        }

        return node;
    }

    private string FindFilePath(GbxRefTableFile file)
    {
        var path = GetFilePath(file);

        if (Path.DirectorySeparatorChar == '\\' || File.Exists(path))
        {
            return path;
        }

        var directory = Path.GetDirectoryName(path);

        if (string.IsNullOrEmpty(directory))
        {
            return path;
        }

        try
        {
            return Directory.EnumerateFiles(directory).FirstOrDefault(candidate =>
                string.Equals(Path.GetFileName(candidate), Path.GetFileName(path), StringComparison.OrdinalIgnoreCase)) ?? path;
        }
        catch (DirectoryNotFoundException)
        {
            return path;
        }
    }

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

    public string GetFilePath(GbxRefTableFile file) => GetFilePath(file.FilePath);
    public string GetFilePath(UnlinkedGbxRefTableFile file) => GetFilePath(file.FilePath);

    public string GetFullFilePath(GbxRefTableFile file) => Path.GetFullPath(GetFilePath(file));
    public string GetFullFilePath(UnlinkedGbxRefTableFile file) => Path.GetFullPath(GetFilePath(file));

    /// <summary>Copies a descriptor into this table, rebasing its path without modifying the source.</summary>
    public GbxRefTableFile ImportFile(GbxRefTableFile file)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (AncestorLevel < 0 || file.RefTable.AncestorLevel < 0)
        {
            throw new InvalidOperationException("Ancestor levels cannot be negative.");
        }

        if ((file.Flags & 4) != 0)
        {
            throw new InvalidOperationException("A file descriptor cannot have the resource flag.");
        }

        var path = file.FilePath;
        if (!ReferenceEquals(file.RefTable, this))
        {
            if (file.RefTable.FileSystemPath is not null && FileSystemPath is not null)
            {
                var destination = Path.GetFullPath(GetFilePath("."));
                var target = file.GetFullPath();
#if NETSTANDARD2_0
                var directoryUri = new Uri(destination.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
                path = Uri.UnescapeDataString(directoryUri.MakeRelativeUri(new Uri(target)).ToString()).Replace('/', Path.DirectorySeparatorChar);
#else
                path = Path.GetRelativePath(destination, target);
#endif
            }
            else if (file.RefTable.AncestorLevel != AncestorLevel || file.RefTable.FileSystemPath is not null)
            {
                throw new InvalidOperationException("Importing this file requires a destination file-system path for rebasing.");
            }
        }

        return new(this, file.Flags, file.UseFile, path);
    }

    public GbxRefTableResource ImportResource(GbxRefTableResource resource)
    {
        if (resource is null)
        {
            throw new ArgumentNullException(nameof(resource));
        }

        if ((resource.Flags & 4) == 0) 
        {
            throw new InvalidOperationException("A resource descriptor must have the resource flag.");
        }
        
        return new(this, resource.Flags, resource.UseFile, resource.ResourceIndex);
    }

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
#if NET5_0_OR_GREATER
                if (OperatingSystem.IsWindows())
                {
                    logger?.LogWarning("File not found: {FilePath}", filePath);
                    return default;
                }
#endif

                var directory = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                {
                    logger?.LogWarning("Directory not found: {Directory}", directory);
                    logger?.LogWarning("File not found: {FilePath}", filePath);
                    return default;
                }

                var fileName = Path.GetFileName(filePath);

                filePath = Directory.EnumerateFiles(directory)
                    .FirstOrDefault(f => string.Equals(Path.GetFileName(f), fileName, StringComparison.OrdinalIgnoreCase));

                if (filePath is null)
                {
                    logger?.LogWarning("File not found in directory {Directory}: {FileName}", directory, fileName);
                    return default;
                }
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
            FileSystemPath = FileSystemPath,
            ReadSettings = ReadSettings
        };

        foreach (var pair in ExternalNodes)
        {
            refTable.ExternalNodes.Add(pair.Key, pair.Value);
        }

        foreach (var pair in ExternalNodesAsync)
        {
            refTable.ExternalNodesAsync.Add(pair.Key, pair.Value);
        }

        return refTable;
    }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
#pragma warning disable GBXNET10001
        var clone = DeepClone();
#pragma warning restore GBXNET10001
        context.Register(this, clone);
        return clone;
    }
}
