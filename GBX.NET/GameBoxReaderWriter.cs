using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET
{
    public class GameBoxReaderWriter
    {
        public GameBoxReader Reader { get; }
        public GameBoxWriter Writer { get; }

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

        public GameBoxReaderWriter(GameBoxReader reader) => Reader = reader;
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

        public void Array<T>(UnknownStream stream, int count)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadArray<T>(count));
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadArray<T>(count));
            }
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

        public T[] ArrayNode<T>(T[] array) where T : Node
        {
            return Array(array, i => Reader.ReadNodeRef<T>(), x => Writer.Write(x));
        }

        public void ArrayNode<T>(ref T[] array) where T : Node
        {
            array = Array(array, i => Reader.ReadNodeRef<T>(), x => Writer.Write(x));
        }

        public Dictionary<int, TValue> DictionaryNode<TValue>(Dictionary<int, TValue> dictionary) where TValue : Node
        {
            if (Reader != null) return Reader.ReadDictionaryNode<TValue>();
            else if (Writer != null) Writer.Write(dictionary);
            return dictionary;
        }

        public void DictionaryNode<TValue>(ref Dictionary<int, TValue> dictionary) where TValue : Node
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

        public bool Boolean(bool variable)
        {
            return Boolean(variable, false);
        }

        public void Boolean(ref bool variable)
        {
            variable = Boolean(variable);
        }

        public void Boolean(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadBoolean());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadBoolean());
            }
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

        public void Byte(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadByte());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadByte());
            }
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

        public void Byte3(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadByte3());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadByte3());
            }
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

        public void Bytes(UnknownStream stream, int count)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadBytes(count));
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadBytes(count));
            }
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

        public void FileRef(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadFileRef());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadFileRef());
            }
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

        public void Int16(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadInt16());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadInt16());
            }
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

        public void Int32(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadInt32());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadInt32());
            }
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

        public void Int64(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadInt64());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadInt64());
            }
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

        public void UInt16(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadUInt16());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadUInt16());
            }
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

        public void UInt32(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadUInt32());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadUInt32());
            }
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

        public void UInt64(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadUInt64());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadUInt64());
            }
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

        public void Int2(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadInt2());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadInt2());
            }
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

        public void Int3(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadInt3());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadInt3());
            }
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

        public void Id(UnknownStream stream, ILookbackable lookbackable)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write((string)Reader.ReadId(lookbackable));
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(new Id(r.ReadString(), lookbackable));
            }
        }

        public void Id(UnknownStream stream)
        {
            if (Reader != null) Id(stream, Reader.Lookbackable);
            else if (Writer != null) Id(stream, Writer.Lookbackable);
            else throw new Exception();
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

        public void Ident(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadIdent());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadIdent());
            }
        }

        public Node NodeRef(Node variable, IGameBoxBody body)
        {
            if (Reader != null) return Reader.ReadNodeRef(body);
            else if (Writer != null) Writer.Write(variable, body);
            return variable;
        }

        public void NodeRef(ref Node variable, IGameBoxBody body)
        {
            variable = NodeRef(variable, body);
        }

        public Node NodeRef(Node variable)
        {
            if (Reader != null) return NodeRef(variable, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) return NodeRef(variable, (IGameBoxBody)Writer.Lookbackable);
            throw new Exception();
        }

        public void NodeRef(ref Node variable)
        {
            variable = NodeRef(variable);
        }

        public void NodeRef(UnknownStream stream, IGameBoxBody body)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, (ILookbackable)body))
                    w.Write(Reader.ReadNodeRef(), body);
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, (ILookbackable)body))
                    Writer.Write(r.ReadNodeRef(), body);
            }
        }

        public void NodeRef(UnknownStream stream)
        {
            if (Reader != null) NodeRef(stream, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) NodeRef(stream, (IGameBoxBody)Writer.Lookbackable);
            else throw new Exception();
        }

        public T NodeRef<T>(T variable, IGameBoxBody body) where T : Node
        {
            if (Reader != null) return Reader.ReadNodeRef<T>(body);
            else if (Writer != null) Writer.Write(variable, body);
            return (T)variable;
        }

        public void NodeRef<T>(ref T variable, IGameBoxBody body) where T : Node
        {
            variable = NodeRef(variable, body);
        }

        public T NodeRef<T>(T variable) where T : Node
        {
            if (Reader != null) return NodeRef(variable, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) return NodeRef(variable, (IGameBoxBody)Writer.Lookbackable);
            else throw new Exception();
        }

        public void NodeRef<T>(ref T variable) where T : Node
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

        public void Single(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadSingle());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadSingle());
            }
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

        public void String(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadString());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadString());
            }
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

        public void Vec2(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadVec2());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadVec2());
            }
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

        public void Vec3(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadVec3());
            }
            else if (Writer != null)
            {
                using (var r = new GameBoxReader(stream, Writer.Lookbackable))
                    Writer.Write(r.ReadVec3());
            }
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

        public void EnumByte<T>(ref T variable) where T : Enum
        {
            variable = (T)(object)Byte((byte)(object)variable);
        }

        public void EnumInt32<T>(ref T variable) where T : Enum
        {
            variable = (T)(object)Int32((int)(object)variable);
        }

        public void TillFacade(UnknownStream stream)
        {
            if (Reader != null)
            {
                using (var w = new GameBoxWriter(stream, Reader.Lookbackable))
                    w.Write(Reader.ReadTillFacade());
            }
            else if (Writer != null)
            {
                var buffer = new byte[stream.Length - stream.Position];
                stream.Read(buffer, 0, buffer.Length);
                Writer.WriteBytes(buffer);
            }
        }
    }

    public enum GameBoxReaderWriterMode
    {
        Read,
        Write
    }
}
