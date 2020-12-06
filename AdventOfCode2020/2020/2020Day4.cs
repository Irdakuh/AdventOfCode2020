using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day4 : _2020Day<_2020Day4>
    {
        public _2020Day4() : base(4) { }

        private List<string> necessaryComponents = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        //I don't want to RegEx and you can't make me!
        //But it does make today *gross*
        public override string Calculate(string[] input)
        {
            List<Tuple<string, string>> currentPassportParts = new List<Tuple<string, string>>();
            int validPassports = 0;

            foreach(string inputString in input)
            {
                if (inputString.Equals(string.Empty))
                {
                    validPassports += Validate(currentPassportParts) ? 1 : 0;
                    currentPassportParts.Clear();
                }
                else
                {
                    currentPassportParts.AddRange(inputString.Split(' ').Select(t => new Tuple<string, string>(t.Split(":")[0], t.Split(":")[1])));
                }
            }

            validPassports += Validate(currentPassportParts) ? 1 : 0;
            currentPassportParts.Clear();
            return validPassports.ToString();
        }

        private bool Validate(List<Tuple<string, string>> currentPassportParts)
        {
            List<string> firsts = currentPassportParts.Select(t => t.Item1).ToList();
            foreach (string component in necessaryComponents)
            {
                if (!firsts.Contains(component))
                {
                    return false;
                }
            }
            return true;
        }

        public override string CalculateV2(string[] inputFile)
        {
            List<Tuple<string, string>> currentPassportParts = new List<Tuple<string, string>>();
            int validPassports = 0;

            foreach (string inputString in inputFile)
            {
                if (inputString.Equals(string.Empty))
                {
                    validPassports += ValidateV2(currentPassportParts) ? 1 : 0;
                    currentPassportParts.Clear();
                }
                else
                {
                    currentPassportParts.AddRange(inputString.Split(' ').Select(t => new Tuple<string, string>(t.Split(":")[0], t.Split(":")[1])));
                }
            }

            validPassports += ValidateV2(currentPassportParts) ? 1 : 0;
            currentPassportParts.Clear();
            return validPassports.ToString();
        }

        private bool ValidateV2(List<Tuple<string, string>> currentPassportParts)
        {
            List<string> firsts = currentPassportParts.Select(t => t.Item1).ToList();
            foreach (string component in necessaryComponents)
            {
                if (!firsts.Contains(component))
                {
                    return false;
                }
            }

            foreach (Tuple<string, string> couple in currentPassportParts)
            {
                switch(couple.Item1)
                {
                    case "byr":
                        if (!ValidateYear(couple.Item2, 1920, 2002)) return false;
                        break;
                    case "iyr":
                        if (!ValidateYear(couple.Item2, 2010, 2020)) return false;
                        break;
                    case "eyr":
                        if (!ValidateYear(couple.Item2, 2020, 2030)) return false;
                        break;
                    case "hgt":
                        string last2 = couple.Item2.Substring(couple.Item2.Length - 2, 2);
                        if (last2 != "in" && last2 != "cm") return false;
                        if (!int.TryParse(couple.Item2.Substring(0, couple.Item2.Length - 2), out var height)) return false;
                        if (last2 == "in")
                        {
                            if (height < 59 || height > 76) return false;
                        }
                        else
                        {
                            if (height < 150 || height > 193) return false;
                        }
                        break;
                    case "hcl":
                        if (couple.Item2[0] != '#') return false;
                        if (couple.Item2.Length != 7) return false;
                        foreach(char character in couple.Item2.Substring(1, 6))
                        {
                            if (!(character >= '0' && character <= '9') && !(character >= 'a' && character <= 'f')) return false;
                        }
                        break;
                    case "ecl":
                        if (!eyeColors.Contains(couple.Item2)) return false;
                        break;
                    case "pid":
                        if (!(couple.Item2.Length == 9) || !int.TryParse(couple.Item2, out var _)) return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private static List<string> eyeColors = new List<string> { "amb", "blu", "brn", "grn", "gry", "hzl", "oth"};

        private static bool ValidateYear(string year, int minYear, int maxYear)
        {
            if (int.TryParse(year, out var parsedYear))
            {
                if (parsedYear < minYear || parsedYear > maxYear)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
