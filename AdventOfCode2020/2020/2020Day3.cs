using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2020
{
    public class _2020Day3 : _2020Day<_2020Day3>
    {
        public _2020Day3() : base(3) { }

        public override string Calculate(string[] inputFile)
        {
            return Calculate(inputFile, 1, 3).ToString();
        }
        public int Calculate(string[] rawMap, int rise, int run)
        {
            bool[][] fullMap = rawMap.Select(t => t.Select(t => t == '#').ToArray()).ToArray();
            int locationRow = rise;
            int locationCol = run;
            int treeCount = 0;
            while(fullMap.Length > locationRow && fullMap[locationRow].Length > locationCol)
            {
                if (fullMap[locationRow][locationCol])
                {
                    treeCount++;
                }
                locationRow += rise;
                locationCol += run;
                locationCol %= fullMap[0].Length;
            }
            return treeCount;
        }
        public override string CalculateV2(string[] rawMap)
        {
            int result = 1;
            result *= Calculate(rawMap, 1, 1);
            result *= Calculate(rawMap, 1, 3);
            result *= Calculate(rawMap, 1, 5);
            result *= Calculate(rawMap, 1, 7);
            result *= Calculate(rawMap, 2, 1);
            return result.ToString();
        }
    }
}
