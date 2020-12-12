using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day12 : _2020Day<_2020Day12>
    {
        public _2020Day12() : base(12) { }

        //enum Heading
        //{
        //    East = 0,
        //    South = 1,
        //    West = 2,
        //    North = 3
        //}

        public void ObeyCommand(char order, int value)
        {
            switch (order)
            {
                case 'N':
                    shipLatitude += value;
                    break;
                case 'S':
                    shipLatitude -= value;
                    break;
                case 'E':
                    shipLongitude += value;
                    break;
                case 'W':
                    shipLongitude -= value;
                    break;
                case 'L':
                    heading -= (value / 90);
                    heading += 4;
                    heading %= 4;
                    break;
                case 'R':
                    heading += (value / 90);
                    heading += 4;
                    heading %= 4;
                    break;
                case 'F':

                    //enum Heading
                    //{
                    //    East = 0,
                    //    South = 1,
                    //    West = 2,
                    //    North = 3
                    //}
                    switch(heading)
                    {
                        case 0:
                            ObeyCommand('E', value);
                            break;
                        case 1:
                            ObeyCommand('S', value);
                            break;
                        case 2:
                            ObeyCommand('W', value);
                            break;
                        case 3:
                            ObeyCommand('N', value);
                            break;
                    }
                    break;
            }
        }

        public int shipLatitude = 0; //-South +North
        public int shipLongitude = 0; //-West +East
        public int heading = 0;

        public override string Calculate(string[] inputFile)
        {
            shipLatitude = 0; //-South +North
            shipLongitude = 0; //-West +East
            heading = 0;
            foreach (string command in inputFile)
            {
                char order = command[0];
                int value = int.Parse(command.Substring(1));
                ObeyCommand(order, value);
            }

            return (Math.Abs(shipLatitude) + Math.Abs(shipLongitude)).ToString();
        }

        public int waypointLatitude = 1;
        public int waypointLongitude = 10;

        public void ObeyWaypointCommand(char order, int value)
        {
            switch (order)
            {
                case 'N':
                    waypointLatitude += value;
                    break;
                case 'S':
                    waypointLatitude -= value;
                    break;
                case 'E':
                    waypointLongitude += value;
                    break;
                case 'W':
                    waypointLongitude -= value;
                    break;
                case 'L':
                    while(value > 0)
                    {
                        int tempL = waypointLatitude;
                        waypointLatitude = waypointLongitude;
                        waypointLongitude = -tempL;
                        value -= 90;
                    }
                    break;
                case 'R':
                    while (value > 0)
                    {
                        int tempR = waypointLatitude;
                        waypointLatitude = -waypointLongitude;
                        waypointLongitude = tempR;
                        value -= 90;
                    }
                    break;
                case 'F':
                    shipLatitude += waypointLatitude * value;
                    shipLongitude += waypointLongitude * value;
                    break;
            }
        }

        public override string CalculateV2(string[] inputFile)
        {
            waypointLatitude = 1;
            waypointLongitude = 10;
            shipLatitude = 0; //-South +North
            shipLongitude = 0; //-West +East
            heading = 0;
            foreach (string command in inputFile)
            {
                char order = command[0];
                int value = int.Parse(command.Substring(1));
                ObeyWaypointCommand(order, value);
            }

            return (Math.Abs(shipLatitude) + Math.Abs(shipLongitude)).ToString();
        }
    }
}
