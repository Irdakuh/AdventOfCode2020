using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day23 : _2020Day<_2020Day23>
    {
        public _2020Day23() : base(23) { }

        public override string Calculate(string[] inputFile)
        {
            List<int> cups = inputFile[0].Select(t => int.Parse(t.ToString())).ToList();
            int currentCup = cups[0];
            List<int> pickedUpCups = new List<int>();

            for (int round = 0; round < 100; round++)
            {
                int currentCupIndex = cups.IndexOf(currentCup);
                pickedUpCups.Clear();
                for (int i = 0; i < 3; i++)
                {
                    currentCupIndex = cups.IndexOf(currentCup);
                    if (currentCupIndex == cups.Count - 1)
                    {
                        //Reached the end, remove from the head
                        pickedUpCups.Add(cups[0]);
                        cups.RemoveAt(0);
                    }
                    else
                    {
                        pickedUpCups.Add(cups[currentCupIndex + 1]);
                        cups.RemoveAt(currentCupIndex + 1);
                    }
                }

                int destinationCup = currentCup - 1;
                while (cups.IndexOf(destinationCup) == -1)
                {
                    destinationCup--;
                    if (destinationCup < 0)
                    {
                        destinationCup = cups.Count; //Wrap to largest value
                    }
                }

                //We have destination, now insert
                cups.InsertRange(cups.IndexOf(destinationCup) + 1, pickedUpCups);

                currentCupIndex = cups.IndexOf(currentCup);
                int newCurrentCupIndex = (currentCupIndex + 1) % cups.Count;
                
                currentCup = cups[newCurrentCupIndex];

            }

            string resultString = string.Empty;
            int startingIndex = cups.IndexOf(1);
            for (int result = 1; result < cups.Count; result++)
            {
                resultString += cups[(startingIndex + result) % cups.Count];
            }

            return resultString;
        }

        public class CrabCup
        {
            public int label;
            public CrabCup next;
        }

        public override string CalculateV2(string[] inputFile)
        {
            Dictionary<int, CrabCup> cupReferences = new Dictionary<int, CrabCup>();
            CrabCup currentCup = new CrabCup();
            CrabCup last = currentCup;
            foreach(int cupLabel in inputFile[0].Select(t => int.Parse(t.ToString())))
            {
                last.label = cupLabel;
                last.next = new CrabCup();
                cupReferences.Add(last.label, last);
                last = last.next;
            }

            for (int i = 10; i <= 1000000; i++)
            {
                last.label = i;
                last.next = new CrabCup();
                cupReferences.Add(last.label, last);
                last = last.next;
            }

            cupReferences[1000000].next = currentCup;
            last = cupReferences[1000000];

            for (int round = 0; round < 10000000; round++)
            {
                CrabCup pickupStart = currentCup.next;
                CrabCup pickupEnd = currentCup.next.next.next;
                currentCup.next = pickupEnd.next; //pickupStart -> pickupEnd is now floating

                int destination = currentCup.label - 1;
                if (destination <= 0)
                {
                    destination = cupReferences.Count;
                }
                while (pickupStart.label == destination ||
                    pickupStart.next.label == destination ||
                    pickupStart.next.next.label == destination)
                {
                    destination--;
                    if (destination <= 0)
                    {
                        destination = cupReferences.Count;
                    }
                }

                CrabCup destinationCup = cupReferences[destination];

                CrabCup fixLoop = destinationCup.next;
                destinationCup.next = pickupStart;
                pickupEnd.next = fixLoop;

                currentCup = currentCup.next;
            }

            CrabCup startingCup = cupReferences[1];
            long result = (long)startingCup.next.label * startingCup.next.next.label;

            return result.ToString();
        }
    }
}
