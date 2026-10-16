using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoMobil
{
    private NodeRefList<CPlugRoadChunk>? LegacyRoadChunks
    {
        get => WidenRoadChunks(RoadChunks);
        set => RoadChunks = ConvertRoadChunks<CPlugRoadChunkTraffic>(value);
    }

    private NodeRefList<CPlugRoadChunk>? LegacyCitizenRoadChunks
    {
        get => WidenRoadChunks(CitizenRoadChunks);
        set => CitizenRoadChunks = ConvertRoadChunks<CPlugRoadChunkCitizen>(value);
    }

    private NodeRefList<CPlugRoadChunk>? LegacyPlacementPatches
    {
        get => WidenRoadChunks(PlacementPatches);
        set => PlacementPatches = ConvertRoadChunks<CPlugPlacementPatch>(value);
    }

    // Native legacy readers change the road chunk's class while retaining its payload.
    private static NodeRefList<CPlugRoadChunk>? WidenRoadChunks<T>(NodeRefList<T>? chunks) where T : CPlugRoadChunk
    {
        if (chunks is null) return null;
        var result = new NodeRefList<CPlugRoadChunk>();
        for (var i = 0; i < chunks.Count; i++)
        {
            var source = chunks.Get(i);
            var reference = new NodeRef<CPlugRoadChunk>();
            if (source.Descriptor is { } descriptor) reference.Descriptor = descriptor;
            else reference.Node = source.CachedNode;
            result.Add(reference);
        }
        return result;
    }

    private static NodeRefList<T>? ConvertRoadChunks<T>(NodeRefList<CPlugRoadChunk>? chunks) where T : CPlugRoadChunk, new()
    {
        if (chunks is null) return null;
        var converted = new NodeRefList<T>();
        var nodes = new Dictionary<CPlugRoadChunk, T>();
        for (var i = 0; i < chunks.Count; i++)
        {
            var reference = chunks.Get(i);
            converted.Add(new NodeRef<T>());
            if (reference.Descriptor is { } descriptor)
            {
                converted.Get(i).Descriptor = descriptor;
                continue;
            }
            var source = reference.CachedNode;
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

    internal CPlugPath? ConvertedRailPath => LegacyRailPolylines is { Count: > 0 } polylines
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
