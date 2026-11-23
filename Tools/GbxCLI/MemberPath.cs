using GBX.NET;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace GbxCLI;

/// <summary>
/// A path to a member, such as <c>Node.MapName</c> or <c>Node.Blocks[3].Coord.X</c>, for <c>set</c>.
/// </summary>
/// <remarks>
/// Names are matched exactly, or ignoring case when that finds one member. From a <see cref="Gbx"/>, a first name that the Gbx
/// doesn't have is looked up on its node, so <c>MapName</c> is the same as <c>Node.MapName</c>.
/// </remarks>
internal sealed class MemberPath
{
    private const string RootedJustification = "GBX.NET, GBX.NET.PAK and TmEssentials are rooted with TrimmerRootAssembly, so the metadata of their members is kept.";

    private readonly Segment[] segments;

    public string Text { get; }

    private MemberPath(string text, Segment[] segments)
    {
        Text = text;
        this.segments = segments;
    }

    /// <exception cref="FormatException">The text is not a member path.</exception>
    public static MemberPath Parse(string text)
    {
        var segments = new List<Segment>();
        var i = 0;

        while (i < text.Length)
        {
            if (text[i] == '[')
            {
                var end = text.IndexOf(']', i);

                if (end < 0 || !int.TryParse(text.AsSpan(i + 1, end - i - 1), NumberStyles.None, CultureInfo.InvariantCulture, out var index))
                {
                    throw new FormatException($"Member path '{text}' has an invalid index. Use a number, such as Blocks[3].");
                }

                segments.Add(new Segment(null, index));
                i = end + 1;
                continue;
            }

            // A dot separates names, so it cannot start the path.
            if (text[i] == '.' && segments.Count > 0)
            {
                i++;
            }

            var start = i;

            while (i < text.Length && (char.IsLetterOrDigit(text[i]) || text[i] == '_'))
            {
                i++;
            }

            if (i == start)
            {
                throw new FormatException($"Member path '{text}' is invalid. Use names separated by dots, such as Node.MapName.");
            }

            segments.Add(new Segment(text[start..i], null));
        }

        return segments.Count > 0 ? new MemberPath(text, [.. segments]) : throw new FormatException("Member path cannot be empty.");
    }

    /// <summary>
    /// Converts <paramref name="text"/> with <see cref="ValueParser"/> and assigns it to the member. A struct on the way, such as a vector,
    /// is assigned back to its parent. A record class is copied first, so other members that share it don't change.
    /// </summary>
    /// <returns>The resolved path, such as <c>Node.MapName</c> for <c>mapname</c>, with the old and the new value.</returns>
    /// <exception cref="InvalidOperationException">The path doesn't lead to a member that can be assigned.</exception>
    /// <exception cref="FormatException">The text is not a valid value of the member.</exception>
    public MemberChange Assign(object root, string text)
    {
        var path = new StringBuilder();

        if (root is Gbx { Node: { } node } && segments[0].Name is { } name && FindMember(root.GetType(), name) is null)
        {
            if (FindMember(node.GetType(), name) is null)
            {
                throw new InvalidOperationException($"Neither the Gbx nor its {ValueFormatter.GetTypeName(node.GetType())} node has a member '{name}'.");
            }

            root = node;
            path.Append(nameof(Gbx.Node));
        }

        object? oldValue = null;
        object? newValue = null;

        // Returns the target, which a caller assigns back when it is a copy.
        object AssignAt(object target, int index)
        {
            var member = Resolve(target, segments[index], path);

            if (index == segments.Length - 1)
            {
                oldValue = member.GetValue();
                newValue = ValueParser.Parse(text, member.Type);
                member.SetValue(newValue);
                return target;
            }

            var value = member.GetValue() ?? throw new InvalidOperationException($"{path} is null, so it has no members.");

            // A value type was boxed as a copy. A record is copied when it can be assigned back.
            var isCopy = value.GetType().IsValueType;

            if (!isCopy && member.CanWrite && Clone(value) is { } clone)
            {
                value = clone;
                isCopy = true;
            }

            var memberPath = path.ToString();
            var updated = AssignAt(value, index + 1);

            if (isCopy)
            {
                if (!member.CanWrite)
                {
                    throw new InvalidOperationException($"{memberPath} is read-only, so its members cannot be changed.");
                }

                member.SetValue(updated);
            }

            return target;
        }

        AssignAt(root, 0);

        return new MemberChange(path.ToString(), oldValue, newValue);
    }

