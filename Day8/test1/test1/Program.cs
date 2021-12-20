using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test1
{
    /// <summary>
    ///   Pos1
    ///  P    P 
    ///  O    O
    ///  S    S
    ///  2    3
    ///   Pos4
    ///  P    P
    ///  O    O
    ///  S    S
    ///  5    6
    ///   Pos7
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day8\input.txt"))
            {
                var sum = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var splitted = line.Split(" | ");

                    var input = splitted[0].Split(' ');
                    var output = splitted[1].Split(' ');

                    var result = ResolveLine(input.ToList(), output.ToList());

                    sum += result;
                }

                Console.WriteLine(sum);
            }
        }

        const string Position1 = "pos1";
        const string Position2 = "pos2";
        const string Position3 = "pos3";
        const string Position4 = "pos4";
        const string Position5 = "pos5";
        const string Position6 = "pos6";
        const string Position7 = "pos7";


        private static int ResolveLine(List<string> input, List<String> output)
        {
            var posToValue = new Dictionary<string, char>();

            // Let's sort all input first, since this will make it easier avec to compare
            for (var i = 0; i < input.Count; ++i)
            {
                var arr = input[i].ToArray();
                Array.Sort(arr);
                input[i] = new string(arr);
            }
            
            // First find the value for 1 (length 2)
            var indexOne = input.FindIndex(x => x.Length == 2);

            if (indexOne == -1)
            {
                throw new Exception("Unable to locate number 1");
            }

            // We have the two positions for the right side
            var valuesOne = input[indexOne];


            // Then let's find number 7 (length 3)
            var indexSeven = input.FindIndex(x => x.Length == 3);

            if (indexSeven == -1)
            {
                throw new Exception("Unable to locate number 7");
            }

            var valuesSeven = input[indexSeven];

            // We can now isolate the POS1 value
            var pos1 = valuesSeven.Where(x => valuesOne.IndexOf(x) == -1).First();
            posToValue[Position1] = pos1;

            // Then let's find the index for number 4 (length of 4)
            var indexFour = input.FindIndex(x => x.Length == 4);

            if (indexFour == -1)
            {
                throw new Exception("Unable to locate number 4");
            }

            var valuesFour = input[indexFour];

            // We should now be able to identify the index of number 9 since it is composed of all characters of 4 + top and bottom line. 0 and 6 don't have all characters of 4
            var lookingForNineValues = new List<char>(valuesFour);
            lookingForNineValues.Add(pos1);
            lookingForNineValues.Sort();
            var lookingForNineStr = new string(lookingForNineValues.ToArray());

            var indexNine = input.FindIndex(x => x.Length == 6 && lookingForNineStr.All(y => x.Contains(y)));

            if (indexNine == -1)
            {
                throw new Exception("unable to locate number 9");
            }

            var valuesNine = input[indexNine];

            // We can now find the index of 0, which is the same number of digits (6) than 9, ensuring that we have both values that one have (since 6 doesn't have those values)
            var indexZero = input.FindIndex(x => x.Length == 6 & input.IndexOf(x) != indexNine && x.Contains(valuesOne[0]) && x.Contains(valuesOne[1]));

            if (indexZero == -1)
            {
                throw new Exception("Unable to locate number 0");
            }

            var valuesZero = input[indexZero];

            // We can now find the index of 6, which is the same number of digits (6) dans 9 and 0, but a different index
            var indexSix = input.FindIndex(x => x.Length == 6 && input.IndexOf(x) != indexZero && input.IndexOf(x) != indexNine);

            if (indexSix == -1)
            {
                throw new Exception("Unable to locate number 6");
            }

            var valuesSix = input[indexSix];

            // Now that we have 6, we can find the value of Pos3. We know that only one of the pos (pos6) is in 6
            var pos3 = valuesOne.First(x => valuesSix.IndexOf(x) == -1);
            posToValue[Position3] = pos3;

            // Now we can defined pos6
            var pos6 = valuesOne.First(x => x != pos3);
            posToValue[Position6] = pos6;

            // We can now find the pos7 value by isolating it into nine
            var toStrip = valuesNine.ToList();
            toStrip.Remove(valuesFour[0]);
            toStrip.Remove(valuesFour[1]);
            toStrip.Remove(valuesFour[2]);
            toStrip.Remove(valuesFour[3]);
            toStrip.Remove(pos1);

            var pos7 = toStrip.First();
            posToValue[Position7] = pos7;

            // We can now find the inddex of number 3, since it is the only 5 digid number containing pos1, pos3, pos6 and pos7
            var indexThree = input.FindIndex(x => x.Length == 5 && x.Contains(pos1) && x.Contains(pos3) && x.Contains(pos6) && x.Contains(pos7));

            if (indexThree == -1)
            {
                throw new Exception("unable to find number 3");
            }

            var valuesThree = input[indexThree];

            // We can now find the value of pos4 by removing all other know pos from 3;
            toStrip = valuesThree.ToList();
            toStrip.Remove(pos1);
            toStrip.Remove(pos3);
            toStrip.Remove(pos6);
            toStrip.Remove(pos7);

            var pos4 = toStrip.First();
            posToValue[Position4] = pos4;

            // we can now isolate pos2 from the values of 9
            toStrip = valuesNine.ToList();
            toStrip.Remove(pos1);
            toStrip.Remove(pos3);
            toStrip.Remove(pos4);
            toStrip.Remove(pos6);
            toStrip.Remove(pos7);

            var pos2 = toStrip.First();
            posToValue[Position2] = pos2;

            // We can finaly guess pos 5;
            toStrip = "abcdefg".ToList();
            toStrip.Remove(pos1);
            toStrip.Remove(pos2);
            toStrip.Remove(pos3);
            toStrip.Remove(pos4);
            toStrip.Remove(pos6);
            toStrip.Remove(pos7);

            var pos5 = toStrip.First();
            posToValue[Position5] = pos5;

            return SolveOutput(output, posToValue);
        }

        /// <summary>
        ///   Pos1
        ///  P    P 
        ///  O    O
        ///  S    S
        ///  2    3
        ///   Pos4
        ///  P    P
        ///  O    O
        ///  S    S
        ///  5    6
        ///   Pos7
        /// </summary>

        private static int SolveOutput(List<string> output, Dictionary<string, char> posToValue)
        {
            var numberToPositions = new Dictionary<char, List<String>>()
            {
                { '0', new List<string>() {Position1, Position2, Position3, Position5, Position6, Position7 } },
                { '1', new List<string>() {Position3, Position6 } },
                { '2', new List<string>() {Position1, Position3, Position4, Position5, Position7} },
                { '3', new List<string>() {Position1, Position3, Position4, Position6, Position7} },
                { '4', new List<string>() {Position2, Position3, Position4, Position6} },
                { '5', new List<string>() {Position1, Position2, Position4, Position6, Position7} },
                { '6', new List<string>() {Position1, Position2, Position4, Position5, Position6, Position7} },
                { '7', new List<string>() {Position1, Position3, Position6} },
                { '8', new List<string>() {Position1, Position2, Position3, Position4, Position5, Position6, Position7} },
                { '9', new List<string>() {Position1, Position2, Position3, Position4, Position6, Position7} },
            };

            var strToInt = new Dictionary<string, char>();
            foreach (var pair in numberToPositions)
            {
                var key = pair.Key;
                var positions = pair.Value;

                var newKey = "";
                foreach (var position in positions)
                {
                    newKey += posToValue[position];
                }

                var arr = newKey.ToArray();
                Array.Sort(arr);

                strToInt.Add(new string(arr), key);
            }

            // Let's sort all input first, since this will make it easier avec to compare
            for (var i = 0; i < output.Count; ++i)
            {
                var arr = output[i].ToArray();
                Array.Sort(arr);
                output[i] = new string(arr);
            }

            var outputNumber = "";

            foreach (var value in output)
            {
                var num = strToInt[value];
                outputNumber += num;
            }

            return int.Parse(outputNumber);
        }
    }
}
