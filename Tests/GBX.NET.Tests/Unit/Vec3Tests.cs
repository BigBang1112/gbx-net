using Xunit;

namespace GBX.NET.Tests.Unit;

public class Vec3Tests
{
    // test GetXY, Z should be 0
    [Fact]
    public void GetXY()
    {
        var vec = new Vec3(1, 2, 3).GetXY();
        Assert.Equal(1, vec.X);
        Assert.Equal(2, vec.Y);
        Assert.Equal(0, vec.Z);
    }

    // test GetXZ, Y should be 0
    [Fact]
    public void GetXZ()
    {
        var vec = new Vec3(1, 2, 3).GetXZ();
        Assert.Equal(1, vec.X);
        Assert.Equal(0, vec.Y);
        Assert.Equal(3, vec.Z);
    }

    // test GetYZ, X should be 0
    [Fact]
    public void GetYZ()
    {
        var vec = new Vec3(1, 2, 3).GetYZ();
        Assert.Equal(0, vec.X);
        Assert.Equal(2, vec.Y);
        Assert.Equal(3, vec.Z);
    }

    // test GetMagnitude
    [Fact]
    public void GetMagnitude()
    {
        var vec = new Vec3(1, 2, 3);
        Assert.Equal(3.7416573867739413f, vec.GetMagnitude());
    }

    // test GetSqrMagnitude
    [Fact]
    public void GetSqrMagnitude()
    {
        var vec = new Vec3(1, 2, 3);
        Assert.Equal(14, vec.GetSqrMagnitude());
    }

    // test Zero
    [Fact]
    public void Zero()
    {
        var vec = Vec3.Zero;
        Assert.Equal(0, vec.X);
        Assert.Equal(0, vec.Y);
        Assert.Equal(0, vec.Z);
    }

