
namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class IdTests
{
    [Test]
    public async Task Constructor_String_IsCorrect()
    {
        // Arrange & Act
        var i = new Id("Test");

        // Assert
        await Assert.That(i.String).IsEqualTo("Test");
    }

    [Test]
    public async Task Constructor_Number_IsCorrect()
    {
        // Arrange & Act
        var i = new Id(12);

        // Assert
        await Assert.That(i.Number).IsEqualTo(12);
    }

    [Test]
    public async Task Empty_IsCorrect()
    {
        // Arrange & Act
        var i = Id.Empty;

        // Assert
        await Assert.That(i).IsEqualTo(new Id());
    }

    [Test]
    [Arguments("Speed", 32, 16, 32)]
    [Arguments("Desert", 32, 16, 32)]
    [Arguments("Alpine", 32, 16, 32)]
    [Arguments("Snow", 32, 16, 32)]
    [Arguments("Rally", 32, 8, 32)]
    [Arguments("Island", 64, 8, 64)]
    [Arguments("Bay", 32, 8, 32)]
    [Arguments("Coast", 16, 4, 16)]
    [Arguments("Stadium", 32, 8, 32)]
    [Arguments("Canyon", 64, 16, 64)]
    [Arguments("Valley", 32, 8, 32)]
    [Arguments("Lagoon", 32, 8, 32)]
    [Arguments("Stadium2020", 32, 8, 32)]
    public async Task GetBlockSize_ByString_ReturnsCorrect(string collection, int x, int y, int z)
    {
        // Arrange
        var i = new Id(collection);

        // Act
        var actual = i.GetBlockSize();

        // Assert
        await Assert.That(actual).IsEqualTo(new(x, y, z));
    }

    [Test]
    public void GetBlockSize_ByString_Invalid_Throws()
    {
        // Arrange
        var i = new Id("nice");

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => i.GetBlockSize());
    }

    [Test]
    [Arguments(0, 32, 16, 32)]
    [Arguments(1, 32, 16, 32)]
    [Arguments(2, 32, 8, 32)]
    [Arguments(3, 64, 8, 64)]
    [Arguments(4, 32, 8, 32)]
    [Arguments(5, 16, 4, 16)]
    [Arguments(6, 32, 8, 32)]
    [Arguments(12, 64, 16, 64)]
    [Arguments(11, 32, 8, 32)]
    [Arguments(13, 32, 8, 32)]
    [Arguments(26, 32, 8, 32)]
    public async Task GetBlockSize_ByIndex_ReturnsCorrect(int collection, int x, int y, int z)
    {
        // Arrange
        var i = new Id(collection);

        // Act
        var actual = i.GetBlockSize();

        // Assert
        await Assert.That(actual).IsEqualTo(new(x, y, z));
    }

    [Test]
    public void GetBlockSize_ByIndex_Invalid_Throws()
    {
        // Arrange
        var i = new Id(69);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => i.GetBlockSize());
    }

    [Test]
    public async Task ImplicitConversionToString_IsCorrect()
    {
        // Arrange
        var i = new Id("Test");

        // Act
        string actual = i;

        // Assert
        await Assert.That(actual).IsEqualTo("Test");
    }

    [Test]
    public async Task ToString_StringId_ReturnsCorrect()
    {
        // Arrange
        var i = new Id("Test");

        // Act
        var actual = i.ToString();

        // Assert
        await Assert.That(actual).IsEqualTo("Test");
    }

    [Test]
    public async Task ToString_IndexId_ReturnsCorrect()
    {
        // Arrange
        var i = new Id(26);

        // Act
        var actual = i.ToString();

        // Assert
        await Assert.That(actual).IsEqualTo("Stadium2020");
    }

    [Test]
    public async Task ToString_IndexId_UnknownCollection_ReturnsCorrect()
    {
        // Arrange
        var i = new Id(69);

        // Act
        var actual = i.ToString();

        // Assert
        await Assert.That(actual).IsEqualTo("69");
    }

    [Test]
    public async Task ImplicitConversionFromInt_IsCorrect()
    {
        // Arrange
        var i = 26;

        // Act
        Id actual = (Id)i;

        // Assert
        await Assert.That(actual.Number).IsEqualTo(i);
    }
}
