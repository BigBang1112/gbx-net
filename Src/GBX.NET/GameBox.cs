using GBX.NET.Components;

namespace GBX.NET;

/// <summary>
/// Old name for <see cref="Gbx"/>.
/// </summary>
public class GameBox(GbxHeader header) : Gbx(header) { }

/// <summary>
/// Old name for <see cref="Gbx{T}"/>.
/// </summary>
public class GameBox<T>(GbxHeader<T> header, T node) : Gbx<T>(header, node) where T : notnull, IClass;