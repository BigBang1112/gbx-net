using GBX.NET.Components;
using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET;

/// <summary>
/// A scalar curve stored as CFuncKeysReal or inline control points.
/// </summary>
public sealed class PlugCurve : IReadableWritable, IReadable, IWritable, IVersionable, IDeepCloneable
{
    private CFuncKeysReal? curve;
    private GbxRefTableFile? curveFile;
    private int count;
    private ushort flags;
    private byte flagsV1;
    private int capacity = 7;
    private Vec2[]? values;

    /// <summary>Gets or sets the original legacy curve node.</summary>
    public CFuncKeysReal? Curve { get => curve; set => curve = value; }
    public GbxRefTableFile? CurveFile { get => curveFile; set => curveFile = value; }
    /// <summary>Gets or sets the inline curve format version.</summary>
    public int Version { get; set; } = 3;
    /// <summary>Gets or sets the serialized point count. Writing uses the length of <see cref="Values"/>.</summary>
    public int Count { get => count; set => count = value; }
    public ushort Flags { get => flags; set => flags = value; }
    public byte FlagsV1 { get => flagsV1; set => flagsV1 = value; }
    public int Capacity { get => capacity; set => capacity = value; }
    public Vec2[]? Values { get => values; set => values = value; }

    public void Read(GbxReader r, int v = 0) => ReadWrite(new GbxReaderWriter(r), v);
    public void Write(GbxWriter w, int v = 0) => ReadWrite(new GbxReaderWriter(w), v);

    /// <summary>Reads or writes a legacy reference for negative versions, or an inline curve for zero and later versions.</summary>
    public void ReadWrite(GbxReaderWriter rw, int version = 0) => ReadWrite(rw, version, useSimpleNode: false);

    internal void ReadWrite(GbxReaderWriter rw, int version, bool useSimpleNode)
    {
        if (version < 0)
        {
            if (rw.Writer is not null && curve is null && curveFile is null && values is { Length: > 0 })
            {
                curve = useSimpleNode ? CreateSimpleNode() : CreateFunctionNode();
            }

            rw.NodeRef<CFuncKeysReal>(ref curve, ref curveFile);
            if (rw.Reader is not null)
            {
                ReadLegacyCurve();
            }
            return;
        }

        rw.VersionInt32(this);
        count = rw.Int32(values?.Length ?? 0);
        if (Version == 1)
        {
            rw.Byte(ref flagsV1);
            if (rw.Reader is not null)
            {
                Flags = (ushort)(flagsV1 > 0 ? 1 : 0);
            }
        }
        if (Version == 2)
        {
            rw.Int32(ref capacity);
        }
        if (Version >= 3)
        {
            rw.UInt16(ref flags);
        }
        rw.Array<Vec2>(ref values, count);
    }

    private CFuncKeysReal CreateFunctionNode()
    {
        var node = new CFuncKeysReal
        {
            Xs = Array.ConvertAll(values!, value => value.X),
            Ys = Array.ConvertAll(values!, value => value.Y),
            RealInterp = (flags & 1) != 0 ? CFuncKeysReal.ERealInterp.None
                : (flags & 0x10) != 0 ? CFuncKeysReal.ERealInterp.Hermite
                : (flags & 2) != 0 ? CFuncKeysReal.ERealInterp.SmoothStep
                : CFuncKeysReal.ERealInterp.Linear,
            ForceTangentMaxX = (flags & 0x20) != 0
        };
        node.CreateChunk<CFuncKeys.Chunk05002001>();
        node.CreateChunk<CFuncKeysReal.Chunk0501A002>();
        return node;
    }

    /// <summary>Converts a legacy node to control points, preserving the original node. A null node produces an empty curve.</summary>
    public static implicit operator PlugCurve(CFuncKeysReal? node)
    {
        var result = new PlugCurve { curve = node };
        result.ReadLegacyCurve();
        return result;
    }

    /// <summary>Returns the original legacy node, or creates one from the control points.</summary>
    public static implicit operator CFuncKeysReal?(PlugCurve? value)
    {
        if (value is null)
        {
            return null;
        }
        if (value.curve is null && value.curveFile is null && value.values is { Length: > 0 })
        {
            value.curve = value.CreateFunctionNode();
        }
        return value.curve;
    }

    private CPlugCurveSimpleNod CreateSimpleNode()
    {
        var node = new CPlugCurveSimpleNod
        {
            Curve = new PlugCurve
            {
                Version = Version,
                Count = values!.Length,
                FlagsV1 = flagsV1,
                Capacity = capacity,
                Flags = flags,
                Values = (Vec2[])values.Clone()
            }
        };
        node.CreateChunk<CPlugCurveSimpleNod.Chunk09185000>();
        return node;
    }

    private void ReadLegacyCurve()
    {
        if (curve is CPlugCurveSimpleNod simple && simple.Chunks.Get<CPlugCurveSimpleNod.Chunk09185000>() is not null)
        {
            Version = simple.Curve.Version;
            FlagsV1 = simple.Curve.FlagsV1;
            Capacity = simple.Curve.Capacity;
            Flags = simple.Curve.Version switch
            {
                1 => (ushort)(simple.Curve.FlagsV1 > 0 ? 1 : 0),
                >= 3 => simple.Curve.Flags,
                _ => 0
            };
            Values = simple.Curve.Values is null ? [] : (Vec2[])simple.Curve.Values.Clone();
        }
        else
        {
            Flags = curve?.RealInterp switch
            {
                CFuncKeysReal.ERealInterp.None => 1,
                CFuncKeysReal.ERealInterp.Hermite => 0x10,
                CFuncKeysReal.ERealInterp.SmoothStep => 2,
                _ => 0
            };
            if (curve?.ForceTangentMaxX == true)
            {
                Flags |= 0x20;
            }
            Values = new Vec2[Math.Min(curve?.Xs?.Length ?? 0, curve?.Ys?.Length ?? 0)];
            for (var i = 0; i < Values.Length; i++)
            {
                Values[i] = new Vec2(curve!.Xs![i], curve.Ys![i]);
            }
        }
        count = Values.Length;
    }

    object IDeepCloneable.DeepClone(DeepCloneContext context)
    {
        var clone = (PlugCurve)MemberwiseClone();
        context.Register(this, clone);
        clone.curve = context.Clone(curve);
        clone.values = context.CloneArray(values);
        return clone;
    }
}
