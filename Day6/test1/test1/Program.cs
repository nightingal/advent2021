using System;
using System.IO;
using System.Linq;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day6\input.txt"))
            {
                var dayCount = 256;

                var input = reader.ReadLine();
                var fishes = input.Split(",").Select(x => int.Parse(x)).ToList();

                int newFishInterval = 9;
                var fishResetInterval = 7;

                var toBeBornOnThatDay = new long[dayCount];

                var fishCount = (long)0;
                fishCount += fishes.Count;

                foreach (var fish in fishes)
                {
                    for (var i = fish; i < dayCount; i += fishResetInterval)
                    {
                        toBeBornOnThatDay[i] += 1;
                    }
                }

                for (var day = 0; day < dayCount; ++day)
                {
                    var amountToAdd = toBeBornOnThatDay[day];

                    fishCount += amountToAdd;

                    var firstDay = day + newFishInterval;
                    
                    if (firstDay < dayCount)
                    {
                        for (var newDay = firstDay; newDay < dayCount; newDay += fishResetInterval)
                        {
                            toBeBornOnThatDay[newDay] += amountToAdd;
                        }
                    }
                }

                Console.WriteLine(fishCount);
            }
        }
    }
}
