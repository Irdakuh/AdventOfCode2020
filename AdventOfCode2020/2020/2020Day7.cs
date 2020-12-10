using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day7 : _2020Day<_2020Day7>
    {
        public _2020Day7() : base(7) { }

        public struct Rule
        {
            public string holdingBag;
            public Dictionary<string, int> containedBags;
        }

        public List<Rule> GetRuleList(string[] inputFile)
        {
            List<Rule> ruleList = new List<Rule>();
            foreach (string rule in inputFile)
            {
                var words = rule.Split(' ');
                string holdingBag = $"{words[0]} {words[1]}";
                int pointer = 4;
                Dictionary<string, int> bagCounts = new Dictionary<string, int>();
                while (pointer < words.Length)
                {
                    if (words[pointer] == "no")
                    {
                        //This bag is always empty, no rule for it
                        break;
                    }
                    else
                    {
                        int count = int.Parse(words[pointer++]);
                        string containedBag = $"{words[pointer++]} {words[pointer++]}";
                        bagCounts.Add(containedBag, count);
                        pointer++;
                    }
                }
                ruleList.Add(new Rule { holdingBag = holdingBag, containedBags = bagCounts });
            }
            return ruleList;
        }

        public override string Calculate(string[] inputFile)
        {
            var rules = GetRuleList(inputFile);
            HashSet<string> bagCanBeHeldBy = new HashSet<string>();
            List<string> checkedBags = new List<string>();
            Queue<string> bagsToTest = new Queue<string>();
            bagsToTest.Enqueue("shiny gold");
            while (bagsToTest.Count > 0)
            {
                string currentBag = bagsToTest.Dequeue();
                foreach (Rule rule in rules)
                {
                    if (rule.containedBags.ContainsKey(currentBag) && !checkedBags.Contains(rule.holdingBag))
                    {
                        bagCanBeHeldBy.Add(rule.holdingBag);
                        bagsToTest.Enqueue(rule.holdingBag);
                    }
                }
                checkedBags.Add(currentBag);
            }
            
            return bagCanBeHeldBy.Count.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            var rules = GetRuleList(inputFile);
            Dictionary<string, int> bagChildrenCounts = new Dictionary<string, int>();

            while (rules.Count > 0)
            {
                List<Rule> rulesToRemove = new List<Rule>();
                foreach (Rule rule in rules)
                {
                    bool canEvaluate = true;
                    foreach (string bag in rule.containedBags.Keys)
                    {
                        canEvaluate &= bagChildrenCounts.ContainsKey(bag);
                    }

                    if (canEvaluate)
                    {
                        int count = 1;
                        foreach (string bag in rule.containedBags.Keys)
                        {
                            count += rule.containedBags[bag] * (bagChildrenCounts[bag]); //A bag counts itself + its children
                        }
                        bagChildrenCounts.Add(rule.holdingBag, count);
                        rulesToRemove.Add(rule);
                    }
                }
                foreach(Rule rule in rulesToRemove)
                {
                    rules.Remove(rule);
                }
            }

            return (bagChildrenCounts["shiny gold"] - 1).ToString();
        }
    }
}
