using System;
using System.Collections.Generic;
using System.IO;

namespace Day_21
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<Player>();

            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day21\input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var split = line.Split(" starting position: ");

                    var position = int.Parse(split[1]);
                    var playerName = split[0].Split(' ')[1];

                    players.Add(new Player(playerName, position));
                }
            }

            var currentPlayerIndex = 0;

            var dice = new DeterministicDice();

            var rollsPersRound = new int[27];
            for (var i = 1; i <=3; ++i)
            {
                for (var j = 1; j <= 3; ++j)
                {
                    for (var k =1; k <= 3; ++k)
                    {
                        rollsPersRound[(i-1) * 9 + (j-1) * 3 + (k-1)] = i + j + k;
                    }
                }
            }

            // position1, position2, score1, score2
            var scores = new Dictionary<(int, int, int, int), long>();
            scores.Add((players[0].Position, players[1].Position, 0, 0), 1);

            var player1WinCount = (long)0;
            var player2WinCount = (long)0;

            while (true)
            {
                // position1, position2, score1, score2
                var newScores = new Dictionary<(int, int, int, int), long>();
                
                foreach (var score in scores)
                {
                    foreach (var roll in rollsPersRound)
                    {
                        var newKey = score.Key;
                        if (currentPlayerIndex == 0)
                        {
                            var position = newKey.Item1;
                            position += roll;
                            while (position > 10)
                            {
                                position -= 10;
                            }

                            newKey = (position, newKey.Item2, newKey.Item3 + position, newKey.Item4);
                        }
                        else
                        {
                            var position = newKey.Item2;
                            position += roll;
                            while (position > 10)
                            {
                                position -= 10;
                            }

                            newKey = (newKey.Item1, position, newKey.Item3, newKey.Item4 + position);
                        }

                        if (newKey.Item3 >= 21)
                        {
                            player1WinCount += score.Value;
                        }
                        else if (newKey.Item4 >= 21)
                        {
                            player2WinCount += score.Value;
                        }
                        else
                        {
                            if (newScores.ContainsKey(newKey))
                            {
                                newScores[newKey] += score.Value;
                            }
                            else
                            {
                                newScores.Add(newKey, score.Value);
                            }
                        }
                    }
                }

                scores = newScores;

                if (scores.Count == 0)
                {
                    break;
                }

                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }

            Console.WriteLine("Player 1 wins: " + player1WinCount);
            Console.WriteLine("Player 2 wins: " + player2WinCount);

            //while (true)
            //{
            //    var player = players[currentPlayerIndex];

            //    var roll = dice.Roll() + dice.Roll() + dice.Roll();

            //    var newPosition = player.Position + roll;

            //    while (newPosition > 10)
            //    {
            //        newPosition -= 10;
            //    }

            //    player.SetPosition(newPosition);
            //    player.AddScore(newPosition);

            //    if (player.Score >= 1000)
            //    {
            //        break;
            //    }

            //    currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            //}

            //var losingPlayerScore = players[(currentPlayerIndex + 1) % 2].Score;
            //var rollCount = dice.RollCount;

            //Console.WriteLine("Out: " + losingPlayerScore * rollCount);
        }

        private abstract class Dice
        {
            public abstract int Roll();
        }

        private class DeterministicDice : Dice
        {
            private int _lastRoll = 100;

            private int _rollCount = 0;

            public int RollCount => this._rollCount;

            public override int Roll()
            {
                ++this._lastRoll;
                if (this._lastRoll > 100)
                {
                    this._lastRoll = 1;
                }

                ++this._rollCount;

                return this._lastRoll;
            }
        }

        private class Player
        {
            public Player(string id, int position)
            {
                Id = id;
                Position = position;
            }

            public string Id { get; }

            public int Position { get; private set; }

            public int Score { get; private set; }

            public void SetPosition(int position)
            {
                this.Position = position;
            }

            public void AddScore(int score)
            {
                this.Score += score;
            }
        }
    }
}
