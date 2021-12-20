using System;
using System.Collections.Generic;
using System.IO;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day17\input.txt"))
            {
                var split = reader.ReadLine().Split(" ");

                var split2 = split[2].Split("=")[1].Split(',')[0].Split("..");
                var minX = Int32.Parse(split2[0]);
                var maxX = Int32.Parse(split2[1]);


                split2 = split[3].Split('=')[1].Split("..");
                var minY = Int32.Parse(split2[0]);
                var maxY = Int32.Parse(split2[1]);

                var launchData = new LaunchData()
                {
                    MinX = minX,
                    MaxX = maxX,
                    MinY = minY,
                    MaxY = maxY,
                };

                LaunchResult? maxReachedYLaunchResult = null;

                List<LaunchResult> workingResults = new List<LaunchResult>();

                for (var i = 0; i < 1000; i++)
                {
                    for (var j = -1000; j < 1000; j++)
                    {
                        var result = ValidateHitTarget(i, j, launchData);
                        if (result.HitTarget)
                        {
                            if (maxReachedYLaunchResult == null || result.MaxY > maxReachedYLaunchResult.Value.MaxY)
                            {
                                maxReachedYLaunchResult = result;
                            }

                            workingResults.Add(result);

                            Console.WriteLine($"Working combination {i},{j}");
                        }
                    }
                }

                Console.WriteLine(maxReachedYLaunchResult.Value.MaxY);

                Console.WriteLine("Working results: " + workingResults.Count);
            }
        }

        private static LaunchResult ValidateHitTarget(int initialXVelocity, int initialYelocity, LaunchData data)
        {
            var step = 0;

            var posX = 0;
            var posY = 0;

            var xVelocity = initialXVelocity;
            var yVelocity = initialYelocity;

            var launchResult = new LaunchResult();

            do
            {
                if (xVelocity > 0)
                {
                    posX += xVelocity;
                    xVelocity -= 1;
                }

                posY += yVelocity;
                yVelocity -= 1;

                if (posY > launchResult.MaxY)
                {
                    launchResult.MaxY = posY;
                }

                if (posX >= data.MinX && posX <= data.MaxX && posY >= data.MinY && posY <= data.MaxY)
                {
                    launchResult.HitTarget = true;
                    break;
                }
                else if (posY < data.MinY)
                {
                    launchResult.HitTarget = false;
                    break;
                }
                else if (posX > data.MaxX)
                {
                    launchResult.HitTarget = false;
                    break;
                }

                ++step;

            } while (true);

            return launchResult;
        }

        private struct LaunchResult
        {
            public int MaxY;

            public bool HitTarget;
        }

        private struct LaunchData
        {
            public int MinX;

            public int MaxX;

            public int MinY;

            public int MaxY;
        }
    }
}
