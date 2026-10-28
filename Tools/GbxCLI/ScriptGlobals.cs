namespace GbxCLI;

/// <summary>Arguments and output available to scripts run by the CLI.</summary>
public sealed class ScriptGlobals(string[] args, TextWriter output)
{
    /// <summary>Gets the arguments passed after the script path.</summary>
    public string[] Args { get; } = args;

    /// <summary>Gets the CLI's standard output writer.</summary>
    public TextWriter Output { get; } = output;

    /// <summary>Writes a value followed by a newline to standard output.</summary>
    public void Print(object? value) => Output.WriteLine(value);
}
