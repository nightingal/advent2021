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
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day7\input.txt"))
            {
                var initialPosition = reader.ReadLine().Split(',').Select(x => int.Parse(x)).ToList();

                var minPosition = int.MaxValue;
                var maxPosition = int.MinValue;

                foreach (var position in initialPosition)
                {
                    if (position < minPosition)
                    {
                        minPosition = position;
                    }

                    if (position > maxPosition)
                    {
                        maxPosition = position;
                    }
                }

                var fuelPerPosition = new Dictionary<int, int>();

                for (var i = minPosition; i <= maxPosition; ++i)
                {
                    var totalFuel = 0;

                    foreach(var pos in initialPosition)
                    {
                        var delta = Math.Abs(pos - i);
                        var cost = 0;
                        for (var a = 0; a <= delta; ++a)
                        {
                            cost += a;
                        }

                        totalFuel += cost;
                    }

                    fuelPerPosition.Add(i, totalFuel);
                }

                var minConsumption = int.MaxValue;
                int positionWithMinConsumption = 0;

                foreach (var entry in fuelPerPosition)
                {
                    if (entry.Value < minConsumption)
                    {
                        minConsumption = entry.Value;
                        positionWithMinConsumption = entry.Key;
                    }
                }

                Console.WriteLine(minConsumption);
            }
        }
    }
}
