using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Computer
{
    public class IntComputer
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
            Print(program);
            int pointer = 0;

            // while not looking at the halting operation
            while (program[pointer] != 99)
            {
                List<int> args = GetArguments(pointer, program);
                var opcode = program[pointer];
                Console.Write(opcode + " ");
                Print(args);

                Compute(opcode % 100, args, program);

                pointer += args.Count + 1;
                //Print(program);
            }

            return program;
        }

        // knows how many args each opcode takes in what mode
        List<int> GetArguments(int pointer, List<int> program)
        {
            var args = new List<int>();

            var modes = GetArgModes(program[pointer]);

            var opcode = program[pointer] % 100;

            if(opcode == 1 || opcode == 2) 
            {
                args.Add(GetArg(program[pointer + 1], modes[2]));
                args.Add(GetArg(program[pointer + 2], modes[1]));
                args.Add(GetArg(program[pointer + 3], 1));
            }
            if(opcode == 3 || opcode == 4) 
            {
                args.Add(program[pointer + 1]);
            }

            return args;

            int GetArg(int val, int mode)
            {
                //Console.WriteLine(val);
                if (mode == 0)
                    return program[val];
                if (mode == 1)
                    return val;
                return 0;
            }

            int[] GetArgModes(int number)
            {
                var str = number.ToString();

                if(str.Length < 5)
                {
                    while(str.Length < 5)
                    {
                        str = "0" + str;
                    }
                }
                var num = str.ToCharArray();

                int[] digits = new int[num.Length];
                for (int i = 0; i < digits.Length; i++)
                {
                    digits[i] = int.Parse(num[i].ToString());
                }

                return digits;
            }
        }

        // Knows how modes affect the aquisition of arguments
        int GetArg(int pointer, List<int> program, int mode)
        {
            var arg = -1;

            if(mode == 0)
            {
                var temp = program[pointer];
                arg = program[temp];
            }
            else if (mode == 1)
            {
                arg = program[pointer];
            }

            return arg;
        }

        // Knows what each operation does
        int Compute(int opcode, List<int> args, List<int> program)
        {
            int result = -1;
            if (opcode == 1) // add
            {
                result = args[0] + args[1];
                program[args[2]] = result;
            }
            else if (opcode == 2) // mult
            {
                result = args[0] * args[1];
                program[args[2]] = result;
            }
            else if (opcode == 3) // take input -> store at arg 0
            {
                var input = 1;
                program[args[0]] = input;
            }
            else if (opcode == 4) // output
            {
                Console.WriteLine("OUTPUT: " + program[args[0]]);
            }

            return result;
        }

        public List<int> Parse(string input)
        {
            List<int> program = new List<int>();

            string[] temp = input.Split(',');

            foreach (string s in temp)
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
