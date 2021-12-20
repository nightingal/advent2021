using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = new Dictionary<string, Node>();

            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day12\input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var input = reader.ReadLine().Split("-");

                    var node1 = nodes.ContainsKey(input[0]) ? nodes[input[0]] : new Node(input[0]);
                    var node2 = nodes.ContainsKey(input[1]) ? nodes[input[1]] : new Node(input[1]);

                    node1.AddAdjacent(node2);
                    node2.AddAdjacent(node1);

                    if (!nodes.ContainsKey(node1.Name))
                    {
                        nodes.Add(node1.Name, node1);
                    }

                    if (!nodes.ContainsKey(node2.Name))
                    {
                        nodes.Add(node2.Name, node2);
                    }
                }
            }

            var start = nodes["start"];

            var completePaths = new List<Path>();
            var paths = new List<Path>();

            paths.Add(new Path(new List<Node>() { start }));

            while (paths.Count > 0)
            {
                var path = paths[0];

                paths.RemoveAt(0);

                var lastNode = path.Nodes.Last();

                if (path.IsComplete)
                {
                    completePaths.Add(path);
                }
                else
                {
                    var createdPath = new List<Path>();
                    foreach (var adjacent in lastNode.AdjacentNodes)
                    {
                        if (!adjacent.IsMinor || !path.Nodes.Contains(adjacent))
                        {
                            var newPath = new Path(path.Nodes);
                            newPath.AddNode(adjacent);

                            if (path.HasVisitedMinorCaveTwice)
                            {
                                newPath.MarkMinorVisitedTwice();
                            }

                            createdPath.Add(newPath);

                            paths.Add(newPath);
                        }
                        else if (adjacent.IsMinor && path.Nodes.Contains(adjacent) && !path.HasVisitedMinorCaveTwice && !adjacent.IsStart)
                        {
                            var newPath = new Path(path.Nodes);
                            newPath.AddNode(adjacent);

                            newPath.MarkMinorVisitedTwice();

                            paths.Add(newPath);
                        }
                    }
                }
            }

            Console.WriteLine("Complete path count: " + completePaths.Count);
        }

        public class Node
        {
            public string Name { get; }

            public List<Node> AdjacentNodes { get; }

            public bool IsMinor { get; }

            public bool IsStart => this.Name == "start";

            public bool IsEnd => this.Name == "end";

            public Node(string name)
            {
                this.Name = name;
                this.IsMinor = char.IsLower(name[0]);
                this.AdjacentNodes = new List<Node>();
            }

            public void AddAdjacent(Node node)
            {
                if (!this.AdjacentNodes.Contains(node))
                {
                    this.AdjacentNodes.Add(node);
                }
            }
        }

        public class Path
        {
            public List<Node> Nodes { get; }

            public bool IsComplete => this.Nodes.Last().IsEnd;

            public bool HasVisitedMinorCaveTwice { get; private set; }

            public Path(List<Node> nodes)
            {
                this.Nodes = new List<Node>(nodes);
            }

            public void AddNode(Node node)
            {
                this.Nodes.Add(node);
            }

            public void MarkMinorVisitedTwice()
            {
                this.HasVisitedMinorCaveTwice = true;
            }
        }
    }
}
