using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        private const string inputFile = @"../../../../input03.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 3");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] triangles = File.ReadAllLines(inputFile);

            int possibleTriangles = 0;

            foreach (string triangle in triangles)
            {
                int[] sides = triangle.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).OrderByDescending(x => x).ToArray();

                if (sides[0] < sides[1] + sides[2])
                {
                    possibleTriangles++;
                }
            }

            Console.WriteLine($"Possible triangles: {possibleTriangles}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            possibleTriangles = 0;

            List<int> setA = new List<int>();
            List<int> setB = new List<int>();
            List<int> setC = new List<int>();

            foreach (string triangle in triangles)
            {
                int[] sides = triangle.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray();

                setA.Add(sides[0]);
                setB.Add(sides[1]);
                setC.Add(sides[2]);

                if (setA.Count == 3)
                {
                    setA.Sort((x, y) => y.CompareTo(x));

                    if (setA[0] < setA[1] + setA[2])
                    {
                        possibleTriangles++;
                    }

                    setB.Sort((x, y) => y.CompareTo(x));

                    if (setB[0] < setB[1] + setB[2])
                    {
                        possibleTriangles++;
                    }

                    setC.Sort((x, y) => y.CompareTo(x));

                    if (setC[0] < setC[1] + setC[2])
                    {
                        possibleTriangles++;
                    }

                    setA.Clear();
                    setB.Clear();
                    setC.Clear();
                }
            }

            Console.WriteLine($"Possible triangles: {possibleTriangles}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
