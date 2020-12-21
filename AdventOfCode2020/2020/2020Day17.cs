using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day17 : _2020Day<_2020Day17>
    {
        public _2020Day17() : base(17) { }

        public int GetNearbyActive((int, int, int) coordinate, Dictionary<(int, int, int), bool> map)
        {
            int count = 0;
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset++)
                {
                    for (int depOffset = -1; depOffset <= 1; depOffset++)
                    {
                        if (rowOffset == 0 && colOffset == 0 && depOffset == 0) continue;
                        (int, int, int) testCoordinate = (coordinate.Item1 + rowOffset, coordinate.Item2 + colOffset, coordinate.Item3 + depOffset);
                        if (map.TryGetValue(testCoordinate, out var value) && value)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public int GetNearbyActive((int, int, int, int) coordinate, Dictionary<(int, int, int, int), bool> map)
        {
            int count = 0;
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset++)
                {
                    for (int depOffset = -1; depOffset <= 1; depOffset++)
                    {
                        for (int wOffset = -1; wOffset <= 1; wOffset++)
                        {
                            if (rowOffset == 0 && colOffset == 0 && depOffset == 0 && wOffset == 0) continue;
                            (int, int, int, int) testCoordinate = (coordinate.Item1 + rowOffset, coordinate.Item2 + colOffset, coordinate.Item3 + depOffset, coordinate.Item4 + wOffset);
                            if (map.TryGetValue(testCoordinate, out var value) && value)
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        public override string Calculate(string[] inputFile)
        {
            //X, Y, Z -> row, col, dep
            Dictionary<(int, int, int), bool> map = new Dictionary<(int, int, int), bool>();
            Dictionary<(int, int, int), bool> nextMap = new Dictionary<(int, int, int), bool>();

            for (int row = 0; row < inputFile.Length; row++)
            {
                for (int col = 0; col < inputFile[row].Length; col++)
                {
                    map.Add((row, col, 0), inputFile[row][col] == '#');
                }
            }

            //Print map
            PrintMap(map);
            for (int cycle = 1; cycle <= 6; cycle++)
            {
                //Cycle
                int minRow = map.Keys.Min(coord => coord.Item1) - 1;
                int maxRow = map.Keys.Max(coord => coord.Item1) + 1;
                int minCol = map.Keys.Min(coord => coord.Item2) - 1;
                int maxCol = map.Keys.Max(coord => coord.Item2) + 1;
                int minDep = map.Keys.Min(coord => coord.Item3) - 1;
                int maxDep = map.Keys.Max(coord => coord.Item3) + 1;

                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        for (int dep = minDep; dep <= maxDep; dep++)
                        {
                            int nearbys = GetNearbyActive((row, col, dep), map);
                            bool alreadyActive = map.TryGetValue((row, col, dep), out var active) && active;
                            nextMap.Add((row, col, dep), (alreadyActive && nearbys == 2) || nearbys == 3);
                        }
                    }
                }

                map.Clear();
                foreach (var kvp in nextMap)
                {
                    map.Add(kvp.Key, kvp.Value);
                }
                nextMap.Clear();
            }

            //PrintMap(map);

            return map.Where(t => t.Value).Count().ToString();
        }

        public void PrintMap(Dictionary<(int, int, int), bool> map)
        {

            int minRow = map.Keys.Min(coord => coord.Item1);
            int maxRow = map.Keys.Max(coord => coord.Item1);
            int minCol = map.Keys.Min(coord => coord.Item2);
            int maxCol = map.Keys.Max(coord => coord.Item2);
            int minDep = map.Keys.Min(coord => coord.Item3);
            int maxDep = map.Keys.Max(coord => coord.Item3);
            for (int dep = minDep; dep <= maxDep; dep++)
            {
                Console.WriteLine();
                Console.WriteLine($"Z = {dep}");
                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        Console.Write(map.TryGetValue((row, col, dep), out var active) && active ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }

        public void PrintMap(Dictionary<(int, int, int, int), bool> map)
        {
            int minRow = map.Keys.Min(coord => coord.Item1);
            int maxRow = map.Keys.Max(coord => coord.Item1);
            int minCol = map.Keys.Min(coord => coord.Item2);
            int maxCol = map.Keys.Max(coord => coord.Item2);
            int minDep = map.Keys.Min(coord => coord.Item3);
            int maxDep = map.Keys.Max(coord => coord.Item3);
            int minW =   map.Keys.Min(coord => coord.Item4);
            int maxW =   map.Keys.Max(coord => coord.Item4);
            for (int w = minW; w <= maxW; w++)
            {
                Console.WriteLine();
                Console.WriteLine($"W = {w}");
                for (int dep = minDep; dep <= maxDep; dep++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Z = {dep}");
                    for (int row = minRow; row <= maxRow; row++)
                    {
                        for (int col = minCol; col <= maxCol; col++)
                        {
                            Console.Write(map.TryGetValue((row, col, dep, w), out var active) && active ? '#' : '.');
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        public override string CalculateV2(string[] inputFile)
        {
            //X, Y, Z -> row, col, dep
            Dictionary<(int, int, int, int), bool> map = new Dictionary<(int, int, int, int), bool>();
            Dictionary<(int, int, int, int), bool> nextMap = new Dictionary<(int, int, int, int), bool>();

            for (int row = 0; row < inputFile.Length; row++)
            {
                for (int col = 0; col < inputFile[row].Length; col++)
                {
                    map.Add((row, col, 0, 0), inputFile[row][col] == '#');
                }
            }

            //Print map
            PrintMap(map);
            for (int cycle = 1; cycle <= 6; cycle++)
            {
                //Cycle
                int minRow = map.Keys.Min(coord => coord.Item1) - 1;
                int maxRow = map.Keys.Max(coord => coord.Item1) + 1;
                int minCol = map.Keys.Min(coord => coord.Item2) - 1;
                int maxCol = map.Keys.Max(coord => coord.Item2) + 1;
                int minDep = map.Keys.Min(coord => coord.Item3) - 1;
                int maxDep = map.Keys.Max(coord => coord.Item3) + 1;
                int minW =   map.Keys.Min(coord => coord.Item4) - 1;
                int maxW =   map.Keys.Max(coord => coord.Item4) + 1;

                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        for (int dep = minDep; dep <= maxDep; dep++)
                        {
                            for (int w = minW; w <= maxW; w++)
                            {
                                int nearbys = GetNearbyActive((row, col, dep, w), map);
                                bool alreadyActive = map.TryGetValue((row, col, dep, w), out var active) && active;
                                nextMap.Add((row, col, dep, w), (alreadyActive && nearbys == 2) || nearbys == 3);
                            }
                        }
                    }
                }

                map.Clear();
                foreach (var kvp in nextMap)
                {
                    map.Add(kvp.Key, kvp.Value);
                }
                nextMap.Clear();
            }

            //PrintMap(map);

            return map.Where(t => t.Value).Count().ToString();
        }
    }
}
