using GBX.NET.ChunkExplorer.Models;
using GBX.NET.Engines.MwFoundations;
using Mapster;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace GBX.NET.ChunkExplorer.Mapping.Adapters
{
    public static class NodeAdapter
    {
        public static void Configure()
        {
            /*_ = TypeAdapterConfig<CMwNod, MainNodeModel>
              .NewConfig()
              .Map(dest => dest.Type, src => src.GetType())
              .Map(dest => dest.Node, dest => dest)
              .Map(dest => dest.FileName, src => src.GBX.FileName)*/
              //.Map(dest => dest.NodeEnumerables, src => new ObservableCollection<IEnumerable<NodeModel>>(src.GetType()
              //    .GetProperties()
              //    .Where(x => x.PropertyType.IsSubclassOf(typeof(IEnumerable)) && x.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsSubclassOf(typeof(CMwNod))) != null)
              //    .Select(x => x.GetValue(src).Adapt<IEnumerable<CMwNod>>().Select(x => new NodeModel { Property = x. })))
              //)
              /*.Map(dest => dest.Nodes, src => new ObservableCollection<NodeModel>(src.GetType()
                  .GetProperties()
                  .Where(x => x.PropertyType.IsSubclassOf(typeof(CMwNod)) && x.GetValue(src) != null)
                  .Select(x => x.GetValue(src).Adapt<NodeModel>().WithName(x.Name)))
              );*/
              /*.Map(dest => dest.Nodes, src => new ObservableCollection<NodeModel>(src.GetType()
                  .GetProperties()
                  .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType) && x.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsSubclassOf(typeof(CMwNod))) != null)
                  .Select(x => new NodeModel { Property = x }))
              );*/
        }
    }
}
