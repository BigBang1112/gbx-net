using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class Meta
    {
        public string ID { get; set; }
        public string Collection { get; set; }
        public string Author { get; set; }

        public Meta()
        {

        }

        public Meta(string id)
        {
            ID = id;
        }

        public Meta(string id, string collection, string author)
        {
            ID = id;
            Collection = collection;
            Author = author;
        }

        public override string ToString()
        {
            return $"(\"{ID}\", \"{Collection}\", \"{Author}\")";
        }

        public static implicit operator Meta((string ID, string Collection, string Author) v)
        {
            return new Meta(v.Item1, v.Item2, v.Item3);
        }

        public static implicit operator (string ID, string Collection, string Author)(Meta v)
        {
            return (v.ID, v.Collection, v.Author);
        }
    }
}
