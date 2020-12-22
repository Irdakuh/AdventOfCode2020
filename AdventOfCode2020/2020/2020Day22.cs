using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day22 : _2020Day<_2020Day22>
    {
        public _2020Day22() : base(22) { }

        public override string Calculate(string[] inputFile)
        {
            Queue<int> player1 = new Queue<int>();
            Queue<int> player2 = new Queue<int>();

            Queue<int> currentReadingPlayer = player1;
            foreach (string line in inputFile)
            {
                if (string.IsNullOrEmpty(line))
                {
                    currentReadingPlayer = player2;
                }
                else if (line.StartsWith("Player"))
                {
                    continue;
                }
                else
                {
                    currentReadingPlayer.Enqueue(int.Parse(line));
                }
            }

            while (player1.Count > 0 && player2.Count > 0)
            {
                int player1Card = player1.Dequeue();
                int player2Card = player2.Dequeue();
                if (player1Card > player2Card)
                {
                    player1.Enqueue(player1Card);
                    player1.Enqueue(player2Card);
                }
                else
                {
                    player2.Enqueue(player2Card);
                    player2.Enqueue(player1Card);
                }
            }

            Queue<int> winningPlayer = player1.Count == 0 ? player2 : player1;

            int multiplier = winningPlayer.Count;
            int sum = 0;
            while (winningPlayer.Count > 0)
            {
                sum += multiplier-- * winningPlayer.Dequeue();
            }

            return sum.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            Queue<int> player1 = new Queue<int>();
            Queue<int> player2 = new Queue<int>();

            Queue<int> currentReadingPlayer = player1;
            foreach (string line in inputFile)
            {
                if (string.IsNullOrEmpty(line))
                {
                    currentReadingPlayer = player2;
                }
                else if (line.StartsWith("Player"))
                {
                    continue;
                }
                else
                {
                    currentReadingPlayer.Enqueue(int.Parse(line));
                }
            }

            Queue<int> winningPlayer = PlayRecursively(player1, player2) ? player1 : player2;

            int multiplier = winningPlayer.Count;
            int sum = 0;
            while (winningPlayer.Count > 0)
            {
                sum += multiplier-- * winningPlayer.Dequeue();
            }

            return sum.ToString();
        }

        public int GetPlayerHash(Queue<int> player)
        {
            string stringBuilder = string.Empty;
            foreach(int number in player)
            {
                if (number < 10)
                {
                    stringBuilder += "0";
                }
                stringBuilder += number;
            }
            return stringBuilder.GetHashCode();
        }

        //True if player1 wins
        public bool PlayRecursively(Queue<int> player1, Queue<int> player2)
        {
            List<(int, int)> roundHashes = new List<(int, int)>();
            while (player1.Count > 0 && player2.Count > 0)
            {
                int player1Hash = GetPlayerHash(player1);
                int player2Hash = GetPlayerHash(player2);
                if (roundHashes.Contains((player1Hash, player2Hash)))
                {
                    return true;
                }
                else
                {
                    roundHashes.Add((player1Hash, player2Hash));
                }

                int player1Card = player1.Dequeue();
                int player2Card = player2.Dequeue();

                bool? player1Wins = null;

                if (player1.Count >= player1Card && player2.Count >= player2Card)
                {
                    Queue<int> newPlayer1 = new Queue<int>(player1.ToList().GetRange(0, player1Card));
                    Queue<int> newPlayer2 = new Queue<int>(player2.ToList().GetRange(0, player2Card));
                    player1Wins = PlayRecursively(newPlayer1, newPlayer2);
                }
                
                if ((player1Wins.HasValue && player1Wins.Value) || (!player1Wins.HasValue && player1Card > player2Card))
                {
                    player1.Enqueue(player1Card);
                    player1.Enqueue(player2Card);
                }
                else
                {
                    player2.Enqueue(player2Card);
                    player2.Enqueue(player1Card);
                }
            }

            return player1.Count > 0;
        }
    }
}
