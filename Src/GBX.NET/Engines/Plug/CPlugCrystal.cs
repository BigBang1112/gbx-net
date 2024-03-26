
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
                    Crystal = r.ReadReadable<Crystal>()
                }
            };
        }

        public override void Write(CPlugCrystal n, GbxWriter w)
        {
            w.Write(Version);

            var geometryLayer = n.Layers.OfType<GeometryLayer>().First();
            w.WriteWritable(geometryLayer.Crystal);
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

                layer.Read(r);

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

                layer.Write(w);
            }
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public abstract partial class Layer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public abstract partial class ModifierLayer
    {
        public override void Read(GbxReader r, int version = 0)
        {
            base.Read(r, version);
            modifierVersion = r.ReadInt32();
            mask = r.ReadArrayReadable<PartInLayer>();
        }

        public override void Write(GbxWriter w, int version = 0)
        {
            base.Write(w, version);
            w.Write(modifierVersion);
            w.WriteArrayWritable<PartInLayer>(mask);
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
}
