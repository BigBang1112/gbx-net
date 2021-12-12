using GBX.NET.Extensions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace GBX.NET;

/// <summary>
/// Reads data types from GameBox serialization.
/// </summary>
public class GameBoxReader : BinaryReader
{
    /// <summary>
    /// Body used to store node references.
    /// </summary>
    public GameBoxBody? Body { get; }

    /// <summary>
    /// An object to look into for the list of already read data.
    /// </summary>
    public ILookbackable? Lookbackable { get; }

    /// <summary>
    /// Constructs a binary reader specialized for GBX.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="body">Body used to store node references. If null, <see cref="CMwNod"/> cannot be read and <see cref="PropertyNullException"/> can be thrown.</param>
    /// <param name="lookbackable">A specified object to look into for the list of already read data. If null while <paramref name="body"/> is null, <see cref="Id"/> or <see cref="Ident"/> cannot be read and <see cref="PropertyNullException"/> can be thrown. If null while <paramref name="body"/> is not null, the body is used as <see cref="ILookbackable"/> instead.</param>
    public GameBoxReader(Stream input, GameBoxBody? body = null, ILookbackable? lookbackable = null) : base(input, Encoding.UTF8, true)
    {
        Body = body;
        Lookbackable = lookbackable ?? body;
    }

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in seconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadInt32_s() => TimeSpan.FromSeconds(ReadInt32());

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadInt32_ms() => TimeSpan.FromMilliseconds(ReadInt32());

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadInt32_sn()
    {
        var time = ReadInt32();
        if (time < 0)
            return null;
        return TimeSpan.FromSeconds(time);
    }

