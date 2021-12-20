using System;
using System.Collections.Generic;
using System.IO;

namespace test2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\test2\input.txt"))
            {
                var windows = new List<int>();
                var currentWindowIndex = 2;

                var val1 = int.Parse(stream.ReadLine());
                var val2 = int.Parse(stream.ReadLine());
                var val3 = int.Parse(stream.ReadLine());

                windows.Add(val1 + val2 + val3);
                windows.Add(val2 + val3);
                windows.Add(val3);

                while (stream.Peek() >= 0)
                {
                    var value = int.Parse(stream.ReadLine());

                    windows.Add(value);
                    windows[currentWindowIndex] += value;
                    windows[currentWindowIndex - 1] += value;

                    ++currentWindowIndex;
                }

                var increaseCount = 0;

                windows.RemoveAt(windows.Count - 1);
                windows.RemoveAt(windows.Count - 1);

                for (var i = 1; i < windows.Count; ++i)
                {
                    var previous = windows[i - 1];
                    var current = windows[i];

                    if (current > previous)
                    {
                        ++increaseCount;
                    }
                }

                Console.WriteLine(increaseCount);
            }
        }
    }
}
