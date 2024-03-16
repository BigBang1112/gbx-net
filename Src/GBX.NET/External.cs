using GBX.NET.Components;

namespace GBX.NET;

public sealed record External<T>(T? Node, GbxRefTableFile? File) where T : IClass;
