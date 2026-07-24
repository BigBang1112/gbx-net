using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.ZLib;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

Gbx.LZO = new Lzo();
Gbx.ZLib = new ZLib();

Console.WriteLine("GBX.NET Inspector");
Console.WriteLine("Enter a path to a Gbx file to parse and inspect it. Leave empty to exit.");

while (true)
{
    Console.Write("\nPath: ");

    var path = Console.ReadLine()?.Trim().Trim('"');

    if (string.IsNullOrEmpty(path))
    {
        break;
    }

    if (!File.Exists(path))
    {
        Console.WriteLine($"File not found: {path}");
        continue;
    }

    Gbx gbx;

    try
    {
        gbx = Gbx.Parse(path);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to parse file:\n{ex}");
        continue;
    }

    if (gbx.Node is null)
    {
        Console.WriteLine("Gbx has no main node (header-only or unrecognized class).");
        continue;
    }

    while (true)
    {
        Console.Write("> ");
        var eval = Console.ReadLine();

        if (string.IsNullOrEmpty(eval))
        {
            break;
        }

        try
        {
            var result = await CSharpScript.EvaluateAsync(eval, ScriptOptions.Default, globals: gbx.Node);
            PrintObject(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void PrintObject(object obj, int indent = 0)
{
    const string Reset = "\u001b[0m";
    const string Red = "\u001b[31m";

    Console.WriteLine(obj);

    foreach (var field in obj.GetType().GetFields())
    {
        Console.Write($"  {field.Name}: ");

        try
        {
            var value = field.GetValue(obj);
            Console.WriteLine(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Red}{ex.Message}{Reset}");
        }
    }

    foreach (var property in obj.GetType().GetProperties())
    {
        if (!property.CanRead)
        {
            continue;
        }

        Console.Write($"  {property.Name}: ");

        try
        {
            var value = property.GetValue(obj);
            Console.WriteLine(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Red}{ex.Message}{Reset}");
        }
    }

    if (obj is System.Collections.IEnumerable enumerable)
    {
        foreach (var item in enumerable)
        {
            Console.WriteLine($"  - {item ?? "null"}");
        }
    }
}