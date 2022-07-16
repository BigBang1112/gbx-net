using GBX.NET;

if (args.Length == 0)
    return;

var pattern = "*.Gbx";
var directory = default(string?);

var argsEnumerator = args.GetEnumerator();

while (argsEnumerator.MoveNext())
{
    switch (argsEnumerator.Current)
    {
        case "-pattern":
            if (argsEnumerator.MoveNext())
                pattern = $"*.{argsEnumerator.Current}.Gbx";
            break;
        default:
            var inputDirectory = (string)argsEnumerator.Current;
            if (Directory.Exists(inputDirectory))
                directory = inputDirectory;
            break;
    }
}

if (directory is null)
    return;

var files = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
var exceptionMessages = new List<string>();

var successful = 0;

for (var i = 0; i < files.Length; i++)
{
    var fileName = files[i];

    try
    {
        var node = GameBox.ParseNode(fileName);

        if (node is null)
        {
            Console.WriteLine(fileName + " returns null!");
        }
        else
        {
            successful++;
        }
    }
    catch (Exception ex)
    {
        if (!exceptionMessages.Contains(ex.Message))
        {
            Console.WriteLine(fileName);
            Console.WriteLine(ex);
            Console.WriteLine();

            exceptionMessages.Add(ex.Message);
        }
    }

    Console.Write("Progress: {0}/{1}/{2} ({3})", successful, i + 1, files.Length, (successful / (float)(i + 1)).ToString("P"));
    Console.CursorLeft = 0;
}

Console.WriteLine("Complete!");