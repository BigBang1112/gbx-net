using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    /// <summary>
    /// Provides single-method reading and writing by wrapping <see cref="GameBoxReader"/> and <see cref="GameBoxWriter"/> depending on the mode.
    /// </summary>
    public class GameBoxReaderWriter
    {
        /// <summary>
        /// Reader component of the reader-writer. This will be null if <see cref="Mode"/> is <see cref="GameBoxReaderWriterMode.Write"/>.
        /// </summary>
        public GameBoxReader Reader { get; }

        /// <summary>
        /// Writer component of the reader-writer. This will be null if <see cref="Mode"/> is <see cref="GameBoxReaderWriterMode.Read"/>.
        /// </summary>
        public GameBoxWriter Writer { get; }

        /// <summary>
        /// Mode of the reader-writer.
        /// </summary>
        public GameBoxReaderWriterMode Mode
        {
            get
            {
                if (Reader != null)
                    return GameBoxReaderWriterMode.Read;
                if (Writer != null)
                    return GameBoxReaderWriterMode.Write;
                return default;
            }
        }

        /// <summary>
        /// Constructs a reader-writer in a reader mode.
        /// </summary>
        /// <param name="reader">Reader to use.</param>
        public GameBoxReaderWriter(GameBoxReader reader) => Reader = reader;

        /// <summary>
        /// Constructs a reader-writer in a writer mode.
        /// </summary>
        /// <param name="writer">Writer to use.</param>
        public GameBoxReaderWriter(GameBoxWriter writer) => Writer = writer;

        public T[] Array<T>(T[] array, int count) where T : struct
        {
            if (Reader != null) return Reader.ReadArray<T>(count);
            else if (Writer != null) Writer.Write(array);
            return array;
        }

        public void Array<T>(ref T[] array, int count) where T : struct
        {
            array = Array(array, count);
        }

        public T[] Array<T>(T[] array) where T : struct
        {
            if (Reader != null) return Reader.ReadArray<T>();
            else if (Writer != null)
            {
                Writer.Write(array.Length);
                Writer.Write(array);
            }
            return array;
        }

        public void Array<T>(ref T[] array) where T : struct
        {
            array = Array(array);
        }

        public T[] Array<T>(T[] array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadArray(forLoopRead);
            else if (Writer != null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[] array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[] Array<T>(T[] array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadArray(forLoopRead);
            else if (Writer != null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[] array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[] Array<T>(T[] array, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadArray(forLoopRead);
            else if (Writer != null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[] array, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[] Array<T>(T[] array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadArray(forLoopRead);
            else if (Writer != null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[] array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[] ArrayNode<T>(T[] array) where T : CMwNod
        {
            return Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public void ArrayNode<T>(ref T[] array) where T : CMwNod
        {
            array = Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable) where T : struct
        {
            return Array(enumerable?.ToArray());
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable) where T : struct
        {
            enumerable = Enumerable(enumerable);
        }

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T> EnumerableNode<T>(IEnumerable<T> enumerable) where T : CMwNod
        {
            return Enumerable(enumerable, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public void EnumerableNode<T>(ref IEnumerable<T> enumerable) where T : CMwNod
        {
            enumerable = Enumerable(enumerable, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public IList<T> List<T>(IList<T> list) where T : struct
        {
            return Array(list?.ToArray());
        }

        public void List<T>(ref IList<T> list) where T : struct
        {
            list = List(list);
        }

        public IList<T> List<T>(IList<T> list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadList(forLoopRead);
            else if (Writer != null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T> list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T> List<T>(IList<T> list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadList(forLoopRead);
            else if (Writer != null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T> list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T> List<T>(IList<T> list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadList(forLoopRead);
            else if (Writer != null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T> list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T> List<T>(IList<T> list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadList(forLoopRead);
            else if (Writer != null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T> list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T> ListNode<T>(IList<T> list) where T : CMwNod
        {
            return List(list,
                r => r.ReadNodeRef<T>(),
                (x, w) => w.Write(x));
        }

        public void ListNode<T>(ref IList<T> list) where T : CMwNod
        {
            list = List(list,
                r => r.ReadNodeRef<T>(),
                (x, w) => w.Write(x));
        }

        public IDictionary<TKey, TValue> Dictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            if (Reader != null) return Reader.ReadDictionary<TKey, TValue>();
            else if (Writer != null) Writer.Write(dictionary);
            return dictionary;
        }

        public void Dictionary<TKey, TValue>(ref IDictionary<TKey, TValue> dictionary)
        {
            dictionary = Dictionary(dictionary);
        }

        public IDictionary<TKey, TValue> DictionaryNode<TKey, TValue>(IDictionary<TKey, TValue> dictionary) where TValue : CMwNod
        {
            if (Reader != null) return Reader.ReadDictionaryNode<TKey, TValue>();
            else if (Writer != null) Writer.WriteDictionaryNode(dictionary);
            return dictionary;
        }

        public void DictionaryNode<TKey, TValue>(ref IDictionary<TKey, TValue> dictionary) where TValue : CMwNod
        {
            dictionary = DictionaryNode(dictionary);
        }

        public bool Boolean(bool variable, bool asByte)
        {
            if (Reader != null) return Reader.ReadBoolean(asByte);
            else if (Writer != null) Writer.Write(variable, asByte);
            return variable;
        }

        public void Boolean(ref bool variable, bool asByte)
        {
            variable = Boolean(variable, asByte);
        }

        public void Boolean(ref bool? variable, bool asByte)
        {
            variable = Boolean(variable.GetValueOrDefault(), asByte);
        }

        public bool Boolean(bool variable)
        {
            return Boolean(variable, false);
        }

        public void Boolean(ref bool variable)
        {
            variable = Boolean(variable);
        }

        public void Boolean(ref bool? variable)
        {
            variable = Boolean(variable.GetValueOrDefault());
        }

        public void Boolean()
        {
            _= Boolean(default);
        }

        public byte Byte(byte variable)
        {
            if (Reader != null) return Reader.ReadByte();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Byte(ref byte variable)
        {
            variable = Byte(variable);
        }

        public void Byte(ref byte? variable)
        {
            variable = Byte(variable.GetValueOrDefault());
        }

        public int Byte(int variable)
        {
            return Byte((byte)variable);
        }

        public void Byte(ref int variable)
        {
            variable = Byte(variable);
        }

        public void Byte(ref int? variable)
        {
            variable = Byte(variable.GetValueOrDefault());
        }

        public void Byte()
        {
            _ = Byte(default);
        }

        public Byte3 Byte3(Byte3 variable)
        {
            if (Reader != null) return Reader.ReadByte3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Byte3(ref Byte3 variable)
        {
            variable = Byte3(variable);
        }

        public void Byte3(ref Byte3? variable)
        {
            variable = Byte3(variable.GetValueOrDefault());
        }

        public byte[] Bytes(byte[] variable, int count)
        {
            if (Reader != null) return Reader.ReadBytes(count);
            else if (Writer != null) Writer.Write(variable, 0, count);
            return variable;
        }

        public void Bytes(ref byte[] variable, int count)
        {
            variable = Bytes(variable, count);
        }

        public byte[] Bytes(byte[] variable)
        {
            if (Reader != null) return Reader.ReadBytes();
            else if (Writer != null)
            {
                Writer.Write(variable.Length);
                Writer.Write(variable);
            }
            return variable;
        }

        public void Bytes(ref byte[] variable)
        {
            variable = Bytes(variable);
        }

        public void Bytes()
        {
            _ = Bytes(default);
        }

        public FileRef FileRef(FileRef variable)
        {
            if (Reader != null) return Reader.ReadFileRef();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void FileRef(ref FileRef variable)
        {
            variable = FileRef(variable);
        }

        public void FileRef()
        {
            _ = FileRef(default);
        }

        public short Int16(short variable)
        {
            if (Reader != null) return Reader.ReadInt16();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int16(ref short variable)
        {
            variable = Int16(variable);
        }

        public void Int16(ref short? variable)
        {
            variable = Int16(variable.GetValueOrDefault());
        }

        public void Int16()
        {
            _ = Int16(default);
        }

        public int Int32(int variable)
        {
            if (Reader != null) return Reader.ReadInt32();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int32(ref int variable)
        {
            variable = Int32(variable);
        }

        public void Int32(ref int? variable)
        {
            variable = Int32(variable.GetValueOrDefault());
        }

        public void Int32()
        {
            _ = Int32(default);
        }

        public TimeSpan Int32_s(TimeSpan variable)
        {
            if (Reader != null) return Reader.ReadInt32_s();
            else if (Writer != null) Writer.WriteInt32_s(variable);
            return variable;
        }

        public void Int32_s(ref TimeSpan variable)
        {
            variable = Int32_s(variable);
        }

        public TimeSpan Int32_ms(TimeSpan variable)
        {
            if (Reader != null) return Reader.ReadInt32_ms();
            else if (Writer != null) Writer.WriteInt32_ms(variable);
            return variable;
        }

        public void Int32_ms(ref TimeSpan variable)
        {
            variable = Int32_ms(variable);
        }

        public TimeSpan? Int32_sn(TimeSpan? variable)
        {
            if (Reader != null) return Reader.ReadInt32_sn();
            else if (Writer != null) Writer.WriteInt32_sn(variable);
            return variable;
        }

        public void Int32_sn(ref TimeSpan? variable)
        {
            variable = Int32_sn(variable);
        }

        public TimeSpan? Int32_msn(TimeSpan? variable)
        {
            if (Reader != null) return Reader.ReadInt32_msn();
            else if (Writer != null) Writer.WriteInt32_msn(variable);
            return variable;
        }

        public void Int32_msn(ref TimeSpan? variable)
        {
            variable = Int32_msn(variable);
        }

        public void Int32_s()
        {
            _ = Int32_s(default);
        }

        public void Int32_ms()
        {
            _ = Int32_ms(default);
        }

        public void Int32_sn()
        {
            _ = Int32_sn(default);
        }

        public void Int32_msn()
        {
            _ = Int32_msn(default);
        }

        public long Int64(long variable)
        {
            if (Reader != null) return Reader.ReadInt64();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int64(ref long variable)
        {
            variable = Int64(variable);
        }

        public void Int64(ref long? variable)
        {
            variable = Int64(variable.GetValueOrDefault());
        }

        public void Int64()
        {
            _ = Int64(default);
        }

        public ushort UInt16(ushort variable)
        {
            if (Reader != null) return Reader.ReadUInt16();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void UInt16(ref ushort variable)
        {
            variable = UInt16(variable);
        }

        public void UInt16(ref ushort? variable)
        {
            variable = UInt16(variable.GetValueOrDefault());
        }

        public void UInt16()
        {
            _ = UInt16(default);
        }

        public uint UInt32(uint variable)
        {
            if (Reader != null) return Reader.ReadUInt32();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void UInt32(ref uint variable)
        {
            variable = UInt32(variable);
        }

        public void UInt32(ref uint? variable)
        {
            variable = UInt32(variable.GetValueOrDefault());
        }

        public void UInt32()
        {
            _ = UInt32(default);
        }

        public ulong UInt64(ulong variable)
        {
            if (Reader != null) return Reader.ReadUInt64();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void UInt64(ref ulong variable)
        {
            variable = UInt64(variable);
        }

        public void UInt64(ref ulong? variable)
        {
            variable = UInt64(variable.GetValueOrDefault());
        }

        public void UInt64()
        {
            _ = UInt64(default);
        }

        public BigInteger Int128(BigInteger variable, int byteLength)
        {
            if (Reader != null) return Reader.ReadBigInt(byteLength);
            else if (Writer != null) Writer.WriteBigInt(variable);
            return variable;
        }

        public BigInteger BigInt(BigInteger variable, int byteLength)
        {
            if (Reader != null) return Reader.ReadBigInt(byteLength);
            else if (Writer != null) Writer.WriteBigInt(variable);
            return variable;
        }

        public void BigInt(ref BigInteger variable, int byteLength)
        {
            variable = BigInt(variable, byteLength);
        }

        public void BigInt(ref BigInteger? variable, int byteLength)
        {
            variable = BigInt(variable.GetValueOrDefault(), byteLength);
        }

        public void BigInt(int byteLength)
        {
            _ = BigInt(default, byteLength);
        }

        public BigInteger Int128(BigInteger variable)
        {
            return BigInt(variable, 16);
        }

        public void Int128(ref BigInteger variable)
        {
            BigInt(ref variable, 16);
        }

        public void Int128(ref BigInteger? variable)
        {
            BigInt(ref variable, 16);
        }

        public void Int128()
        {
            _ = Int128(default, 16);
        }

        public Int2 Int2(Int2 variable)
        {
            if (Reader != null) return Reader.ReadInt2();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int2(ref Int2 variable)
        {
            variable = Int2(variable);
        }

        public void Int2(ref Int2? variable)
        {
            variable = Int2(variable.GetValueOrDefault());
        }

        public void Int2()
        {
            _ = Int2(default);
        }

        public Int3 Int3(Int3 variable)
        {
            if (Reader != null) return Reader.ReadInt3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int3(ref Int3 variable)
        {
            variable = Int3(variable);
        }

        public void Int3(ref Int3? variable)
        {
            variable = Int3(variable.GetValueOrDefault());
        }

        public void Int3()
        {
            _ = Int3(default);
        }

        public string Id(string variable, ILookbackable lookbackable)
        {
            if (Reader != null) return Reader.ReadId(lookbackable);
            else if (Writer != null) Writer.Write(new Id(variable, lookbackable));
            return variable;
        }

        public void Id(ref string variable, ILookbackable lookbackable)
        {
            variable = Id(variable, lookbackable);
        }

        public string Id(string variable)
        {
            if (Reader != null) return Id(variable, Reader.Lookbackable);
            else if (Writer != null) return Id(variable, Writer.Lookbackable);
            throw new Exception();
        }

        public void Id(ref string variable)
        {
            variable = Id(variable);
        }

        public void Id()
        {
            _ = Id(default);
        }

        public Ident Ident(Ident variable, ILookbackable lookbackable)
        {
            if (Reader != null) return Reader.ReadIdent(lookbackable);
            else if (Writer != null) Writer.Write(variable, lookbackable);
            return variable;
        }

        public void Ident(ref Ident variable, ILookbackable lookbackable)
        {
            variable = Ident(variable, lookbackable);
        }

        public Ident Ident(Ident variable)
        {
            if (Reader != null) return Ident(variable, Reader.Lookbackable);
            else if (Writer != null) return Ident(variable, Writer.Lookbackable);
            throw new Exception();
        }

        public void Ident(ref Ident variable)
        {
            variable = Ident(variable);
        }

        public void Ident()
        {
            _ = Ident(default);
        }

        public CMwNod NodeRef(CMwNod variable, GameBoxBody body)
        {
            if (Reader != null) return Reader.ReadNodeRef(body);
            else if (Writer != null) Writer.Write(variable, body);
            return variable;
        }

        public void NodeRef(ref CMwNod variable, GameBoxBody body)
        {
            variable = NodeRef(variable, body);
        }

        public CMwNod NodeRef(CMwNod variable)
        {
            if (Reader != null) return NodeRef(variable, Reader.Body);
            else if (Writer != null) return NodeRef(variable, Writer.Body);
            throw new Exception();
        }

        public void NodeRef(ref CMwNod variable)
        {
            variable = NodeRef(variable);
        }

        public void NodeRef()
        {
            _ = NodeRef(default);
        }

        public T NodeRef<T>(T variable, GameBoxBody body) where T : CMwNod
        {
            if (Reader != null) return Reader.ReadNodeRef<T>(body);
            else if (Writer != null) Writer.Write(variable, body);
            return variable;
        }

        public void NodeRef<T>(ref T variable, GameBoxBody body) where T : CMwNod
        {
            variable = NodeRef(variable, body);
        }

        public T NodeRef<T>(T variable) where T : CMwNod
        {
            if (Reader != null) return NodeRef(variable, Reader.Body);
            else if (Writer != null) return NodeRef(variable, Writer.Body);
            else throw new Exception();
        }

        public void NodeRef<T>(ref T variable) where T : CMwNod
        {
            variable = NodeRef(variable);
        }

        public float Single(float variable)
        {
            if (Reader != null) return Reader.ReadSingle();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Single(ref float variable)
        {
            variable = Single(variable);
        }

        public void Single(ref float? variable)
        {
            variable = Single(variable.GetValueOrDefault());
        }

        public void Single()
        {
            _ = Single(default);
        }

        public TimeSpan Single_s(TimeSpan variable)
        {
            if (Reader != null) return Reader.ReadSingle_s();
            else if (Writer != null) Writer.WriteSingle_s(variable);
            return variable;
        }

        public void Single_s(ref TimeSpan variable)
        {
            variable = Single_s(variable);
        }

        public TimeSpan Single_ms(TimeSpan variable)
        {
            if (Reader != null) return Reader.ReadSingle_ms();
            else if (Writer != null) Writer.WriteSingle_ms(variable);
            return variable;
        }

        public void Single_ms(ref TimeSpan variable)
        {
            variable = Single_ms(variable);
        }

        public TimeSpan? Single_sn(TimeSpan? variable)
        {
            if (Reader != null) return Reader.ReadSingle_sn();
            else if (Writer != null) Writer.WriteSingle_sn(variable);
            return variable;
        }

        public void Single_sn(ref TimeSpan? variable)
        {
            variable = Single_sn(variable);
        }

        public TimeSpan? Single_msn(TimeSpan? variable)
        {
            if (Reader != null) return Reader.ReadSingle_msn();
            else if (Writer != null) Writer.WriteSingle_msn(variable);
            return variable;
        }

        public void Single_msn(ref TimeSpan? variable)
        {
            variable = Single_msn(variable);
        }

        public void Single_s()
        {
            _ = Single_s(default);
        }

        public void Single_ms()
        {
            _ = Single_ms(default);
        }

        public void Single_sn()
        {
            _ = Single_sn(default);
        }

        public void Single_msn()
        {
            _ = Single_msn(default);
        }

        public string String(string variable, StringLengthPrefix readPrefix)
        {
            if (Reader != null) return Reader.ReadString(readPrefix);
            else if (Writer != null) Writer.Write(variable, readPrefix);
            return variable;
        }

        public void String(ref string variable, StringLengthPrefix readPrefix)
        {
            variable = String(variable, readPrefix);
        }

        public string String(string variable)
        {
            return String(variable, StringLengthPrefix.Int32);
        }

        public void String(ref string variable)
        {
            variable = String(variable);
        }

        public void String()
        {
            _ = String(default);
        }

        public Vec2 Vec2(Vec2 variable)
        {
            if (Reader != null) return Reader.ReadVec2();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Vec2(ref Vec2 variable)
        {
            variable = Vec2(variable);
        }

        public void Vec2(ref Vec2? variable)
        {
            variable = Vec2(variable.GetValueOrDefault());
        }

        public void Vec2()
        {
            _ = Vec2(default);
        }

        public Vec3 Vec3(Vec3 variable)
        {
            if (Reader != null) return Reader.ReadVec3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Vec3(ref Vec3 variable)
        {
            variable = Vec3(variable);
        }

        public void Vec3(ref Vec3? variable)
        {
            variable = Vec3(variable.GetValueOrDefault());
        }

        public void Vec3()
        {
            _ = Vec3(default);
        }

        public void EnumByte<T>(ref T variable) where T : struct, Enum
        {
            variable = (T)(object)Convert.ToInt32(Byte((byte)Convert.ToInt32(variable)));
        }

        public void EnumInt32<T>(ref T variable) where T : struct, Enum
        {
            variable = (T)(object)Int32((int)(object)variable);
        }

        public void EnumInt32<T>(ref T? variable) where T : struct, Enum
        {
            variable = (T)(object)Int32((variable as object as int?).GetValueOrDefault());
        }

        public void UntilFacade(MemoryStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadUntilFacade());
            }
            else if (Writer != null)
            {
                var buffer = new byte[stream.Length - stream.Position];
                stream.Read(buffer, 0, buffer.Length);
                Writer.WriteBytes(buffer);
            }
        }
    }

    /// <summary>
    /// Reader-writer mode.
    /// </summary>
    public enum GameBoxReaderWriterMode
    {
        Read,
        Write
    }
}