    // test GetDotProduct
    [Fact]
    public void GetDotProduct()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec3(4, 5, 6);
        Assert.Equal(32, Vec3.GetDotProduct(vec1, vec2));
    }

    // test operator + (Vec3)
    [Fact]
    public void OperatorPlus_Vec3()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 + vec2;
        Assert.Equal(5, vec3.X);
        Assert.Equal(7, vec3.Y);
        Assert.Equal(9, vec3.Z);
    }

    // test operator + (Int3)
    [Fact]
    public void OperatorPlus_Int3()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Int3(4, 5, 6);
        var vec3 = vec1 + vec2;
        Assert.Equal(5, vec3.X);
        Assert.Equal(7, vec3.Y);
        Assert.Equal(9, vec3.Z);
    }

    // test operator + (Vec2)
    [Fact]
    public void OperatorPlus_Vec2()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec2(4, 5);
        var vec3 = vec1 + vec2;
        Assert.Equal(5, vec3.X);
        Assert.Equal(7, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator + (Int2)
    [Fact]
    public void OperatorPlus_Int2()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Int2(4, 5);
        var vec3 = vec1 + vec2;
        Assert.Equal(5, vec3.X);
        Assert.Equal(7, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator + (int)
    [Fact]
    public void OperatorPlus_Int()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = 4;
        var vec3 = vec1 + vec2;
        Assert.Equal(5, vec3.X);
        Assert.Equal(6, vec3.Y);
        Assert.Equal(7, vec3.Z);
    }

    // test operator + (float)
    [Fact]
    public void OperatorPlus_Float()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = vec1 + 4.5f;
        Assert.Equal(5.5f, vec2.X);
        Assert.Equal(6.5f, vec2.Y);
        Assert.Equal(7.5f, vec2.Z);
    }

    // test operator - (unary)
    [Fact]
    public void OperatorMinusUnary()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = -vec1;
        Assert.Equal(-1, vec2.X);
        Assert.Equal(-2, vec2.Y);
        Assert.Equal(-3, vec2.Z);
    }

    // test operator -
    [Fact]
    public void OperatorMinus_Vec3()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 - vec2;
        Assert.Equal(-3, vec3.X);
        Assert.Equal(-3, vec3.Y);
        Assert.Equal(-3, vec3.Z);
    }

    // test operator - (Int3)
    [Fact]
    public void OperatorMinus_Int3()
    {
        var vec = new Vec3(1, 2, 3);
        var int3 = new Int3(4, 5, 6);
        var vec3 = vec - int3;
        Assert.Equal(-3, vec3.X);
        Assert.Equal(-3, vec3.Y);
        Assert.Equal(-3, vec3.Z);
    }

    // test operator - (Vec2)
    [Fact]
    public void OperatorMinus_Vec2()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec2(4, 5);
        var vec3 = vec1 - vec2;
        Assert.Equal(-3, vec3.X);
        Assert.Equal(-3, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator - (Int2)
    [Fact]
    public void OperatorMinus_Int2()
    {
        var vec = new Vec3(1, 2, 3);
        var int2 = new Int2(4, 5);
        var vec3 = vec - int2;
        Assert.Equal(-3, vec3.X);
        Assert.Equal(-3, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator - (int)
    [Fact]
    public void OperatorMinus_Int()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = 4;
        var vec3 = vec1 - vec2;
        Assert.Equal(-3, vec3.X);
        Assert.Equal(-2, vec3.Y);
        Assert.Equal(-1, vec3.Z);
    }

    // test operator - (float)
    [Fact]
    public void OperatorMinus_Float()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = vec1 - 4.5f;
        Assert.Equal(-3.5f, vec2.X);
        Assert.Equal(-2.5f, vec2.Y);
        Assert.Equal(-1.5f, vec2.Z);
    }

    // test operator *
    [Fact]
    public void OperatorMultiply_Vec3Vec3()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(18, vec3.Z);
    }

    // test operator * (Int3)
    [Fact]
    public void OperatorMultiply_Vec3Int3()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Int3(4, 5, 6);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(18, vec3.Z);
    }

    // test operator * (Vec2)
    [Fact]
    public void OperatorMultiply_Vec3Vec2()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Vec2(4, 5);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator * (Int2)
    [Fact]
    public void OperatorMultiply_Vec3Int2()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = new Int2(4, 5);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(3, vec3.Z);
    }

    // test operator * (int)
    [Fact]
    public void OperatorMultiply_Vec3Int()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = 4;
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(8, vec3.Y);
        Assert.Equal(12, vec3.Z);
    }

    // test operator * (float)
    [Fact]
    public void OperatorMultiply_Vec3Float()
    {
        var vec1 = new Vec3(1, 2, 3);
        var vec2 = vec1 * 4.5f;
        Assert.Equal(4.5f, vec2.X);
        Assert.Equal(9, vec2.Y);
        Assert.Equal(13.5f, vec2.Z);
    }

    // test operator * (Int3, Vec3)
    [Fact]
    public void OperatorMultiply_Int3Vec3()
    {
        var vec1 = new Int3(1, 2, 3);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(18, vec3.Z);
    }

    // test operator * (Vec2, Vec3)
    [Fact]
    public void OperatorMultiply_Vec2Vec3()
    {
        var vec1 = new Vec2(1, 2);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(6, vec3.Z);
    }

    // test operator * (Int2, Vec3)
    [Fact]
    public void OperatorMultiply_Int2Vec3()
    {
        var vec1 = new Int2(1, 2);
        var vec2 = new Vec3(4, 5, 6);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(10, vec3.Y);
        Assert.Equal(6, vec3.Z);
    }

    // test operator * (int, Vec3)
    [Fact]
    public void OperatorMultiply_IntVec3()
    {
        var vec1 = 4;
        var vec2 = new Vec3(1, 2, 3);
        var vec3 = vec1 * vec2;
        Assert.Equal(4, vec3.X);
        Assert.Equal(8, vec3.Y);
        Assert.Equal(12, vec3.Z);
    }

    // test operator * (float, Vec3)
    [Fact]
    public void OperatorMultiply_FloatVec3()
    {
        var vec1 = 4.5f;
        var vec2 = new Vec3(1, 2, 3);
        var vec3 = vec1 * vec2;
        Assert.Equal(4.5f, vec3.X);
        Assert.Equal(9, vec3.Y);
        Assert.Equal(13.5f, vec3.Z);
    }

    // test ToString
    [Fact]
    public void ToStringTest()
    {
        var vec = new Vec3(1, 2, 3);
        Assert.Equal("<1, 2, 3>", vec.ToString());
    }
}
