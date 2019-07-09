using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        private const string inputFile = @"../../../../input02.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 2");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int[,] keypad =
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };

            string[] instructions = File.ReadAllLines(inputFile);

            //Start at 5
            int x = 1;
            int y = 1;

            List<int> combo = new List<int>();


            foreach (string line in instructions)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'U':
                            x = Math.Max(0, x - 1);
                            break;

                        case 'D':
                            x = Math.Min(2, x + 1);
                            break;

                        case 'L':
                            y = Math.Max(0, y - 1);
                            break;

                        case 'R':
                            y = Math.Min(2, y + 1);
                            break;

                        default:
                            throw new Exception($"Unexpected char: {c}");
                    }
                }

                combo.Add(keypad[x, y]);
            }


            Console.WriteLine($"Combo: {string.Join("", combo.Select(i => i.ToString()))}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            char[,] keypad2 =
            {
                {'\0', '\0', '1', '\0', '\0' },
                {'\0', '2', '3', '4', '\0' },
                {'5', '6', '7', '8', '9' },
                {'\0', 'A', 'B', 'C', '\0' },
                {'\0', '\0', 'D', '\0', '\0' }
            };

            List<char> combo2 = new List<char>();

            x = 2;
            y = 2;

            foreach (string line in instructions)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'U':
                            if (x > 0 && keypad2[x - 1, y] != '\0')
                            {
                                x--;
                            }
                            break;

                        case 'D':
                            if (x < 4 && keypad2[x + 1, y] != '\0')
                            {
                                x++;
                            }
                            break;

                        case 'L':
                            if (y > 0 && keypad2[x, y - 1] != '\0')
                            {
                                y--;
                            }
                            break;

                        case 'R':
                            if (y < 4 && keypad2[x, y + 1] != '\0')
                            {
                                y++;
                            }
                            break;

                        default:
                            throw new Exception($"Unexpected char: {c}");
                    }
                }

                combo2.Add(keypad2[x, y]);
            }

            Console.WriteLine($"Combo: {string.Join("", combo2.Select(i => i.ToString()))}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
