namespace GBX.NET
{
    public class Meta
    {
        public string ID { get; set; }
        public Collection Collection { get; set; }
        public string Author { get; set; }

        public Meta()
        {

        }

        public Meta(string id)
        {
            ID = id;
        }

        public Meta(string id, Collection collection, string author)
        {
            ID = id;
            Collection = collection;
            Author = author;
        }

        public override string ToString() => $"(\"{ID}\", \"{Collection}\", \"{Author}\")";
        public override int GetHashCode() => ID.GetHashCode() ^ Collection.GetHashCode() ^ Author.GetHashCode();
        public override bool Equals(object obj) => this is Meta m && this == m;

        public static bool operator ==(Meta a, Meta b) => a?.ID == b?.ID && a?.Collection == b?.Collection && a?.Author == b?.Author;
        public static bool operator !=(Meta a, Meta b) => !(a?.ID == b?.ID && a?.Collection == b?.Collection && a?.Author == b?.Author);

        public static implicit operator Meta((string ID, Collection Collection, string Author) v) => new Meta(v.ID, v.Collection, v.Author);
        public static implicit operator (string ID, Collection Collection, string Author)(Meta v) => (v.ID, v.Collection, v.Author);
    }
}
