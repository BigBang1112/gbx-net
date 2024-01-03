using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class GbxReaderWriterGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var namesAndContents = context.CompilationProvider.Select(GetTypeSymbols);

        context.RegisterSourceOutput(namesAndContents, GenerateSource);
    }

    private IEnumerable<(IMethodSymbol ReaderMethod, IMethodSymbol WriterMethod, bool IsNamed)> GetTypeSymbols(Compilation compilation, CancellationToken token)
    {
        var serializationNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "GBX")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "NET")
            .GetNamespaceMembers()
            .FirstOrDefault(x => x.Name == "Serialization");

        var reader = default(ITypeSymbol);
        var writer = default(ITypeSymbol);

        foreach (var typeSymbol in serializationNamespace.GetTypeMembers())
        {
            if (typeSymbol.Name == "IGbxReader")
            {
                reader = typeSymbol;
            }
            else if (typeSymbol.Name == "IGbxWriter")
            {
                writer = typeSymbol;
            }
        }

        if (reader is null || writer is null)
        {
            yield break;
        }

        var readerMethods = reader.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(x => !x.IsOverride && x.Name.StartsWith("Read"))
            .ToList();

        var writerMethods = writer.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(x => !x.IsOverride && x.Name.StartsWith("Write"))
            .ToList();

        foreach (var readerMethod in readerMethods)
        {
            var typeStr = readerMethod.Name.Substring("Read".Length); // Read>>Boolean<<

            foreach (var writerMethod in writerMethods)
            {
                var writerTypeStr = writerMethod.Name.Substring("Write".Length); // Write>>Format<< or Write>><<

                if (!string.IsNullOrEmpty(writerTypeStr))
                {
                    if (typeStr.Equals(writerTypeStr))
                    {
                        if (readerMethod.Parameters.Length == 0)
                        {
                            yield return (readerMethod, writerMethod, IsNamed: true);
                            break;
                        }
                        else if (readerMethod.TypeParameters.Length == writerMethod.TypeParameters.Length && readerMethod.Parameters.Length == writerMethod.Parameters.Length - 1)
                        {
                            for (int i = 0; i < readerMethod.Parameters.Length; i++)
                            {
                                var parameter = readerMethod.Parameters[i];
                                var parameterWriter = writerMethod.Parameters[i + 1];

                                if (parameter.Type.Name != parameterWriter.Type.Name)
                                {
                                    break;
                                }

                                if (i == readerMethod.Parameters.Length - 1)
                                {
                                    yield return (readerMethod, writerMethod, IsNamed: true);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (readerMethod.Parameters.IsEmpty && writerMethod.Parameters.Length > 0 && writerMethod.Parameters[0].Type.Name == typeStr)
                    {
                        yield return (readerMethod, writerMethod, IsNamed: false);
                        break;
                    }
                    else
                    {
                        if (readerMethod.TypeParameters.Length == writerMethod.TypeParameters.Length && readerMethod.Parameters.Length == writerMethod.Parameters.Length - 1)
                        {
                            for (int i = 0; i < readerMethod.Parameters.Length; i++)
                            {
                                var parameter = readerMethod.Parameters[i];
                                var parameterWriter = writerMethod.Parameters[i + 1];

                                if (parameter.Type.Name != parameterWriter.Type.Name)
                                {
                                    break;
                                }

                                if (i == readerMethod.Parameters.Length - 1)
                                {
                                    yield return (readerMethod, writerMethod, IsNamed: false);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerable<INamespaceSymbol> RecurseNamespaces(INamespaceSymbol namespaceSymbol)
    {
        yield return namespaceSymbol;

        foreach (var n in namespaceSymbol.GetNamespaceMembers())
        {
            foreach (var nn in RecurseNamespaces(n))
            {
                yield return nn;
            }
        }
    }

    private void GenerateSource(SourceProductionContext context, IEnumerable<(IMethodSymbol ReaderMethod, IMethodSymbol WriterMethod, bool IsNamed)> symbols)
    {
        var sb = new StringBuilder();
        sb.AppendLine("#if NET6_0_OR_GREATER");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("#endif");
        sb.AppendLine();
        sb.AppendLine("namespace GBX.NET.Serialization;");
        sb.AppendLine();

        var sbInterface = new StringBuilder();
        sbInterface.AppendLine("partial interface IGbxReaderWriter");
        sbInterface.AppendLine("{");

        var sbClass = new StringBuilder();
        sbClass.AppendLine("partial class GbxReaderWriter");
        sbClass.AppendLine("{");

        foreach (var (readerMethod, writerMethod, isNamed) in symbols)
        {
            var nullableReturn = !writerMethod.Parameters.IsEmpty
                && writerMethod.Parameters[0].NullableAnnotation == NullableAnnotation.Annotated
                && !writerMethod.Parameters[0].Type.IsValueType;

            var isNonNullableValueType = readerMethod.ReturnType.IsValueType
                && readerMethod.ReturnType.NullableAnnotation != NullableAnnotation.Annotated
                && !writerMethod.Parameters.IsEmpty;

            //var isArrayOrList = readerMethod.ReturnType is IArrayTypeSymbol or { Name: "IList" };

            // INTERFACE
            for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
            {
                var isNullableVariant = i == 1;

                sbInterface.Append("    ");
                sbInterface.Append(readerMethod.ReturnType);

                if (nullableReturn || isNullableVariant)
                {
                    sbInterface.Append('?');
                }

                sbInterface.Append(' ');

                if (isNamed)
                {
                    sbInterface.Append(readerMethod.Name.Substring("Read".Length));
                }
                else
                {
                    sbInterface.Append(readerMethod.ReturnType.Name);
                }

                if (readerMethod.TypeParameters.Length > 0)
                {
                    sbInterface.Append('<');

                    var firstType = true;

                    foreach (var typeParameter in readerMethod.TypeParameters)
                    {
                        if (!firstType)
                        {
                            sbInterface.Append(", ");
                        }

                        sbInterface.Append(typeParameter);

                        firstType = false;
                    }

                    sbInterface.Append('>');
                }

                sbInterface.Append('(');

                var first = true;

                foreach (var parameter in writerMethod.Parameters)
                {
                    if (!first)
                    {
                        sbInterface.Append(", ");
                    }

                    if (isNullableVariant && first)
                    {
                        sbInterface.Append(parameter.Type);
                        sbInterface.Append("? ");
                        sbInterface.Append(parameter.Name);
                    }
                    else
                    {
                        sbInterface.Append(parameter);
                    }

                    if (parameter.HasExplicitDefaultValue)
                    {
                        sbInterface.Append(" = ");
                        sbInterface.Append(parameter.ExplicitDefaultValue switch
                        {
                            bool b => b ? "true" : "false",
                            string s => $"\"{s}\"",
                            _ => parameter.ExplicitDefaultValue
                        });
                    }

                    first = false;
                }

                /*if (isArrayOrList)
                {
                    foreach (var p in readerMethod.Parameters)
                    {
                        if (!first)
                        {
                            sbInterface.Append(", ");
                        }

                        sbInterface.Append(p);

                        if (p.HasExplicitDefaultValue)
                        {
                            sbInterface.Append(" = ");
                            sbInterface.Append(p.ExplicitDefaultValue switch
                            {
                                bool b => b ? "true" : "false",
                                string s => $"\"{s}\"",
                                _ => p.ExplicitDefaultValue
                            });
                        }

                        first = false;
                    }
                }
                else*/
                if (isNullableVariant && !writerMethod.Parameters.IsEmpty)
                {
                    sbInterface.Append(", ");
                    sbInterface.Append(writerMethod.Parameters[0].Type);
                    sbInterface.Append(" defaultValue");
                }

                if (/*!isArrayOrList &&*/ writerMethod.Parameters.Length == 1)
                {
                    sbInterface.Append(" = default");
                }

                sbInterface.Append(')');
                AppendConstraints(sbInterface, readerMethod);
                sbInterface.AppendLine(";");
            }

            if (!writerMethod.Parameters.IsEmpty)
            {
                for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
                {
                    var isNullableVariant = i == 1;

                    sbInterface.Append("    void ");

                    if (isNamed)
                    {
                        sbInterface.Append(readerMethod.Name.Substring("Read".Length));
                    }
                    else
                    {
                        sbInterface.Append(readerMethod.ReturnType.Name);
                    }

                    if (readerMethod.TypeParameters.Length > 0)
                    {
                        sbInterface.Append('<');

                        var firstType = true;

                        foreach (var typeParameter in readerMethod.TypeParameters)
                        {
                            if (!firstType)
                            {
                                sbInterface.Append(", ");
                            }

                            sbInterface.Append(typeParameter);

                            firstType = false;
                        }

                        sbInterface.Append('>');
                    }

                    sbInterface.Append("(ref ");

                    var first = true;

                    foreach (var parameter in writerMethod.Parameters)
                    {
                        if (!first)
                        {
                            sbInterface.Append(", ");
                        }

                        if (isNullableVariant && first)
                        {
                            sbInterface.Append(parameter.Type);
                            sbInterface.Append("? ");
                            sbInterface.Append(parameter.Name);
                        }
                        else
                        {
                            sbInterface.Append(parameter);
                        }

                        if (parameter.HasExplicitDefaultValue)
                        {
                            sbInterface.Append(" = ");
                            sbInterface.Append(parameter.ExplicitDefaultValue switch
                            {
                                bool b => b ? "true" : "false",
                                string s => $"\"{s}\"",
                                _ => parameter.ExplicitDefaultValue
                            });
                        }

                        first = false;
                    }

                    /*if (isArrayOrList)
                    {
                        foreach (var p in readerMethod.Parameters)
                        {
                            if (!first)
                            {
                                sbInterface.Append(", ");
                            }

                            sbInterface.Append(p);

                            if (p.HasExplicitDefaultValue)
                            {
                                sbInterface.Append(" = ");
                                sbInterface.Append(p.ExplicitDefaultValue switch
                                {
                                    bool b => b ? "true" : "false",
                                    string s => $"\"{s}\"",
                                    _ => p.ExplicitDefaultValue
                                });
                            }

                            first = false;
                        }
                    }
                    else*/ if (isNullableVariant && !writerMethod.Parameters.IsEmpty)
                    {
                        sbInterface.Append(", ");
                        sbInterface.Append(writerMethod.Parameters[0].Type);
                        sbInterface.Append(" defaultValue = default");
                    }

                    sbInterface.Append(')');
                    AppendConstraints(sbInterface, readerMethod);
                    sbInterface.AppendLine(";");
                }
            }

            sbInterface.AppendLine();


            // CLASS
            for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
            {
                var isNullableVariant = i == 1;

                if (!writerMethod.Parameters.IsEmpty)
                {
                    sbClass.AppendLine("#if NET6_0_OR_GREATER");
                    sbClass.AppendLine("    [return: NotNullIfNotNull(nameof(value))]");
                    sbClass.AppendLine("#endif");
                }

                sbClass.Append("    public ");
                sbClass.Append(readerMethod.ReturnType);

                if (nullableReturn || isNullableVariant)
                {
                    sbClass.Append('?');
                }

                sbClass.Append(' ');

                if (isNamed)
                {
                    sbClass.Append(readerMethod.Name.Substring("Read".Length));
                }
                else
                {
                    sbClass.Append(readerMethod.ReturnType.Name);
                }

                if (readerMethod.TypeParameters.Length > 0)
                {
                    sbClass.Append('<');

                    var firstType = true;

                    foreach (var typeParameter in readerMethod.TypeParameters)
                    {
                        if (!firstType)
                        {
                            sbClass.Append(", ");
                        }

                        sbClass.Append(typeParameter);

                        firstType = false;
                    }

                    sbClass.Append('>');
                }

                sbClass.Append('(');

                var first = true;

                foreach (var parameter in writerMethod.Parameters)
                {
                    if (!first)
                    {
                        sbClass.Append(", ");
                    }

                    if (isNullableVariant && first)
                    {
                        sbClass.Append(parameter.Type);
                        sbClass.Append("? ");
                        sbClass.Append(parameter.Name);
                    }
                    else
                    {
                        sbClass.Append(parameter);
                    }

                    if (parameter.HasExplicitDefaultValue)
                    {
                        sbClass.Append(" = ");
                        sbClass.Append(parameter.ExplicitDefaultValue switch
                        {
                            bool b => b ? "true" : "false",
                            string s => $"\"{s}\"",
                            _ => parameter.ExplicitDefaultValue
                        });
                    }

                    first = false;
                }

                /*if (isArrayOrList)
                {
                    foreach (var p in readerMethod.Parameters)
                    {
                        if (!first)
                        {
                            sbClass.Append(", ");
                        }

                        sbClass.Append(p);

                        if (p.HasExplicitDefaultValue)
                        {
                            sbClass.Append(" = ");
                            sbClass.Append(p.ExplicitDefaultValue switch
                            {
                                bool b => b ? "true" : "false",
                                string s => $"\"{s}\"",
                                _ => p.ExplicitDefaultValue
                            });
                        }

                        first = false;
                    }
                }
                else*/ if (isNullableVariant && !writerMethod.Parameters.IsEmpty)
                {
                    sbClass.Append(", ");
                    sbClass.Append(writerMethod.Parameters[0].Type);
                    sbClass.Append(" defaultValue");
                }

                if (/*!isArrayOrList &&*/ writerMethod.Parameters.Length == 1)
                {
                    sbClass.Append(" = default");
                }

                sbClass.Append(')');
                AppendConstraints(sbClass, readerMethod);
                sbClass.AppendLine();

                sbClass.AppendLine("    {");

                if (writerMethod.Parameters.IsEmpty)
                {
                    sbClass.Append("        ");
                    sbClass.Append(readerMethod.ReturnType);
                    sbClass.AppendLine(" value = default;");
                }

                // Reader
                sbClass.Append("        if (Reader is not null) ");
                sbClass.Append(writerMethod.Parameters.IsEmpty ? "value" : writerMethod.Parameters[0].Name);
                sbClass.Append(" = Reader.");
                sbClass.Append(readerMethod.Name);

                if (readerMethod.TypeParameters.Length > 0)
                {
                    sbClass.Append('<');

                    var firstType = true;

                    foreach (var typeParameter in readerMethod.TypeParameters)
                    {
                        if (!firstType)
                        {
                            sbClass.Append(", ");
                        }

                        sbClass.Append(typeParameter);

                        firstType = false;
                    }

                    sbClass.Append('>');
                }

                sbClass.Append('(');

                var firstReader = true;

                foreach (var parameter in writerMethod.Parameters.Skip(1))
                {
                    if (!firstReader)
                    {
                        sbClass.Append(", ");
                    }

                    sbClass.Append(parameter.Name);

                    firstReader = false;
                }

                /*if (isArrayOrList)
                {
                    foreach (var p in readerMethod.Parameters)
                    {
                        if (!firstReader)
                        {
                            sbClass.Append(", ");
                        }

                        sbClass.Append(p.Name);

                        firstReader = false;
                    }
                }*/

                sbClass.AppendLine(");");

                // Writer
                sbClass.Append("        Writer?.");
                sbClass.Append(writerMethod.Name);
                sbClass.Append('(');

                var firstWriter = true;

                foreach (var parameter in writerMethod.Parameters)
                {
                    if (!firstWriter)
                    {
                        sbClass.Append(", ");
                    }

                    sbClass.Append(parameter.Name);

                    if (isNullableVariant && firstWriter)
                    {
                        sbClass.Append(".GetValueOrDefault(defaultValue)");
                    }

                    firstWriter = false;
                }

                sbClass.AppendLine(");");
                sbClass.Append("        return ");
                sbClass.Append(writerMethod.Parameters.IsEmpty ? "value" : writerMethod.Parameters[0].Name);
                sbClass.AppendLine(";");

                sbClass.AppendLine("    }");

                sbClass.AppendLine();
            }

            if (!writerMethod.Parameters.IsEmpty)
            {
                for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
                {
                    var isNullableVariant = i == 1;

                    sbClass.Append("    public void ");

                    if (isNamed)
                    {
                        sbClass.Append(readerMethod.Name.Substring("Read".Length));
                    }
                    else
                    {
                        sbClass.Append(readerMethod.ReturnType.Name);
                    }

                    if (readerMethod.TypeParameters.Length > 0)
                    {
                        sbClass.Append('<');

                        var firstType = true;

                        foreach (var typeParameter in readerMethod.TypeParameters)
                        {
                            if (!firstType)
                            {
                                sbClass.Append(", ");
                            }

                            sbClass.Append(typeParameter);

                            firstType = false;
                        }

                        sbClass.Append('>');
                    }

                    sbClass.AppendLine("(");
                    sbClass.AppendLine("#if NET6_0_OR_GREATER");
                    sbClass.AppendLine("        [NotNullIfNotNull(nameof(value))]");
                    sbClass.AppendLine("#endif");
                    sbClass.Append("        ref ");

                    var first = true;

                    foreach (var parameter in writerMethod.Parameters)
                    {
                        if (!first)
                        {
                            sbClass.Append(", ");
                        }

                        if (isNullableVariant && first)
                        {
                            sbClass.Append(parameter.Type);
                            sbClass.Append("? ");
                            sbClass.Append(parameter.Name);
                        }
                        else
                        {
                            sbClass.Append(parameter);
                        }

                        if (parameter.HasExplicitDefaultValue)
                        {
                            sbClass.Append(" = ");
                            sbClass.Append(parameter.ExplicitDefaultValue switch
                            {
                                bool b => b ? "true" : "false",
                                string s => $"\"{s}\"",
                                _ => parameter.ExplicitDefaultValue
                            });
                        }

                        first = false;
                    }

                    /*if (isArrayOrList)
                    {
                        foreach (var p in readerMethod.Parameters)
                        {
                            if (!first)
                            {
                                sbClass.Append(", ");
                            }

                            sbClass.Append(p);

                            if (p.HasExplicitDefaultValue)
                            {
                                sbClass.Append(" = ");
                                sbClass.Append(p.ExplicitDefaultValue switch
                                {
                                    bool b => b ? "true" : "false",
                                    string s => $"\"{s}\"",
                                    _ => p.ExplicitDefaultValue
                                });
                            }

                            first = false;
                        }
                    }
                    else*/ if (isNullableVariant && !writerMethod.Parameters.IsEmpty)
                    {
                        sbClass.Append(", ");
                        sbClass.Append(writerMethod.Parameters[0].Type);
                        sbClass.Append(" defaultValue = default");
                    }

                    sbClass.Append(')');
                    AppendConstraints(sbClass, readerMethod);
                    sbClass.Append(" => ");
                    sbClass.Append(writerMethod.Parameters[0].Name);
                    sbClass.Append(" = ");
                    sbClass.Append(readerMethod.Name.Substring("Read".Length));
                    sbClass.Append('(');

                    var first3Class = true;

                    foreach (var parameter in writerMethod.Parameters)
                    {
                        if (!first3Class)
                        {
                            sbClass.Append(", ");
                        }

                        sbClass.Append(parameter.Name);

                        first3Class = false;
                    }

                    if (isNullableVariant)
                    {
                        sbClass.Append(", defaultValue");
                    }

                    sbClass.AppendLine(");");
                    sbClass.AppendLine();
                }
            }
        }

        sbInterface.AppendLine("}");
        sbClass.AppendLine("}");

        sb.Append(sbInterface.ToString());
        sb.AppendLine();
        sb.Append(sbClass.ToString());
        context.AddSource("GbxReaderWriter", sb.ToString());
    }

    private static void AppendConstraints(StringBuilder sb, IMethodSymbol methodSymbol)
    {
        if (methodSymbol.TypeParameters.Length <= 0)
        {
            return;
        }

        foreach (var typeParameter in methodSymbol.TypeParameters)
        {
            if (!typeParameter.HasValueTypeConstraint && typeParameter.ConstraintTypes.IsDefaultOrEmpty)
            {
                continue;
            }

            sb.Append(" where ");
            sb.Append(typeParameter);
            sb.Append(" : ");

            if (typeParameter.HasValueTypeConstraint)
            {
                sb.Append("struct");
            }
            else
            {
                var firstType = true;

                foreach (var constraintType in typeParameter.ConstraintTypes)
                {
                    if (!firstType)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(constraintType);

                    firstType = false;
                }
            }
        }
    }
}