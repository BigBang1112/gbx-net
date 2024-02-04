using ChunkL.Structure;
using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record ArchiveDataModel(
    ArchiveDefinition? ChunkLDefinition,
    INamedTypeSymbol? TypeSymbol);