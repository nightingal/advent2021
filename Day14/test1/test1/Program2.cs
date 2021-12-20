using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace test1
{
    class Program2
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day14\input.txt"))
            {
                var template = reader.ReadLine().ToCharArray().ToList();

                reader.ReadLine();

                var rules = new Dictionary<(char, char), char>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(" -> ");

                    rules.Add((line[0][0], line[0][1]), line[1][0]);
                }

                DoSteps(40, template, rules);
            }
        }

        private static void DoSteps(int stepCount, List<char> template, Dictionary<(char, char), char> rules)
        {
            var pairs = new Dictionary<(char, char), long>();

            foreach (var rule in rules)
            {
                pairs.Add(rule.Key, 0);
            }

            for (var i = 0; i < template.Count - 1; ++i)
            {
                pairs[(template[i], template[i + 1])]++;
            }

            for (var i = 0; i < stepCount; ++i)
            {
                var stepDictionary = new Dictionary<(char, char), long>();
                foreach (var rule in rules)
                {
                    stepDictionary.Add(rule.Key, 0);
                }

                foreach (var pair in pairs)
                {
                    var addedChar = rules[pair.Key];

                    var addedPair1 = (pair.Key.Item1, addedChar);
                    var addedPair2 = (addedChar, pair.Key.Item2);

                    stepDictionary[addedPair1] += pair.Value;
                    stepDictionary[addedPair2] += pair.Value;
                }

                pairs = stepDictionary;
            }

            var charCount = new Dictionary<char, long>();

            foreach (var pair in pairs)
            {
                var c = pair.Key.Item1;

                if (!charCount.ContainsKey(c))
                {
                    charCount[c] = (long)pair.Value;
                }
                else
                {
                    charCount[c] += pair.Value;
                }
            }

            charCount[template[template.Count - 1]]++;

            DisplayMinMaxRecursive(charCount);
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
    }
}
