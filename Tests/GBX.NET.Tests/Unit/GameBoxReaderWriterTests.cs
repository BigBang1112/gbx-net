using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Unit
{
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
            var stream = rw.Writer.BaseStream;
            stream.Seek(0, SeekOrigin.Begin);
            using var r = new GameBoxReader(rw.Writer.BaseStream);
            return reader.Invoke(r);
        }

        private static void GeneralTest_Read_RefShouldEqualExampleValue<T>(Func<GameBoxReaderWriter, T, T> action,
            Action<GameBoxWriter> intialWrite, T exampleValue = default)
        {
            // Arrange
            var rw = PrepareInReadMode(intialWrite);
            var value = default(T);

            // Act
            value = action.Invoke(rw, value);

            // Assert
            Assert.Equal(exampleValue, value);
        }

        private static void GeneralTest_Read_NullValueShouldNotEqual<T>(Func<GameBoxReaderWriter, T, T> action,
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

        private static void GeneralTest_Write_RefShouldBeEqual<T>(Func<GameBoxReaderWriter, T, T> action, T value = default)
        {
            // Arrange
            var rw = PrepareInWriteMode();
            var expected = value;

            // Act
            value = action.Invoke(rw, value);

            // Assert
            Assert.Equal(expected, actual: value);
        }

        private static void GeneralTest_Write_Nullable_DefaultShouldBeWritten<TValue, TDefault>(
            Action<GameBoxReaderWriter, TValue, TDefault> action, Func<GameBoxReader, TDefault> valueBack, TDefault defaultValue)
        {
            // Arrange
            var rw = PrepareInWriteMode();
            var value = default(TValue);

            // Act
            action.Invoke(rw, value, defaultValue);

            var actualValue = GetValueBack(rw, valueBack);

            // Assert
            Assert.Equal(expected: defaultValue, actual: actualValue);
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)-69)]
        [InlineData((short)6969)]
        public void Int16_Read_RefShouldEqualExampleValue(short exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int16(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)-69)]
        [InlineData((short)6969)]
        public void Int16_Nullable_Read_RefShouldEqualExampleValue(short? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int16(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void Int16_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<short>((rw, value) =>
            {
                rw.Int16(ref value);
                return value;
            }, w => w.Write(default(short)));
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)-69)]
        [InlineData((short)6969)]
        public void Int16_Write_RefShouldBeEqual(short value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int16(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)-69)]
        [InlineData((short)6969)]
        public void Int16_Nullable_Write_RefShouldBeEqual(short? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int16(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)1)]
        [InlineData((short)-69)]
        [InlineData((short)6969)]
        public void Int16_Nullable_Write_DefaultShouldBeWritten(short defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<short?, short>(
                (rw, value, defaultValue) => rw.Int16(ref value, defaultValue),
                r => r.ReadInt16(), defaultValue);
        }

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)69)]
        [InlineData((ushort)42069)]
        public void UInt16_Read_RefShouldEqualExampleValue(ushort exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt16(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)69)]
        [InlineData((ushort)42069)]
        public void UInt16_Nullable_Read_RefShouldEqualExampleValue(ushort? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt16(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void UInt16_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<ushort>((rw, value) =>
            {
                rw.UInt16(ref value);
                return value;
            }, w => w.Write(default(ushort)));
        }

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)69)]
        [InlineData((ushort)42069)]
        public void UInt16_Write_RefShouldBeEqual(ushort value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt16(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)69)]
        [InlineData((ushort)42069)]
        public void UInt16_Nullable_Write_RefShouldBeEqual(ushort? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt16(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)69)]
        [InlineData((ushort)42069)]
        public void UInt16_Nullable_Write_DefaultShouldBeWritten(ushort defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<ushort?, ushort>(
                (rw, value, defaultValue) => rw.UInt16(ref value, defaultValue),
                r => r.ReadUInt16(), defaultValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-69)]
        [InlineData(420420)]
        public void Int32_Read_RefShouldEqualExampleValue(int exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int32(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-69)]
        [InlineData(420420)]
        public void Int32_Nullable_Read_RefShouldEqualExampleValue(int? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int32(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void Int32_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<int>((rw, value) =>
            {
                rw.Int32(ref value);
                return value;
            }, w => w.Write(default(int)));
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-69)]
        [InlineData(420420)]
        public void Int32_Write_RefShouldBeEqual(int value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-69)]
        [InlineData(420420)]
        public void Int32_Nullable_Write_RefShouldBeEqual(int? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-69)]
        [InlineData(420420)]
        public void Int32_Nullable_Write_DefaultShouldBeWritten(int defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<int?, int>(
                (rw, value, defaultValue) => rw.Int32(ref value, defaultValue),
                r => r.ReadInt32(), defaultValue);
        }

        [Theory]
        [InlineData((uint)0)]
        [InlineData((uint)1)]
        [InlineData((uint)69)]
        [InlineData((uint)420420)]
        public void UInt32_Read_RefShouldEqualExampleValue(uint exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt32(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData((uint)0)]
        [InlineData((uint)1)]
        [InlineData((uint)69)]
        [InlineData((uint)420420)]
        public void UInt32_Nullable_Read_RefShouldEqualExampleValue(uint? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt32(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void UInt32_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<uint>((rw, value) =>
            {
                rw.UInt32(ref value);
                return value;
            }, w => w.Write(default(uint)));
        }

        [Theory]
        [InlineData((uint)0)]
        [InlineData((uint)1)]
        [InlineData((uint)69)]
        [InlineData((uint)420420)]
        public void UInt32_Write_RefShouldBeEqual(uint value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((uint)0)]
        [InlineData((uint)1)]
        [InlineData((uint)69)]
        [InlineData((uint)420420)]
        public void UInt32_Nullable_Write_RefShouldBeEqual(uint? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData((uint)0)]
        [InlineData((uint)1)]
        [InlineData((uint)69)]
        [InlineData((uint)420420)]
        public void UInt32_Nullable_Write_DefaultShouldBeWritten(uint defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<uint?, uint>(
                (rw, value, defaultValue) => rw.UInt32(ref value, defaultValue),
                r => r.ReadUInt32(), defaultValue);
        }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)-69)]
        [InlineData((long)420420420420)]
        public void Int64_Read_RefShouldEqualExampleValue(long exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int64(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)-69)]
        [InlineData((long)420420420420)]
        public void Int64_Nullable_Read_RefShouldEqualExampleValue(long? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.Int64(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void Int64_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<long>((rw, value) =>
            {
                rw.Int64(ref value);
                return value;
            }, w => w.Write(default(long)));
        }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)-69)]
        [InlineData((long)420420420420)]
        public void Int64_Write_RefShouldBeEqual(long value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int64(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)-69)]
        [InlineData((long)420420420420)]
        public void Int64_Nullable_Write_RefShouldBeEqual(long? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.Int64(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData((long)0)]
        [InlineData((long)1)]
        [InlineData((long)-69)]
        [InlineData((long)420420420420)]
        public void Int64_Nullable_Write_DefaultShouldBeWritten(long defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<long?, long>(
                (rw, value, defaultValue) => rw.Int64(ref value, defaultValue),
                r => r.ReadInt64(), defaultValue);
        }

        [Theory]
        [InlineData((ulong)0)]
        [InlineData((ulong)1)]
        [InlineData((ulong)69)]
        [InlineData((ulong)420420420420)]
        public void UInt64_Read_RefShouldEqualExampleValue(ulong exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt64(ref value);
                return value;
            }, w => w.Write(exampleValue), exampleValue);
        }

        [Theory]
        [InlineData((ulong)0)]
        [InlineData((ulong)1)]
        [InlineData((ulong)69)]
        [InlineData((ulong)420420420420)]
        public void UInt64_Nullable_Read_RefShouldEqualExampleValue(ulong? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.UInt64(ref value);
                return value;
            }, w => w.Write(exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void UInt64_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<ulong>((rw, value) =>
            {
                rw.UInt64(ref value);
                return value;
            }, w => w.Write(default(ulong)));
        }

        [Theory]
        [InlineData((ulong)0)]
        [InlineData((ulong)1)]
        [InlineData((ulong)69)]
        [InlineData((ulong)420420420420)]
        public void UInt64_Write_RefShouldBeEqual(ulong value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt64(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((ulong)0)]
        [InlineData((ulong)1)]
        [InlineData((ulong)69)]
        [InlineData((ulong)420420420420)]
        public void UInt64_Nullable_Write_RefShouldBeEqual(ulong? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.UInt64(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData((ulong)0)]
        [InlineData((ulong)1)]
        [InlineData((ulong)69)]
        [InlineData((ulong)420420420420)]
        public void UInt64_Nullable_Write_DefaultShouldBeWritten(ulong defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<ulong?, ulong>(
                (rw, value, defaultValue) => rw.UInt64(ref value, defaultValue),
                r => r.ReadUInt64(), defaultValue);
        }

        [Theory]
        [InlineData(TestEnum.One)]
        [InlineData(TestEnum.Two)]
        [InlineData(TestEnum.Three)]
        [InlineData(TestEnum.Four)]
        public void EnumInt32_Read_RefShouldEqualExampleValue(TestEnum exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.EnumInt32(ref value);
                return value;
            }, w => w.Write((int)exampleValue), exampleValue);
        }

        [Theory]
        [InlineData(TestEnum.One)]
        [InlineData(TestEnum.Two)]
        [InlineData(TestEnum.Three)]
        [InlineData(TestEnum.Four)]
        public void EnumInt32_Nullable_Read_RefShouldEqualExampleValue(TestEnum? exampleValue)
        {
            GeneralTest_Read_RefShouldEqualExampleValue((rw, value) =>
            {
                rw.EnumInt32(ref value);
                return value;
            }, w => w.Write((int)exampleValue.GetValueOrDefault()), exampleValue);
        }

        [Fact]
        public void EnumInt32_Nullable_Read_NullValueShouldNotEqual()
        {
            GeneralTest_Read_NullValueShouldNotEqual<TestEnum>((rw, value) =>
            {
                rw.EnumInt32(ref value);
                return value;
            }, w => w.Write((int)default(TestEnum)));
        }
        
        [Theory]
        [InlineData(TestEnum.One)]
        [InlineData(TestEnum.Two)]
        [InlineData(TestEnum.Three)]
        [InlineData(TestEnum.Four)]
        public void EnumInt32_Write_RefShouldBeEqual(TestEnum value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.EnumInt32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(TestEnum.One)]
        [InlineData(TestEnum.Two)]
        [InlineData(TestEnum.Three)]
        [InlineData(TestEnum.Four)]
        public void EnumInt32_Nullable_Write_RefShouldBeEqual(TestEnum? value)
        {
            GeneralTest_Write_RefShouldBeEqual((rw, value) =>
            {
                rw.EnumInt32(ref value);
                return value;
            }, value);
        }

        [Theory]
        [InlineData(TestEnum.Zero)]
        [InlineData(TestEnum.One)]
        [InlineData(TestEnum.Two)]
        [InlineData(TestEnum.Three)]
        [InlineData(TestEnum.Four)]
        public void EnumInt32_Nullable_Write_DefaultShouldBeWritten(TestEnum defaultValue)
        {
            GeneralTest_Write_Nullable_DefaultShouldBeWritten<TestEnum?, TestEnum>(
                (rw, value, defaultValue) => rw.EnumInt32(ref value, defaultValue),
                r => (TestEnum)r.ReadInt32(), defaultValue);
        }
    }
}
