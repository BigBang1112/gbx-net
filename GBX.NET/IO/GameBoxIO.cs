namespace GBX.NET.IO
{
    public abstract class GameBoxIO<T> : GameBoxIOCustom<T, GameBox<T>> where T : Node
    {
        public GameBoxIO(GameBox gbx) : base(gbx)
        {

        }

        public GameBoxIO(GameBox<T> gbx) : base(gbx)
        {

        }
    }
}
