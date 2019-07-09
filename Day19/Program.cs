using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    class Program
    {
        private const int input = 3_014_387;
        //private const int input = 5;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 19");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Elf firstElf = new Elf(1);
            Elf lastElf = firstElf;

            for (int i = 2; i <= input; i++)
            {
                lastElf = new Elf(i, lastElf);
            }
            lastElf.next = firstElf;

            Elf currentElf = firstElf;

            while (currentElf.Steal())
            {
                currentElf = currentElf.next;
            }


            Console.WriteLine($"Last Elf: {currentElf.id}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            firstElf = new Elf(1);
            lastElf = firstElf;

            for (int i = 2; i <= input; i++)
            {
                lastElf = new Elf(i, lastElf);
            }
            lastElf.next = firstElf;

            int currentCount = input;

            Elf preAcross = firstElf;
            int steps = currentCount / 2 - 1;
            for(int i = 0; i < steps; i++)
            {
                preAcross = preAcross.next;
            }

            while (preAcross.next != preAcross)
            {
                preAcross.next = preAcross.next.next;
                if (currentCount % 2 == 1)
                {
                    preAcross = preAcross.next;
                }

                currentCount--;
            }

            Console.WriteLine($"Last Elf: {preAcross.id}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private class Elf
        {
            public Elf next;

            public readonly int id;

            public Elf(int id)
            {
                this.id = id;
            }

            public Elf(int id, Elf prev)
            {
                this.id = id;

                prev.next = this;
            }

            public bool Steal()
            {
                if (next == this)
                {
                    return false;
                }

                next = next.next;
                return true;
            }
        }
    }
}
