using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day1 : _2020Day<_2020Day1>
    {
        public _2020Day1() : base(1) { }

        public override string Calculate(string[] inputFile)
        {
            var listInt = inputFile.Select(t => int.Parse(t));
            foreach (int first in listInt)
            {
                foreach (int second in listInt)
                {
                    if (first + second == 2020)
                    {
                        return (first * second).ToString();
                    }
                }
            }

            return null;
        }

        public override string CalculateV2(string[] inputFile)
        {
            var listInt = inputFile.Select(t => int.Parse(t));
            foreach (int first in listInt)
            {
                foreach (int second in listInt)
                {
                    if (first + second >= 2020)
                    {
                        continue;
                    }
                    if (listInt.Contains(2020 - (first + second)))
                    {
                        int result = first * second * (2020 - (first + second));
                        return result.ToString();
                    }
                }
            }
            return null;
        }
    }
}
