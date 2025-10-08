﻿
using GBX.NET.Extensions.Exporters;

namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    private List<Layer>? layers;
    public List<Layer> Layers
    {
        get => layers ??= [];
        set => layers = value;
    }

    public void ExportToObj(TextWriter objWriter, TextWriter mtlWriter, int? mergeVerticesDigitThreshold = null)
    {
        ObjExporter.Export(this, objWriter, mtlWriter, mergeVerticesDigitThreshold);
    }

    public void ExportToObj(string objFilePath, string mtlFilePath, int? mergeVerticesDigitThreshold = null)
    {
        using var objWriter = new StreamWriter(objFilePath);
        using var mtlWriter = new StreamWriter(mtlFilePath);

        ExportToObj(objWriter, mtlWriter, mergeVerticesDigitThreshold);
    }

    public Vec2[] GetLightmapCoords()
    {
        return Layers.OfType<GeometryLayer>()
            .Select(x => x.Crystal)
            .OfType<Crystal>()
            .SelectMany(c => c.Faces)
            .SelectMany(f => f.Vertices.Select(v => v.LightmapCoord))
            .ToArray();
    }

    public Vec2[][] GetLightmapCoordFaces()
    {
        return Layers.OfType<GeometryLayer>()
            .Select(x => x.Crystal)
            .OfType<Crystal>()
            .SelectMany(c => c.Faces)
            .Select(f => f.Vertices.Select(v => v.LightmapCoord).ToArray())
            .ToArray();
    }

    public partial class Chunk09003000 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GbxReader r)
        {
            Version = r.ReadInt32();

            n.Layers = new()
            {
                new GeometryLayer()
                {
                    LayerId = "Layer0",
                    LayerName = "Geometry",
                    Crystal = r.ReadReadable<Crystal, CPlugCrystal>(n)
                }
            };
        }

        public override void Write(CPlugCrystal n, GbxWriter w)
        {
            w.Write(Version);

            var geometryLayer = n.Layers?.OfType<GeometryLayer>().FirstOrDefault();
            w.WriteWritable(geometryLayer?.Crystal, n);
        }
    }

    public partial class Chunk09003005 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GbxReader r)
        {
            Version = r.ReadInt32();

            var layerCount = r.ReadInt32();
            n.Layers = new List<Layer>(layerCount);

            for (var i = 0; i < layerCount; i++)
            {
                var layerType = (ELayerType)r.ReadInt32();
                Layer layer = layerType switch
                {
                    ELayerType.Geometry => new GeometryLayer(),
                    ELayerType.SubdivideSmooth => new SubdivideSmoothLayer(),
                    ELayerType.Translation => new TranslationLayer(),
                    ELayerType.Rotation => new RotationLayer(),
                    ELayerType.Scale => new ScaleLayer(),
                    ELayerType.Mirror => new MirrorLayer(),
                    ELayerType.MoveToGround => new MoveToGroundLayer(),
                    ELayerType.Extrude => new ExtrudeLayer(),
                    ELayerType.Subdivide => new SubdivideLayer(),
                    ELayerType.Chaos => new ChaosLayer(),
                    ELayerType.Smooth => new SmoothLayer(),
                    ELayerType.BorderTransition => new BorderTransitionLayer(),
                    ELayerType.Deformation => new DeformationLayer(),
                    ELayerType.Cubes => new CubesLayer(),
                    ELayerType.Trigger => new TriggerLayer(),
                    ELayerType.SpawnPosition => new SpawnPositionLayer(),
                    ELayerType.Light => new LightLayer(),
                    _ => throw new NotSupportedException($"Layer type {layerType} is not supported")
                };

                layer.Read(r, n);

                n.Layers.Add(layer);
            }
        }

        public override void Write(CPlugCrystal n, GbxWriter w)
        {
            w.Write(Version);

            w.Write(n.Layers?.Count ?? 0);

            foreach (var layer in n.Layers ?? [])
            {
                w.Write(layer switch
                {
                    GeometryLayer => (int)ELayerType.Geometry,
                    SubdivideSmoothLayer => (int)ELayerType.SubdivideSmooth,
                    TranslationLayer => (int)ELayerType.Translation,
                    RotationLayer => (int)ELayerType.Rotation,
                    ScaleLayer => (int)ELayerType.Scale,
                    MirrorLayer => (int)ELayerType.Mirror,
                    MoveToGroundLayer => (int)ELayerType.MoveToGround,
                    ExtrudeLayer => (int)ELayerType.Extrude,
                    SubdivideLayer => (int)ELayerType.Subdivide,
                    ChaosLayer => (int)ELayerType.Chaos,
                    SmoothLayer => (int)ELayerType.Smooth,
                    BorderTransitionLayer => (int)ELayerType.BorderTransition,
                    DeformationLayer => (int)ELayerType.Deformation,
                    CubesLayer => (int)ELayerType.Cubes,
                    TriggerLayer => (int)ELayerType.Trigger,
                    SpawnPositionLayer => (int)ELayerType.SpawnPosition,
                    LightLayer => (int)ELayerType.Light,
                    _ => throw new NotSupportedException()
                });

                layer.Write(w, n);
            }
        }
    }

    public partial class Chunk09003006 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GbxReader r)
        {
            Version = r.ReadInt32();

            var faces = n.Layers.OfType<GeometryLayer>()
                .Where(x => x.IsVisible)
                .Select(x => x.Crystal)
                .OfType<Crystal>()
                .SelectMany(c => c.Faces);

            var lightmapCoordCount = r.ReadInt32();

            if (Version == 0)
            {
                var counter = 0;
                foreach (var face in faces)
                {
                    for (int i = 0; i < face.Vertices.Length; i++)
                    {
                        face.Vertices[i] = face.Vertices[i] with { LightmapCoord = r.ReadVec2() };
                        counter++;

                        if (counter > lightmapCoordCount)
                        {
                            throw new Exception("LightmapCoord count exceeded");
                        }
                    }
                }

                if (counter != lightmapCoordCount)
                {
                    throw new Exception("LightmapCoord count mismatch");
                }
            }

            if (Version >= 1)
            {
                var lightmapCoords = new Vec2[lightmapCoordCount];

                for (int i = 0; i < lightmapCoordCount; i++)
                {
                    lightmapCoords[i] = (r.ReadUInt16() / (float)ushort.MaxValue, r.ReadUInt16() / (float)ushort.MaxValue);
                }

                // indices of lightmap coords
                var indices = Version >= 2 ? r.ReadArrayOptimizedInt() : null;

                var lightmapCount = indices?.Length ?? lightmapCoordCount;

                var counter = 0;
                foreach (var face in faces)
                {
                    for (int i = 0; i < face.Vertices.Length; i++)
                    {
                        var index = indices is null ? counter : indices[counter];
                        face.Vertices[i] = face.Vertices[i] with { LightmapCoord = lightmapCoords[index] };
                        counter++;

                        if (counter > lightmapCount)
                        {
                            throw new Exception("LightmapCoord count exceeded");
                        }
                    }
                }

                if (counter != lightmapCount)
                {
                    throw new Exception("LightmapCoord count mismatch");
                }
            }
        }

        public override void Write(CPlugCrystal n, GbxWriter w)
        {
            w.Write(Version);

            var faces = n.Layers.OfType<GeometryLayer>()
                .Where(x => x.IsVisible)
                .Select(x => x.Crystal)
                .OfType<Crystal>()
                .SelectMany(c => c.Faces);

            var lightmapCoords = faces.SelectMany(f => f.Vertices.Select(v => v.LightmapCoord));

            if (Version == 0)
            {
                w.WriteArray(lightmapCoords.ToArray());
            }

            if (Version >= 1)
            {
                var lightmapCoordArray = Version == 1
                    ? lightmapCoords.ToArray()
                    : lightmapCoords.Distinct().ToArray();

                w.Write(lightmapCoordArray.Length);

                foreach (var lightmap in lightmapCoordArray)
                {
                    w.Write((ushort)(lightmap.X * ushort.MaxValue));
                    w.Write((ushort)(lightmap.Y * ushort.MaxValue));
                }

                if (Version >= 2)
                {
                    var lightmapCoordIndices = lightmapCoordArray
                        .Select((x, i) => (x, i))
                        .ToDictionary(x => x.x, x => x.i);
                    var indices = faces.SelectMany(f => f.Vertices.Select(v => lightmapCoordIndices[v.LightmapCoord])).ToArray();

                    w.WriteArrayOptimizedInt(indices);
                }
            }
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public abstract partial class Layer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public abstract partial class ModifierLayer
    {
        public override void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            base.Read(r, n, v);
            ModifierVersion = r.ReadInt32();
            Mask = r.ReadArrayReadable<PartInLayer>();
        }

        public override void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            base.Write(w, n, v);
            w.Write(ModifierVersion);
            w.WriteArrayWritable<PartInLayer>(Mask);
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class PartInLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class GeometryLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SubdivideSmoothLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class TranslationLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class RotationLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class ScaleLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class MirrorLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class MoveToGroundLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class ExtrudeLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SubdivideLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class ChaosLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SmoothLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class BorderTransitionLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class DeformationLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class CubesLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class TriggerLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SpawnPositionLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class LightLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class LightPos;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VoxelSpace;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Crystal;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VisualLevel;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class AnchorInfo;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Part
    {
        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public override string ToString()
        {
            return $"{Name} {U01} {U02} {U03} {U04} [{string.Join(", ", U05 ?? [])}]";
        }
    }

    public sealed partial class Crystal : IVersionable
    {
        public int Version { get; set; }
        /// <summary>
        /// 4
        /// </summary>
        public int U01 { get; set; }
        public VisualLevel[] VisualLevels { get; set; } = [];
        public AnchorInfo[] AnchorInfos { get; set; } = [];
        public Part[] Groups { get; set; } = [];
        public bool IsEmbeddedCrystal { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public Vec3[] Positions { get; set; } = [];
        public Int2[] Edges { get; set; } = [];
        public Face[] Faces { get; set; } = [];
        public int U04 { get; set; }
        public int U05 { get; set; }
        public string? U06 { get; set; }
        public int U07 { get; set; }

        /// <summary>
        /// If <see cref="Edges"/> contains edges that are part of a face.
        /// </summary>
        public bool HasFacedEdges => Version < 35;

        public void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            Version = r.ReadInt32();

            if (Version >= 13)
            {
                U01 = r.ReadInt32(); // 4
                VisualLevels = r.ReadArrayReadable<VisualLevel>();
            }

            if (Version >= 23)
            {
                AnchorInfos = r.ReadArrayReadable<AnchorInfo>();
            }

            if (Version >= 22)
            {
                Groups = r.ReadArrayReadable<Part>(version: Version);
            }

            if (Version < 21)
            {
                throw new VersionNotSupportedException(Version);
            }

            if (Version >= 25)
            {
                if (Version < 29)
                {
                    IsEmbeddedCrystal = r.ReadBoolean();
                    IsEmbeddedCrystal = r.ReadBoolean();
                }

                IsEmbeddedCrystal = r.ReadBoolean(asByte: Version >= 34);

                if (Version >= 33)
                {
                    U02 = r.ReadInt32();
                    U03 = r.ReadInt32();
                }
            }

            if (!IsEmbeddedCrystal)
            {
                throw new NotSupportedException("Crystal.Gbx is not supported");
            }

            Positions = r.ReadArray<Vec3>();

            var edgeCount = r.ReadInt32();

            Edges = HasFacedEdges
                ? r.ReadArray<Int2>(edgeCount)
                : r.ReadArrayOptimizedInt2();

            var faceCount = r.ReadInt32();

            var texCoords = Array.Empty<Vec2>();
            var texCoordIndices = Array.Empty<int>();

            if (Version >= 37)
            {
                texCoords = r.ReadArray<Vec2>();
                texCoordIndices = r.ReadArrayOptimizedInt();
            }

            var faceVertexIndex = 0;

            Faces = new Face[faceCount];

            for (var i = 0; i < faceCount; i++)
            {
                var vertCount = Version >= 35 ? (r.ReadByte() + 3) : r.ReadInt32();
                var inds = Version >= 34 ? r.ReadArrayOptimizedInt(vertCount, Positions.Length) : r.ReadArray<int>(vertCount);

                var vertices = new Vertex[vertCount];
                var u01 = default(Vec3?);

                if (Version < 27)
                {
                    var uvCount = Math.Min(r.ReadInt32(), vertCount);

                    // this doesnt sound right
                    for (var j = 0; j < uvCount; j++)
                    {
                        vertices[j] = new Vertex(inds[j], TexCoord: r.ReadVec2(), default);
                    }

                    u01 = r.ReadVec3(); // normal?
                }
                else if (Version < 37)
                {
                    for (var j = 0; j < vertices.Length; j++)
                    {
                        vertices[j] = new Vertex(inds[j], TexCoord: r.ReadVec2(), default);
                    }
                }
                else
                {
                    for (var j = 0; j < vertCount; j++)
                    {
                        vertices[j] = new Vertex(inds[j], TexCoord: texCoords[texCoordIndices[faceVertexIndex++]], default);
                    }
                }

                var materialIndex = -1;

                if (Version >= 25)
                {
                    materialIndex = Version >= 33 && n.Materials.Count > 0
                        ? r.ReadOptimizedInt(n.Materials.Count) // normal int when count = 0? yes
                        : r.ReadInt32();
                }

                var groupIndex = Version >= 33 ? r.ReadOptimizedInt(Groups.Length) : r.ReadInt32();

                var material = n.Materials.Count == 0 || materialIndex == -1 ? null : n.Materials[materialIndex];

                Faces[i] = new Face(vertices, Groups[groupIndex], material, u01);
            }

            foreach (var face in Faces)
            {
                if (!IsEmbeddedCrystal)
                {
                    r.ReadInt32();
                }

                if (Version < 30 || !IsEmbeddedCrystal)
                {
                    r.ReadInt32();
                }

                if (Version >= 22 && !IsEmbeddedCrystal)
                {
                    r.ReadInt32();
                }
            }

            if (Version < 29)
            {
                foreach (var pos in Positions)
                {
                    r.ReadSingle();
                }
            }

            U04 = r.ReadInt32();

            if (Version >= 7 && Version < 32)
            {
                var crystalLinkLength = r.ReadInt32(); // crystal link array

                if (crystalLinkLength > 0)
                {
                    throw new NotSupportedException("CCrystalLink array length > 0");
                }

                if (Version >= 10)
                {
                    U05 = r.ReadInt32(); // 0 on newer crystals
                    U06 = r.ReadString(); // empty on newer crystals

                    if (Version < 30)
                    {
                        r.ReadArray<float>(); // SCrystalSmoothingGroup array
                    }
                }
            }

            if (Version < 36)
            {
                var numFaces = r.ReadInt32();
                var numEdges = r.ReadInt32();
                var numVerts = r.ReadInt32();

                r.SkipData(numFaces * 4);
                r.SkipData(numEdges * 4);
                r.SkipData(numVerts * 4);

                U07 = r.ReadInt32(); // always zero on newer crystals

                // on non-embedded crystal some face properties array: int, 4 floats
            }
        }

        public void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            w.Write(Version);

            if (Version >= 13)
            {
                w.Write(U01); // 4
                w.WriteArrayWritable<VisualLevel>(VisualLevels);
            }

            if (Version >= 23)
            {
                w.WriteArrayWritable<AnchorInfo>(AnchorInfos);
            }

            if (Version >= 22)
            {
                w.WriteArrayWritable<Part>(Groups, version: Version);
            }

            if (Version >= 25)
            {
                if (Version < 29)
                {
                    w.Write(IsEmbeddedCrystal);
                    w.Write(IsEmbeddedCrystal);
                }

                w.Write(IsEmbeddedCrystal, asByte: Version >= 34);

                if (Version >= 33)
                {
                    w.Write(U02);
                    w.Write(U03);
                }
            }

            if (!IsEmbeddedCrystal)
            {
                throw new NotSupportedException("Crystal.Gbx is not supported");
            }

            w.WriteArray<Vec3>(Positions);

            if (HasFacedEdges)
            {
                w.WriteArray<Int2>(Edges);
            }
            else
            {
                w.Write(0);
                w.WriteArrayOptimizedInt2(Edges);
            }

            w.Write(Faces.Length);

            if (Version >= 37)
            {
                if (Faces.Length == 0)
                {
                    w.Write(0);
                    w.Write(0);
                }
                else
                {
                    var texCoordDict = new Dictionary<Vec2, int>();
                    var texCoordIndices = new List<int>();

                    foreach (var face in Faces)
                    {
                        foreach (var vertex in face.Vertices)
                        {
                            if (texCoordDict.TryGetValue(vertex.TexCoord, out var texCoordIndex))
                            {
                                texCoordIndices.Add(texCoordIndex);
                            }
                            else
                            {
                                texCoordIndices.Add(texCoordDict.Count);
                                texCoordDict[vertex.TexCoord] = texCoordDict.Count;
                            }
                        }
                    }

                    w.WriteArray<Vec2>(texCoordDict.Keys.ToArray());
                    w.WriteArrayOptimizedInt(texCoordIndices.ToArray());
                }
            }

            foreach (var face in Faces)
            {
                if (Version >= 35)
                {
                    w.Write((byte)(face.Vertices.Length - 3));
                }
                else
                {
                    w.Write(face.Vertices.Length);
                }

                var indices = new int[face.Vertices.Length];

                for (var i = 0; i < face.Vertices.Length; i++)
                {
                    indices[i] = face.Vertices[i].Index;
                }

                if (Version >= 34)
                {
                    w.WriteArrayOptimizedInt(indices, Positions.Length, hasLengthPrefix: false);
                }
                else
                {
                    for (var i = 0; i < indices.Length; i++)
                    {
                        w.Write(indices[i]);
                    }
                }

                if (Version < 27)
                {
                    // this doesnt sound right
                    w.Write(face.Vertices.Length);

                    foreach (var vertex in face.Vertices)
                    {
                        w.Write(vertex.TexCoord);
                    }

                    w.Write(face.U01 ?? Vec3.Zero); // normal?
                }
                else if (Version < 37)
                {
                    foreach (var vertex in face.Vertices)
                    {
                        w.Write(vertex.TexCoord);
                    }
                }

                if (Version >= 25)
                {
                    var materialIndex = face.Material is null ? -1 : n.Materials.IndexOf(face.Material);

                    if (Version >= 33 && n.Materials.Count > 0)
                    {
                        // this can write 255 in case of -1, which is not correct?
                        w.WriteOptimizedInt(materialIndex, n.Materials.Count);
                    }
                    else
                    {
                        w.Write(materialIndex);
                    }
                }

                var groupIndex = Array.IndexOf(Groups, face.Group);

                if (Version >= 33)
                {
                    w.WriteOptimizedInt(groupIndex, Groups.Length);
                }
                else
                {
                    w.Write(groupIndex);
                }
            }

            if (Version < 29)
            {
                foreach (var pos in Positions)
                {
                    w.Write(0);
                }
            }

            w.Write(U04);

            if (Version >= 7 && Version < 32)
            {
                w.Write(0);

                if (Version >= 10)
                {
                    w.Write(U05);
                    w.Write(U06);

                    if (Version < 30)
                    {
                        w.Write(0); // SCrystalSmoothingGroup array
                    }
                }
            }

            if (Version < 36)
            {
                w.Write(Faces.Length);
                w.Write(Edges.Length);
                w.Write(Positions.Length);

                w.Write(new byte[Faces.Length * 4 + Edges.Length * 4 + Positions.Length * 4]);

                w.Write(U07);
            }
        }
    }

    public sealed record Face(Vertex[] Vertices, Part Group, Material? Material, Vec3? U01)
    {
        public override string ToString()
        {
            return $"{Vertices.Length} vertices, material: {Material?.MaterialUserInst?.Link ?? Material?.MaterialName ?? "none"}";
        }
    }

    public readonly record struct Vertex(int Index, Vec2 TexCoord, Vec2 LightmapCoord);
}
