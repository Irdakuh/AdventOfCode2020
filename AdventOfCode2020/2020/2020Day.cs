using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public abstract class _2020Day<T> where T: _2020Day<T>, new()
    {
        public static T Instance { get; } = new T();

        protected int day;
        public string[] Input => GetInput(day, false);
        public string[] TestInput => GetInput(day, true);
        protected string[] GetInput(int day, bool test = false)
        {
            return System.IO.File.ReadAllLines($"C:\\Users\\gejohnst\\source\\repos\\AdventOfCode2020\\AdventOfCode2020\\2020\\Day{day}Input{(test ? "Test" : "")}.txt");
        }

        public _2020Day(int day)
        {
            this.day = day;
        }

        public abstract string Calculate(string[] inputFile);
        public abstract string CalculateV2(string[] inputFile);
        public void Run()
        {
            Console.WriteLine($"Running Day {day} Test Input:");
            Console.WriteLine(Calculate(TestInput));
            Console.WriteLine($"Running Day {day} Input:");
            Console.WriteLine(Calculate(Input));

            Console.WriteLine($"Running Day {day} Test Input:");
            Console.WriteLine(CalculateV2(TestInput));
            Console.WriteLine($"Running Day {day} Input:");
            Console.WriteLine(CalculateV2(Input));
        }
    }
}
