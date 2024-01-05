namespace GBX.NET.Tests.Unit;

public class IdentTests
{
    [Fact]
    public void Constructor_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident();

        // Assert
        Assert.Equal(expected: string.Empty, actual: i.Id);
        Assert.Equal(expected: Id.Empty, actual: i.Collection);
        Assert.Equal(expected: string.Empty, actual: i.Author);
    }

    [Fact]
    public void Contructor_String_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident("Test");

        // Assert
        Assert.Equal(expected: "Test", actual: i.Id);
        Assert.Equal(expected: Id.Empty, actual: i.Collection);
        Assert.Equal(expected: string.Empty, actual: i.Author);
    }

    [Fact]
    public void Constructor_StringIdString_IsCorrect()
    {
        // Arrange & Act
        var i = new Ident("Test", new(12), "Hell");

        // Assert
        Assert.Equal(expected: "Test", actual: i.Id);
        Assert.Equal(expected: new(12), actual: i.Collection);
        Assert.Equal(expected: "Hell", actual: i.Author);
    }

    [Fact]
    public void ToString_ReturnsCorrect()
    {
        // Arrange
        var i = new Ident("Test", new(12), "Hell");

        // Act
        var actual = i.ToString();

        // Assert
        Assert.Equal(expected: "(\"Test\", \"Canyon\", \"Hell\")", actual);
    }

    [Fact]
    public void Empty_IsCorrect()
    {
        // Arrange & Act
        var i = Ident.Empty;

        // Assert
        Assert.Equal(expected: string.Empty, actual: i.Id);
        Assert.Equal(expected: Id.Empty, actual: i.Collection);
        Assert.Equal(expected: string.Empty, actual: i.Author);
    }

    [Fact]
    public void ImplicitConversionFromTuple_IsCorrect()
    {
        // Arrange
        var t = ("Test", new Id(12), "Hell");

        // Act
        Ident i = t;

        // Assert
        Assert.Equal(expected: "Test", actual: i.Id);
        Assert.Equal(expected: new Id(12), actual: i.Collection);
        Assert.Equal(expected: "Hell", actual: i.Author);
    }

    [Fact]
    public void ImplicitConversionToTuple_IsCorrect()
    {
        // Arrange
        var i = new Ident("Test", new Id(12), "Hell");

        // Act
        (string Id, Id Collection, string Author) t = i;

        // Assert
        Assert.Equal(expected: "Test", actual: t.Id);
        Assert.Equal(expected: new Id(12), actual: t.Collection);
        Assert.Equal(expected: "Hell", actual: t.Author);
    }
}
