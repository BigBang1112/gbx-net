using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace GBX.NET;

/// <summary>
/// Writes data types from GameBox serialization.
/// </summary>
public class GameBoxWriter : BinaryWriter
{
    private readonly ILogger? logger;

    public GameBoxWriterSettings Settings { get; }

    /// <summary>
    /// Constructs a binary writer specialized for Gbx serializing.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="gbx">Gbx that holds node references and lookback strings while writing. If null, <see cref="Node"/>, <see cref="Id"/>, or <see cref="Ident"/> cannot be written and <see cref="PropertyNullException"/> will be thrown.</param>
    /// <param name="remap">Node ID remap mode.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="logger">Logger.</param>
    public GameBoxWriter(Stream output,
                         GameBox? gbx = null,
                         IDRemap? remap = null,
                         GameBoxAsyncWriteAction? asyncAction = null,
                         ILogger? logger = null) : base(output, Encoding.UTF8, true)
    {
        Settings = new GameBoxWriterSettings(gbx, remap.GetValueOrDefault(), asyncAction);

        this.logger = logger;
    }

    /// <summary>
    /// Constructs a binary writer specialized for Gbx serializing.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="settings">Settings for the writer.</param>
    /// <param name="logger">Logger.</param>
    public GameBoxWriter(Stream output, GameBoxWriterSettings settings, ILogger? logger = null) : base(output, Encoding.UTF8, true)
    {
        Settings = settings;

        this.logger = logger;
    }
    
