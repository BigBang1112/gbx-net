using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;

var fileName = args[0];

Gbx.LZO = new Lzo();

var node = Gbx.ParseNode(fileName);

var mergeVerticesDigitThreshold = 3;
var objFileName = fileName + ".obj";
var mtlFileName = fileName + ".mtl";

switch (node)
{
    case CPlugSolid solid:
        solid.ExportToObj(objFileName, mtlFileName, mergeVerticesDigitThreshold);
        break;
    case CPlugSolid2Model solid2:
        solid2.ExportToObj(objFileName, mtlFileName, mergeVerticesDigitThreshold);
        break;
    case CPlugCrystal crystal:
        crystal.ExportToObj(objFileName, mtlFileName, mergeVerticesDigitThreshold);
        break;
    case CPlugPrefab prefab:
        for (int i = 0; i < prefab.Ents.Length; i++)
        {
            var ent = prefab.Ents[i];

            if (ent.Model is not CPlugStaticObjectModel staticObject)
            {
                continue;
            }

            if (staticObject.Mesh is null)
            {
                continue;
            }

            staticObject.Mesh.ExportToObj($"{fileName}_{i}.obj", $"{fileName}_{i}.mtl", mergeVerticesDigitThreshold);
        }
        break;
    case CGameItemModel itemModel:
        if (itemModel.EntityModelEdition is CGameCommonItemEntityModelEdition { MeshCrystal: not null } edition)
        {
            edition.MeshCrystal.ExportToObj(objFileName, mtlFileName, mergeVerticesDigitThreshold);
        }
        else if (itemModel.EntityModelEdition is CGameBlockItem block)
        {
            foreach (var variant in block.CustomizedVariants)
            {
                if (variant.Crystal is null)
                {
                    continue;
                }

                variant.Crystal.ExportToObj($"{fileName}_{variant.Id}.obj", $"{fileName}_{variant.Id}.mtl", mergeVerticesDigitThreshold);
            }
        }
        else if (itemModel.EntityModel is CGameCommonItemEntityModel { StaticObject.Mesh: not null } model)
        {
            model.StaticObject.Mesh.ExportToObj(objFileName, mtlFileName, mergeVerticesDigitThreshold);
        }
        else
        {
            throw new Exception("Item has no mesh that would be supported.");
        }
        break;
}