using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class RoslynProject
{
    public AdhocWorkspace Workspace { get; init; }
    private Document UseOnlyOnceDocument { get; init; }
    public DocumentId DocumentId { get; init; }
    
    public RoslynProject(IEnumerable<MetadataReference> references, string code)
    {
        Workspace = new AdhocWorkspace();
        var projectInfo = ProjectInfo
            .Create(ProjectId.CreateNewId(), VersionStamp.Create(), "UserCode", "UserCode", LanguageNames.CSharp)
            .WithMetadataReferences(references)
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var project = Workspace.AddProject(projectInfo);
        
        UseOnlyOnceDocument = Workspace.AddDocument(project.Id, "Code.cs", SourceText.From(code));
        DocumentId = UseOnlyOnceDocument.Id;
    }

    

}