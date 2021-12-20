using System;
using System.IO;

namespace test2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\Day2\test1\input.txt"))
            {
                var horizontal = 0;
                var depth = 0;
                var aim = 0;

                while (stream.Peek() >= 0)
                {
                    var line = stream.ReadLine();

                    var split = line.Split(' ');

                    var direction = split[0];
                    var amount = int.Parse(split[1]);

                    switch (direction)
                    {
                        case "up":
                            aim -= amount;
                            break;

                        case "down":
                            aim += amount;
                            break;

                        case "forward":
                            horizontal += amount;
                            depth += aim * amount;
                            break;
                    }
                }

                var result = horizontal * depth;
                Console.WriteLine(result);
            }
        }
    }
}
