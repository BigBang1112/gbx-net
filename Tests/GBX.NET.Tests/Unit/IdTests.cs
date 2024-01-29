namespace GBX.NET.Tests.Unit;

public class IdTests
{
    [Fact]
    public void Constructor_String_IsCorrect()
    {
        // Arrange & Act
        var i = new Id("Test");

        // Assert
        Assert.Equal(expected: "Test", actual: i.String);
    }

    [Fact]
    public void Constructor_Number_IsCorrect()
    {
        // Arrange & Act
        var i = new Id(12);

        // Assert
        Assert.Equal(expected: 12, actual: i.Number);
    }

    [Fact]
    public void Empty_IsCorrect()
    {
        // Arrange & Act
        var i = Id.Empty;

        // Assert
        Assert.Equal(expected: new Id(), actual: i);
    }

    [Theory]
    [InlineData("Speed", 32, 16, 32)]
    [InlineData("Desert", 32, 16, 32)]
    [InlineData("Alpine", 32, 16, 32)]
    [InlineData("Snow", 32, 16, 32)]
    [InlineData("Rally", 32, 8, 32)]
    [InlineData("Island", 64, 8, 64)]
    [InlineData("Bay", 32, 8, 32)]
    [InlineData("Coast", 16, 4, 16)]
    [InlineData("Stadium", 32, 8, 32)]
    [InlineData("Canyon", 64, 16, 64)]
    [InlineData("Valley", 32, 8, 32)]
    [InlineData("Lagoon", 32, 8, 32)]
    [InlineData("Stadium2020", 32, 8, 32)]
    public void GetBlockSize_ByString_ReturnsCorrect(string collection, int x, int y, int z)
    {
        // Arrange
        var i = new Id(collection);

        // Act
        var actual = i.GetBlockSize();

        // Assert
        Assert.Equal(expected: new(x, y, z), actual);
    }

    [Fact]
    public void GetBlockSize_ByString_Invalid_Throws()
    {
        // Arrange
        var i = new Id("nice");

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => i.GetBlockSize());
    }

    [Theory]
    [InlineData(0, 32, 16, 32)]
    [InlineData(1, 32, 16, 32)]
    [InlineData(2, 32, 8, 32)]
    [InlineData(3, 64, 8, 64)]
    [InlineData(4, 32, 8, 32)]
    [InlineData(5, 16, 4, 16)]
    [InlineData(6, 32, 8, 32)]
    [InlineData(12, 64, 16, 64)]
    [InlineData(11, 32, 8, 32)]
    [InlineData(13, 32, 8, 32)]
    [InlineData(26, 32, 8, 32)]
    public void GetBlockSize_ByIndex_ReturnsCorrect(int collection, int x, int y, int z)
    {
        // Arrange
        var i = new Id(collection);

        // Act
        var actual = i.GetBlockSize();

        // Assert
        Assert.Equal(expected: new(x, y, z), actual);
    }

    [Fact]
    public void GetBlockSize_ByIndex_Invalid_Throws()
    {
        // Arrange
        var i = new Id(69);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => i.GetBlockSize());
    }

    [Fact]
    public void ImplicitConversionToString_IsCorrect()
    {
        // Arrange
        var i = new Id("Test");

        // Act
        string actual = i;

        // Assert
        Assert.Equal(expected: "Test", actual);
    }

    [Fact]
    public void ToString_StringId_ReturnsCorrect()
    {
        // Arrange
        var i = new Id("Test");

        // Act
        var actual = i.ToString();

        // Assert
        Assert.Equal(expected: "Test", actual);
    }

    [Fact]
    public void ToString_IndexId_ReturnsCorrect()
    {
        // Arrange
        var i = new Id(26);

        // Act
        var actual = i.ToString();

        // Assert
        Assert.Equal(expected: "Stadium2020", actual);
    }

    [Fact]
    public void ToString_IndexId_UnknownCollection_ReturnsCorrect()
    {
        // Arrange
        var i = new Id(69);

        // Act
        var actual = i.ToString();

        // Assert
        Assert.Equal(expected: "69", actual);
    }
}
