using GBX.NET;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

using GBX.NET.Engines.MwFoundations;

namespace DocGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.Load("GBX.NET");
            var nodes = assembly.DefinedTypes.Where(x => x != null && x.Namespace != null && x.Namespace.StartsWith("GBX.NET.Engines") && !x.IsNested);

            foreach (var node in nodes)
            {
                var folder = "doc/" + node.Namespace["GBX.NET.Engines.".Length..].Replace('.', '/');
                Directory.CreateDirectory(folder);

                using (StringWriter w = new StringWriter())
                {
                    var nodAttribute = node.GetCustomAttribute<NodeAttribute>();

                    if (nodAttribute != null)
                    {
                        w.WriteLine($"# {node.Name} (0x{nodAttribute.ID.ToString("x8").ToUpper()})");
                        w.WriteLine();

                        if (node.BaseType != typeof(CMwNod) && node.BaseType != typeof(object))
                        {
                            w.WriteLine($"### Inherits [{node.BaseType.Name}]({node.BaseType.Name}.md)");
                            w.WriteLine();
                        }

                        var chunks = node.DeclaredNestedTypes.Where(x => x.GetCustomAttribute<ChunkAttribute>() != null);

                        if (chunks.Count() > 0)
                        {
                            w.WriteLine("## Chunks");
                            w.WriteLine();

                            foreach (var chunk in chunks)
                            {
                                var att = chunk.GetCustomAttribute<ChunkAttribute>();

                                var linkName = $"0x{att.ChunkID.ToString("x3").ToUpper()}{(chunk.BaseType.GetInterface("IHeaderChunk") == null ? (chunk.BaseType.GetInterface("ISkippableChunk") == null ? "" : " - skippable") : " - header chunk")}{(att.Description == null ? "" : $" ({att.Description})")}";

                                w.WriteLine($"- [{linkName}](#{linkName.Replace(' ', '-').Replace("(", null).Replace(")", null).ToLower()})");
                            }

                            foreach (var chunk in chunks)
                            {
                                var att = chunk.GetCustomAttribute<ChunkAttribute>();

                                var linkName = $"0x{att.ChunkID.ToString("x3").ToUpper()}{(chunk.BaseType.GetInterface("IHeaderChunk") == null ? (chunk.BaseType.GetInterface("ISkippableChunk") == null ? "" : " - skippable") : " - header chunk")}{(att.Description == null ? "" : $" ({att.Description})")}";

                                w.WriteLine();
                                w.WriteLine("### " + linkName);
                                w.WriteLine();
                                w.WriteLine("```cs");
                                w.WriteLine("void Read(GameBoxReader r)");
                                w.WriteLine("{");
                                w.WriteLine("    ");
                                w.WriteLine("}");
                                w.WriteLine("```");
                            }
                        }

                        Console.WriteLine(w.ToString());
                        File.WriteAllText($"{folder}/{node.Name}.md", w.ToString());
                    }
                }
            }
        }
    }
}
