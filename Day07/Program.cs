using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        private const string inputFile = @"../../../../input07.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 7");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            // IPv7 supports TLS if:
            //    It has ABBA (any 4-char sequences of XY then YX, where X!=Y) in supernet sequence
            //    No ABBA in hypernet sequences (square brackets)

            string[] lines = File.ReadAllLines(inputFile);
            int TLScount = lines.Select(SupportsTLS).Where(x => x == true).Count();

            Console.WriteLine($"{TLScount} IPs support TLS");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            // IPv7 supports SSL if:
            //    It has ABA (any 3-char sequences of XYX, where X!=Y) in supernet sequence
            //    A matching BAB in any hypernet sequence

            int SSLcount = lines.Select(SupportsSSL).Where(x => x == true).Count();

            Console.WriteLine($"{SSLcount} IPs support SSL");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static bool SupportsTLS(string ipAddr)
        {
            if (GetSequences(ipAddr, true).Any(HasABBA))
            {
                //Doesn't support TLS if Any hypernet sequences contain ABBA
                return false;
            }

            //Then, supports TLS if Any supernet sequence contains ABBA
            return GetSequences(ipAddr, false).Any(HasABBA);
        }

        public static bool SupportsSSL(string ipAddr)
        {
            IEnumerable<string> babSequences = GetSequences(ipAddr, false).SelectMany(GetBABSequences).Distinct();

            foreach (string hypernetSequence in GetSequences(ipAddr, true))
            {
                foreach(string babSeq in babSequences)
                {
                    if (hypernetSequence.Contains(babSeq))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HasABBA(string segment)
        {
            for (int i = 0; i < segment.Length - 3; i++)
            {
                if (segment[i] == segment[i + 3] &&
                    segment[i + 1] == segment[i + 2] &&
                    segment[i] != segment[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<string> GetSequences(string ipAddr, bool hypernet)
        {
            bool insideHypernet = false;
            string[] splitIP = ipAddr.Split('[', ']');
            foreach (string segment in splitIP)
            {
                if (hypernet == insideHypernet)
                {
                    yield return segment;
                }

                insideHypernet = !insideHypernet;
            }
        }

        public static IEnumerable<string> GetBABSequences(string segment)
        {
            for (int i = 0; i < segment.Length - 2; i++)
            {
                if (segment[i] == segment[i + 2] &&
                    segment[i] != segment[i + 1])
                {
                    yield return $"{segment[i + 1]}{segment[i]}{segment[i + 1]}";
                }
            }
        }
    }
}
