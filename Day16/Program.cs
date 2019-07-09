using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        private const string inputFile = @"../../../../input16.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 16");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            //Call the data you have at this point "a".
            //Make a copy of "a"; call this copy "b".
            //Reverse the order of the characters in "b".
            //In "b", replace all instances of 0 with 1 and all 1s with 0.
            //The resulting data is "a", then a single 0, then "b".


            const int targetLength = 272;
            string line = File.ReadAllText(inputFile);

            IEnumerable<bool> input = line.Select(x => x == '1').ToArray();

            while (input.Count() < targetLength)
            {
                input = Step(input).ToArray();
            }


            IEnumerable<bool> target = input.Take(targetLength);
            IEnumerable<bool> checkSum = target;

            while (checkSum.Count() % 2 == 0)
            {
                checkSum = CheckSum(checkSum).ToArray();
            }

            Console.WriteLine($"CheckSum: {new string(checkSum.Select(x => x ? '1' : '0').ToArray())}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            const int targetLength2 = 35651584;

            input = line.Select(x => x == '1').ToArray();

            while (input.Count() < targetLength2)
            {
                input = Step(input).ToArray();
            }


            target = input.Take(targetLength2);
            checkSum = target;

            while (checkSum.Count() % 2 == 0)
            {
                checkSum = CheckSum(checkSum).ToArray();
            }

            Console.WriteLine($"CheckSum: {new string(checkSum.Select(x => x ? '1' : '0').ToArray())}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static IEnumerable<bool> Step(IEnumerable<bool> input)
        {
            foreach (bool value in input)
            {
                yield return value;
            }

            yield return false;

            foreach (bool value in input.Reverse())
            {
                yield return !value;
            }
        }

        static IEnumerable<bool> CheckSum(IEnumerable<bool> input)
        {
            if (input.Count() % 2 == 1)
            {
                throw new Exception($"input already odd");
            }

            IEnumerator<bool> enumerator = input.GetEnumerator();

            while (enumerator.MoveNext())
            {
                bool valueA = enumerator.Current;

                enumerator.MoveNext();

                bool valueB = enumerator.Current;

                yield return valueA == valueB;
            }
        }
    }


}
