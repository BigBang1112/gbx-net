using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public struct Collection
    {
        public int ID { get; }

        public Collection(int collectionID)
        {
            ID = collectionID;
        }

        public static implicit operator string(Collection a) => LookbackString.CollectionIDs[a.ID];
    }
}
