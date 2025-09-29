using GbxExplorerOld.Client.Sections.SectionEditor.OmniSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using OmniSharp.Models;
using OmniSharp.Models.SignatureHelp;
using OmniSharp.Models.v1.Completion;
using OmniSharp.Options;
namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class AutocompleteService
{
    private readonly RoslynProject completionProject;
    private readonly OmniSharpCompletionService completionService;
    private readonly OmniSharpSignatureHelpService signatureService;
    private readonly OmniSharpQuickInfoProvider quickInfoProvider;

    public AutocompleteService(List<MetadataReference> references, string code)
    {
        completionProject = new RoslynProject(references, code);

        var loggerFactory = LoggerFactory.Create(builder => {
            builder.SetMinimumLevel(LogLevel.Error);
        });
        var formattingOptions = new FormattingOptions();


        completionService = new OmniSharpCompletionService(completionProject.Workspace, formattingOptions, loggerFactory);
        signatureService = new OmniSharpSignatureHelpService(completionProject.Workspace);
        quickInfoProvider = new OmniSharpQuickInfoProvider(completionProject.Workspace, formattingOptions, loggerFactory);
    }

    public record Diagnostic
    {
        public LinePosition Start { get; init; }
        public LinePosition End { get; init; }
        public string Message { get; init; }
        public int Severity { get; init; }
    }

    public async Task<CompletionResponse> GetCompletionAsync(string code, CompletionRequest completionRequest)
    {
        Solution updatedSolution;

        do
        {
            updatedSolution = completionProject.Workspace.CurrentSolution.WithDocumentText(completionProject.DocumentId, SourceText.From(code));
        } while (!completionProject.Workspace.TryApplyChanges(updatedSolution));

        var document = updatedSolution.GetDocument(completionProject.DocumentId);
        var completionResponse = await completionService.Handle(completionRequest, document);
        
        return completionResponse;
    }

    public async Task<CompletionResolveResponse> GetCompletionResolveAsync(CompletionResolveRequest completionResolveRequest)
    {
        var document = completionProject.Workspace.CurrentSolution.GetDocument(completionProject.DocumentId);
        var completionResponse = await completionService.Handle(completionResolveRequest, document);

        return completionResponse;
    }

    public async Task<SignatureHelpResponse> GetSignatureHelpAsync(string code, SignatureHelpRequest signatureHelpRequest)
    {
        Solution updatedSolution;

        do
        {
            updatedSolution = completionProject.Workspace.CurrentSolution.WithDocumentText(completionProject.DocumentId, SourceText.From(code));
        } while (!completionProject.Workspace.TryApplyChanges(updatedSolution));

        var document = updatedSolution.GetDocument(completionProject.DocumentId);
        var signatureHelpResponse = await signatureService.Handle(signatureHelpRequest, document);

        return signatureHelpResponse;
    }

    public async Task<QuickInfoResponse> GetQuickInfoAsync(QuickInfoRequest quickInfoRequest)
    {
        var document = completionProject.Workspace.CurrentSolution.GetDocument(completionProject.DocumentId);
        var quickInfoResponse = await quickInfoProvider.Handle(quickInfoRequest, document);

        return quickInfoResponse;
    }

    public async Task<List<Diagnostic>> GetDiagnosticsAsync(string code)
    {
        Solution updatedSolution;

        do
        {
            updatedSolution = completionProject.Workspace.CurrentSolution.WithDocumentText(completionProject.DocumentId, SourceText.From(code));
        } while (!completionProject.Workspace.TryApplyChanges(updatedSolution));

        var compilation = await updatedSolution.Projects.First().GetCompilationAsync();
        var dotnetDiagnostics = compilation.GetDiagnostics();

        var diagnostics = dotnetDiagnostics.Select(current => {
            var lineSpan = current.Location.GetLineSpan();

            return new Diagnostic
            {
                Start = lineSpan.StartLinePosition,
                End = lineSpan.EndLinePosition,
                Message = current.GetMessage(),
                Severity = GetSeverity(current.Severity)
            };
        }).ToList();

        return diagnostics;
    }

    private int GetSeverity(DiagnosticSeverity severity)
    {
        return severity switch
        {
            DiagnosticSeverity.Hidden => 1,
            DiagnosticSeverity.Info => 2,
            DiagnosticSeverity.Warning => 4,
            DiagnosticSeverity.Error => 8,
            _ => throw new Exception("Unknown diagnostic severity.")
        };
    }
}