    /// <summary>
    /// Writes a byte array to the underlying stream.
    /// </summary>
    /// <param name="buffer">A byte array containing the data to write.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override void Write(byte[]? buffer)
    {
        if (buffer is not null)
        {
            base.Write(buffer);
        }
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(string? value, StringLengthPrefix lengthPrefix)
    {
        var length = value is null ? 0 : Encoding.UTF8.GetByteCount(value);

        switch (lengthPrefix)
        {
            case StringLengthPrefix.Byte:
                Write((byte)length);
                break;
            case StringLengthPrefix.Int32:
                Write(length);
                break;
        }

        if (value is not null)
        {
            Write(Encoding.UTF8.GetBytes(value));
        }
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override void Write(string? value)
    {
        Write(value, StringLengthPrefix.Int32);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(bool value, bool asByte)
    {
        if (asByte)
        {
            base.Write(value);
            return;
        }

        Write(Convert.ToInt32(value));
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override void Write(bool value)
    {
        Write(value, false);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Vec2 value)
    {
        Write(value.X);
        Write(value.Y);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Vec3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Vec4 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.W);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Quat value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.W);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Rect value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.X2);
        Write(value.Y2);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Box value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
        Write(value.X2);
        Write(value.Y2);
        Write(value.Z2);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Iso4 value)
    {
        Write(value.XX);
        Write(value.XY);
        Write(value.XZ);
        Write(value.YX);
        Write(value.YY);
        Write(value.YZ);
        Write(value.ZX);
        Write(value.ZY);
        Write(value.ZZ);
        Write(value.TX);
        Write(value.TY);
        Write(value.TZ);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Int3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Int2 value)
    {
        Write(value.X);
        Write(value.Y);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Byte3 value)
    {
        Write(value.X);
        Write(value.Y);
        Write(value.Z);
    }

    public void WriteTimeOfDay(TimeSpan? timeOfDay)
    {
        if (timeOfDay is null)
        {
            Write(-1);
            return;
        }

        var maxTime = TimeSpan.FromDays(1) - TimeSpan.FromTicks(1);
        var maxSecs = maxTime.TotalSeconds;
        var secs = timeOfDay.Value.TotalSeconds % maxTime.TotalSeconds;

        Write((int)(secs / maxSecs * ushort.MaxValue));
    }

    /// <exception cref="ArgumentNullException"><paramref name="fileRef"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(FileRef fileRef)
    {
        if (fileRef is null)
        {
            throw new ArgumentNullException(nameof(fileRef));
        }

        Write(fileRef.Version);

        if (fileRef.Version >= 3)
        {
            Write(fileRef.Checksum ?? FileRef.DefaultChecksum);
        }

        Write(fileRef.FilePath);

        if (fileRef.FilePath is not null
            && ((fileRef.FilePath.Length > 0 && fileRef.Version >= 1)
                || fileRef.Version >= 3))
        {
            Write(fileRef.LocatorUrl);
        }
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxWriterSettings.Gbx"/> is null.</exception>
    public void WriteId(string? value, bool tryParseToInt32 = false)
    {
        var gbx = Settings.GetGbxOrThrow();

        WriteIdVersionIfNotWritten(gbx);
        WriteIdAsString(value ?? "", gbx, tryParseToInt32);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteId(Id value)
    {
        var gbx = Settings.GetGbxOrThrow();

        WriteIdVersionIfNotWritten(gbx);

        if (value.Index.HasValue)
        {
            Write(value.Index.Value);
            return;
        }

        WriteIdAsString(value, gbx, tryParseToInt32: false); // Could be true
    }

    private void WriteIdVersionIfNotWritten(GameBox gbx)
    {
        if (gbx.IdIsWritten)
        {
            return;
        }

        Write(gbx.IdVersion ?? 3);
        gbx.IdIsWritten = true;
    }

    private void WriteIdAsString(string value, GameBox gbx, bool tryParseToInt32)
    {
        if (value == "")
        {
            Write(0xFFFFFFFF);
            return;
        }

        if (value == "Unassigned")
        {
            Write(0xBFFFFFFF);
            return;
        }

        var index = gbx.IdStringsInWriteMode.IndexOf(value);

        if (index != -1)
        {
            Write(index + 1 + 0x40000000);
            return;
        }

        if (tryParseToInt32 && int.TryParse(value, out index))
        {
            Write(index);
            return;
        }

        Write(0x40000000);
        Write(value);

        gbx.IdStringsInWriteMode.Add(value);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Ident? ident)
    {
        ident ??= Ident.Empty;

        WriteId(ident.Id);
        WriteId(ident.Collection);
        WriteId(ident.Author);
    }

    /// <exception cref="PropertyNullException"><see cref="GameBoxWriterSettings.Gbx"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Node? node, GameBoxRefTable.File? nodeFile = null)
    {
        var gbx = Settings.GetGbxOrThrow();

        if (nodeFile is not null)
        {
            var nodeFileIndex = nodeFile.NodeIndex + 1;

            while (gbx.AuxNodesInWriteMode.ContainsKey(nodeFileIndex))
            {
                nodeFileIndex++;
            }

            nodeFile.NodeIndex = nodeFileIndex;

            Write(nodeFileIndex);
            
            gbx.AuxNodesInWriteMode.Add(nodeFileIndex, null);

            return;
        }

        if (node is null)
        {
            Write(-1);
            return;
        }

        if (gbx.AuxNodesInWriteMode.ContainsValue(node))
        {
            Write(gbx.AuxNodesInWriteMode.FirstOrDefault(x => (x.Value ?? throw new Exception("Node or its external index not found")).Equals(node)).Key + 1);
            return;
        }

        var index = gbx.AuxNodesInWriteMode.Count;

        while (gbx.AuxNodesInWriteMode.ContainsKey(index))
        {
            index++;
        }

        gbx.AuxNodesInWriteMode.Add(index, node);

        Write(index + 1);
        Write(Chunk.Remap(node.Id, Settings.Remap));

        node.Write(this, logger);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteBigInt(BigInteger bigInteger) => Write(bigInteger.ToByteArray());

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_s(ITime variable) => Write((int)variable.TotalSeconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    [Obsolete("Prefer using WriteTimeInt32()")]
    public void WriteInt32_ms(ITime variable) => Write((int)variable.TotalMilliseconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_sn(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }

        Write((int)variable.TotalSeconds);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    [Obsolete("Prefer using WriteTimeInt32Nullable()")]
    public void WriteInt32_msn(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }

        Write((int)variable.TotalMilliseconds);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    [Obsolete("Prefer using WriteTimeSingle()")]
    public void WriteSingle_s(ITime variable) => Write(variable.TotalSeconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_ms(ITime variable) => Write(variable.TotalMilliseconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    [Obsolete("Prefer using WriteTimeSingleNullable()")]
    public void WriteSingle_sn(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }
        
        Write(variable.TotalSeconds);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_msn(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }

        Write(variable.TotalMilliseconds);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteTimeInt32(ITime variable) => Write((int)variable.TotalMilliseconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteTimeInt32Nullable(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }

        Write((int)variable.TotalMilliseconds);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteTimeSingle(ITime variable) => Write(variable.TotalSeconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteTimeSingleNullable(ITime? variable)
    {
        if (variable is null)
        {
            Write(-1);
            return;
        }

        Write(variable.TotalSeconds);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public async ValueTask WriteBytesAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken = default)
    {
        await BaseStream.WriteAsync(bytes, cancellationToken);
    }
#endif
    
    public async Task WriteBytesAsync(byte[]? bytes, CancellationToken cancellationToken = default)
    {
        if (bytes is not null)
        {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            await BaseStream.WriteAsync(bytes, cancellationToken);
#else
            await BaseStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
#endif
        }
    }

    /// <summary>
    /// Writes the node array that are presented directly and not as a node reference.
    /// </summary>
    /// <typeparam name="T">Type of the node.</typeparam>
    /// <param name="nodes">Node array.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OverflowException">There's more nodes than <see cref="int.MaxValue"/>.</exception>
    public void WriteNodeArray<T>(IEnumerable<T?>? nodes) where T : Node
    {
        if (nodes is null)
        {
            Write(0);
            return;
        }

        var watch = Stopwatch.StartNew();

        var count = nodes.Count();

        var nodeType = typeof(T);

        Write(count);

        var counter = 0;

        foreach (var node in nodes)
        {
            if (node is null)
            {
                Write(-1);
                continue;
            }

            Write(node.Id);
            node.Write(this, logger: logger);

            if (logger?.IsEnabled(LogLevel.Debug) == true)
            {
                logger?.LogDebug("[{className}] {current}/{count} ({time}ms)",
                    nodeType.FullName!.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::"),
                    counter + 1,
                    count,
                    watch.Elapsed.TotalMilliseconds);
            }

            counter += 1;
        }
    }

    /// <exception cref="ArgumentNullException">Key in dictionary is null.</exception>
    /// <exception cref="PropertyNullException"><see cref="GameBoxWriterSettings.Gbx"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteDictionaryNode<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary, Action<TKey, GameBoxWriter>? keyWriter = null) where TValue : Node
    {
        if (dictionary is null)
        {
            Write(0);
            return;
        }

        Write(dictionary.Count);

        foreach (var pair in dictionary)
        {
            if (pair.Key is null)
            {
                throw new ArgumentNullException(nameof(pair.Key));
            }

            if (keyWriter is null)
            {
                WriteAny(pair.Key);
            }
            else
            {
                keyWriter(pair.Key, this);
            }

            Write(pair.Value);
        }
    }

    public void WriteSpan<T>(ReadOnlySpan<T> span) where T : struct
    {
        var bytes = MemoryMarshal.Cast<T, byte>(span);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        Write(bytes);
#else
        Write(bytes.ToArray());
#endif
    }

    public void WriteOptimizedInt(int value, int determineFrom)
    {
        switch ((uint)determineFrom)
        {
            case >= ushort.MaxValue:
                Write(value);
                break;
            case >= byte.MaxValue:
                Write((uint)value);
                break;
            default:
                Write((byte)value);
                break;
        };
    }

    /// <summary>
    /// Writes any kind of value. Prefer using specified methods for better performance. Supported types are <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>,
    /// <see cref="long"/>, <see cref="float"/>, <see cref="bool"/>, <see cref="string"/>, <see cref="sbyte"/>, <see cref="ushort"/>,
    /// <see cref="uint"/>, <see cref="ulong"/>, <see cref="Byte3"/>, <see cref="Vec2"/>, <see cref="Vec3"/>,
    /// <see cref="Vec4"/>, <see cref="Int3"/>, <see cref="Id"/>, and <see cref="Ident"/>.
    /// </summary>
    /// <param name="any">Any supported object.</param>
    /// <exception cref="ArgumentNullException"><paramref name="any"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    private void WriteAny(object any)
    {
        if (any is null)
            throw new ArgumentNullException(nameof(any));

        switch (any)
        {
            case byte       v: Write(v); break;
            case short      v: Write(v); break;
            case int        v: Write(v); break;
            case long       v: Write(v); break;
            case float      v: Write(v); break;
            case string     v: Write(v); break;
            case sbyte      v: Write(v); break;
            case ushort     v: Write(v); break;
            case uint       v: Write(v); break;
            case ulong      v: Write(v); break;
            case Byte3      v: Write(v); break;
            case Vec2       v: Write(v); break;
            case Vec3       v: Write(v); break;
            case Vec4       v: Write(v); break;
            case Int2       v: Write(v); break;
            case Int3       v: Write(v); break;
            case Id v: Write(v); break;
            case Ident      v: Write(v); break;

            default: throw new NotSupportedException($"{any.GetType()} is not supported for Read<T>.");
        }
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array) where T : struct
    {
        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);
        WriteArray_NoPrefix(array);
    }

    /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray_NoPrefix<T>(T[] array) where T : struct
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Write(MemoryMarshal.Cast<T, byte>(array));
#else
        Write(MemoryMarshal.Cast<T, byte>(array).ToArray());
#endif
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteByteArray(byte[]? array)
    {
        if (array is null)
        {
            Write(0);
        }
        else
        {
            Write(array.Length);
            Write(array);
        }
    }

    /// <summary>
    /// First writes an <see cref="int"/> representing the length, then does a for loop with this length, each yield having an option to write something from <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="array">An array.</param>
    /// <param name="forLoop">Each element.</param>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array, Action<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
        {
            forLoop.Invoke(array[i]);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteArray<T>(T[]? array, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
        {
            forLoop.Invoke(array[i], this);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteList<T>(IList<T>? list, Action<T> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
        {
            forLoop.Invoke(list[i]);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteList<T>(IList<T>? list, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
        {
            throw new ArgumentNullException(nameof(forLoop));
        }

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
        {
            forLoop.Invoke(list[i], this);
        }
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OverflowException">The number of elements in source is larger than <see cref="int.MaxValue"/>.</exception>
    public void WriteEnumerable<T>(IEnumerable<T>? enumerable, Action<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (enumerable is null)
        {
            Write(0);
            return;
        }

        var count = enumerable.Count();

        Write(count);

        foreach (var element in enumerable)
        {
            forLoop.Invoke(element);
        }
    }

    /// <exception cref="ArgumentNullException">Key or value in dictionary is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteDictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
    {
        if (dictionary is null)
        {
            Write(0);
            return;
        }

        Write(dictionary.Count);

        foreach (var pair in dictionary)
        {
            if (pair.Key is null)
            {
                throw new ArgumentNullException(nameof(pair.Key));
            }

            if (pair.Value is null)
            {
                throw new ArgumentNullException(nameof(pair.Value));
            }

            WriteAny(pair.Key);
            WriteAny(pair.Value);
        }
    }

    public void WriteOptimizedIntArray(int[]? array, int? determineFrom = null)
    {
        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);
        WriteOptimizedIntArray_NoPrefix(array, determineFrom);
    }

    public void WriteOptimizedIntArray_NoPrefix(int[] array, int? determineFrom = null)
    {
        switch ((uint)determineFrom.GetValueOrDefault(array.Length))
        {
            case >= ushort.MaxValue:
                WriteArray(array);
                break;
            case >= byte.MaxValue:
                WriteArray(Array.ConvertAll(array, x => (ushort)x));
                break;
            default:
                Write(Array.ConvertAll(array, x => (byte)x));
                break;
        }
    }

    public void WriteExternalNodeArray<T>(ExternalNode<T>[]? array) where T : Node
    {
        WriteArray(array, (x, w) => w.Write(x.Node, x.File));
    }
}
