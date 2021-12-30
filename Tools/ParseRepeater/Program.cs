using GBX.NET;
using Microsoft.Extensions.Logging;

if (args.Length == 0)
    return;

var fileName = args[0];

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

while (true)
{
    try
    {
        var gbx = GameBox.Parse(fileName, logger: logger);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine();
    Console.ReadKey(true);
}