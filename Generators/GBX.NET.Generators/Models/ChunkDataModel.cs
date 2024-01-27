using ChunkL.Structure;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record ChunkDataModel(
    uint? Id,
    ChunkDefinition? ChunkLDefinition,
    INamedTypeSymbol? TypeSymbol);