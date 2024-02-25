using GBX.NET;
using GBX.NET.LZO;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ParseRepeater <filename>");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

var fileName = args[0];

Gbx.LZO = new MiniLZO();

while (true)
{
    try
    {
        var gbx = Gbx.ParseNode(fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception during parse: " + ex);
    }

    Console.WriteLine();
    Console.ReadKey(true);
}