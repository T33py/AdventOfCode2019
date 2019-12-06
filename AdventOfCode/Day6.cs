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
            string input = File.ReadAllText("OrbitMap.txt");
            //string input = File.ReadAllText("OrbitMapTest5.txt");
            string[] orbitalMap = input.Split('\n');

            var orbits = ocu.GenerateOrbitalMap(orbitalMap);
            Console.WriteLine();
            Console.WriteLine(orbits.ToString());
            Console.WriteLine();
            Console.WriteLine(orbits.OrbitalChecksum());
            Console.WriteLine();
            var route = orbits.CalculateRoute("YOU", "SAN", orbits.Map[0]);
            Console.WriteLine("Dist you -> san " + orbits.CalculateDistance("YOU", "SAN", orbits.Map[0]));
            Console.WriteLine("route:");
            foreach(Orbit o in route)
            {
                Console.WriteLine(o.name);
            }

        }
    }

    class OrbitCalculationUnit
    {
        public OrbitalMap GenerateOrbitalMap(string[] mapstring)
        {
            List<(string orbitee, string orbiter)> orbits = new List<(string orbitee, string orbiter)>();

            foreach (string o in mapstring)
            {
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
                Console.WriteLine("add " + mapping.orbitee + " ) " + mapping.orbiter);
                map.Add(mapping.orbitee, mapping.orbiter);
            }

            return map;
        }
    }

    // this is a tree
    class OrbitalMap
    {
        List<Orbit> roots = new List<Orbit>();
        public List<Orbit> Map { get => roots; }

        public void Add(string orbitee, string orbiter)
        {
            Orbit _orbitee = null;
            Orbit _orbiter = null;

            foreach(Orbit o in roots)
            {
                var c1 = FindOrbit(o, orbitee);
                if (c1.found)
                    _orbitee = c1.thing;

                var c2 = FindOrbit(o, orbiter);
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

                if(roots.Contains(_orbiter))
                    roots.Remove(_orbiter);
            }
        }

        (bool found, Orbit thing) FindOrbit(Orbit root, string find)
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

        public int CalculateDistance(string from, string to, Orbit map)
        {
            var route = CalculateRoute(from, to, map);
            return route.Count - 1; // distance is allways jumps = nodes - 1
        }

        public List<Orbit> CalculateRoute(string from, string to, Orbit map)
        {
            var routeToStart = new List<Orbit>();
            var routeToEnd = new List<Orbit>();
            var route = new List<Orbit>();

            Orbit start;
            Orbit end;
            var temp = FindOrbit(map, from);
            if (temp.found)
                start = temp.thing;
            else
                throw new ArgumentException();
            
            temp = FindOrbit(map, to);
            if (temp.found)
                end = temp.thing;
            else
                throw new ArgumentException();


            while (start.parent != null)
            {
                routeToStart.Add(start.parent);
                start = start.parent;
            }
            while (end.parent != null)
            {
                routeToEnd.Add(end.parent);
                end = end.parent;
            }

            Orbit commonParent = null;
            foreach(Orbit o in routeToStart)
            {
                route.Add(o);
                if (routeToEnd.Contains(o)) // first common ancestor, as we generate the lists from the target not the root
                {
                    commonParent = o;
                    break;
                }
            }

            foreach(Orbit o in routeToEnd)
            {
                if(o == commonParent)
                {
                    break;
                }
                route.Add(o);
            }

            return route;
        }

        public int OrbitalChecksum()
        {
            if (roots.Count == 0)
                return 0;

            int checksum = 0;
            List<Orbit> todo = new List<Orbit>();


            foreach (Orbit root in roots)
            {
                todo.Add(root);
            }

            while (todo.Count > 0)
            {
                var current = todo[0];
                checksum += current.Checksum();

                todo.Remove(current);

                // add more work to the que
                foreach (Orbit c in current.children)
                {
                    todo.Add(c);
                }
            }
            return checksum;
        }

        public new string ToString()
        {
            string s = "";
            foreach(Orbit o in roots)
            {
                s = s + o.ToString() + "\n\n";
            }
            return s;
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

        public int Checksum()
        {
            if (parent == null)
                return 0;
            else return parent.Checksum() + 1;
        }

        public new string ToString()
        {
            String s = "";
            foreach (Orbit c in children)
            {
                s = s + name + " ) " + c.name + "\n";
                s = s + c.ToString();
            }

            return s;
        }
    }
}
