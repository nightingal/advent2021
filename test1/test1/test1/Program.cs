using System;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\test1\input.txt"))
            {
                var previousValue = int.Parse(stream.ReadLine());
                int increaseCount = 0;

                while (stream.Peek() >= 0)
                {
                    var value = int.Parse(stream.ReadLine());

                    if (value > previousValue)
                    {
                        ++increaseCount;
                    }

                    previousValue = value;
                }

                var gamma = "";
                var epsilon = "";

                Console.WriteLine(increaseCount);
            }
        }
    }
}
