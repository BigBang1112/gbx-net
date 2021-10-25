using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using Mapster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.ChunkExplorer.Models
{
    public class MainNodeModel : NodeModel
    {
        public string FileName { get; set; }
        public string ShortFileName => Path.GetFileName(FileName);

        public MainNodeModel(CMwNod node)
        {
            Type = node.GetType();
            Node = node;
            FileName = node.GBX!.FileName!;
            AuxNodes = GetAuxNodes(Type, node);
        }

        private IEnumerable<SingleNodeModel> GetAuxNodes(Type type, CMwNod? node)
        {
            if (node is null)
                yield break;

            var allProperties = type.GetProperties();

            var nodeCollectionProperties = allProperties.Where(prop =>
                typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                && prop.PropertyType.IsGenericType
                && (prop.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsSubclassOf(typeof(CMwNod))) != null
                || prop.PropertyType.GenericTypeArguments.Contains(typeof(CGameCtnMediaClipGroup.ClipTrigger))));

            foreach (var prop in nodeCollectionProperties)
            {
                var propValues = (prop.GetValue(node) as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();

                yield return new SingleNodeModel
                {
                    TypeName = ToGenericTypeString(prop.PropertyType),
                    Type = prop.PropertyType,
                    Name = prop.Name,
                    AuxNodes = propValues.Select((propValue, i) =>
                    {
                        var nod = propValue switch
                        {
                            CGameCtnMediaClipGroup.ClipTrigger clipTrigger => clipTrigger.Clip,
                            _ => propValue as CMwNod,
                        };

                        var elementType = nod is null ? propValue.GetType() : nod.GetType();

                        return new ElementNodeModel
                        {
                            TypeName = elementType.Name,
                            Type = elementType,
                            Node = nod,
                            Key = i,
                            AuxNodes = GetAuxNodes(elementType, nod)
                        };
                    })
                };
            }

            var nodeArrayProperties = allProperties.Where(prop =>
                prop.PropertyType.IsArray && prop.PropertyType.GetElementType()?.IsSubclassOf(typeof(CMwNod)) == true);

            foreach (var prop in nodeArrayProperties)
            {
                var propValues = (prop.GetValue(node) as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();

                yield return new SingleNodeModel
                {
                    TypeName = prop.PropertyType.Name,
                    Type = prop.PropertyType,
                    Name = prop.Name,
                    AuxNodes = propValues.Select((propValue, i) =>
                    {
                        var elementType = propValue.GetType();

                        var nod = propValue as CMwNod;

                        return new ElementNodeModel
                        {
                            TypeName = elementType.Name,
                            Type = elementType,
                            Node = nod,
                            Key = i,
                            AuxNodes = GetAuxNodes(elementType, nod)
                        };
                    })
                };
            }

            var nodeProperties = allProperties.Where(prop => prop.PropertyType.IsSubclassOf(typeof(CMwNod)) && prop.GetGetMethod()?.IsStatic == false);

            foreach (var prop in nodeProperties)
            {
                var nod = prop.GetValue(node) as CMwNod;

                yield return new SingleNodeModel
                {
                    TypeName = prop.PropertyType.Name,
                    Type = prop.PropertyType,
                    Node = nod,
                    Name = prop.Name,
                    AuxNodes = GetAuxNodes(prop.PropertyType, nod)
                };
            }
        }

        private static string ToGenericTypeString(Type t)
        {
            if (!t.IsGenericType)
                return t.Name;
            string genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0,
                genericTypeName.IndexOf('`'));
            string genericArgs = string.Join(",",
                t.GetGenericArguments()
                    .Select(ta => ToGenericTypeString(ta)).ToArray());
            return genericTypeName + "<" + genericArgs + ">";
        }
    }
}
