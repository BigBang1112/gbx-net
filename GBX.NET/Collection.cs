using System.Linq;

namespace GBX.NET
{
    public struct Collection
    {
        public int? ID { get; }
        public string Name { get; }

        public Collection(string name)
        {
            ID = null;
            Name = name;
        }

        public Collection(int collectionID)
        {
            ID = collectionID;
            Name = null;
        }

        public override string ToString()
        {
            if (ID.HasValue)
            {
                if (LookbackString.CollectionIDs.TryGetValue(ID.Value, out string value))
                    return value;
                return ID.ToString();
            }
            return Name;
        }

        public LookbackString ToLookbackString(ILookbackable lookbackable)
        {
            if (ID.HasValue)
                return new LookbackString(ID.ToString(), lookbackable);
            return new LookbackString(Name, lookbackable);
        }

        public static implicit operator Collection(string a) => string.IsNullOrEmpty(a) ? new Collection() : new Collection(LookbackString.CollectionIDs.FirstOrDefault(x => x.Value == a).Key);
        public static implicit operator Collection(LookbackString a)
        {
            if (string.IsNullOrEmpty(a))
                return new Collection();
            else if (int.TryParse(a, out int collectionID))
                return new Collection(collectionID);
            else
                return new Collection(a);
        }
        public static implicit operator Collection(int a) => new Collection(a);
        public static implicit operator string(Collection a) => a.ToString();
    }
}