    private static Member Resolve(object target, Segment segment, StringBuilder path)
    {
        if (segment.Index is not { } index)
        {
            var info = FindMember(target.GetType(), segment.Name!)
                ?? throw new InvalidOperationException($"{(path.Length == 0 ? ValueFormatter.GetTypeName(target.GetType()) : path)} has no member '{segment.Name}'.");

            if (path.Length > 0)
            {
                path.Append('.');
            }

            path.Append(info.Name);

            return new PropertyOrFieldMember(target, info, path.ToString());
        }

        var parentPath = path.ToString();
        path.Append('[').Append(index).Append(']');

        if (target is not IList list)
        {
            throw new InvalidOperationException($"{parentPath} is not a list, so it has no item [{index}].");
        }

        if (index >= list.Count)
        {
            throw new InvalidOperationException($"{parentPath} has {list.Count:N0} items, so it has no item [{index}].");
        }

        return new ListItemMember(list, index, GetItemType(list.GetType()), path.ToString());
    }

    /// <summary>
    /// Finds a public instance property or field. The most derived one wins, as with a property hidden with <c>new</c>.
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = RootedJustification)]
    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = RootedJustification)]
    private static MemberInfo? FindMember(Type type, string name)
    {
        var members = new List<MemberInfo>();

        for (var current = type; current is not null; current = current.BaseType)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

            members.AddRange(current.GetProperties(flags).Where(property => property.GetMethod?.IsPublic == true && property.GetIndexParameters().Length == 0));
            members.AddRange(current.GetFields(flags));
        }

        if (members.FirstOrDefault(member => member.Name == name) is { } exact)
        {
            return exact;
        }

        var matches = members.Where(member => string.Equals(member.Name, name, StringComparison.OrdinalIgnoreCase)).DistinctBy(member => member.Name).ToArray();

        return matches.Length switch
        {
            0 => null,
            1 => matches[0],
            _ => throw new InvalidOperationException($"Member '{name}' is ambiguous. Use one of: {string.Join(", ", matches.Select(member => member.Name))}.")
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = RootedJustification)]
    private static Type GetItemType(Type listType)
        => listType.IsArray ? listType.GetElementType()!
            : listType.GetInterfaces().FirstOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))?.GetGenericArguments()[0]
            ?? typeof(object);

    /// <summary>
    /// Copies a record class through its compiler-generated clone method.
    /// </summary>
    /// <returns>The copy, or null if the value is not a record.</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = RootedJustification)]
    private static object? Clone(object value)
        => value.GetType().GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes)?.Invoke(value, null);

    private readonly record struct Segment(string? Name, int? Index);

    private abstract class Member(Type type, string path)
    {
        public Type Type { get; } = type;

        /// <summary>
        /// The resolved path of the member, for messages.
        /// </summary>
        protected string Path { get; } = path;

        public abstract bool CanWrite { get; }

        public abstract object? GetValue();

        protected abstract void SetValueCore(object? value);

        public void SetValue(object? value)
        {
            if (!CanWrite)
            {
                throw new InvalidOperationException($"{Path} is read-only.");
            }

            try
            {
                SetValueCore(value);
            }
            catch (TargetInvocationException exception) when (exception.InnerException is { } inner)
            {
                throw new InvalidOperationException($"{Path} cannot be set: {inner.Message}", inner);
            }
            catch (NotSupportedException exception)
            {
                // A fixed or immutable list, for example.
                throw new InvalidOperationException($"{Path} cannot be set: {exception.Message}", exception);
            }
        }
    }

    private sealed class PropertyOrFieldMember(object target, MemberInfo info, string path)
        : Member(info is PropertyInfo property ? property.PropertyType : ((FieldInfo)info).FieldType, path)
    {
        public override bool CanWrite => info switch
        {
            // An init accessor counts, because records and structs often only have that.
            PropertyInfo property => property.SetMethod?.IsPublic == true,
            FieldInfo fieldInfo => !fieldInfo.IsLiteral,
            _ => false
        };

        public override object? GetValue()
        {
            try
            {
                return info is PropertyInfo property ? property.GetValue(target) : ((FieldInfo)info).GetValue(target);
            }
            catch (TargetInvocationException exception) when (exception.InnerException is { } inner)
            {
                throw new InvalidOperationException($"{Path} cannot be read: {inner.Message}", inner);
            }
        }

        protected override void SetValueCore(object? value)
        {
            if (info is PropertyInfo property)
            {
                property.SetValue(target, value);
            }
            else
            {
                ((FieldInfo)info).SetValue(target, value);
            }
        }
    }

    private sealed class ListItemMember(IList list, int index, Type itemType, string path) : Member(itemType, path)
    {
        public override bool CanWrite => !list.IsReadOnly;

        public override object? GetValue() => list[index];

        protected override void SetValueCore(object? value) => list[index] = value;
    }
}
