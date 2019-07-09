using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day24
{
    class Program
    {
        private const string inputFile = @"../../../../input24.txt";

        public static int W;
        public static int H;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 24");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            //There are 7 points of interest.
            //Can be Visited in any order
            //Possible orderings: 7!
            //5040 possible orderings.

            //It is feasible to just brute-force test all orderings.
            string[] lines = File.ReadAllLines(inputFile);

            W = lines[0].Length;
            H = lines.Length;

            bool[,] grid = new bool[W, H];
            int[,] distance = new int[W, H];

            Point start = (0, 0);

            List<Point> pointsOfInterest = new List<Point>();

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    char c = lines[y][x];
                    if (c == '.')
                    {
                        grid[x, y] = true;
                    }
                    else if (c == '#')
                    {
                        grid[x, y] = false;
                    }
                    else if (c == '0')
                    {
                        grid[x, y] = true;
                        start = new Point(x, y);
                    }
                    else
                    {
                        grid[x, y] = true;
                        pointsOfInterest.Add(new Point(x, y));
                    }
                }
            }

            pointsOfInterest.Insert(0, start);

            //If the optimal path from point A to point B crosses point C, I don't need to bother
            //including that permutation.

            //Start by calculating the distance between each pair of points of interest

            Dictionary<(int a, int b), int> distanceMap = new Dictionary<(int a, int b), int>();

            for (int a = 0; a < pointsOfInterest.Count - 1; a++)
            {
                for (int b = a + 1; b < pointsOfInterest.Count; b++)
                {
                    int steps = CalculateSteps(pointsOfInterest[a], pointsOfInterest[b], grid, distance);
                    distanceMap.Add((a, b), steps);
                    distanceMap.Add((b, a), steps);
                }
            }

            List<int> destinationList = new List<int>();

            int shortestPath = ShortestPath(0, Enumerable.Range(1, pointsOfInterest.Count - 1), distanceMap);

            Console.WriteLine($"Shortest Path: {shortestPath}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //What is the shortest path to visit every marked space on the map and return to the origin?

            int shortestPath2 = ShortestPath2(0, Enumerable.Range(1, pointsOfInterest.Count - 1), distanceMap);

            Console.WriteLine($"Shortest Path2: {shortestPath2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        private static int ShortestPath(
            int start,
            IEnumerable<int> remaining,
            Dictionary<(int a, int b), int> distanceMap)
        {
            int remainingCount = remaining.Count();
            if (remainingCount == 1)
            {
                return distanceMap[(start, remaining.First())];
            }

            int min = int.MaxValue;
            for (int i = 0; i < remainingCount; i++)
            {
                int value = remaining.ElementAt(i);
                int newPath = distanceMap[(start, value)] + ShortestPath(
                    start: remaining.ElementAt(i),
                    remaining: remaining.Where((x, j) => j != i),
                    distanceMap: distanceMap);
                min = Math.Min(min, newPath);
            }

            return min;
        }

        private static int ShortestPath2(
            int start,
            IEnumerable<int> remaining,
            Dictionary<(int a, int b), int> distanceMap)
        {
            int remainingCount = remaining.Count();
            if (remainingCount == 1)
            {
                int final = remaining.First();
                return distanceMap[(start, final)] + distanceMap[(final,0)];
            }

            int min = int.MaxValue;
            for (int i = 0; i < remainingCount; i++)
            {
                int value = remaining.ElementAt(i);
                int newPath = distanceMap[(start, value)] + ShortestPath2(
                    start: remaining.ElementAt(i),
                    remaining: remaining.Where((x, j) => j != i),
                    distanceMap: distanceMap);
                min = Math.Min(min, newPath);
            }

            return min;
        }


        private static int CalculateSteps(in Point start, in Point end, bool[,] grid, int[,] distance)
        {
            //Reset
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    distance[x, y] = int.MaxValue;
                }
            }

            Point tempEnd = end;

            PriorityQueue<Point> pendingPoints = new PriorityQueue<Point>((Point point) => (tempEnd - point).Length);
            pendingPoints.Enqueue(start);
            distance[start.x, start.y] = 0;

            while (pendingPoints.Count > 0)
            {
                Point nextPoint = pendingPoints.Dequeue();
                int steps = distance[nextPoint.x, nextPoint.y] + 1;

                if (steps > distance[end.x, end.y])
                {
                    continue;
                }


                foreach (Point adj in nextPoint.GetAdjacentBoundedPoints())
                {
                    if (grid[adj.x, adj.y] && distance[adj.x, adj.y] > steps)
                    {
                        distance[adj.x, adj.y] = steps;
                        pendingPoints.EnqueueOrUpdate(adj);
                    }
                }
            }

            return distance[end.x, end.y];
        }

        public readonly struct Point
        {
            public readonly int x;
            public readonly int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static implicit operator Point((int x, int y) point) => new Point(point.x, point.y);
            public static implicit operator (int x, int y)(Point point) => (point.x, point.y);

            public override bool Equals(object obj)
            {
                if (!(obj is Point other))
                {
                    return false;
                }

                return x == other.x && y == other.y;
            }

            public override int GetHashCode() => HashCode.Combine(x, y);
            public override string ToString() => $"({x}, {y})";

            public IEnumerable<Point> GetAdjacentBoundedPoints()
            {
                if (x > 0)
                {
                    yield return new Point(x - 1, y);
                }

                if (x < W - 1)
                {
                    yield return new Point(x + 1, y);
                }

                if (y > 0)
                {
                    yield return new Point(x, y - 1);
                }

                if (y < H - 1)
                {
                    yield return new Point(x, y + 1);
                }
            }

            public static bool operator ==(in Point lhs, in Point rhs) =>
                lhs.x == rhs.x && lhs.y == rhs.y;

            public static bool operator !=(in Point lhs, in Point rhs) =>
                lhs.x != rhs.x || lhs.y != rhs.y;

            public static Point operator +(in Point lhs, in Point rhs) =>
                new Point(lhs.x + rhs.x, lhs.y + rhs.y);
            public static Point operator -(in Point lhs, in Point rhs) =>
                new Point(lhs.x - rhs.x, lhs.y - rhs.y);

            public int Length => Math.Abs(x) + Math.Abs(y);
        }
    }
}
