using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using GBX.NET.Engines.MwFoundations;
using GBX.NET.Exceptions;

namespace GBX.NET
{
    /// <summary>
    /// Reads data types from GameBox serialization.
    /// </summary>
    public class GameBoxReader : BinaryReader
    {
        public GameBoxBody? Body { get; }
        public ILookbackable? Lookbackable { get; }

        public GameBox? GBX
        {
            get
            {
                if (Body is not null)
                    return Body.GBX;
                return Lookbackable?.GBX;
            }
        }

        public GameBoxReader(Stream input) : base(input, Encoding.UTF8, true)
        {

        }

        public GameBoxReader(Stream input, ILookbackable? lookbackable) : this(input)
        {
            Lookbackable = lookbackable;

            if (lookbackable is GameBoxBody b)
                Body = b;
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// The integer is then presented as time in seconds.
        /// </summary>
        /// <returns>A TimeSpan converted from the integer.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public TimeSpan ReadInt32_s()
        {
            return TimeSpan.FromSeconds(ReadInt32());
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// The integer is then presented as time in milliseconds.
        /// </summary>
        /// <returns>A TimeSpan converted from the integer.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public TimeSpan ReadInt32_ms()
        {
            return TimeSpan.FromMilliseconds(ReadInt32());
        }

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
        public TimeSpan ReadSingle_s()
        {
            return TimeSpan.FromSeconds(ReadSingle());
        }

        /// <summary>
        /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
        /// The floating point value is then presented as time in milliseconds.
        /// </summary>
        /// <returns>A TimeSpan converted from the floating point value.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public TimeSpan ReadSingle_ms()
        {
            return TimeSpan.FromMilliseconds(ReadSingle());
        }

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
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>A byte array.</returns>
        public byte[] ReadBytes()
        {
            var length = ReadInt32();
            return ReadBytes(length);
        }

        /// <summary>
        /// Reads a <see cref="string"/> from the current stream with one of the prefix reading methods.
        /// </summary>
        /// <param name="readPrefix">The method to read the prefix.</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
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
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
        /// <returns>The string being read.</returns>
        public override string ReadString()
        {
            return ReadString(StringLengthPrefix.Int32);
        }

        /// <summary>
        /// Reads a <see cref="string"/> from the current stream using the <paramref name="length"/> parameter.
        /// </summary>
        /// <param name="length">Length of the bytes to read.</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>The string being read.</returns>
        public string ReadString(int length)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        /// <summary>
        /// Reads the next <see cref="int"/> from the current stream, casts it as <see cref="bool"/> and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <returns>A boolean.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public override bool ReadBoolean()
        {
            return Convert.ToBoolean(ReadInt32());
        }

        /// <summary>
        /// If <paramref name="asByte"/> is true, reads the next <see cref="byte"/> from the current stream and casts it as <see cref="bool"/>. Otherwise <see cref="ReadBoolean()"/> is called.
        /// </summary>
        /// <param name="asByte">Read the boolean as <see cref="byte"/> or <see cref="int"/>.</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <returns>A boolean.</returns>
        public bool ReadBoolean(bool asByte)
        {
            if (asByte) return base.ReadBoolean();
            else return ReadBoolean();
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
        /// <exception cref="CorruptedIdException"></exception>
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
            {
                return new Id(index.ToString(), lookbackable);
            }

            if (lookbackable.IdStrings.Count > (index & 0x3FFF) - 1)
                return new Id(lookbackable.IdStrings[(int)(index & 0x3FFF) - 1], lookbackable);
            
            return new Id("", lookbackable);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
        /// <exception cref="CorruptedIdException"></exception>
        /// <exception cref="PropertyNullException"></exception>
        public Id ReadId()
        {
            if (Lookbackable is null)
                throw new PropertyNullException(nameof(Lookbackable));

            return ReadId(Lookbackable);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
        /// <exception cref="CorruptedIdException"></exception>
        public Ident ReadIdent(ILookbackable lookbackable)
        {
            if (lookbackable is null)
                throw new ArgumentNullException(nameof(lookbackable));

            var id = ReadId(lookbackable);
            var collection = ReadId(lookbackable);
            var author = ReadId(lookbackable);

            return new Ident(id, collection, author);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
        /// <exception cref="CorruptedIdException"></exception>
        /// <exception cref="PropertyNullException"></exception>
        public Ident ReadIdent()
        {
            if (Lookbackable is null)
                throw new PropertyNullException(nameof(Lookbackable));

            return ReadIdent(Lookbackable);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public T? ReadNodeRef<T>(GameBoxBody body) where T : CMwNod
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));

            var index = ReadInt32() - 1; // GBX seems to start the index at 1

            var refTable = body.GBX.RefTable;
            if (refTable is not null) // First checks if reference table is used
            {
                var allFiles = refTable.GetAllFiles(); // Returns available external references
                if(allFiles.Count() > 0) // If there's one
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

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="PropertyNullException"></exception>
        public T? ReadNodeRef<T>() where T : CMwNod
        {
            if (Body is null)
                throw new PropertyNullException(nameof(Body));

            return ReadNodeRef<T>(Body);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CMwNod? ReadNodeRef(GameBoxBody body)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));

            return ReadNodeRef<CMwNod>(body);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="PropertyNullException"></exception>
        public CMwNod? ReadNodeRef()
        {
            if (Body is null)
                throw new PropertyNullException(nameof(Body));

            return ReadNodeRef<CMwNod>(Body);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="StringLengthOutOfRangeException"></exception>
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] ReadArray<T>() where T : struct
        {
            return ReadArray<T>(ReadInt32());
        }

        /// <summary>
        /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="length">Length of the array.</param>
        /// <param name="forLoop">Each element with an index parameter.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">Array length is lower than 0.</exception>
        public T[] ReadArray<T>(int length, Func<int, T> forLoop)
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
        /// <exception cref="ArgumentOutOfRangeException">Array length is lower than 0.</exception>
        public T[] ReadArray<T>(Func<int, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var length = ReadInt32();

            return ReadArray(length, forLoop);
        }

        /// <summary>
        /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="length">Length of the array.</param>
        /// <param name="forLoop">Each element.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] ReadArray<T>(int length, Func<T> forLoop)
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] ReadArray<T>(Func<T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var length = ReadInt32();

            return ReadArray(length, forLoop);
        }

        /// <summary>
        /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="length">Length of the array.</param>
        /// <param name="forLoop">Each element with an index parameter and this reader.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <param name="forLoop">Each element with an index parameter and this reader.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] ReadArray<T>(Func<int, GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var length = ReadInt32();

            return ReadArray(length, forLoop);
        }

        /// <summary>
        /// Does a for loop with <paramref name="length"/> parameter, each element requiring to return an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="length">Length of the array.</param>
        /// <param name="forLoop">Each element with this reader.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <param name="forLoop">Each element with this reader.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] ReadArray<T>(Func<GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var length = ReadInt32();

            return ReadArray(length, forLoop);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(int length, Func<int, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var result = new List<T>(length);

            for (var i = 0; i < length; i++)
                result.Add(forLoop.Invoke(i));

            return result;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(Func<int, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            return ReadList(ReadInt32(), forLoop);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(int length, Func<T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var result = new List<T>(length);

            for (var i = 0; i < length; i++)
                result.Add(forLoop.Invoke());

            return result;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(Func<T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            return ReadList(ReadInt32(), forLoop);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(int length, Func<int, GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var result = new List<T>(length);

            for (var i = 0; i < length; i++)
                result.Add(forLoop.Invoke(i, this));

            return result;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(Func<int, GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            return ReadList(ReadInt32(), forLoop);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(int length, Func<GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var result = new List<T>(length);

            for (var i = 0; i < length; i++)
                result.Add(forLoop.Invoke(this));

            return result;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IList<T> ReadList<T>(Func<GameBoxReader, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            return ReadList(ReadInt32(), forLoop);
        }

        /// <summary>
        /// Reads values in a dictionary kind (first key, then value). For node dictionaries, use the <see cref="ReadDictionaryNode{TKey, TValue}"/> method for better performance.
        /// </summary>
        /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>.</typeparam>
        /// <typeparam name="TValue">One of the supported types of <see cref="Read{T}"/>.</typeparam>
        /// <returns>A dictionary.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentException"></exception>
        public IDictionary<TKey, TValue> ReadDictionary<TKey, TValue>() where TKey : notnull
        {
            var dictionary = new Dictionary<TKey, TValue>();

            var length = ReadInt32();

            for (var i = 0; i < length; i++)
            {
                var key = Read<TKey>();
                var value = Read<TValue>();

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Reads nodes in a dictionary kind (first key, then value).
        /// </summary>
        /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>.</typeparam>
        /// <typeparam name="TValue">A node that is presented as node reference.</typeparam>
        /// <returns>A dictionary.</returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="PropertyNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IDictionary<TKey, TValue?> ReadDictionaryNode<TKey, TValue>() where TKey : notnull where TValue : CMwNod
        {
            var dictionary = new Dictionary<TKey, TValue?>();

            var length = ReadInt32();

            for (var i = 0; i < length; i++)
            {
                var key = Read<TKey>();
                var value = ReadNodeRef<TValue>();

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Vec2 ReadVec2()
        {
            var floats = ReadArray<float>(2);
            return new Vec2(floats[0], floats[1]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Vec3 ReadVec3()
        {
            var floats = ReadArray<float>(3);
            return new Vec3(floats[0], floats[1], floats[2]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Vec4 ReadVec4()
        {
            var floats = ReadArray<float>(4);
            return new Vec4(floats[0], floats[1], floats[2], floats[3]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Int3 ReadInt3()
        {
            var ints = ReadArray<int>(3);
            return (ints[0], ints[1], ints[2]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Int2 ReadInt2()
        {
            var ints = ReadArray<int>(2);
            return (ints[0], ints[1]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public Byte3 ReadByte3()
        {
            var bytes = ReadBytes(3);
            return (bytes[0], bytes[1], bytes[2]);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public BigInteger ReadBigInt(int byteLength)
        {
            var bytes = ReadBytes(byteLength);
            return new BigInteger(bytes);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public (Vec3 position, Quaternion rotation, float speed, Vec3 velocity) ReadTransform()
        {
            var pos = ReadVec3();
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

            return (pos, quaternion, speed, velocity);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public IEnumerable<byte> ReadUntil(uint uint32)
        {
            while (PeekUInt32() != uint32)
                yield return ReadByte();
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public IEnumerable<byte> ReadUntilFacade()
        {
            return ReadUntil(0xFACADE01);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte[] ReadToEnd()
        {
            return ReadBytes((int)(BaseStream.Length - BaseStream.Position));
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public string ReadStringUntilFacade()
        {
            return Encoding.UTF8.GetString(ReadUntilFacade().ToArray());
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public T[] ReadArrayUntilFacade<T>()
        {
            var bytes = ReadUntilFacade().ToArray();

            var array = new T[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            return array;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public (T1[], T2[]) ReadArrayUntilFacade<T1, T2>()
        {
            var bytes = ReadUntilFacade().ToArray();

            var array = new T1[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T1)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            var array2 = new T2[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T2)))];
            Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);

            return (array, array2);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public (T1[], T2[], T3[]) ReadArrayUntilFacade<T1, T2, T3>()
        {
            var bytes = ReadUntilFacade().ToArray();

            var array = new T1[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T1)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            var array2 = new T2[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T2)))];
            Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);

            var array3 = new T3[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T3)))];
            Buffer.BlockCopy(bytes, 0, array3, 0, bytes.Length);

            return (array, array2, array3);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public uint PeekUInt32()
        {
            var result = ReadUInt32();
            BaseStream.Position -= sizeof(uint);
            return result;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T[] PeekArray<T>(int length) where T : struct
        {
            var array = ReadArray<T>(length);
            BaseStream.Position -= length * Marshal.SizeOf(default(T));
            return array;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public T[] PeekArray<T>(int length, Func<int, T> forLoop)
        {
            if (forLoop is null)
                throw new ArgumentNullException(nameof(forLoop));

            var beforePos = BaseStream.Position;

            var array = ReadArray(length, forLoop);

            BaseStream.Position = beforePos;

            return array;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool HasMagic(string magic)
        {
            if (magic is null)
                throw new ArgumentNullException(nameof(magic));

            return ReadString(magic.Length) == magic;
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
        private T Read<T>()
        {
            return typeof(T) switch
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
    }
}