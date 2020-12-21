using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day15 : _2020Day<_2020Day15>
    {
        public _2020Day15() : base(15) { }

        public override string Calculate(string[] inputFile)
        {
            foreach(string input in inputFile)
            {
                List<int> numbers = input.Split(',').Select(t => int.Parse(t)).ToList();
                Dictionary<int, (int, int)> lastSpoken = new Dictionary<int, (int, int)>();
                int numberSpoken = 1;
                int lastNumberSpoken = 0;
                for (int i = 0; i < numbers.Count; i++)
                {
                    lastSpoken.Add(numbers[i], (numberSpoken, numberSpoken));
                    lastNumberSpoken = numbers[i];
                    numberSpoken++;
                }
                while (numberSpoken <= 2020)
                {
                    //If they match, 0.
                    int numberToSpeak = lastSpoken[lastNumberSpoken].Item1 - lastSpoken[lastNumberSpoken].Item2;
                    if (lastSpoken.ContainsKey(numberToSpeak))
                    {
                        lastSpoken[numberToSpeak] = (numberSpoken, lastSpoken[numberToSpeak].Item1);
                    }
                    else
                    {
                        lastSpoken.Add(numberToSpeak, (numberSpoken, numberSpoken));
                    }
                    lastNumberSpoken = numberToSpeak;
                    numberSpoken++;
                }
                Console.WriteLine(lastNumberSpoken.ToString());
            }
            return null;
        }

        public override string CalculateV2(string[] inputFile)
        {
            foreach (string input in inputFile)
            {
                List<int> numbers = input.Split(',').Select(t => int.Parse(t)).ToList();
                Dictionary<int, (int, int)> lastSpoken = new Dictionary<int, (int, int)>();
                int numberSpoken = 1;
                int lastNumberSpoken = 0;
                for (int i = 0; i < numbers.Count; i++)
                {
                    lastSpoken.Add(numbers[i], (numberSpoken, numberSpoken));
                    lastNumberSpoken = numbers[i];
                    numberSpoken++;
                }
                while (numberSpoken <= 30000000)
                {
                    //If they match, 0.
                    int numberToSpeak = lastSpoken[lastNumberSpoken].Item1 - lastSpoken[lastNumberSpoken].Item2;
                    if (lastSpoken.ContainsKey(numberToSpeak))
                    {
                        lastSpoken[numberToSpeak] = (numberSpoken, lastSpoken[numberToSpeak].Item1);
                    }
                    else
                    {
                        lastSpoken.Add(numberToSpeak, (numberSpoken, numberSpoken));
                    }
                    lastNumberSpoken = numberToSpeak;
                    numberSpoken++;
                }
                Console.WriteLine(lastNumberSpoken.ToString());
            }

            return null;
        }
    }
}
