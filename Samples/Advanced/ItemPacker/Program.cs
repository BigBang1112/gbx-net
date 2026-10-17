using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using Microsoft.Extensions.Logging;

Gbx.LZO = new Lzo();

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

var itemFileName = args[0];
var itemDirectory = Path.GetDirectoryName(itemFileName) ?? Environment.CurrentDirectory;

var itemGbx = Gbx.Parse<CGameItemModel>(itemFileName, new() { Logger = logger });
var itemNode = itemGbx.Node;

if (itemNode.PhyModelCustom is CGameObjectPhyModel { MoveShape: not null } phyModel)
{
    var shape = Gbx.ParseNode<CPlugSurface>(Path.Combine(itemDirectory, phyModel.MoveShape), new() { Logger = logger });
    phyModel.MoveShapeFid = shape;
    phyModel.MoveShape = null;
}

if (itemNode.VisModelCustom is CGameObjectVisModel { Mesh: not null } visModel)
{
    var mesh = Gbx.ParseNode<CPlugSolid2Model>(Path.Combine(itemDirectory, visModel.Mesh), new() { Logger = logger });
    visModel.MeshShaded = mesh;
    visModel.Mesh = null;
}

itemGbx.Save(itemGbx.GetFileNameWithoutExtension() + "-packed.Item.Gbx");
