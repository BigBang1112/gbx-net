using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace GBX.NET;

/// <summary>
/// Writes data types from GameBox serialization.
/// </summary>
public class GameBoxWriter : BinaryWriter
{
    /// <summary>
    /// Body used to store node references.
    /// </summary>
    public GameBoxBody? Body { get; }

    /// <summary>
    /// An object to look into for the list of already written data.
    /// </summary>
    public ILookbackable? Lookbackable { get; }

    /// <summary>
    /// Constructs a binary writer specialized for GBX.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="body">Body used to store node references. If null, <see cref="CMwNod"/> cannot be written and <see cref="PropertyNullException"/> can be thrown.</param>
    /// <param name="lookbackable">A specified object to look into for the list of already written data. If null while <paramref name="body"/> is null, <see cref="Id"/> or <see cref="Ident"/> cannot be written and <see cref="PropertyNullException"/> can be thrown. If null while <paramref name="body"/> is not null, the body is used as <see cref="ILookbackable"/> instead.</param>
    public GameBoxWriter(Stream output, GameBoxBody? body = null, ILookbackable? lookbackable = null) : base(output, Encoding.UTF8, true)
    {
        Body = body;
        Lookbackable = lookbackable ?? body;
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
            WriteBytes(Encoding.UTF8.GetBytes(value));
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override void Write(string? value) => Write(value, StringLengthPrefix.Int32);

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
    public override void Write(bool value) => Write(value, false);

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
            throw new ArgumentNullException(nameof(array));

        var bytes = new byte[array.Length * Marshal.SizeOf(default(T))];
        Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
        WriteBytes(bytes);
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
    public void Write<T>(T[]? array, Action<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
            forLoop.Invoke(array[i]);
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    internal void Write<T>(T[]? array, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (array is null)
        {
            Write(0);
            return;
        }

        Write(array.Length);

        for (var i = 0; i < array.Length; i++)
            forLoop.Invoke(array[i], this);
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write<T>(IList<T>? list, Action<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
            forLoop.Invoke(list[i]);
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write<T>(IList<T>? list, Action<T, GameBoxWriter> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (list is null)
        {
            Write(0);
            return;
        }

        Write(list.Count);

        for (var i = 0; i < list.Count; i++)
            forLoop.Invoke(list[i], this);
    }

    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OverflowException">The number of elements in source is larger than <see cref="int.MaxValue"/>.</exception>
    public void Write<T>(IEnumerable<T>? enumerable, Action<T> forLoop)
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

        IEnumerator<T> enumerator = enumerable.GetEnumerator();

        while (enumerator.MoveNext())
            forLoop.Invoke(enumerator.Current);
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

    /// <exception cref="ArgumentNullException"><paramref name="fileRef"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(FileRef fileRef)
    {
        if (fileRef is null)
            throw new ArgumentNullException(nameof(fileRef));

        Write(fileRef.Version);

        if (fileRef.Version >= 3)
            WriteBytes(fileRef.Checksum ?? FileRef.DefaultChecksum);

        Write(fileRef.FilePath);

        if (fileRef.FilePath is not null && ((fileRef.FilePath.Length > 0 && fileRef.Version >= 1) || fileRef.Version >= 3))
            Write(fileRef.LocatorUrl?.ToString());
    }

    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Id value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        var l = value.Owner;

        if (!l.IdWritten)
        {
            if (l.IdVersion.HasValue)
                Write(l.IdVersion.Value);
            else Write(3);
            l.IdWritten = true;
        }

        if (value == "Unassigned")
        {
            Write(0xBFFFFFFF);
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            Write(0xFFFFFFFF);
            return;
        }

        if (l.IdStrings.Contains(value))
        {
            Write(value.Index + 1 + 0x40000000);
            return;
        }

        if (int.TryParse(value, out int cID))
        {
            Write(cID);
            return;
        }

        Write(0x40000000);
        Write(value.ToString());
        l.IdStrings.Add(value);
    }

    /// <exception cref="PropertyNullException"><see cref="Lookbackable"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteId(string? value)
    {
        if (Lookbackable is null)
            throw new PropertyNullException(nameof(Lookbackable));

        Write(new Id(value, Lookbackable));
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Ident? ident, ILookbackable lookbackable)
    {
        Write(new Id(ident?.ID, lookbackable));
        Write(ident?.Collection.ToId(lookbackable) ?? new Id(null, lookbackable));
        Write(new Id(ident?.Author, lookbackable));
    }

    /// <exception cref="PropertyNullException"><see cref="Lookbackable"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(Ident? ident)
    {
        if (Lookbackable is null)
            throw new PropertyNullException(nameof(Lookbackable));

        Write(ident, Lookbackable);
    }

    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(CMwNod? node, GameBoxBody body)
    {
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        if (node is null)
        {
            Write(-1);
            return;
        }

        if (body.AuxilaryNodes.ContainsValue(node))
        {
            Write(body.AuxilaryNodes.FirstOrDefault(x => x.Equals(node)).Key);
            return;
        }

        body.AuxilaryNodes[body.AuxilaryNodes.Count] = node;
        Write(body.AuxilaryNodes.Count);
        Write(Chunk.Remap(node.ID, body.GBX.Remap));
        node.Write(this, body.GBX.Remap);
    }

    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write(CMwNod? node)
    {
        if (Body is null)
            throw new PropertyNullException(nameof(Body));

        Write(node, Body);
    }

    /// <exception cref="ArgumentNullException">Key or value in dictionary is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void Write<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
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
                throw new ArgumentNullException(nameof(pair.Key));

            if (pair.Value is null)
                throw new ArgumentNullException(nameof(pair.Value));

            WriteAny(pair.Key);
            WriteAny(pair.Value);
        }
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteBigInt(BigInteger bigInteger) => WriteBytes(bigInteger.ToByteArray());

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_s(TimeSpan variable) => Write((int)variable.TotalSeconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_ms(TimeSpan variable) => Write((int)variable.TotalMilliseconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_sn(TimeSpan? variable)
    {
        if (variable.HasValue)
        {
            Write((int)variable.Value.TotalSeconds);
            return;
        }

        Write(-1);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteInt32_msn(TimeSpan? variable)
    {
        if (variable.HasValue)
        {
            Write((int)variable.Value.TotalMilliseconds);
            return;
        }

        Write(-1);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_s(TimeSpan variable) => Write((float)variable.TotalSeconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_ms(TimeSpan variable) => Write((float)variable.TotalMilliseconds);

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_sn(TimeSpan? variable)
    {
        if (variable.HasValue)
        {
            Write((float)variable.Value.TotalSeconds);
            return;
        }

        Write(-1);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteSingle_msn(TimeSpan? variable)
    {
        if (variable.HasValue)
        {
            Write((float)variable.Value.TotalMilliseconds);
            return;
        }

        Write(-1);
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteBytes(byte[]? bytes)
    {
        if (bytes is null) return;

        Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Writes the node array that are presented directly and not as a node reference.
    /// </summary>
    /// <typeparam name="T">Type of the node.</typeparam>
    /// <param name="nodes">Node array.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OverflowException">There's more nodes than <see cref="int.MaxValue"/>.</exception>
    public void WriteNodes<T>(IEnumerable<T>? nodes) where T : CMwNod
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
            Write(node.ID);
            node.Write(this);

            string logProgress = $"[{nodeType.FullName!.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::")}] {counter + 1}/{count} ({watch.Elapsed.TotalMilliseconds}ms)";
            if (Body == null || !Body.GBX.ID.HasValue || CMwNod.Remap(Body.GBX.ID.Value) != node.ID)
                logProgress = "~ " + logProgress;

            Log.Write(logProgress, ConsoleColor.Magenta);

            if (counter != count - 1)
                Log.Push(node.Chunks.Count + 2);

            counter += 1;
        }
    }

    /// <exception cref="ArgumentNullException">Key in dictionary is null.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public void WriteDictionaryNode<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary) where TValue : CMwNod
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
                throw new ArgumentNullException(nameof(pair.Key));

            WriteAny(pair.Key);
            Write(pair.Value);
        }
    }

    /// <summary>
    /// Writes any kind of value. Prefer using specified methods for better performance. Supported types are <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>,
    /// <see cref="long"/>, <see cref="float"/>, <see cref="bool"/>, <see cref="string"/>, <see cref="sbyte"/>, <see cref="ushort"/>,
    /// <see cref="uint"/>, <see cref="ulong"/>, <see cref="Byte3"/>, <see cref="Vec2"/>, <see cref="Vec3"/>,
    /// <see cref="Vec4"/>, <see cref="Int3"/>, <see cref="Id"/> and <see cref="Ident"/>.
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
            case byte   v: Write(v); break;
            case short  v: Write(v); break;
            case int    v: Write(v); break;
            case long   v: Write(v); break;
            case float  v: Write(v); break;
            case string v: Write(v); break;
            case sbyte  v: Write(v); break;
            case ushort v: Write(v); break;
            case uint   v: Write(v); break;
            case ulong  v: Write(v); break;
            case Byte3  v: Write(v); break;
            case Vec2   v: Write(v); break;
            case Vec3   v: Write(v); break;
            case Vec4   v: Write(v); break;
            case Int2   v: Write(v); break;
            case Int3   v: Write(v); break;
            case Id     v: Write(v); break;
            case Ident  v: Write(v); break;

            default: throw new NotSupportedException($"{any.GetType()} is not supported for Read<T>.");
        }
    }
}
