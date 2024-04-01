namespace GBX.NET.Components;

public sealed record ImmutableGbxRefTableResource(int Flags, bool UseFile, int ResourceIndex) : ImmutableGbxRefTableNode(Flags, UseFile);
