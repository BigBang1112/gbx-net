
namespace GBX.NET.Tests.Unit;

public class Int3Tests
{
    [Test]
    public async Task Contructor_XYZIsCorrect()
    {
        // Arrange & Act
        var i = new Int3(1, 2, 3);

        // Assert
        await Assert.That(i.X).IsEqualTo(1);
        await Assert.That(i.Y).IsEqualTo(2);
        await Assert.That(i.Z).IsEqualTo(3);
    }

    [Test]
    public async Task Initializer_XYZIsCorrect()
    {
        // Arrange & Act
        var i = new Int3 { X = 1, Y = 2, Z = 3 };

        // Assert
        await Assert.That(i.X).IsEqualTo(1);
        await Assert.That(i.Y).IsEqualTo(2);
        await Assert.That(i.Z).IsEqualTo(3);
    }

    [Test]
    public async Task Zero_IsCorrect()
    {
        // Arrange & Act
        var i = Int3.Zero;

        // Assert
        await Assert.That(i.X).IsEqualTo(0);
        await Assert.That(i.Y).IsEqualTo(0);
        await Assert.That(i.Z).IsEqualTo(0);
    }

    [Test]
    public async Task ToString_ReturnsCorrect()
    {
        // Arrange
        var i = new Int3(1, 2, 3);

        // Act
        var s = i.ToString();

        // Assert
        await Assert.That(s).IsEqualTo("<1, 2, 3>");
    }

    [Test]
    public async Task ImplicitConversionFromTuple_IsCorrect()
    {
        // Arrange
        var t = (1, 2, 3);

        // Act
        Int3 i = t;

        // Assert
        await Assert.That(i.X).IsEqualTo(1);
        await Assert.That(i.Y).IsEqualTo(2);
        await Assert.That(i.Z).IsEqualTo(3);
    }

    [Test]
    public async Task Equals_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        await Assert.That(i1.Equals(i2)).IsTrue();
        await Assert.That(i1.Equals(i3)).IsFalse();
    }

    [Test]
    public async Task EqualsObject_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        await Assert.That(i1.Equals((object)i2)).IsTrue();
        await Assert.That(i1.Equals((object)i3)).IsFalse();
    }

    [Test]
    public async Task GetHashCode_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        await Assert.That(i2.GetHashCode()).IsEqualTo(i1.GetHashCode());
        await Assert.That(i3.GetHashCode()).IsNotEqualTo(i1.GetHashCode());
    }

    [Test]
    public async Task EqualityOperator_IsCorrect()
    {
        // Arrange
        var i1 = new Int3(1, 2, 3);
        var i2 = new Int3(1, 2, 3);
        var i3 = new Int3(3, 2, 1);

        // Act & Assert
        await Assert.That(i1 == i2).IsTrue();
        await Assert.That(i1 == i3).IsFalse();
    }
}
