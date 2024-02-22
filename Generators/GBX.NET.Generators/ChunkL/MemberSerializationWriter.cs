using ChunkL.Structure;
using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators.ChunkL;

internal sealed class MemberSerializationWriter
{
    private readonly StringBuilder sb;
    private readonly SerializationType serializationType;
    private readonly bool self;
    private readonly SourceProductionContext context;

    public MemberSerializationWriter(
        StringBuilder sb, 
        SerializationType serializationType,
        bool self,
        SourceProductionContext context)
    {
        this.sb = sb;
        this.serializationType = serializationType;
        this.self = self;
        this.context = context;
    }

    public void Append(int indent, IEnumerable<IChunkMember> members)
    {
        sb.AppendLine(indent, "throw new NotImplementedException();");
    }
}
