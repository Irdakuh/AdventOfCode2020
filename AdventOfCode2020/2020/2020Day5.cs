using System;
using System.Collections.Generic;
using System.Text;
namespace AdventOfCode2020
{
    public class _2020Day5 : _2020Day<_2020Day5>
    {
        public _2020Day5() : base(5) { }

        public override string Calculate(string[] passes)
        {
            List<int> seatIds = new List<int>();
            int maxSeat = 0;
            foreach (string pass in passes)
            {
                int row = BinaryToDecimal(pass.Substring(0, 7), 'B');
                int col = BinaryToDecimal(pass.Substring(7, 3), 'R');
                int seatId = row * 8 + col;
                seatIds.Add(seatId);
                maxSeat = Math.Max(seatId, maxSeat);
            }
            return maxSeat.ToString();
        }

        public override string CalculateV2(string[] passes)
        {
            List<int> seatIds = new List<int>();
            foreach (string pass in passes)
            {
                int row = BinaryToDecimal(pass.Substring(0, 7), 'B');
                int col = BinaryToDecimal(pass.Substring(7, 3), 'R');
                int seatId = row * 8 + col;
                seatIds.Add(seatId);
            }

            seatIds.Sort();
            int priorSeat = seatIds[0];
            for(int i = 1; i < seatIds.Count; i++)
            {
                if (seatIds[i] == priorSeat + 2)
                {
                    return (seatIds[i] - 1).ToString();
                }
                else
                {
                    priorSeat = seatIds[i];
                }
            }

            return "NO!";
        }

        public int BinaryToDecimal(string binary, char one)
        {
            int result = 0;
            foreach(char bit in binary)
            {
                result <<= 1;
                if (bit == one)
                {
                    result += 1;
                }
            }
            return result;
        }
    }
}
