using GBX.NET.Lua;

using var cts = new CancellationTokenSource();

var engine = new LuaEngine();

foreach (var filePath in args)
{
    var code = await File.ReadAllTextAsync(filePath, cts.Token);

    try
    {
        engine.Run(code);
    }
    catch (LuaException e)
    {
        Console.WriteLine($"Error in {filePath}:\n{e.Message}");
    }
}