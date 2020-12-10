using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day8 : _2020Day<_2020Day8>
    {
        public _2020Day8() : base(8) { }

        public int accumulator = 0;
        public int programCounter = 0;

        public struct Instruction
        {
            public string operation;
            public int value;
            public readonly int programCounter;

            public Instruction(string operation, int value, int programCounter)
            {
                this.operation = operation;
                this.value = value;
                this.programCounter = programCounter;
            }
        }

        public void HandleOperation(Instruction instruction)
        {
            switch (instruction.operation)
            {
                case "acc":
                    accumulator += instruction.value;
                    programCounter++;
                    break;
                case "jmp":
                    programCounter += instruction.value;
                    break;
                case "nop":
                default:
                    programCounter++;
                    break;
            }
        }

        public HashSet<Instruction> RunProgramAndHalt(List<Instruction> program)
        {
            programCounter = 0;
            accumulator = 0;
            HashSet<Instruction> runInstructions = new HashSet<Instruction>();
            Instruction currentInstruction = program[programCounter];
            while(!runInstructions.Contains(currentInstruction)) 
            {
                HandleOperation(currentInstruction);
                runInstructions.Add(currentInstruction);
                if (programCounter >= program.Count)
                {
                    break;
                }
                currentInstruction = program[programCounter];
            }
            return runInstructions;
        }

        public override string Calculate(string[] inputFile)
        {
            List<Instruction> instructions = new List<Instruction>();
            int instructionCounter = 0;
            foreach(string instruction in inputFile)
            {
                string[] split = instruction.Split(' ');
                string operation = split[0];
                int value = int.Parse(split[1]);
                instructions.Add(new Instruction(operation, value, instructionCounter++));
            }
            RunProgramAndHalt(instructions);

            return accumulator.ToString();
        }

        public override string CalculateV2(string[] inputFile)
        {
            List<Instruction> instructions = new List<Instruction>();
            int instructionCounter = 0;
            foreach (string instruction in inputFile)
            {
                string[] split = instruction.Split(' ');
                string operation = split[0];
                int value = int.Parse(split[1]);
                instructions.Add(new Instruction(operation, value, instructionCounter++));
            }

            Dictionary<int, List<Instruction>> commandThatReaches = new Dictionary<int, List<Instruction>>();
            Dictionary<int, List<Instruction>> swappedCommandThatReaches = new Dictionary<int, List<Instruction>>();
            foreach(Instruction instruction in instructions)
            {
                programCounter = instruction.programCounter;
                HandleOperation(instruction);
                if (!commandThatReaches.ContainsKey(programCounter))
                {
                    commandThatReaches.Add(programCounter, new List<Instruction>());
                }
                commandThatReaches[programCounter].Add(instruction);
            }
            foreach(Instruction instruction in instructions)
            {
                Instruction currentInstruction = instruction;
                switch(currentInstruction.operation)
                {
                    case "acc":
                        continue;
                    case "jmp":
                        currentInstruction.operation = "nop";
                        break;
                    case "nop":
                        currentInstruction.operation = "jmp";
                        break;

                }

                programCounter = currentInstruction.programCounter;
                HandleOperation(currentInstruction);
                if (!swappedCommandThatReaches.ContainsKey(programCounter))
                {
                    swappedCommandThatReaches.Add(programCounter, new List<Instruction>());
                }
                swappedCommandThatReaches[programCounter].Add(instruction);
            }

            //Get all instructions we hit in the forward walk
            HashSet<Instruction> forwardWalk = RunProgramAndHalt(instructions);
            Queue<Instruction> instructionsToCheck = new Queue<Instruction>();
            instructionsToCheck.Enqueue(instructions[instructions.Count - 1]);
            while(instructionsToCheck.Count > 0)
            {
                Instruction currentInstruction = instructionsToCheck.Dequeue();
                if (swappedCommandThatReaches.TryGetValue(currentInstruction.programCounter, out var instructionSet))
                {
                    foreach(Instruction testInstruction in instructionSet)
                    {
                        if (forwardWalk.Contains(testInstruction))
                        {
                            //We found it!
                            Instruction modifiedInstruction = testInstruction;
                            modifiedInstruction.operation = modifiedInstruction.operation == "nop" ? "jmp" : "nop";
                            instructions[testInstruction.programCounter] = modifiedInstruction;
                            RunProgramAndHalt(instructions);
                            return accumulator.ToString();
                        }
                    }
                 }
                if (commandThatReaches.TryGetValue(currentInstruction.programCounter, out var commandsThatReach))
                {
                    foreach (Instruction nextInstruction in commandsThatReach)
                    {
                        instructionsToCheck.Enqueue(nextInstruction);
                    }
                }
            }

            return "Could not solve";
        }
    }
}
