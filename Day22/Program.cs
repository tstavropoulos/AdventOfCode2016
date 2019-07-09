using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day22
{
    class Program
    {
        private const string inputFile = @"../../../../input22.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 22");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<Node> nodes = File.ReadAllLines(inputFile).Skip(2).Select(x => new Node(x)).ToList();


            Node[] availSortedNodes = nodes.Where(x => x.avail != 0).OrderByDescending(x => x.avail).ToArray();
            Node[] usedSortedNodes = nodes.Where(x => x.used != 0).OrderBy(x => x.used).ToArray();

            int pairCount = 0;

            int usedRemaining = usedSortedNodes.Length - 1;
            int availIndex = 0;

            //Tick through used nodes in reverse, and available nodes forward
            //Start by comparing the largest Used with the largest Available
            while (usedRemaining >= 0 && availIndex < availSortedNodes.Length)
            {
                if (availSortedNodes[availIndex].avail >= usedSortedNodes[usedRemaining].used)
                {
                    //if the current Used fits in the current Available,
                    //  then *every* smaller used will too, so add them *all* to paircount, and advance
                    //  to the next available
                    pairCount += usedRemaining + 1;
                    availIndex++;
                }
                else
                {
                    //If the current Used doesn't fit in the current Available,
                    //  then move on to the next Used.
                    usedRemaining--;
                }
            }

            Console.WriteLine($"Fitting Pairs: {pairCount}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //Need to move from (0,27) to (0,0)

            //Print grid:
            int maxX = nodes.Where(node => node.y == 0).Select(node => node.x).Max() + 1;
            int maxY = nodes.Where(node => node.x == 0).Select(node => node.y).Max() + 1;

            StringBuilder builder = new StringBuilder();
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    builder.Append(Node.nodeGrid[(x, y)].GetGridIcon());
                }

                Console.WriteLine(builder.ToString());
                builder.Clear();
            }

            //The grid:

            //    .....................................*
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    ......................................
            //    .........XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            //    ......................................
            //    .................................. ...
            //    ......................................

            //80 moves to move it to the left of the target data
            //1 move to move the target data over one column
            //  5 moves per target dataset over after that
            //  36 steps up = 180 steps
            //180 + 1 + 80 = 261

            Console.WriteLine();
            Console.ReadKey();
        }

        public class Node
        {
            public readonly int x;
            public readonly int y;

            public readonly int size;

            public readonly int used;
            public readonly int avail;

            public static readonly Dictionary<(int, int), Node> nodeGrid = new Dictionary<(int, int), Node>();
            private static readonly char[] splitChars = new char[] { ' ', '-' };

            public Node(string line)
            {
                string[] splitLine = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                x = int.Parse(splitLine[1].Substring(1));
                y = int.Parse(splitLine[2].Substring(1));

                size = int.Parse(splitLine[3].Substring(0, splitLine[3].Length - 1));
                used = int.Parse(splitLine[4].Substring(0, splitLine[4].Length - 1));
                avail = int.Parse(splitLine[5].Substring(0, splitLine[5].Length - 1));

                if (size != used + avail)
                {
                    throw new Exception("Wat?");
                }

                nodeGrid[(x, y)] = this;
            }

            private string Name => $"node-x{x}-y{y}";

            public override string ToString() => $"{Name,13}  {size,3}T  {used,3}T  {avail,3}T";

            public char GetGridIcon()
            {
                if (used == 0)
                {
                    return ' ';
                }

                if (used > 100)
                {
                    return 'X';
                }

                if (x == 37 && y == 0)
                {
                    return '*';
                }

                return '.';
            }
        }
    }
}
