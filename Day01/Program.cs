using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    public enum Facing
    {
        North,
        East,
        South,
        West,
        MAX
    }

    class Program
    {
        private const string inputFile = @"../../../../input01.txt";


        static void Main(string[] args)
        {
            Console.WriteLine("Day 1");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            var instructions = File.ReadAllText(inputFile).Split(", ");
            //var instructions = "R8, R4, R4, R8".Split(", ");


            //Start facing north.
            Facing facing = Facing.North;

            (int x, int y) position = (0, 0);

            HashSet<(int x, int y)> visitedLocations = new HashSet<(int x, int y)>() { (0, 0) };

            bool found = false;
            (int x, int y) hqPos = (0, 0);

            foreach (string instruction in instructions)
            {
                switch (instruction[0])
                {
                    case 'R':
                        facing = facing.Rotate(true);
                        break;

                    case 'L':
                        facing = facing.Rotate(false);
                        break;

                    default:
                        throw new ArgumentException($"Unexpected direction: {instruction[0]}");
                }

                int distance = int.Parse(instruction.Substring(1));

                foreach ((int x, int y) p in position.Move(facing, distance))
                {
                    if (!found)
                    {
                        if (!visitedLocations.Add(p))
                        {
                            found = true;
                            hqPos = p;
                        }
                    }
                }

                position = position.Move(facing, distance).Last();
            }

            Console.WriteLine($"Final distance: {Math.Abs(position.x) + Math.Abs(position.y)}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"HQ Location: {Math.Abs(hqPos.x) + Math.Abs(hqPos.y)}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public static class FacingExtensions
    {
        public static Facing Rotate(this Facing facing, bool right)
        {
            if (right)
            {
                return (Facing)((int)(facing + 1) % (int)Facing.MAX);
            }

            return (Facing)((int)(facing + (int)Facing.MAX - 1) % (int)Facing.MAX);
        }

        public static IEnumerable<(int x, int y)> Move(this (int x, int y) p, Facing facing, int distance)
        {
            int dx = 0;
            int dy = 0;

            switch (facing)
            {
                case Facing.North:
                    dy = 1;
                    break;

                case Facing.East:
                    dx = 1;
                    break;

                case Facing.South:
                    dy = -1;
                    break;

                case Facing.West:
                    dx = -1;
                    break;

                default:
                    throw new ArgumentException($"Unexpected Facing {facing}");
            }

            for (int i = 1; i <= distance; i++)
            {
                yield return (p.x + i * dx, p.y + i * dy);
            }
        }
    }

}
