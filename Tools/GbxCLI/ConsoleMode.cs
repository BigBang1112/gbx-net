using System.Runtime.InteropServices;

namespace GbxCLI;

/// <summary>
/// Turns on ANSI escape sequences in the Windows console host. Windows Terminal and other platforms process them already.
/// </summary>
internal static partial class ConsoleMode
{
    private const int StdOutputHandle = -11;
    private const int StdErrorHandle = -12;
    private const uint EnableVirtualTerminalProcessing = 0x0004;

    private static readonly Lazy<bool> output = new(() => TryEnable(StdOutputHandle));
    private static readonly Lazy<bool> error = new(() => TryEnable(StdErrorHandle));

    /// <returns>True if the console shows ANSI colors.</returns>
    public static bool TryEnableVirtualTerminal(bool isError) => isError ? error.Value : output.Value;

    private static bool TryEnable(int standardHandle)
    {
        if (!OperatingSystem.IsWindows())
        {
            return true;
        }

        try
        {
            var handle = GetStdHandle(standardHandle);

            if (!GetConsoleMode(handle, out var mode))
            {
                return false;
            }

            return (mode & EnableVirtualTerminalProcessing) != 0 || SetConsoleMode(handle, mode | EnableVirtualTerminalProcessing);
        }
        catch (Exception exception) when (exception is DllNotFoundException or EntryPointNotFoundException)
        {
            return false;
        }
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial nint GetStdHandle(int standardHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(nint handle, out uint mode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(nint handle, uint mode);
}
