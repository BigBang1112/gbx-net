/* 
This code is adapted from https://github.com/OmniSharp/omnisharp-vscode

MIT License

Copyright (c) .NET Foundation and Contributors
All Rights Reserved

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
*/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OmniSharp.Models;
using OmniSharp.Models.SignatureHelp;
using OmniSharp.Roslyn.CSharp.Services.Documentation;
namespace GbxExplorerOld.Client.Sections.SectionEditor.OmniSharp
{
    
    public class OmniSharpSignatureHelpService 
    {
        private readonly AdhocWorkspace _workspace;

        
        public OmniSharpSignatureHelpService(AdhocWorkspace workspace)
        {
            _workspace = workspace;
        }

        public async Task<SignatureHelpResponse> Handle(SignatureHelpRequest request, Document document2)
        {
            var invocations = new List<InvocationContext>();
            foreach (var document in new [] { document2 })
            {
                var invocation = await GetInvocation(document, request);
                if (invocation != null)
                {
                    invocations.Add(invocation);
                }
            }

            if (invocations.Count == 0)
            {
                return null;
            }

            var response = new SignatureHelpResponse();

            // define active parameter by position
            foreach (var comma in invocations.First().Separators)
            {
                if (comma.Span.Start > invocations.First().Position)
                {
                    break;
                }
                response.ActiveParameter += 1;
            }

            // process all signatures, define active signature by types
            var signaturesSet = new HashSet<SignatureHelpItem>();
            var bestScore = int.MinValue;
            SignatureHelpItem bestScoredItem = null;

            foreach (var invocation in invocations)
            {
                var types = invocation.ArgumentTypes;
                ISymbol throughSymbol = null;
                ISymbol throughType = null;
                var methodGroup = invocation.SemanticModel.GetMemberGroup(invocation.Receiver).OfType<IMethodSymbol>();
                if (invocation.Receiver is MemberAccessExpressionSyntax)
                {
                    var throughExpression = ((MemberAccessExpressionSyntax)invocation.Receiver).Expression;
                    throughSymbol = invocation.SemanticModel.GetSpeculativeSymbolInfo(invocation.Position, throughExpression, SpeculativeBindingOption.BindAsExpression).Symbol;
                    throughType = invocation.SemanticModel.GetSpeculativeTypeInfo(invocation.Position, throughExpression, SpeculativeBindingOption.BindAsTypeOrNamespace).Type;
                    var includeInstance = (throughSymbol != null && !(throughSymbol is ITypeSymbol)) ||
                        throughExpression is LiteralExpressionSyntax ||
                        throughExpression is TypeOfExpressionSyntax;
                    var includeStatic = (throughSymbol is INamedTypeSymbol) || throughType != null;
                    methodGroup = methodGroup.Where(m => (m.IsStatic && includeStatic) || (!m.IsStatic && includeInstance));
                }
                else if (invocation.Receiver is SimpleNameSyntax && invocation.IsInStaticContext)
                {
                    methodGroup = methodGroup.Where(m => m.IsStatic || m.MethodKind == MethodKind.LocalFunction);
                }

                foreach (var methodOverload in methodGroup)
                {
                    var signature = BuildSignature(methodOverload);
                    signaturesSet.Add(signature);

                    var score = InvocationScore(methodOverload, types);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestScoredItem = signature;
                    }
                }
            }

            var signaturesList = signaturesSet.ToList();
            response.Signatures = signaturesList;
            response.ActiveSignature = signaturesList.IndexOf(bestScoredItem);

            return response;
        }

        private async Task<InvocationContext> GetInvocation(Document document, Request request)
        {
            var sourceText = await document.GetTextAsync();
            var position = sourceText.GetTextPosition(request);
            var tree = await document.GetSyntaxTreeAsync();
            var root = await tree.GetRootAsync();
            var node = root.FindToken(position).Parent;

            // Walk up until we find a node that we're interested in.
            while (node != null)
            {
                if (node is InvocationExpressionSyntax invocation && invocation.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, invocation.Expression, invocation.ArgumentList, invocation.IsInStaticContext());
                }

                if (node is ObjectCreationExpressionSyntax objectCreation && objectCreation.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, objectCreation, objectCreation.ArgumentList, objectCreation.IsInStaticContext());
                }

                if (node is AttributeSyntax attributeSyntax && attributeSyntax.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, attributeSyntax, attributeSyntax.ArgumentList, attributeSyntax.IsInStaticContext());
                }

                node = node.Parent;
            }

            return null;
        }

        private int InvocationScore(IMethodSymbol symbol, IEnumerable<TypeInfo> types)
        {
            var parameters = symbol.Parameters;
            if (parameters.Count() < types.Count())
            {
                return int.MinValue;
            }

            var score = 0;
            var invocationEnum = types.GetEnumerator();
            var definitionEnum = parameters.GetEnumerator();
            while (invocationEnum.MoveNext() && definitionEnum.MoveNext())
            {
                if (invocationEnum.Current.ConvertedType == null)
                {
                    // 1 point for having a parameter
                    score += 1;
                }
                else if (SymbolEqualityComparer.Default.Equals(invocationEnum.Current.ConvertedType, definitionEnum.Current.Type))
                {
                    // 2 points for having a parameter and being
                    // the same type
                    score += 2;
                }
            }

            return score;
        }

        private static SignatureHelpItem BuildSignature(IMethodSymbol symbol)
        {
            var signature = new SignatureHelpItem();
            signature.Documentation = symbol.GetDocumentationCommentXml();
            signature.Name = symbol.MethodKind == MethodKind.Constructor ? symbol.ContainingType.Name : symbol.Name;
            signature.Label = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            signature.StructuredDocumentation = DocumentationConverter.GetStructuredDocumentation(symbol);

            signature.Parameters = symbol.Parameters.Select(parameter =>
            {
                return new SignatureHelpParameter()
                {
                    Name = parameter.Name,
                    Label = parameter.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                    Documentation = signature.StructuredDocumentation.GetParameterText(parameter.Name)
                };
            });

            return signature;
        }
    }
}
