﻿
namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public IList<Layer>? Layers { get; set; }

    public partial class Chunk09003000 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GbxReader r)
        {
            Version = r.ReadInt32();

            n.Layers = new List<Layer>()
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
                Layer layer = (ELayerType)r.ReadInt32() switch
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
                    _ => throw new NotSupportedException()
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
                    _ => throw new NotSupportedException()
                });

                layer.Write(w, n);
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
        public Int2[]? UnfacedEdges { get; set; }
        public Int2[]? Edges { get; set; }
        public Face[] Faces { get; set; } = [];
        public int U04 { get; set; }
        public int U05 { get; set; }
        public string? U06 { get; set; }
        public int U07 { get; set; }

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

            if (Version >= 35)
            {
                UnfacedEdges = r.ReadArrayOptimizedInt2();
            }
            else
            {
                Edges = r.ReadArray<Int2>(edgeCount);
            }

            var faceCount = r.ReadInt32();

            var texCoords = Array.Empty<Vec2>();
            var faceIndices = Array.Empty<int>();

            if (Version >= 37)
            {
                texCoords = r.ReadArray<Vec2>();
                faceIndices = r.ReadArrayOptimizedInt();
            }

            var faceIndicesIndex = 0;

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
                        vertices[j] = new Vertex(Positions[inds[j]], TexCoord: r.ReadVec2());
                    }

                    u01 = r.ReadVec3(); // normal?
                }
                else if (Version < 37)
                {
                    for (var j = 0; j < vertices.Length; j++)
                    {
                        vertices[j] = new Vertex(Positions[inds[j]], TexCoord: r.ReadVec2());
                    }
                }
                else
                {
                    for (var j = 0; j < vertCount; j++)
                    {
                        vertices[j] = new Vertex(Positions[inds[j]], TexCoord: texCoords[faceIndices[faceIndicesIndex++]]);
                    }
                }

                var materialIndex = -1;

                if (Version >= 25)
                {
                    materialIndex = Version >= 33
                        ? r.ReadOptimizedInt(n.Materials.Count) // normal int when count = 0?
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

            w.Write(Edges?.Length ?? 0);

            if (Version >= 35)
            {
                w.WriteArrayOptimizedInt2(UnfacedEdges);
            }
            else
            {
                w.WriteArray<Int2>(Edges);
            }

            w.Write(Faces?.Length ?? 0);

            if (Version >= 37)
            {
                w.WriteArray<Vec2>([]);
                w.WriteArray<int>([]);
            }
        }
    }

    public sealed record Face(Vertex[] Vertices, Part Group, Material? Material, Vec3? U01);

    public readonly record struct Vertex(Vec3 Position, Vec2 TexCoord);
}
