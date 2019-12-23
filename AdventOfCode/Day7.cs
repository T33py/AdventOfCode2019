using AdventOfCode.Computer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Day7
    {
        static void Main(string[] args)
        {
            string stuff = File.ReadAllText("day7amp.txt");
            string[] programs = stuff.Split('\n');
            Console.WriteLine(programs[0]);

            var amps = new List<IntComputer> { new IntComputer(), new IntComputer(), new IntComputer(), new IntComputer(), new IntComputer() };
            var util = new Util();
            var phasepermutations = util.FeedbackPhasePermutations();
            var results = new List<int>();

            // task 1 program
            //foreach(List<int> permutaiton in phasepermutations)
            //{
            //    foreach (int i in permutaiton)
            //    {
            //        Console.Write(i + ",");
            //    }
            //    Console.WriteLine();

            //    var p = 0;
            //    var res = 0;
            //    var phases = permutaiton;
            //    var input = new List<int>();

            //    foreach(IntComputer amp in amps)
            //    {
            //        Console.WriteLine("Run " + p);
            //        input.Add(phases[p]);
            //        input.Add(res);

            //        var program = amp.Parse(programs[0]);
            //        amp.Run(program, input);
            //        res = amp.output[0];
            //        Console.WriteLine("result: " + res);

            //        p++;
            //        input = new List<int>();
            //    }

            //    Console.WriteLine(res);
            //    results.Add(res);
            //}


            // task 2 program
            var result = 0;
            void RunFeedback()
            {
                foreach(IntComputer amp in amps)
                {
                    
                    amp.GiveInput(result);
                    amp.Resume();
                    result = amp.output[0];
                }
            }

            foreach (List<int> permutation in phasepermutations)
            {
                var p = 0;
                var phases = permutation;
                var input = new List<int>();
                foreach (IntComputer amp in amps)
                {
                    input.Add(phases[p]);
                    input.Add(result);

                    var program = amp.Parse(programs[0]);
                    amp.Run(program, input);
                    result = amp.output[0];

                    p++;
                    input = new List<int>();
                }

                while (!amps[4].halted)
                {
                    RunFeedback();
                }
                results.Add(amps[4].output[0]);
                
                foreach(IntComputer amp in amps)
                {
                    amp.Reset();
                }
                result = 0;
            }

            var biggest = 0;
            var indexOfBiggest = 0;
            foreach(int res in results)
            {
                if(biggest < res)
                {
                    biggest = res;
                    indexOfBiggest = results.IndexOf(biggest);
                }
            }

            Console.WriteLine("Biggest result: " + biggest);
            Console.Write("from phase configuration: ");
            foreach(int i in phasepermutations[indexOfBiggest])
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
        }
    }

    class Util
    {

        public List<List<int>> PhasePermutations()
        {
            var permutations = new List<List<int>>();

            for(int a  = 0; a < 5; a++)
            {
                for(int b = 0; b < 5; b++)
                {
                    for(int c = 0; c < 5; c++)
                    {
                        for(int d  = 0; d < 5; d++)
                        {
                            var permutation = new List<int>();
                            if(a != b && a != c && a != d && b != c && b != d && c != d)
                            {
                                permutation.Add(a);
                                permutation.Add(b);
                                permutation.Add(c);
                                permutation.Add(d);
                                for(int i = 0; i < 5; i++)
                                {
                                    if (!permutation.Contains(i))
                                        permutation.Add(i);
                                }
                                permutations.Add(permutation);
                            }

                        }
                    }
                }
            }

            return permutations;
        }

        public List<List<int>> FeedbackPhasePermutations()
        {
            var permutations = new List<List<int>>();

            for (int a = 5; a < 10; a++)
            {
                for (int b = 5; b < 10; b++)
                {
                    for (int c = 5; c < 10; c++)
                    {
                        for (int d = 5; d < 10; d++)
                        {
                            var permutation = new List<int>();
                            if (a != b && a != c && a != d && b != c && b != d && c != d)
                            {
                                permutation.Add(a);
                                permutation.Add(b);
                                permutation.Add(c);
                                permutation.Add(d);
                                for (int i = 5; i < 10; i++)
                                {
                                    if (!permutation.Contains(i))
                                        permutation.Add(i);
                                }
                                permutations.Add(permutation);
                            }

                        }
                    }
                }
            }

            return permutations;
        }

    }
}
