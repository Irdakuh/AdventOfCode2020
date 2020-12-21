using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day18 : _2020Day<_2020Day18>
    {
        public _2020Day18() : base(18) { }

        public long Evaluate(string input)
        {
            //Find first parenthetical, and evaluate that
            int parenStart = -1;
            int parenEnd = -1;
            int unclosedOpeningParens = 0;
            for(int walk = 0; walk < input.Length; walk++)
            {
                if (input[walk] == '(')
                {
                    if (parenStart == -1)
                    {
                        //Found the first paren
                        parenStart = walk;
                    }
                    unclosedOpeningParens++;
                }
                if (input[walk] == ')')
                {
                    if (unclosedOpeningParens == 1)
                    {
                        //Found the end of the first paren.  Replace it with the answer
                        parenEnd = walk - 1;
                        string substring = input.Substring(parenStart + 1, parenEnd - parenStart);
                        return Evaluate(input.Replace($"({substring})", Evaluate(substring).ToString()));
                    }
                    else
                    {
                        unclosedOpeningParens--;
                    }
                }
            }

            //If we made it here, there are no parens anymore.  Evaluate left to right
            int firstIndex = input.LastIndexOfAny("+*".ToCharArray());
            if (firstIndex == -1)
            {
                return long.Parse(input);
            }
            string lhs = input.Substring(0, firstIndex);
            string rhs = input.Substring(firstIndex + 1);
            long lhsValue = Evaluate(lhs);
            long rhsValue = Evaluate(rhs);
            if (input[firstIndex] == '+')
            {
                return lhsValue + rhsValue;
            }
            else 
            {
                return lhsValue * rhsValue;
            }
        }

        public long EvaluateV2(string input)
        {
            //Find first parenthetical, and evaluate that
            int parenStart = -1;
            int parenEnd = -1;
            int unclosedOpeningParens = 0;
            for (int walk = 0; walk < input.Length; walk++)
            {
                if (input[walk] == '(')
                {
                    if (parenStart == -1)
                    {
                        //Found the first paren
                        parenStart = walk;
                    }
                    unclosedOpeningParens++;
                }
                if (input[walk] == ')')
                {
                    if (unclosedOpeningParens == 1)
                    {
                        //Found the end of the first paren.  Replace it with the answer
                        parenEnd = walk - 1;
                        string substring = input.Substring(parenStart + 1, parenEnd - parenStart);
                        return EvaluateV2(input.Replace($"({substring})", EvaluateV2(substring).ToString()));
                    }
                    else
                    {
                        unclosedOpeningParens--;
                    }
                }
            }

            //If we made it here, there are no parens anymore.  Evaluate left to right
            int firstIndex = input.LastIndexOf('*');
            if (firstIndex != -1)
            {
                string lhs = input.Substring(0, firstIndex);
                string rhs = input.Substring(firstIndex + 1);
                long lhsValue = EvaluateV2(lhs);
                long rhsValue = EvaluateV2(rhs);
                return lhsValue * rhsValue;
            }

            firstIndex = input.LastIndexOf('+');
            if (firstIndex != -1)
            {
                string lhs = input.Substring(0, firstIndex);
                string rhs = input.Substring(firstIndex + 1);
                long lhsValue = EvaluateV2(lhs);
                long rhsValue = EvaluateV2(rhs);
                return lhsValue + rhsValue;
            }

            return long.Parse(input);
        }

        public override string Calculate(string[] inputFile)
        {
            long sum = 0;
            foreach(string line in inputFile)
            {
                Console.Write($"{line}: ");
                long value = Evaluate(line);
                Console.WriteLine(value);
                sum += value;
            }
            return sum.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            long sum = 0;
            foreach (string line in inputFile)
            {
                Console.Write($"{line}: ");
                long value = EvaluateV2(line);
                Console.WriteLine(value);
                sum += value;
            }
            return sum.ToString();
        }
    }
}
