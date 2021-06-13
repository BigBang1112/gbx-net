using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public T[] Array<T>(T[] array, int count)
        {
            if (Reader != null) return Reader.ReadArray<T>(count);
            else if (Writer != null) Writer.Write(array);
            return array;
        }

        public void Array<T>(ref T[] array, int count)
        {
            array = Array(array, count);
        }

        public T[] Array<T>(T[] array)
        {
            if (Reader != null) return Reader.ReadArray<T>();
            else if (Writer != null)
            {
                Writer.Write(array.Length);
                Writer.Write(array);
            }
            return array;
        }

        public void Array<T>(ref T[] array)
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

        public IEnumerable<T> Enumerable<T>(IEnumerable<T> enumerable) where T : CMwNod
        {
            return Array(enumerable?.ToArray());
        }

        public void Enumerable<T>(ref IEnumerable<T> enumerable) where T : CMwNod
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

        public List<T> List<T>(List<T> list) where T : CMwNod
        {
            return Enumerable(list).ToList();
        }

        public void List<T>(ref List<T> list) where T : CMwNod
        {
            list = List(list);
        }

        public List<T> List<T>(List<T> list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            return Enumerable(list, forLoopRead, forLoopWrite).ToList();
        }

        public void List<T>(ref List<T> list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public List<T> List<T>(List<T> list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Enumerable(list, forLoopRead, forLoopWrite).ToList();
        }

        public void List<T>(ref List<T> list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public List<T> List<T>(List<T> list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            return Enumerable(list, forLoopRead, forLoopWrite).ToList();
        }

        public void List<T>(ref List<T> list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public List<T> List<T>(List<T> list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Enumerable(list, forLoopRead, forLoopWrite).ToList();
        }

        public void List<T>(ref List<T> list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public List<T> ListNode<T>(List<T> list) where T : CMwNod
        {
            return List(list, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public void ListNode<T>(ref List<T> list) where T : CMwNod
        {
            list = List(list, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            if (Reader != null) return Reader.ReadDictionary<TKey, TValue>();
            else if (Writer != null) Writer.Write(dictionary);
            return dictionary;
        }

        public void Dictionary<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary)
        {
            dictionary = Dictionary(dictionary);
        }

        public Dictionary<TKey, TValue> NodeDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TValue : CMwNod
        {
            if (Reader != null) return Reader.ReadNodeDictionary<TKey, TValue>();
            else if (Writer != null) Writer.WriteNodeDictionary(dictionary);
            return dictionary;
        }

        public void NodeDictionary<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary) where TValue : CMwNod
        {
            dictionary = NodeDictionary(dictionary);
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

        public TimeSpan? TimeSpan32(TimeSpan? variable)
        {
            if (Reader != null) return Reader.ReadTimeSpan();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void TimeSpan32(ref TimeSpan? variable)
        {
            variable = TimeSpan32(variable);
        }

        public void TimeSpan32()
        {
            _ = TimeSpan32(default);
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
