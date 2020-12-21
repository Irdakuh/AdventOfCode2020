using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day21 : _2020Day<_2020Day21>
    {
        public _2020Day21() : base(21) { }

        public override string Calculate(string[] inputFile)
        {
            Dictionary<string, int> ingredientCounts = new Dictionary<string, int>();
            Dictionary<string, List<string>> allergenPossibilities = new Dictionary<string, List<string>>();
            foreach (string recipe in inputFile)
            {
                string[] separateAllergens = recipe.Split('(');
                List<string> food = separateAllergens[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach(string ingredient in food)
                {
                    if (!ingredientCounts.ContainsKey(ingredient))
                    {
                        ingredientCounts.Add(ingredient, 0);
                    }
                    ingredientCounts[ingredient] += 1;
                }
                List<string> allergens = separateAllergens[1].Split(new char[] { ' ', ')', ','}, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                foreach(string allergen in allergens)
                {
                    List<string> possibility;
                    if (allergenPossibilities.TryGetValue(allergen, out possibility))
                    {
                        allergenPossibilities[allergen] = possibility.Intersect(food).ToList();
                    }
                    else
                    {
                        allergenPossibilities.Add(allergen, food);
                    }
                }
            }

            List<string> badIngredients = new List<string>();
            foreach(var kvp in allergenPossibilities)
            {
                badIngredients = badIngredients.Union(kvp.Value).ToList();
            }

            int count = 0;
            foreach(var ingredientKvp in ingredientCounts)
            {
                if (!badIngredients.Contains(ingredientKvp.Key))
                {
                    count += ingredientKvp.Value;
                }
            }
            return count.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            Dictionary<string, int> ingredientCounts = new Dictionary<string, int>();
            Dictionary<string, List<string>> allergenPossibilities = new Dictionary<string, List<string>>();
            foreach (string recipe in inputFile)
            {
                string[] separateAllergens = recipe.Split('(');
                List<string> food = separateAllergens[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string ingredient in food)
                {
                    if (!ingredientCounts.ContainsKey(ingredient))
                    {
                        ingredientCounts.Add(ingredient, 0);
                    }
                    ingredientCounts[ingredient] += 1;
                }
                List<string> allergens = separateAllergens[1].Split(new char[] { ' ', ')', ',' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                foreach (string allergen in allergens)
                {
                    List<string> possibility;
                    if (allergenPossibilities.TryGetValue(allergen, out possibility))
                    {
                        allergenPossibilities[allergen] = possibility.Intersect(food).ToList();
                    }
                    else
                    {
                        allergenPossibilities.Add(allergen, food);
                    }
                }
            }

            Dictionary<string, string> knownAllergenList = new Dictionary<string, string>();
            while (allergenPossibilities.Count > 0)
            {
                KeyValuePair<string, List<string>> knownAllergen = allergenPossibilities.Where(t => t.Value.Count == 1).First();
                knownAllergenList.Add(knownAllergen.Key, knownAllergen.Value[0]);
                allergenPossibilities.Remove(knownAllergen.Key);
                foreach(var kvp in allergenPossibilities)
                {
                    allergenPossibilities[kvp.Key].Remove(knownAllergen.Value[0]);
                }
            }

            List<string> sortedKeys = knownAllergenList.Keys.ToList();
            sortedKeys.Sort();
            string finalString = string.Empty;
            foreach(string key in sortedKeys)
            {
                finalString += $",{knownAllergenList[key]}";
            }

            return finalString;
        }
    }
}
