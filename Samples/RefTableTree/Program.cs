using GBX.NET;
using GBX.NET.Exceptions;
using System.Dynamic;
using System.Text.Json;

if (args.Length == 0)
{
    return;
}

var fileName = args[0];
var mainFolderPath = Path.GetDirectoryName(fileName) ?? throw new Exception();

var expando = new ExpandoObject();
var dictOfUniqueGbxs = new Dictionary<string, GameBox?>();

RecurseRefTable(fileName, mainFolderPath, expando, level: 0);

File.WriteAllText(fileName + ".RefTableTree.json", JsonSerializer.Serialize(expando));

void RecurseRefTable(string fileName, string folderPath, ExpandoObject expando, int level)
{
    var itsOwnExpando = new ExpandoObject();

    if (expando is not IDictionary<string, object> dictionary)
    {
        throw new Exception();
    }

    dictionary[Path.GetFileName(fileName)] = itsOwnExpando;

    if (!dictOfUniqueGbxs.TryGetValue(fileName, out GameBox? gbx))
    {
        dictOfUniqueGbxs.Add(fileName, null);

        if (fileName.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase))
        {
            gbx = GameBox.ParseHeader(fileName);
            dictOfUniqueGbxs[fileName] = gbx;
        }
        else if (!File.Exists(fileName))
        {
            Console.WriteLine("File not found: {0}", fileName);
        }
    }

    if (gbx is null)
    {
        return;
    }

    var refTable = gbx.RefTable;

    if (refTable is not null)
    {
        foreach (var file in refTable.Files)
        {
            if (file.FileName is null)
            {
                continue;
            }

            var extNodePath = refTable.GetRelativeFolderPathToFile(file);
            var extNodeFileName = Path.GetFullPath(Path.Combine(folderPath, extNodePath, file.FileName));

            RecurseRefTable(extNodeFileName, Path.GetDirectoryName(extNodeFileName) ?? throw new Exception(), itsOwnExpando, level + 1);
        }
    }
}