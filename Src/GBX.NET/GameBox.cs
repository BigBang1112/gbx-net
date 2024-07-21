using GBX.NET.Components;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET;

/// <summary>
/// Old name for <see cref="Gbx"/>.
/// </summary>
public class GameBox(GbxHeader header, GbxBody body) : Gbx(header, body);

/// <summary>
/// Old name for <see cref="Gbx{T}"/>.
/// </summary>
public class GameBox<
#if NET6_0_OR_GREATER
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
	T>(GbxHeader<T> header, GbxBody body, T node) : Gbx<T>(header, body, node) where T : notnull, CMwNod;