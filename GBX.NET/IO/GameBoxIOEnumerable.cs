using System.Collections.Generic;

namespace GBX.NET.IO
{
    public abstract class GameBoxIOEnumerable<T> : GameBoxIOCustom<T, IEnumerable<GameBox<T>>> where T : Node
    {
        public GameBoxIOEnumerable(GameBox gbx) : base(gbx)
        {

        }

        public GameBoxIOEnumerable(GameBox<T> gbx) : base(gbx)
        {

        }
    }
}
