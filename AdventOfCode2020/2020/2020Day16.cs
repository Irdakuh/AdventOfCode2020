using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2020
{
    public class _2020Day16 : _2020Day<_2020Day16>
    {
        public _2020Day16() : base(16) { }

        public struct Rule
        {
            public string name;
            public (int, int) firstRule;
            public (int, int) secondRule;
        }

        public (int, int) GetRange(string input)
        {
            string[] brokenInput = input.Split('-');
            return (int.Parse(brokenInput[0]), int.Parse(brokenInput[1]));
        }

        public override string Calculate(string[] inputFile)
        {
            bool gatherRules = true;
            bool getMyTicket = true;
            List<int> myTicketNumbers = new List<int>();
            List<List<int>> nearbyTickets = new List<List<int>>();
            List<Rule> allRules = new List<Rule>();
            for (int i = 0; i < inputFile.Length; i++)
            {
                string rawString = inputFile[i];
                if (gatherRules)
                {
                    if (string.IsNullOrEmpty(rawString))
                    {
                        gatherRules = false;
                        i++;
                        continue;
                    }
                    Rule rule = new Rule();
                    string[] nameAndRules = rawString.Split(':');
                    rule.name = nameAndRules[0];
                    string[] sections = nameAndRules[1].Split(' ');
                    rule.firstRule = GetRange(sections[1]);
                    rule.secondRule = GetRange(sections[3]);
                    allRules.Add(rule);
                }
                else if (getMyTicket)
                {
                    myTicketNumbers = rawString.Split(',').Select(t => int.Parse(t)).ToList();
                    i += 2;
                    getMyTicket = false;
                }
                else
                {
                    nearbyTickets.Add(rawString.Split(',').Select(t => int.Parse(t)).ToList());
                }
            }

            //Merge the segments to do a better search for the first part
            List<(int, int)> finalRanges = new List<(int, int)>();
            List<(int, int)> baseRanges = allRules.SelectMany(t => new List<(int, int)> { t.firstRule, t.secondRule }).ToList();
            baseRanges.Sort((t,u) => t.Item1 - u.Item1);
            int currentMin = baseRanges[0].Item1;
            int currentMax = baseRanges[0].Item2;
            baseRanges.RemoveAt(0);
            foreach ((int,int) range in baseRanges)
            {
                if (range.Item1 > currentMax)
                {
                    finalRanges.Add((currentMin, currentMax));
                    currentMin = range.Item1;
                    currentMax = range.Item2;
                }
                else
                {
                    currentMax = Math.Max(currentMax, range.Item2);
                }
            }
            finalRanges.Add((currentMin, currentMax));

            //Now that we have final ranges, we have a clean evaluation that should be faster than before (although this probably wasn't worth it :P).

            int summation = 0;
            foreach(List<int> ticket in nearbyTickets)
            {
                foreach(int field in ticket)
                {
                    bool isValid = false;
                    foreach ((int, int) rule in finalRanges)
                    {
                        if (field <= rule.Item2 && field >= rule.Item1)
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (!isValid)
                    {
                        summation += field;
                    }
                }
            }

            return summation.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            bool gatherRules = true;
            bool getMyTicket = true;
            List<int> myTicketNumbers = new List<int>();
            List<List<int>> nearbyTickets = new List<List<int>>();
            List<Rule> allRules = new List<Rule>();
            for (int i = 0; i < inputFile.Length; i++)
            {
                string rawString = inputFile[i];
                if (gatherRules)
                {
                    if (string.IsNullOrEmpty(rawString))
                    {
                        gatherRules = false;
                        i++;
                        continue;
                    }
                    Rule rule = new Rule();
                    string[] nameAndRules = rawString.Split(':');
                    rule.name = nameAndRules[0];
                    string[] sections = nameAndRules[1].Split(' ');
                    rule.firstRule = GetRange(sections[1]);
                    rule.secondRule = GetRange(sections[3]);
                    allRules.Add(rule);
                }
                else if (getMyTicket)
                {
                    myTicketNumbers = rawString.Split(',').Select(t => int.Parse(t)).ToList();
                    i += 2;
                    getMyTicket = false;
                }
                else
                {
                    nearbyTickets.Add(rawString.Split(',').Select(t => int.Parse(t)).ToList());
                }
            }

            //Merge the segments to do a better search for the first part
            List<(int, int)> finalRanges = new List<(int, int)>();
            List<(int, int)> baseRanges = allRules.SelectMany(t => new List<(int, int)> { t.firstRule, t.secondRule }).ToList();
            baseRanges.Sort((t, u) => t.Item1 - u.Item1);
            int currentMin = baseRanges[0].Item1;
            int currentMax = baseRanges[0].Item2;
            baseRanges.RemoveAt(0);
            foreach ((int, int) range in baseRanges)
            {
                if (range.Item1 > currentMax)
                {
                    finalRanges.Add((currentMin, currentMax));
                    currentMin = range.Item1;
                    currentMax = range.Item2;
                }
                else
                {
                    currentMax = Math.Max(currentMax, range.Item2);
                }
            }
            finalRanges.Add((currentMin, currentMax));

            //Now that we have final ranges, we have a clean evaluation that should be faster than before (although this probably wasn't worth it :P).

            List<List<int>> validTickets = new List<List<int>>();
            foreach (List<int> ticket in nearbyTickets)
            {
                bool isTicketValid = true;
                foreach (int field in ticket)
                {
                    bool isValid = false;
                    foreach ((int, int) rule in finalRanges)
                    {
                        if (field <= rule.Item2 && field >= rule.Item1)
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (!isValid)
                    {
                        isTicketValid = false;
                    }
                }
                if (isTicketValid)
                {
                    validTickets.Add(ticket);
                }
            }

            //Determine the rules that match each position
            Dictionary<int, List<Rule>> possibleFields = new Dictionary<int, List<Rule>>();
            for(int i = 0; i < myTicketNumbers.Count; i++)
            {
                possibleFields.Add(i, new List<Rule>());
                foreach(Rule rule in allRules)
                {
                    if (IsRuleValidForValue(rule, myTicketNumbers[i]))
                    {
                        possibleFields[i].Add(rule);
                    }
                }
            }

            Dictionary<string, int> knownRules = new Dictionary<string, int>();
            foreach(List<int> ticket in validTickets)
            {
                for (int i = 0; i < ticket.Count; i++)
                {
                    if (knownRules.Values.Contains(i))
                    {
                        continue;
                    }
                    List<Rule> ruleList = new List<Rule>();
                    foreach(Rule rule in possibleFields[i])
                    {
                        if (!knownRules.ContainsKey(rule.name) && IsRuleValidForValue(rule, ticket[i]))
                        {
                            ruleList.Add(rule);
                        }
                    }
                    possibleFields[i] = ruleList;
                    if (ruleList.Count == 1)
                    {
                        knownRules.Add(ruleList[0].name, i);
                    }
                }
            }

            //Clean up the known rules
            while (knownRules.Count < possibleFields.Count)
            {
                foreach(KeyValuePair<int, List<Rule>> kvp in possibleFields) 
                {
                    if (knownRules.Values.Contains(kvp.Key))
                    {
                        continue;
                    }
                    string possibleRule = null;
                    foreach(Rule rule in kvp.Value)
                    {
                        if (!knownRules.ContainsKey(rule.name))
                        {
                            if (possibleRule != null)
                            {
                                possibleRule = null;
                                break;
                            }
                            else
                            {
                                possibleRule = rule.name;
                            }
                        }
                    }
                    if (possibleRule != null)
                    {
                        knownRules.Add(possibleRule, kvp.Key);
                    }
                }
            }

            long product = 1;
            //FINALLY
            foreach (var knownRule in knownRules)
            {
                if (knownRule.Key.StartsWith("departure"))
                {
                    product *= myTicketNumbers[knownRule.Value];
                }
            }


            return product.ToString();
        }

        public bool IsRuleValidForValue(Rule rule, int value)
        {
            return ((rule.firstRule.Item1 <= value && rule.firstRule.Item2 >= value) ||
                        (rule.secondRule.Item1 <= value && rule.secondRule.Item2 >= value));
        }
    }
}
