using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Day6
    {
        static void Main(string[] args)
        {
            OrbitCalculationUnit ocu = new OrbitCalculationUnit();
            //string input = File.ReadAllText("OrbitMap.txt");
            string input = File.ReadAllText("OrbitMapTest4.txt");
            string[] orbitalMap = input.Split('\n');

            var orbits = ocu.GenerateOrbitalMap(orbitalMap);
            Console.WriteLine();
            Console.WriteLine(orbits.ToString());
            Console.WriteLine();
            Console.WriteLine(orbits.OrbitalChecksum());
        }
    }

    class OrbitCalculationUnit
    {
        public OrbitalMap GenerateOrbitalMap(string[] mapstring)
        {
            List<(string orbitee, string orbiter)> orbits = new List<(string orbitee, string orbiter)>();

            foreach (string o in mapstring)
            {
                Console.WriteLine("add " + o);
                var temp = o.Split(')');

                if (temp.Length > 1)
                {
                    // bullshit formatting of input string -> it contains controll characters between names and \n
                    if(temp[1].Length > 3)
                        orbits.Add((temp[0], temp[1].Substring(0, temp[1].Length - 1)));
                    else if(temp[1].Length == 3)
                        orbits.Add((temp[0], temp[1].Substring(0, temp[1].Length)));
                    else if(temp[1].Length > 1)
                        orbits.Add((temp[0], temp[1].Substring(0, temp[1].Length - 1)));
                    else
                        orbits.Add((temp[0], temp[1].Substring(0, temp[1].Length)));
                }
            }

            OrbitalMap map = new OrbitalMap();

            foreach((string orbitee, string orbiter) mapping in orbits)
            {
                map.Add(mapping.orbitee, mapping.orbiter);
            }

            return map;
        }
    }

    // this is a tree
    class OrbitalMap
    {
        List<Orbit> roots = new List<Orbit>();

        public void Add(string orbitee, string orbiter)
        {
            Orbit _orbitee = null;
            Orbit _orbiter = null;

            foreach(Orbit o in roots)
            {
                var c1 = Crawl(o, orbitee);
                if (c1.found)
                    _orbitee = c1.thing;

                var c2 = Crawl(o, orbiter);
                if (c2.found)
                    _orbiter = c2.thing;
            }

            if(_orbitee == null && _orbiter == null)
            {
                _orbitee = new Orbit(orbitee);
                _orbiter = new Orbit(orbiter);
                
                _orbitee.children.Add(_orbiter);
                _orbiter.parent = _orbitee;

                roots.Add(_orbitee);
            }
            else if(_orbiter == null)
            {
                _orbiter = new Orbit(orbiter);

                _orbitee.children.Add(_orbiter);
                _orbiter.parent = _orbitee;
            }
            else if(_orbitee == null)
            {
                _orbitee = new Orbit(orbitee);
                
                _orbitee.children.Add(_orbiter);
                _orbiter.parent = _orbitee;

                if(roots.Contains(_orbiter))
                    roots.Remove(_orbiter);
                roots.Add(_orbitee);
            }
            else
            {
                _orbitee.children.Add(_orbiter);
                _orbiter.parent = _orbitee;

                if(roots.Contains(_orbitee))
                    roots.Remove(_orbitee);
                if(roots.Contains(_orbiter))
                    roots.Remove(_orbiter);
            }
        }

        (bool found, Orbit thing) Crawl(Orbit root, string find)
        {
            List<Orbit> todo = new List<Orbit>();

            todo.Add(root);

            while(todo.Count > 0)
            {
                var current = todo[0];
                if (current.name.Equals(find))
                {
                    return (true, current);
                }

                todo.Remove(current);

                // add more work to the que
                foreach(Orbit c in current.children)
                {
                    todo.Add(c);
                }
            }

            return (false, null);
        }

        public int OrbitalChecksum()
        {
            return 0;
        }
    }

    class Orbit
    {
        // the closest thing this orbits
        public Orbit parent;

        // things that orbits this
        public List<Orbit> children;

        // the name of this node/leaf
        public string name;

        public Orbit(string name)
        {
            this.name = name;
            children = new List<Orbit>();
        }
    }
}
