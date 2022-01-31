namespace GBX.NET;

public interface IState
{
    Guid? StateGuid { get; internal set; }
}
