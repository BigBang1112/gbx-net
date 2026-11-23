using GBX.NET;
using System.Collections.Immutable;

namespace GbxCLI.Tests;

public class MemberPathTests
{
    [Test]
    public async Task Assign_StructMember_AssignsCopyBack()
    {
        var holder = new Holder { Position = new Vec3(1, 2, 3) };

        var change = MemberPath.Parse("Position.Y").Assign(holder, "5");

        await Assert.That(holder.Position).IsEqualTo(new Vec3(1, 5, 3));
        await Assert.That(change).IsEqualTo(new MemberChange("Position.Y", 2f, 5f));
    }

    [Test]
    public async Task Assign_RecordMember_KeepsSharedRecord()
    {
        var shared = new Ident("Day", new Id("Stadium"), "Nadeo");
        var holder = new Holder { Decoration = shared };

        MemberPath.Parse("Decoration.Author").Assign(holder, "Me");

        await Assert.That(holder.Decoration).IsEqualTo(new Ident("Day", new Id("Stadium"), "Me"));
        await Assert.That(shared.Author).IsEqualTo("Nadeo");
    }

    [Test]
    public async Task Assign_ListAndArrayItems_AreSet()
    {
        var holder = new Holder();

        MemberPath.Parse("Numbers[1]").Assign(holder, "20");
        MemberPath.Parse("Positions[0].Z").Assign(holder, "9");

        await Assert.That(holder.Numbers).IsEquivalentTo([1, 20]);
        await Assert.That(holder.Positions[0]).IsEqualTo(new Vec3(0, 0, 9));
    }

    [Test]
    public async Task Assign_NameWithOtherCase_UsesMemberName()
    {
        var holder = new Holder();

        var change = MemberPath.Parse("position.x").Assign(holder, "4");

        await Assert.That(change.Path).IsEqualTo("Position.X");
        await Assert.That(holder.Position.X).IsEqualTo(4f);
    }

    [Test]
    [Arguments("Computed", "Computed is read-only.")]
    [Arguments("Immutable[0]", "Immutable[0] is read-only.")]
    [Arguments("Nothing.Length", "Nothing is null, so it has no members.")]
    [Arguments("Position[0]", "Position is not a list, so it has no item [0].")]
    public async Task Assign_InvalidTarget_Throws(string path, string message)
    {
        await Assert.That(() => MemberPath.Parse(path).Assign(new Holder(), "1"))
            .Throws<InvalidOperationException>().WithMessage(message);
    }

    [Test]
    [Arguments("")]
    [Arguments(".Name")]
    [Arguments("Name..Other")]
    [Arguments("Name[x]")]
    [Arguments("Name[1")]
    [Arguments("Name-Other")]
    public async Task Parse_InvalidPath_Throws(string path)
    {
        await Assert.That(() => MemberPath.Parse(path)).Throws<FormatException>();
    }

    public sealed class Holder
    {
        public Vec3 Position { get; set; }

        public Ident Decoration { get; set; } = Ident.Empty;

        public List<int> Numbers { get; set; } = [1, 2];

        public Vec3[] Positions { get; set; } = [default];

        public ImmutableArray<int> Immutable { get; set; } = [1];

        public string? Nothing { get; set; }

        public int Computed => Numbers.Count;
    }
}
