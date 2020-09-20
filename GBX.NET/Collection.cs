using System.Linq;

namespace GBX.NET
{
    public struct Collection
    {
        public int? ID { get; }

        public Collection(int collectionID)
        {
            ID = collectionID;
        }

        public override string ToString()
        {
            if (ID.HasValue)
            {
                if (LookbackString.CollectionIDs.TryGetValue(ID.Value, out string value))
                    return value;
                return ID.ToString();
            }
            return "";
        }

        public static implicit operator Collection(string a) => string.IsNullOrEmpty(a) ? new Collection() : new Collection(LookbackString.CollectionIDs.FirstOrDefault(x => x.Value == a).Key);
        public static implicit operator Collection(LookbackString a) => a.Index == -1 ? new Collection() : new Collection(LookbackString.CollectionIDs.FirstOrDefault(x => x.Value == a).Key);
        public static implicit operator Collection(int a) => new Collection(a);
        public static implicit operator string(Collection a) => a.ToString();
    }
}
