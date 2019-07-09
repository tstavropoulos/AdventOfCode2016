using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        private const string inputFile = @"../../../../input18.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 18");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            //The rule is *actually*: Tile is a trap if LeftTrapStatus != RightTrapStatus
            //  Center doesn't make a difference

            string line = File.ReadAllText(inputFile);

            List<string> rows = new List<string>() { line };
            char[] newLine = new char[line.Length];

            for (int i = 1; i < 40; i++)
            {
                for (int j = 0; j < newLine.Length; j++)
                {
                    bool leftTrap = j == 0 ? false : rows[i - 1][j - 1] == '^';
                    bool rightTrap = j == newLine.Length - 1 ? false : rows[i - 1][j + 1] == '^';

                    newLine[j] = leftTrap != rightTrap ? '^' : '.';
                }

                rows.Add(new string(newLine));
            }

            int safeCount = 0;
            foreach (string row in rows)
            {
                safeCount += row.Count(x => x == '.');
            }

            Console.WriteLine($"Safe Tiles in 40 rows: {safeCount}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            for (int i = 0; i < newLine.Length; i++)
            {
                newLine[i] = line[i];
            }

            long safeCount2 = line.Count(x => x == '.');

            for (int i = 1; i < 400_000; i++)
            {
                for (int j = 0; j < newLine.Length; j++)
                {
                    bool leftTrap = j == 0 ? false : line[j - 1] == '^';
                    bool rightTrap = j == newLine.Length - 1 ? false : line[j + 1] == '^';

                    newLine[j] = leftTrap != rightTrap ? '^' : '.';
                }

                line = new string(newLine);

                safeCount2 += line.Count(x => x == '.');
            }


            Console.WriteLine($"Safe Tiles in 400000 rows: {safeCount2}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
