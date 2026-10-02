using ChunkL;
using ChunkL.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class SerializationWriter
{
    private readonly CodeWriter code;
    private readonly LayoutModel layout;
    private readonly ScopeModel scope;
    private readonly IReadOnlyDictionary<string, LayoutModel> layouts;
    private readonly SerializationMode mode;
    private readonly bool chunk;

    private int loopCount;
    private readonly Stack<HashSet<string>> locals = new();

    public SerializationWriter(CodeWriter code, LayoutModel layout, ScopeModel scope,
        IReadOnlyDictionary<string, LayoutModel> layouts, SerializationMode mode, bool chunk)
    {
        this.code = code;
        this.layout = layout;
        this.scope = scope;
        this.layouts = layouts;
        this.mode = mode;
        this.chunk = chunk;
    }

    public void Write(IEnumerable<IBodyStatement> statements)
    {
        locals.Push(new HashSet<string>(StringComparer.Ordinal));
        foreach (var statement in statements)
        {
            switch (statement)
            {
                case FieldDeclaration field:
                    Field(field);
                    break;

                case VersionCondition version:
                    code.BlankLine();
                    var archiveVersion = LayoutModel.Has(version.Attributes, "archive");
                    var chunkVersion = LayoutModel.Has(version.Attributes, "chunk");
                    if (archiveVersion && chunkVersion)
                        throw new InvalidOperationException("A version condition cannot select both archive and chunk versions.");
                    if (!chunk && scope.HasVersion && !archiveVersion && !chunkVersion)
                        throw new InvalidOperationException("A version condition with archive and inherited versions must select (archive) or (chunk).");
                    if (chunk && archiveVersion)
                        throw new InvalidOperationException("A chunk version condition cannot select an archive version.");
                    if (archiveVersion && !scope.HasVersion)
                        throw new InvalidOperationException("An (archive) version condition requires a version field in that archive.");

                    var variable = chunk || archiveVersion || (scope.HasVersion && !chunkVersion) ? "Version" : "v";
                    var condition = version.Kind switch
                    {
                        VersionConditionKind.Exact => $"{variable} == {version.Version}",
                        VersionConditionKind.LessOrEqual => $"{variable} <= {version.Version}",
                        VersionConditionKind.Range => $"{variable} >= {version.Version} && {variable} <= {version.VersionEnd}",
                        _ => $"{variable} >= {version.Version}"
                    };
                    code.Open("if (" + condition + ")");
                    Write(version.Body);
                    code.Close();
                    code.BlankLine();
                    break;

                case IfStatement conditional:
                    code.BlankLine();
                    code.Open("if (" + Expression(conditional.Condition) + ")");
                    Write(conditional.Body);
                    code.Close();

                    foreach (var alternative in conditional.ElseIfs)
                    {
                        code.Open("else if (" + Expression(alternative.Condition) + ")");
                        Write(alternative.Body);
                        code.Close();
                    }

                    if (conditional.Else is not null)
                    {
                        code.Open("else");
                        Write(conditional.Else.Body);
                        code.Close();
                    }

                    code.BlankLine();
                    break;

                case BlockStatement block:
                    code.BlankLine();
                    var encapsulated = LayoutModel.Has(block.Attributes, "encapsulated");
                    var io = mode switch
                    {
                        SerializationMode.Read => "r",
                        SerializationMode.Write => "w",
                        _ => "rw"
                    };
                    var method = mode switch
                    {
                        SerializationMode.Read => "ReadEncapsulated",
                        SerializationMode.Write => "WriteEncapsulated",
                        _ => "Encapsulated"
                    };
                    code.Open(encapsulated ? $"{io}.{method}({io} =>" : "");
                    Write(block.Body);
                    code.Close(encapsulated ? ");" : "");
                    code.BlankLine();
                    break;

                case WhileStatement loop:
                    code.BlankLine();
                    code.Open("while (" + Expression(loop.Condition) + ")");
                    Write(loop.Body);
                    code.Close();
                    code.BlankLine();
                    break;

                case LoopStatement loop:
                    code.BlankLine();
                    var index = "i" + ++loopCount;

                    code.Open($"for (var {index} = 0; {index} < {Expression(loop.CountExpression)}; {index}++)");
                    Write(loop.Body);
                    code.Close();
                    code.BlankLine();
                    break;

                case SwitchStatement selection:
                    code.BlankLine();
                    code.Open("switch (" + Expression(selection.Expression) + ")");

                    foreach (var branch in selection.Cases)
                    {
                        code.Line("case " + Expression(branch.Value) + ":");
                        code.Indent++;
                        var hasLocals = branch.Body.OfType<FieldDeclaration>().Any(static x => LayoutModel.Has(x.Attributes, "local"));
                        if (hasLocals) code.Open("");
                        Write(branch.Body);
                        if (!EndsWithExit(branch.Body)) code.Line("break;");
                        if (hasLocals) code.Close();
                        code.Indent--;
                    }

                    if (selection.Default is not null)
                    {
                        code.Line("default:");
                        code.Indent++;
                        var hasLocals = selection.Default.Body.OfType<FieldDeclaration>().Any(static x => LayoutModel.Has(x.Attributes, "local"));
                        if (hasLocals) code.Open("");
                        Write(selection.Default.Body);
                        if (!EndsWithExit(selection.Default.Body)) code.Line("break;");
                        if (hasLocals) code.Close();
                        code.Indent--;
                    }

                    code.Close();
                    code.BlankLine();
                    break;

                case ReturnStatement:
                    code.Line("return;");
                    break;

                case ThrowStatement thrown:
                    Throw(thrown.Attributes);
                    break;

                case ComputedAssignment assignment:
                    code.Line(Identifier(assignment.TargetName) + " = " + Expression(assignment.Expression) + ";");
                    break;

                case SkipStatement skip:
                    code.Line(mode == SerializationMode.Read ? $"r.BaseStream.Position += {Expression(skip.Expression)};" :
                        mode == SerializationMode.Write ? $"w.Write(new byte[{Expression(skip.Expression)}]);" :
                        $"rw.Data(new byte[{Expression(skip.Expression)}], {Expression(skip.Expression)});");
                    break;

                case AssertStatement assertion:
                    code.Line($"if (!({Expression(assertion.Condition)})) throw new InvalidDataException();");
                    break;

                default:
                    throw new NotSupportedException("Unsupported ChunkL statement: " + statement.GetType().Name);
            }
        }
        locals.Pop();
    }

    private static bool EndsWithExit(IEnumerable<IBodyStatement> statements) => statements.LastOrDefault() is
        ReturnStatement or ThrowStatement or FieldDeclaration { IsSpecialKeyword: true, Type.Name: "return" or "throw" };

    private void Field(FieldDeclaration declaration)
    {
        if (declaration.IsSpecialKeyword && declaration.Type.Name is not ("version" or "versionb"))
        {
            if (declaration.Type.Name == "base")
            {
                var baseMethod = mode.ToString();
                var io = mode switch
                {
                    SerializationMode.Read => "r",
                    SerializationMode.Write => "w",
                    _ => "rw"
                };

                code.Line(chunk ? $"base.{baseMethod}(n, {io});" :
                    $"base.{baseMethod}({io}, {(LayoutModel.Has(scope.Attributes, "contextual") ? "n, " : "")}v);");
            }
            else if (declaration.Type.Name == "return")
            {
                code.Line("return;");
            }
            else if (declaration.Type.Name == "throw")
            {
                Throw(declaration.Attributes);
            }
            else
            {
                throw new NotSupportedException("Unsupported ChunkL keyword: " + declaration.Type.Name);
            }

            return;
        }

        var field = scope.Occurrences[declaration];
        var write = LayoutModel.WriteExpression(declaration.Attributes);
        if (field.IsVersion)
        {
            code.Line(mode switch
            {
                SerializationMode.Read => $"Version = r.Read{(declaration.Type.Name == "versionb" ? "Byte" : "Int32")}();",
                SerializationMode.Write => $"w.Write({(declaration.Type.Name == "versionb" ? "(byte)" : "")}{(write is null ? "Version" : "(" + ExpressionText(write) + ")")});",
                SerializationMode.ReadWrite when write is not null => $"Version = rw.{(declaration.Type.Name == "versionb" ? "Byte" : "Int32")}({WriteArgument(write)});",
                _ => $"rw.Version{(declaration.Type.Name == "versionb" ? "Byte" : "Int32")}(this);"
            });

            return;
        }

        var owner = chunk && !field.IsUnknown ? layout.Scope : scope;
        var prefix = field.IsLocal ? "" : chunk ? field.IsUnknown ? "" : "n." : "this.";
        var backing = chunk && field.IsUnknown ? SyntaxOverlap.Escape(field.Name) : SyntaxOverlap.Backing(field.Name);
        var property = field.IsLocal ? SyntaxOverlap.Backing(field.Name) : SyntaxOverlap.Escape(field.Name);
        var hasBacking = !field.IsLocal && !field.Occurrences.Any(static x => LayoutModel.Has(x.Attributes, "inherited")) &&
            (!SyntaxOverlap.Has(owner.Existing, field.Name) || SyntaxOverlap.Has(owner.Existing, backing.TrimStart('@')));
        var target = prefix + (hasBacking ? backing : property);
        var storageType = (field.IsLocal ? null : SyntaxOverlap.MemberType(owner.Existing, (hasBacking ? backing : property).TrimStart('@'))) ??
            WireTypes.CSharp(field.Declaration);
        if (field.IsLocal && WireTypes.Nullable(field))
        {
            storageType += "?";
        }
        var external = LayoutModel.Has(declaration.Attributes, "external") && declaration.Type.ArrayDimensions == 0;

        if (field.IsLocal && external)
        {
            code.Line($"Components.GbxRefTableFile? {property.TrimStart('@')}File = null;");
        }

        var method = Method(declaration, storageType, mode);
        var arguments = new List<string>();

        if (layout.Archives.TryGetValue(declaration.Type.Name, out var archive) && LayoutModel.Has(archive.Attributes, "contextual"))
        {
            arguments.Add("n");
        }

        if (declaration.Type.FixedArrayCount is { Length: > 0 } length)
        {
            arguments.Add(ExpressionText(length, nullSafeCount: true));
        }

        if (LayoutModel.Attribute(declaration.Attributes, "version") is string version)
        {
            arguments.Add("version: " + ExpressionText(version));
        }
        else if (IsArchive(declaration) && (chunk ? scope.HasVersion : true))
        {
            arguments.Add("version: " + (scope.HasVersion ? "Version" : "v"));
        }

        if (declaration.Type.Name == "boolbyte")
        {
            arguments.Add("asByte: true");
        }

        if (declaration.Type.Name == "booltext")
        {
            arguments.Add("type: BoolType.Text");
        }

        if (LayoutModel.Attribute(declaration.Attributes, "prefix") == "byte")
        {
            arguments.Add("byteLengthPrefix: true");
        }

        if (external)
        {
            arguments.Add((mode == SerializationMode.Read ? "out " : mode == SerializationMode.ReadWrite ? "ref " : "") + prefix + (field.IsLocal ? property : backing).TrimStart('@') + "File");
        }

        var argumentSuffix = arguments.Count == 0 ? "" : ", " + string.Join(", ", arguments);

        if (mode == SerializationMode.Read)
        {
            var readMethod = method;
            var cast = declaration.Type.CastTarget is not null ? "(" + WireTypes.Cast(declaration.Type) + ")" :
                SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(WireTypes.CSharp(declaration)) &&
                    WireTypes.Value(declaration.Type.Name) && declaration.Type.ArrayDimensions == 0 ? "(" + storageType + ")" : "";

            code.Line($"{(field.IsLocal ? "var " : "")}{target} = {cast}r.Read{readMethod}({string.Join(", ", arguments)});");
        }
        else if (mode == SerializationMode.Write)
        {
            var value = write is null ? target : "(" + ExpressionText(write) + ")";
            if (field.IsLocal)
            {
                code.Line($"{storageType} {target} = {value};");
                value = target;
            }
            var writeMethod = method == "Id" ? "IdAsString" : method;

            if (declaration.Type.ArrayDimensions == 1 && declaration.Type.Name is "string" or "ident" or "meta" or "packdesc" or "fileref")
            {
                writeMethod = (LayoutModel.Has(declaration.Attributes, "list") ? "List" : "Array") + (LayoutModel.Has(declaration.Attributes, "deprec") ? "_deprec" : "");
            }

            if (declaration.Type.CastTarget is not null && declaration.Type.ArrayDimensions == 0)
            {
                writeMethod = WireTypes.Method(declaration.Type.Name);
            }

            var cast = declaration.Type.CastTarget is not null ||
                (WireTypes.Value(declaration.Type.Name) && declaration.Type.ArrayDimensions == 0 && SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(WireTypes.CSharp(declaration)))
                ? "(" + WireTypes.Map(declaration.Type.Name) + ")" : "";

            if (declaration.Type.ArrayDimensions == 0 && WireTypes.Primitive(declaration.Type.Name) &&
                declaration.Type.Name is not ("id" or "lookbackstring" or "data" or "optimizedint" or "vec3_6" or "filetime" or "systemtime" or "unixtime" or "timeofday"))
            {
                writeMethod = "";
            }

            code.Line($"w.Write{writeMethod}({cast}{value}{argumentSuffix});");
        }
        else
        {
            if (declaration.Type.CastTarget is not null)
            {
                method = "Enum" + WireTypes.Method(declaration.Type.Name) + "<" + WireTypes.Cast(declaration.Type) + ">";
            }

            var wireType = WireTypes.Map(declaration.Type.Name);
            var numeric = SyntaxOverlap.Normalize(storageType) is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint" or "long" or "ulong" or "float" or "double";
            var conversion = numeric && declaration.Type.ArrayDimensions == 0 && SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(wireType);
            var value = write is null ? target : WriteArgument(write);
            var assignment = (field.IsLocal ? "var " : "") + target;

            if (conversion)
            {
                code.Line($"{assignment} = ({storageType})rw.{method}(({wireType}{(storageType.EndsWith("?", StringComparison.Ordinal) ? "?" : "")}){value}{argumentSuffix});");
            }
            else if (declaration.Type.Name is "systemtime" or "filetime" or "unixtime" or "timeofday")
            {
                code.Line($"{assignment} = rw.{method}({value}{argumentSuffix}){(declaration.Type.Name == "unixtime" || storageType.EndsWith("?", StringComparison.Ordinal) || WireTypes.Nullable(field) ? "" : ".GetValueOrDefault()")};");
            }
            else if (hasBacking && write is null)
            {
                code.Line($"rw.{method}(ref {target}{(declaration.Type.ArrayDimensions > 0 ? "!" : "")}{argumentSuffix});");
            }
            else
            {
                var cast = write is not null && declaration.Type.ArrayDimensions == 0 && ((numeric && storageType != "int") || declaration.Type.CastTarget is not null)
                    ? "(" + storageType + ")" : "";
                code.Line($"{assignment} = rw.{method}({cast}{value}{argumentSuffix});");
            }
        }
        if (field.IsLocal) locals.Peek().Add(field.Name);
    }

    private string WriteArgument(string expression)
        => "(rw.Writer is null ? default : (" + ExpressionText(expression) + "))";

    private string Method(FieldDeclaration field, string storageType, SerializationMode mode)
    {
        if (field.Type.Name == "data")
        {
            return "Data";
        }

        if (field.Type.ArrayDimensions == 2 && !LayoutModel.Has(field.Attributes, "list"))
        {
            var jaggedName = field.Type.Name;

            if (LayoutModel.Has(field.Attributes, "deprec"))
            {
                throw new NotSupportedException("Jagged arrays do not support deprec.");
            }

            if (IsArchive(field))
            {
                return "JaggedArray" + (mode switch
                {
                    SerializationMode.Read => "Readable",
                    SerializationMode.Write => "Writable",
                    _ => "ReadableWritable"
                }) + "<" + jaggedName + ">";
            }

            if (!WireTypes.Primitive(jaggedName))
            {
                return "JaggedArray" + (LayoutModel.Has(field.Attributes, "external") ? "External" : "") + "NodeRef<" + jaggedName + ">";
            }

            return jaggedName switch
            {
                "id" or "lookbackstring" => "JaggedArrayId",
                "string" => "JaggedArrayString",
                "ident" or "meta" => "JaggedArrayIdent",
                "packdesc" or "fileref" => "JaggedArrayPackDesc",
                _ when WireTypes.Value(jaggedName) && jaggedName != "optimizedint" => "JaggedArray<" + WireTypes.Map(jaggedName) + ">",
                _ => throw new NotSupportedException("Unsupported jagged array element: " + jaggedName)
            };
        }

        var collection = field.Type.ArrayDimensions > 0;
        var list = LayoutModel.Has(field.Attributes, "list");
        var deprec = LayoutModel.Has(field.Attributes, "deprec") ? "_deprec" : "";
        var name = field.Type.Name;
        var generic = "";
        string method;

        if (!WireTypes.Primitive(name))
        {
            var archive = IsArchive(field);
            method = archive ? mode switch
            {
                SerializationMode.Read => "Readable",
                SerializationMode.Write => "Writable",
                _ => "ReadableWritable"
            } :
                LayoutModel.Has(field.Attributes, "meta") ? "MetaRef" : LayoutModel.Has(field.Attributes, "direct") ? "Node" : "NodeRef";

            generic = "<" + name + ">";

            if (layout.Archives.TryGetValue(name, out var local) && LayoutModel.Has(local.Attributes, "contextual"))
            {
                generic = "<" + name + ", " + layout.Name + ">";
            }
        }
        else
        {
            method = WireTypes.Method(name);
        }

        if (collection)
        {
            var prefix = list ? "List" : "Array";

            if (LayoutModel.Has(field.Attributes, "external") && mode != SerializationMode.ReadWrite)
            {
                prefix += "External";
            }

            if (WireTypes.Value(name) && name != "optimizedint")
            {
                return prefix + deprec + "<" + WireTypes.Map(name) + ">";
            }

            return prefix + method + deprec + generic;
        }

        if (field.Type.IsNullable && method is "TimeInt32" or "TimeSingle")
        {
            method += "Nullable";
        }

        return method + generic;
    }

    public string Expression(Expression expression)
        => ExpressionText(ChunkLParser.WriteExpression(expression));

    public string CountExpression(Expression expression)
        => ExpressionText(ChunkLParser.WriteExpression(expression), nullSafeCount: true);

    private bool IsArchive(FieldDeclaration field)
    {
        var name = field.Type.Name;
        return layout.Archives.ContainsKey(name) ||
            (!field.Type.ChunkPreference && layouts.TryGetValue(name, out var referenced) && referenced.SelfArchive is not null);
    }

    private void Throw(AttributeList? attributes)
    {
        var type = LayoutModel.Attribute(attributes, "type") ?? "NotSupportedException";
        var message = LayoutModel.Attribute(attributes, "message");

        code.Line("throw new " + type + "(" + (message is null ? "" : Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(message, true)) + ");");
    }

    private string ExpressionText(string text, bool nullSafeCount = false)
    {
        if (nullSafeCount)
        {
            var separator = text.IndexOf("::", StringComparison.Ordinal);

            if (separator > 0 && text.IndexOf("::", separator + 2, StringComparison.Ordinal) < 0)
            {
                var owner = text.Substring(0, separator);
                var member = text.Substring(separator + 2);
                var field = layout.Scope.Fields.FirstOrDefault(x => x.Name == owner);

                // A missing referenced node or array contributes zero elements to a fixed count.
                if (Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier(owner) &&
                    Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier(member) &&
                    field is not null && WireTypes.Nullable(field))
                {
                    text = owner + "?." + member + " ?? 0";
                }
            }
        }

        var expression = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression(text.Replace("::", "."));
        return new IdentifierRewriter(Identifier).Visit(expression)!.ToString();
    }

    private string Identifier(string name)
    {
        if (locals.Any(x => x.Contains(name)))
        {
            return SyntaxOverlap.Backing(name);
        }
        if (!chunk)
        {
            return SyntaxOverlap.Escape(name);
        }

        if (scope.Fields.Any(x => x.Name == name && (x.IsUnknown || x.IsVersion)))
        {
            return SyntaxOverlap.Escape(name);
        }

        if (layout.Scope.Fields.Any(x => x.Name == name || x.Name + "File" == name) || SyntaxOverlap.Has(layout.Existing, name))
        {
            return "n." + SyntaxOverlap.Escape(name);
        }

        return SyntaxOverlap.Escape(name);
    }
}
