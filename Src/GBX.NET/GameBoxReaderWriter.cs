using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Exceptions;

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
        public GameBoxReader? Reader { get; }

        /// <summary>
        /// Writer component of the reader-writer. This will be null if <see cref="Mode"/> is <see cref="GameBoxReaderWriterMode.Read"/>.
        /// </summary>
        public GameBoxWriter? Writer { get; }

        /// <summary>
        /// Mode of the reader-writer.
        /// </summary>
        public GameBoxReaderWriterMode Mode
        {
            get
            {
                if (Reader is not null)
                    return GameBoxReaderWriterMode.Read;
                if (Writer is not null)
                    return GameBoxReaderWriterMode.Write;
                throw new ThisShouldNotHappenException();
            }
        }

        /// <summary>
        /// Constructs a reader-writer in reader mode.
        /// </summary>
        /// <param name="reader">Reader to use.</param>
        public GameBoxReaderWriter(GameBoxReader reader) => Reader = reader;

        /// <summary>
        /// Constructs a reader-writer in writer mode.
        /// </summary>
        /// <param name="writer">Writer to use.</param>
        public GameBoxReaderWriter(GameBoxWriter writer) => Writer = writer;

        public bool Boolean(bool variable = default, bool asByte = false)
        {
            if (Reader is not null) return Reader.ReadBoolean(asByte);
            if (Writer is not null) Writer.Write(variable, asByte);
            return variable;
        }

        public bool? Boolean(bool? variable, bool defaultValue = default, bool asByte = false)
        {
            if (Reader is not null) return Reader.ReadBoolean(asByte);
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue), asByte);
            return variable;
        }

        public void Boolean(ref bool variable, bool asByte = false)
        {
            variable = Boolean(variable, asByte);
        }

        public void Boolean(ref bool? variable, bool defaultValue = default, bool asByte = false)
        {
            variable = Boolean(variable, defaultValue, asByte);
        }

        public byte Byte(byte variable = default)
        {
            if (Reader is not null) return Reader.ReadByte();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public byte? Byte(byte? variable, byte defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadByte();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Byte(ref byte variable)
        {
            variable = Byte(variable);
        }

        public void Byte(ref byte? variable, byte defaultValue = default)
        {
            variable = Byte(variable, defaultValue);
        }

        public int Byte(int variable)
        {
            return Byte((byte)variable);
        }

        public int? Byte(int? variable, int defaultValue = default)
        {
            return Byte(variable.HasValue ? (byte)variable.Value : null, (byte)defaultValue);
        }

        public void Byte(ref int variable)
        {
            variable = Byte(variable);
        }

        public void Byte(ref int? variable, int defaultValue = default)
        {
            variable = Byte(variable, defaultValue);
        }

        public short Int16(short variable = default)
        {
            if (Reader is not null) return Reader.ReadInt16();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public short? Int16(short? variable, short defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt16();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int16(ref short variable)
        {
            variable = Int16(variable);
        }

        public void Int16(ref short? variable, short defaultValue = default)
        {
            variable = Int16(variable, defaultValue);
        }

        public int Int32(int variable = default)
        {
            if (Reader is not null) return Reader.ReadInt32();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public int? Int32(int? variable, int defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt32();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int32(ref int variable)
        {
            variable = Int32(variable);
        }

        public void Int32(ref int? variable, int defaultValue = default)
        {
            variable = Int32(variable, defaultValue);
        }

        public TimeSpan Int32_s(TimeSpan variable = default)
        {
            if (Reader is not null) return Reader.ReadInt32_s();
            if (Writer is not null) Writer.WriteInt32_s(variable);
            return variable;
        }

        public TimeSpan? Int32_s(TimeSpan? variable, TimeSpan defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt32_s();
            if (Writer is not null) Writer.WriteInt32_s(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int32_s(ref TimeSpan variable)
        {
            variable = Int32_s(variable);
        }

        public void Int32_s(ref TimeSpan? variable, TimeSpan defaultValue = default)
        {
            variable = Int32_s(variable, defaultValue);
        }

        public TimeSpan Int32_ms(TimeSpan variable = default)
        {
            if (Reader is not null) return Reader.ReadInt32_ms();
            if (Writer is not null) Writer.WriteInt32_ms(variable);
            return variable;
        }

        public TimeSpan? Int32_ms(TimeSpan? variable, TimeSpan defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt32_ms();
            if (Writer is not null) Writer.WriteInt32_ms(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int32_ms(ref TimeSpan variable)
        {
            variable = Int32_ms(variable);
        }

        public void Int32_ms(ref TimeSpan? variable, TimeSpan defaultValue = default)
        {
            variable = Int32_ms(variable, defaultValue);
        }

        public TimeSpan? Int32_sn(TimeSpan? variable = default)
        {
            if (Reader is not null) return Reader.ReadInt32_sn();
            if (Writer is not null) Writer.WriteInt32_sn(variable);
            return variable;
        }

        public void Int32_sn(ref TimeSpan? variable)
        {
            variable = Int32_sn(variable);
        }

        public TimeSpan? Int32_msn(TimeSpan? variable = default)
        {
            if (Reader is not null) return Reader.ReadInt32_msn();
            if (Writer is not null) Writer.WriteInt32_msn(variable);
            return variable;
        }

        public void Int32_msn(ref TimeSpan? variable)
        {
            variable = Int32_msn(variable);
        }

        public long Int64(long variable = default)
        {
            if (Reader is not null) return Reader.ReadInt64();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public long? Int64(long? variable, long defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt64();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int64(ref long variable)
        {
            variable = Int64(variable);
        }

        public void Int64(ref long? variable, long defaultValue = default)
        {
            variable = Int64(variable, defaultValue);
        }

        public ushort UInt16(ushort variable = default)
        {
            if (Reader is not null) return Reader.ReadUInt16();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public ushort? UInt16(ushort? variable, ushort defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadUInt16();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void UInt16(ref ushort variable)
        {
            variable = UInt16(variable);
        }

        public void UInt16(ref ushort? variable, ushort defaultValue = default)
        {
            variable = UInt16(variable, defaultValue);
        }

        public uint UInt32(uint variable = default)
        {
            if (Reader is not null) return Reader.ReadUInt32();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public uint? UInt32(uint? variable, uint defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadUInt32();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void UInt32(ref uint variable)
        {
            variable = UInt32(variable);
        }

        public void UInt32(ref uint? variable, uint defaultValue = default)
        {
            variable = UInt32(variable, defaultValue);
        }

        public ulong UInt64(ulong variable = default)
        {
            if (Reader is not null) return Reader.ReadUInt64();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public ulong? UInt64(ulong? variable, ulong defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadUInt64();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void UInt64(ref ulong variable)
        {
            variable = UInt64(variable);
        }

        public void UInt64(ref ulong? variable, ulong defaultValue = default)
        {
            variable = UInt64(variable, defaultValue);
        }

        public float Single(float variable = default)
        {
            if (Reader is not null) return Reader.ReadSingle();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public void Single(ref float variable)
        {
            variable = Single(variable);
        }

        public float? Single(float? variable, float defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadSingle();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Single(ref float? variable, float defaultValue = default)
        {
            variable = Single(variable, defaultValue);
        }

        public TimeSpan Single_s(TimeSpan variable = default)
        {
            if (Reader is not null) return Reader.ReadSingle_s();
            if (Writer is not null) Writer.WriteSingle_s(variable);
            return variable;
        }

        public TimeSpan? Single_s(TimeSpan? variable, TimeSpan defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadSingle_s();
            if (Writer is not null) Writer.WriteSingle_s(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Single_s(ref TimeSpan variable)
        {
            variable = Single_s(variable);
        }

        public void Single_s(ref TimeSpan? variable, TimeSpan defaultValue = default)
        {
            variable = Single_s(variable, defaultValue);
        }

        public TimeSpan Single_ms(TimeSpan variable = default)
        {
            if (Reader is not null) return Reader.ReadSingle_ms();
            if (Writer is not null) Writer.WriteSingle_ms(variable);
            return variable;
        }

        public TimeSpan? Single_ms(TimeSpan? variable, TimeSpan defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadSingle_ms();
            if (Writer is not null) Writer.WriteSingle_ms(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Single_ms(ref TimeSpan variable)
        {
            variable = Single_ms(variable);
        }

        public void Single_ms(ref TimeSpan? variable, TimeSpan defaultValue = default)
        {
            variable = Single_ms(variable, defaultValue);
        }

        public TimeSpan? Single_sn(TimeSpan? variable = default)
        {
            if (Reader is not null) return Reader.ReadSingle_sn();
            if (Writer is not null) Writer.WriteSingle_sn(variable);
            return variable;
        }

        public void Single_sn(ref TimeSpan? variable)
        {
            variable = Single_sn(variable);
        }

        public TimeSpan? Single_msn(TimeSpan? variable = default)
        {
            if (Reader is not null) return Reader.ReadSingle_msn();
            if (Writer is not null) Writer.WriteSingle_msn(variable);
            return variable;
        }

        public void Single_msn(ref TimeSpan? variable)
        {
            variable = Single_msn(variable);
        }

        public BigInteger BigInt(BigInteger variable, int byteLength)
        {
            if (Reader is not null) return Reader.ReadBigInt(byteLength);
            if (Writer is not null) Writer.WriteBigInt(variable);
            return variable;
        }

        public BigInteger? BigInt(BigInteger? variable, int byteLength, BigInteger defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadBigInt(byteLength);
            if (Writer is not null) Writer.WriteBigInt(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void BigInt(ref BigInteger variable, int byteLength)
        {
            variable = BigInt(variable, byteLength);
        }

        public void BigInt(ref BigInteger? variable, int byteLength, BigInteger defaultValue = default)
        {
            variable = BigInt(variable, byteLength, defaultValue);
        }

        public BigInteger BigInt(int byteLength)
        {
            return BigInt(default, byteLength);
        }

        public BigInteger Int128(BigInteger variable = default)
        {
            return BigInt(variable, 16);
        }

        public BigInteger? Int128(BigInteger? variable, BigInteger defaultValue = default)
        {
            return BigInt(variable, 16, defaultValue);
        }

        public void Int128(ref BigInteger variable)
        {
            BigInt(ref variable, 16);
        }

        public void Int128(ref BigInteger? variable, BigInteger defaultValue = default)
        {
            BigInt(ref variable, 16, defaultValue);
        }

        public Int2 Int2(Int2 variable = default)
        {
            if (Reader is not null) return Reader.ReadInt2();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public Int2? Int2(Int2? variable = default, Int2 defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt2();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int2(ref Int2 variable)
        {
            variable = Int2(variable);
        }

        public void Int2(ref Int2? variable, Int2 defaultValue = default)
        {
            variable = Int2(variable, defaultValue);
        }

        public Int3 Int3(Int3 variable = default)
        {
            if (Reader is not null) return Reader.ReadInt3();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public Int3? Int3(Int3? variable, Int3 defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadInt3();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Int3(ref Int3 variable)
        {
            variable = Int3(variable);
        }

        public void Int3(ref Int3? variable, Int3 defaultValue = default)
        {
            variable = Int3(variable, defaultValue);
        }

        public Byte3 Byte3(Byte3 variable = default)
        {
            if (Reader is not null) return Reader.ReadByte3();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public Byte3? Byte3(Byte3? variable, Byte3 defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadByte3();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Byte3(ref Byte3 variable)
        {
            variable = Byte3(variable);
        }

        public void Byte3(ref Byte3? variable, Byte3 defaultValue = default)
        {
            variable = Byte3(variable, defaultValue);
        }

        public Vec2 Vec2(Vec2 variable = default)
        {
            if (Reader is not null) return Reader.ReadVec2();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public Vec2? Vec2(Vec2? variable, Vec2 defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadVec2();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Vec2(ref Vec2 variable)
        {
            variable = Vec2(variable);
        }

        public void Vec2(ref Vec2? variable, Vec2 defaultValue = default)
        {
            variable = Vec2(variable, defaultValue);
        }

        public Vec3 Vec3(Vec3 variable = default)
        {
            if (Reader is not null) return Reader.ReadVec3();
            if (Writer is not null) Writer.Write(variable);
            return variable;
        }

        public Vec3? Vec3(Vec3? variable, Vec3 defaultValue = default)
        {
            if (Reader is not null) return Reader.ReadVec3();
            if (Writer is not null) Writer.Write(variable.GetValueOrDefault(defaultValue));
            return variable;
        }

        public void Vec3(ref Vec3 variable)
        {
            variable = Vec3(variable);
        }

        public void Vec3(ref Vec3? variable, Vec3 defaultValue = default)
        {
            variable = Vec3(variable, defaultValue);
        }

        public FileRef? FileRef(FileRef? variable = default)
        {
            if (Reader is not null) return Reader.ReadFileRef();
            if (Writer is not null) Writer.Write(variable ?? new FileRef());
            return variable;
        }

        public void FileRef(ref FileRef? variable)
        {
            variable = FileRef(variable);
        }

        public string? Id(string? variable, ILookbackable lookbackable)
        {
            if (Reader is not null) return Reader.ReadId(lookbackable);
            if (Writer is not null) Writer.Write(new Id(variable, lookbackable));
            return variable;
        }

        public void Id(ref string? variable, ILookbackable lookbackable)
        {
            variable = Id(variable, lookbackable);
        }

        public string? Id(string? variable = default)
        {
            if (Reader is not null)
            {
                if (Reader.Lookbackable is null)
                    throw new PropertyNullException(nameof(Reader.Lookbackable));

                return Id(variable, Reader.Lookbackable);
            }

            if (Writer is not null)
            {
                if (Writer.Lookbackable is null)
                    throw new PropertyNullException(nameof(Writer.Lookbackable));

                return Id(variable, Writer.Lookbackable);
            }

            throw new ThisShouldNotHappenException();
        }

        public void Id(ref string? variable)
        {
            variable = Id(variable);
        }

        public Ident? Ident(Ident? variable, ILookbackable lookbackable)
        {
            if (Reader is not null) return Reader.ReadIdent(lookbackable);
            if (Writer is not null) Writer.Write(variable, lookbackable);
            return variable;
        }

        public void Ident(ref Ident? variable, ILookbackable lookbackable)
        {
            variable = Ident(variable, lookbackable);
        }

        public Ident? Ident(Ident? variable = default)
        {
            if (Reader is not null)
            {
                if (Reader.Lookbackable is null)
                    throw new PropertyNullException(nameof(Reader.Lookbackable));

                return Ident(variable, Reader.Lookbackable);
            }

            if (Writer is not null)
            {
                if (Writer.Lookbackable is null)
                    throw new PropertyNullException(nameof(Writer.Lookbackable));

                return Ident(variable, Writer.Lookbackable);
            }

            throw new ThisShouldNotHappenException();
        }

        public void Ident(ref Ident? variable)
        {
            variable = Ident(variable);
        }

        public CMwNod? NodeRef(CMwNod? variable, GameBoxBody body)
        {
            if (Reader is not null) return Reader.ReadNodeRef(body);
            if (Writer is not null) Writer.Write(variable, body);
            return variable;
        }

        public void NodeRef(ref CMwNod? variable, GameBoxBody body)
        {
            variable = NodeRef(variable, body);
        }

        public CMwNod? NodeRef(CMwNod? variable = default)
        {
            if (Reader is not null)
            {
                if (Reader.Body is null)
                    throw new PropertyNullException(nameof(Reader.Body));

                return NodeRef(variable, Reader.Body);
            }

            if (Writer is not null)
            {
                if (Writer.Body is null)
                    throw new PropertyNullException(nameof(Writer.Body));

                return NodeRef(variable, Writer.Body);
            }

            throw new ThisShouldNotHappenException();
        }

        public void NodeRef(ref CMwNod? variable)
        {
            variable = NodeRef(variable);
        }

        public T? NodeRef<T>(T? variable, GameBoxBody body) where T : CMwNod
        {
            if (Reader is not null) return Reader.ReadNodeRef<T>(body);
            if (Writer is not null) Writer.Write(variable, body);
            return variable;
        }

        public void NodeRef<T>(ref T? variable, GameBoxBody body) where T : CMwNod
        {
            variable = NodeRef(variable, body);
        }

        public T? NodeRef<T>(T? variable = default) where T : CMwNod
        {
            if (Reader is not null)
            {
                if (Reader.Body is null)
                    throw new PropertyNullException(nameof(Reader.Body));

                return NodeRef(variable, Reader.Body);
            }

            if (Writer is not null)
            {
                if (Writer.Body is null)
                    throw new PropertyNullException(nameof(Writer.Body));

                return NodeRef(variable, Writer.Body);
            }

            throw new ThisShouldNotHappenException();
        }

        public void NodeRef<T>(ref T? variable) where T : CMwNod
        {
            variable = NodeRef(variable);
        }

        public void EnumByte<T>(ref T variable) where T : struct, Enum
        {
            var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<byte>.From(variable) : default;

            Byte(ref v);

            if (Mode == GameBoxReaderWriterMode.Read)
                variable = CastTo<T>.From(v);
        }

        public void EnumInt32<T>(ref T variable) where T : struct, Enum
        {
            var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<int>.From(variable) : default;

            Int32(ref v);

            if (Mode == GameBoxReaderWriterMode.Read)
                variable = CastTo<T>.From(v);
        }

        public void EnumInt32<T>(ref T? variable, T defaultValue = default) where T : struct, Enum
        {
            var v = Mode == GameBoxReaderWriterMode.Write ? CastTo<int?>.From(variable) : default;

            if (Mode == GameBoxReaderWriterMode.Write)
            {
                if (defaultValue.Equals(default(T)))
                {
                    Int32(ref v);
                    return;
                }

                Int32(ref v, CastTo<int>.From(defaultValue));
            }

            if (Mode == GameBoxReaderWriterMode.Read)
            {
                Int32(ref v);
                variable = CastTo<T>.From(v);
            }
        }

        public string? String(string? variable = default, StringLengthPrefix readPrefix = default)
        {
            if (Reader is not null) return Reader.ReadString(readPrefix);
            if (Writer is not null) Writer.Write(variable, readPrefix);
            return variable;
        }

        public void String(ref string? variable, StringLengthPrefix readPrefix = default)
        {
            variable = String(variable, readPrefix);
        }

        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
        public byte[]? Bytes(byte[]? variable, int count)
        {
            if (Reader is not null) return Reader.ReadBytes(count);
            if (Writer is not null)
            {
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count), "Count is negative");

                Writer.Write(variable ?? new byte[count], 0, count);

                return variable;
            }

            throw new ThisShouldNotHappenException();
        }

        public void Bytes(ref byte[]? variable, int count)
        {
            if (Reader is not null) variable = Reader.ReadBytes(count);
            if (Writer is not null) Writer.Write(variable, 0, count);
        }

        public byte[]? Bytes(byte[]? variable = default)
        {
            if (Reader is not null) return Reader.ReadBytes();
            if (Writer is not null)
            {
                if (variable is null)
                {
                    Writer.Write(0);
                    return variable;
                }    

                Writer.Write(variable.Length);
                Writer.Write(variable);
            }

            return variable;
        }

        public void Bytes(ref byte[]? variable)
        {
            variable = Bytes(variable);
        }

        public T[]? Array<T>(T[]? array, int count) where T : struct
        {
            if (Reader is not null) return Reader.ReadArray<T>(count);
            if (Writer is not null) Writer.WriteArray_NoPrefix(CreateArrayIfNull(array));
            return array;
        }

        public void Array<T>(ref T[]? array, int count) where T : struct
        {
            array = Array(array, count);
        }

        public T[]? Array<T>(T[]? array = default) where T : struct
        {
            if (Reader is not null) return Reader.ReadArray<T>();
            if (Writer is not null) Writer.WriteArray(array);
            return array;
        }

        public void Array<T>(ref T[]? array) where T : struct
        {
            array = Array(array);
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Array length is lower than 0.</exception>
        public T[]? Array<T>(T[]? array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadArray(forLoopRead);
            if (Writer is not null) Writer.Write(array, forLoopWrite);
            return array;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="forLoopRead"/> or <paramref name="forLoopWrite"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Array length is lower than 0.</exception>
        public void Array<T>(ref T[]? array, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[]? Array<T>(T[]? array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadArray(forLoopRead);
            if (Writer is not null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[]? array, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[]? Array<T>(T[]? array, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadArray(forLoopRead);
            if (Writer is not null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[]? array, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T[]? Array<T>(T[]? array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadArray(forLoopRead);
            if (Writer is not null) Writer.Write(array, forLoopWrite);
            return array;
        }

        public void Array<T>(ref T[]? array, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            array = Array(array, forLoopRead, forLoopWrite);
        }

        public T?[]? ArrayNode<T>(T?[]? array = default) where T : CMwNod
        {
            return Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public void ArrayNode<T>(ref T?[]? array) where T : CMwNod
        {
            array = Array(array, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public IEnumerable<T>? Enumerable<T>(IEnumerable<T>? enumerable = default) where T : struct
        {
            return Array(enumerable?.ToArray());
        }

        public void Enumerable<T>(ref IEnumerable<T>? enumerable) where T : struct
        {
            enumerable = Enumerable(enumerable);
        }

        public IEnumerable<T>? Enumerable<T>(IEnumerable<T>? enumerable, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T>? enumerable, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T>? Enumerable<T>(IEnumerable<T>? enumerable, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T>? enumerable, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T>? Enumerable<T>(IEnumerable<T>? enumerable, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T>? enumerable, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T>? Enumerable<T>(IEnumerable<T>? enumerable, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            return Array(enumerable?.ToArray(), forLoopRead, forLoopWrite);
        }

        public void Enumerable<T>(ref IEnumerable<T>? enumerable, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            enumerable = Enumerable(enumerable, forLoopRead, forLoopWrite);
        }

        public IEnumerable<T?>? EnumerableNode<T>(IEnumerable<T?>? enumerable = default) where T : CMwNod
        {
            return Enumerable(enumerable, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public void EnumerableNode<T>(ref IEnumerable<T?>? enumerable) where T : CMwNod
        {
            enumerable = Enumerable(enumerable, r => r.ReadNodeRef<T>(), (x, w) => w.Write(x));
        }

        public IList<T>? List<T>(IList<T>? list = default) where T : struct
        {
            return Array(list?.ToArray());
        }

        public void List<T>(ref IList<T>? list) where T : struct
        {
            list = List(list);
        }

        public IList<T>? List<T>(IList<T>? list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadList(forLoopRead);
            if (Writer is not null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T>? list, Func<int, T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T>? List<T>(IList<T>? list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadList(forLoopRead);
            if (Writer is not null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T>? list, Func<int, GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T>? List<T>(IList<T>? list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadList(forLoopRead);
            if (Writer is not null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T>? list, Func<T> forLoopRead, Action<T> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T>? List<T>(IList<T>? list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            if (Reader is not null) return Reader.ReadList(forLoopRead);
            if (Writer is not null) Writer.Write(list, forLoopWrite);
            return list;
        }

        public void List<T>(ref IList<T>? list, Func<GameBoxReader, T> forLoopRead, Action<T, GameBoxWriter> forLoopWrite)
        {
            list = List(list, forLoopRead, forLoopWrite);
        }

        public IList<T?>? ListNode<T>(IList<T?>? list = default) where T : CMwNod
        {
            return List(list,
                r => r.ReadNodeRef<T>(),
                (x, w) => w.Write(x));
        }

        public void ListNode<T>(ref IList<T?>? list) where T : CMwNod
        {
            list = List(list,
                r => r.ReadNodeRef<T>(),
                (x, w) => w.Write(x));
        }

        public IDictionary<TKey, TValue>? Dictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary = default)
        {
            if (Reader is not null) return Reader.ReadDictionary<TKey, TValue>();
            if (Writer is not null) Writer.Write(dictionary);
            return dictionary;
        }

        public void Dictionary<TKey, TValue>(ref IDictionary<TKey, TValue>? dictionary)
        {
            dictionary = Dictionary(dictionary);
        }

        public IDictionary<TKey, TValue?>? DictionaryNode<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary = default) where TValue : CMwNod
        {
            if (Reader is not null) return Reader.ReadDictionaryNode<TKey, TValue>();
            if (Writer is not null) Writer.WriteDictionaryNode(dictionary);
            return dictionary;
        }

        public void DictionaryNode<TKey, TValue>(ref IDictionary<TKey, TValue?>? dictionary) where TValue : CMwNod
        {
            dictionary = DictionaryNode(dictionary);
        }

        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="PropertyNullException"><see cref="GameBoxReader.Lookbackable"/> of <see cref="Reader"/> is null.</exception>
        /// <exception cref="EndOfStreamException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public void UntilFacade(MemoryStream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (Reader is not null)
            {
                if (Reader.Lookbackable is null)
                    throw new PropertyNullException(nameof(Reader.Lookbackable));

                using var w = new GameBoxWriter(stream, Reader.Lookbackable);
                w.Write(Reader.ReadUntilFacade().ToArray());

                return;
            }
            
            if (Writer is not null)
            {
                var buffer = new byte[stream.Length - stream.Position];
                stream.Read(buffer, 0, buffer.Length);
                Writer.WriteBytes(buffer);

                return;
            }

            throw new ThisShouldNotHappenException();
        }

        private T[] CreateArrayIfNull<T>(T[]? array)
        {
            if (array is not null)
                return array;

#if NETSTANDARD2_0_OR_GREATER
            array = System.Array.Empty<T>();
#else
            array = new T[0];
#endif

            return array;
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
