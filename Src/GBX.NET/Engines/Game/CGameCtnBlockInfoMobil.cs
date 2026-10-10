using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobil
{
    private CPlugRoadChunk?[]? LegacyRoadChunks
    {
        get => RoadChunks;
        set => RoadChunks = ConvertRoadChunks<CPlugRoadChunkTraffic>(value);
    }

    private CPlugRoadChunk?[]? LegacyCitizenRoadChunks
    {
        get => CitizenRoadChunks;
        set => CitizenRoadChunks = ConvertRoadChunks<CPlugRoadChunkCitizen>(value);
    }

    private CPlugRoadChunk?[]? LegacyPlacementPatches
    {
        get => PlacementPatches;
        set => PlacementPatches = ConvertRoadChunks<CPlugPlacementPatch>(value);
    }

    // Native legacy readers change the road chunk's class while retaining its payload.
    private static T[]? ConvertRoadChunks<T>(CPlugRoadChunk?[]? chunks) where T : CPlugRoadChunk, new()
    {
        if (chunks is null) return null;
        if (chunks is T[] typedChunks) return typedChunks;

        var converted = new T[chunks.Length];
        var nodes = new Dictionary<CPlugRoadChunk, T>();
        for (var i = 0; i < chunks.Length; i++)
        {
            var source = chunks[i];
            if (source is null) continue;
            if (source is T typed)
            {
                converted[i] = typed;
                continue;
            }

            if (!nodes.TryGetValue(source, out var target))
            {
                target = new T();
                foreach (var chunk in source.Chunks) target.Chunks.Add(chunk);
                nodes.Add(source, target);
            }
            converted[i] = target;
        }
        return converted;
    }

    internal CPlugPath? ConvertedRailPath => LegacyRailPolylines is { Length: > 0 } polylines
        ? new CPlugPath { PolyLines = polylines }
        : null;

    internal bool ComputeHasGeomTransformation()
    {
        return !(IsNearlyZero(GeomTranslation.X)
            && IsNearlyZero(GeomTranslation.Y)
            && IsNearlyZero(GeomTranslation.Z)
            && IsNearlyZero(GeomRotation.X)
            && IsNearlyZero(GeomRotation.Y)
            && IsNearlyZero(GeomRotation.Z));
    }

    private static bool IsNearlyZero(float value)
        => Math.Abs(value) <= Math.Max(1, Math.Abs(value)) * 0.00001f;
}
