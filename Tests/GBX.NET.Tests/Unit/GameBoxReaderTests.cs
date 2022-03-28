using GBX.NET.Exceptions;
using GBX.NET.Managers;
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

    // test constructor
    [Fact]
    public void Constructor_InputSettingsLogger()
    {
        // arrange
        var ms = new MemoryStream();
        var settings = new GameBoxReaderSettings(Guid.NewGuid(), new GameBoxAsyncReadAction());
        var logger = CreateLogger();
        var reader = new GameBoxReader(ms, settings, logger);

        // assert
        Assert.Equal(ms, reader.BaseStream);
        Assert.Equal(settings, reader.Settings);
        Assert.Equal(logger, reader.Logger);
    }

    // test constructor with null stream
    [Fact]
    public void Constructor_InputSettingsLogger_NullStream()
    {
        // arrange
        var settings = new GameBoxReaderSettings(Guid.NewGuid(), new GameBoxAsyncReadAction());
        var logger = CreateLogger();

        // act
        var ex = Assert.Throws<ArgumentNullException>(() => new GameBoxReader(null!, settings));

        // assert
        Assert.Equal("input", ex.ParamName);
    }

    // test constructor input, stateGuid, asyncReadAction and logger
    [Fact]
    public void Constructor_InputSettingsLoggerAsyncReadAction()
    {
        // arrange
        var ms = new MemoryStream();
        var logger = CreateLogger();
        var stateGuid = Guid.NewGuid();
        var asyncReadAction = new GameBoxAsyncReadAction();
        var reader = new GameBoxReader(ms, stateGuid, asyncReadAction, logger);

        // assert
        Assert.Equal(ms, reader.BaseStream);
        Assert.Equal(stateGuid, reader.Settings.StateGuid);
        Assert.Equal(asyncReadAction, reader.Settings.AsyncAction);
        Assert.Equal(logger, reader.Logger);
    }

    // test constructor input, stateGuid, asyncReadAction and logger
    [Fact]
    public void Constructor_InputSettingsLoggerAsyncReadAction_NullStream()
    {
        // arrange
        var logger = CreateLogger();
        var stateGuid = Guid.NewGuid();
        var asyncReadAction = new GameBoxAsyncReadAction();

        // act
        var ex = Assert.Throws<ArgumentNullException>(() => new GameBoxReader(null!, stateGuid, asyncReadAction, logger));

        // assert
        Assert.Equal("input", ex.ParamName);
    }

    // test ReadBoolean
    [Fact]
    public void ReadBoolean()
    {
        // arrange
        var ms = new MemoryStream(BitConverter.GetBytes(1));
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        var reader = new GameBoxReader(ms);

        Assert.Throws<EndOfStreamException>(() => reader.ReadBoolean());
    }

    // test ReadBoolean asByte
    [Fact]
    public void ReadBoolean_AsByte()
    {
        // arrange
        var ms = new MemoryStream(new byte[] { 1 });
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        var reader = new GameBoxReader(ms);

        Assert.Throws<EndOfStreamException>(() => reader.ReadBoolean(asByte: true));
    }

    // test ReadString readPrefix Int32
    [Fact]
    public void ReadString_ReadPrefixInt32()
    {
        // arrange
        var text = "Hello World";
        var ms = new MemoryStream(BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream(new byte[] { (byte)text.Length }.Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
        var reader = new GameBoxReader(ms);

        Assert.Throws<ArgumentException>(() => reader.ReadString(StringLengthPrefix.None));
    }

    // test ReadString readPrefix Int32 negative length
    [Fact]
    public void ReadString_ReadPrefixInt32_NegativeLength()
    {
        // arrange
        var text = "Hello World";
        var ms = new MemoryStream(BitConverter.GetBytes(-text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        var reader = new GameBoxReader(ms);

        Assert.Throws<StringLengthOutOfRangeException>(() => reader.ReadString(StringLengthPrefix.Int32));
    }

    // test ReadString no params
    [Fact]
    public void ReadString_NoParams()
    {
        // arrange
        var text = "Hello World";
        var ms = new MemoryStream(BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray());
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream(totalBytes);
        var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadBytes();

        // assert
        Assert.Equal(bytes, result);
    }

    // test ReadId null StateGuid
    [Fact]
    public void ReadId_NullStateGuid()
    {
        // arrange
        var ms = new MemoryStream();
        var reader = new GameBoxReader(ms);

        Assert.Throws<PropertyNullException>(() => reader.ReadId());
    }
    
    // test ReadId with version index -1
    [Fact]
    public void ReadId_WithVersion_IndexMinus1()
    {
        // arrange
        var state = StateManager.Shared.CreateState(null);
        var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 255, 255, 255, 255 });
        var reader = new GameBoxReader(ms, state);

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
        var state = StateManager.Shared.CreateState(null);
        var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 0, 0, 0, 64, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f });
        var reader = new GameBoxReader(ms, state);

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
        var state = StateManager.Shared.CreateState(null);
        var ms = new MemoryStream(new byte[] { 3, 0, 0, 0, 0, 0, 0, 128, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f });
        var reader = new GameBoxReader(ms, state);

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
        var state = StateManager.Shared.CreateState(null);
        var ms = new MemoryStream(new byte[] { 3, 0, 0, 0,
            0, 0, 0, 128, 5, 0, 0, 0, 0x48, 0x65, 0x6c, 0x6c, 0x6f,
            0, 0, 0, 128, 2, 0, 0, 0, 0x48, 0x69,
            0, 0, 0, 128, 9, 0, 0, 0, 0x47, 0x6f, 0x6f, 0x64, 0x20, 0x67, 0x6f, 0x6f, 0x64
        });
        var reader = new GameBoxReader(ms, state);

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
        var ms = new MemoryStream();
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

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var filePath = "Path/To/File.dds";
        var locatorUrl = "https://locatorurl.cz/File.dds";

        w.Write((byte)2);
        w.Write(filePath.Length);
        w.Write(Encoding.UTF8.GetBytes(filePath));
        w.Write(locatorUrl.Length);
        w.Write(Encoding.UTF8.GetBytes(locatorUrl));

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;

        w.Write(x);
        w.Write(y);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1.0f;
        var y = 2.0f;
        var z = 3.0f;

        w.Write(x);
        w.Write(y);
        w.Write(z);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
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

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
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

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;
        var z = 3;

        w.Write(x);
        w.Write(y);
        w.Write(z);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;

        w.Write(x);
        w.Write(y);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = 1;
        var y = 2;
        var z = 3;

        w.Write((byte)x);
        w.Write((byte)y);
        w.Write((byte)z);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = new byte[] { 128, 128, 128, 255 };
        var bigInt = new BigInteger(x);

        w.Write(x);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
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

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
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

        var reader = new GameBoxReader(ms);

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
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);

        var x = ushort.MaxValue / 2 + 1;
        
        w.Write(x);

        ms.Position = 0;

        var reader = new GameBoxReader(ms);

        // act
        var result = reader.ReadTimeOfDay();

        // assert
        Assert.NotNull(result);
        Assert.Equal(expected: (int)TimeSpan.FromHours(12).TotalSeconds, actual: (int)result!.Value.TotalSeconds);
    }
}
