using ChunkL;
using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;

namespace GBX.NET.Generators.Generation;

internal sealed class SerializationWriter
{
    private readonly CodeWriter code;
    private readonly LayoutModel layout;
    private readonly ScopeModel scope;
    private readonly IReadOnlyDictionary<string, LayoutModel> layouts;
    private readonly IReadOnlyDictionary<string, ExistingType> existingTypes;
    private readonly SerializationMode mode;
    private readonly bool chunk;

    private int loopCount;
    private readonly Stack<Dictionary<string, FieldModel>> locals = new();

    public SerializationWriter(CodeWriter code, LayoutModel layout, ScopeModel scope,
        IReadOnlyDictionary<string, LayoutModel> layouts, IReadOnlyDictionary<string, ExistingType> existingTypes,
        SerializationMode mode, bool chunk)
    {
        this.code = code;
        this.layout = layout;
        this.scope = scope;
        this.layouts = layouts;
        this.existingTypes = existingTypes;
        this.mode = mode;
        this.chunk = chunk;
    }

    public void Write(IEnumerable<IBodyStatement> statements)
    {
        locals.Push(new Dictionary<string, FieldModel>(StringComparer.Ordinal));
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
                    if (mode == SerializationMode.Write) break;
                    if (mode == SerializationMode.ReadWrite) code.Open("if (rw.Reader is not null)");
                    var sourceSyntax = SyntaxFactory.ParseExpression(ChunkLParser.WriteExpression(assignment.Expression));
                    while (sourceSyntax is ParenthesizedExpressionSyntax parentheses) sourceSyntax = parentheses.Expression;
                    var sourceName = sourceSyntax switch
                    {
                        IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
                        MemberAccessExpressionSyntax { Expression: ThisExpressionSyntax, Name: IdentifierNameSyntax member } => member.Identifier.ValueText,
                        MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "n" }, Name: IdentifierNameSyntax member } when chunk => member.Identifier.ValueText,
                        _ => null
                    };
                    if (sourceName is not null && StoredReferenceIdentifier(assignment.TargetName) is { } copyTarget && StoredReferenceIdentifier(sourceName) is { } copySource &&
                        ReferenceElement(assignment.TargetName) == ReferenceElement(sourceName))
                    {
                        code.Line("NodeRef.CopyFrom(ref " + copyTarget + ", " + copySource + ");");
                        if (mode == SerializationMode.ReadWrite) code.Close();
                        break;
                    }
                    var assignmentField = scope.FindField(assignment.TargetName) ??
                        layout.Scope.FindField(assignment.TargetName);
                    var assignmentProperty = layout.FindProperty(assignment.TargetName);
                    var assignmentType = assignmentField is not null ? WireTypes.CSharp(assignmentField.Declaration) :
                        assignmentProperty is not null ? WireTypes.CSharp(assignmentProperty.Type) : null;
                    var requiredNodes = mode is SerializationMode.Read or SerializationMode.ReadWrite
                        ? sourceSyntax.DescendantNodesAndSelf().OfType<MemberAccessExpressionSyntax>()
                            .Select(x => IdentifierRewriter.ReferenceName(x.Expression))
                            .Where(static x => x is not null)
                            .Select(x => CachedReferenceIdentifier(x!))
                            .Where(static x => x is not null).Distinct().ToArray()
                        : [];
                    if (requiredNodes.Length > 0)
                    {
                        // Resolve data dependencies when reading. Presence checks and reference
                        // copies can use descriptors alone, but derived values need the node.
                        code.Open("if (" + string.Join(" && ", requiredNodes.Select(static x => x + "?.Resolve() is not null")) + ")");
                    }
                    if (CachedReferenceIdentifier(assignment.TargetName) is { } assignmentHandle &&
                        ReferenceFieldByName(assignment.TargetName)?.Name != assignment.TargetName)
                        code.Line("NodeRef.SetEmbedded(ref " + assignmentHandle + ", " + Expression(assignment.Expression, assignmentType) + ");");
                    else
                        code.Line(Identifier(assignment.TargetName) + " = " + Expression(assignment.Expression, assignmentType) + ";");
                    if (requiredNodes.Length > 0) code.Close();
                    if (mode == SerializationMode.ReadWrite) code.Close();
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

            if (chunk && scope.HasGameQualifiers)
            {
                var maximum = scope.ChunkVersions.Count > 0 ? scope.ChunkVersions.Values.Max().ToString() :
                    declaration.DefaultValue is { } defaultVersion ? Expression(defaultVersion) : "0";
                if (scope.ChunkVersions.Count > 0 && declaration.DefaultValue is { } fallbackVersion)
                {
                    maximum = $"global::System.Math.Max({maximum}, {Expression(fallbackVersion)})";
                }
                code.Open($"if (Version > {maximum})");
                code.Line("throw new global::GBX.NET.Exceptions.ChunkVersionNotSupportedException(Version);");
                code.Close();
                code.BlankLine();
            }

            return;
        }

        var owner = chunk && !field.IsUnknown ? layout.Scope : scope;
        var prefix = field.IsLocal ? "" : chunk ? field.IsUnknown ? "" : "n." : "this.";
        var backing = chunk && field.IsUnknown ? SyntaxOverlap.Escape(field.Name) : SyntaxOverlap.Backing(field.Name);
        var property = field.IsLocal ? SyntaxOverlap.Backing(field.Name) : SyntaxOverlap.Escape(field.Name);
        var hasBacking = !field.IsLocal && !field.Occurrences.Any(static x => LayoutModel.Has(x.Attributes, "inherited")) &&
            (!SyntaxOverlap.Has(owner.Existing, field.Name) || SyntaxOverlap.Has(owner.Existing, backing.TrimStart('@')));
        var target = prefix + (hasBacking ? backing : property);
        var storedField = !field.IsLocal && owner.Occurrences.TryGetValue(declaration, out var shared) ? shared : field;
        if (FieldModel.IsLegacyCurve(declaration) && LayoutModel.Has(declaration.Attributes, "direct") &&
            storedField.StorageDeclaration.Type.Name == "PlugCurve")
        {
            var direct = $"({target} ??= new PlugCurve()).ReadWriteDirect<{declaration.Type.Name}>";
            if (mode == SerializationMode.ReadWrite) code.Line($"{direct}(rw);");
            else
            {
                code.Line($"using (var curveRw = new GbxReaderWriter({(mode == SerializationMode.Read ? "r" : "w")}))");
                code.Line($"    {direct}(curveRw);");
            }
            return;
        }
        if (declaration.Type.Name == "CFuncKeysReal" && storedField.StorageDeclaration.Type.Name == "PlugCurve")
        {
            if (mode == SerializationMode.ReadWrite) code.Line($"{target} = rw.ReadableWritable<PlugCurve>({target}, version: -1);");
            else
            {
                code.Line($"using (var curveRw = new GbxReaderWriter({(mode == SerializationMode.Read ? "r" : "w")}))");
                code.Line($"    {target} = curveRw.ReadableWritable<PlugCurve>({target}, version: -1);");
            }
            return;
        }
        if (storedField.StorageDeclaration.Type.ArrayDimensions == 1 &&
            ReferenceModel.IsNode(storedField.StorageDeclaration, layout, layouts, existingTypes) &&
            !ReferenceModel.IsNode(declaration, layout, layouts, existingTypes) && IsArchive(declaration))
        {
            var options = declaration.Type.FixedArrayCount is { Length: > 0 } count ? ", length: " + ExpressionText(count) : "";
            if (LayoutModel.Attribute(declaration.Attributes, "version") is { } archiveVersion) options += ", version: " + ExpressionText(archiveVersion);
            if (LayoutModel.Has(declaration.Attributes, "deprec")) options += ", deprec: true";
            if (mode == SerializationMode.ReadWrite) code.Line($"{target} = rw.EmbeddedNodeRefList({target}{options});");
            else
            {
                code.Line($"using (var referenceRw = new GbxReaderWriter({(mode == SerializationMode.Read ? "r" : "w")}))");
                code.Line($"    {target} = referenceRw.EmbeddedNodeRefList({target}{options});");
            }
            return;
        }
        if (!field.IsLocal && storedField.StorageDeclaration.Type.ArrayDimensions == 0 &&
            ReferenceModel.IsNode(storedField.StorageDeclaration, layout, layouts, existingTypes)) hasBacking = false;
        if (ReferenceModel.IsNode(declaration, layout, layouts, existingTypes) &&
            ReferenceModel.IsNode(storedField.StorageDeclaration, layout, layouts, existingTypes))
        {
            ReferenceField(declaration, storedField, field.IsLocal, prefix, target);
            if (field.IsLocal) locals.Peek()[field.Name] = field;
            return;
        }
        var embeddedHandle = !field.IsLocal && storedField.StorageDeclaration.Type.ArrayDimensions == 0 &&
            ReferenceModel.IsNode(storedField.StorageDeclaration, layout, layouts, existingTypes) &&
            !SyntaxOverlap.Has(owner.Existing, backing.TrimStart('@'))
            ? prefix + ReferenceModel.Handle(storedField, chunk && storedField.IsUnknown) : null;
        if (embeddedHandle is not null) target = embeddedHandle + "?.CachedNode";
        string Assign(string value) => embeddedHandle is null
            ? (field.IsLocal ? "var " : "") + target + " = " + value + ";"
            : "NodeRef.SetEmbedded(ref " + embeddedHandle + ", " + value + ");";
        var storageType = (field.IsLocal ? null : SyntaxOverlap.MemberType(owner.Existing, (hasBacking ? backing : property).TrimStart('@'))) ??
            WireTypes.CSharp(storedField.StorageDeclaration) +
            (!field.IsLocal && WireTypes.Nullable(storedField) ? "?" : "");
        if (field.IsLocal && WireTypes.Nullable(field))
        {
            storageType += "?";
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

        var argumentSuffix = arguments.Count == 0 ? "" : ", " + string.Join(", ", arguments);

        if (mode == SerializationMode.Read)
        {
            var readMethod = method;
            var cast = declaration.Type.CastTarget is not null ? "(" + WireTypes.Cast(declaration.Type) + ")" :
                SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(WireTypes.CSharp(declaration)) &&
                    (WireTypes.Value(declaration.Type.Name) || !WireTypes.Primitive(declaration.Type.Name)) && declaration.Type.ArrayDimensions == 0 ? "(" + storageType + ")" : "";

            code.Line(Assign($"{cast}r.Read{readMethod}({string.Join(", ", arguments)})"));
        }
        else if (mode == SerializationMode.Write)
        {
            var value = write is null ? target : "(" + ExpressionText(write) + ")";
            if (field.IsLocal)
            {
                code.Line($"{storageType} {target} = {value};");
                value = target;
            }
            if (declaration.Type.ArrayDimensions == 0 && WireTypes.Value(declaration.Type.Name) &&
                storageType.EndsWith("?", StringComparison.Ordinal) &&
                !LayoutModel.Has(declaration.Attributes, "nullable") && !LayoutModel.Has(declaration.Attributes, "time") &&
                declaration.Type.Name is not ("filetime" or "systemtime" or "unixtime" or "timeofday"))
            {
                value += ".GetValueOrDefault()";
            }
            var writeMethod = method == "Id" ? "IdAsString" : method;

            if (declaration.Type.ArrayDimensions == 1 && declaration.Type.Name is "string" or "ident" or "meta" or "packdesc" or "fileref")
            {
                writeMethod = (LayoutModel.Has(declaration.Attributes, "list") ? "List" : "Array") + (LayoutModel.Has(declaration.Attributes, "deprec") ? "_deprec" : "");
            }

            if (declaration.Type.CastTarget is not null && declaration.Type.ArrayDimensions == 0 && WireTypes.Primitive(declaration.Type.Name))
            {
                writeMethod = WireTypes.Method(declaration.Type.Name, declaration.Attributes);
            }

            var cast = declaration.Type.CastTarget is not null ||
                ((WireTypes.Value(declaration.Type.Name) || !WireTypes.Primitive(declaration.Type.Name)) && declaration.Type.ArrayDimensions == 0 && SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(WireTypes.CSharp(declaration)))
                ? "(" + WireTypes.Map(declaration.Type.Name, declaration.Attributes) +
                    (LayoutModel.Has(declaration.Attributes, "nullable") ? "?" : "") + ")" : "";

            if (declaration.Type.ArrayDimensions == 0 && WireTypes.Primitive(declaration.Type.Name) &&
                !LayoutModel.Has(declaration.Attributes, "nullable") &&
                declaration.Type.Name is not ("id" or "lookbackstring" or "data" or "optimizedint" or "vec3_6" or "filetime" or "systemtime" or "unixtime" or "timeofday" or "ipv4"))
            {
                writeMethod = "";
            }

            code.Line($"w.Write{writeMethod}({cast}{value}{argumentSuffix});");
        }
        else
        {
            var numeric = SyntaxOverlap.Normalize(storageType) is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint" or "long" or "ulong" or "float" or "double";
            if (declaration.Type.CastTarget is not null && !numeric && WireTypes.Primitive(declaration.Type.Name))
            {
                method = "Enum" + WireTypes.Method(declaration.Type.Name, declaration.Attributes) + "<" + WireTypes.Cast(declaration.Type) + ">";
            }

            var wireType = WireTypes.Map(declaration.Type.Name, declaration.Attributes);
            var conversion = (numeric || !WireTypes.Primitive(declaration.Type.Name)) && declaration.Type.ArrayDimensions == 0 && SyntaxOverlap.Normalize(storageType) != SyntaxOverlap.Normalize(wireType);
            var value = write is null ? target : WriteArgument(write);
            if (conversion)
            {
                code.Line(Assign($"({storageType})rw.{method}(({wireType}{(storageType.EndsWith("?", StringComparison.Ordinal) ? "?" : "")}){value}{argumentSuffix})"));
            }
            else if (declaration.Type.Name is "systemtime" or "filetime" or "unixtime" or "timeofday")
            {
                code.Line(Assign($"rw.{method}({value}{argumentSuffix}){(storageType.EndsWith("?", StringComparison.Ordinal) || WireTypes.Nullable(field) ? "" : ".GetValueOrDefault()")}"));
            }
            else if (embeddedHandle is not null && write is null && arguments.Count == 0 &&
                method == "Node<" + ReferenceModel.Element(storedField) + ">")
            {
                code.Line($"rw.Node(ref {embeddedHandle});");
            }
            else if (hasBacking && write is null)
            {
                code.Line($"rw.{method}(ref {target}{(declaration.Type.ArrayDimensions > 0 ? "!" : "")}{argumentSuffix});");
            }
            else
            {
                var cast = write is not null && declaration.Type.ArrayDimensions == 0 && ((numeric && storageType != "int") || declaration.Type.CastTarget is not null)
                    ? "(" + storageType + ")" : "";
                code.Line(Assign($"rw.{method}({cast}{value}{argumentSuffix})"));
            }
        }
        if (field.IsLocal) locals.Peek()[field.Name] = field;
    }

    private string WriteArgument(string expression)
        => "(rw.Writer is null ? default : (" + ExpressionText(expression) + "))";

    private void ReferenceField(FieldDeclaration declaration, FieldModel field, bool local, string prefix, string target)
    {
        var scalar = declaration.Type.ArrayDimensions == 0;
        var name = scalar ? WireTypes.Map(declaration.Type.Name) : ReferenceModel.Element(field);
        if (scalar)
        {
            if (!local && field.Occurrences.Any(static x => LayoutModel.Has(x.Attributes, "inherited")))
            {
                var reference = prefix + "References." + SyntaxOverlap.Escape(field.Name);
                code.Line(mode switch
                {
                    SerializationMode.Read => $"r.ReadNodeRef({reference});",
                    SerializationMode.Write => $"w.WriteNodeRef({reference});",
                    _ => $"rw.NodeRef({reference});"
                });
                return;
            }

            var handle = prefix + ReferenceModel.Handle(field, chunk && field.IsUnknown);
            if (local)
            {
                handle = SyntaxOverlap.Backing(field.Name).TrimStart('@') + "Ref";
                code.Line($"NodeRef<{name}>? {handle} = null;");
            }
            var owner = chunk && !field.IsUnknown ? layout.Scope : scope;
            var customStorage = local ? null : SyntaxOverlap.MemberType(owner.Existing, SyntaxOverlap.Backing(field.Name).TrimStart('@'));
            var converted = customStorage is not null && SyntaxOverlap.Normalize(customStorage.TrimEnd('?')) != name;
            if (converted && mode != SerializationMode.Read)
            {
                if (mode == SerializationMode.ReadWrite) code.Line($"if (rw.Writer is not null && {handle}?.Descriptor is null) NodeRef.SetEmbedded(ref {handle}, ({name}?){target});");
                else code.Line($"if ({handle}?.Descriptor is null) NodeRef.SetEmbedded(ref {handle}, ({name}?){target});");
            }
            code.Line(mode switch
            {
                SerializationMode.Read => $"r.ReadNodeRef(ref {handle});",
                SerializationMode.Write => $"w.WriteNodeRef({handle});",
                _ => $"rw.NodeRef(ref {handle});"
            });
            if (converted && mode != SerializationMode.Write)
            {
                var assignment = $"{target} = ({customStorage}){handle}?.CachedNode;";
                code.Line(mode == SerializationMode.ReadWrite ? "if (rw.Reader is not null) " + assignment : assignment);
            }
            if (local) code.Line($"var {target} = {handle}?.CachedNode;");
            return;
        }

        var arguments = declaration.Type.FixedArrayCount is { Length: > 0 } length
            ? declaration.Type.ArrayDimensions > 1 ? "innerLength: " + ExpressionText(length, nullSafeCount: true) :
                ExpressionText(length, nullSafeCount: true) : "";
        var method = declaration.Type.ArrayDimensions > 1 ? "JaggedArrayNodeRef" :
            "ListNodeRef" + (LayoutModel.Has(declaration.Attributes, "deprec") ? "_deprec" : "");
        var suffix = arguments.Length == 0 ? "" : ", " + arguments;
        var readArguments = declaration.Type.FixedArrayCount is { Length: > 0 } readLength
            ? declaration.Type.ArrayDimensions > 1 ? "innerLength: " + ExpressionText(readLength, nullSafeCount: true) :
                "length: " + ExpressionText(readLength, nullSafeCount: true)
            : "";
        var readMethod = declaration.Type.ArrayDimensions > 1 ? "JaggedListNodeRef" : method;
        code.Line(mode switch
        {
            SerializationMode.Read => $"{(local ? "var " : "")}{target} = r.Read{readMethod}<{name}>({readArguments});",
            SerializationMode.Write => $"w.Write{method}({target}{suffix});",
            _ => $"{(local ? "var " : "")}{target} = rw.{method}<{name}>({(local ? "null" : target)}{suffix});"
        });
    }

    private string Method(FieldDeclaration field, string storageType, SerializationMode mode)
    {
        if (field.Type.Name == "ipv4" && field.Type.ArrayDimensions > 0)
        {
            throw new NotSupportedException("IPv4 fields do not support arrays or lists.");
        }

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
                return "JaggedArrayNodeRef<" + jaggedName + ">";
            }

            return jaggedName switch
            {
                "id" or "lookbackstring" => "JaggedArrayId",
                "string" => "JaggedArrayString",
                "ident" or "meta" => "JaggedArrayIdent",
                "packdesc" or "fileref" => "JaggedArrayPackDesc",
                _ when WireTypes.Value(jaggedName) && jaggedName != "optimizedint" => "JaggedArray<" + WireTypes.Map(jaggedName, field.Attributes) + ">",
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
            method = WireTypes.Method(name, field.Attributes);
        }

        if (collection)
        {
            var prefix = list ? "List" : "Array";

            if (WireTypes.Value(name) && name != "optimizedint")
            {
                return prefix + deprec + "<" + WireTypes.Map(name, field.Attributes) + ">";
            }

            return prefix + method + deprec + generic;
        }

        if (LayoutModel.Has(field.Attributes, "nullable") ||
            (field.Type.IsNullable && method is "TimeInt32" or "TimeSingle"))
        {
            method += "Nullable";
        }

        return method + generic;
    }

    public string Expression(Expression expression, string? targetType = null)
        => IntegerResult(ChunkLParser.WriteExpression(expression), targetType);

    public string CountExpression(Expression expression, string? targetType = null)
        => IntegerResult(ChunkLParser.WriteExpression(expression), targetType, nullSafeCount: true);

    private string IntegerResult(string expression, string? targetType, bool nullSafeCount = false)
    {
        var syntax = SyntaxFactory.ParseExpression(expression);
        var rewritten = RewriteExpression(syntax, nullSafeCount);
        if (targetType is not ("byte" or "sbyte" or "short" or "ushort" or "int" or "uint")) return rewritten;

        // C# already converts in-range integer literals without a cast.
        if (syntax is LiteralExpressionSyntax) return rewritten;
        var sourceType = IntegerType(syntax);
        return sourceType is not null && sourceType != targetType
            ? "(" + targetType + ")(" + rewritten + ")" : rewritten;
    }

    private string? IntegerType(ExpressionSyntax expression)
    {
        if (expression is ParenthesizedExpressionSyntax parentheses)
            return IntegerType(parentheses.Expression);
        if (expression is LiteralExpressionSyntax literal)
            return literal.Token.Value switch { int => "int", uint => "uint", long => "long", ulong => "ulong", _ => null };
        if (expression is IdentifierNameSyntax identifier)
        {
            var name = identifier.Identifier.ValueText;
            var field = FindLocal(name) ??
                scope.FindField(name) ?? layout.Scope.FindField(name);
            var property = layout.FindProperty(name);
            var type = field is not null ? WireTypes.CSharp(field.Declaration) :
                property is not null ? WireTypes.CSharp(property.Type) : SyntaxOverlap.MemberType(scope.Existing, name);
            return type is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint" or "long" or "ulong" ? type : null;
        }
        if (expression is not BinaryExpressionSyntax binary) return null;
        var left = IntegerType(binary.Left);
        var right = IntegerType(binary.Right);
        if (binary.RawKind is (int)SyntaxKind.LeftShiftExpression or
            (int)SyntaxKind.RightShiftExpression)
            return left is "byte" or "sbyte" or "short" or "ushort" ? "int" : left;
        if (binary.RawKind is not ((int)SyntaxKind.BitwiseAndExpression or
            (int)SyntaxKind.BitwiseOrExpression or
            (int)SyntaxKind.ExclusiveOrExpression or
            (int)SyntaxKind.AddExpression or
            (int)SyntaxKind.SubtractExpression or
            (int)SyntaxKind.MultiplyExpression or
            (int)SyntaxKind.DivideExpression or
            (int)SyntaxKind.ModuloExpression)) return null;
        if (left is null || right is null) return null;
        if (left == "ulong" || right == "ulong") return "ulong";
        if (left == "long" || right == "long") return "long";
        if (left == "uint" || right == "uint")
        {
            var other = left == "uint" ? binary.Right : binary.Left;
            return IntegerType(other) == "int" &&
                other is not LiteralExpressionSyntax { Token.Value: int and >= 0 }
                ? "long" : "uint";
        }
        return "int";
    }

    private bool IsArchive(FieldDeclaration field)
    {
        var name = field.Type.Name;
        if (!field.Type.ChunkPreference &&
            (existingTypes.TryGetValue(layout.File.Namespace + "." + name, out var existing) ||
             existingTypes.TryGetValue("GBX.NET." + name, out existing) ||
             existingTypes.TryGetValue(name, out existing)) &&
            existing.BaseTypes.Any(static x => SyntaxOverlap.Normalize(x.ToString()) == "IReadableWritable"))
        {
            return true;
        }

        var separator = name.IndexOf('.');
        if (separator >= 0)
        {
            var ownerName = name.Substring(0, separator);
            var archiveName = name.Substring(separator + 1);
            if (!layouts.TryGetValue(ownerName, out var owner) || !owner.Archives.ContainsKey(archiveName))
            {
                throw new InvalidOperationException("Unknown archive: " + name);
            }

            return true;
        }

        return layout.Archives.ContainsKey(name) ||
            (!field.Type.ChunkPreference && layouts.TryGetValue(name, out var referenced) && referenced.SelfArchive is not null);
    }

    private void Throw(AttributeList? attributes)
    {
        var type = LayoutModel.Attribute(attributes, "type") ?? "NotSupportedException";
        var message = LayoutModel.Attribute(attributes, "message");

        code.Line("throw new " + type + "(" + (message is null ? "" : SymbolDisplay.FormatLiteral(message, true)) + ");");
    }

    private string ExpressionText(string text, bool nullSafeCount = false)
        => RewriteExpression(SyntaxFactory.ParseExpression(text), nullSafeCount);

    private string RewriteExpression(ExpressionSyntax expression, bool nullSafeCount = false)
    {
        if (nullSafeCount && expression is MemberAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax owner,
                Name: IdentifierNameSyntax member
            } && SyntaxFacts.IsValidIdentifier(owner.Identifier.Text) &&
            SyntaxFacts.IsValidIdentifier(member.Identifier.Text) &&
            layout.Scope.FindField(owner.Identifier.ValueText) is { } field && WireTypes.Nullable(field))
        {
            // A missing referenced node or array contributes zero elements to a fixed count.
            expression = SyntaxFactory.ParseExpression(owner.Identifier.Text + "?." + member.Identifier.Text + " ?? 0");
        }

        return new IdentifierRewriter(DataIdentifier, IsTimeInt32, ReferenceIdentifier,
            CachedReferenceIdentifier).Visit(expression)!.ToString();
    }

    private FieldModel? ReferenceFieldByName(string name) => scope.FindField(name) ?? layout.Scope.FindField(name) ??
        scope.Fields.Concat(layout.Scope.Fields).FirstOrDefault(x => SyntaxOverlap.Backing(x.Name).TrimStart('@') == name);

    private string? ReferenceElement(string name) => ReferenceFieldByName(name) is { } field ? ReferenceModel.Element(field) : null;

    private string? StoredReferenceIdentifier(string name)
    {
        if (FindLocal(name) is not null) return null;
        var field = ReferenceFieldByName(name);
        if (field is null || field.StorageDeclaration.Type.ArrayDimensions != 0 ||
            !ReferenceModel.IsNode(field.StorageDeclaration, layout, layouts, existingTypes)) return null;
        return (chunk && !field.IsUnknown ? "n." : "") + ReferenceModel.Handle(field, chunk && field.IsUnknown);
    }

    private string DataIdentifier(string name)
    {
        if (CachedReferenceIdentifier(name) is { } handle) return handle + "?.CachedNode";
        return Identifier(name);
    }

    private string? CachedReferenceIdentifier(string name)
    {
        if (mode == SerializationMode.None && (ReferenceFieldByName(name)?.Name == name ||
            SyntaxOverlap.Has(scope.Existing, name) || SyntaxOverlap.Has(layout.Existing, name))) return null;
        return StoredReferenceIdentifier(name);
    }

    private string? ReferenceIdentifier(string name)
    {
        var local = FindLocal(name);
        var field = local ?? scope.FindField(name) ?? layout.Scope.FindField(name);
        if (field is null || field.StorageDeclaration.Type.ArrayDimensions != 0 ||
            !ReferenceModel.IsNode(field.StorageDeclaration, layout, layouts, existingTypes)) return null;
        var prefix = local is not null ? "" : chunk && !field.IsUnknown ? "n." : "";
        return prefix + ReferenceModel.Handle(field, local is null && chunk && field.IsUnknown);
    }

    private FieldModel? FindLocal(string name)
    {
        foreach (var localScope in locals)
        {
            if (localScope.TryGetValue(name, out var field)) return field;
        }

        return null;
    }

    private bool IsTimeInt32(ExpressionSyntax expression)
    {
        while (expression is ParenthesizedExpressionSyntax parentheses)
            expression = parentheses.Expression;
        if (expression is not IdentifierNameSyntax identifier) return false;

        var name = identifier.Identifier.ValueText;
        if (FindLocal(name) is { } local)
            return SyntaxOverlap.Normalize(WireTypes.CSharp(local.Declaration)) == "TimeInt32";

        var field = scope.FindField(name) ?? layout.Scope.FindField(name);
        var owner = chunk && field?.IsUnknown != true && field?.IsVersion != true ? layout.Scope : scope;
        var type = SyntaxOverlap.MemberType(owner.Existing, name) ?? (field is null ? null : WireTypes.CSharp(field.Declaration));
        return type is not null && SyntaxOverlap.Normalize(type) == "TimeInt32";
    }

    private string Identifier(string name)
    {
        if (FindLocal(name) is not null)
        {
            return SyntaxOverlap.Backing(name);
        }
        if (!chunk)
        {
            return SyntaxOverlap.Escape(name);
        }

        if (scope.FindField(name) is { IsUnknown: true } or { IsVersion: true })
        {
            return SyntaxOverlap.Escape(name);
        }

        if (layout.Scope.FindField(name) is not null ||
            layout.FindProperty(name) is not null || SyntaxOverlap.Has(layout.Existing, name))
        {
            return "n." + SyntaxOverlap.Escape(name);
        }

        return SyntaxOverlap.Escape(name);
    }
}
