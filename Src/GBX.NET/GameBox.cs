using GBX.NET.Components;

namespace GBX.NET;

/// <summary>
/// Old name for <see cref="Gbx"/>.
/// </summary>
public class GameBox(GbxHeader header, GbxBody body) : Gbx(header, body);

/// <summary>
/// Old name for <see cref="Gbx{T}"/>.
/// </summary>
public class GameBox<T>(GbxHeader<T> header, GbxBody body, T node) : Gbx<T>(header, body, node) where T : notnull, IClass;