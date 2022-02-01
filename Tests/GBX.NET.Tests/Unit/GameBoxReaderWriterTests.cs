using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class GameBoxReaderWriterTests
{
    public enum TestEnum
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five
    }

    private static GameBoxReaderWriter PrepareInReadMode(Action<GameBoxWriter> initialWrites)
    {
        var ms = new MemoryStream();

        using var w = new GameBoxWriter(ms);
        initialWrites.Invoke(w);
        ms.Seek(0, SeekOrigin.Begin);

        var r = new GameBoxReader(ms);
        return new GameBoxReaderWriter(r);
    }

    private static GameBoxReaderWriter PrepareInWriteMode()
    {
        var ms = new MemoryStream();
        var w = new GameBoxWriter(ms);
        return new GameBoxReaderWriter(w);
    }

    private static T GetValueBack<T>(GameBoxReaderWriter rw, Func<GameBoxReader, T> reader)
    {
        if (rw.Writer is null) throw new Exception();

        var stream = rw.Writer.BaseStream;
        stream.Seek(0, SeekOrigin.Begin);
        using var r = new GameBoxReader(rw.Writer.BaseStream);
        return reader.Invoke(r);
    }

    private static void GeneralTest_Read_Ref_ShouldEqualExampleValue<T>(Func<GameBoxReaderWriter, T?, T> action,
        Action<GameBoxWriter> intialWrite, T? exampleValue = default)
    {
        // Arrange
        var rw = PrepareInReadMode(intialWrite);
        var value = default(T);

        // Act
        value = action.Invoke(rw, value);

        // Assert
        Assert.Equal(exampleValue, value);
    }

    private static void GeneralTest_Read_Ref_NullValueShouldNotEqual<T>(Func<GameBoxReaderWriter, T?, T> action,
        Action<GameBoxWriter> intialWrite)
    {
        // Arrange
        var rw = PrepareInReadMode(intialWrite);
        var value = default(T);

        // Act
        value = action.Invoke(rw, value);

        // Assert
        Assert.NotNull(value);
    }

    private static void GeneralTest_Write_Ref_ShouldBeEqual<T>(Func<GameBoxReaderWriter, T?, T> action, T? value = default)
    {
        // Arrange
        var rw = PrepareInWriteMode();
        var expected = value;

        // Act
        value = action.Invoke(rw, value);

        // Assert
        Assert.Equal(expected, actual: value);
    }

    private static void GeneralTest_Write_Nullable_DefaultShouldBeWritten<T>(
        Action<GameBoxReaderWriter, T?, T> action, Func<GameBoxReader, T> valueBack, T defaultValue) where T : struct
    {
        // Arrange
        var rw = PrepareInWriteMode();
        var value = default(T?);

        // Act
        action.Invoke(rw, value, defaultValue);

        var actualValue = GetValueBack(rw, valueBack);

        // Assert
        Assert.Equal(expected: defaultValue, actual: actualValue);
    }

    #region Byte

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)69)]
    [InlineData((byte)255)]
    public void Byte_Read_Ref_ShouldEqualExampleValue(byte exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Byte(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)69)]
    [InlineData((byte)255)]
    public void Byte_Nullable_Read_Ref_ShouldEqualExampleValue(byte exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<byte?>((rw, value) =>
        {
            rw.Byte(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Byte_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<byte>((rw, value) =>
        {
            rw.Byte(ref value);
            return value;
        },
        w => w.Write(default(byte)));
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)69)]
    [InlineData((byte)255)]
    public void Byte_Write_Ref_ShouldBeEqual(byte value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Byte(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)69)]
    [InlineData((byte)255)]
    public void Byte_Nullable_Write_Ref_ShouldBeEqual(byte? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Byte(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData((byte)69)]
    [InlineData((byte)255)]
    public void Byte_Nullable_Write_DefaultShouldBeWritten(byte defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Byte(ref value, defaultValue),
            r => r.ReadByte(), defaultValue);
    }

    #endregion

    #region Boolean

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Boolean_Read_Ref_ShouldEqualExampleValue(bool exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Boolean(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Boolean_Nullable_Read_Ref_ShouldEqualExampleValue(bool exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<bool?>((rw, value) =>
        {
            rw.Boolean(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Boolean_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<bool>((rw, value) =>
        {
            rw.Boolean(ref value);
            return value;
        },
        w => w.Write(default(bool)));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Boolean_Write_Ref_ShouldBeEqual(bool value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Boolean(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public void Boolean_Nullable_Write_Ref_ShouldBeEqual(bool? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Boolean(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Boolean_Nullable_Write_DefaultShouldBeWritten(bool defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Boolean(ref value, defaultValue),
            r => r.ReadBoolean(), defaultValue);
    }

    #endregion

    #region Int16

    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-69)]
    [InlineData((short)6969)]
    public void Int16_Read_Ref_ShouldEqualExampleValue(short exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Int16(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-69)]
    [InlineData((short)6969)]
    public void Int16_Nullable_Read_Ref_ShouldEqualExampleValue(short exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<short?>((rw, value) =>
        {
            rw.Int16(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Int16_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<short>((rw, value) =>
        {
            rw.Int16(ref value);
            return value;
        },
        w => w.Write(default(short)));
    }

    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-69)]
    [InlineData((short)6969)]
    public void Int16_Write_Ref_ShouldBeEqual(short value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int16(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-69)]
    [InlineData((short)6969)]
    public void Int16_Nullable_Write_Ref_ShouldBeEqual(short? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int16(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-69)]
    [InlineData((short)6969)]
    public void Int16_Nullable_Write_DefaultShouldBeWritten(short defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Int16(ref value, defaultValue),
            r => r.ReadInt16(), defaultValue);
    }

    #endregion

    #region UInt16

    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)69)]
    [InlineData((ushort)42069)]
    public void UInt16_Read_Ref_ShouldEqualExampleValue(ushort exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.UInt16(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)69)]
    [InlineData((ushort)42069)]
    public void UInt16_Nullable_Read_Ref_ShouldEqualExampleValue(ushort exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<ushort?>((rw, value) =>
        {
            rw.UInt16(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void UInt16_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<ushort>((rw, value) =>
        {
            rw.UInt16(ref value);
            return value;
        },
        w => w.Write(default(ushort)));
    }

    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)69)]
    [InlineData((ushort)42069)]
    public void UInt16_Write_Ref_ShouldBeEqual(ushort value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt16(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)69)]
    [InlineData((ushort)42069)]
    public void UInt16_Nullable_Write_Ref_ShouldBeEqual(ushort? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt16(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)69)]
    [InlineData((ushort)42069)]
    public void UInt16_Nullable_Write_DefaultShouldBeWritten(ushort defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.UInt16(ref value, defaultValue),
            r => r.ReadUInt16(), defaultValue);
    }

    #endregion

    #region Int32

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-69)]
    [InlineData(420420)]
    public void Int32_Read_Ref_ShouldEqualExampleValue(int exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Int32(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-69)]
    [InlineData(420420)]
    public void Int32_Nullable_Read_Ref_ShouldEqualExampleValue(int exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<int?>((rw, value) =>
        {
            rw.Int32(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Int32_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<int>((rw, value) =>
        {
            rw.Int32(ref value);
            return value;
        },
        w => w.Write(default(int)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-69)]
    [InlineData(420420)]
    public void Int32_Write_Ref_ShouldBeEqual(int value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-69)]
    [InlineData(420420)]
    public void Int32_Nullable_Write_Ref_ShouldBeEqual(int? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-69)]
    [InlineData(420420)]
    public void Int32_Nullable_Write_DefaultShouldBeWritten(int defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Int32(ref value, defaultValue),
            r => r.ReadInt32(), defaultValue);
    }

    #endregion

    #region UInt32

    [Theory]
    [InlineData((uint)0)]
    [InlineData((uint)1)]
    [InlineData((uint)69)]
    [InlineData((uint)420420)]
    public void UInt32_Read_Ref_ShouldEqualExampleValue(uint exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.UInt32(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((uint)0)]
    [InlineData((uint)1)]
    [InlineData((uint)69)]
    [InlineData((uint)420420)]
    public void UInt32_Nullable_Read_Ref_ShouldEqualExampleValue(uint exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<uint?>((rw, value) =>
        {
            rw.UInt32(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void UInt32_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<uint>((rw, value) =>
        {
            rw.UInt32(ref value);
            return value;
        },
        w => w.Write(default(uint)));
    }

    [Theory]
    [InlineData((uint)0)]
    [InlineData((uint)1)]
    [InlineData((uint)69)]
    [InlineData((uint)420420)]
    public void UInt32_Write_Ref_ShouldBeEqual(uint value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((uint)0)]
    [InlineData((uint)1)]
    [InlineData((uint)69)]
    [InlineData((uint)420420)]
    public void UInt32_Nullable_Write_Ref_ShouldBeEqual(uint? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((uint)0)]
    [InlineData((uint)1)]
    [InlineData((uint)69)]
    [InlineData((uint)420420)]
    public void UInt32_Nullable_Write_DefaultShouldBeWritten(uint defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.UInt32(ref value, defaultValue),
            r => r.ReadUInt32(), defaultValue);
    }

    #endregion

    #region Int64

    [Theory]
    [InlineData((long)0)]
    [InlineData((long)1)]
    [InlineData((long)-69)]
    [InlineData((long)420420420420)]
    public void Int64_Read_Ref_ShouldEqualExampleValue(long exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Int64(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((long)0)]
    [InlineData((long)1)]
    [InlineData((long)-69)]
    [InlineData((long)420420420420)]
    public void Int64_Nullable_Read_Ref_ShouldEqualExampleValue(long exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<long?>((rw, value) =>
        {
            rw.Int64(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Int64_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<long>((rw, value) =>
        {
            rw.Int64(ref value);
            return value;
        },
        w => w.Write(default(long)));
    }

    [Theory]
    [InlineData((long)0)]
    [InlineData((long)1)]
    [InlineData((long)-69)]
    [InlineData((long)420420420420)]
    public void Int64_Write_Ref_ShouldBeEqual(long value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int64(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((long)0)]
    [InlineData((long)1)]
    [InlineData((long)-69)]
    [InlineData((long)420420420420)]
    public void Int64_Nullable_Write_Ref_ShouldBeEqual(long? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int64(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((long)0)]
    [InlineData((long)1)]
    [InlineData((long)-69)]
    [InlineData((long)420420420420)]
    public void Int64_Nullable_Write_DefaultShouldBeWritten(long defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Int64(ref value, defaultValue),
            r => r.ReadInt64(), defaultValue);
    }

    #endregion

    #region UInt64

    [Theory]
    [InlineData((ulong)0)]
    [InlineData((ulong)1)]
    [InlineData((ulong)69)]
    [InlineData((ulong)420420420420)]
    public void UInt64_Read_Ref_ShouldEqualExampleValue(ulong exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.UInt64(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData((ulong)0)]
    [InlineData((ulong)1)]
    [InlineData((ulong)69)]
    [InlineData((ulong)420420420420)]
    public void UInt64_Nullable_Read_Ref_ShouldEqualExampleValue(ulong exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<ulong?>((rw, value) =>
        {
            rw.UInt64(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void UInt64_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<ulong>((rw, value) =>
        {
            rw.UInt64(ref value);
            return value;
        },
        w => w.Write(default(ulong)));
    }

    [Theory]
    [InlineData((ulong)0)]
    [InlineData((ulong)1)]
    [InlineData((ulong)69)]
    [InlineData((ulong)420420420420)]
    public void UInt64_Write_Ref_ShouldBeEqual(ulong value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt64(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData((ulong)0)]
    [InlineData((ulong)1)]
    [InlineData((ulong)69)]
    [InlineData((ulong)420420420420)]
    public void UInt64_Nullable_Write_Ref_ShouldBeEqual(ulong? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.UInt64(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData((ulong)0)]
    [InlineData((ulong)1)]
    [InlineData((ulong)69)]
    [InlineData((ulong)420420420420)]
    public void UInt64_Nullable_Write_DefaultShouldBeWritten(ulong defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.UInt64(ref value, defaultValue),
            r => r.ReadUInt64(), defaultValue);
    }

    #endregion

    #region Single

    [Theory]
    [InlineData(0f)]
    [InlineData(1.123f)]
    [InlineData(69.420f)]
    [InlineData(420.420420f)]
    public void Single_Read_Ref_ShouldEqualExampleValue(float exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Single(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(1.123f)]
    [InlineData(69.420f)]
    [InlineData(420.420420f)]
    public void Single_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<float?>((rw, value) =>
        {
            rw.Single(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Single_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<float>((rw, value) =>
        {
            rw.Single(ref value);
            return value;
        },
        w => w.Write(default(float)));
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(1.123f)]
    [InlineData(69.420f)]
    [InlineData(420.420420f)]
    public void Single_Write_Ref_ShouldBeEqual(float value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Single(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0f)]
    [InlineData(1.123f)]
    [InlineData(69.420f)]
    [InlineData(420.420420f)]
    public void Single_Nullable_Write_Ref_ShouldBeEqual(float? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Single(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(1.123f)]
    [InlineData(69.420f)]
    [InlineData(420.420420f)]
    public void Single_Nullable_Write_DefaultShouldBeWritten(float defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Single(ref value, defaultValue),
            r => r.ReadSingle(), defaultValue);
    }

    #endregion

    #region EnumInt32

    [Theory]
    [InlineData(TestEnum.Zero)]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void EnumInt32_Read_Ref_ShouldEqualExampleValue(TestEnum exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.EnumInt32(ref value);
            return value;
        },
        w => w.Write((int)exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(TestEnum.Zero)]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void EnumInt32_Nullable_Read_Ref_ShouldEqualExampleValue(TestEnum exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<TestEnum?>((rw, value) =>
        {
            rw.EnumInt32(ref value);
            return value;
        },
        w => w.Write((int)exampleValue), exampleValue);
    }

    [Fact]
    public void EnumInt32_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<TestEnum>((rw, value) =>
        {
            rw.EnumInt32(ref value);
            return value;
        },
        w => w.Write((int)default(TestEnum)));
    }

    [Theory]
    [InlineData(TestEnum.Zero)]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void EnumInt32_Write_Ref_ShouldBeEqual(TestEnum value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.EnumInt32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(TestEnum.Zero)]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void EnumInt32_Nullable_Write_Ref_ShouldBeEqual(TestEnum? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.EnumInt32(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(TestEnum.Zero)]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void EnumInt32_Nullable_Write_DefaultShouldBeWritten(TestEnum defaultValue)
    {
        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.EnumInt32(ref value, defaultValue),
            r => (TestEnum)r.ReadInt32(), defaultValue);
    }

    #endregion

    #region Int2

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    [InlineData(69, 420)]
    [InlineData(420, 420)]
    public void Int2_Read_Ref_ShouldEqualExampleValue(int exampleValueX, int exampleValueY)
    {
        var exampleValue = new Int2(exampleValueX, exampleValueY);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Int2(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    [InlineData(69, 420)]
    [InlineData(420, 420)]
    public void Int2_Nullable_Read_Ref_ShouldEqualExampleValue(int exampleValueX, int exampleValueY)
    {
        var exampleValue = new Int2(exampleValueX, exampleValueY);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Int2?>((rw, value) =>
        {
            rw.Int2(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Int2_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Int2>((rw, value) =>
        {
            rw.Int2(ref value);
            return value;
        },
        w => w.Write(default(Int2)));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    [InlineData(69, 420)]
    [InlineData(420, 420)]
    public void Int2_Write_Ref_ShouldBeEqual(int valueX, int valueY)
    {
        var value = new Int2(valueX, valueY);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int2(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    [InlineData(69, 420)]
    [InlineData(420, 420)]
    public void Int2_Nullable_Write_Ref_ShouldBeEqual(int? valueX, int? valueY)
    {
        var value = valueX.HasValue && valueY.HasValue
            ? new Int2(valueX.Value, valueY.Value)
            : default(Int2?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int2(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(5, 10)]
    [InlineData(69, 420)]
    [InlineData(420, 420)]
    public void Int2_Nullable_Write_DefaultShouldBeWritten(int defaultValueX, int defaultValueY)
    {
        var defaultValue = new Int2(defaultValueX, defaultValueY);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Int2(ref value, defaultValue),
            r => r.ReadInt2(), defaultValue);
    }

    #endregion

    #region Int3

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(5, 10, 15)]
    [InlineData(69, 420, 69)]
    [InlineData(420, 420, 69)]
    public void Int3_Read_Ref_ShouldEqualExampleValue(int exampleValueX, int exampleValueY, int exampleValueZ)
    {
        var exampleValue = new Int3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Int3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(5, 10, 15)]
    [InlineData(69, 420, 69)]
    [InlineData(420, 420, 69)]
    public void Int3_Nullable_Read_Ref_ShouldEqualExampleValue(int exampleValueX, int exampleValueY, int exampleValueZ)
    {
        var exampleValue = new Int3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Int3?>((rw, value) =>
        {
            rw.Int3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Int3_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Int3>((rw, value) =>
        {
            rw.Int3(ref value);
            return value;
        },
        w => w.Write(default(Int3)));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(5, 10, 15)]
    [InlineData(69, 420, 69)]
    [InlineData(420, 420, 69)]
    public void Int3_Write_Ref_ShouldBeEqual(int valueX, int valueY, int valueZ)
    {
        var value = new Int3(valueX, valueY, valueZ);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(5, 10, 15)]
    [InlineData(69, 420, 69)]
    [InlineData(420, 420, 69)]
    public void Int3_Nullable_Write_Ref_ShouldBeEqual(int? valueX, int? valueY, int? valueZ)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue
            ? new Int3(valueX.Value, valueY.Value, valueZ.Value)
            : default(Int3?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Int3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(5, 10, 15)]
    [InlineData(69, 420, 69)]
    [InlineData(420, 420, 69)]
    public void Int3_Nullable_Write_DefaultShouldBeWritten(int defaultValueX, int defaultValueY, int defaultValueZ)
    {
        var defaultValue = new Int3(defaultValueX, defaultValueY, defaultValueZ);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Int3(ref value, defaultValue),
            r => r.ReadInt3(), defaultValue);
    }

    #endregion

    #region Byte3

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(0, 255, 255)]
    [InlineData(255, 255, 255)]
    public void Byte3_Read_Ref_ShouldEqualExampleValue(byte exampleValueX, byte exampleValueY, byte exampleValueZ)
    {
        var exampleValue = new Byte3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Byte3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(0, 255, 255)]
    [InlineData(255, 255, 255)]
    public void Byte3_Nullable_Read_Ref_ShouldEqualExampleValue(byte exampleValueX, byte exampleValueY, byte exampleValueZ)
    {
        var exampleValue = new Byte3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Byte3?>((rw, value) =>
        {
            rw.Byte3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Byte3_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Byte3>((rw, value) =>
        {
            rw.Byte3(ref value);
            return value;
        },
        w => w.Write(default(Byte3)));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(0, 255, 255)]
    [InlineData(255, 255, 255)]
    public void Byte3_Write_Ref_ShouldBeEqual(byte valueX, byte valueY, byte valueZ)
    {
        var value = new Byte3(valueX, valueY, valueZ);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Byte3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData((byte)0, (byte)0, (byte)0)]
    [InlineData((byte)0, (byte)1, (byte)2)]
    [InlineData((byte)3, (byte)4, (byte)5)]
    [InlineData((byte)0, (byte)255, (byte)255)]
    [InlineData((byte)255, (byte)255, (byte)255)]
    public void Byte3_Nullable_Write_Ref_ShouldBeEqual(byte? valueX, byte? valueY, byte? valueZ)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue
            ? new Byte3(valueX.Value, valueY.Value, valueZ.Value)
            : default(Byte3?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Byte3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(0, 255, 255)]
    [InlineData(255, 255, 255)]
    public void Byte3_Nullable_Write_DefaultShouldBeWritten(byte defaultValueX, byte defaultValueY, byte defaultValueZ)
    {
        var defaultValue = new Byte3(defaultValueX, defaultValueY, defaultValueZ);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Byte3(ref value, defaultValue),
            r => r.ReadByte3(), defaultValue);
    }

    #endregion

    #region Vec2

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.12f, 1.123f)]
    [InlineData(5.5f, 10.15f)]
    [InlineData(69.420f, 420.69f)]
    [InlineData(420.69f, 420.1f)]
    public void Vec2_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY)
    {
        var exampleValue = new Vec2(exampleValueX, exampleValueY);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Vec2(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.12f, 1.123f)]
    [InlineData(5.5f, 10.15f)]
    [InlineData(69.420f, 420.69f)]
    [InlineData(420.69f, 420.1f)]
    public void Vec2_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY)
    {
        var exampleValue = new Vec2(exampleValueX, exampleValueY);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Vec2?>((rw, value) =>
        {
            rw.Vec2(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Vec2_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Vec2>((rw, value) =>
        {
            rw.Vec2(ref value);
            return value;
        },
        w => w.Write(default(Vec2)));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.12f, 1.123f)]
    [InlineData(5.5f, 10.15f)]
    [InlineData(69.420f, 420.69f)]
    [InlineData(420.69f, 420.1f)]
    public void Vec2_Write_Ref_ShouldBeEqual(float valueX, float valueY)
    {
        var value = new Vec2(valueX, valueY);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec2(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(0, 0)]
    [InlineData(0.12f, 1.123f)]
    [InlineData(5.5f, 10.15f)]
    [InlineData(69.420f, 420.69f)]
    [InlineData(420.69f, 420.1f)]
    public void Vec2_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY)
    {
        var value = valueX.HasValue && valueY.HasValue
            ? new Vec2(valueX.Value, valueY.Value)
            : default(Vec2?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec2(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.12f, 1.123f)]
    [InlineData(5.5f, 10.15f)]
    [InlineData(69.420f, 420.69f)]
    [InlineData(420.69f, 420.1f)]
    public void Vec2_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY)
    {
        var defaultValue = new Vec2(defaultValueX, defaultValueY);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Vec2(ref value, defaultValue),
            r => r.ReadVec2(), defaultValue);
    }

    #endregion

    #region Vec3

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f)]
    [InlineData(5.5f, 10.15f, 15.30f)]
    [InlineData(69.420f, 420.69f, 69.69f)]
    [InlineData(420.420f, 420.69f, 69.1f)]
    public void Vec3_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ)
    {
        var exampleValue = new Vec3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Vec3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f)]
    [InlineData(5.5f, 10.15f, 15.30f)]
    [InlineData(69.420f, 420.69f, 69.69f)]
    [InlineData(420.420f, 420.69f, 69.1f)]
    public void Vec3_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ)
    {
        var exampleValue = new Vec3(exampleValueX, exampleValueY, exampleValueZ);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Vec3?>((rw, value) =>
        {
            rw.Vec3(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Vec3_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Vec3>((rw, value) =>
        {
            rw.Vec3(ref value);
            return value;
        },
        w => w.Write(default(Vec3)));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f)]
    [InlineData(5.5f, 10.15f, 15.30f)]
    [InlineData(69.420f, 420.69f, 69.69f)]
    [InlineData(420.420f, 420.69f, 69.1f)]
    public void Vec3_Write_Ref_ShouldBeEqual(float valueX, float valueY, float valueZ)
    {
        var value = new Vec3(valueX, valueY, valueZ);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f)]
    [InlineData(5.5f, 10.15f, 15.30f)]
    [InlineData(69.420f, 420.69f, 69.69f)]
    [InlineData(420.420f, 420.69f, 69.1f)]
    public void Vec3_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY, float? valueZ)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue
            ? new Vec3(valueX.Value, valueY.Value, valueZ.Value)
            : default(Vec3?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec3(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f)]
    [InlineData(5.5f, 10.15f, 15.30f)]
    [InlineData(69.420f, 420.69f, 69.69f)]
    [InlineData(420.420f, 420.69f, 69.1f)]
    public void Vec3_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY, float defaultValueZ)
    {
        var defaultValue = new Vec3(defaultValueX, defaultValueY, defaultValueZ);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Vec3(ref value, defaultValue),
            r => r.ReadVec3(), defaultValue);
    }

    #endregion

    #region Vec4

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Vec4_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueW)
    {
        var exampleValue = new Vec4(exampleValueX, exampleValueY, exampleValueZ, exampleValueW);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Vec4(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Vec4_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueW)
    {
        var exampleValue = new Vec4(exampleValueX, exampleValueY, exampleValueZ, exampleValueW);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Vec4?>((rw, value) =>
        {
            rw.Vec4(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Vec4_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Vec4>((rw, value) =>
        {
            rw.Vec4(ref value);
            return value;
        },
        w => w.Write(default(Vec4)));
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Vec4_Write_Ref_ShouldBeEqual(float valueX, float valueY, float valueZ, float valueW)
    {
        var value = new Vec4(valueX, valueY, valueZ, valueW);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec4(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Vec4_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY, float? valueZ, float? valueW)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue && valueW.HasValue
            ? new Vec4(valueX.Value, valueY.Value, valueZ.Value, valueW.Value)
            : default(Vec4?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Vec4(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Vec4_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY, float defaultValueZ, float defaultValueW)
    {
        var defaultValue = new Vec4(defaultValueX, defaultValueY, defaultValueZ, defaultValueW);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Vec4(ref value, defaultValue),
            r => r.ReadVec4(), defaultValue);
    }

    #endregion

    #region Rect

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Rect_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueX2, float exampleValueY2)
    {
        var exampleValue = new Rect(exampleValueX, exampleValueY, exampleValueX2, exampleValueY2);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Rect(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Rect_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueX2, float exampleValueY2)
    {
        var exampleValue = new Rect(exampleValueX, exampleValueY, exampleValueX2, exampleValueY2);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Rect?>((rw, value) =>
        {
            rw.Rect(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Rect_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Rect>((rw, value) =>
        {
            rw.Rect(ref value);
            return value;
        },
        w => w.Write(default(Rect)));
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Rect_Write_Ref_ShouldBeEqual(float valueX, float valueY, float valueX2, float valueY2)
    {
        var value = new Rect(valueX, valueY, valueX2, valueY2);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Rect(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Rect_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY, float? valueX2, float? valueY2)
    {
        var value = valueX.HasValue && valueY.HasValue && valueX2.HasValue && valueY2.HasValue
            ? new Rect(valueX.Value, valueY.Value, valueX2.Value, valueY2.Value)
            : default(Rect?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Rect(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Rect_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY, float defaultValueX2, float defaultValueY2)
    {
        var defaultValue = new Rect(defaultValueX, defaultValueY, defaultValueX2, defaultValueY2);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Rect(ref value, defaultValue),
            r => r.ReadRect(), defaultValue);
    }

    #endregion

    #region Box

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0, 69.1f, 0)]
    public void Box_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueX2, float exampleValueY2, float exampleValueZ2)
    {
        var exampleValue = new Box(exampleValueX, exampleValueY, exampleValueZ, exampleValueX2, exampleValueY2, exampleValueZ2);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Box(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0, 69.1f, 0)]
    public void Box_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueX2, float exampleValueY2, float exampleValueZ2)
    {
        var exampleValue = new Box(exampleValueX, exampleValueY, exampleValueZ, exampleValueX2, exampleValueY2, exampleValueZ2);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Box?>((rw, value) =>
        {
            rw.Box(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Box_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Box>((rw, value) =>
        {
            rw.Box(ref value);
            return value;
        },
        w => w.Write(default(Box)));
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0, 69.1f, 0)]
    public void Box_Write_Ref_ShouldBeEqual(float valueX, float valueY, float valueZ, float valueX2, float valueY2, float valueZ2)
    {
        var value = new Box(valueX, valueY, valueZ, valueX2, valueY2, valueZ2);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Box(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null, null, null, null)]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0, 69.1f, 0)]
    public void Box_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY, float? valueZ, float? valueX2, float? valueY2, float? valueZ2)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue && valueX2.HasValue && valueY2.HasValue && valueZ2.HasValue
            ? new Box(valueX.Value, valueY.Value, valueZ.Value, valueX2.Value, valueY2.Value, valueZ2.Value)
            : default(Box?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Box(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0, 69.1f, 0)]
    public void Box_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY, float defaultValueZ, float defaultValueX2, float defaultValueY2, float defaultValueZ2)
    {
        var defaultValue = new Box(defaultValueX, defaultValueY, defaultValueZ, defaultValueX2, defaultValueY2, defaultValueZ2);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Box(ref value, defaultValue),
            r => r.ReadBox(), defaultValue);
    }

    #endregion

    #region Quat

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Quat_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueW)
    {
        var exampleValue = new Quat(exampleValueX, exampleValueY, exampleValueZ, exampleValueW);

        GeneralTest_Read_Ref_ShouldEqualExampleValue((rw, value) =>
        {
            rw.Quat(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Quat_Nullable_Read_Ref_ShouldEqualExampleValue(float exampleValueX, float exampleValueY, float exampleValueZ, float exampleValueW)
    {
        var exampleValue = new Quat(exampleValueX, exampleValueY, exampleValueZ, exampleValueW);

        GeneralTest_Read_Ref_ShouldEqualExampleValue<Quat?>((rw, value) =>
        {
            rw.Quat(ref value);
            return value;
        },
        w => w.Write(exampleValue), exampleValue);
    }

    [Fact]
    public void Quat_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<Quat>((rw, value) =>
        {
            rw.Quat(ref value);
            return value;
        },
        w => w.Write(default(Quat)));
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Quat_Write_Ref_ShouldBeEqual(float valueX, float valueY, float valueZ, float valueW)
    {
        var value = new Quat(valueX, valueY, valueZ, valueW);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Quat(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Quat_Nullable_Write_Ref_ShouldBeEqual(float? valueX, float? valueY, float? valueZ, float? valueW)
    {
        var value = valueX.HasValue && valueY.HasValue && valueZ.HasValue && valueW.HasValue
            ? new Quat(valueX.Value, valueY.Value, valueZ.Value, valueW.Value)
            : default(Quat?);

        GeneralTest_Write_Ref_ShouldBeEqual((rw, value) =>
        {
            rw.Quat(ref value);
            return value;
        },
        value);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0.12f, 1.123f, 2.345f, 1.568f)]
    [InlineData(5.5f, 10.15f, 15.30f, 16.15f)]
    [InlineData(69.420f, 420.69f, 69.69f, 111.2f)]
    [InlineData(420.420f, 420.69f, 69.1f, 0)]
    public void Quat_Nullable_Write_DefaultShouldBeWritten(float defaultValueX, float defaultValueY, float defaultValueZ, float defaultValueW)
    {
        var defaultValue = new Quat(defaultValueX, defaultValueY, defaultValueZ, defaultValueW);

        GeneralTest_Write_Nullable_DefaultShouldBeWritten(
            (rw, value, defaultValue) => rw.Quat(ref value, defaultValue),
            r => r.ReadQuat(), defaultValue);
    }

    #endregion

    #region Bytes

    [Theory]
    [InlineData(new byte[] { 0, 1, 2 }, 3, new byte[] { 0, 1, 2 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255 }, 4, new byte[] { 3, 4, 5, 0 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255, 255, 255, 255 }, 6, new byte[] { 3, 4, 5, 0, 255, 255 })]
    public void Bytes_Count_Read_Ref_ShouldEqualValueWithExampleLength(byte[] exampleValue, int exampleLength, byte[] expectedValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<byte[]?>((rw, value) =>
        {
            rw.Bytes(ref value, exampleLength);
            return value;
        },
        w => w.WriteBytes(exampleValue), expectedValue);
    }

    [Theory]
    [InlineData(new byte[] { 0, 1, 2 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255, 255, 255, 255 })]
    public void Bytes_Read_Ref_ShouldEqualExampleValue(byte[] exampleValue)
    {
        GeneralTest_Read_Ref_ShouldEqualExampleValue<byte[]?>((rw, value) =>
        {
            rw.Bytes(ref value);
            return value;
        },
        w => w.WriteArray(exampleValue), exampleValue);
    }

    [Fact]
    public void Bytes_Nullable_Read_Ref_NullValueShouldNotEqual()
    {
        GeneralTest_Read_Ref_NullValueShouldNotEqual<byte[]?>((rw, value) =>
        {
            rw.Bytes(ref value);
            return value;
        },
        w => w.Write(0));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[] { 0, 1, 2 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255 })]
    [InlineData(new byte[] { 3, 4, 5, 0, 255, 255, 255, 255, 255 })]
    public void Bytes_Write_Ref_ShouldBeEqual(byte[]? value)
    {
        GeneralTest_Write_Ref_ShouldBeEqual<byte[]?>((rw, value) =>
        {
            rw.Bytes(ref value);
            return value;
        },
        value);
    }

    #endregion
}
