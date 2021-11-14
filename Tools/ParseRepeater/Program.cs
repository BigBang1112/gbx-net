using GBX.NET;

if (args.Length == 0)
    return;

var fileName = args[0];

while (true)
{
    try
    {
        GameBox.Parse(fileName);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine();
    Console.ReadKey(true);
}