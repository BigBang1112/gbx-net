using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Writes data types from GameBox serialization.
    /// </summary>
    public class GameBoxWriter : BinaryWriter
    {
        public GameBoxBody? Body { get; }
        public ILookbackable? Lookbackable { get; }

        public GameBox? GBX
        {
            get
            {
                if (Body != null)
                    return Body.GBX;
                return Lookbackable?.GBX;
            }
        }

        public GameBoxWriter(Stream output) : base(output, Encoding.UTF8, true)
        {

        }

        public GameBoxWriter(Stream output, ILookbackable? lookbackable) : this(output)
        {
            Lookbackable = lookbackable;

            if (lookbackable is GameBoxBody b)
                Body = b;
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public void Write(string? value, StringLengthPrefix lengthPrefix)
        {
            if (value is null)
            {
                switch (lengthPrefix)
                {
                    case StringLengthPrefix.Byte:
                        Write((byte)0);
                        break;
                    case StringLengthPrefix.Int32:
                        Write(0);
                        break;
                }

                return;
            }

            switch (lengthPrefix)
            {
                case StringLengthPrefix.Byte:
                    Write((byte)Encoding.UTF8.GetByteCount(value));
                    break;
                case StringLengthPrefix.Int32:
                    Write(Encoding.UTF8.GetByteCount(value));
                    break;
            }

            var bytes = Encoding.UTF8.GetBytes(value);
            WriteBytes(bytes);
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

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
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
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="OverflowException"></exception>
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

        /// <exception cref="ArgumentNullException"></exception>
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

            if (fileRef.FilePath is not null)
            {
                if ((fileRef.FilePath.Length > 0 && fileRef.Version >= 1) || fileRef.Version >= 3)
                {
                    Write(fileRef.LocatorUrl?.ToString());
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="PropertyNullException"></exception>
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

        /// <exception cref="PropertyNullException"></exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public void Write(Ident? ident)
        {
            if (Lookbackable is null)
                throw new PropertyNullException(nameof(Lookbackable));

            Write(ident, Lookbackable);
        }

        /// <exception cref="ArgumentNullException"></exception>
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

        /// <exception cref="PropertyNullException"></exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public void Write(CMwNod? node)
        {
            if (Body is null)
                throw new PropertyNullException(nameof(Body));

            Write(node, Body);
        }

        /// <exception cref="ArgumentNullException"></exception>
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
        public void WriteBigInt(BigInteger bigInteger)
        {
            WriteBytes(bigInteger.ToByteArray());
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="OverflowException"></exception>
        public void WriteInt32_s(TimeSpan variable)
        {
            Write(Convert.ToInt32(variable.TotalSeconds));
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="OverflowException"></exception>
        public void WriteInt32_ms(TimeSpan variable)
        {
            Write(Convert.ToInt32(variable.TotalMilliseconds));
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="OverflowException"></exception>
        public void WriteInt32_sn(TimeSpan? variable)
        {
            if (variable.HasValue)
            {
                Write(Convert.ToInt32(variable.Value.TotalSeconds));
                return;
            }

            Write(-1);
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="OverflowException"></exception>
        public void WriteInt32_msn(TimeSpan? variable)
        {
            if (variable.HasValue)
            {
                Write(Convert.ToInt32(variable.Value.TotalMilliseconds));
                return;
            }
            
            Write(-1);
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public void WriteSingle_s(TimeSpan variable)
        {
            Write((float)variable.TotalSeconds);
        }

        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public void WriteSingle_ms(TimeSpan variable)
        {
            Write((float)variable.TotalMilliseconds);
        }

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

                string logProgress = $"[{nodeType.FullName.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::")}] {counter + 1}/{count} ({watch.Elapsed.TotalMilliseconds}ms)";
                if (GBX == null || !GBX.ID.HasValue || CMwNod.Remap(GBX.ID.Value) != node.ID)
                    logProgress = "~ " + logProgress;

                Log.Write(logProgress, ConsoleColor.Magenta);

                if (counter != count - 1)
                    Log.Push(node.Chunks.Count + 2);

                counter += 1;
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PropertyNullException"></exception>
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        private void WriteAny(object any)
        {
            if (any is null)
                throw new ArgumentNullException(nameof(any));

            switch (any)
            {
                case byte byteValue: Write(byteValue); break;
                case short shortValue: Write(shortValue); break;
                case int intValue: Write(intValue); break;
                case long longValue: Write(longValue); break;
                case float floatValue: Write(floatValue); break;
                case string stringValue: Write(stringValue); break;
                case sbyte sbyteValue: Write(sbyteValue); break;
                case ushort ushortValue: Write(ushortValue); break;
                case uint uintValue: Write(uintValue); break;
                case ulong ulongValue: Write(ulongValue); break;
                case Byte3 byte3Value: Write(byte3Value); break;
                case Vec2 vec2Value: Write(vec2Value); break;
                case Vec3 vec3Value: Write(vec3Value); break;
                case Vec4 vec4Value: Write(vec4Value); break;
                case Int2 int2Value: Write(int2Value); break;
                case Int3 int3Value: Write(int3Value); break;
                case Id idValue: Write(idValue); break;
                case Ident identValue: Write(identValue); break;
                default: throw new NotSupportedException($"{any.GetType()} is not supported for Read<T>.");
            }
        }
    }
}
