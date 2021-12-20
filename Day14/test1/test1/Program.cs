using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace test1
{
    class Program
    {
        static void Main2(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day14\input.txt"))
            {
                var template = reader.ReadLine().ToCharArray().ToList();

                reader.ReadLine();

                var rules = new Dictionary<Tuple<char, char>, char>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(" -> ");

                    rules.Add(new Tuple<char, char>(line[0][0], line[0][1]), line[1][0]);
                }

                //DoStepsInMemory(10, template, rules);            
                //DoStepsOnDisc(10, template, rules);
                DoStepsTree(19, template, rules);
            }
        }

        private static void DoStepsOnDisc(int stepCount, List<char> template, Dictionary<Tuple<char, char>, char> rules)
        {
            using (StreamWriter writer = new StreamWriter("step0"))
            {
                writer.Write(new string(template.ToArray()));
            }

            for (var step = 0; step < stepCount; ++step)
            {
                Console.WriteLine("Step: " + step);
                using (StreamReader reader = new StreamReader("step" + step))
                {
                    var char1 = (char)reader.Read();
                    var char2 = (char)reader.Read();

                    using (StreamWriter writer = new StreamWriter("step" + (step + 1)))
                    {
                        do
                        {
                            var toInsert = rules[new Tuple<char, char>(char1, char2)];
                            writer.Write(char1);
                            writer.Write(toInsert);

                            char1 = char2;

                            var char2int = reader.Read();

                            if (char2int == -1)
                            {
                                break;
                            }

                            char2 = (char)char2int;
                        } while (true);

                        writer.Write(char1);
                    }
                }

                WriteStepOuputInfoFromDisc("step" + (step + 1));
            }
        }

        private static void DoStepsInMemory(int stepCount, List<Char> template, Dictionary<Tuple<char, char>, char> rules)
        {
            for (var step = 0; step < stepCount; ++step)
            {
                Console.WriteLine("Step: " + step);
                for (var i = template.Count - 2; i >= 0; --i)
                {
                    var char1 = template[i];
                    var char2 = template[i + 1];

                    var toInsert = rules[new Tuple<char, char>(char1, char2)];

                    template.Insert(i + 1, toInsert);
                }

                WriteStepOuputInfo(template);
            }
        }

        private static void WriteStepOuputInfoFromDisc(string filePath)
        {
            var countPerElement = new Dictionary<char, long>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var c = (char)reader.Read();
                    if (!countPerElement.ContainsKey(c))
                    {
                        countPerElement.Add(c, 1);
                    }
                    else
                    {
                        countPerElement[c]++;
                    }
                }
            }

            var min = long.MaxValue;
            var max = long.MinValue;

            var maxChar = ' ';
            var minChar = ' ';

            foreach (var pair in countPerElement)
            {
                if (pair.Value < min)
                {
                    min = pair.Value;
                    minChar = pair.Key;
                }

                if (pair.Value > max)
                {
                    max = pair.Value;
                    maxChar = pair.Key;
                }
            }

            var diff = max - min;

            Console.WriteLine($"Most frequent is {maxChar} with {max} showing");
            Console.WriteLine($"Least frequent is {minChar} with {min} showing");
            Console.WriteLine("Diff min max: " + diff);
        }

        private static void WriteStepOuputInfo(List<char> template)
        {
            var countPerElement = new Dictionary<char, int>();

            foreach (var c in template)
            {
                if (!countPerElement.ContainsKey(c))
                {
                    countPerElement.Add(c, 1);
                }
                else
                {
                    countPerElement[c]++;
                }
            }

            var min = int.MaxValue;
            var max = int.MinValue;

            var maxChar = ' ';
            var minChar = ' ';

            foreach (var pair in countPerElement)
            {
                if (pair.Value < min)
                {
                    min = pair.Value;
                    minChar = pair.Key;
                }

                if (pair.Value > max)
                {
                    max = pair.Value;
                    maxChar = pair.Key;
                }

                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            var diff = max - min;

            Console.WriteLine($"Most frequent is {maxChar} with {max} showing");
            Console.WriteLine($"Least frequent is {minChar} with {min} showing");
            Console.WriteLine("Diff min max: " + diff);
        }

        private static void DisplayMinMaxRecursive(Dictionary<char, long> count)
        {
            var min = long.MaxValue;
            var max = long.MinValue;

            var maxChar = ' ';
            var minChar = ' ';

            foreach (var pair in count)
            {
                if (pair.Value < min)
                {
                    min = pair.Value;
                    minChar = pair.Key;
                }

                if (pair.Value > max)
                {
                    max = pair.Value;
                    maxChar = pair.Key;
                }

                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            var diff = max - min;

            Console.WriteLine($"Most frequent is {maxChar} with {max} showing");
            Console.WriteLine($"Least frequent is {minChar} with {min} showing");
            Console.WriteLine("Diff min max: " + diff);

            using (StreamWriter writer = new StreamWriter("RESULTS"))
            {
                writer.WriteLine($"Most frequent is {maxChar} with {max} showing");
                writer.WriteLine($"Least frequent is {minChar} with {min} showing");
                writer.WriteLine("Diff min max: " + diff);
            }
        }

        private struct TreeStartInfo
        {
            public char char1;
            public char char2;

            public int stepCount;

            public Dictionary<Tuple<char, char>, char> rules;
        }

        private static ConcurrentStack<TreeSolver> Solvers = new ConcurrentStack<TreeSolver>();

        private static ConcurrentStack<TreeSolver> DoneSolvers = new ConcurrentStack<TreeSolver>();

        private static int TemplateCount;

        private static void DoStepsTree(int stepCount, List<char> template, Dictionary<Tuple<char, char>, char> rules)
        {
            TemplateCount = template.Count;

            Dictionary<char, long> count = new Dictionary<char, long>();

            var startedThreadCount = 0;

            for (var i = 0; i < template.Count - 1; ++i)
            {
                var char1 = template[i];
                var char2 = template[i + 1];

                ++startedThreadCount;

                Thread thread = new Thread(TreeThreadStart);
                thread.Start(new TreeStartInfo()
                {
                    char1 = char1,
                    char2 = char2,
                    stepCount = stepCount,
                    rules = rules,
                }) ;

                while (startedThreadCount - DoneSolvers.Count > 20)
                {
                    //Thread.Sleep(1000);
                }

                if (i % 100 == 0)
                {
                    Console.WriteLine($"Started {i} thread");
                }

                //HandleNodeRecursive(stepCount, char1, char2, count, rules, true);
            }

            while (DoneSolvers.Count != startedThreadCount)
            {
                Thread.Sleep(30000);
            }

            while (!Solvers.IsEmpty)
            {
                if (Solvers.TryPop(out var result))
                {
                    foreach (var pair in result.count)
                    {
                        if (!count.ContainsKey(pair.Key))
                        {
                            count[pair.Key] = pair.Value;
                        }
                        else
                        {
                            count[pair.Key] += pair.Value;
                        }
                    }
                }
            }

            DisplayMinMaxRecursive(count);
        }

        private static void TreeThreadStart(object obj)
        {
            var info = (TreeStartInfo)obj;
            var tree = new TreeSolver();

            Solvers.Push(tree);

            tree.HandleNodeRecursive(info.stepCount, info.char1, info.char2, info.rules, true);

            DoneSolvers.Push(tree);

            Console.WriteLine("Thread done: " + DoneSolvers.Count + "/" + TemplateCount + "(" + (100.0f * DoneSolvers.Count / TemplateCount +")"));
        }

        private static void HandleNodeRecursive(int stepLeft, char char1, char char2, Dictionary<char, long> count, Dictionary<Tuple<char, char>, char> rules, bool isLeftNode)
        {
            var toInsert = rules[new Tuple<char, char>(char1, char2)];

            if (stepLeft == 0)
            {

                if (!count.ContainsKey(char1))
                {
                    count.Add(char1, 1);
                }
                else
                {
                    count[char1]++;
                }

                if (!count.ContainsKey(toInsert))
                {
                    count.Add(toInsert, 1);
                }
                else
                {
                    count[toInsert]++;
                }

                if (!isLeftNode)
                {
                    if (!count.ContainsKey(char2))
                    {
                        count.Add(char2, 1);
                    }
                    else
                    {
                        count[char2]++;
                    }
                }
            }
            else
            {
                HandleNodeRecursive(stepLeft - 1, char1, toInsert, count, rules, true);
                HandleNodeRecursive(stepLeft - 1, toInsert, char2, count, rules, false);
                
                if (stepLeft > 1)
                {
                    count[toInsert]--;
                }
            }
        }
    }
}
