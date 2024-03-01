namespace GBX.NET.Tests.Unit;

public class PackDescTests
{
    [Fact]
    public void GetLocatorUri_WhenLocatorUrlSpecified_ReturnsUri()
    {
        // Arrange
        var packDesc = new PackDesc("test.txt", default, "http://locator.url");

        // Act
        var result = packDesc.GetLocatorUri();

        // Assert
        Assert.Equal(expected: new Uri("http://locator.url"), actual: result);
    }

    [Fact]
    public void GetLocatorUri_LocatorUrlEmpty_ReturnsNull()
    {
        // Arrange
        var packDesc = new PackDesc("test.txt");

        // Act
        var result = packDesc.GetLocatorUri();

        // Assert
        Assert.Null(result);
    }
}
