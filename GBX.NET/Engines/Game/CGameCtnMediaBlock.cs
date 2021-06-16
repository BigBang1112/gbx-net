using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block (0x03077000)
    /// </summary>
    [Node(0x03077000)]
    public class CGameCtnMediaBlock : CMwNod
    {
        public abstract class Key
        {
            public TimeSpan Time { get; set; }

            protected Key()
            {

            }

            protected Key(GameBoxReader r)
            {
                Time = r.ReadSingle_s();
            }
        }

        public interface IHasTwoKeys
        {
            TimeSpan Start { get; set; }
            TimeSpan End { get; set; }
        }

        public interface IHasKeys
        {
            IEnumerable<Key> Keys { get; set; }
        }
    }
}