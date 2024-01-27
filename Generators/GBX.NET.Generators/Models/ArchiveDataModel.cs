using ChunkL.Structure;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record ArchiveDataModel(
    uint Id,
    ArchiveDefinition? ChunkLDefinition,
    INamedTypeSymbol? TypeSymbol);