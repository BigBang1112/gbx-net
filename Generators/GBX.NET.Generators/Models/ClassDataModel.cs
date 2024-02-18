using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace GBX.NET.Generators.Models;

internal record ClassDataModel(
    string Name,
    uint? Id,
    string Engine,
    string? Inherits,
    string? Description,
    INamedTypeSymbol? TypeSymbol,
    ImmutableDictionary<uint, ChunkDataModel> HeaderChunks,
    ImmutableDictionary<uint, ChunkDataModel> Chunks,
    ImmutableList<INamedTypeSymbol> ChunksWithNoId,
    ArchiveDataModel? NamelessArchive,
    ImmutableDictionary<string, ArchiveDataModel> Archives,
    ImmutableDictionary<string, EnumDataModel> Enums);