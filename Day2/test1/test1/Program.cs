using System;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\Day2\test1\input.txt"))
            {
                var horizontal = 0;
                var depth = 0;

                while (stream.Peek() >= 0)
                {
                    var line = stream.ReadLine();

                    var split = line.Split(' ');

                    var direction = split[0];
                    var amount = int.Parse(split[1]);

                    switch (direction)
                    {
                        case "up":
                            depth -= amount;
                            break;

                        case "down":
                            depth += amount;
                            break;

                        case "forward":
                            horizontal += amount;
                            break;

                        case "backward":
                            horizontal -= amount;
                            break;
                    }
                }

                Console.WriteLine(horizontal * depth);
            }
        }
    }
}



