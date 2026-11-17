using System.Runtime.InteropServices;

namespace GbxCLI;

/// <summary>
/// Turns on ANSI escape sequences in the Windows console host, which Windows Terminal and other platforms process already,
/// and tells whether the console window belongs to this process.
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

    /// <summary>
    /// Whether this process is the only one attached to its console, so the window closes when it ends.
    /// That happens when gbx.exe is started from Explorer, such as by a double-click or a file dropped on it, but not from a terminal.
    /// </summary>
    public static bool OwnsConsoleWindow()
    {
        if (!OperatingSystem.IsWindows())
        {
            return false;
        }

        try
        {
            Span<uint> processIds = stackalloc uint[2];
            return GetConsoleProcessList(ref processIds[0], (uint)processIds.Length) == 1;
        }
        catch (Exception exception) when (exception is DllNotFoundException or EntryPointNotFoundException)
        {
            return false;
        }
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial uint GetConsoleProcessList(ref uint processIds, uint processCount);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial nint GetStdHandle(int standardHandle);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(nint handle, out uint mode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(nint handle, uint mode);
}
