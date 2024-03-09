namespace GBX.NET.Tests.Unit;

public class Int3Tests
{
    [Fact]
    public void Contructor_XYZIsCorrect()
    {
        // Arrange & Act
        var i = new Int3(1, 2, 3);

        // Assert
        Assert.Equal(expected: 1, actual: i.X);
        Assert.Equal(expected: 2, actual: i.Y);
        Assert.Equal(expected: 3, actual: i.Z);
    }

    [Fact]
    public void Initializer_XYZIsCorrect()
    {
        // Arrange & Act
        var i = new Int3 { X = 1, Y = 2, Z = 3 };

        // Assert
        Assert.Equal(expected: 1, actual: i.X);
        Assert.Equal(expected: 2, actual: i.Y);
        Assert.Equal(expected: 3, actual: i.Z);
    }

    [Fact]
    public void Zero_IsCorrect()
    {
        // Arrange & Act
        var i = Int3.Zero;

        // Assert
        Assert.Equal(expected: 0, actual: i.X);
        Assert.Equal(expected: 0, actual: i.Y);
        Assert.Equal(expected: 0, actual: i.Z);
    }

    [Fact]
    public void ToString_ReturnsCorrect()
    {
        // Arrange
        var i = new Int3(1, 2, 3);

        // Act
        var s = i.ToString();

        // Assert
        Assert.Equal(expected: "<1, 2, 3>", actual: s);
    }

    [Fact]
    public void ImplicitConversionFromTuple_IsCorrect()
    {
        // Arrange
        var t = (1, 2, 3);

        // Act
        Int3 i = t;

        // Assert
        Assert.Equal(expected: 1, actual: i.X);
        Assert.Equal(expected: 2, actual: i.Y);
        Assert.Equal(expected: 3, actual: i.Z);
    }

    [Fact]
    public void Equals_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        Assert.True(i1.Equals(i2));
        Assert.False(i1.Equals(i3));
    }

    [Fact]
    public void EqualsObject_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        Assert.True(i1.Equals((object)i2));
        Assert.False(i1.Equals((object)i3));
    }

    [Fact]
    public void GetHashCode_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        Assert.Equal(expected: i1.GetHashCode(), actual: i2.GetHashCode());
        Assert.NotEqual(expected: i1.GetHashCode(), actual: i3.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        Assert.True(i1 == i2);
        Assert.False(i1 == i3);
    }
}
