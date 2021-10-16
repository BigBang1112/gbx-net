using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GBX.NET.Json
{
    public class GameBoxContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            if(objectType.BaseType == typeof(Task))
                return new List<MemberInfo> { objectType.GetProperty("Result") };

            return objectType
                .GetProperties()
                .Where(x => x.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                .Cast<MemberInfo>().ToList();
        }
    }
}
