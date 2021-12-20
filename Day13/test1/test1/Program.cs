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
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day13\input.txt"))
            {
                var coords = new List<Tuple<int, int>>();
                var folds = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    var split = line.Split(',');

                    coords.Add(new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1])));
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    folds.Add(line);
                }

                foreach (var fold in folds)
                {
                    var split = fold.Split(' ').Last().Split('=');
                    var foldAxis = split[0];
                    var foldIndex = int.Parse(split[1]);

                    if (foldAxis == "x")
                    {
                        for (var i = 0; i < coords.Count; ++i)
                        {
                            var node = coords[i];

                            if (node.Item1 > foldIndex)
                            {
                                var newNode = new Tuple<int, int>(foldIndex - (node.Item1 - foldIndex), node.Item2);
                                coords[i] = newNode;
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < coords.Count; ++i)
                        {
                            var node = coords[i];

                            if (node.Item2 > foldIndex)
                            {
                                var newNode = new Tuple<int, int>(node.Item1, foldIndex - (node.Item2 - foldIndex));
                                coords[i] = newNode;

                                //Console.WriteLine($"Node 1 {node.Item1},{node.Item2} became {newNode.Item1},{newNode.Item2}");
                            }  
                        }
                    }

                    var newListOfNodes = new List<Tuple<int, int>>();
                    var compareHash = new HashSet<string>();

                    for (var i = coords.Count - 1; i >= 0; --i)
                    {
                        var node = coords[i];
                        var hash = node.Item1 + "__" + node.Item2;

                        if (!compareHash.Contains(hash))
                        {
                            compareHash.Add(hash);
                            newListOfNodes.Add(node);
                        }
                    }

                    coords = newListOfNodes;

                    //WriteDebugDisplay(coords);
                }

                WriteDebugDisplay(coords);
            }
        }

        static private void WriteDebugDisplay(List<Tuple<int, int>> coords)
        {
            var maxX = 0;
            var maxY = 0;

            foreach (var coord in coords)
            {
                if (coord.Item1 > maxX)
                {
                    maxX = coord.Item1;
                }

                if (coord.Item2 > maxY)
                {
                    maxY = coord.Item2;
                }
            }

            Console.WriteLine();

            for (var row = 0; row <= maxY; ++row)
            {
                for (var col = 0; col <= maxX; ++col)
                {
                    var node = coords.FirstOrDefault(x => x.Item1 == col && x.Item2 == row);

                    if (node == null)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write('#');
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
