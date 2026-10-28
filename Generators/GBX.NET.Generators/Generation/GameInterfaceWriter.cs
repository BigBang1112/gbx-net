namespace GBX.NET.Generators.Generation;

// Writes the game version interfaces of a node and the node partial implementing them.
internal static class GameInterfaceWriter
{
    public static string Interfaces(GameNodeModel node, GameVersionModel model, ImmutableArray<string> classContract)
    {
        var code = CodeWriter.Generated(node.InterfaceNamespace);
        var parent = model.Parent(node);
        var groups = model.InterfaceGroups(node);
        WriteGroups(code, node, groups, model);
        foreach (var game in node.Views)
        {
            var name = node.InterfaceName + game;
            if (model.Plan.Existing.TryGetValue(node.InterfaceNamespace + "." + name, out var existing) && !existing.IsPartial)
                throw new InvalidOperationException("Existing interface must be partial: " + existing.Key);
            var bases = new List<string> { game.Length == 0 ? $"IClassVersion<{node.Layout.QualifiedName}>" : node.InterfaceName };
            if (parent is not null && (game.Length == 0 || parent.Games.Contains(game))) bases.Add(parent.Interface(game));
            bases.AddRange(groups.Where(g => g.Games.Contains(game)).Select(g => g.Contract(game)));
            code.Open($"public partial interface {name} : " + string.Join(", ", bases));
            if (game.Length == 0 && parent is not null)
            {
                foreach (var line in classContract) code.Line(line);
            }
            foreach (var member in node.MembersFor(game))
            {
                if (!model.OwnsProjection(node, game, member)) continue;
                if (groups.Any(g => g.Games.Contains(game) && g.Members.Contains(member))) continue;
                var type = model.Project(member, game);
                var hidesInherited = game.Length > 0 && node.IsCommon(member.Name) ||
                    GameNodeModel.ExistingContract(model.Plan, node, game, member.Name) is not null ||
                    parent?.MemberFor(game, member.Name) is not null;
                var declaration = $"{(hidesInherited ? "new " : "")}{type} {SyntaxOverlap.Escape(member.Name)}";
                if (model.DefaultImplementations && type != member.Type)
                {
                    var child = model.Child(member);
                    code.Open(declaration);
                    ProjectedAccessors(code, node, member, type, child, child.Interface(game), game.Length > 0, model.Plan, defaultImplementation: true);
                    code.Close();
                }
                else
                {
                    code.Line($"{declaration} {{ get;{(member.Writable ? " set;" : "")} }}");
                }
            }
            foreach (var method in node.MethodsFor(game))
            {
                if (!model.OwnsMethod(node, game, method)) continue;
                var common = game.Length > 0 && node.Games.All(method.Games.Contains);
                var returned = model.ProjectReturn(method, game);
                if (common && returned == model.ProjectReturn(method, "")) continue;
                var methodName = method.Declaration.Identifier.Text;
                code.Line($"{(common ? "new " : "")}{returned} {methodName}({method.Parameters})" + (model.DefaultImplementations && returned != method.ReturnType
                    ? $" => (({node.Layout.QualifiedName})this).{methodName}({method.Arguments});" : ";"));
            }
            code.Close();
            code.BlankLine();
        }
        return code.ToString();
    }

