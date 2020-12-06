using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day6 : _2020Day<_2020Day6>
    {
        public _2020Day6() : base(6) { }

        public override string Calculate(string[] inputFile)
        {
            HashSet<char> answers = new HashSet<char>();
            int runningTotal = 0;
            for (int index = 0; index < inputFile.Length; index++)
            {
                if (inputFile[index] == string.Empty)
                {
                    runningTotal += answers.Count;
                    answers.Clear();
                }
                else
                {
                    foreach(char response in inputFile[index])
                    {
                        answers.Add(response);
                    }
                }
            }
            runningTotal += answers.Count;
            answers.Clear();
            return runningTotal.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            HashSet<char> groupAnswers = null;
            HashSet<char> answers = new HashSet<char>();
            int runningTotal = 0;
            for (int index = 0; index < inputFile.Length; index++)
            {
                if (inputFile[index] == string.Empty)
                {
                    runningTotal += groupAnswers.Count;
                    answers.Clear();
                    groupAnswers = null;
                }
                else
                {
                    answers.Clear();
                    foreach (char response in inputFile[index])
                    {
                        answers.Add(response);
                    }

                    if (groupAnswers == null)
                    {
                        groupAnswers = new HashSet<char>();
                        groupAnswers.UnionWith(answers);
                    }
                    else
                    {
                        groupAnswers.IntersectWith(answers);
                    }
                }
            }
            runningTotal += groupAnswers.Count;
            return runningTotal.ToString();
        }
    }
}