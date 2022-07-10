namespace GBX.NET;

public record ExternalNode<T>(T? Node, GameBoxRefTable.File? File) where T : Node;
