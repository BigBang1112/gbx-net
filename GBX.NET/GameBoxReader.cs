using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace GBX.NET
{
    public class GameBoxReader : BinaryReader
    {
        public ILookbackable Lookbackable { get; internal set; }
        public Chunk Chunk { get; internal set; }

        public GameBoxReader(Stream input) : base(input, Encoding.UTF8, true)
        {

        }

        public GameBoxReader(Stream input, ILookbackable lookbackable) : this(input)
        {
            Lookbackable = lookbackable;
        }

        public byte[] ReadBytes()
        {
            return ReadBytes(ReadInt32());
        }

        public string ReadString(StringLengthPrefix readPrefix)
        {
            int length;
            if (readPrefix == StringLengthPrefix.Byte)
                length = ReadByte();
            else if (readPrefix == StringLengthPrefix.Int32)
                length = ReadInt32();
            else
                throw new Exception("Can't read string without knowing its length.");
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as <see cref="int"/>.
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            return ReadString(StringLengthPrefix.Int32);
        }

        public string ReadString(int length)
        {
            return new string(ReadChars(length));
        }

        /// <summary>
        /// Reads the next <see cref="int"/> from the current stream, casts it as <see cref="bool"/> and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <returns></returns>
        public override bool ReadBoolean()
        {
            return Convert.ToBoolean(ReadInt32());
        }

        public bool ReadBoolean(bool asByte)
        {
            if (asByte) return base.ReadBoolean();
            else return ReadBoolean();
        }

        public Id ReadId(ILookbackable lookbackable)
        {
            if (!lookbackable.IdVersion.HasValue)
            {
                lookbackable.IdVersion = ReadInt32();

                if ((lookbackable.IdVersion & 0xC0000000) > 10) // Edge-case scenario where Id doesn't have a version for whatever reason (can be multiple)
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
            else if ((index & 0x3FFF) == 0x3FFF)
            {
                switch(index >> 30)
                {
                    case 2:
                        return new Id("Unassigned", lookbackable);
                    case 3:
                        return new Id("", lookbackable);
                    default:
                        throw new Exception();
                }
            }
            else if (index >> 30 == 0)
            {
                if (Id.CollectionIDs.TryGetValue((int)index, out string val))
                    return new Id(index.ToString(), lookbackable);
                else
                    return new Id("???", lookbackable);
            }
            else if (lookbackable.IdStrings.Count > (index & 0x3FFF) - 1)
                return new Id(lookbackable.IdStrings[(int)(index & 0x3FFF) - 1], lookbackable);
            else
                return new Id("", lookbackable);
        }

        public Id ReadId()
        {
            return ReadId(Lookbackable);
        }

        public Ident ReadIdent(ILookbackable lookbackable)
        {
            var id = ReadId(lookbackable);
            var collection = ReadId(lookbackable);
            var author = ReadId(lookbackable);

            return new Ident(id, collection, author);
        }

        public Ident ReadIdent()
        {
            return ReadIdent(Lookbackable);
        }

        public T ReadNodeRef<T>(IGameBoxBody body) where T : Node
        {
            var index = ReadInt32() - 1; // GBX seems to start the index at 1

            if (index >= 0 && (!body.AuxilaryNodes.ContainsKey(index) || body.AuxilaryNodes[index] == null)) // If index is 0 or bigger and the node wasn't read yet, or is null
                body.AuxilaryNodes[index] = Node.Parse<T>(this);

            if (index < 0) // If aux node index is below 0 then there's not much to solve
                return null;
            body.AuxilaryNodes.TryGetValue(index, out Node n); // Tries to get the available node from index
            T nod = n as T;
            if (nod == null) // But sometimes it indexes the node reference that is further in the expected indexes
                return (T)body.AuxilaryNodes.Last().Value; // So it grabs the last one instead, needs to be further tested
            else // If the node is presented at the index, then it's simple
                return nod;
        }

        public T ReadNodeRef<T>() where T : Node
        {
            return ReadNodeRef<T>((IGameBoxBody)Lookbackable);
        }

        public Node ReadNodeRef(IGameBoxBody body)
        {
            return ReadNodeRef<Node>(body);
        }

        public Node ReadNodeRef()
        {
            return ReadNodeRef<Node>((IGameBoxBody)Lookbackable);
        }

        public FileRef ReadFileRef()
        {
            var version = ReadByte();

            byte[] checksum = null;
            string locatorUrl = "";

            if (version >= 3)
                checksum = ReadBytes(32);

            var filePath = ReadString();

            if ((filePath.Length > 0 && version >= 1) || version >= 3)
                locatorUrl = ReadString();

            return new FileRef(version, checksum, filePath, locatorUrl);
        }

        public T[] ReadArray<T>(int count)
        {
            var buffer = ReadBytes(count * Marshal.SizeOf(default(T)));
            var array = new T[count];
            Buffer.BlockCopy(buffer, 0, array, 0, buffer.Length);
            return array;
        }

        public T[] ReadArray<T>()
        {
            return ReadArray<T>(ReadInt32());
        }

        /// <summary>
        /// First reads an <see cref="int"/> representing the length, then does a for loop with this length, each element requiring to return an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="forLoop">Each element.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        public T[] ReadArray<T>(Func<int, T> forLoop)
        {
            return ReadArray(ReadInt32(), forLoop);
        }

        public T[] ReadArray<T>(int length, Func<int, T> forLoop)
        {
            var result = new T[length];

            for (var i = 0; i < length; i++)
                result[i] = forLoop.Invoke(i);

            return result;
        }

        internal T[] ReadArray<T>(int length, Func<int, GameBoxReader, T> forLoop)
        {
            var result = new T[length];

            for (var i = 0; i < length; i++)
                result[i] = forLoop.Invoke(i, this);

            return result;
        }

        internal T[] ReadArray<T>(Func<int, GameBoxReader, T> forLoop)
        {
            return ReadArray(ReadInt32(), forLoop);
        }

        /// <summary>
        /// Reads values in a dictionary kind (first key, then value). For node dictionaries, use the <see cref="ReadNodeDictionary{TKey, TValue}"/> method for better performance.
        /// </summary>
        /// <typeparam name="TKey">One of the supported types of <see cref="Read{T}"/>.</typeparam>
        /// <typeparam name="TValue">One of the supported types of <see cref="Read{T}"/>.</typeparam>
        /// <returns></returns>
        public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>()
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
        /// <returns></returns>
        public Dictionary<TKey, TValue> ReadNodeDictionary<TKey, TValue>() where TValue : Node
        {
            var dictionary = new Dictionary<TKey, TValue>();

            var length = ReadInt32();

            for (var i = 0; i < length; i++)
            {
                var key = Read<TKey>();
                var value = ReadNodeRef<TValue>();

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public Vec2 ReadVec2()
        {
            var floats = ReadArray<float>(2);
            return new Vec2(floats[0], floats[1]);
        }

        public Vec3 ReadVec3()
        {
            var floats = ReadArray<float>(3);
            return new Vec3(floats[0], floats[1], floats[2]);
        }

        public Vec4 ReadVec4()
        {
            var floats = ReadArray<float>(4);
            return new Vec4(floats[0], floats[1], floats[2], floats[3]);
        }

        public Int3 ReadInt3()
        {
            var ints = ReadArray<int>(3);
            return (ints[0], ints[1], ints[2]);
        }

        public Int2 ReadInt2()
        {
            var ints = ReadArray<int>(2);
            return (ints[0], ints[1]);
        }

        public Byte3 ReadByte3()
        {
            var bytes = ReadBytes(3);
            return (bytes[0], bytes[1], bytes[2]);
        }

        public TimeSpan? ReadTimeSpan()
        {
            var time = ReadInt32();
            if (time < 0)
                return null;
            return TimeSpan.FromMilliseconds(time);
        }

        public byte[] ReadTill(uint uint32)
        {
            List<byte> bytes = new List<byte>();
            while (PeekUInt32() != uint32)
                bytes.Add(ReadByte());
            return bytes.ToArray();
        }

        public byte[] ReadTillFacade()
        {
            return ReadTill(0xFACADE01);
        }

        public byte[] ReadToEnd()
        {
            return ReadBytes((int)(BaseStream.Length - BaseStream.Position));
        }

        public string ReadStringTillFacade()
        {
            return Encoding.UTF8.GetString(ReadTillFacade());
        }

        public T[] ReadArrayTillFacade<T>()
        {
            var bytes = ReadTillFacade();

            var array = new T[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            return array;
        }

        public (T1[], T2[]) ReadArrayTillFacade<T1, T2>()
        {
            var bytes = ReadTillFacade();

            var array = new T1[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T1)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            var array2 = new T2[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T2)))];
            Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);

            return (array, array2);
        }

        public (T1[], T2[], T3[]) ReadArrayTillFacade<T1, T2, T3>()
        {
            var bytes = ReadTillFacade();

            var array = new T1[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T1)))];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

            var array2 = new T2[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T2)))];
            Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);

            var array3 = new T3[(int)Math.Ceiling(bytes.Length / (float)Marshal.SizeOf(default(T3)))];
            Buffer.BlockCopy(bytes, 0, array3, 0, bytes.Length);

            return (array, array2, array3);
        }

        public uint PeekUInt32()
        {
            var result = ReadUInt32();
            BaseStream.Position -= sizeof(uint);
            return result;
        }

        public bool HasMagic(string magic)
        {
            return ReadString(magic.Length) == magic;
        }

        /// <summary>
        /// A generic read method of parameterless types for the cost of performance loss. Prefer using the pre-defined data read methods.
        /// </summary>
        /// <typeparam name="T">Type of the variable to read. Supported types are <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>,
        /// <see cref="long"/>, <see cref="float"/>, <see cref="bool"/>, <see cref="string"/>, <see cref="sbyte"/>, <see cref="ushort"/>,
        /// <see cref="uint"/>, <see cref="ulong"/>, <see cref="Byte3"/>, <see cref="Vec2"/>, <see cref="Vec3"/>,
        /// <see cref="Vec4"/>, <see cref="Int3"/>, <see cref="Id"/> and <see cref="Ident"/>.</typeparam>
        /// <returns></returns>
        private T Read<T>()
        {
            switch (typeof(T))
            {
                case Type byteType when byteType == typeof(byte):
                    return (T)Convert.ChangeType(ReadByte(), typeof(T));
                case Type shortType when shortType == typeof(short):
                    return (T)Convert.ChangeType(ReadInt16(), typeof(T));
                case Type intType when intType == typeof(int):
                    return (T)Convert.ChangeType(ReadInt32(), typeof(T));
                case Type longType when longType == typeof(long):
                    return (T)Convert.ChangeType(ReadInt64(), typeof(T));
                case Type floatType when floatType == typeof(float):
                    return (T)Convert.ChangeType(ReadSingle(), typeof(T));
                case Type boolType when boolType == typeof(bool):
                    return (T)Convert.ChangeType(ReadBoolean(), typeof(T));
                case Type stringType when stringType == typeof(string):
                    return (T)Convert.ChangeType(ReadString(), typeof(T));
                case Type sbyteType when sbyteType == typeof(sbyte):
                    return (T)Convert.ChangeType(ReadSByte(), typeof(T));
                case Type ushortType when ushortType == typeof(ushort):
                    return (T)Convert.ChangeType(ReadUInt16(), typeof(T));
                case Type uintType when uintType == typeof(uint):
                    return (T)Convert.ChangeType(ReadUInt32(), typeof(T));
                case Type ulongType when ulongType == typeof(ulong):
                    return (T)Convert.ChangeType(ReadUInt64(), typeof(T));
                case Type byte3Type when byte3Type == typeof(Byte3):
                    return (T)Convert.ChangeType(ReadByte3(), typeof(T));
                case Type vec2Type when vec2Type == typeof(Vec2):
                    return (T)Convert.ChangeType(ReadVec2(), typeof(T));
                case Type vec3Type when vec3Type == typeof(Vec3):
                    return (T)Convert.ChangeType(ReadVec3(), typeof(T));
                case Type vec4Type when vec4Type == typeof(Vec4):
                    return (T)Convert.ChangeType(ReadVec4(), typeof(T));
                case Type int2Type when int2Type == typeof(Int2):
                    return (T)Convert.ChangeType(ReadInt2(), typeof(T));
                case Type int3Type when int3Type == typeof(Int3):
                    return (T)Convert.ChangeType(ReadInt3(), typeof(T));
                case Type idType when idType == typeof(Id):
                    return (T)Convert.ChangeType(ReadId(), typeof(T));
                case Type identType when identType == typeof(Ident):
                    return (T)Convert.ChangeType(ReadIdent(), typeof(T));
                default:
                    throw new NotSupportedException($"{typeof(T)} is not supported for Read<T>.");
            }
        }
    }
}