namespace GBX.NET.Serialization;

internal readonly record struct NodeRefResolutionSnapshot(
    int Generation,
    NodeRefState State,
    CMwNod? Node,
    Exception? Exception);
