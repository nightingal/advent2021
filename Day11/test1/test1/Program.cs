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
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day11\input.txt"))
            {
                var numbers = new List<int>();

                while (!reader.EndOfStream)
                {
                    numbers.AddRange(reader.ReadLine().Select(x => int.Parse(x.ToString())));
                }

                var grid = new Grid(numbers, 10, 10);

                for (var i = 0; i < 1000; ++i)
                {
                    var previousFlashCount = grid.FlashCount;
                    grid.DoStep();

                    Console.WriteLine($"Turn {i}: delta flash count: " + (grid.FlashCount - previousFlashCount));

                    if (grid.FlashCount - previousFlashCount == 100)
                    {
                        Console.WriteLine("Synched flash on turn " + i);
                    }
                }

                Console.WriteLine(grid.FlashCount); 
            }
        }

        private class Grid
        {
            private List<int> _values;
            private int _rowCount;
            private int _colCount;

            private int _flashCount = 0;

            private static List<Tuple<int, int>> NeighborsDelta;

            public int FlashCount => this._flashCount;

            public Grid(List<int> values, int rowCount, int colCount)
            {
                this._values = values;
                this._rowCount = rowCount;
                this._colCount = colCount;

                NeighborsDelta = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(-1, -1),
                    new Tuple<int, int>(0, -1),
                    new Tuple<int, int>(1, -1),
                    new Tuple<int, int>(-1, 0),
                    new Tuple<int, int>(1, 0),
                    new Tuple<int, int>(-1, 1),
                    new Tuple<int, int>(0, 1),
                    new Tuple<int, int>(1, 1),
                };
            }

            public void DoStep()
            {
                for (var i = 0; i < this._values.Count; ++i)
                {
                    this._values[i] += 1;
                }

                var cellFlashed = false;
                do
                {
                    cellFlashed = false;

                    for (var i = 0; i < this._values.Count; ++i)
                    {
                        if (this._values[i] > 9)
                        {
                            this.FlashCell(i / this._colCount, i % this._colCount);
                            cellFlashed = true;

                            ++this._flashCount;
                        }
                    }
                } while (cellFlashed);
            }

            private void FlashCell(int row, int col)
            {
                this._values[row * this._colCount + col] = 0;

                foreach (var adjacent in NeighborsDelta)
                {
                    var newRow = row + adjacent.Item1;
                    var newCol = col + adjacent.Item2;

                    if (newRow >= 0 && newRow < this._rowCount && newCol >= 0 && newCol < this._colCount)
                    {
                        var index = newRow * this._colCount + newCol;
                        if (this._values[index] != 0)
                        {
                            this._values[index] += 1;
                        }
                    }
                }
            }
        }
    }
}
