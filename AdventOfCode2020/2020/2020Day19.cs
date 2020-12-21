using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day19 : _2020Day<_2020Day19>
    {
        public _2020Day19() : base(19) { }

        public struct Construction
        {
            public List<int> rules;
            public char letter;
        }

        public struct Rule
        {
            public int ruleNumber;
            public List<Construction> possibilites;
        }

        public bool Evaluate(string input, Dictionary<int, Rule> rules, List<int> currentRuleList)
        {
            if (currentRuleList.Count > input.Length)
            {
                return false;
            }
            if (currentRuleList.Count == 0)
            {
                return input.Length == 0;
            }

            Rule currentRule = rules[currentRuleList[0]];
            string newInput = input;
            foreach (Construction construct in currentRule.possibilites)
            {
                List<int> passThroughRuleList = new List<int>(currentRuleList.Skip(1));
                //Branch here
                if (construct.rules.Count == 0)
                {
                    if (input[0] == construct.letter)
                    {
                        newInput = newInput.Substring(1);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    passThroughRuleList.InsertRange(0, construct.rules);
                }

                if (Evaluate(newInput, rules, passThroughRuleList))
                {
                    return true;
                }
            }
            return false;
        }

        public Rule ProcessRule(string input)
        {
            Rule rule = new Rule();
            rule.possibilites = new List<Construction>();
            string[] getNumber = input.Split(':');
            rule.ruleNumber = int.Parse(getNumber[0]);
            string evaluations = getNumber[1];
            foreach(string subRule in evaluations.Split('|'))
            {
                Construction construct = new Construction();
                construct.rules = new List<int>();
                string[] components = subRule.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach(string component in components)
                {
                    if (component.Contains('"'))
                    {
                        construct.letter = component[1];
                    }
                    else
                    {
                        construct.rules.Add(int.Parse(component));
                    }
                }
                rule.possibilites.Add(construct);
            }
            return rule;
        }

        public override string Calculate(string[] inputFile)
        {
            Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
            bool readingRules = true;
            int count = 0;
            foreach(string line in inputFile)
            {
                if (readingRules)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        readingRules = false;
                    }
                    else
                    {
                        Rule newRule = ProcessRule(line);
                        rules.Add(newRule.ruleNumber, newRule);
                    }
                }
                else
                {
                    List<int> startingRules = new List<int> { 0 };
                    bool valid = Evaluate(line, rules, startingRules);
                    Console.WriteLine($"{line}: {valid}");
                    if (valid) count++;
                }
            }
            return count.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            return Calculate(inputFile);
        }
    }
}
