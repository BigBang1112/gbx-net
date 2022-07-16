using Xunit;

namespace GBX.NET.Tests.Unit;

public class Byte3Tests
{
    // test ToString
    [Fact]
    public void ToStringTest()
    {
        var b = new Byte3(2, 4, 6);
        Assert.Equal("<2, 4, 6>", b.ToString());
    }

    // test operator + Byte3
    [Fact]
    public void OperatorPlus_Byte3()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = new Byte3(1, 3, 5);
        var b3 = b1 + b2;
        Assert.Equal(new Byte3(3, 7, 11), b3);
    }

    // test operator + byte
    [Fact]
    public void OperatorPlus_Byte()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = b1 + 1;
        Assert.Equal(new Byte3(3, 5, 7), b2);
    }

    // test operator - Byte3
    [Fact]
    public void OperatorMinus_Byte3()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = new Byte3(1, 3, 5);
        var b3 = b1 - b2;
        Assert.Equal(new Byte3(1, 1, 1), b3);
    }

    // test operator - byte
    [Fact]
    public void OperatorMinus_Byte()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = b1 - 1;
        Assert.Equal(new Byte3(1, 3, 5), b2);
    }

    // test operator * Byte3
    [Fact]
    public void OperatorMultiply_Byte3()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = new Byte3(1, 3, 5);
        var b3 = b1 * b2;
        Assert.Equal(new Byte3(2, 12, 30), b3);
    }

    // test operator * byte
    [Fact]
    public void OperatorMultiply_Byte()
    {
        var b1 = new Byte3(2, 4, 6);
        var b2 = b1 * 2;
        Assert.Equal(new Byte3(4, 8, 12), b2);
    }

    // test implicit operator Byte3 (byte X, byte Y, byte Z)
    [Fact]
    public void ImplicitOperator_Byte3_ByteXByteYByteZ()
    {
        var b = ((byte)2, (byte)4, (byte)6);
        Assert.Equal<Byte3>(new Byte3(2, 4, 6), b);
    }

    // test implicit operator (byte X, byte Y, byte Z) Byte3
    [Fact]
    public void ImplicitOperator_ByteXByteYByteZ_Byte3()
    {
        var b = new Byte3(2, 4, 6);
        Assert.Equal<(byte, byte, byte)>((2, 4, 6), b);
    }

    // test explicit operator Int3
    [Fact]
    public void ExplicitOperator_Int3()
    {
        var b = new Byte3(2, 4, 6);
        Assert.Equal(new Int3(2, 4, 6), (Int3)b);
    }
}
