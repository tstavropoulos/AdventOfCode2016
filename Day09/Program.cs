using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        private const string inputFile = @"../../../../input09.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 9");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            Console.WriteLine($"Decompressed Line Length: {Expand(line)}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"Decompressed Line Length: {Expand2(line)}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static long Expand(string input)
        {
            long count = 0;
            int index = 0;

            while (index < input.Length)
            {
                if (input[index] == '(')
                {
                    int endParen = input.IndexOf(')', index);
                    (int l, int r) = ReadRep(input.Substring(index + 1, endParen - index - 1));

                    index = endParen + 1;

                    string repeatedSeq = input.Substring(index, l);

                    count += r * l;
                    index += l;
                }
                else
                {
                    count++;
                    index++;
                }
            }

            return count;
        }

        public static long Expand2(string input)
        {
            long count = 0;
            int index = 0;

            while (index < input.Length)
            {
                if (input[index] == '(')
                {
                    int endParen = input.IndexOf(')', index);
                    (int l, int r) = ReadRep(input.Substring(index + 1, endParen - index - 1));

                    index = endParen + 1;

                    count += r * Expand2(input.Substring(index, l));
                    index += l;
                }
                else
                {
                    count++;
                    index++;
                }
            }

            return count;
        }

        public static (int l,int r) ReadRep(string segment)
        {
            int indexOfX = segment.IndexOf('x');
            return (int.Parse(segment.Substring(0, indexOfX)), int.Parse(segment.Substring(indexOfX + 1)));
        }
    }
}
