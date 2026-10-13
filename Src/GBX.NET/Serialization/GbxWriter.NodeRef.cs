using GBX.NET.Components;

namespace GBX.NET.Serialization;

public partial class GbxWriter
{
    public void WriteNodeRef(NodeRef? nodeRef)
    {
        var value = nodeRef?.CachedNode;
        var descriptor = nodeRef?.Descriptor;

        if (descriptor is not null && encapsulation is not null)
        {
            throw new NotSupportedException("External references cannot be written in an encapsulated context.");
        }

        if (descriptor is not null && ReferenceTable is not null)
        {
            if (ReferenceTable.InferAncestorLevelOnWrite)
            {
                ReferenceTable.AncestorLevel = descriptor.RefTable.AncestorLevel;
                ReferenceTable.InferAncestorLevelOnWrite = false;
            }

            importedRefs ??= [];

            if (!importedRefs.TryGetValue(descriptor, out var imported))
            {
                imported = ReferenceTable.Import(descriptor);
                importedRefs.Add(descriptor, imported);
            }

            descriptor = imported;
        }

        if (value is null && descriptor is null)
        {
            Write(-1);
            return;
        }

        if (encapsulation is null)
        {
            if ((descriptor is not null && NodeDict.TryGetValue(descriptor, out int index))
                || (descriptor is null && value is not null && NodeDict.TryGetValue(value, out index)))
            {
                Write(index);
                return;
            }

            var key = descriptor ?? (object?)value ?? throw new InvalidOperationException("Descriptor or value is null.");

            if (nextNodeIndex == 1 && NodeDict.Count > 0)
            {
                nextNodeIndex = NodeDict.Values.Max() + 1;
            }

            index = nextNodeIndex++;

            NodeDict[key] = index;

            Write(index);

            if (descriptor is not null)
            {
                return;
            }
        }

        if (value is null)
        {
            throw new InvalidOperationException("Value is null.");
        }

        var classId = value.GetId();

        if (!value.IsWriteSupported)
        {
            throw new ClassWriteNotSupportedException(classId);
        }

        WriteClassId(classId);

        rw ??= new GbxReaderWriter(this);

        value.ReadWrite(rw);
    }

    public void WriteListNodeRef<T>(NodeRefList<T>? references, int? length = null) where T : CMwNod
    {
        var count = length ?? references?.Count ?? 0;

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (!length.HasValue)
        {
            Write(count);
        }

        for (var i = 0; i < count; i++)
        {
            WriteNodeRef(references is not null && i < references.Count ? references.Get(i) : null);
        }
    }

    public void WriteListNodeRef_deprec<T>(NodeRefList<T>? references, int? length = null) where T : CMwNod
    {
        WriteDeprecVersion();
        WriteListNodeRef(references, length);
    }

    public void WriteJaggedArrayNodeRef<T>(
        NodeRefList<T>[]? references,
        int? innerLength = null,
        int? outerLength = null) where T : CMwNod
    {
        var count = outerLength ?? references?.Length ?? 0;

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(outerLength));
        }

        if (!outerLength.HasValue)
        {
            Write(count);
        }

        for (var i = 0; i < count; i++)
        {
            WriteListNodeRef(references is not null && i < references.Length ? references[i] : null, innerLength);
        }
    }
}
