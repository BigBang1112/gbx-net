namespace GBX.NET;

public interface IStateRefTable : IState
{
    string? FileName { get; internal set; }
}
