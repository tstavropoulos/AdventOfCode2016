using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        private const string inputFile = @"../../../../input04.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 4");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            List<Room> rooms = lines.Select(x => new Room(x)).ToList();

            long cumulativeIds = 0;

            foreach(Room room in rooms)
            {
                if (room.IsReal())
                {
                    cumulativeIds += room.sectorID;
                }
            }

            Console.WriteLine($"Cumulative real sector IDs: {cumulativeIds}");

            Room testRoom = new Room("qzmt-zixmtkozy-ivhz-343[abcde]");

            Console.WriteLine($"  {testRoom.GetNewReal()}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            foreach (Room room in rooms)
            {
                string newReal = room.GetNewReal();

                //Console.WriteLine($"  {newReal}");

                if (newReal.Contains("north"))
                {
                    Console.WriteLine($"New real sector: {room.name} ID: {room.sectorID}");
                }
            }

            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public readonly struct Room
    {
        public readonly string name;
        public readonly int sectorID;
        public readonly string checksum;
        
        public Room(string line)
        {
            int lastDash = line.LastIndexOf('-');
            int lastBracket = line.LastIndexOf('[');

            sectorID = int.Parse(line.Substring(lastDash + 1, lastBracket - lastDash - 1));

            checksum = line.Substring(lastBracket + 1, line.Length - lastBracket - 2);

            name = line.Substring(0, lastDash);
        }

        public bool IsReal()
        {
            string expectedCheck = string.Join("",name.Where(x=>x != '-')
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key)
                .Select(x=>x.Key).Take(5));

            return expectedCheck == checksum;
        }

        public string GetNewReal() => string.Join("", name.Select(Rotate));

        public char Rotate(char c)
        {
            if (c == '-')
            {
                return ' ';
            }

            return (char)('a' + (((c - 'a') + sectorID) % ('z' - 'a' + 1)));
        }
    }
}
