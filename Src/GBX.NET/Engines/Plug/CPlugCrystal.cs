
namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public IList<Layer>? Layers { get; set; }

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
                var layer = (ELayerType)r.ReadInt32() switch
                {
                    ELayerType.Geometry => new GeometryLayer(),
                    _ => throw new NotSupportedException()
                };

                layer.Read(r);

                n.Layers.Add(layer);
            }
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public abstract partial class Layer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class GeometryLayer;

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Crystal;
}
