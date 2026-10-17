using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NodeRefListNullabilityTests
{
    [Test]
    public async Task NonNullableElements_ExposeNonNullableCollectionContracts()
    {
        var node = new CMwNod();
        NodeRefList<CMwNod>? references = new CMwNod[] { node };
        IList<CMwNod> list = references!;
        IReadOnlyList<CMwNod> readOnly = references!;
        CMwNod element = list[0];
        CMwNod[] array = references!.ToArray();
        var copy = new CMwNod[1];
        references.CopyTo(copy, 0);
        using IEnumerator<CMwNod> enumerator = references.GetEnumerator();

        await Assert.That(enumerator.MoveNext()).IsTrue();
        await Assert.That(enumerator.Current.GetType()).IsEqualTo(typeof(CMwNod));
        await Assert.That(enumerator.MoveNext()).IsFalse();
        foreach (var value in references)
        {
            await Assert.That(value.GetType()).IsEqualTo(typeof(CMwNod));
        }
        await Assert.That(element).IsSameReferenceAs(node);
        await Assert.That(readOnly[0]).IsSameReferenceAs(node);
        await Assert.That(array[0]).IsSameReferenceAs(node);
        await Assert.That(copy[0]).IsSameReferenceAs(node);
        await Assert.That(references.All(value => value.GetType() == typeof(CMwNod))).IsTrue();
        await Assert.That(references.RemoveAll(value => value.GetType() == typeof(CMwNod))).IsEqualTo(1);
    }

    [Test]
    public async Task NullableElements_AcceptNullsThroughCollectionContracts()
    {
        var node = new CMwNod();
        NodeRefList<CMwNod?>? references = new List<CMwNod?> { node, null };
        IList<CMwNod?> list = references!;
        IReadOnlyList<CMwNod?> readOnly = references!;
        list.Add(null);
        list.Insert(0, null);
        CMwNod?[] array = references!.ToArray();
        var copy = new CMwNod?[4];
        references.CopyTo(copy, 0);
        using IEnumerator<CMwNod?> enumerator = references.GetEnumerator();

        await Assert.That(enumerator.MoveNext()).IsTrue();
        await Assert.That(enumerator.Current).IsNull();
        var visited = new List<CMwNod?>();
        foreach (var value in references)
        {
            visited.Add(value);
        }
        await Assert.That(visited).IsEquivalentTo(array, CollectionOrdering.Matching);
        await Assert.That(readOnly[0]).IsNull();
        await Assert.That(array).IsEquivalentTo(new CMwNod?[] { null, node, null, null }, CollectionOrdering.Matching);
        await Assert.That(copy).IsEquivalentTo(array, CollectionOrdering.Matching);
        await Assert.That(list.Contains(null)).IsTrue();
        await Assert.That(list.IndexOf(null)).IsEqualTo(0);
        await Assert.That(list.Remove(null)).IsTrue();
        await Assert.That(references.RemoveAll(value => value is null)).IsEqualTo(2);
        await Assert.That(references[0]).IsSameReferenceAs(node);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Serialize_NullableElements_PreservesNullsAndSharedNodes(int mode)
    {
        var node = new CMwNod();
        var references = new NodeRefList<CMwNod?> { node, null, node };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using (var rw = new GbxReaderWriter(writer))
        {
            if (mode == 0) rw.ListNodeRef(references);
            else if (mode == 1) rw.ListNodeRef_deprec(references);
            else rw.JaggedArrayNodeRef(new[] { references });
        }
        writer.Write(0x12345678);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        IGbxReader typedReader = reader;
        var restored = mode switch
        {
            0 => typedReader.ReadListNodeRef<CMwNod?>(),
            1 => typedReader.ReadListNodeRef_deprec<CMwNod?>(),
            _ => typedReader.ReadJaggedListNodeRef<CMwNod?>()[0]
        };

        await Assert.That(restored.Count).IsEqualTo(3);
        await Assert.That(restored[0]).IsNotNull();
        await Assert.That(restored[1]).IsNull();
        await Assert.That(restored[2]).IsSameReferenceAs(restored[0]);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }
}
