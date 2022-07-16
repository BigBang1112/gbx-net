using System;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class Vec4Tests
{
    [Fact]
    public void Constructor()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal(1, vec.X);
        Assert.Equal(2, vec.Y);
        Assert.Equal(3, vec.Z);
        Assert.Equal(4, vec.W);
    }

    // test GetSqrMagniture
    [Fact]
    public void GetSqrMagnitude()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal(30, vec.GetSqrMagnitude());
    }

    // test GetMagnitude
    [Fact]
    public void GetMagnitude()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal(5.477226f, vec.GetMagnitude());
    }

    // test GetVec4FromReadOnlySpan
    [Fact]
    public void GetVec4FromReadOnlySpan_4()
    {
        var vec = new Vec4(1, 2, 3, 4);
        var span = new ReadOnlySpan<float>(new float[] { 1, 2, 3, 4 });
        Assert.Equal(vec, Vec4.GetVec4FromReadOnlySpan(span));
    }

    // test GetVec4FromReadOnlySpan
    [Fact]
    public void GetVec4FromReadOnlySpan_3()
    {
        var vec = new Vec4(1, 2, 3, 0);
        var span = new ReadOnlySpan<float>(new float[] { 1, 2, 3 });
        Assert.Equal(vec, Vec4.GetVec4FromReadOnlySpan(span));
    }

    // test GetVec4FromReadOnlySpan
    [Fact]
    public void GetVec4FromReadOnlySpan_2()
    {
        var vec = new Vec4(1, 2, 0, 0);
        var span = new ReadOnlySpan<float>(new float[] { 1, 2 });
        Assert.Equal(vec, Vec4.GetVec4FromReadOnlySpan(span));
    }

    // test GetVec4FromReadOnlySpan
    [Fact]
    public void GetVec4FromReadOnlySpan_1()
    {
        var vec = new Vec4(1, 0, 0, 0);
        var span = new ReadOnlySpan<float>(new float[] { 1 });
        Assert.Equal(vec, Vec4.GetVec4FromReadOnlySpan(span));
    }

    // test GetVec4FromReadOnlySpan
    [Fact]
    public void GetVec4FromReadOnlySpan_0()
    {
        var vec = new Vec4(0, 0, 0, 0);
        var span = ReadOnlySpan<float>.Empty;
        Assert.Equal(vec, Vec4.GetVec4FromReadOnlySpan(span));
    }

    // test operator +
    [Fact]
    public void OperatorPlus()
    {
        var vec1 = new Vec4(1, 2, 3, 4);
        var vec2 = new Vec4(5, 6, 7, 8);
        Assert.Equal(new Vec4(6, 8, 10, 12), vec1 + vec2);
    }

    // test operator - 
    [Fact]
    public void OperatorMinus()
    {
        var vec1 = new Vec4(1, 2, 3, 4);
        var vec2 = new Vec4(5, 6, 7, 8);
        Assert.Equal(new Vec4(-4, -4, -4, -4), vec1 - vec2);
    }

    // test operator - (unary)
    [Fact]
    public void OperatorMinusUnary()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal(new Vec4(-1, -2, -3, -4), -vec);
    }

    // test operator *
    [Fact]
    public void OperatorMultiply()
    {
        var vec1 = new Vec4(1, 2, 3, 4);
        var vec2 = new Vec4(5, 6, 7, 8);
        Assert.Equal(new Vec4(5, 12, 21, 32), vec1 * vec2);
    }

    // test GetDotProduct
    [Fact]
    public void GetDotProduct()
    {
        var vec1 = new Vec4(1, 2, 3, 4);
        var vec2 = new Vec4(5, 6, 7, 8);
        Assert.Equal(70, Vec4.GetDotProduct(vec1, vec2));
    }

    // test Zero
    [Fact]
    public void Zero()
    {
        var vec = Vec4.Zero;
        Assert.Equal(0, vec.X);
        Assert.Equal(0, vec.Y);
        Assert.Equal(0, vec.Z);
        Assert.Equal(0, vec.W);
    }

    // test explicit operator Vec4(Vec3)
    [Fact]
    public void ExplicitOperator_Vec4Vec3()
    {
        var vec = new Vec3(1, 2, 3);
        Assert.Equal(new Vec4(1, 2, 3, 0), (Vec4)vec);
    }

    // test explicit operator Vec4(Vec2)
    [Fact]
    public void ExplicitOperator_Vec4Vec2()
    {
        var vec = new Vec2(1, 2);
        Assert.Equal(new Vec4(1, 2, 0, 0), (Vec4)vec);
    }

    // test implicit operator Vec4(float X, float Y, float Z, float W)
    [Fact]
    public void ImplicitOperator_Vec4FloatXFloatYFloatZFloatW()
    {
        var vec = (1, 2, 3, 4);
        Assert.Equal(new Vec4(1, 2, 3, 4), vec);
    }

    // test implicit operator (float X, float Y, float Z, float W)Vec4
    [Fact]
    public void ImplicitOperator_FloatXFloatYFloatZFloatWVec4()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal((1, 2, 3, 4), vec);
    }

    // test ToString
    [Fact]
    public void ToStringTest()
    {
        var vec = new Vec4(1, 2, 3, 4);
        Assert.Equal("<1, 2, 3, 4>", vec.ToString());
    }
}
