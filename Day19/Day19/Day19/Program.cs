using System;
using System.Collections.Generic;
using System.IO;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Scanner> scanners = new List<Scanner>();

            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day19\input.txt"))
            {
                var nextIsName = true;
                Scanner currentScanner = null;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (nextIsName)
                    {
                        nextIsName = false;
                        currentScanner = new Scanner(line);
                        scanners.Add(currentScanner);
                    }
                    else if (string.IsNullOrEmpty(line))
                    {
                        nextIsName = true;
                    }
                    else
                    {
                        var coords = line.Split(',');
                        var beacon = new Beacon(Guid.NewGuid(), int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
                        currentScanner.Add(beacon);
                    }
                }
            }

            foreach (var scanner in scanners)
            {
                scanner.CalculateDistanceBetweenAllPairs();
            }

            var scanner1Distances = new List<long>(scanners[0].DistanceToPairs.Keys);
            

            scanner1Distances.Sort();

            var matchingDistances = new List<long>();

            for (var i = 1; i < scanners.Count; ++i)
            {
                var scanner2Distances = new List<long>(scanners[i].DistanceToPairs.Keys);
                foreach (var dist in scanner1Distances)
                {
                    if (scanner2Distances.Contains(dist))
                    {
                        matchingDistances.Add(dist);
                        continue;
                    }
                }
            }


        }

        private class Scanner
        {
            public List<Beacon> Beacons;

            public Dictionary<(Beacon, Beacon), long> _distanceBetweenBeacons;
            public Dictionary<long, (Beacon, Beacon)> _distanceToPairs;

            public Dictionary<long, (Beacon, Beacon)> DistanceToPairs => this._distanceToPairs;

            public string Name { get; }

            public Scanner(string name)
            {
                this.Name = name;

                this.Beacons = new List<Beacon>();
            }

            public void Add(Beacon beacon)
            {
                this.Beacons.Add(beacon);
            }

            public void CalculateDistanceBetweenAllPairs()
            {
                this._distanceBetweenBeacons = new Dictionary<(Beacon, Beacon), long>();
                this._distanceToPairs = new Dictionary<long, (Beacon, Beacon)>();

                for (var i = 0; i < this.Beacons.Count; ++i)
                {
                    for (var j = i; j < this.Beacons.Count; ++j)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        var first = this.Beacons[i];
                        var second = this.Beacons[j];

                        var squareDistance = GetSquareDistance(first, second);

                        this._distanceBetweenBeacons.Add((first, second), squareDistance);
                        this._distanceToPairs.Add(squareDistance, (first, second));
                    }
                }
            }

            private long GetSquareDistance(Beacon first, Beacon second)
            {
                return (long)Math.Pow(second.X - first.X, 2) + (long)Math.Pow(second.Y - first.Y, 2) + (long)Math.Pow(second.Z - first.Z, 2);
            }
        }

        public class Beacon
        {
            public Guid Id { get; }

            public int X { get; }

            public int Y { get; }

            public int Z { get; }

            public Beacon(Guid id, int x, int y, int z)
            {
                this.Id = id;
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
        }
    }
}
