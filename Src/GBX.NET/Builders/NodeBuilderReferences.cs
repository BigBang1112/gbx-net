namespace GBX.NET.Builders;

internal static class NodeBuilderReferences
{
    public static NodeRefList<T> CopyList<T>(NodeRefList<T> source) where T : CMwNod?
    {
        var copy = new NodeRefList<T>(source.Count);
        foreach (var reference in source.EnumerateReferences()) copy.Add(reference);
        return copy;
    }
}
