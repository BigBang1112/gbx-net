namespace GbxCLI;

internal static class ArgumentExtensions
{
    /// <summary>
    /// Reads the value after an option, such as the path after <c>-o</c>.
    /// </summary>
    public static string NextValue(this IEnumerator<string> args, string option)
    {
        if (!args.MoveNext() || string.IsNullOrWhiteSpace(args.Current))
        {
            throw new ArgumentException($"Missing value after {option}.");
        }

        return args.Current;
    }

    public static ArgumentException Unexpected(string command, string arg) => new($"Unexpected argument for {command}: {arg}.");

    public static ArgumentException MissingInput(string command) => new($"Missing input file for {command}.");
}
