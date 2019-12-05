using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AdventOfCode.Computer;

namespace AdventOfCode
{
    class Day5
    {
        static void Main(string[] args)
        {
            IntComputer comp = new IntComputer();
            string input = File.ReadAllText("IntComputerAircondition1.txt");
            //string input = File.ReadAllText("IntComputerTests.txt");
            string[] programs = input.Split('\n');

            foreach (string p in programs)
            {
                comp.Print(comp.Parse(p));
                comp.Run(p);
            }


            //foreach (string p in programs)
            //{
            //    Console.WriteLine(p);


            //    for (int i = 0; i < 100; i++)
            //    {
            //        for (int j = 0; j < 100; j++)
            //        {
            //            try
            //            {
            //                var program = comp.Parse(p);
            //                program[1] = i;
            //                program[2] = j;
            //                var result = comp.Run(program);
            //                Console.WriteLine(i + " " + j + " " + result[0]);

            //                if (result[0] == 19690720)
            //                {
            //                    comp.Print(program);
            //                    return;
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                // invalid input
            //            }

            //        }
            //    }


            //    Console.WriteLine();
            //}

            Console.ReadLine();
        }
    }
}
