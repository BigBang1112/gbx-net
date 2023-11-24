namespace GBX.NET.Builders.Engines.Script;

public partial class CScriptTraitsMetadataBuilder : Builder
{
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    internal static CScriptTraitsMetadata NewNode() => new();

    public class TM2 : GameBuilder<CScriptTraitsMetadataBuilder, CScriptTraitsMetadata>
    {
        public TM2(CScriptTraitsMetadataBuilder baseBuilder, CScriptTraitsMetadata node) : base(baseBuilder, node) { }

        public override CScriptTraitsMetadata Build()
        {
            Node.CreateChunk<CScriptTraitsMetadata.Chunk11002000>().Version = 5;
            return Node;
        }
    }

    public class TM2020 : GameBuilder<CScriptTraitsMetadataBuilder, CScriptTraitsMetadata>
    {
        public TM2020(CScriptTraitsMetadataBuilder baseBuilder, CScriptTraitsMetadata node) : base(baseBuilder, node) { }

        public override CScriptTraitsMetadata Build()
        {
            Node.CreateChunk<CScriptTraitsMetadata.Chunk11002000>().Version = 6;
            return Node;
        }
    }
}