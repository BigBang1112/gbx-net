namespace GbxExplorerOld.Client.Services;

public interface IValueRendererService
{
    Type this[Type? type] { get; }
}