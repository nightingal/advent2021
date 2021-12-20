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

            using (StreamReader stream = new StreamReader(@"C:\dev\advantofcode\Day4\test1\input.txt"))
            {
                var line = stream.ReadLine();
                stream.ReadLine();

                var numbers = line.Split(',');

                var boards = new List<Board>();

                var pendingBoardNumbers = new List<int>();

                while (!stream.EndOfStream)
                {
                    line = stream.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        boards.Add(new Board(new List<int>(pendingBoardNumbers)));
                        pendingBoardNumbers.Clear();
                    }
                    else
                    {
                        pendingBoardNumbers.AddRange(line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)));
                    }
                }

                if (pendingBoardNumbers.Count > 0)
                {
                    boards.Add(new Board(pendingBoardNumbers));
                }

                var largestTurnCount = 0;
                var largestScore = 0;

                foreach (var board in boards)  
                {
                    for (var i = 0; i < numbers.Length; ++i)
                    {
                        var num = int.Parse(numbers[i]);
                        board.AddDrawnNumber(num);

                        if (board.CheckForWin())
                        {
                            board.SetDone(i, num);
                            Console.WriteLine(board.TotalNumberDone + "   " + board.BoardScore);

                            if (board.TotalNumberDone > largestTurnCount)
                            {
                                largestTurnCount = board.TotalNumberDone;
                                largestScore = board.BoardScore;
                            }

                            break;

                        }
                    }
                }

                Console.WriteLine("Last board score: " + largestScore);

                //foreach (var board in boards)
                //{
                //    Console.WriteLine(board.TotalNumberDone + "   " + board.BoardScore);
                //}
            }
        }

        public struct Board
        {
            public Board(List<int> numbers)
            {
                this.Numbers = numbers;

                if (numbers.Count != 25)
                {
                    throw new Exception("Invalid array lenght. Must be 25");
                }

                this.Done = false;
                this.WinningNumber = 0;
                this.BoardScore = 0;
                this.TotalNumberDone = 0;
                this.NumberDrawn = new bool[25];
            }

            public void SetDone(int totalNumberDone, int winningNumber)
            {
                this.Done = true;
                this.TotalNumberDone = totalNumberDone;
                this.WinningNumber = winningNumber;
                this.BoardScore = GetSumOfUnmarkedNumber() * winningNumber;
            }

            public bool IsNumberDrawn(int row, int col)
            {
                return this.NumberDrawn[row * 5 + col];
            }

            public int GetSumOfUnmarkedNumber()
            {
                var sum = 0;

                for (var i = 0; i < 25; ++i)
                {
                    if (!this.NumberDrawn[i])
                    {
                        sum += this.Numbers[i];
                    }
                }

                return sum;
            }

            public void AddDrawnNumber(int num)
            {
                var index = this.Numbers.IndexOf(num);

                if (index != -1)
                {
                    this.NumberDrawn[index] = true;
                }
            }

            public bool CheckForWin()
            {
                for (var row = 0; row < 5; ++row)
                {
                    var drawnCountCol = 0;

                    for (var col = 0; col < 5; ++col)
                    {
                        if (this.IsNumberDrawn(row, col))
                        {
                            ++drawnCountCol;
                        }
                    }

                    if (drawnCountCol == 5)
                    {
                        return true;
                    }
                }

                for (var col = 0; col < 5; ++col)
                {
                    var drawnCount = 0;

                    for (var row = 0; row < 5; ++row)
                    {
                        if (this.IsNumberDrawn(row, col))
                        {
                            ++drawnCount;
                        }
                    }

                    if (drawnCount == 5)
                    {
                        return true;
                    }
                }

                return false;
            }

            public List<int> Numbers { get; }

            public bool[] NumberDrawn { get; }

            public bool Done;

            public int WinningNumber;

            public int BoardScore;

            public int TotalNumberDone;
        }
    }
}
