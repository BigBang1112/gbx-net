namespace GBX.NET
{
    /// <summary>
    /// Identifier defined by ID, collection and author. Also known as "meta".
    /// </summary>
    public struct Ident
    {
        public string ID { get; set; }
        public Collection Collection { get; set; }
        public string Author { get; set; }

        public Ident(string id, Collection collection, string author)
        {
            ID = id;
            Collection = collection;
            Author = author;
        }

        public Ident(string id) : this(id, new Collection(), string.Empty)
        {
            
        }

        public override string ToString() => $"(\"{ID}\", \"{Collection}\", \"{Author}\")";
        public override int GetHashCode() => ID.GetHashCode() ^ Collection.GetHashCode() ^ Author.GetHashCode();
        public override bool Equals(object obj) => this is Ident m && this == m;

        public static bool operator ==(Ident a, Ident b) => a.ID == b.ID && a.Collection == b.Collection && a.Author == b.Author;
        public static bool operator !=(Ident a, Ident b) => !(a.ID == b.ID && a.Collection == b.Collection && a.Author == b.Author);

        public static implicit operator Ident((string ID, Collection Collection, string Author) v) => new Ident(v.ID, v.Collection, v.Author);
        public static implicit operator (string ID, Collection Collection, string Author)(Ident v) => (v.ID, v.Collection, v.Author);
    }
}
