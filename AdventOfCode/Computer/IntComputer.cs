using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Computer
{
    public class IntComputer
    {
        public List<int> output = new List<int>();
        int pointer = 0;
        List<int> program = new List<int>();
        List<int> inputs = new List<int>();
        public bool waitingForInput = false;
        public bool halted = false;

        public void GiveInput(List<int> input)
        {
            inputs = input;
        }

        public void GiveInput(int input)
        {
            waitingForInput = false;
            inputs.Add(input);
        }

        public List<int> Resume()
        {
            output = new List<int>();
            //Console.WriteLine("resumed at " + pointer);
            return Compute(program, inputs);
        }

        public void Reset()
        {
            pointer = 0;
            program = new List<int>();
            waitingForInput = false;
            halted = false;
        }

        public List<int> Run(string input)
        {
            List<int> program = Parse(input);
            this.program = program;

            return Compute(program, new List<int>());
        }

        public List<int> Run(List<int> program)
        {
            this.program = program;
            return Compute(program, new List<int>());
        }

        public List<int> Run(List<int> program, List<int> inputs)
        {
            this.program = program;
            return Compute(program, inputs);
        }

        List<int> Compute(List<int> program, List<int> inputs)
        {
            output = new List<int>();
            //Print(program);
            
            // while not looking at the halting operation
            while (program[pointer] != 99)
            {
                List<int> args = GetArguments(pointer, program);
                var opcode = program[pointer];

                //Console.Write(opcode + " ");
                //Print(args);

                var _goto = Compute(pointer, args, program, inputs);

                //Print(program);

                pointer = _goto;
                //Print(program);

                if (waitingForInput)
                {
                    return program;
                }
            }

            Console.WriteLine("Halt");
            halted = true;

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
            else if(opcode == 3) 
            {
                args.Add(GetArg(program[pointer + 1], 1));
            }
            else if (opcode == 4)
            {
                args.Add(GetArg(program[pointer + 1], modes[2]));
            }
            else if (opcode == 5 || opcode == 6)
            {
                args.Add(GetArg(program[pointer + 1], modes[2]));
                args.Add(GetArg(program[pointer + 2], modes[1]));
            }
            else if (opcode == 7 || opcode == 8)
            {
                args.Add(GetArg(program[pointer + 1], modes[2]));
                args.Add(GetArg(program[pointer + 2], modes[1]));
                args.Add(GetArg(program[pointer + 3], 1));
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

        // Knows what each operation does
        int Compute(int pointer, List<int> args, List<int> program, List<int> inputs)
        {
            int opcode = program[pointer] % 100;
            int result = 0;

            if (opcode == 1) // add
            {
                result = args[0] + args[1];
                program[args[2]] = result;
                pointer += 4;
            }
            else if (opcode == 2) // mult
            {
                result = args[0] * args[1];
                program[args[2]] = result;
                pointer += 4;
            }
            else if (opcode == 3) // take input -> store at arg 0
            {
                if (inputs.Count > 0) { 
                    var input = inputs[0];
                    inputs.RemoveAt(0);
                    //Console.WriteLine("INPUT! " + input + " stored at " + args[0]);
                    //Console.WriteLine("Place: " + input + " at " + args[0]);
                    program[args[0]] = input;
                    //Console.WriteLine("done");
                    //Console.WriteLine("got input at: " + pointer);
                    pointer += 2;
                }
                else
                {
                    waitingForInput = true;
                    //Console.WriteLine("waiting for input at: " + pointer);
                }

            }
            else if (opcode == 4) // output
            {
                //Print(args);
                output.Add(args[0]);
                //Console.WriteLine("OUTPUT: " + args[0]);
                pointer += 2;
            }
            else if (opcode == 5) // jmp true
            {
                if (args[0] != 0)
                {
                    pointer = args[1];
                }
                else
                {
                    pointer += 3;
                }
            }
            else if (opcode == 6) // jmp false
            {
                if (args[0] == 0)
                {
                    pointer = args[1];
                }
                else
                {
                    pointer += 3;
                }
            }
            else if (opcode == 7) // lt
            {
                if (args[0] < args[1])
                {
                    program[args[2]] = 1;
                }
                else
                {
                    program[args[2]] = 0;
                }
                pointer += 4;
            }
            else if (opcode == 8) // eq
            {
                //Console.WriteLine(args[0] + " == " + args[1] + " => " + args[2]);
                if (args[0] == args[1])
                {
                    program[args[2]] = 1;
                }
                else
                {
                    program[args[2]] = 0;
                }
                pointer += 4;
            }

            return pointer;
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
