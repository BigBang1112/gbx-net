using GbxCLI;
using System.Text;

try
{
    // Map names and zones often use characters outside the console's legacy code page.
    Console.OutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}
catch (IOException)
{
    // There is no console to configure.
}

return await Cli.RunAsync(args, Console.In, Console.Out, Console.Error);
