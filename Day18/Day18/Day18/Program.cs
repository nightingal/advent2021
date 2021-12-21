using System;
using System.Collections.Generic;
using System.IO;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day18\input.txt"))
            {
                var pairs = new List<Pair>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    line = line.Substring(1, line.Length - 2);

                    pairs.Add(Parse(line, null));
                }

                //{
                //    pairs.Clear();
                //    pairs.Add(Parse("[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]", null));
                //    pairs.Add(Parse("7,[[[3,7],[4,3]],[[6,3],[8,8]]]", null));
                //}

                var index = 1;
                var pair = pairs[0];

                while (index < pairs.Count)
                {
                    var newPair = new Pair(pair, pairs[index], null);

                    //Console.WriteLine("Adding: " + pair.ToString() + " and " + pairs[index].ToString());
                    //Console.WriteLine(newPair.ToString());

                    RecursiveValidateParent(newPair);

                    while (true)
                    {
                        if (!DoRecursiveCheckExplode(newPair))
                        {
                            if (!DoRecursiveCheckSplit(newPair))
                            {
                                break;
                            }
                        }
                        //Console.WriteLine("Post operation: " + newPair);
                    }

                    //Console.WriteLine("Result post addition: " + newPair.ToString());

                    pair = newPair;

                    ++index;
                }

                Console.WriteLine(pair.ToString());
                Console.WriteLine("Magnitude: " + pair.GetMagnitude());
            }
        }

        static void RecursiveValidateParent(Pair root)
        {
            if (root is PairValue)
            { 
                return;
            }

            if (root.Left.Parent != root)
            {
                throw new Exception("Invalid parent");
            }

            if (root.Right.Parent != root)
            {
                throw new Exception("Invalid parent");
            }

            RecursiveValidateParent(root.Left);
            RecursiveValidateParent(root.Right);
        }

        static bool DoRecursiveCheckSplit(Pair root)
        {
            if (root is PairValue)
            {
                return false;
            }

            if (DoRecursiveCheckSplit(root.Left))
            {
                return true;
            }

            if (CheckSplit(root))
            {
                return true;
            }

            if (DoRecursiveCheckSplit(root.Right))
            {
                return true;
            }

            return false;
        }

        static bool DoRecursiveCheckExplode(Pair root)
        {
            if (root is PairValue)
            {
                return false;
            }

            if (DoRecursiveCheckExplode(root.Left))
            {
                return true;
            }

            if (CheckExplode(root))
            {
                return true;
            }

            if (DoRecursiveCheckExplode(root.Right))
            {
                return true;
            }

            return false;
        }

        static bool CheckExplode(Pair root)
        {
            if (root.Left is PairValue leftPairValue && root.Right is PairValue rightPairValue && root.Depth > 3)
            {
                // Need to find the left side and right side values to add our values to them
                {
                    // Left side
                    var parent = root.Parent;
                    var checkingPair = root;

                    do
                    {
                        if (parent.Left == checkingPair)
                        {
                            checkingPair = parent;
                            parent = parent.Parent;
                        }
                        else
                        {
                            var pairValue = GetRightMostValue(parent.Left);
                            pairValue.Value = pairValue.Value + leftPairValue.Value;
                            break;
                        }
                    } while (parent != null);
                }

                {
                    // Right side
                    var parent = root.Parent;
                    var checkingPair = root;

                    do
                    {
                        if (parent.Right == checkingPair)
                        {
                            checkingPair = parent;
                            parent = parent.Parent;
                        }
                        else
                        {
                            var pairValue = GetLeftMostValue(parent.Right);
                            pairValue.Value = pairValue.Value + rightPairValue.Value;
                            break;
                        }
                    } while (parent != null);
                }

                //then swap this pair for a value of 0
                bool isLeft = root.Parent.Left == root;

                if (isLeft)
                {
                    root.Parent.SetLeft(new PairValue(0, root.Parent));
                }
                else
                {
                    root.Parent.SetRight(new PairValue(0, root.Parent));
                }

                Console.WriteLine("Explode done");

                return true;
            }

            return false;
        }

        static PairValue GetRightMostValue(Pair root)
        {
            if (root is PairValue)
            {
                return root as PairValue;
            }
            else if (root.Right is PairValue val)
            {
                return val;
            }
            else
            {
                return GetRightMostValue(root.Right);
            }
        }

        static PairValue GetLeftMostValue(Pair root)
        {
            if (root is PairValue)
            {
                return root as PairValue;
            }
            else if (root.Left is PairValue val)
            {
                return val;
            }
            else
            {
                return GetLeftMostValue(root.Left);
            }
        }

        static bool CheckSplit(Pair root)
        {
            var changed = false;

            {
                if (root.Left is PairValue val && val.Value >= 10)
                {
                    var newPair = new Pair(null, null, root);
                    newPair.SetLeft(new PairValue((int)Math.Floor(val.Value / 2.0f), newPair));
                    newPair.SetRight(new PairValue((int)Math.Ceiling(val.Value / 2.0f), newPair));

                    root.SetLeft(newPair);
                    changed = true;
                }
            }

            if (!changed)
            {
                if (root.Right is PairValue val && val.Value >= 10)
                {
                    var newPair = new Pair(null, null, root);
                    newPair.SetLeft(new PairValue((int)Math.Floor(val.Value / 2.0f), newPair));
                    newPair.SetRight(new PairValue((int)Math.Ceiling(val.Value / 2.0f), newPair));

                    root.SetRight(newPair);
                    changed = true;
                }
            }

            if (changed)
            {
                Console.WriteLine("split done");
            }

            return changed;
        }

        static Pair Parse(string line, Pair parent)
        {
            var index = 0;

            var newPair = new Pair(null, null, parent);

            while (index < line.Length)
            {
                if (line[index] == '[')
                {
                    //todo find matching closing bracket
                    var openingBracketCount = 0;
                    var newClosingIndex = index + 1;

                    while (true)
                    {
                        if (line[newClosingIndex] == ']')
                        {
                            if (openingBracketCount == 0)
                            {
                                break;
                            }
                            else
                            { 
                                --openingBracketCount;
                            }
                        }
                        else if (line[newClosingIndex] == '[')
                        {
                            ++openingBracketCount;
                        }

                        newClosingIndex++;
                    }

                    var pair = Parse(line.Substring(index + 1, newClosingIndex - index - 1), newPair);
                    index = newClosingIndex + 1;

                    if (newPair.Left == null)
                    {
                        newPair.SetLeft(pair);
                    }
                    else
                    {
                        newPair.SetRight(pair);
                        break;
                    }
                }
                else if (line[index] == ',')
                {
                    ++index;
                }
                else
                {
                    var value = int.Parse(line[index].ToString());
                    if (newPair.Left == null)
                    {
                        newPair.SetLeft(new PairValue(value, newPair));
                        ++index;
                    }
                    else
                    {
                        newPair.SetRight(new PairValue(value, newPair));
                        break;
                    }
                }
            }

            return newPair;
        }

        public class Pair
        {
            public Pair(Pair left, Pair right, Pair parent)
            {
                this.Left = left;
                this.Right = right;

                this.Parent = parent;

                //Ensure childs have correct parent
                if (this.Left != null)
                {
                    this.Left.Parent = this;
                }

                if (this.Right != null)
                {
                    this.Right.Parent = this;
                }
            }

            public int Depth
            {
                get
                {
                    int count = 0;
                    var parent = this.Parent;
                    
                    while (parent != null)
                    {
                        ++count;
                        parent = parent.Parent;
                    }

                    return count;
                }
            }

            public Pair Left { get; private set; }

            public Pair Right { get; private set; }

            public Pair Parent { get; private set; }

            public override string ToString()
            {
                return "[" + this.Left.ToString() + "," + this.Right.ToString() + "]";
            }

            public void SetLeft(Pair left)
            {
                this.Left = left;

                if (left != null)
                {
                    left.Parent = this;
                }
            }

            public void SetRight(Pair right)
            {
                this.Right = right;

                if (right != null)
                {
                    right.Parent = this;
                }
            }

            public virtual int GetMagnitude()
            {
                return 3 * this.Left.GetMagnitude() + 2 * this.Right.GetMagnitude();
            }
        }

        public class PairValue : Pair
        {
            public int Value;

            public PairValue(int value, Pair parent) : base(null, null, parent)
            {
                this.Value = value;
            }

            public override string ToString()
            {
                return this.Value.ToString();
            }

            public override int GetMagnitude()
            {
                return Value;
            }
        }
    }
}
