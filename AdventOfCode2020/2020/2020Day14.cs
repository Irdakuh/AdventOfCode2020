using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day14 : _2020Day<_2020Day14>
    {
        public _2020Day14() : base(14) { }

        public bool?[] GetBitMask(string bitMask)
        {
            return bitMask.Select(t => t == '1' ? true : t == '0' ? (bool?)false : null).ToArray();
        }

        public bool[] Get36BitBinary(long number)
        {
            bool[] binary = new bool[36];
            int bit = 35;
            while (number > 0)
            {
                binary[bit--] = number % 2 == 1;
                number /= 2;
            }
            return binary;
        }

        public long GetLongFrom36BitBinary(bool[] binary)
        {
            long result = 0;
            foreach(bool bit in binary)
            {
                result *= 2;
                if (bit)
                {
                    result += 1;
                }
            }
            return result;
        }

        public bool[] ApplyBitMask(bool?[] bitMask, bool[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (bitMask[i].HasValue)
                {
                    value[i] = bitMask[i].Value;
                }
            }
            return value;
        }

        public override string Calculate(string[] inputFile)
        {
            bool?[] bitMask = new bool?[36];
            Dictionary<int, bool[]> memory = new Dictionary<int, bool[]>();
            foreach (string inputString in inputFile)
            {
                if (inputString.StartsWith("mask = "))
                {
                    bitMask = GetBitMask(inputString.Substring(7));
                }
                else
                {
                    int memStart = inputString.IndexOf('[') + 1;
                    int memEnd = inputString.IndexOf(']');
                    string address = inputString.Substring(memStart, memEnd - memStart);
                    int memAddress = int.Parse(address);
                    string valueString = inputString.Substring(memEnd + 4);
                    long value = long.Parse(valueString);
                    if (!memory.ContainsKey(memAddress))
                    {
                        memory.Add(memAddress, new bool[36]);
                    }

                    memory[memAddress] = ApplyBitMask(bitMask, Get36BitBinary(value));
                }
            }

            long total = 0;
            foreach(bool[] value in memory.Values)
            {
                total += GetLongFrom36BitBinary(value);
            }
            return total.ToString();
        }

        public bool?[] ApplyBitMaskFloating(bool?[] bitMask, bool[] value)
        {
            bool?[] result = new bool?[36];
            for (int i = 0; i < value.Length; i++)
            {
                if (bitMask[i].HasValue)
                {
                    result[i] = bitMask[i].Value ? bitMask[i].Value : value[i];
                }
                else
                {
                    result[i] = null;
                }
            }
            return result;
        }

        public List<long> GetAllMemoryAddresses(bool?[] bitMask, long value)
        {
            List<long> memoryAddresses = new List<long>();
            bool[] memoryBits = Get36BitBinary(value);
            bool?[] floatingMemory = ApplyBitMaskFloating(bitMask, memoryBits);

            Queue<bool?[]> addressesToProcess = new Queue<bool?[]>();
            addressesToProcess.Enqueue(floatingMemory);

            while(addressesToProcess.Count > 0)
            {
                bool isComplete = true;
                bool?[] currentMemoryAddress = addressesToProcess.Dequeue();
                bool[] finalCopy = new bool[36];
                for(int i = 0; i < currentMemoryAddress.Length; i++)
                {
                    if (!currentMemoryAddress[i].HasValue)
                    {
                        bool?[] copy = new bool?[36];
                        currentMemoryAddress.CopyTo(copy, 0);
                        copy[i] = true;
                        addressesToProcess.Enqueue(copy);
                        copy = new bool?[36];
                        currentMemoryAddress.CopyTo(copy, 0);
                        copy[i] = false;
                        addressesToProcess.Enqueue(copy);
                        isComplete = false;
                        break;
                    }
                    else
                    {
                        finalCopy[i] = currentMemoryAddress[i].Value;
                    }
                }
                if (isComplete)
                {
                    memoryAddresses.Add(GetLongFrom36BitBinary(finalCopy));
                }
            }

            return memoryAddresses;
        }


        public override string CalculateV2(string[] inputFile)
        {
            bool?[] bitMask = new bool?[36];
            Dictionary<long, long> memory = new Dictionary<long, long>();
            foreach (string inputString in inputFile)
            {
                if (inputString.StartsWith("mask = "))
                {
                    bitMask = GetBitMask(inputString.Substring(7));
                }
                else
                {
                    int memStart = inputString.IndexOf('[') + 1;
                    int memEnd = inputString.IndexOf(']');
                    string address = inputString.Substring(memStart, memEnd - memStart);
                    int memAddress = int.Parse(address);
                    string valueString = inputString.Substring(memEnd + 4);
                    long value = long.Parse(valueString);

                    foreach (long memoryAddress in GetAllMemoryAddresses(bitMask, memAddress))
                    {
                        memory[memoryAddress] = value;
                    }
                }
            }

            long total = 0;
            foreach (long value in memory.Values)
            {
                total += value;
            }
            return total.ToString();
        }
    }
}