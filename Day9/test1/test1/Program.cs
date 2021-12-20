using System;
using System.Collections.Generic;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader  = new StreamReader(@"C:\dev\advantofcode\Day9\input.txt"))
            {
                var line = reader.ReadLine();

                var columnCount = line.Length;

                var lines = new List<string>();
                lines.Add(line);

                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }

                var rowCount = lines.Count;

                var lowPointsValues = new List<int>();
                var lowPointCoords = new List<Tuple<int, int>>();

                for (var row = 0; row < rowCount; ++row)
                {
                    for (var col = 0; col < columnCount; ++col)
                    {
                        var valueInt = int.Parse(lines[row][col].ToString());

                        if (row != 0)
                        {
                            var top = int.Parse(lines[row - 1][col].ToString());

                            if (top <= valueInt) continue;
                        }

                        if (row != rowCount - 1)
                        {
                            var under = int.Parse(lines[row + 1][col].ToString());

                            if (under <= valueInt) continue;
                        }

                        if (col != 0)
                        {
                            var left = int.Parse(lines[row][col-1].ToString());

                            if (left <= valueInt) continue;
                        }

                        if (col != columnCount - 1)
                        {
                            var right = int.Parse(lines[row][col + 1].ToString());

                            if (right <= valueInt) continue;
                        }

                        lowPointsValues.Add(valueInt);
                        lowPointCoords.Add(new Tuple<int, int>(row, col));
                    }
                }

                var sum = 0;
                foreach (var val in lowPointsValues)
                {
                    sum = sum + val + 1;
                }

                Console.WriteLine(sum);

                var sizes = new List<int>();

                foreach (var coords in lowPointCoords)
                {
                    var bassinSize = FindBassinSize(coords.Item1, coords.Item2, lines, rowCount, columnCount);
                    sizes.Add(bassinSize);
                }

                sizes.Sort();

                var count = sizes.Count;
                var mul = sizes[count - 1] * sizes[count - 2] * sizes[count - 3];

                Console.WriteLine(mul);
            }
        }

        private static int FindBassinSize(int startRow, int startCol, List<string> lines, int rowCount, int colCount)
        {
            var size = 0;

            var toCheck = new List<Tuple<int, int>>();
            toCheck.Add(new Tuple<int, int>(startRow, startCol));

            var alreadyChecked = new HashSet<Tuple<int, int>>();

            while (toCheck.Count > 0)
            {
                var coords = toCheck[0];
                toCheck.RemoveAt(0);

                if (alreadyChecked.Contains(coords))
                {
                    continue;
                }

                var row = coords.Item1;
                var col = coords.Item2;

                var currentValue = GetInt(coords.Item1, coords.Item2, lines);

                size += 1;

                alreadyChecked.Add(coords);

                if (row != 0)
                {
                    var topCoords = new Tuple<int, int>(coords.Item1 - 1, coords.Item2);
                    var top = GetInt(topCoords.Item1, topCoords.Item2, lines);

                    if (top != 9 && top >= currentValue && !alreadyChecked.Contains(topCoords))
                    {
                        toCheck.Add(topCoords);
                    }
                }

                if (row != rowCount - 1)
                {
                    var underCoords = new Tuple<int, int>(coords.Item1 + 1, coords.Item2);
                    var under = GetInt(underCoords.Item1, underCoords.Item2, lines);

                    if (under != 9 && under >= currentValue && !alreadyChecked.Contains(underCoords))
                    {
                        toCheck.Add(underCoords);
                    }
                }

                if (col != 0)
                {
                    var leftCoords = new Tuple<int, int>(coords.Item1, coords.Item2 - 1);
                    var left = GetInt(leftCoords.Item1, leftCoords.Item2, lines);

                    if (left != 9 && left >= currentValue && !alreadyChecked.Contains(leftCoords))
                    {
                        toCheck.Add(leftCoords);
                    }
                }

                if (col != colCount - 1)
                {
                    var rightCoords = new Tuple<int, int>(coords.Item1, coords.Item2 + 1);
                    var right = GetInt(rightCoords.Item1, rightCoords.Item2, lines);

                    if (right != 9 && right >= currentValue && !alreadyChecked.Contains(rightCoords))
                    {
                        toCheck.Add(rightCoords);
                    }
                }
            }

            return size;
        }

        private static int GetInt(int row, int col, List<string> lines)
        {
            return int.Parse(lines[row][col].ToString());
        }
    }
}
