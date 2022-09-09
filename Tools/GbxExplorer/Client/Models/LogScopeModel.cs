namespace GbxExplorer.Client.Models;

public class LogScopeModel : IDisposable
{
    private readonly MemoryLogger logger;

    public string Name { get; }
    public LogScopeModel? Parent { get; }

    public LogScopeModel(string name, LogScopeModel? parent, MemoryLogger logger)
    {
        Name = name;
        Parent = parent;
        this.logger = logger;
    }

    public override string ToString()
    {
        return Name;
    }

    public void Dispose()
    {
        logger.CurrentScope = Parent;
    }
}
