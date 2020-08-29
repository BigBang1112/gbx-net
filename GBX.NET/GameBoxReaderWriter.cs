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

        public GameBoxReaderWriter(GameBoxReader reader)
        {
            Reader = reader;
        }

        public GameBoxReaderWriter(GameBoxWriter writer)
        {
            Writer = writer;
        }

        public T[] Array<T>(T[] array, int count)
        {
            if (Reader != null) return Reader.ReadArray<T>(count);
            else if (Writer != null) Writer.Write(array);
            return array;
        }

        public void Array<T>(Stream stream, int count)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadArray<T>(count));
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadArray<T>(count));
            }
        }

        public T[] Array<T>(T[] array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader != null) return Reader.ReadArray(forLoopRead);
            else if (Writer != null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public bool Boolean(bool variable, bool asByte)
        {
            if (Reader != null) return Reader.ReadBoolean(asByte);
            else if (Writer != null) Writer.Write(variable, asByte);
            return variable;
        }

        public bool Boolean(bool variable)
        {
            return Boolean(variable, false);
        }

        public void Boolean(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadBoolean());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadBoolean());
            }
        }

        public byte Byte(byte variable)
        {
            if (Reader != null) return Reader.ReadByte();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Byte(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadByte());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadByte());
            }
        }

        public Byte3 Byte3(Byte3 variable)
        {
            if (Reader != null) return Reader.ReadByte3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public byte[] Bytes(byte[] variable, int count)
        {
            if (Reader != null) return Reader.ReadBytes(count);
            else if (Writer != null) Writer.Write(variable, 0, count);
            return variable;
        }

        public void Bytes(Stream stream, int count)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadBytes(count));
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadBytes(count));
            }
        }

        public FileRef FileRef(FileRef variable)
        {
            if (Reader != null) return Reader.ReadFileRef();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public short Int16(short variable)
        {
            if (Reader != null) return Reader.ReadInt16();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int16(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadInt16());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadInt16());
            }
        }

        public int Int32(int variable)
        {
            if (Reader != null) return Reader.ReadInt32();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int32(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadInt32());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadInt32());
            }
        }

        public long Int64(long variable)
        {
            if (Reader != null) return Reader.ReadInt64();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Int64(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadInt64());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadInt64());
            }
        }

        public ushort UInt16(ushort variable)
        {
            if (Reader != null) return Reader.ReadUInt16();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public uint UInt32(uint variable)
        {
            if (Reader != null) return Reader.ReadUInt32();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void UInt32(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadUInt32());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadUInt32());
            }
        }

        public ulong UInt64(ulong variable)
        {
            if (Reader != null) return Reader.ReadUInt64();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public Int2 Int2(Int2 variable)
        {
            if (Reader != null) return Reader.ReadInt2();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public Int3 Int3(Int3 variable)
        {
            if (Reader != null) return Reader.ReadInt3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public string LookbackString(string variable, ILookbackable lookbackable)
        {
            if (Reader != null) return Reader.ReadLookbackString(lookbackable);
            else if (Writer != null) Writer.Write(new LookbackString(variable, lookbackable));
            return variable;
        }

        public string LookbackString(string variable)
        {
            if (Reader != null) return LookbackString(variable, Reader.Lookbackable);
            else if (Writer != null) return LookbackString(variable, Writer.Lookbackable);
            throw new Exception();
        }

        public void LookbackString(Stream stream, ILookbackable lookbackable)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write((string)Reader.ReadLookbackString(lookbackable));
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(new LookbackString(r.ReadString(), lookbackable));
            }
        }

        public void LookbackString(Stream stream)
        {
            if (Reader != null) LookbackString(stream, Reader.Lookbackable);
            else if (Writer != null) LookbackString(stream, Writer.Lookbackable);
            else throw new Exception();
        }

        public Meta Meta(Meta variable, ILookbackable lookbackable)
        {
            if (Reader != null) return Reader.ReadMeta(lookbackable);
            else if (Writer != null) Writer.Write(variable, lookbackable);
            return variable;
        }

        public Meta Meta(Meta variable)
        {
            if (Reader != null) return Meta(variable, Reader.Lookbackable);
            else if (Writer != null) return Meta(variable, Writer.Lookbackable);
            throw new Exception();
        }

        public Node NodeRef(Node variable, IGameBoxBody body)
        {
            if (Reader != null) return Reader.ReadNodeRef(body);
            else if (Writer != null) Writer.Write(variable, body);
            return variable;
        }

        public Node NodeRef(Node variable)
        {
            if (Reader != null) return NodeRef(variable, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) return NodeRef(variable, (IGameBoxBody)Writer.Lookbackable);
            throw new Exception();
        }

        public void NodeRef(Stream stream, IGameBoxBody body)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, body);
                w.Write(Reader.ReadNodeRef(), body);
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, body);
                Writer.Write(r.ReadNodeRef(), body);
            }
        }

        public void NodeRef(Stream stream)
        {
            if (Reader != null) NodeRef(stream, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) NodeRef(stream, (IGameBoxBody)Writer.Lookbackable);
            else throw new Exception();
        }

        public T NodeRef<T>(Node variable, bool hasInheritance, IGameBoxBody body) where T : Node
        {
            if (Reader != null) return Reader.ReadNodeRef<T>(hasInheritance, body);
            else if (Writer != null) Writer.Write(variable, body);
            return (T)variable;
        }

        public T NodeRef<T>(Node variable, bool hasInheritance) where T : Node
        {
            if (Reader != null) return NodeRef<T>(variable, hasInheritance, (IGameBoxBody)Reader.Lookbackable);
            else if (Writer != null) return NodeRef<T>(variable, hasInheritance, (IGameBoxBody)Writer.Lookbackable);
            else throw new Exception();
        }

        public T NodeRef<T>(Node variable) where T : Node
        {
            return NodeRef<T>(variable, false);
        }

        public float Single(float variable)
        {
            if (Reader != null) return Reader.ReadSingle();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Single(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadSingle());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadSingle());
            }
        }

        public string String(string variable, StringLengthPrefix readPrefix)
        {
            if (Reader != null) return Reader.ReadString(readPrefix);
            else if (Writer != null) Writer.Write(variable, readPrefix);
            return variable;
        }

        public string String(string variable)
        {
            return String(variable, StringLengthPrefix.Int32);
        }

        public void String(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadString());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadString());
            }
        }

        public Vector2 Vec2(Vector2 variable)
        {
            if (Reader != null) return Reader.ReadVec2();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public Vector3 Vec3(Vector3 variable)
        {
            if (Reader != null) return Reader.ReadVec3();
            else if (Writer != null) Writer.Write(variable);
            return variable;
        }

        public void Vec3(Stream stream)
        {
            if (Reader != null)
            {
                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadVec3());
                w.Flush();
            }
            else if (Writer != null)
            {
                using var r = new GameBoxReader(stream, Writer.Lookbackable);
                Writer.Write(r.ReadVec3());
            }
        }

        public TimeSpan? TimeSpan32(TimeSpan? variable)
        {
            if (Reader != null)
            {
                var time = Reader.ReadInt32();
                if (time < 0)
                    return null;
                return TimeSpan.FromMilliseconds(time);
            }
            else if (Writer != null)
            {
                if (variable != null && variable.HasValue)
                    Writer.Write(Convert.ToInt32(variable.Value.TotalMilliseconds));
                else Writer.Write(-1);
            }
            return variable;
        }
    }

    public enum GameBoxReaderWriterMode
    {
        Read,
        Write
    }
}
