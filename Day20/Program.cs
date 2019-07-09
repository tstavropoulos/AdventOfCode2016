using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        private const string inputFile = @"../../../../input20.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 20");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            List<Range> ranges = new List<Range>();

            string[] lines = File.ReadAllLines(inputFile);

            foreach (string line in lines)
            {
                int dashIndex = line.IndexOf('-');
                long low = long.Parse(line.Substring(0, dashIndex));
                long high = long.Parse(line.Substring(dashIndex + 1));
                Range newRange = new Range(low, high);

                if (ranges.Count == 0)
                {
                    ranges.Add(newRange);
                }
                else
                {
                    bool found = false;
                    for (int i = 0; i < ranges.Count; i++)
                    {
                        if (newRange.IsOverlapping(ranges[i]))
                        {
                            newRange = newRange.Merge(ranges[i]);
                            ranges.RemoveAt(i);
                            i--;
                            found = true;
                        }
                        else if (found)
                        {
                            //Reached the end of overlaps
                            break;
                        }
                    }

                    //Insert
                    int index;
                    for (index = 0; index < ranges.Count; index++)
                    {
                        if (ranges[index].low > newRange.low)
                        {
                            break;
                        }
                    }

                    ranges.Insert(index, newRange);
                }
            }

            Console.WriteLine($"First Range: {ranges[0].low} : {ranges[0].high}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long totalIPs = 0;
            long maxIP = 4294967295;

            long last = 0;


            foreach (Range range in ranges)
            {
                totalIPs += range.low - last;
                last = range.high + 1;
            }

            totalIPs += maxIP - last + 1;

            Console.WriteLine($"Total Allowed IPs: {totalIPs}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private readonly struct Range
        {
            public readonly long low;
            public readonly long high;

            public Range(long low, long high)
            {
                this.low = low;
                this.high = high;
            }

            public bool IsOverlapping(in Range otherRange)
            {
                if (high < otherRange.low - 1)
                {
                    return false;
                }

                if (low > otherRange.high + 1)
                {
                    return false;
                }

                return true;
            }

            public Range Merge(in Range otherRange) =>
                new Range(Math.Min(low, otherRange.low), Math.Max(high, otherRange.high));
        }
    }
}
