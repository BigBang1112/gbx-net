using GBX.NET;
using TmEssentials;

namespace GbxCLI.Tests;

public class ValueParserTests
{
    [Test]
    public async Task Parse_Vector_TakesPartsSeparatedByCommas()
    {
        await Assert.That(ValueParser.Parse("1, 2.5,-3", typeof(Vec3))).IsEqualTo(new Vec3(1, 2.5f, -3));
    }

    [Test]
    public async Task Parse_Enum_IgnoresCase()
    {
        await Assert.That(ValueParser.Parse("west", typeof(Direction))).IsEqualTo(Direction.West);
    }

    [Test]
    public async Task Parse_UndefinedEnumValue_Throws()
    {
        await Assert.That(() => ValueParser.Parse("7", typeof(Direction)))
            .Throws<FormatException>().WithMessage("'7' is not a valid Direction. Use one of: North, East, South, West.");
    }

    [Test]
    public async Task Parse_HexadecimalInteger_IsAccepted()
    {
        await Assert.That(ValueParser.Parse("0x03043000", typeof(uint))).IsEqualTo(0x03043000u);
    }

    [Test]
    public async Task Parse_Time_UsesParseMethod()
    {
        await Assert.That(ValueParser.Parse("1:23.456", typeof(TimeInt32))).IsEqualTo(new TimeInt32(83456));
    }

    [Test]
    public async Task Parse_Id_NumberIsCollectionAndTextIsString()
    {
        await Assert.That(ValueParser.Parse("26", typeof(Id))).IsEqualTo(new Id(26));
        await Assert.That(ValueParser.Parse("Stadium", typeof(Id))).IsEqualTo(new Id("Stadium"));
    }

    [Test]
    public async Task Parse_Ident_ConvertsEachPart()
    {
        await Assert.That(ValueParser.Parse("Night,Stadium,Nadeo", typeof(Ident))).IsEqualTo(new Ident("Night", new Id("Stadium"), "Nadeo"));
    }

    [Test]
    public async Task Parse_Null_ClearsNullableButNotString()
    {
        await Assert.That(ValueParser.Parse("null", typeof(TimeInt32?))).IsNull();
        await Assert.That(ValueParser.Parse("null", typeof(string))).IsEqualTo("null");
    }

    [Test]
    public async Task Parse_WrongPartCount_ShowsExpectedParts()
    {
        await Assert.That(() => ValueParser.Parse("1,2", typeof(Int3)))
            .Throws<FormatException>().WithMessage("'1,2' is not a valid Int3. Use X,Y,Z.");
    }

    [Test]
    public async Task Parse_Collection_IsNotSupported()
    {
        await Assert.That(() => ValueParser.Parse("1", typeof(List<int>))).Throws<NotSupportedException>();
    }
}
