using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class GbxReaderWriterGenerator : IIncrementalGenerator
{
    private const bool Debug = false;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        var readAndWriteSymbols = context.CompilationProvider.Select(GetReaderAndWriterMethods);

        context.RegisterSourceOutput(readAndWriteSymbols, GenerateReaderWriterSource);
    }

    private IEnumerable<(IMethodSymbol ReaderMethod, IMethodSymbol WriterMethod, bool IsNamed)> GetReaderAndWriterMethods(Compilation compilation, CancellationToken token)
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
            .ToImmutableList();

        var writerMethods = writer.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(x => !x.IsOverride && x.Name.StartsWith("Write"))
            .ToImmutableList();

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

    private void GenerateReaderWriterSource(SourceProductionContext context, IEnumerable<(IMethodSymbol ReaderMethod, IMethodSymbol WriterMethod, bool IsNamed)> symbols)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
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
            var isNonNullableValueType = readerMethod.ReturnType.IsValueType
                && readerMethod.ReturnType.NullableAnnotation != NullableAnnotation.Annotated
                && !writerMethod.Parameters.IsEmpty;

            var methodType = readerMethod.Name.Substring("Read".Length);

            //var isArrayOrList = readerMethod.ReturnType is IArrayTypeSymbol or { Name: "IList" };

            // INTERFACE
            for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
            {
                var isNullableVariant = i == 1;

                if (!writerMethod.Parameters.IsEmpty)
                {
                    sbInterface.AppendLine("    [return: NotNullIfNotNull(nameof(value))]");
                }

                sbInterface.Append("    ");
                sbInterface.Append(writerMethod.Parameters.Length > 0 && writerMethod.Parameters[0].Type.Name == readerMethod.ReturnType.Name
                    ? writerMethod.Parameters[0].Type : readerMethod.ReturnType);

                if (isNullableVariant)
                {
                    sbInterface.Append('?');
                }

                sbInterface.Append(' ');

                if (isNamed)
                {
                    sbInterface.Append(methodType);
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
                        sbInterface.Append(methodType);
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


                    sbInterface.Append("([NotNullIfNotNull(nameof(value))] ref ");

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

            var isForArrayGenerator = !readerMethod.ReturnType.IsValueType
                && readerMethod.Parameters.IsDefaultOrEmpty
                && readerMethod.TypeParameters.IsDefaultOrEmpty
                && !string.IsNullOrEmpty(readerMethod.ReturnType.Name)
                && methodType.StartsWith(readerMethod.ReturnType.Name);

            if (isForArrayGenerator)
            {
                for (var i = 0; i < 2; i++)
                {
                    var isRef = i == 1;

                    for (var j = 0; j < 2; j++)
                    {
                        var isList = j == 1;

                        for (var k = 0; k < 3; k++)
                        {
                            sbInterface.Append("    ");

                            if (isRef)
                            {
                                sbInterface.Append("void ");
                            }
                            else
                            {
                                if (isList)
                                {
                                    sbInterface.Append("IList<");
                                }

                                sbInterface.Append(readerMethod.ReturnType);

                                if (isList)
                                {
                                    sbInterface.Append(">? ");
                                }
                                else
                                {
                                    sbInterface.Append("[]? ");
                                }
                            }

                            if (isList)
                            {
                                sbInterface.Append("List");
                            }
                            else
                            {
                                sbInterface.Append("Array");
                            }

                            sbInterface.Append(methodType);

                            if (k == 2) // deprec variant
                            {
                                sbInterface.Append("_deprec");
                            }

                            sbInterface.Append('(');

                            if (isRef)
                            {
                                sbInterface.Append("ref ");
                            }

                            if (isList)
                            {
                                sbInterface.Append("IList<");
                                sbInterface.Append(readerMethod.ReturnType);
                                sbInterface.Append(">?");
                            }
                            else
                            {
                                sbInterface.Append(readerMethod.ReturnType);
                                sbInterface.Append("[]?");
                            }

                            sbInterface.Append(" value");

                            if (k == 0) // default variant
                            {
                                if (!isRef)
                                {
                                    sbInterface.Append(" = default");
                                }
                            }
                            else if (k == 1) // length variant
                            { 
                                sbInterface.Append(", int length");
                            }

                            sbInterface.AppendLine(");");
                        }
                    }
                }
            }


            sbInterface.AppendLine();

            // CLASS
            for (var i = 0; i < (isNonNullableValueType ? 2 : 1); i++)
            {
                var isNullableVariant = i == 1;

                if (!writerMethod.Parameters.IsEmpty)
                {
                    sbClass.AppendLine("    [return: NotNullIfNotNull(nameof(value))]");
                }

                sbClass.Append("    public ");
                sbClass.Append(writerMethod.Parameters.Length > 0 && writerMethod.Parameters[0].Type.Name == readerMethod.ReturnType.Name
                    ? writerMethod.Parameters[0].Type : readerMethod.ReturnType);

                if (isNullableVariant)
                {
                    sbClass.Append('?');
                }

                sbClass.Append(' ');

                if (isNamed)
                {
                    sbClass.Append(methodType);
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
                        sbClass.Append(methodType);
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

                    sbClass.Append("([NotNullIfNotNull(nameof(value))] ref ");

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
                    sbClass.Append(methodType);
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

            if (isForArrayGenerator)
            {
                for (var i = 0; i < 2; i++)
                {
                    var isList = i == 1;

                    for (var j = 0; j < 3; j++)
                    {
                        var isLengthVariant = j == 1;
                        var isDeprecVariant = j == 2;

                        if (!writerMethod.Parameters.IsEmpty)
                        {
                            sbClass.AppendLine("    [return: NotNullIfNotNull(nameof(value))]");
                        }

                        sbClass.Append("    public ");

                        if (isList)
                        {
                            sbClass.Append("IList<");
                        }

                        sbClass.Append(readerMethod.ReturnType);
                        sbClass.Append(isList ? ">? List" : "[]? Array");
                        sbClass.Append(methodType);

                        if (isDeprecVariant)
                        {
                            sbClass.Append("_deprec");
                        }

                        sbClass.Append('(');

                        if (isList)
                        {
                            sbClass.Append("IList<");
                        }

                        sbClass.Append(readerMethod.ReturnType);
                        sbClass.Append(isList ? ">?" : "[]?");
                        sbClass.Append(" value");
                        sbClass.Append(isLengthVariant ? ", int length" : " = default");
                        sbClass.AppendLine(")");
                        sbClass.AppendLine("    {");

                        // Reader
                        sbClass.Append("        if (Reader is not null) ");
                        sbClass.Append(writerMethod.Parameters.IsEmpty ? "value" : writerMethod.Parameters[0].Name);
                        sbClass.Append(" = Reader.Read");
                        sbClass.Append(isList ? "List" : "Array");
                        sbClass.Append(methodType);

                        if (isDeprecVariant)
                        {
                            sbClass.Append("_deprec");
                        }

                        sbClass.Append('(');

                        if (isLengthVariant)
                        {
                            sbClass.Append("length");
                        }

                        sbClass.AppendLine(");");

                        // Writer
                        sbClass.Append("        Writer?.Write");
                        sbClass.Append(isList ? "List" : "Array");
                        sbClass.Append(methodType);

                        if (isDeprecVariant)
                        {
                            sbClass.Append("_deprec");
                        }

                        sbClass.Append("(value");

                        if (isLengthVariant)
                        {
                            sbClass.Append(", length");
                        }

                        sbClass.AppendLine(");");
                        sbClass.AppendLine("        return value;");

                        sbClass.AppendLine("    }");

                        sbClass.AppendLine();
                    }
                }

                for (var i = 0; i < 2; i++)
                {
                    var isList = i == 1;

                    for (var j = 0; j < 3; j++)
                    {
                        var isLengthVariant = j == 1;
                        var isDeprecVariant = j == 2;

                        sbClass.Append("    public void ");
                        sbClass.Append(isList ? "List" : "Array");
                        sbClass.Append(methodType);

                        if (isDeprecVariant)
                        {
                            sbClass.Append("_deprec");
                        }

                        sbClass.Append("([NotNullIfNotNull(nameof(value))] ref ");

                        if (isList)
                        {
                            sbClass.Append("IList<");
                        }

                        sbClass.Append(readerMethod.ReturnType);
                        sbClass.Append(isList ? ">?" : "[]?");
                        sbClass.Append(" value");

                        if (isLengthVariant)
                        {
                            sbClass.Append(", int length");
                        }

                        sbClass.Append(") => value = ");
                        sbClass.Append(isList ? "List" : "Array");
                        sbClass.Append(methodType);

                        if (isDeprecVariant)
                        {
                            sbClass.Append("_deprec");
                        }

                        sbClass.Append("(value");

                        if (isLengthVariant)
                        {
                            sbClass.Append(", length");
                        }

                        sbClass.AppendLine(");");
                        sbClass.AppendLine();
                    }
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
            if (!typeParameter.HasValueTypeConstraint && typeParameter.ConstraintTypes.IsDefaultOrEmpty && !typeParameter.HasConstructorConstraint)
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

            if (typeParameter.HasConstructorConstraint)
            {
                sb.Append(", new()");
            }
        }
    }
}