using GbxExplorer.Client.Models;

namespace GbxExplorer.Client;

public class MemoryLog
{
    public event Action<LogMessageModel>? OnChange;

    public List<LogMessageModel> Logs { get; } = new();

    public void Log(LogMessageModel log)
    {
        Logs.Add(log);
        OnChange?.Invoke(log);
    }
}