    /// <summary>
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// The integer is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the integer. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadInt32_msn()
    {
        var time = ReadInt32();
        if (time < 0)
            return null;
        return TimeSpan.FromMilliseconds(time);
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in seconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadSingle_s() => TimeSpan.FromSeconds(ReadSingle());

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in milliseconds.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan ReadSingle_ms() => TimeSpan.FromMilliseconds(ReadSingle());

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in seconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadSingle_sn()
    {
        var time = ReadSingle();
        if (time < 0)
            return null;
        return TimeSpan.FromSeconds(time);
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
    /// The floating point value is then presented as time in milliseconds. If value is -1, a null is returned instead.
    /// </summary>
    /// <returns>A TimeSpan converted from the floating point value. Null if the read value is -1.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public TimeSpan? ReadSingle_msn()
    {
        var time = ReadSingle();
        if (time < 0)
            return null;
        return TimeSpan.FromMilliseconds(time);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads the sequence of bytes.
    /// </summary>
    /// <exception cref="ArgumentException">The number of decoded characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    /// <returns>A byte array.</returns>
    public byte[] ReadBytes() => ReadBytes(count: ReadInt32());

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream with one of the prefix reading methods.
    /// </summary>
    /// <param name="readPrefix">The method to read the prefix.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException"><paramref name="readPrefix"/> is <see cref="StringLengthPrefix.None"/>.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <returns>The string being read.</returns>
    public string ReadString(StringLengthPrefix readPrefix)
    {
        var length = readPrefix switch
        {
            StringLengthPrefix.Byte => ReadByte(),
            StringLengthPrefix.Int32 => ReadInt32(),
            _ => throw new ArgumentException("Can't read string without knowing its length."),
        };

        if (length < 0)
            throw new StringLengthOutOfRangeException(length);

        return ReadString(length);
    }

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream. The string is prefixed with the length, encoded as <see cref="int"/>.
    /// </summary>
    /// <returns>The string being read.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public override string ReadString() => ReadString(readPrefix: StringLengthPrefix.Int32);

    /// <summary>
    /// Reads a <see cref="string"/> from the current stream using the <paramref name="length"/> parameter.
    /// </summary>
    /// <param name="length">Length of the bytes to read.</param>
    /// <returns>The string being read.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public string ReadString(int length) => Encoding.UTF8.GetString(ReadBytes(length));

    /// <summary>
    /// Reads the next <see cref="int"/> from the current stream, casts it as <see cref="bool"/> and advances the current position of the stream by 4 bytes.
    /// </summary>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public override bool ReadBoolean() => Convert.ToBoolean(ReadInt32());

    /// <summary>
    /// If <paramref name="asByte"/> is true, reads the next <see cref="byte"/> from the current stream and casts it as <see cref="bool"/>. Otherwise <see cref="ReadBoolean()"/> is called.
    /// </summary>
    /// <param name="asByte">Read the boolean as <see cref="byte"/> or <see cref="int"/>.</param>
    /// <returns>A boolean.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public bool ReadBoolean(bool asByte) => asByte ? base.ReadBoolean() : ReadBoolean();

    /// <summary>
    /// First reads the <see cref="int"/> of the Id version, if not read yet, considering the information from <paramref name="lookbackable"/>. Then reads an <see cref="int"/> (index) that holds the flags of the representing <see cref="string"/>. If the first 30 bits are 0, a fresh <see cref="string"/> is also read.
    /// </summary>
    /// <param name="lookbackable">An object to look into for the list of already read data.</param>
    /// <returns>An <see cref="Id"/> which can be implicitly casted to <see cref="string"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="lookbackable"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Id ReadId(ILookbackable lookbackable)
    {
        if (lookbackable is null)
            throw new ArgumentNullException(nameof(lookbackable));

        if (!lookbackable.IdVersion.HasValue)
        {
            lookbackable.IdVersion = ReadInt32();

            // Edge-case scenario where Id doesn't have a version for whatever reason (can be multiple)
            if ((lookbackable.IdVersion & 0xC0000000) > 10)
            {
                lookbackable.IdVersion = 3;

                if (BaseStream.CanSeek)
                    BaseStream.Seek(-4, SeekOrigin.Current);
                else
                    throw new NotSupportedException("GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.");
            }
        }

        var index = ReadUInt32();

        if ((index & 0x3FFF) == 0 && (index >> 30 == 1 || index >> 30 == 2))
        {
            var str = ReadString();
            lookbackable.IdStrings.Add(str);
            return new Id(str, lookbackable);
        }

        if ((index & 0x3FFF) == 0x3FFF)
        {
            return (index >> 30) switch
            {
                2 => new Id("Unassigned", lookbackable),
                3 => new Id("", lookbackable),
                _ => throw new CorruptedIdException(index >> 30),
            };
        }

        if (index >> 30 == 0)
            return new Id(index.ToString(), lookbackable);

        if (lookbackable.IdStrings.Count > (index & 0x3FFF) - 1)
            return new Id(lookbackable.IdStrings[(int)(index & 0x3FFF) - 1], lookbackable);

        return new Id("", lookbackable);
    }

    /// <summary>
    /// First reads the <see cref="int"/> of the Id version, if not read yet, considering the information from <see cref="Lookbackable"/>. Then reads an <see cref="int"/> (index) that holds the flags of the representing <see cref="string"/>. If the first 30 bits are 0, a fresh <see cref="string"/> is also read.
    /// </summary>
    /// <returns>An <see cref="Id"/> which can be implicitly casted to <see cref="string"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    /// <exception cref="PropertyNullException"><see cref="Lookbackable"/> is null.</exception>
    public Id ReadId()
    {
        if (Lookbackable is null)
            throw new PropertyNullException(nameof(Lookbackable));

        return ReadId(Lookbackable);
    }

    /// <summary>
    /// Reads an <see cref="Id"/> using <see cref="ReadId(ILookbackable)"/> 3 times.
    /// </summary>
    /// <param name="lookbackable">An object to look into for the list of already read data.</param>
    /// <returns>An <see cref="Ident"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="lookbackable"/> is null.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    public Ident ReadIdent(ILookbackable lookbackable)
    {
        if (lookbackable is null)
            throw new ArgumentNullException(nameof(lookbackable));

        var id = ReadId(lookbackable);
        var collection = ReadId(lookbackable);
        var author = ReadId(lookbackable);

        return new Ident(id, collection, author);
    }

    /// <summary>
    /// Reads an <see cref="Id"/> using <see cref="ReadId(ILookbackable)"/> 3 times.
    /// </summary>
    /// <returns>An <see cref="Ident"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">GBX has the first Id presented without a version. Solution exists, but the stream does not support seeking.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    /// <exception cref="CorruptedIdException">The Id index is not matching any known values.</exception>
    /// <exception cref="PropertyNullException"><see cref="Lookbackable"/> is null.</exception>
    public Ident ReadIdent()
    {
        if (Lookbackable is null)
            throw new PropertyNullException(nameof(Lookbackable));

        return ReadIdent(Lookbackable);
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="CMwNod"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public T? ReadNodeRef<T>(GameBoxBody body) where T : CMwNod
    {
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        var index = ReadInt32() - 1; // GBX seems to start the index at 1

        var refTable = body.GBX.RefTable;

        // First checks if reference table is used
        if (refTable is not null && (refTable.Folders.Count > 0 || refTable.Folders.Count > 0))
        {
            var allFiles = refTable.GetAllFiles(); // Returns available external references
            if (allFiles.Any()) // If there's one
            {
                // Tries to get the one with this node index
                var refTableNode = allFiles.FirstOrDefault(x => x.NodeIndex == index + 1);
                if (refTableNode is not null)
                    return null; // Temporary, resolve later

                // Else it's a nested object
            }
        }

        // If index is 0 or bigger and the node wasn't read yet, or is null
        if (index >= 0 && (!body.AuxilaryNodes.ContainsKey(index) || body.AuxilaryNodes[index] == null))
            body.AuxilaryNodes[index] = CMwNod.Parse<T>(this)!;

        if (index < 0) // If aux node index is below 0 then there's not much to solve
            return null;

        body.AuxilaryNodes.TryGetValue(index, out CMwNod? n); // Tries to get the available node from index

        if (n is T nod) // If the node is presented at the index, then it's simple
            return nod;

        // But sometimes it indexes the node reference that is further in the expected indexes
        return (T)body.AuxilaryNodes.Last().Value; // So it grabs the last one instead, needs to be further tested
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="CMwNod"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <typeparam name="T">Type of node.</typeparam>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public T? ReadNodeRef<T>() where T : CMwNod
    {
        if (Body is null)
            throw new PropertyNullException(nameof(Body));

        return ReadNodeRef<T>(Body);
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="CMwNod"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <param name="body">Body used to store node references.</param>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    public CMwNod? ReadNodeRef(GameBoxBody body)
    {
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        return ReadNodeRef<CMwNod>(body);
    }

    /// <summary>
    /// Reads an <see cref="int"/> containing the node reference index, then the node using the <see cref="CMwNod"/>'s Parse method. The index is also checked if it isn't a part of a reference table, which currently returns null.
    /// </summary>
    /// <returns>A node, or null if the index is -1 or the node is from reference table.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    public CMwNod? ReadNodeRef()
    {
        if (Body is null)
            throw new PropertyNullException(nameof(Body));

        return ReadNodeRef<CMwNod>(Body);
    }

    /// <summary>
    /// Reads the file reference data type - file path, checksum and locator URL.
    /// </summary>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="StringLengthOutOfRangeException">String length is negative.</exception>
    public FileRef ReadFileRef()
    {
        var version = ReadByte();

        byte[]? checksum = null;
        string locatorUrl = "";

        if (version >= 3)
            checksum = ReadBytes(32);

        var filePath = ReadString();

        if ((filePath.Length > 0 && version >= 1) || version >= 3)
            locatorUrl = ReadString();

        return new FileRef(version, checksum, filePath, locatorUrl);
    }

    /// <summary>
    /// Reads an array of primitive types (only some are supported) with <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] ReadArray<T>(int length) where T : struct
    {
        var buffer = ReadBytes(length * Marshal.SizeOf(default(T)));
        var array = new T[length];
        Buffer.BlockCopy(buffer, 0, array, 0, buffer.Length);
        return array;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then reads an array of primitive types (only some are supported) with this length.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>() where T : struct => ReadArray<T>(length: ReadInt32());

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static T[] ReadArray<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        var result = new T[length];

        for (var i = 0; i < length; i++)
            result[i] = forLoop.Invoke(i);

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<int, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static T[] ReadArray<T>(int length, Func<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        var result = new T[length];

        for (var i = 0; i < length; i++)
            result[i] = forLoop.Invoke();

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] ReadArray<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        var result = new T[length];

        for (var i = 0; i < length; i++)
            result[i] = forLoop.Invoke(i, this);

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] ReadArray<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        var result = new T[length];

        for (var i = 0; i < length; i++)
            result[i] = forLoop.Invoke(this);

        return result;
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the array.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public T[] ReadArray<T>(Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        return ReadArray(length: ReadInt32(), forLoop);
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        for (var i = 0; i < length; i++)
            yield return forLoop.Invoke();
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
            yield return x;
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        for (var i = 0; i < length; i++)
            yield return forLoop.Invoke(this);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
            yield return x;
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IEnumerable<T> ReadEnumerable<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        for (var i = 0; i < length; i++)
            yield return forLoop.Invoke(i);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
            yield return x;
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="length">Length of the enumerable.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(int length, Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        for (var i = 0; i < length; i++)
            yield return forLoop.Invoke(i, this);
    }

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. Instead of array allocation, elements are yielded one by one.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>Enumerable of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IEnumerable<T> ReadEnumerable<T>(Func<int, GameBoxReader, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        foreach (var x in ReadEnumerable(length: ReadInt32(), forLoop))
            yield return x;
    }

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IList<T> ReadList<T>(int length, Func<T> forLoop) => ReadEnumerable(length, forLoop).ToList(length);

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<T> forLoop) => ReadList(length: ReadInt32(), forLoop);

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public IList<T> ReadList<T>(int length, Func<GameBoxReader, T> forLoop) => ReadEnumerable(length, forLoop).ToList(length);

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<GameBoxReader, T> forLoop) => ReadList(length: ReadInt32(), forLoop);

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public static IList<T> ReadList<T>(int length, Func<int, T> forLoop) => ReadEnumerable(length, forLoop).ToList(length);

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<int, T> forLoop) => ReadList(length: ReadInt32(), forLoop);

    /// <summary>
    /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="length">Length of the list.</param>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(int length, Func<int, GameBoxReader, T> forLoop) => ReadEnumerable(length, forLoop).ToList(length);

    /// <summary>
    /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>. A list is allocated and elements are added via enumeration.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="forLoop">Each element with an index parameter and this reader (to avoid closures).</param>
    /// <returns>List of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length is negative.</exception>
    public IList<T> ReadList<T>(Func<int, GameBoxReader, T> forLoop) => ReadList(length: ReadInt32(), forLoop);

    /// <summary>
    /// Reads values in a dictionary kind (first key, then value). For node dictionaries, use the <see cref="ReadDictionaryNode{TKey, TValue}"/> method for better performance.
    /// </summary>
    /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>. Mustn't be null.</typeparam>
    /// <typeparam name="TValue">One of the supported types of <see cref="Read{T}"/>.</typeparam>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue> ReadDictionary<TKey, TValue>() where TKey : notnull
    {
        var length = ReadInt32();

        var dictionary = new Dictionary<TKey, TValue>(length);

        for (var i = 0; i < length; i++)
            dictionary.Add(key: Read<TKey>(), value: Read<TValue>());

        return dictionary;
    }

    /// <summary>
    /// Reads nodes in a dictionary kind (first key, then value).
    /// </summary>
    /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>. Mustn't be null.</typeparam>
    /// <typeparam name="TValue">A node that is presented as node reference.</typeparam>
    /// <returns>A dictionary.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="PropertyNullException"><see cref="Body"/> is null.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
    public IDictionary<TKey, TValue?> ReadDictionaryNode<TKey, TValue>() where TKey : notnull where TValue : CMwNod
    {
        var length = ReadInt32();

        var dictionary = new Dictionary<TKey, TValue?>(length);

        for (var i = 0; i < length; i++)
            dictionary.Add(key: Read<TKey>(), value: ReadNodeRef<TValue>());

        return dictionary;
    }

    /// <summary>
    /// Reads 2 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 2D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec2 ReadVec2() => new(x: ReadSingle(), y: ReadSingle());

    /// <summary>
    /// Reads 3 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 3D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec3 ReadVec3() => new(x: ReadSingle(), y: ReadSingle(), z: ReadSingle());

    /// <summary>
    /// Reads 4 <see cref="float"/>s.
    /// </summary>
    /// <returns>A 4D vector.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Vec4 ReadVec4() => new(x: ReadSingle(), y: ReadSingle(), z: ReadSingle(), w: ReadSingle());

    /// <summary>
    /// Reads 3 <see cref="int"/>s.
    /// </summary>
    /// <returns>A 3-int.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int3 ReadInt3() => new(x: ReadInt32(), y: ReadInt32(), z: ReadInt32());

    /// <summary>
    /// Reads 2 <see cref="int"/>s.
    /// </summary>
    /// <returns>A 2-int.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Int2 ReadInt2() => new(x: ReadInt32(), y: ReadInt32());

    /// <summary>
    /// Reads 3 <see cref="byte"/>s.
    /// </summary>
    /// <returns>A 3-byte.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public Byte3 ReadByte3() => new(x: ReadByte(), y: ReadByte(), z: ReadByte());

    /// <summary>
    /// Reads a <paramref name="byteLength"/> amount of bytes and converts them into <see cref="BigInteger"/>.
    /// </summary>
    /// <returns>A big integer.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public BigInteger ReadBigInt(int byteLength) => new(ReadBytes(byteLength));

    /// <summary>
    /// Reads a common transform representation.
    /// </summary>
    /// <returns>3D position, quaternion rotation, speed, vector velocity.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public (Vec3 position, Quaternion rotation, float speed, Vec3 velocity) ReadTransform()
    {
        var pos = ReadVec3();
        
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

        var angle = ReadUInt16() / (float)ushort.MaxValue * MathF.PI;
        var axisHeading = ReadInt16() / (float)short.MaxValue * MathF.PI;
        var axisPitch = ReadInt16() / (float)short.MaxValue * MathF.PI / 2;
        var speed = (float)Math.Exp(ReadInt16() / 1000.0);
        var velocityHeading = ReadSByte() / (float)sbyte.MaxValue * MathF.PI;
        var velocityPitch = ReadSByte() / (float)sbyte.MaxValue * MathF.PI / 2;

        var axis = new Vec3((float)(MathF.Sin(angle) * MathF.Cos(axisPitch) * MathF.Cos(axisHeading)),
            (float)(MathF.Sin(angle) * MathF.Cos(axisPitch) * MathF.Sin(axisHeading)),
            (float)(MathF.Sin(angle) * MathF.Sin(axisPitch)));

        var quaternion = new Quaternion(axis, MathF.Cos(angle));

        var velocity = new Vec3((float)(speed * MathF.Cos(velocityPitch) * MathF.Cos(velocityHeading)),
            (float)(speed * MathF.Cos(velocityPitch) * MathF.Sin(velocityHeading)),
            (float)(speed * MathF.Sin(velocityPitch)));

#else

        var angle = ReadUInt16() / (double)ushort.MaxValue * Math.PI;
        var axisHeading = ReadInt16() / (double)short.MaxValue * Math.PI;
        var axisPitch = ReadInt16() / (double)short.MaxValue * Math.PI / 2;
        var speed = (float)Math.Exp(ReadInt16() / 1000.0);
        var velocityHeading = ReadSByte() / (double)sbyte.MaxValue * Math.PI;
        var velocityPitch = ReadSByte() / (double)sbyte.MaxValue * Math.PI / 2;

        var axis = new Vec3((float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Cos(axisHeading)),
            (float)(Math.Sin(angle) * Math.Cos(axisPitch) * Math.Sin(axisHeading)),
            (float)(Math.Sin(angle) * Math.Sin(axisPitch)));

        var quaternion = new Quaternion(axis, (float)Math.Cos(angle));

        var velocity = new Vec3((float)(speed * Math.Cos(velocityPitch) * Math.Cos(velocityHeading)),
            (float)(speed * Math.Cos(velocityPitch) * Math.Sin(velocityHeading)),
            (float)(speed * Math.Sin(velocityPitch)));

#endif

        return (pos, quaternion, speed, velocity);
    }

    /// <summary>
    /// Continues reading the stream until a specific <paramref name="value"/> is reached.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public IEnumerable<byte> ReadUntilUInt32(uint value)
    {
        while (PeekUInt32() != value)
            yield return ReadByte();
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public IEnumerable<byte> ReadUntilFacade() => ReadUntilUInt32(0xFACADE01);

    /// <summary>
    /// Continues reading the stream until the end of the is reached. The stream can use either seeking or non-seeking method, depends what works best for you.
    /// </summary>
    /// <param name="seek">If the method should use seeking features.</param>
    /// <returns>Array of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public byte[] ReadToEnd(bool seek = false)
    {
        if (seek)
            return ReadBytes((int)(BaseStream.Length - BaseStream.Position));

        using var ms = new MemoryStream();
        BaseStream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and converts the result to string.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public string ReadStringUntilFacade() => Encoding.UTF8.GetString(ReadUntilFacade().ToArray());

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T"/>.
    /// </summary>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public T[] ReadArrayUntilFacade<T>() => CreateArrayForUntilFacade<T>(bytes: ReadUntilFacade().ToArray());

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T1"/> and <typeparamref name="T2"/>.
    /// </summary>
    /// <returns>An tuple of arrays.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public (T1[], T2[]) ReadArrayUntilFacade<T1, T2>()
    {
        var bytes = ReadUntilFacade().ToArray();

        return (
            CreateArrayForUntilFacade<T1>(bytes),
            CreateArrayForUntilFacade<T2>(bytes)
        );
    }

    /// <summary>
    /// Continues reading the stream until facade (<c>0xFACADE01</c>) is reached and the result is converted into an array of <typeparamref name="T1"/>, <typeparamref name="T2"/> and <typeparamref name="T3"/>.
    /// </summary>
    /// <returns>An tuple of arrays.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public (T1[], T2[], T3[]) ReadArrayUntilFacade<T1, T2, T3>()
    {
        var bytes = ReadUntilFacade().ToArray();

        return (
            CreateArrayForUntilFacade<T1>(bytes), 
            CreateArrayForUntilFacade<T2>(bytes), 
            CreateArrayForUntilFacade<T3>(bytes)
        );
    }

    private static T[] CreateArrayForUntilFacade<T>(byte[] bytes)
    {
        var array = new T[(int)Math.Ceiling(bytes.Length / (double)Marshal.SizeOf(default(T)))];
        Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
        return array;
    }

    /// <summary>
    /// Continues reading the stream until a specific chunk ID with the base <paramref name="classId"/> is reached, using remapping and masking.
    /// </summary>
    /// <returns>Enumeration of bytes.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public IEnumerable<byte> ReadUntilNextChunk(uint classId)
    {
        classId |= 0x00000FFF;

        while (true)
        {
            var peekedUint = PeekUInt32();
            if ((Chunk.Remap(peekedUint) | 0x00000FFF) == classId)
                yield break;
            yield return ReadByte();
        }
    }

    /// <summary>
    /// Returns the next available <see cref="uint"/> but does not consume it.
    /// </summary>
    /// <returns>An <see cref="uint"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    public uint PeekUInt32()
    {
        var result = ReadUInt32();
        BaseStream.Position -= sizeof(uint);
        return result;
    }

    /// <summary>
    /// Returns the upcoming array but does not consume it.
    /// </summary>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    public T[] PeekArray<T>(int length) where T : struct
    {
        var array = ReadArray<T>(length);
        BaseStream.Position -= length * Marshal.SizeOf(default(T));
        return array;
    }

    /// <summary>
    /// Returns the upcoming array but does not consume it.
    /// </summary>
    /// <param name="length">Length of the array.</param>
    /// <param name="forLoop">Each element with an index parameter.</param>
    /// <returns>An array of <typeparamref name="T"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="forLoop"/> is null.</exception>
    public T[] PeekArray<T>(int length, Func<int, T> forLoop)
    {
        if (forLoop is null)
            throw new ArgumentNullException(nameof(forLoop));

        var beforePos = BaseStream.Position;

        var array = ReadArray(length, forLoop);

        BaseStream.Position = beforePos;

        return array;
    }

    /// <summary>
    /// Compares the <paramref name="magic"/> with the string to be read, using its length.
    /// </summary>
    /// <returns>If the <see cref="string"/> inside the stream is equal to <paramref name="magic"/>.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="magic"/> is null.</exception>
    public bool HasMagic(string magic)
    {
        if (magic is null)
            throw new ArgumentNullException(nameof(magic));

        return ReadString(Encoding.UTF8.GetByteCount(magic)) == magic;
    }

    /// <summary>
    /// A generic read method of parameterless types for the cost of performance loss. Prefer using the pre-defined data read methods.
    /// </summary>
    /// <typeparam name="T">Type of the variable to read. Supported types are <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>,
    /// <see cref="long"/>, <see cref="float"/>, <see cref="bool"/>, <see cref="string"/>, <see cref="sbyte"/>, <see cref="ushort"/>,
    /// <see cref="uint"/>, <see cref="ulong"/>, <see cref="Byte3"/>, <see cref="Vec2"/>, <see cref="Vec3"/>,
    /// <see cref="Vec4"/>, <see cref="Int3"/>, <see cref="Id"/> and <see cref="Ident"/>.</typeparam>
    /// <returns>Object read from the stream.</returns>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public T Read<T>() => typeof(T) switch
    {
        Type byteType   when byteType   == typeof(byte)     => (T)Convert.ChangeType(ReadByte(), typeof(T)),
        Type shortType  when shortType  == typeof(short)    => (T)Convert.ChangeType(ReadInt16(), typeof(T)),
        Type intType    when intType    == typeof(int)      => (T)Convert.ChangeType(ReadInt32(), typeof(T)),
        Type longType   when longType   == typeof(long)     => (T)Convert.ChangeType(ReadInt64(), typeof(T)),
        Type floatType  when floatType  == typeof(float)    => (T)Convert.ChangeType(ReadSingle(), typeof(T)),
        Type boolType   when boolType   == typeof(bool)     => (T)Convert.ChangeType(ReadBoolean(), typeof(T)),
        Type stringType when stringType == typeof(string)   => (T)Convert.ChangeType(ReadString(), typeof(T)),
        Type sbyteType  when sbyteType  == typeof(sbyte)    => (T)Convert.ChangeType(ReadSByte(), typeof(T)),
        Type ushortType when ushortType == typeof(ushort)   => (T)Convert.ChangeType(ReadUInt16(), typeof(T)),
        Type uintType   when uintType   == typeof(uint)     => (T)Convert.ChangeType(ReadUInt32(), typeof(T)),
        Type ulongType  when ulongType  == typeof(ulong)    => (T)Convert.ChangeType(ReadUInt64(), typeof(T)),
        Type byte3Type  when byte3Type  == typeof(Byte3)    => (T)Convert.ChangeType(ReadByte3(), typeof(T)),
        Type vec2Type   when vec2Type   == typeof(Vec2)     => (T)Convert.ChangeType(ReadVec2(), typeof(T)),
        Type vec3Type   when vec3Type   == typeof(Vec3)     => (T)Convert.ChangeType(ReadVec3(), typeof(T)),
        Type vec4Type   when vec4Type   == typeof(Vec4)     => (T)Convert.ChangeType(ReadVec4(), typeof(T)),
        Type int2Type   when int2Type   == typeof(Int2)     => (T)Convert.ChangeType(ReadInt2(), typeof(T)),
        Type int3Type   when int3Type   == typeof(Int3)     => (T)Convert.ChangeType(ReadInt3(), typeof(T)),
        Type idType     when idType     == typeof(Id)       => (T)Convert.ChangeType(ReadId(), typeof(T)),
        Type identType  when identType  == typeof(Ident)    => (T)Convert.ChangeType(ReadIdent(), typeof(T)),

        _ => throw new NotSupportedException($"{typeof(T)} is not supported for Read<T>."),
    };
}
