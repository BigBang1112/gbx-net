
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

        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            var crystal = n.Layers.OfType<GeometryLayer>().FirstOrDefault()?.Crystal;
            if (rw.Reader is not null)
            {
                crystal = new Crystal();
                crystal.ReadWrite(rw, n);
                n.Layers =
                [
                    new GeometryLayer
                    {
                        LayerId = "Layer0",
                        LayerName = "Layer0",
                        Crystal = crystal,
                        GroupIndices = Enumerable.Range(0, crystal!.Groups.Length).ToArray()
                    }
                ];
            }
            else
            {
                (crystal ?? new Crystal()).ReadWrite(rw, n);
            }
        }
    }

    public partial class Chunk09003003
    {
        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.Materials = rw.ListReadableWritable(n.Materials, version: Version) ?? [];

            if (Version >= 2)
            {
                if (rw.Reader is not null)
                {
                    foreach (var face in n.Layers.OfType<GeometryLayer>().SelectMany(layer => layer.Crystal?.Faces ?? []))
                    {
                        face.Material = face.MaterialIndex < 0 ? null : n.Materials[face.MaterialIndex];
                    }
                }
                return;
            }

            foreach (var face in n.Layers.OfType<GeometryLayer>().First().Crystal!.Faces)
            {
                var materialIndex = rw.Int32(face.Material is null ? face.MaterialIndex : n.Materials.IndexOf(face.Material));
                face.LegacyMaterialValues = rw.Vec4(face.LegacyMaterialValues);
                
                if (rw.Reader is not null)
                {
                    face.Material = materialIndex == -1 ? null : n.Materials[materialIndex];
                    face.MaterialIndex = materialIndex;
                }
            }
        }
    }

    public partial class Chunk09003005
    {
        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            var layerCount = rw.Int32(n.Layers.Count);
            
            if (rw.Reader is not null)
            {
                n.Layers = new List<Layer>(layerCount);
            }

            for (var i = 0; i < layerCount; i++)
            {
                var layerType = rw.Int32(rw.Writer is null ? 0 : (int)GetLayerType(n.Layers[i]));
                if (rw.Reader is not null)
                {
                    Layer layer = (ELayerType)layerType switch
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
                        ELayerType.Sector => new SectorLayer(),
                        ELayerType.ParticleEmitter => new ParticleEmitterLayer(),
                        ELayerType.Light => new LightLayer(),
                        ELayerType.WaterShape => new WaterShapeLayer(),
                        _ => throw new NotSupportedException($"Layer type {layerType} is not supported")
                    };
                    layer.Read(rw.Reader, n);
                    n.Layers.Add(layer);
                }
                else
                {
                    n.Layers[i].Write(rw.Writer!, n);
                }
            }
        }

        private static ELayerType GetLayerType(Layer layer) => layer switch
        {
            GeometryLayer => ELayerType.Geometry,
            SubdivideSmoothLayer => ELayerType.SubdivideSmooth,
            TranslationLayer => ELayerType.Translation,
            RotationLayer => ELayerType.Rotation,
            ScaleLayer => ELayerType.Scale,
            MirrorLayer => ELayerType.Mirror,
            MoveToGroundLayer => ELayerType.MoveToGround,
            ExtrudeLayer => ELayerType.Extrude,
            SubdivideLayer => ELayerType.Subdivide,
            ChaosLayer => ELayerType.Chaos,
            SmoothLayer => ELayerType.Smooth,
            BorderTransitionLayer => ELayerType.BorderTransition,
            DeformationLayer => ELayerType.Deformation,
            CubesLayer => ELayerType.Cubes,
            TriggerLayer => ELayerType.Trigger,
            SpawnPositionLayer => ELayerType.SpawnPosition,
            SectorLayer => ELayerType.Sector,
            ParticleEmitterLayer => ELayerType.ParticleEmitter,
            LightLayer => ELayerType.Light,
            WaterShapeLayer => ELayerType.WaterShape,
            _ => throw new NotSupportedException($"Layer {layer.GetType().Name} is not supported")
        };
    }

    public partial class Chunk09003006 : IVersionable
    {
        private bool hasReadLightmapCoords;

        public int Version { get; set; }

        /// <summary>
        /// Lightmap UVs for the resulting geometry, which can differ from the editable layers.
        /// </summary>
        public Vec2[] LightmapCoords { get; set; } = [];
        public int[] LightmapCoordIndices { get; set; } = [];

        public override void Read(CPlugCrystal n, GbxReader r)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(n, rw);
        }

        public override void Write(CPlugCrystal n, GbxWriter w)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(n, rw);
        }

        public override void ReadWrite(CPlugCrystal n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version < 0 || Version > 2) throw new VersionNotSupportedException(Version);

            var faces = n.Layers.OfType<GeometryLayer>()
                .Where(x => x.IsEnabled && x.IsVisible)
                .Select(x => x.Crystal)
                .OfType<Crystal>()
                .SelectMany(c => c.Faces).ToArray();
            var vertexCount = faces.Sum(face => face.Vertices.Length);
            var lightmapCount = Version >= 2 ? LightmapCoordIndices.Length : LightmapCoords.Length;
            if (rw.Writer is not null && (lightmapCount == vertexCount || (!hasReadLightmapCoords && LightmapCoords.Length == 0)))
            {
                var coords = faces.SelectMany(face => face.Vertices.Select(vertex => vertex.LightmapCoord)).ToArray();
                var storedCoords = Version < 2 ? LightmapCoords.AsEnumerable()
                    : LightmapCoordIndices.Select(index => LightmapCoords[index]);
                if (!storedCoords.SequenceEqual(coords))
                {
                    LightmapCoords = Version < 2 ? coords : coords.Distinct().ToArray();
                    if (Version >= 2)
                    {
                        var indices = LightmapCoords.Select((coord, index) => (coord, index)).ToDictionary(x => x.coord, x => x.index);
                        LightmapCoordIndices = coords.Select(coord => indices[coord]).ToArray();
                    }
                }
            }

            var count = rw.Int32(LightmapCoords.Length);
            if (rw.Reader is not null)
            {
                hasReadLightmapCoords = true;
                LightmapCoords = new Vec2[count];
            }
            for (var i = 0; i < count; i++)
            {
                if (Version == 0) LightmapCoords[i] = rw.Vec2(LightmapCoords[i]);
                else
                {
                    var coord = LightmapCoords[i];
                    LightmapCoords[i] = new Vec2(
                        rw.UInt16((ushort)Math.Min(ushort.MaxValue, Math.Max(0, coord.X * ushort.MaxValue))) / (float)ushort.MaxValue,
                        rw.UInt16((ushort)Math.Min(ushort.MaxValue, Math.Max(0, coord.Y * ushort.MaxValue))) / (float)ushort.MaxValue);
                }
            }
            if (Version >= 2) LightmapCoordIndices = rw.ArrayOptimizedInt(LightmapCoordIndices)!;

            lightmapCount = Version >= 2 ? LightmapCoordIndices.Length : count;
            if (rw.Reader is not null && lightmapCount == vertexCount)
            {
                var index = 0;
                foreach (var face in faces)
                {
                    for (var i = 0; i < face.Vertices.Length; i++)
                    {
                        var coordIndex = Version < 2 ? index : LightmapCoordIndices[index];
                        face.Vertices[i] = face.Vertices[i] with { LightmapCoord = LightmapCoords[coordIndex] };
                        index++;
                    }
                }
            }
        }
    }

    public partial class DeformationLayer
    {
        public BoxAligned Box { get; set; } = new(0, 0, 0, -1, -1, -1);
    }

    public abstract partial class Layer;
    public abstract partial class ModifierLayer;

    public partial class Part
    {
        public override string ToString()
        {
            return $"{Name} {U01} {IsInUse} {ParentIndex} {AnchorIndex} [{string.Join(", ", Children ?? [])}]";
        }
    }

    public sealed partial class Crystal : IVersionable
    {
        public int Version { get; set; } = 37;
        public int BaseVisualLevel { get; set; } = 4;
        public VisualLevel[] VisualLevels { get; set; } =
        [
            new() { U01 = 4, U02 = 64 },
            new() { U01 = 2, U02 = 128 },
            new() { U01 = 1, U02 = 192 }
        ];
        public AnchorInfo[] AnchorInfos { get; set; } = [];
        public Part[] Groups { get; set; } = [];
        public bool IsEmbeddedCrystal { get; set; }
        public int MaxMaterialIndex { get; set; }
        public int MaxGroupIndex { get; set; }
        public Vec3[] Positions { get; set; } = [];
        public Int2[] Edges { get; set; } = [];
        public int TotalEdgeCount { get; set; }
        public Face[] Faces { get; set; } = [];
        public int U04 { get; set; }
        public int U05 { get; set; }
        public string? U06 { get; set; }
        public int U07 { get; set; }
        public int[] FaceFlags { get; set; } = [];
        public int[] FaceProperties { get; set; } = [];
        public int[] LegacyFaceValues { get; set; } = [];
        public float[] VertexValues { get; set; } = [];
        public float[] SmoothingGroups { get; set; } = [0];
        public int[] U08 { get; set; } = [];
        public int[] U09 { get; set; } = [];
        public int[] U10 { get; set; } = [];

        /// <summary>
        /// Whether <see cref="Edges"/> includes edges belonging to faces.
        /// </summary>
        public bool HasFacedEdges => !IsEmbeddedCrystal || Version < 35;

        public void Read(GbxReader r, CPlugCrystal n, int v = 0)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(rw, n, v);
        }

        public void Write(GbxWriter w, CPlugCrystal n, int v = 0)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, n, v);
        }

        public void ReadWrite(GbxReaderWriter rw, CPlugCrystal n, int v = 0)
        {
            rw.VersionInt32(this);
            if (Version < 21 || Version > 37)
            {
                throw new VersionNotSupportedException(Version);
            }

            if (Version >= 13)
            {
                BaseVisualLevel = rw.Int32(BaseVisualLevel);
                VisualLevels = rw.ArrayReadableWritable(VisualLevels)!;
            }
            if (Version >= 23)
            {
                AnchorInfos = rw.ArrayReadableWritable(AnchorInfos)!;
            }
            if (Version >= 22)
            {
                Groups = rw.ArrayReadableWritable(Groups, version: Version)!;
            }
            if (Version >= 25)
            {
                if (Version < 29)
                {
                    IsEmbeddedCrystal = rw.Boolean(IsEmbeddedCrystal);
                    IsEmbeddedCrystal = rw.Boolean(IsEmbeddedCrystal);
                }
                IsEmbeddedCrystal = rw.Boolean(IsEmbeddedCrystal, asByte: Version >= 35);
            }

            if (Version >= 33)
            {
                if (rw.Writer is not null)
                {
                    MaxMaterialIndex = Math.Max(MaxMaterialIndex, Faces.Select(f => f.Material is null ? f.MaterialIndex : n.Materials.IndexOf(f.Material)).DefaultIfEmpty().Max());
                    MaxGroupIndex = Math.Max(MaxGroupIndex, Faces.Select(f => Array.IndexOf(Groups, f.Group)).DefaultIfEmpty().Max());
                }
                MaxMaterialIndex = rw.Int32(MaxMaterialIndex);
                MaxGroupIndex = rw.Int32(MaxGroupIndex);
            }
            if (!IsEmbeddedCrystal)
            {
                ReadWriteEditableGeometry(rw);
            }
            else
            {
                Positions = rw.Array(Positions)!;
                TotalEdgeCount = rw.Int32(HasFacedEdges ? Edges.Length : TotalEdgeCount);
                if (rw.Reader is not null)
                {
                    Edges = Version < 34
                        ? rw.Reader.ReadArray<Int2>(TotalEdgeCount)
                        : rw.Reader.ReadArrayOptimizedInt2(Version < 35 ? TotalEdgeCount : rw.Reader.ReadInt32(), Positions.Length);
                }
                if (rw.Writer is not null)
                {
                    if (Version < 34) rw.Writer.WriteArray(Edges, Edges.Length);
                    else rw.Writer.WriteArrayOptimizedInt2(Edges, Positions.Length, hasLengthPrefix: Version >= 35);
                }

                var faceCount = rw.Int32(Faces.Length);
                var texCoords = Array.Empty<Vec2>();
                var texCoordIndices = Array.Empty<int>();
                if (Version >= 37)
                {
                    if (rw.Writer is not null)
                    {
                        texCoords = Faces.SelectMany(f => f.Vertices.Select(vertex => vertex.TexCoord)).Distinct().ToArray();
                        var indices = texCoords.Select((coord, index) => (coord, index)).ToDictionary(x => x.coord, x => x.index);
                        texCoordIndices = Faces.SelectMany(f => f.Vertices.Select(vertex => indices[vertex.TexCoord])).ToArray();
                    }
                    texCoords = rw.Array(texCoords)!;
                    texCoordIndices = rw.ArrayOptimizedInt(texCoordIndices)!;
                }

                var faceVertexIndex = 0;
                if (rw.Reader is not null) Faces = new Face[faceCount];
                for (var i = 0; i < faceCount; i++)
                {
                    var face = rw.Writer is null ? null : Faces[i];
                    var vertexCount = Version >= 35
                        ? rw.Byte((byte)((face?.Vertices.Length ?? 3) - 3)) + 3
                        : rw.Int32(face?.Vertices.Length ?? 0);
                    var indices = face?.Vertices.Select(vertex => vertex.Index).ToArray() ?? [];
                    if (rw.Reader is not null)
                    {
                        indices = Version >= 34
                            ? rw.Reader.ReadArrayOptimizedInt(vertexCount, Positions.Length)
                            : rw.Reader.ReadArray<int>(vertexCount);
                    }
                    if (rw.Writer is not null)
                    {
                        if (Version >= 34) rw.Writer.WriteArrayOptimizedInt(indices, Positions.Length, hasLengthPrefix: false);
                        else rw.Writer.WriteArray(indices, indices.Length);
                    }

                    var vertices = face?.Vertices ?? new Vertex[vertexCount];
                    var uvLayers = face?.TexCoordLayers ?? [];
                    if (Version < 27)
                    {
                        var uvLayerCount = rw.Int32(uvLayers.Length == 0 ? 1 : uvLayers.Length);
                        if (rw.Reader is not null) uvLayers = new Vec2[uvLayerCount][];
                        for (var layer = 0; layer < uvLayerCount; layer++)
                        {
                            var coords = rw.Writer is null ? null : uvLayers.Length == 0
                                ? vertices.Select(vertex => vertex.TexCoord).ToArray() : uvLayers[layer];
                            coords = rw.Array(coords, vertexCount)!;
                            if (rw.Reader is not null) uvLayers[layer] = coords;
                        }
                    }
                    for (var j = 0; j < vertexCount; j++)
                    {
                        var texCoord = Version < 27 ? (uvLayers.Length == 0 ? default : uvLayers[0][j])
                            : Version < 37 ? rw.Vec2(vertices[j].TexCoord)
                            : texCoords[texCoordIndices[faceVertexIndex++]];
                        vertices[j] = new Vertex(indices[j], texCoord, vertices[j].LightmapCoord);
                    }
                    var normal = Version < 27 ? rw.Vec3(face?.Normal ?? Vec3.Zero) : default(Vec3?);
                    var materialIndex = face?.Material is null ? face?.MaterialIndex ?? -1 : n.Materials.IndexOf(face.Material);
                    if (Version >= 25)
                    {
                        materialIndex = Version >= 33 ? FaceIndex(rw, materialIndex, MaxMaterialIndex) : rw.Int32(materialIndex);
                    }
                    var materialValues = Version >= 25 && Version < 28 ? rw.Vec4(face?.LegacyMaterialValues ?? default) : default;
                    var groupIndex = face is null ? 0 : Array.IndexOf(Groups, face.Group);
                    groupIndex = Version >= 33 ? FaceIndex(rw, groupIndex, MaxGroupIndex) : rw.Int32(groupIndex);
                    if (rw.Reader is not null)
                    {
                        if (Groups.Length == 0) Groups = [new Part { Name = "part", IsInUse = true }];
                        if (groupIndex == -1) groupIndex = 0;
                        Faces[i] = new Face(vertices, Groups[groupIndex], materialIndex < 0 || n.Materials.Count == 0 ? null : n.Materials[materialIndex], normal)
                        {
                            MaterialIndex = materialIndex,
                            TexCoordLayers = uvLayers,
                            LegacyMaterialValues = materialValues
                        };
                    }
                }

                for (var i = 0; i < Faces.Length; i++)
                {
                    if (Version < 29)
                    {
                        FaceProperties = Resize(FaceProperties, Faces.Length);
                        LegacyFaceValues = Resize(LegacyFaceValues, Faces.Length);
                        FaceProperties[i] = rw.Int32(FaceProperties[i]);
                        LegacyFaceValues[i] = rw.Int32(LegacyFaceValues[i]);
                    }
                    if (Version < 30)
                    {
                        FaceFlags = Resize(FaceFlags, Faces.Length);
                        FaceFlags[i] = rw.Int32(FaceFlags[i]);
                    }
                }
                if (Version < 29) VertexValues = rw.Array(VertexValues, Positions.Length)!;
            }

            U04 = rw.Int32(U04);
            if (Version < 32)
            {
                var linkCount = rw.Int32(0);
                if (linkCount != 0) throw new NotSupportedException("CCrystalLink array length > 0");
                U05 = rw.Int32(U05);
                U06 = rw.String(U06);
            }
            if (Version >= 15 && Version < 30) SmoothingGroups = rw.Array(SmoothingGroups)!;
            if (Version < 36)
            {
                var numFaces = rw.Int32(Faces.Length);
                var numEdges = rw.Int32(TotalEdgeCount);
                var numVerts = rw.Int32(Positions.Length);
                U08 = rw.Array(U08, numFaces)!;
                U09 = rw.Array(U09, numEdges)!;
                U10 = rw.Array(U10, numVerts)!;
                U07 = rw.Int32(U07);
            }

            if (!IsEmbeddedCrystal && Version >= 24)
            {
                foreach (var face in Faces)
                {
                    face.MaterialIndex = rw.Int32(face.Material is null ? face.MaterialIndex : n.Materials.IndexOf(face.Material));
                    face.LegacyMaterialValues = rw.Vec4(face.LegacyMaterialValues);
                    if (rw.Reader is not null && face.MaterialIndex >= 0 && n.Materials.Count > 0)
                    {
                        face.Material = n.Materials[face.MaterialIndex];
                    }
                }
            }
        }

        private static int FaceIndex(GbxReaderWriter rw, int value, int maximum)
        {
            if ((uint)maximum < byte.MaxValue)
            {
                var result = rw.Byte((byte)value);
                return result == byte.MaxValue ? -1 : result;
            }
            if ((uint)maximum < ushort.MaxValue)
            {
                var result = rw.UInt16((ushort)value);
                return result == ushort.MaxValue ? -1 : result;
            }
            return rw.Int32(value);
        }

        private static int[] Resize(int[] values, int length)
        {
            if (values.Length != length) Array.Resize(ref values, length);
            return values;
        }
    }

    public sealed record Face(Vertex[] Vertices, Part Group, Material? Material, Vec3? Normal)
    {
        [Obsolete("Use Normal instead.")]
        public Vec3? U01
        {
            get => Normal;
            init => Normal = value;
        }

        public Material? Material { get; set; } = Material;
        public int MaterialIndex { get; set; } = -1;
        public Vec4 LegacyMaterialValues { get; set; }
        public Vec2[][] TexCoordLayers { get; set; } = [];

        public override string ToString()
        {
            return $"{Vertices.Length} vertices, material: {Material?.MaterialUserInst?.Link ?? Material?.MaterialName ?? "none"}";
        }
    }

    public readonly record struct Vertex(int Index, Vec2 TexCoord, Vec2 LightmapCoord);
}
