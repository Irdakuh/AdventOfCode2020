using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day10 : _2020Day<_2020Day10>
    {
        public _2020Day10() : base(10) { }

        public override string Calculate(string[] inputFile)
        {
            List<int> joltages = inputFile.Select(t => int.Parse(t)).ToList();
            joltages.Add(0);
            joltages.Add(joltages.Max() + 3);
            joltages.Sort();
            int[] joltageDifferentials = new int[joltages.Max()];
            for (int adapterIndex = 0; adapterIndex < joltages.Count - 1; adapterIndex++)
            {
                int differential = joltages[adapterIndex + 1] - joltages[adapterIndex];
                joltageDifferentials[differential] += 1;
            }

            return (joltageDifferentials[1] * joltageDifferentials[3]).ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            List<int> joltages = inputFile.Select(t => int.Parse(t)).ToList();
            joltages.Add(joltages.Max() + 3);
            joltages.Sort();
            long[] waysToReach = new long[joltages.Max() + 1];
            waysToReach[0] = 1;
            Func<int, long> getWaysToReachAdapter = adapter =>
            {
                if (adapter < 0) return 0;
                return waysToReach[adapter];
            };

            foreach(int adapter in joltages)
            {
                waysToReach[adapter] = getWaysToReachAdapter(adapter - 1) +
                                        getWaysToReachAdapter(adapter - 2) +
                                        getWaysToReachAdapter(adapter - 3);
            }
            return waysToReach[joltages.Max()].ToString();
        }
    }
}
