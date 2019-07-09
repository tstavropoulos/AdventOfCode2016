using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        private const string inputFile = @"../../../../input06.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 6");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            CharDict[] dicts = new CharDict[lines[0].Length];
            for (int i = 0; i < dicts.Length; i++)
            {
                dicts[i] = new CharDict();
            }

            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    dicts[i].AddChar(line[i]);
                }
            }

            Console.WriteLine($"The message is: {new string(dicts.Select(x => x.GetMostCommonChar()).ToArray())}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"The message is: {new string(dicts.Select(x => x.GetLeastCommonChar()).ToArray())}");

            Console.WriteLine();
            Console.ReadKey();
        }


        private class CharDict
        {
            readonly int[] charCounts = new int[26];

            public void AddChar(char c)
            {
                charCounts[c - 'a']++;
            }

            public char GetMostCommonChar()
            {
                int maxIndex = 0;
                int max = charCounts[0];
                for (int i = 1; i < charCounts.Length; i++)
                {
                    if (charCounts[i] > max)
                    {
                        maxIndex = i;
                        max = charCounts[i];
                    }
                }

                return (char)('a' + maxIndex);
            }

            public char GetLeastCommonChar()
            {
                int minIndex = -1;
                int min = int.MaxValue;
                for (int i = 0; i < charCounts.Length; i++)
                {
                    if (charCounts[i] > 0 && charCounts[i] < min)
                    {
                        minIndex = i;
                        min = charCounts[i];
                    }
                }

                return (char)('a' + minIndex);
            }
        }
    }
}
