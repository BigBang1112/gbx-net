namespace GbxExplorer.Client.Services
{
    public interface IValuePreviewService
    {
        Type this[Type? type] { get; }
    }
}