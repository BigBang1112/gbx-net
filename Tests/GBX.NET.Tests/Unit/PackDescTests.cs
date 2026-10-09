
namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class PackDescTests
{
    [Test]
    public async Task GetLocatorUri_WhenLocatorUrlSpecified_ReturnsUri()
    {
        // Arrange
        var packDesc = new PackDesc("test.txt", default, "http://locator.url");

        // Act
        var result = packDesc.GetLocatorUri();

        // Assert
        await Assert.That(result).IsEqualTo(new Uri("http://locator.url"));
    }

    [Test]
    public async Task GetLocatorUri_LocatorUrlEmpty_ReturnsNull()
    {
        // Arrange
        var packDesc = new PackDesc("test.txt");

        // Act
        var result = packDesc.GetLocatorUri();

        // Assert
        await Assert.That(result).IsNull();
    }
}
