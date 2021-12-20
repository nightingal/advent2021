using System;
using System.Collections.Generic;
using System.IO;

namespace test2
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

                var numbers = new List<string>();

                while (stream.Peek() >= 0)
                {
                    var line = stream.ReadLine();
                    numbers.Add(line);

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

                var oxygen = FindOxygenRating(numbers);
                var co2 = FindCO2ScrubberRating(numbers);

                var result = oxygen * co2;

                Console.WriteLine(result);
            }
        }

        private static int FindCO2ScrubberRating(List<string> numbers)
        {
            var numberLeft = new List<string>(numbers);

            for (var i = 0; i < 12; ++i)
            {
                char lookingForChar;
                var newList = new List<string>();

                var zerosCount = 0;
                var onesCount = 0;

                foreach (var num in numberLeft)
                {
                    if (num[i] == '0')
                    {
                        ++zerosCount;
                    }
                    else
                    {
                        ++onesCount;
                    }
                }

                if (zerosCount <= onesCount)
                {
                    lookingForChar = '0';
                }
                else
                {
                    lookingForChar = '1';
                }

                foreach (var num in numberLeft)
                {
                    if (num[i] == lookingForChar)
                    {
                        newList.Add(num);
                    }
                }

                numberLeft = newList;

                if (numberLeft.Count == 1)
                {
                    break;
                }
            }

            return Convert.ToInt32(numberLeft[0], 2);
        }

        private static int FindOxygenRating(List<string> numbers)
        {
            var numberLeft = new List<string>(numbers);

            for (var i = 0; i < 12; ++i)
            {
                char lookingForChar;
                var newList = new List<string>();

                var zerosCount = 0;
                var onesCount = 0;

                foreach (var num in numberLeft)
                {
                    if (num[i] == '0')
                    {
                        ++zerosCount;
                    }
                    else
                    {
                        ++onesCount;
                    }
                }

                if (onesCount >= zerosCount)
                {
                    lookingForChar = '1';
                }
                else
                {
                    lookingForChar = '0';
                }

                foreach (var num in numberLeft)
                {
                    if (num[i] == lookingForChar)
                    {
                        newList.Add(num);
                    }
                }

                numberLeft = newList;

                if (numberLeft.Count == 1)
                {
                    break;
                }
            }

            return Convert.ToInt32(numberLeft[0], 2);
        }
    }
}
