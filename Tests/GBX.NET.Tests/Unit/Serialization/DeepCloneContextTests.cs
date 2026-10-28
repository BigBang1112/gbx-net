using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Collections.Immutable;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class DeepCloneContextTests
{
    [Test]
    public async Task Clone_ImmutableList_PreservesNullAndEmptyLists()
    {
        var context = new DeepCloneContext();

        await Assert.That(context.Clone((ImmutableList<int>?)null)).IsNull();
        await Assert.That(context.Clone(ImmutableList<int>.Empty)!).IsEmpty();
    }

    [Test]
    public async Task Clone_ImmutableList_CopiesMutableElementsAndPreservesAliases()
    {
        var info = new CGameCtnReplayRecord.InterfaceScriptInfo { Manialinks = ["<manialink />"] };
        var source = ImmutableList.Create(info, info);
        var context = new DeepCloneContext();
        var clone = context.Clone(source)!;
        clone[0].Manialinks![0] = "Changed";

        await Assert.That(info.Manialinks![0]).IsEqualTo("<manialink />");
        await Assert.That(clone[0]).IsSameReferenceAs(clone[1]);
        await Assert.That(context.Clone(source)).IsSameReferenceAs(clone);
    }

    [Test]
    public async Task Clone_ImmutableList_PreservesElementBackReferences()
    {
        var item = new ListItem();
        var source = ImmutableList.Create(item);
        item.Items = source;

        var clone = new DeepCloneContext().Clone(source)!;

        await Assert.That(clone[0].Items).IsSameReferenceAs(clone);
        await Assert.That(ReferenceEquals(clone[0], item)).IsFalse();
    }

    private sealed class ListItem : IDeepCloneable
    {
        public ImmutableList<ListItem>? Items { get; set; }

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = (ListItem)MemberwiseClone();
            context.Register(this, clone);
            clone.Items = context.Clone(Items);
            return clone;
        }
    }
}