    private static void WriteGroups(CodeWriter code, GameNodeModel node, List<InterfaceMemberGroup> groups, GameVersionModel model)
    {
        foreach (var group in groups)
        {
            code.Line("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
            code.Open($"public interface {group.Name}{group.Parameters} : {node.Interface()}" +
                string.Concat(group.Children.Select(static (_, i) => $" where T{i} : class")));
            var game = group.Games[0];
            foreach (var member in group.Members)
            {
                var type = model.Project(member, game);
                var projected = type != member.Type;
                for (var i = 0; i < group.Children.Count; i++) type = type.Replace(group.Children[i].Interface(game), "T" + i);
                var declaration = $"{(node.IsCommon(member.Name) ? "new " : "")}{type} {SyntaxOverlap.Escape(member.Name)}";
                if (model.DefaultImplementations && projected)
                {
                    // One body serves every game that closes this group.
                    var child = model.Child(member);
                    code.Open(declaration);
                    ProjectedAccessors(code, node, member, type, child, "T" + group.Children.IndexOf(child), true, model.Plan, defaultImplementation: true);
                    code.Close();
                }
                else
                {
                    code.Line($"{declaration} {{ get;{(member.Writable ? " set;" : "")} }}");
                }
            }
            code.Close();
        }
    }

    public static string Implementation(GameNodeModel node, GameVersionModel model)
    {
        var code = CodeWriter.Generated(node.Layout.File.Namespace);
        code.Open("public partial class " + node.Layout.Name + " : " + string.Join(", ", node.Views.Select(node.Interface)));
        WriteForwardedMembers(code, node, model);

        // Default implementations on the interfaces replace the projections below.
        foreach (var game in model.DefaultImplementations ? [] : node.Views)
        {
            foreach (var member in node.MembersFor(game))
            {
                if (!model.OwnsProjection(node, game, member)) continue;
                var projected = model.Project(member, game);
                if (projected == member.Type) continue;
                var child = model.Child(member);
                code.Open($"{projected} {model.PropertyContract(node, game, member)}.{SyntaxOverlap.Escape(member.Name)}");
                ProjectedAccessors(code, node, member, projected, child, child.Interface(game), game.Length > 0, model.Plan, defaultImplementation: false);
                code.Close();
            }
            foreach (var method in node.MethodsFor(game))
            {
                if (!model.OwnsMethod(node, game, method)) continue;
                var returned = model.ProjectReturn(method, game);
                if (returned == method.ReturnType || game.Length > 0 && node.Games.All(method.Games.Contains) &&
                    returned == model.ProjectReturn(method, "")) continue;
                code.Line($"{returned} {node.Interface(game)}.{method.Declaration.Identifier.Text}({method.Parameters}) => {method.Declaration.Identifier.Text}({method.Arguments});");
            }
        }
        foreach (var member in node.Members.Where(m => NeedsInterfaceReferences(node, m, model)))
        {
            // A base node already reading the same property shares its helper.
            var inherited = InheritedInterfaceReferences(node, member, model);
            if (inherited?.Owner == member.Owner) continue;
            var element = GameNodeModel.Qualify(ReferenceModel.Element(member.Field!), member.Owner, model.Plan);
            code.Line($"internal {(inherited is null ? "" : "new ")}NodeRefList<{element}>? Get{member.Name}InterfaceReferences() => {SyntaxOverlap.Escape(member.Name)};");
        }
        code.Close();
        return code.ToString();
    }

    private static bool NeedsInterfaceReferences(GameNodeModel node, GameMember member, GameVersionModel model)
        => member.Field?.StorageDeclaration.Type.ArrayDimensions == 1 && node.Views.Any(game => node.MembersFor(game).Contains(member) &&
            !model.ExistingProperty(node, game, member) && !model.InheritedProperty(node, game, member) && model.Project(member, game) != member.Type);

    private static GameMember? InheritedInterfaceReferences(GameNodeModel node, GameMember member, GameVersionModel model)
    {
        foreach (var layout in GameNodeModel.Hierarchy(node.Layout, model.Plan.Layouts).Skip(1))
        {
            if (!model.Nodes.TryGetValue(layout.Name, out var ancestor) || ancestor.Games.Count == 0) continue;
            var inherited = ancestor.Members.FirstOrDefault(m => m.Name == member.Name);
            if (inherited is not null && NeedsInterfaceReferences(ancestor, inherited, model)) return inherited;
        }
        return null;
    }

    // A node member hiding an unprojected base node member would otherwise
    // implement the base interface. Forward it to the base node instead.
    private static void WriteForwardedMembers(CodeWriter code, GameNodeModel node, GameVersionModel model)
    {
        if (model.Parent(node) is not { } parent) return;
        var forwarded = new HashSet<string>(StringComparer.Ordinal);
        foreach (var game in parent.Views)
        {
            foreach (var member in parent.MembersFor(game))
            {
                var own = node.Members.FirstOrDefault(m => m.Name == member.Name);
                if (own is null || own.Owner == member.Owner || model.Project(member, game) != member.Type) continue;
                var common = game.Length > 0 && parent.IsCommon(member.Name) && model.Project(member, "") == member.Type;
                var contract = model.PropertyContract(parent, common ? "" : game, member);
                if (!forwarded.Add(contract + "." + member.Name)) continue;
                var name = SyntaxOverlap.Escape(member.Name);
                var access = $"(({parent.Layout.QualifiedName})this).{name}";
                code.Open($"{member.Type} {contract}.{name}");
                var required = !member.Type.EndsWith("?", StringComparison.Ordinal) && IsReferenceType(member.Type, model.Plan);
                code.Line($"get => {access}{(required ? " ?? throw new MemberNullException(" + SymbolDisplay.FormatLiteral(member.Name, true) + ")" : "")};");
                if (member.Writable) code.Line($"set => {access} = value;");
                code.Close();
            }
        }
    }

    // Converts a node member to its game interface projection. A default
    // implementation reaches the node through a cast, and a generic member group
    // can supply a type parameter as the child view, which needs a cast through object.
    private static void ProjectedAccessors(CodeWriter code, GameNodeModel node, GameMember member, string projected, GameNodeModel child,
        string childView, bool scoped, GenerationPlan plan, bool defaultImplementation)
    {
        var receiver = defaultImplementation ? $"(({node.Layout.QualifiedName})this)." : "";
        var property = receiver + SyntaxOverlap.Escape(member.Name);
        var name = SymbolDisplay.FormatLiteral(member.Name, true);
        var nullable = member.Type.EndsWith("?", StringComparison.Ordinal);
        var field = member.Field!;
        if (field.StorageDeclaration.Type.ArrayDimensions == 0)
        {
            var required = scoped && !nullable;
            var value = required ? $"{property} ?? throw new MemberNullException({name})" : property;
            code.Line(defaultImplementation
                ? $"get => ({projected})({(required ? "object" : "object?")})({value});"
                : $"get => {value};");
            if (member.Writable)
                code.Line($"set => {property} = value is null ? {(nullable ? "null" : "throw new ArgumentNullException(nameof(value))")} : value is {child.Layout.QualifiedName} node ? node : throw new ArgumentException(\"A GBX.NET node is required.\", nameof(value));");
            return;
        }
        var element = GameNodeModel.Qualify(ReferenceModel.Element(field), member.Owner, plan);
        var view = childView + (element.EndsWith("?", StringComparison.Ordinal) ? "?" : "");
        var requiredElement = !element.EndsWith("?", StringComparison.Ordinal);
        code.Line($"get => {(nullable ? property + " is null ? null : " : "")}new global::GBX.NET.Interfaces.ScopedNodeList<{element}, {view}>({receiver}Get{member.Name}InterfaceReferences, {(requiredElement ? "true" : "false")}, {name});");
        if (member.Writable)
            code.Line($"set => {property} = value is null ? {(nullable ? "null" : "throw new ArgumentNullException(nameof(value))")} : new NodeRefList<{element}>(global::GBX.NET.Interfaces.NodeInterfaceReferences.ToNodes<{element}, {view}>(value, {(requiredElement ? "false" : "true")}));");
    }

    // A null check on a value type would not compile. Engine types are classes
    // when they are nodes, archives or nested classes declared by hand.
    private static bool IsReferenceType(string type, GenerationPlan plan)
    {
        if (type is "string" or "Ident" or "PackDesc" || type.EndsWith("[]", StringComparison.Ordinal)) return true;
        var name = type.StartsWith("global::", StringComparison.Ordinal) ? type.Substring("global::".Length) : type;
        var generic = name.IndexOf('<');
        if (generic >= 0)
        {
            return name.Substring(0, generic) is "List" or "NodeRefList" or "Dictionary" or "GBX.NET.NodeRefList" or
                "System.Collections.Generic.List" or "System.Collections.Generic.Dictionary";
        }
        if (!name.StartsWith("GBX.NET.Engines.", StringComparison.Ordinal)) return false;
        if (plan.Layouts.Values.Any(l => l.File.TypeKey == name)) return true;
        var dot = name.LastIndexOf('.');
        var simple = name.Substring(dot + 1);
        var container = plan.Layouts.Values.FirstOrDefault(l => l.File.TypeKey == name.Substring(0, dot));
        if (container is null) return false;
        if (container.Archives.ContainsKey(simple)) return true;
        return plan.Existing.TryGetValue(container.File.TypeKey + "+" + simple, out var nested) &&
            nested.Declarations.All(static d => d is ClassDeclarationSyntax || d is RecordDeclarationSyntax record &&
                !record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword));
    }
}
