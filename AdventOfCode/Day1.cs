using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    class Day1
    {
        static void Main(string[] args)
        {
            FuelCostSolver solver = new FuelCostSolver();
            string input = File.ReadAllText("RocketModuleWeights.txt");
            //string input = File.ReadAllText("RocketModuleWeightTests.txt");
            string[] weightStrings = input.Split('\n');
            List<int> weights = new List<int>();

            foreach(string s in weightStrings)
            {
                weights.Add(int.Parse(s));
            }

            List<int> fuelCosts = new List<int>();

            foreach(int i in weights)
            {
                int fuel = solver.TotalFuel(i);

                fuelCosts.Add(fuel);
                Console.WriteLine(fuel);
            }

            var fuelTotal = 0;

            foreach(int f in fuelCosts)
            {
                fuelTotal += f;
            }

            Console.WriteLine("Total: " + fuelTotal);

            Console.ReadLine();
        }
    }

    class FuelCostSolver
    {
        public int TotalFuel(int weight)
        {
            //Console.WriteLine("weight: " + weight);
            int fuel = 0;

            Double temp = weight;

            fuel = ((int)Math.Floor((temp / 3))) - 2;

            if(fuel >= 0)
            {
                fuel += TotalFuel(fuel);
            }
            else
            {
                fuel = 0;
            }

            //Console.WriteLine("fuel: " + fuel);
            return fuel;
        }
    }
}
