using System;
using System.Collections.Generic;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day10\input.txt"))
            {
                var corruptedCharacterList = new List<char>();

                var inChars = new List<char>() { '{', '[', '<', '(' };
                var outChars = new List<char>() { '}', ']', '>', ')' };

                var outScores = new List<int>() { 1197, 57, 25137, 3 };
                var outScores2 = new List<int>() { 3, 2, 4, 1 };

                var linesToComplete = new List<Stack<char>>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var stack = new Stack<char>();

                    var isCorrupted = false;

                    for (var i = 0; i < line.Length; ++i)
                    {
                        var c = line[i];

                        if (inChars.Contains(c))
                        {
                            stack.Push(c);
                        }
                        else
                        {
                            var lastIn = stack.Peek();
                            var lastInIndex = inChars.IndexOf(lastIn);

                            var equivalentOutChars = outChars[lastInIndex];

                            if (c != equivalentOutChars)
                            {
                                corruptedCharacterList.Add(c);
                                isCorrupted = true;
                                break;
                            }
                            else
                            {
                                stack.Pop();
                            }
                        }
                    }

                    if (!isCorrupted && stack.Count > 0)
                    {
                        linesToComplete.Add(stack);
                    }
                }

                var totalScore = 0;

                foreach (var corrupted in corruptedCharacterList)
                {
                    var index = outChars.IndexOf(corrupted);
                    var score = outScores[index];

                    totalScore += score;
                }

                Console.WriteLine(totalScore);

                var lineScores = new List<long>();

                foreach (var stack in linesToComplete)
                {
                    var lineScore = (long)0;

                    while (stack.Count > 0)
                    {
                        var c = stack.Pop();

                        var inIndex = inChars.IndexOf(c);
                        var outIndex = inChars.IndexOf(c);

                        if (inIndex != -1)
                        {
                            var equivalentOut = outChars[inIndex];

                            var score = outScores2[inIndex];

                            lineScore = lineScore * 5 + score;
                        }
                        else
                        {
                            var equivalentIn = inChars[outIndex];

                            // Need to find that in char and remove it to ensure we don't close it again
                            var charsToAddBack = new Stack<char>();

                            while (true)
                            {
                                var charFromStack = stack.Pop();

                                if (charFromStack == equivalentIn)
                                {
                                    while (charsToAddBack.Count > 0)
                                    {
                                        stack.Push(charsToAddBack.Pop());
                                    }

                                    break;
                                }
                                else
                                {
                                    charsToAddBack.Push(charFromStack);
                                }
                            }
                        }
                    }

                    lineScores.Add(lineScore);
                }

                lineScores.Sort();

                var middle = ((int)(lineScores.Count / 2));

                var middleScore = lineScores[middle];

                Console.WriteLine(lineScores);
            }
        }
    }
}
