using GBX.NET;
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
}