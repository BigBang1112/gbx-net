using ChunkL.Structure;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record ChunkDataModel(
    uint Id,
    string? Description,
    bool IsSkippable,
    ChunkDefinition? ChunkLDefinition,
    INamedTypeSymbol? TypeSymbol);