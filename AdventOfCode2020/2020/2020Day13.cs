using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day13 : _2020Day<_2020Day13>
    {
        public _2020Day13() : base(13) { }

        public override string Calculate(string[] inputFile)
        {
            int time = int.Parse(inputFile[0]);
            int bestBus = 0;
            int bestBusTime = int.MaxValue;
            foreach((int, int) bus in inputFile[1].Split(',').Where(t => t != "x").Select(t => int.Parse(t)).Select( t => (t - (time % t), t)))
            {
                if (bus.Item1 < bestBusTime)
                {
                    bestBusTime = bus.Item1;
                    bestBus = bus.Item2;
                }
            }

            return (bestBus * bestBusTime).ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            string[] busTimeRaw = inputFile[1].Split(',');
            List<int?> busTimes = new List<int?>();
            for(int i = 0; i < busTimeRaw.Length; i++)
            {
                if (int.TryParse(busTimeRaw[i], out int result))
                {
                    busTimes.Add(result);
                }
                else 
                {
                    busTimes.Add(null);
                }
            }

            //Assumption - all the bus times are coprime, because inverse mod magic doesn't work without it, and that's going to be the clever solution I bet
            long interval = busTimes[0].Value;
            long bestTimeSoFar = 0;
            for (int i = 1; i < busTimes.Count; i++)
            {
                if (!busTimes[i].HasValue) continue;
                int currentBusNumber = busTimes[i].Value;
                long checkingTime = (((bestTimeSoFar / currentBusNumber) + 1) * currentBusNumber) - i;
                bool keepGoing = true;
                while (keepGoing)
                {
                    if (checkingTime > bestTimeSoFar)
                    {
                        bestTimeSoFar += interval;
                    }
                    else if (bestTimeSoFar > checkingTime)
                    {
                        //Let's cheat at counting
                        long test = checkingTime;
                        checkingTime = (((bestTimeSoFar / currentBusNumber) + 1) * currentBusNumber) - i;

                        //I'm too lazy to make this good, but it's pretty fast as is.
                        while (test >= checkingTime)
                        {
                            checkingTime += currentBusNumber;
                        }
                    }
                    else
                    {
                        keepGoing = false;
                        bestTimeSoFar = checkingTime;
                        interval *= currentBusNumber;
                    }
                }
            }

            return bestTimeSoFar.ToString();
        }
    }
}