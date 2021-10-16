using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.IO
{
    public abstract class GameBoxIO<T> : GameBoxIOCustom<T, GameBox<T>> where T : CMwNod
    {
        public GameBoxIO(GameBox gbx) : base(gbx)
        {

        }

        public GameBoxIO(GameBox<T> gbx) : base(gbx)
        {

        }
    }
}
