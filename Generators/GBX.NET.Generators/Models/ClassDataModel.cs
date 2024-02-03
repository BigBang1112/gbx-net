using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Models;

internal record ClassDataModel(
    string Name,
    uint? Id,
    string Engine,
    string? Inherits,
    string? Description,
    INamedTypeSymbol? TypeSymbol,
    Dictionary<uint, ChunkDataModel> HeaderChunks,
    Dictionary<uint, ChunkDataModel> Chunks,
    List<INamedTypeSymbol> ChunksWithNoId);
