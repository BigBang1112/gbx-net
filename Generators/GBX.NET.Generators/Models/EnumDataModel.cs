using ChunkL.Structure;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record EnumDataModel(
    EnumDefinition ChunkLDefinition,
    INamedTypeSymbol? TypeSymbol);
