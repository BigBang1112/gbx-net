using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class GbxReaderTests
{
    [Test]
    public async Task Constructor_InputStream_AssignsBaseStream()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var r = new GbxReader(ms);

        // Assert
        await Assert.That(r.BaseStream).IsSameReferenceAs(ms);
    }

    [Test]
    public async Task Constructor_InputStreamWithLeaveOpen_AssignsBaseStream()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var r = new GbxReader(ms);

        // Assert
        await Assert.That(r.BaseStream).IsSameReferenceAs(ms);
    }

    [Test]
    public async Task ReadGbxMagic_CorrectMagic_ReturnsTrue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'B', (byte)'X', 69]);
        ms.Position = 0;

        // Act
        var result = r.ReadGbxMagic();

        // Assert
        await Assert.That(result).IsTrue().Because("GBX magic is invalid.");
        await Assert.That(ms.Position).IsEqualTo(3);
    }

    [Test]
    public async Task ReadGbxMagic_IncorrectMagic_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'X', (byte)'Z', 69]);
        ms.Position = 0;

        // Act
        var result = r.ReadGbxMagic();

        // Assert
        await Assert.That(result).IsFalse().Because("GBX magic is valid but it shouldn't be.");
        await Assert.That(ms.Position).IsEqualTo(3);
    }

    [Test]
    public void ReadGbxMagic_MissingData_ThrowsEndOfStream()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'B']);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<EndOfStreamException>(() => r.ReadGbxMagic());
    }

    [Test]
    public async Task ReadBoolean_Int32One_ReturnsTrue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([1, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean();

        // Assert
        await Assert.That(value).IsTrue();
        await Assert.That(ms.Position).IsEqualTo(4);
    }

    [Test]
    public async Task ReadBoolean_Int32Zero_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean();

        // Assert
        await Assert.That(value).IsFalse();
        await Assert.That(ms.Position).IsEqualTo(4);
    }

    [Test]
    [NotInParallel]
    public void ReadBoolean_NoncanonicalInt32_ThrowsBooleanOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([2, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act & Assert
        var strictBooleans = Gbx.StrictBooleans;
        try
        {
            Gbx.StrictBooleans = true;
            Assert.Throws<BooleanOutOfRangeException>(() => r.ReadBoolean());
        }
        finally
        {
            Gbx.StrictBooleans = strictBooleans;
        }
    }

    [Test]
    public async Task ReadBoolean_Int32WithAsByteFalse_ReturnsExpectedValue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: false);

        // Assert
        await Assert.That(value).IsFalse();
        await Assert.That(ms.Position).IsEqualTo(4);
    }

    [Test]
    public async Task ReadBoolean_ByteOne_ReturnsTrue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(1);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: true);

        // Assert
        await Assert.That(value).IsTrue();
        await Assert.That(ms.Position).IsEqualTo(1);
    }

    [Test]
    public async Task ReadBoolean_ByteZero_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(0);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: true);

        // Assert
        await Assert.That(value).IsFalse();
        await Assert.That(ms.Position).IsEqualTo(1);
    }

    [Test]
    [NotInParallel]
    public void ReadBoolean_NoncanonicalByte_ThrowsBooleanOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(2);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act & Assert
        var strictBooleans = Gbx.StrictBooleans;
        try
        {
            Gbx.StrictBooleans = true;
            Assert.Throws<BooleanOutOfRangeException>(() => r.ReadBoolean(asByte: true));
        }
        finally
        {
            Gbx.StrictBooleans = strictBooleans;
        }
    }

    [Test]
    public async Task ReadString_Int32LengthPrefix_ReturnsText()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString();

        // Assert
        await Assert.That(value).IsEqualTo("Test");
        await Assert.That(ms.Position).IsEqualTo(8);
    }

    [Test]
    public async Task ReadString_Int32_Empty()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString();

        // Assert
        await Assert.That(value).IsEqualTo(string.Empty);
        await Assert.That(ms.Position).IsEqualTo(4);
    }

    [Test]
    public void ReadString_NegativeInt32Length_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([255, 255, 255, 255, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => r.ReadString());
    }

    [Test]
    public void ReadString_Int32LengthOverLimit_ThrowsLengthLimitException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write(BitConverter.GetBytes(GbxReader.MaxDataSize + 1));
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadString());
    }

    [Test]
    public async Task ReadString_ByteLengthPrefix_ReturnsText()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString(StringLengthPrefix.Byte);

        // Assert
        await Assert.That(value).IsEqualTo("Test");
        await Assert.That(ms.Position).IsEqualTo(5);
    }

    [Test]
    public async Task ReadString_ExplicitInt32LengthPrefix_ReturnsText()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString(StringLengthPrefix.Int32);

        // Assert
        await Assert.That(value).IsEqualTo("Test");
        await Assert.That(ms.Position).IsEqualTo(8);
    }

    [Test]
    public void ReadString_UnknownLengthPrefix_ThrowsArgumentException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => r.ReadString((StringLengthPrefix)69));
    }

    [Test]
    public void ReadString_NegativePrefixedLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([255, 255, 255, 255, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => r.ReadString(StringLengthPrefix.Int32));
    }

    [Test]
    public void ReadString_PrefixedLengthOverLimit_ThrowsLengthLimitException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write(BitConverter.GetBytes(GbxReader.MaxDataSize + 1));
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadString(StringLengthPrefix.Int32));
    }

    [Test]
    public void ReadData_LengthOverLimit_ThrowsLengthLimitException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write(BitConverter.GetBytes(GbxReader.MaxDataSize + 1));
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadData());
    }

    [Test]
    public void ReadBytes_CountOverLimit_ThrowsLengthLimitExceptionWithLength()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadBytes(GbxReader.MaxDataSize + 1));
    }

    [Test]
    public async Task ReadData_NoCount_ReadsBytes()
    {
        // Arrange
        using var ms = new MemoryStream([3, 0, 0, 0, 1, 2, 3, 69]);
        using var r = new GbxReader(ms);

        // Act
        var bytes = r.ReadData();

        // Assert
        await Assert.That(bytes).IsEquivalentTo((byte[])[1, 2, 3], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadBytes_WithCount_ReadsBytes()
    {
        // Arrange
        using var ms = new MemoryStream([1, 2, 3, 69]);
        using var r = new GbxReader(ms);

        // Act
        var bytes = r.ReadBytes(3);

        // Assert
        await Assert.That(bytes).IsEquivalentTo((byte[])[1, 2, 3], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadId_VersionNotSupported_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([2, 0, 0, 0, 0, 0, 0, 64, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => r.ReadId());
        await Assert.That(ms.Position).IsEqualTo(4);
    }

    [Test]
    public async Task ReadId_HasIdString_ReturnsIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 69]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(id.String).IsEqualTo("Hi!");
        await Assert.That(id.Number).IsNull();
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict).HasSingleItem();
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(ms.Position).IsEqualTo(15);
    }

    [Test]
    public async Task ReadId_HasIdCollection_ReturnsIdWithCorrectNumber()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 4, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(id.Number).IsEqualTo(4);
        await Assert.That(id.String).IsNull();
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(ms.Position).IsEqualTo(8);
    }

    [Test]
    public async Task ReadId_HasReusedIdString_ReturnsSameIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 1, 0, 0, 64, 69]);
        ms.Position = 0;

        var id = r.ReadId();

        // Act
        var reusedId = r.ReadId();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(reusedId).IsEqualTo(id);
        await Assert.That(reusedId.Number).IsNull();
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict).HasSingleItem();
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(ms.Position).IsEqualTo(19);
    }

    [Test]
    public async Task ReadId_Has2IdStrings_ReturnsIdsWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([
            3,
            0,
            0,
            0,
            0,
            0,
            0,
            64,
            3,
            0,
            0,
            0,
            (byte)'H',
            (byte)'i',
            (byte)'!',
            0,
            0,
            0,
            64,
            2,
            0,
            0,
            0,
            (byte)'T',
            (byte)'M',
            69
        ]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();
        var anotherId = r.ReadId();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(id.String).IsEqualTo("Hi!");
        await Assert.That(anotherId.String).IsEqualTo("TM");
        await Assert.That(id.Number).IsNull();
        await Assert.That(anotherId.Number).IsNull();
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict.Count).IsEqualTo(2);
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(r.IdDict.TryGetValue(0x40000002, out var val2) && val2 == "TM").IsTrue().Because(@"IdList does not contain (0x40000002, ""TM"")");
        await Assert.That(ms.Position).IsEqualTo(25);
    }

    [Test]
    public async Task ReadIdAsString_HasIdString_ReturnsIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 69]);
        ms.Position = 0;

        // Act
        var str = r.ReadIdAsString();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(str).IsEqualTo("Hi!");
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict).HasSingleItem();
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(ms.Position).IsEqualTo(15);
    }

    [Test]
    public async Task ReadIdAsString_HasIdCollection_ToStringifiedIndex()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 4, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var str = r.ReadIdAsString();

        // Assert
        await Assert.That(str).IsEqualTo("4");
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(ms.Position).IsEqualTo(8);
    }

    [Test]
    public async Task ReadIdAsString_HasReusedIdString_ReturnsSameIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 1, 0, 0, 64, 69]);
        ms.Position = 0;

        var id = r.ReadIdAsString();

        // Act
        var reusedId = r.ReadIdAsString();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(reusedId).IsEqualTo(id);
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict).HasSingleItem();
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(ms.Position).IsEqualTo(19);
    }

    [Test]
    public async Task ReadIdent_ThreeIds_ReturnsIdentifier()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3,
            0,
            0,
            0,
            0,
            0,
            0,
            64,
            3,
            0,
            0,
            0,
            (byte)'H',
            (byte)'i',
            (byte)'!',
            4,
            0,
            0,
            0,
            1,
            0,
            0,
            64,
            69]);
        ms.Position = 0;

        // Act
        var ident = r.ReadIdent();

        // Assert
        await Assert.That(r.IdVersion).IsEqualTo(3);
        await Assert.That(ident).IsEqualTo(("Hi!", new(4), "Hi!"));
        await Assert.That(r.IdDict).IsNotNull();
        await Assert.That(r.IdDict).HasSingleItem();
        await Assert.That(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!").IsTrue().Because(@"IdList does not contain (0x40000001, ""Hi!"")");
        await Assert.That(ms.Position).IsEqualTo(23);
    }

    [Test]
    public async Task SkipData_WhenBaseStreamCanSeek_ShouldSeekCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([1, 2, 3, 4, 5, 6, 7]);
        using var r = new GbxReader(ms);

        ms.Position = 2;

        // Act
        r.SkipData(5);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(7);
    }

    [Test]
    public async Task SkipData_WhenBaseStreamCannotSeek_ShouldSkipCorrectly()
    {
        // Arrange
        using var stream = new NonSeekableStream([1, 2, 3, 4, 5, 6, 7], position: 2);
        using var r = new GbxReader(stream);

        // Act
        r.SkipData(5);

        // Assert
        await Assert.That(stream.Position).IsEqualTo(7);
    }

    [Test]
    public async Task IdVersion_NewReader_IsNull()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        await Assert.That(r.IdVersion).IsNull();
    }

    [Test]
    public async Task IdVersion_WithoutEncapsulation_PreservesAssignedValue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act
        r.IdVersion = 3;
        var actual = r.IdVersion;

        // Assert
        await Assert.That(actual).IsEqualTo(3);
    }

    [Test]
    public async Task IdVersion_WithEncapsulation_PreservesAssignedValue()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);
        using var encapsulation = new Encapsulation(r);

        // Act
        r.IdVersion = 3;
        var actual = r.Encapsulation?.IdVersion;

        // Assert
        await Assert.That(actual).IsEqualTo(3);
    }

    [Test]
    public async Task IdDict_NewReader_IsNotNull()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        await Assert.That(r.IdDict).IsNotNull();
    }

    [Test]
    public async Task ReadPackDesc_Version3WithoutLocatorUrl_ReturnsChecksumAndFilePath()
    {
        // Arrange
        var gibbrish = new byte[32];
        Random.Shared.NextBytes(gibbrish);
        var checksum256 = new Checksum256(gibbrish);
        var filePath = "folder\\test.txt";

        using var ms = new MemoryStream(new byte[] { 3 }
            .Concat(gibbrish)
            .Concat(BitConverter.GetBytes(filePath.Length))
            .Concat(Encoding.UTF8.GetBytes(filePath))
            .Concat(BitConverter.GetBytes(0))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsEqualTo(checksum256);
        await Assert.That(result.FilePath).IsEqualTo(filePath);
        await Assert.That(result.LocatorUrl).IsNotNull();
        await Assert.That(result.LocatorUrl).IsEmpty();
        await Assert.That(ms.Position).IsEqualTo(56);
    }

    [Test]
    public async Task ReadPackDesc_Version3WithLocatorUrl_ReturnsChecksumFilePathAndLocatorUrl()
    {
        // Arrange
        var gibbrish = new byte[32];
        Random.Shared.NextBytes(gibbrish);
        var checksum256 = new Checksum256(gibbrish);
        var filePath = "folder\\test.txt";
        var locatorUrl = "https://google.com";

        using var ms = new MemoryStream(new byte[] { 3 }
            .Concat(gibbrish)
            .Concat(BitConverter.GetBytes(filePath.Length))
            .Concat(Encoding.UTF8.GetBytes(filePath))
            .Concat(BitConverter.GetBytes(locatorUrl.Length))
            .Concat(Encoding.UTF8.GetBytes(locatorUrl))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsEqualTo(checksum256);
        await Assert.That(result.FilePath).IsEqualTo(filePath);
        await Assert.That(result.LocatorUrl).IsEqualTo(locatorUrl);
        await Assert.That(ms.Position).IsEqualTo(74);
    }

    [Test]
    public async Task ReadPackDesc_Version3WithoutFilePath_ReturnsChecksumAndLocatorUrl()
    {
        // Arrange
        var gibbrish = new byte[32];
        Random.Shared.NextBytes(gibbrish);
        var checksum256 = new Checksum256(gibbrish);
        var locatorUrl = "https://google.com";

        using var ms = new MemoryStream(new byte[] { 3 }
            .Concat(gibbrish)
            .Concat(BitConverter.GetBytes(0))
            .Concat(BitConverter.GetBytes(locatorUrl.Length))
            .Concat(Encoding.UTF8.GetBytes(locatorUrl))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsEqualTo(checksum256);
        await Assert.That(result.FilePath).IsEmpty();
        await Assert.That(result.LocatorUrl).IsEqualTo(locatorUrl);
        await Assert.That(ms.Position).IsEqualTo(59);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadPackDesc_Version1And2WithoutLocatorUrl_ReturnsFilePath(byte version)
    {
        // Arrange
        var filePath = "folder\\test.txt";

        using var ms = new MemoryStream(new byte[] { version }
            .Concat(BitConverter.GetBytes(filePath.Length))
            .Concat(Encoding.UTF8.GetBytes(filePath))
            .Concat(BitConverter.GetBytes(0))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsNull();
        await Assert.That(result.FilePath).IsEqualTo(filePath);
        await Assert.That(result.LocatorUrl).IsNotNull();
        await Assert.That(result.LocatorUrl).IsEmpty();
        await Assert.That(ms.Position).IsEqualTo(24);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadPackDesc_Version1And2WithLocatorUrl_ReturnsFilePathAndLocatorUrl(byte version)
    {
        // Arrange
        var filePath = "folder\\test.txt";
        var locatorUrl = "https://google.com";

        using var ms = new MemoryStream(new byte[] { version }
            .Concat(BitConverter.GetBytes(filePath.Length))
            .Concat(Encoding.UTF8.GetBytes(filePath))
            .Concat(BitConverter.GetBytes(locatorUrl.Length))
            .Concat(Encoding.UTF8.GetBytes(locatorUrl))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsNull();
        await Assert.That(result.FilePath).IsEqualTo(filePath);
        await Assert.That(result.LocatorUrl).IsEqualTo(locatorUrl);
        await Assert.That(ms.Position).IsEqualTo(42);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadPackDesc_Version1And2WithoutFilePath_ReturnsEmptyPaths(byte version)
    {
        // Arrange
        using var ms = new MemoryStream(new byte[] { version }
            .Concat(BitConverter.GetBytes(0))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsNull();
        await Assert.That(result.FilePath).IsNotNull();
        await Assert.That(result.FilePath).IsEmpty();
        await Assert.That(result.LocatorUrl).IsNotNull();
        await Assert.That(result.LocatorUrl).IsEmpty();
        await Assert.That(ms.Position).IsEqualTo(5);
    }

    [Test]
    public async Task ReadPackDesc_Version0_ReturnsFilePathWithoutChecksumOrLocatorUrl()
    {
        // Arrange
        var filePath = "folder\\test.txt";

        using var ms = new MemoryStream(new byte[] { 0 }
            .Concat(BitConverter.GetBytes(filePath.Length))
            .Concat(Encoding.UTF8.GetBytes(filePath))
            .Append((byte)69)
            .ToArray());
        using var r = new GbxReader(ms);

        // Act
        var result = r.ReadPackDesc();

        // Assert
        await Assert.That(result.Checksum).IsNull();
        await Assert.That(result.FilePath).IsEqualTo(filePath);
        await Assert.That(result.LocatorUrl).IsNotNull();
        await Assert.That(result.LocatorUrl).IsEmpty();
        await Assert.That(ms.Position).IsEqualTo(20);
    }
}
