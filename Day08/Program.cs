using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        private const string inputFile = @"../../../../input08.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 8");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            bool[,] screen = new bool[50, 6];
            bool[] tempRow = new bool[50];
            bool[] tempColumn = new bool[6];

            string[] lines = File.ReadAllLines(inputFile);

            foreach (string line in lines)
            {
                if (line.StartsWith("rect "))
                {
                    (int w, int h) = ParseRectArg(line.Substring(5));

                    for (int x = 0; x < w; x++)
                    {
                        for (int y = 0; y < h; y++)
                        {
                            screen[x, y] = true;
                        }
                    }
                }
                else if (line.StartsWith("rotate row y="))
                {
                    (int y, int dx) = ParseRotateArg(line.Substring(13));

                    for (int x = 0; x < 50; x++)
                    {
                        tempRow[(x + dx) % 50] = screen[x, y];
                    }

                    for (int x = 0; x < 50; x++)
                    {
                        screen[x, y] = tempRow[x];
                    }
                }
                else if (line.StartsWith("rotate column x="))
                {
                    (int x, int dy) = ParseRotateArg(line.Substring(16));

                    for (int y = 0; y < 6; y++)
                    {
                        tempColumn[(y + dy) % 6] = screen[x, y];
                    }

                    for (int y = 0; y < 6; y++)
                    {
                        screen[x, y] = tempColumn[y];
                    }
                }
                else
                {
                    throw new Exception($"Unexpected operation: {line}");
                }
            }

            int litCount = 0;

            {
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        if (screen[x, y])
                        {
                            litCount++;
                        }
                    }
                }
            }

            Console.WriteLine($"Lit Pixels: {litCount}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine("Output:");
            Console.WriteLine();

            {
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 50; x++)
                    {
                        Console.BackgroundColor = screen[x, y] ? ConsoleColor.White : ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Done");
            Console.WriteLine();
            Console.ReadKey();
        }

        public static (int, int) ParseRectArg(string segment)
        {
            int indexOfX = segment.IndexOf('x');
            return (int.Parse(segment.Substring(0, indexOfX)), int.Parse(segment.Substring(indexOfX + 1)));
        }

        public static (int, int) ParseRotateArg(string segment)
        {
            int indexOfX = segment.IndexOf(" by ");
            return (int.Parse(segment.Substring(0, indexOfX)), int.Parse(segment.Substring(indexOfX + 4)));
        }
    }
}
