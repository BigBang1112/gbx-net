using GBX.NET.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class GameBoxReaderTests
{
    private static ILogger<GameBoxReader> CreateLogger()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<ILoggerFactory>();

        return factory.CreateLogger<GameBoxReader>();
    }

    // test ReadBoolean
    [Fact]
    public void ReadBoolean()
    {
        // arrange
        using var ms = new MemoryStream(BitConverter.GetBytes(1));
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBoolean();

        // assert
        Assert.True(result);
    }

    // test ReadBoolean_EmptyStream
    [Fact]
    public void ReadBoolean_EmptyStream()
    {
        // arrange
        using var ms = new MemoryStream();
        using var reader = new GameBoxReader(ms);

        Assert.Throws<EndOfStreamException>(() => reader.ReadBoolean());
    }

    // test ReadBoolean asByte
    [Fact]
    public void ReadBoolean_AsByte()
    {
        // arrange
        using var ms = new MemoryStream(new byte[] { 1 });
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBoolean(asByte: true);

        // assert
        Assert.True(result);
    }

    // test ReadBoolean asByte_EmptyStream
    [Fact]
    public void ReadBoolean_AsByte_EmptyStream()
    {
        // arrange
        using var ms = new MemoryStream();
        using var reader = new GameBoxReader(ms);

        Assert.Throws<EndOfStreamException>(() => reader.ReadBoolean(asByte: true));
    }

    // test ReadString readPrefix Int32
    [Fact]
    public void ReadString_ReadPrefixInt32()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadString(StringLengthPrefix.Int32);

        // assert
        Assert.Equal(text, result);
    }

    // test ReadString readPrefix Byte
    [Fact]
    public void ReadString_ReadPrefixByte()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(new byte[] { (byte)text.Length }.Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadString(StringLengthPrefix.Byte);

        // assert
        Assert.Equal(text, result);
    }

    // test ReadString readPrefix None
    [Fact]
    public void ReadString_ReadPrefixNone()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
        using var reader = new GameBoxReader(ms);

        Assert.Throws<ArgumentException>(() => reader.ReadString(StringLengthPrefix.None));
    }

    // test ReadString readPrefix Int32 negative length
    [Fact]
    public void ReadString_ReadPrefixInt32_NegativeLength()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(BitConverter.GetBytes(-text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        using var reader = new GameBoxReader(ms);

        Assert.Throws<ArgumentOutOfRangeException>(() => reader.ReadString(StringLengthPrefix.Int32));
    }

    // test ReadString no params
    [Fact]
    public void ReadString_NoParams()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadString();

        // assert
        Assert.Equal(text, result);
    }

    // test ReadString length
    [Fact]
    public void ReadString_Length()
    {
        // arrange
        var text = "Hello World";
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadString(text.Length);

        // assert
        Assert.Equal(text, result);
    }

    // test ReadBytes
    [Fact]
    public void ReadBytes()
    {
        // arrange
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        var totalBytes = BitConverter.GetBytes(bytes.Length).Concat(new byte[] { 1, 2, 3, 4, 5 }).ToArray();
        using var ms = new MemoryStream(totalBytes);
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBytes();

        // assert
        Assert.Equal(bytes, result);
    }
    
    // test ReadId with version index -1
    [Fact]
    public void ReadId_WithVersion_IndexMinus1()
    {
        // arrange
        using var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 255, 255, 255, 255 });
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadId();

        // assert
        Assert.Equal(expected: "", actual: result);
    }

    // test ReadId with version index 0 and bit 30 is 1
    [Fact]
    public void ReadId_WithVersion_Index0_Bit30Is1()
    {
        // arrange
        using var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 0, 0, 0, 64, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f });
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadId();
        
        // assert
        Assert.Equal(expected: "Hello", actual: result.String);
        Assert.Null(result.Index);
    }

    // test ReadId with version index 0 and bit 31 is 1
    [Fact]
    public void ReadId_WithVersion_Index0_Bit31Is1()
    {
        // arrange
        using var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 0, 0, 0, 128, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f });
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadId();

        // assert
        Assert.Equal(expected: "Hello", actual: result.String);
        Assert.Null(result.Index);
    }

    // test ReadIdent
    [Fact]
    public void ReadIdent()
    {
        // arrange
        using var ms = new MemoryStream(new byte[] { 3, 0, 0, 0,
            0, 0, 0, 128, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f,
            0, 0, 0, 128, 2, 0, 0, 0, 0x48, 0x69,
            0, 0, 0, 128, 9, 0, 0, 0, 0x47, 0x6f, 0x6f, 0x64, 0x20, 0x67, 0x6f, 0x6f, 0x64
        });
        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadIdent();

        // assert
        Assert.Equal(expected: "Hello", actual: result.Id);
        Assert.Equal(expected: "Hi", actual: result.Collection.String);
        Assert.Equal(expected: "Good good", actual: result.Author);
    }

    // test ReadFileRef version 3
    [Fact]
    public void ReadFileRef_Version3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var checksum = new byte[32];
        new Random().NextBytes(checksum);

        var filePath = "Path/To/File.dds";
        var locatorUrl = "https://locatorurl.cz/File.dds";

        w.Write((byte)3);
        w.Write(checksum);
        w.Write(filePath.Length);
        w.Write(Encoding.UTF8.GetBytes(filePath));
        w.Write(locatorUrl.Length);
        w.Write(Encoding.UTF8.GetBytes(locatorUrl));

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadFileRef();

        // assert
        Assert.Equal(expected: 3, actual: result.Version);
        Assert.Equal(expected: checksum, actual: result.Checksum);
        Assert.Equal(expected: filePath, actual: result.FilePath);
        Assert.Equal(expected: locatorUrl, actual: result.LocatorUrl);
    }

    // test ReadFileRef version 2
    [Fact]
    public void ReadFileRef_Version2()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var filePath = "Path/To/File.dds";
        var locatorUrl = "https://locatorurl.cz/File.dds";

        w.Write((byte)2);
        w.Write(filePath.Length);
        w.Write(Encoding.UTF8.GetBytes(filePath));
        w.Write(locatorUrl.Length);
        w.Write(Encoding.UTF8.GetBytes(locatorUrl));

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadFileRef();

        // assert
        Assert.Equal(expected: 2, actual: result.Version);
        Assert.Equal(expected: filePath, actual: result.FilePath);
        Assert.Equal(expected: locatorUrl, actual: result.LocatorUrl);
    }

    // test ReadVec2
    [Fact]
    public void ReadVec2()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;

        w.Write(x);
        w.Write(y);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadVec2();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
    }

    // test ReadVec3
    [Fact]
    public void ReadVec3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var z = 3.0f;

        w.Write(x);
        w.Write(y);
        w.Write(z);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadVec3();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
    }

    // test ReadVec4
    [Fact]
    public void ReadVec4()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var z = 3.0f;
        var ww = 4.0f;

        w.Write(x);
        w.Write(y);
        w.Write(z);
        w.Write(ww);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadVec4();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
        Assert.Equal(expected: ww, actual: result.W);
    }

    // test ReadQuat
    [Fact]
    public void ReadQuat()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var z = 3.0f;
        var ww = 4.0f;

        w.Write(x);
        w.Write(y);
        w.Write(z);
        w.Write(ww);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadQuat();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
        Assert.Equal(expected: ww, actual: result.W);
    }

    // test ReadInt3
    [Fact]
    public void ReadInt3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;
        var z = 3;

        w.Write(x);
        w.Write(y);
        w.Write(z);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadInt3();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
    }

    // test ReadInt2
    [Fact]
    public void ReadInt2()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;

        w.Write(x);
        w.Write(y);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadInt2();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
    }

    // test ReadByte3
    [Fact]
    public void ReadByte3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;
        var z = 3;

        w.Write((byte)x);
        w.Write((byte)y);
        w.Write((byte)z);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadByte3();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
    }

    // test ReadBigInt
    [Fact]
    public void ReadBigInt()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = new byte[] { 128, 128, 128, 255 };
        var bigInt = new BigInteger(x);

        w.Write(x);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBigInt(x.Length);

        // assert
        Assert.Equal(expected: bigInt, actual: result);
    }

    // test ReadRect
    [Fact]
    public void ReadRect()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var ww = 3.0f;
        var hh = 4.0f;

        w.Write(x);
        w.Write(y);
        w.Write(ww);
        w.Write(hh);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadRect();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: ww, actual: result.X2);
        Assert.Equal(expected: hh, actual: result.Y2);
    }

    // test ReadBox
    [Fact]
    public void ReadBox()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var z = 3.0f;
        var ww = 4.0f;
        var hh = 5.0f;
        var dd = 6.0f;

        w.Write(x);
        w.Write(y);
        w.Write(z);
        w.Write(ww);
        w.Write(hh);
        w.Write(dd);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBox();

        // assert
        Assert.Equal(expected: x, actual: result.X);
        Assert.Equal(expected: y, actual: result.Y);
        Assert.Equal(expected: z, actual: result.Z);
        Assert.Equal(expected: ww, actual: result.X2);
        Assert.Equal(expected: hh, actual: result.Y2);
        Assert.Equal(expected: dd, actual: result.Z2);
    }

    // test ReadTimeOfDay
    [Fact]
    public void ReadTimeOfDay()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = ushort.MaxValue / 2 + 1;
        
        w.Write(x);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadTimeOfDay();

        // assert
        Assert.NotNull(result);
        Assert.Equal(expected: (int)TimeSpan.FromHours(12).TotalSeconds, actual: (int)result!.Value.TotalSeconds);
    }

    // test ReadToEnd
    [Fact]
    public void ReadToEnd()
    {
        // arrange
        var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        using var ms = new MemoryStream(bytes);

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadToEnd();

        // assert
        Assert.Equal(expected: bytes, actual: result);
    }

    // test ReadToEnd seek true
    [Fact]
    public void ReadToEnd_Seek()
    {
        // arrange
        var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        using var ms = new MemoryStream(bytes);

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadToEnd(seek: true);

        // assert
        Assert.Equal(expected: bytes, actual: result);
    }

    // test Read<byte>
    [Fact]
    public void Read_Byte()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((byte)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<byte>();

        // assert
        Assert.Equal(expected: 5, actual: result);
    }

    // test Read<short>
    [Fact]
    public void Read_Short()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((short)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<short>();

        // assert
        Assert.Equal(expected: 5, actual: result);
    }

    // test Read<int>
    [Fact]
    public void Read_Int()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<int>();

        // assert
        Assert.Equal(expected: 5, actual: result);
    }

    // test Read<long>
    [Fact]
    public void Read_Long()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((long)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<long>();

        // assert
        Assert.Equal(expected: 5, actual: result);
    }

    // test Read<float>
    [Fact]
    public void Read_Float()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(5f);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<float>();

        // assert
        Assert.Equal(expected: 5.0, actual: result);
    }

    // test Read<bool>
    [Fact]
    public void Read_Bool()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(new byte[] { 1, 0, 0, 0 });

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<bool>();

        // assert
        Assert.True(result);
    }

    // test Read<string>
    [Fact]
    public void Read_String()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(5);
        w.Write(Encoding.UTF8.GetBytes("Hello"));

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<string>();

        // assert
        Assert.Equal(expected: "Hello", actual: result);
    }

    // test Read<sbyte>
    [Fact]
    public void Read_SByte()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((sbyte)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<sbyte>();

        // assert
        Assert.Equal(expected: 5, actual: result);
    }

    // test Read<uint>
    [Fact]
    public void Read_UInt()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((uint)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<uint>();

        // assert
        Assert.Equal<uint>(expected: 5, actual: result);
    }

    // test Read<ulong>
    [Fact]
    public void Read_ULong()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write((ulong)5);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<ulong>();

        // assert
        Assert.Equal<ulong>(expected: 5, actual: result);
    }

    // test Read<Byte3>
    [Fact]
    public void Read_Byte3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(new byte[] { 1, 2, 3 });

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Byte3>();

        // assert
        Assert.Equal(expected: new Byte3(1, 2, 3), actual: result);
    }

    // test Read<Vec2>
    [Fact]
    public void Read_Vec2()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1f);
        w.Write(2f);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Vec2>();

        // assert
        Assert.Equal(expected: new Vec2(1, 2), actual: result);
    }

    // test Read<Vec3>
    [Fact]
    public void Read_Vec3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1f);
        w.Write(2f);
        w.Write(3f);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Vec3>();

        // assert
        Assert.Equal(expected: new Vec3(1, 2, 3), actual: result);
    }

    // test Read<Vec4>
    [Fact]
    public void Read_Vec4()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1f);
        w.Write(2f);
        w.Write(3f);
        w.Write(4f);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Vec4>();

        // assert
        Assert.Equal(expected: new Vec4(1, 2, 3, 4), actual: result);
    }

    // test Read<Int2>
    [Fact]
    public void Read_Int2()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1);
        w.Write(2);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Int2>();

        // assert
        Assert.Equal(expected: new Int2(1, 2), actual: result);
    }

    // test Read<Int3>
    [Fact]
    public void Read_Int3()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1);
        w.Write(2);
        w.Write(3);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // act
        var result = reader.Read<Int3>();

        // assert
        Assert.Equal(expected: new Int3(1, 2, 3), actual: result);
    }

    // test Read anything else
    [Fact]
    public void Read_AnythingElse()
    {
        // arrange
        using var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        w.Write(1);
        w.Write(2);
        w.Write(3);
        w.Write(4);

        ms.Position = 0;

        using var reader = new GameBoxReader(ms);

        // assert
        Assert.Throws<NotSupportedException>(() => /* act */ reader.Read<BigInteger>());
    }
}
