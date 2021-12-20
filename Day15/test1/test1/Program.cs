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
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day15\input.txt"))
            {
                var line = reader.ReadLine();

                var grid = new Grid(line.Length);

                grid.AddRow(line.Select(x => int.Parse(x.ToString())).ToList());

                while (!reader.EndOfStream)
                {
                    grid.AddRow(reader.ReadLine().Select(x => int.Parse(x.ToString())).ToList());
                }

                // Part 2 grid building
                {
                    var templateGrid = grid;

                    var newGrid = new Grid(line.Length * 5, grid.RowCount * 5);

                    for (var rowOffset = 0; rowOffset < 5; ++rowOffset)
                    {
                        for (var colOffset = 0; colOffset < 5; ++colOffset)
                        {
                            newGrid.AddSubGrid(grid, rowOffset * grid.RowCount, colOffset * grid.ColCount, rowOffset + colOffset);
                        }
                    }

                    grid = newGrid;
                }


                var nodesToPath = new Dictionary<Node, Path>();

                var toCheck = new Queue<Node>();

                var startNode = grid.GetNode(0, 0);
                toCheck.Enqueue(startNode);
                var path = new Path(new List<Node>() { startNode });

                nodesToPath.Add(startNode, path);

                while (toCheck.Count > 0)
                {
                    var node = toCheck.Dequeue();

                    var currentNodePath = nodesToPath[node];

                    var adjacents = grid.GetAdjacentNodes(node.Row, node.Col);

                    foreach (var adjacent in adjacents)
                    {
                        Path adjacentPath;

                        if (nodesToPath.TryGetValue(adjacent, out adjacentPath))
                        {
                            if (currentNodePath.Score + adjacent.Score < adjacentPath.Score)
                            {
                                var newPath = new Path(currentNodePath.Nodes);
                                newPath.AddNode(adjacent);

                                nodesToPath[adjacent] = newPath;

                                toCheck.Enqueue(adjacent);
                            }
                        }
                        else
                        {
                            adjacentPath = new Path(currentNodePath.Nodes);
                            adjacentPath.AddNode(adjacent);

                            nodesToPath[adjacent] = adjacentPath;

                            toCheck.Enqueue(adjacent);
                        }
                    }
                }

                var lastNode = grid.GetNode(grid.RowCount - 1, grid.ColCount - 1);

                var lastPath = nodesToPath[lastNode];

                Console.WriteLine(lastPath.Score - startNode.Score);
            }
        }

        private class Path
        {
            public List<Node> Nodes;

            public int Score { get; private set; }

            public Path(List<Node> nodes)
            {
                this.Nodes = new List<Node>();
                this.Nodes.AddRange(nodes);

                this.UpdateScore();
            }

            public void AddNode(Node node)
            {
                this.Nodes.Add(node);

                this.UpdateScore();
            }

            private void UpdateScore()
            {
                var score = 0;

                foreach (var node in this.Nodes)
                {
                    score += node.Score;
                }

                this.Score = score;
            }
        }

        private class Node
        {
            public int Row { get; }

            public int Col { get; }

            public int Score { get; }

            public Node(int row, int col, int score)
            {
                this.Row = row;
                this.Col = col;
                this.Score = score;
            }
        }

        private class Grid
        {
            private int _colCount;
            private int _rowCount;

            private List<Node> _cells;

            public int ColCount => this._colCount;

            public int RowCount => this._rowCount;

            public Grid(int colCount)
            {
                this._colCount = colCount;

                this._cells = new List<Node>();
            }

            public Grid(int colCount, int rowCount)
            {
                this._colCount = colCount;
                this._rowCount = rowCount;

                this._cells = new List<Node>();

                var length = colCount * rowCount;

                for (var i = 0; i < length; ++i)
                {
                    this._cells.Add(null);
                }
            }

            public void AddSubGrid(Grid subGrid, int startRow, int startCol, int numberOffset)
            {
                foreach (var node in subGrid._cells)
                {
                    var row = startRow + node.Row;
                    var col = startCol + node.Col;
                    var value = node.Score + numberOffset;

                    while (value > 9)
                    {
                        value -= 9;
                    }

                    this._cells[row * this.ColCount + col] = new Node(row, col, value);
                }
            }

            public void AddRow(List<int> row)
            {
                var rowNumber = this._rowCount;
                for (var i = 0; i < row.Count; ++i)
                {
                    this._cells.Add(new Node(rowNumber, i, row[i]));
                }

                ++this._rowCount;
            }

            public Node GetNode(int row, int col)
            {
                var value = this._cells[row * this._colCount + col];
                return value;
            }

            public List<Node> GetAdjacentNodes(int row, int col)
            {
                var results = new List<Node>();

                if (row > 0)
                {
                    results.Add(this.GetNode(row - 1, col));
                }

                if (row < this._rowCount-1)
                {
                    results.Add(this.GetNode(row + 1, col));
                }

                if (col > 0)
                {
                    results.Add(this.GetNode(row, col - 1));
                }

                if (col < this._colCount-1)
                {
                    results.Add(this.GetNode(row, col + 1));
                }

                return results;
            }
        }
    }
}
