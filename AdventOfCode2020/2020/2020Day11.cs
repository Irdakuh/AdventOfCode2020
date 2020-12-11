using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day11 : _2020Day<_2020Day11>
    {
        public _2020Day11() : base(11) { }

        public enum SeatStatus
        {
            Floor,
            Empty,
            Filled
        }

        public List<List<SeatStatus>> GetSeatMap(string[] inputFile)
        {
            List<List<SeatStatus>> seats = new List<List<SeatStatus>>();
            foreach (string row in inputFile)
            {
                List<SeatStatus> seatRow = new List<SeatStatus>();
                foreach (char col in row)
                {
                    seatRow.Add(col == 'L' ? SeatStatus.Empty : SeatStatus.Floor);
                }
                seats.Add(seatRow);
            }
            return seats;
        }

        public bool CopySeatMap(ref List<List<SeatStatus>> original, ref List<List<SeatStatus>> destination)
        {
            bool madeAChange = false;
            for (int row = 0; row < original.Count; row++)
            {
                for (int col = 0; col < original[row].Count; col++)
                {
                    if (original[row][col] != destination[row][col])
                    {
                        madeAChange = true;
                        destination[row][col] = original[row][col];
                    }
                }
            }
            return madeAChange;
        }

        public int GetFilledAdjacentSeats(List<List<SeatStatus>> seatMap, int row, int col)
        {
            int filledSeatCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                if (row + i < 0 || row + i >= seatMap.Count) continue;
                for (int j = -1; j <= 1; j++)
                {
                    if (col + j < 0 || col + j >= seatMap[row + i].Count) continue;
                    if (i == 0 && j == 0) continue;
                    filledSeatCount += seatMap[row + i][col + j] == SeatStatus.Filled ? 1 : 0;
                }
            }
            return filledSeatCount;
        }

        public int GetFilledSeats(List<List<SeatStatus>> seatMap)
        {
            int filledSeats = 0;
            for (int row = 0; row < seatMap.Count; row++)
            {
                for (int col = 0; col < seatMap[row].Count; col++)
                {
                    filledSeats += seatMap[row][col] == SeatStatus.Filled ? 1 : 0;
                }
            }
            return filledSeats;
        }

        public override string Calculate(string[] inputFile)
        {
            var seats = GetSeatMap(inputFile);
            var nextSeats = GetSeatMap(inputFile);

            do
            {
                for (int row = 0; row < seats.Count; row++)
                {
                    for (int col = 0; col < seats[row].Count; col++)
                    {
                        if (seats[row][col] == SeatStatus.Floor) continue;
                        int adjacentFilledSeats = GetFilledAdjacentSeats(seats, row, col);
                        if (seats[row][col] == SeatStatus.Empty && adjacentFilledSeats == 0)
                        {
                            nextSeats[row][col] = SeatStatus.Filled;
                        }
                        if (seats[row][col] == SeatStatus.Filled && adjacentFilledSeats >= 4)
                        {
                            nextSeats[row][col] = SeatStatus.Empty;
                        }
                    }
                }
            }
            while (CopySeatMap(ref nextSeats, ref seats));
            return GetFilledSeats(seats).ToString();
        }

        public int GetFilledInLineSeats(List<List<SeatStatus>> seatMap, int row, int col)
        {
            int filledSeatCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int rowTest = row + i;
                    int colTest = col + j;

                    while (rowTest >= 0 && rowTest < seatMap.Count &&
                        colTest >= 0 && colTest < seatMap[rowTest].Count &&
                        seatMap[rowTest][colTest] == SeatStatus.Floor)
                    {
                        rowTest += i;
                        colTest += j;
                    }

                    if (rowTest >= 0 && rowTest < seatMap.Count &&
                        colTest >= 0 && colTest < seatMap[rowTest].Count)
                    {
                        filledSeatCount += seatMap[rowTest][colTest] == SeatStatus.Filled ? 1 : 0;
                    }
                }
            }
            return filledSeatCount;
        }

        public override string CalculateV2(string[] inputFile)
        {
            var seats = GetSeatMap(inputFile);
            var nextSeats = GetSeatMap(inputFile);

            do
            {
                for (int row = 0; row < seats.Count; row++)
                {
                    for (int col = 0; col < seats[row].Count; col++)
                    {
                        if (seats[row][col] == SeatStatus.Floor) continue;
                        int inLineFilledSeats = GetFilledInLineSeats(seats, row, col);
                        if (seats[row][col] == SeatStatus.Empty && inLineFilledSeats == 0)
                        {
                            nextSeats[row][col] = SeatStatus.Filled;
                        }
                        if (seats[row][col] == SeatStatus.Filled && inLineFilledSeats >= 5)
                        {
                            nextSeats[row][col] = SeatStatus.Empty;
                        }
                    }
                }
            }
            while (CopySeatMap(ref nextSeats, ref seats));
            return GetFilledSeats(seats).ToString();
        }
    }
}
