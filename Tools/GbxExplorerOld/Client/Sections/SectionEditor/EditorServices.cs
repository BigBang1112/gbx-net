using GbxExplorerOld.Client.Services;
namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class EditorServices(IGbxService gbxService, IDownloadStreamService downloadStreamService)
{
    public IGbxService GbxService = gbxService;
    public IDownloadStreamService DownloadStreamService = downloadStreamService;
}
