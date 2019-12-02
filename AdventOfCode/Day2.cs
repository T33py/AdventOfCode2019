using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Day2
    {
        static void Main(string[] args)
        {
            IntcodeComputer comp = new IntcodeComputer();
            string input = File.ReadAllText("IntCode.txt");
            //string input = File.ReadAllText("IntCodeTests.txt");
            string[] programs = input.Split('\n');

            foreach(string p in programs)
            {
                Console.WriteLine(p);


                for(int i = 0; i<100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        try
                        {
                            var program = comp.Parse(p);
                            program[1] = i;
                            program[2] = j;
                            var result = comp.Run(program);
                            Console.WriteLine(i + " " + j + " " + result[0]);

                            if (result[0] == 19690720)
                            {
                                comp.Print(program);
                                return;
                            }
                        }
                        catch (Exception e)
                        {
                            // invalid input
                        }
                    
                    }
                }


                Console.WriteLine();
            }

            Console.ReadLine();
        }

        
    }

    class IntcodeComputer
    {
        public List<int> Run(string input)
        {
            List<int> program = Parse(input);

            return Compute(program);
        }

        public List<int> Run(List<int> input)
        {
            return Compute(input);
        }

        List<int> Compute(List<int> program)
        {
            int pointer = 0;

            // while not looking at the halting operation
            while (program[pointer] != 99)
            {
                var opcode = program[pointer];
                var a = program[pointer + 1];
                var b = program[pointer + 2];
                var storeTo = program[pointer + 3];

                var result = compute(opcode, program[a], program[b]);

                program[storeTo] = result;

                pointer = pointer + 4;
                //Print(program);
            }

            return program;
        }

        int compute(int opcode, int var1, int var2)
        {
            int result = -1;
            if(opcode == 1)
            {
                result = var1 + var2;
            }
            else if(opcode == 2)
            {
                result = var1 * var2;
            }

            return result;
        }

        public List<int> Parse(string input)
        {
            List<int> program = new List<int>();

            string[] temp = input.Split(',');

            foreach(string s in temp)
            {
                program.Add(int.Parse(s));
            }

            return program;
        }

        public void Print(List<int> program)
        {
            Console.Write("{ ");
            for (int i = 0; i < program.Count - 1; i++)
            {
                Console.Write(program[i] + ", ");
            }
            Console.Write(program[program.Count - 1]);
            Console.WriteLine(" }");
        }
    }
}
