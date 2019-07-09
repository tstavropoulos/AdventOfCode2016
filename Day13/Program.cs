using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        const int input = 1362;
        static Dictionary<byte, int> bitCount = new Dictionary<byte, int>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 13");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            bitCount.Add(0, 0);
            bitCount.Add(1, 1);

            List<byte> internalValues = new List<byte>() { 0, 1 };

            for (int order = 1; order < 8; order++)
            {
                foreach (byte interior in internalValues.ToArray())
                {
                    byte value = (byte)(interior | (1 << order));
                    bitCount.Add(value, bitCount[interior] + 1);

                    internalValues.Add(value);
                }
            }

            {
                (int x, int y) destination = (31, 39);

                Dictionary<(int x, int y), int> distances = new Dictionary<(int x, int y), int>();
                distances[(1, 1)] = 0;
                distances[destination] = int.MaxValue;

                Queue<(int x, int y)> pendingLocations = new Queue<(int x, int y)>();
                pendingLocations.Enqueue((1, 1));

                while (pendingLocations.Count > 0)
                {
                    (int x, int y) pos = pendingLocations.Dequeue();

                    int distanceCount = distances[pos];

                    if (distanceCount >= distances[destination])
                    {
                        //Too slow to matter
                        continue;
                    }

                    int nextDistanceCount = distanceCount + 1;
                    foreach ((int x, int y) newPos in GetAdjacentPositions(pos))
                    {
                        if (!IsOpen(newPos))
                        {
                            //Inside a wall
                            continue;
                        }

                        if (!distances.ContainsKey(newPos) || distances[newPos] > nextDistanceCount)
                        {
                            //Update or add
                            distances[newPos] = nextDistanceCount;
                            if (!pendingLocations.Contains(newPos) && newPos != destination)
                            {
                                pendingLocations.Enqueue(newPos);
                            }
                        }
                    }
                }

                Console.WriteLine($"Minimum number of steps: {distances[destination]}");
            }

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //How many places are within 50 steps?
            {
                Dictionary<(int x, int y), int> distances = new Dictionary<(int x, int y), int>();
                distances[(1, 1)] = 0;

                Queue<(int x, int y)> pendingLocations = new Queue<(int x, int y)>();
                pendingLocations.Enqueue((1, 1));

                while (pendingLocations.Count > 0)
                {
                    (int x, int y) pos = pendingLocations.Dequeue();

                    int nextDistanceCount = distances[pos] + 1;
                    foreach ((int x, int y) newPos in GetAdjacentPositions(pos))
                    {
                        if (!IsOpen(newPos))
                        {
                            //Inside a wall
                            continue;
                        }

                        if (!distances.ContainsKey(newPos) || distances[newPos] > nextDistanceCount)
                        {
                            //Update or add
                            distances[newPos] = nextDistanceCount;
                            if (!pendingLocations.Contains(newPos) && nextDistanceCount < 50)
                            {
                                pendingLocations.Enqueue(newPos);
                            }
                        }
                    }
                }

                Console.WriteLine($"Total positions within 50 Steps: {distances.Count}");

                //DrawGrid(distances);
            }

            Console.WriteLine();
            Console.ReadKey();
        }

        private void DrawGrid(Dictionary<(int x, int y), int> distances)
        {
            Console.WindowWidth = 200;

            Console.WriteLine();

            for (int y = 0; y < 52; y++)
            {
                for (int x = 0; x < 52; x++)
                {
                    if (distances.ContainsKey((x, y)))
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{distances[(x, y)]:D2} ");
                    }
                    else
                    {
                        Console.BackgroundColor = IsOpen((x, y)) ? ConsoleColor.Black : ConsoleColor.White;
                        Console.Write("   ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static bool IsOpen((int x, int y) pos)
        {
            int sum = pos.x * (pos.x + 3 + 2 * pos.y) + pos.y * (1 + pos.y) + input;

            int count = 0;

            do
            {
                count += bitCount[(byte)(sum & 0b1111_1111)];
                sum >>= 8;
            }
            while (sum > 0);

            return count % 2 == 0;
        }

        public static IEnumerable<(int x, int y)> GetAdjacentPositions((int x, int y) pos)
        {
            bool up = pos.y > 0;
            bool left = pos.x > 0;

            yield return (pos.x + 1, pos.y);
            yield return (pos.x, pos.y + 1);

            if (up)
            {
                yield return (pos.x, pos.y - 1);
            }

            if (left)
            {
                yield return (pos.x - 1, pos.y);
            }


        }
    }
}
