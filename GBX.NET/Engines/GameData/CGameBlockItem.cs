using System.Collections.Generic;
using System.Diagnostics;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E025000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameBlockItem : Node
    {
        #region Fields

        private string archetypeBlockInfoId;
        private string archetypeBlockInfoCollectionId;
        private Dictionary<int, CPlugCrystal> customizedVariants;

        #endregion

        #region Properties

        [NodeMember]
        public string ArchetypeBlockInfoId
        {
            get => archetypeBlockInfoId;
            set => archetypeBlockInfoId = value;
        }

        [NodeMember]
        public string ArchetypeBlockInfoCollectionId
        {
            get => archetypeBlockInfoCollectionId;
            set => archetypeBlockInfoCollectionId = value;
        }

        [NodeMember]
        public Dictionary<int, CPlugCrystal> CustomizedVariants
        {
            get => customizedVariants;
            set => customizedVariants = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameBlockItem 0x000 chunk
        /// </summary>
        [Chunk(0x2E025000)]
        public class Chunk2E025000 : Chunk<CGameBlockItem>
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameBlockItem n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Id(ref n.archetypeBlockInfoId);
                rw.Id(ref n.archetypeBlockInfoCollectionId);
                rw.DictionaryNode(ref n.customizedVariants);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameBlockItem node;

            public string ArchetypeBlockInfoId => node.ArchetypeBlockInfoId;
            public string ArchetypeBlockInfoCollectionId => node.ArchetypeBlockInfoCollectionId;
            public Dictionary<int, CPlugCrystal> CustomizedVariants => node.CustomizedVariants;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameBlockItem node) => this.node = node;
        }

        #endregion
    }
}
