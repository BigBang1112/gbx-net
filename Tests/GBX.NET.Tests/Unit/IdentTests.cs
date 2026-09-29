
namespace GBX.NET.Tests.Unit;

public class IdentTests
{
    [Test]
    public async Task Constructor_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident();

        // Assert
        await Assert.That(i.Id).IsEqualTo(string.Empty);
        await Assert.That(i.Collection).IsEqualTo(Id.Empty);
        await Assert.That(i.Author).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Contructor_String_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident("Test");

        // Assert
        await Assert.That(i.Id).IsEqualTo("Test");
        await Assert.That(i.Collection).IsEqualTo(Id.Empty);
        await Assert.That(i.Author).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Constructor_StringIdString_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident("Test", 12, "Hell");

        // Assert
        await Assert.That(i.Id).IsEqualTo("Test");
        await Assert.That(i.Collection).IsEqualTo(new(12));
        await Assert.That(i.Author).IsEqualTo("Hell");
    }

    [Test]
    public async Task ToString_ReturnsCorrect()
    {
        // Arrange
        var i = new Ident("Test", 12, "Hell");

        // Act
        var actual = i.ToString();

        // Assert
        await Assert.That(actual).IsEqualTo("(\"Test\", \"Canyon\", \"Hell\")");
    }

    [Test]
    public async Task Empty_IsCorrect()
    {
        // Arrange & Act
        var i = Ident.Empty;

        // Assert
        await Assert.That(i.Id).IsEqualTo(string.Empty);
        await Assert.That(i.Collection).IsEqualTo(Id.Empty);
        await Assert.That(i.Author).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task ImplicitConversionFromTuple_IsCorrect()
    {
        // Arrange
        var t = ("Test", new Id(12), "Hell");

        // Act
        Ident i = t;

        // Assert
        await Assert.That(i.Id).IsEqualTo("Test");
        await Assert.That(i.Collection).IsEqualTo(new Id(12));
        await Assert.That(i.Author).IsEqualTo("Hell");
    }
}
