﻿using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;

Gbx.LZO = new Lzo();

foreach (var filePath in args)
{
    try
    {
        var node = Gbx.ParseNode(filePath);

        switch (node)
        {
            case CGameItemModel itemModel:
                if (itemModel.EntityModelEdition is CGameCommonItemEntityModelEdition modelEdition)
                {
                    if (modelEdition.MeshCrystal is not null)
                    {
                        modelEdition.MeshCrystal.ExportToObj(filePath + ".obj", filePath + ".mtl");
                    }
                }

                break;
            case CPlugSolid solid:
                solid.ExportToObj(filePath + ".obj", filePath + ".mtl");
                break;
            case CPlugSolid2Model solid2:
                break;
            default:
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}