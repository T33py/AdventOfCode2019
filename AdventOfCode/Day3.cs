using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode
{
    class Day3
    {
        static void Main(string[] args)
        {
            Measurerer measurerer = new Measurerer();
            string input = File.ReadAllText("WireIntersections.txt");
            //string input = File.ReadAllText("WireIntersectionTests.txt");
            string[] wires = input.Split('\n');

            for (int i = 0; i < wires.Length; i = i + 2)
            {
                Console.WriteLine("dist: " + measurerer.FindFewestStepsIntersection(wires[i], wires[i + 1]));
            }
        }
    }

    class Measurerer
    {
        /// <summary>
        /// Part 2
        /// </summary>
        /// <param name="wire1"></param>
        /// <param name="wire2"></param>
        /// <returns></returns>
        public int FindFewestStepsIntersection(string wire1, string wire2)
        {
            Console.WriteLine(wire1);
            Console.WriteLine(wire2);

            int dist = 0;

            var w1 = ParseWire(wire1);
            var w2 = ParseWire(wire2);

            List<Coordinate> intersections = new List<Coordinate>();

            WireMap map = new WireMap();
            Console.WriteLine("wire2 length: " + w2.Count);
            foreach (Coordinate c1 in w1)
            {
                map.Add(c1);
            }
            foreach (Coordinate c2 in w2)
            {
                if (map.Find(c2))
                {
                    intersections.Add(c2);
                    Console.WriteLine("  " + c2.ToString() + " at " + w2.IndexOf(c2));
                }
            }

            foreach (Coordinate c in intersections)
            {
                var fewestSteps = c.steps + map.Get(c).steps;
                if (fewestSteps < dist || dist == 0) // take smallest dist !=0
                {
                    dist = fewestSteps;
                }
            }

            return dist;
        }

        /// <summary>
        /// Part 1
        /// </summary>
        public int FindClosestIntersection(string wire1, string wire2)
        {
            Console.WriteLine(wire1);
            Console.WriteLine(wire2);

            int dist = 0;

            var w1 = ParseWire(wire1);
            var w2 = ParseWire(wire2);

            List<Coordinate> intersections = new List<Coordinate>();

            WireMap map = new WireMap();
            Console.WriteLine("wire2 length: " + w2.Count);
            foreach (Coordinate c1 in w1)
            {
                map.Add(c1);
            }
            foreach (Coordinate c2 in w2)
            {
                if (map.Find(c2))
                {
                    intersections.Add(c2);
                    Console.WriteLine("  " + c2.ToString() + " at " + w2.IndexOf(c2));
                }
            }

            foreach(Coordinate c in intersections)
            {
                var manhattenDistance = Math.Abs(c.x) + Math.Abs(c.y);
                if (manhattenDistance < dist || dist == 0) // take smallest dist !=0
                {
                    dist = manhattenDistance;
                }
            }

            return dist;
        }

        List<Coordinate> ParseWire(string wire)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            var moves = wire.Split(',');
            int x = 0;
            int y = 0;
            int step = 0;

            foreach(string move in moves)
            {
                var direction = move[0];
                var distance = int.Parse(move.Substring(1, move.Length - 1));

                Console.WriteLine("Direction: " + direction + ", distance: " + distance);

                for (int i = 0; i < distance; i++) // move
                {
                    if (direction.Equals('R')) // going right
                    {
                        x++;
                    }
                    else if (direction.Equals('L')) // going left
                    {
                        x--;
                    }
                    else if (direction.Equals('U')) // going up
                    {
                        y++;
                    }
                    else if (direction.Equals('D')) // going down
                    {
                        y--;
                    }

                    step++;
                    
                    var c = new Coordinate(x, y, step);
                    //Console.WriteLine(c.ToString());
                    coordinates.Add(c);
                }
            }

            return coordinates;
        }

    }

    class WireMap
    {
        List<int> xs = new List<int>();
        List<List<int>> ys = new List<List<int>>();
        List<List<Coordinate>> coordinates = new List<List<Coordinate>>();

        public bool Find(Coordinate coordinate)
        {
            if (xs.Contains(coordinate.x))
            {
                var index = xs.IndexOf(coordinate.x);
                if (ys[index].Contains(coordinate.y))
                {
                    return true;
                }
            }
            return false;
        }

        public Coordinate Get(Coordinate coordinate)
        {
            if (Find(coordinate)) 
            {
                var x = xs.IndexOf(coordinate.x);
                var y = ys[x].IndexOf(coordinate.y);
                return coordinates[x][y];
            }
            return null;
        }

        public void Add(Coordinate coordinate)
        {
            if (!xs.Contains(coordinate.x))
            {
                xs.Add(coordinate.x);
                var y = new List<int> { coordinate.y };
                ys.Add(y);

                var c = new List<Coordinate> { coordinate };
                coordinates.Add(c);
            }
            else
            {
                var index = xs.IndexOf(coordinate.x);
                if (!ys[index].Contains(coordinate.y))
                {
                    ys[index].Add(coordinate.y);
                    coordinates[index].Add(coordinate);
                }
            }
        }
    }

    class Coordinate
    {
        public int x;
        public int y;
        public int steps;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
            steps = 0;
        }

        public Coordinate(int x, int y, int steps)
        {
            this.x = x;
            this.y = y;
            this.steps = steps;
        }

        public new string ToString()
        {
            return "( " + x + ", " + y + " )";
        }

        public new bool Equals(object other)
        {
            if(other is Coordinate)
            {
                var _other = other as Coordinate;
                if(x == _other.x && y == _other.y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
