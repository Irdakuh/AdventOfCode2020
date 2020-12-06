using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day2 : _2020Day<_2020Day2>
    {
        public _2020Day2() : base(2) { }

        public override string Calculate(string[] passwords)
        {
            int validPasswords = 0;
            foreach(string passwordFile in passwords)
            {
                string[] split = passwordFile.Split(' ');
                string[] counts = split[0].Split('-');
                int minCount = int.Parse(counts[0]);
                int maxCount = int.Parse(counts[1]);
                char letter = split[1][0];
                string password = split[2];
                int numOccurences = password.Count(c => c == letter);
                if (numOccurences >= minCount && numOccurences <= maxCount)
                {
                    validPasswords++;
                }
            }
            return validPasswords.ToString();
        }

        public override string CalculateV2(string[] passwords)
        {
            int validPasswords = 0;
            foreach (string passwordFile in passwords)
            {
                string[] split = passwordFile.Split(' ');
                string[] counts = split[0].Split('-');
                int firstCount = int.Parse(counts[0]);
                int secondCount = int.Parse(counts[1]);
                char letter = split[1][0];
                string password = split[2];
                if (password[firstCount - 1] == letter ^ password[secondCount - 1] == letter)
                {
                    validPasswords++;
                }
            }
            return validPasswords.ToString();
        }
    }
}
