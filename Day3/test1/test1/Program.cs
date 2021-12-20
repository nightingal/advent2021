using System;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\Day3\test1\input.txt"))
            {
                int bitCount = 12;

                var zerosCount = new int[bitCount];
                var onesCount = new int[bitCount];

                while (stream.Peek() >= 0)
                {
                    var line = stream.ReadLine();

                    for (var i = 0; i < bitCount; ++i)
                    {
                        if (line[i] == '0')
                        {
                            ++zerosCount[i];
                        }
                        else
                        {
                            ++onesCount[i];
                        }
                    }
                }

                var gamma = "";
                var epsilon = "";

                for (var i = 0; i < bitCount; ++i)
                {
                    if (zerosCount[i] > onesCount[i])
                    {
                        gamma += "0";
                        epsilon += "1";
                    }
                    else if (zerosCount[i] < onesCount[i])
                    {
                        gamma += "1";
                        epsilon += "0";
                    }
                    else
                    {
                        Console.WriteLine("oh no");
                    }
                }

                var gammaInt = Convert.ToInt32(gamma, 2);
                var epsilonInt = Convert.ToInt32(epsilon, 2);

                var result = gammaInt * epsilonInt;

                Console.WriteLine(result);
            }
        }
    }
}
