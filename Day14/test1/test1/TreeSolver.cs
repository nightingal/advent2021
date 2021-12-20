using System;
using System.Collections.Generic;
using System.Text;

namespace test1
{
    public class TreeSolver
    {
        public Dictionary<char, long> count = new Dictionary<char, long>();

        public void HandleNodeRecursive(int stepLeft, char char1, char char2, Dictionary<Tuple<char, char>, char> rules, bool isLeftNode)
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
                this.HandleNodeRecursive(stepLeft - 1, char1, toInsert, rules, true);
                this.HandleNodeRecursive(stepLeft - 1, toInsert, char2, rules, false);

                if (stepLeft > 1)
                {
                    count[toInsert]--;
                }
            }
        }
    }
}
