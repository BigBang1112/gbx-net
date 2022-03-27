using GBX.NET.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
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
}
