using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var bufferLength = 400;
            var iterationCount = 50;

            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day20\input.txt"))
            {
                var algorithm = reader.ReadLine();

                reader.ReadLine();

                var image = new Image();

                var firstLineDone = false;

                var buffer = "";

                var prefix = "";
                var suffix = "";

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!firstLineDone)
                    {
                        firstLineDone = true;
                        for (var i = 0; i < line.Length + bufferLength; ++i)
                        {
                            buffer += '.';
                        }

                        for (var i = 0; i < bufferLength / 2; ++i)
                        {
                            image.AddRow(buffer);

                            prefix += ".";
                            suffix += ".";
                        }
                    }

                    image.AddRow(prefix + line + suffix);
                }

                for (var i = 0; i < bufferLength / 2; ++i)
                {
                    image.AddRow(buffer);
                }

                //image.Display();

                for (var i = 0; i < iterationCount; ++i)
                {
                    Console.WriteLine("Starting iteration: " + i);
                    image = ProcessImage(image, algorithm);

                    image.Display();
                }

                image.WriteToFile("Result.txt");
               
                var count = image.CountChar('#', bufferLength);
                Console.WriteLine("Count: " + count);
            }
        }

        static Image ProcessImage(Image image, string algorithm)
        {
            var newImage = new Image();

            for (var rowIndex = 0; rowIndex < image.RowCount; ++rowIndex)
            {
                var newRow = "";

                for (var colIndex = 0; colIndex < image.ColCount; ++colIndex)
                {
                    var chars = image.GetCharsAround(rowIndex, colIndex).ToCharArray().Select(x => x == '.' ? '0' : '1').ToArray();
                    var str = new string(chars);
                    var index = Convert.ToInt32(str, 2);

                    newRow += algorithm[index];

                    //Console.WriteLine($"{rowIndex},{colIndex}: {index}");
                }

                newImage.AddRow(newRow);
            }

            return newImage;
        }

        private class Image
        {
            private List<string> _rows;

            private int _colCount;

            public int RowCount => this._rows.Count;
            public int ColCount => this._colCount;

            public Image()
            {
                this._rows = new List<string>();
            }

            public void AddRow(string row)
            {
                this._rows.Add(row);
                this._colCount = row.Length;
            }

            public char GetValue(int row, int col)
            {
                if (row < 0 || row >= this._rows.Count || col < 0 || col >= this._colCount)
                {
                    return '.';
                }

                return this._rows[row][col];
            }
            public string GetCharsAround(int centerRow, int centerCol)
            {
                var result = "";

                for (var row = centerRow - 1; row <= centerRow + 1; ++row)
                {
                    for (var col = centerCol - 1; col <= centerCol + 1; ++col)
                    {
                        result += GetValue(row, col);
                    }
                }

                return result;
            }

            public int CountChar(char c, int bufferSize)
            {
                var count = 0;
                var halfBuffer = bufferSize / 2;

                for (var row = halfBuffer; row < this.RowCount - halfBuffer; ++row)
                {
                    for (var col = halfBuffer; col < this.ColCount - halfBuffer; ++col)
                    {
                        var strC = this._rows[row][col];
                        if (strC == c)
                        {
                            ++count;
                        }
                    }
                }

                return count;
            }

            public void Display()
            {
                Console.WriteLine("--------------------");
                foreach (var row in this._rows)
                {
                    Console.WriteLine(row);
                }
                Console.WriteLine("--------------------");
            }

            public void WriteToFile(string path)
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (var row in this._rows)
                    {
                        writer.WriteLine(row);
                    }
                }
            }
        }
    }
}
