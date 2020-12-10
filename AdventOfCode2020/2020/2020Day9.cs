using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day9 : _2020Day<_2020Day9>
    {
        public _2020Day9() : base(9) { }

        public override string Calculate(string[] inputFile)
        {
            List<long> numbers = inputFile.Select(t => long.Parse(t)).ToList();
            //Deal with my insertion of the preamble
            int preamble = (int)numbers[0];
            numbers.RemoveAt(0);
            List<long> allSums = new List<long>();

            for (int walkingIndex = 0; walkingIndex < numbers.Count; walkingIndex++)
            {
                for (int preambleIndex = walkingIndex + 1; preambleIndex < walkingIndex + preamble; preambleIndex++)
                {
                    if (preambleIndex >= numbers.Count)
                    {
                        allSums.Add(0); //Guaranteed false
                    }
                    else
                    {
                        allSums.Add(numbers[walkingIndex] + numbers[preambleIndex]);
                    }
                    
                    //This should come in sets of preamble-1
                }
            }

            for(int checkingIndex = preamble; checkingIndex < numbers.Count; checkingIndex++)
            {
                int checkNumber = checkingIndex - preamble;
                List<long> possibleSums = allSums.GetRange(checkNumber * (preamble - 1), preamble * (preamble - 1));
                if (!possibleSums.Contains(numbers[checkingIndex]))
                {
                    return numbers[checkingIndex].ToString();
                }
            }

            return "Could not find an invalid sum";
        }

        public override string CalculateV2(string[] inputFile)
        {
            List<long> numbers = inputFile.Select(t => long.Parse(t)).ToList();
            //Deal with my insertion of the preamble
            int preamble = (int)numbers[0];
            numbers.RemoveAt(0);
            long invalidNumber = long.Parse(Calculate(inputFile));

            long sum = numbers[0];
            int firstIndex = 0;
            int secondIndex = 0;
            while (sum != invalidNumber)
            {
                if (sum < invalidNumber)
                {
                    secondIndex++;
                    sum += numbers[secondIndex];
                } 
                else if (sum > invalidNumber)
                {
                    sum -= numbers[firstIndex];
                    firstIndex++;
                }
            }
            List<long> subset = numbers.GetRange(firstIndex, secondIndex - firstIndex);
            return (subset.Min() + subset.Max()).ToString();
        }
    }
}
