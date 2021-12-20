using System;
using System.Collections.Generic;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day5\test1\input.txt"))
            {
                var grid = new Grid(1000);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var splitted = line.Split(" -> ");

                    var coords1 = splitted[0].Split(',');
                    var coords2 = splitted[1].Split(',');

                    var x1 = int.Parse(coords1[0]);
                    var y1 = int.Parse(coords1[1]);

                    var x2 = int.Parse(coords2[0]);
                    var y2 = int.Parse(coords2[1]);

                    grid.AddLine(x1, y1, x2, y2);
                }

                var overlapCount = grid.CountOverlaps();

                Console.WriteLine(overlapCount);
            }
        }

        class Grid
        {
            private int _size;

            private List<List<int>> _grid;

            public Grid(int size)
            {
                this._size = size;

                this._grid = new List<List<int>>();

                for (var i = 0; i < size; ++i)
                {
                    var row = new List<int>(size);

                    for (var j = 0; j < size; ++j)
                    {
                        row.Add(0);
                    }

                    this._grid.Add(row);
                }
            }

            internal void AddLine(int x1, int y1, int x2, int y2)
            {
                if (x1 >= this._size || y1 >= this._size || x2 >= this._size || y2 >= this._size)
                {
                    throw new Exception($"Grid is too small for numbers {x1} {y1} {x2} {y2}");
                }

                if (x1 == x2)
                {
                    if (y1 > y2)
                    {
                        var temp = y1;
                        y1 = y2;
                        y2 = temp;
                    }

                    for (var i = y1; i <= y2; ++i)
                    {
                        this._grid[x1][i]++;
                    }
                }
                else if (y1 == y2)
                {
                    if (x1 > x2)
                    {
                        var temp = x1;
                        x1 = x2;
                        x2 = temp;
                    }

                    for (var i = x1; i <= x2; ++i)
                    {
                        this._grid[i][y1]++;
                    }
                }
                else
                {
                    // It is a vertical line

                    // Let's make it so that lines are always left-right
                    if (x1 > x2)
                    {
                        var temp = x1;
                        x1 = x2;
                        x2 = temp;

                        temp = y1;
                        y1 = y2;
                        y2 = temp;
                    }

                    if (y2 > y1)
                    {
                        for (var x = x1; x <= x2; ++x)
                        {
                            this._grid[x][y1 + (x - x1)]++;
                        }
                    }
                    else
                    {
                        for (var x = x1; x <= x2; ++x)
                        {
                            this._grid[x][y1 - (x - x1)]++;
                        }
                    }
                }
            }

            internal object CountOverlaps()
            {
                var count = 0;

                foreach (var row in this._grid)
                {
                    foreach (var cell in row)
                    {
                        if (cell >= 2)
                        {
                            ++count;
                        }
                    }
                }

                return count;
            }
        }
    }
}
