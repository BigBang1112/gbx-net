using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Json
{
    public static class JsonExtensions
    {
        static readonly JsonSerializerSettings settings;

        static JsonExtensions()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new GameBoxContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = HandleSerializationError
            };
        }

        public static void HandleSerializationError(object sender, ErrorEventArgs errorArgs)
        {
            errorArgs.ErrorContext.Handled = true;
        }

        public static string ToJson(this GameBox gbx, bool format = true)
        {
            return JsonConvert.SerializeObject(gbx, format ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJson(this CMwNod node, bool format = true)
        {
            return JsonConvert.SerializeObject(node, format ? Formatting.Indented : Formatting.None, settings);
        }
    }
}
