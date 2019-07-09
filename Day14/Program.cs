using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        const string input = "jlmsuwbz";
        //const string input = "abc";

        static Regex threePlus = new Regex("(.)\\1{2,}");
        static Regex fivePlus = new Regex("(.)\\1{4,}");

        static void Main(string[] args)
        {
            Console.WriteLine("Day 14");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            //First, scan first 1000 and identify any sequences of 5 or more
            //  Log onset sequence number and repeated digit.
            //Then, rolling loop, expire outdated 5+ matches, and look for matching 3+ match

            List<(int index, char digit)> validNums = new List<(int index, char digit)>();

            using (MD5 md5Hash = MD5.Create())
            {
                int hitCount = 0;

                //Prep
                for (int i = 1; i < 1000; i++)
                {
                    string hash = GetMd5Hash(md5Hash, $"{input}{i}");

                    if (fivePlus.IsMatch(hash))
                    {
                        MatchCollection matches = fivePlus.Matches(hash);
                        foreach (Match match in matches)
                        {
                            validNums.Add((i, match.Value[0]));
                        }
                    }
                }

                int index = -1;
                while (hitCount < 64)
                {
                    index++;

                    //Remove defunct matches
                    while (validNums.Count > 0 && validNums[0].index <= index)
                    {
                        validNums.RemoveAt(0);
                    }

                    //Potentially add new match
                    string hash = GetMd5Hash(md5Hash, $"{input}{index + 1000}");

                    if (fivePlus.IsMatch(hash))
                    {
                        MatchCollection matches = fivePlus.Matches(hash);
                        foreach (Match match in matches)
                        {
                            validNums.Add((index + 1000, match.Value[0]));
                        }
                    }

                    hash = GetMd5Hash(md5Hash, $"{input}{index}");
                    if (threePlus.IsMatch(hash))
                    {
                        Match match = threePlus.Matches(hash).First();
                        if (validNums.Any(x => x.digit == match.Value[0]))
                        {
                            hitCount++;
                        }
                    }
                }

                Console.WriteLine($"Found {hitCount} pads at index {index}");
            }


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            validNums.Clear();

            using (MD5 md5Hash = MD5.Create())
            {
                int hitCount = 0;

                //Prep
                for (int i = 1; i < 1000; i++)
                {
                    string hash = GetSuperMd5Hash(md5Hash, $"{input}{i}");

                    if (fivePlus.IsMatch(hash))
                    {
                        MatchCollection matches = fivePlus.Matches(hash);
                        foreach (Match match in matches)
                        {
                            validNums.Add((i, match.Value[0]));
                        }
                    }
                }

                int index = -1;
                while (hitCount < 64)
                {
                    index++;

                    //Remove defunct matches
                    while (validNums.Count > 0 && validNums[0].index <= index)
                    {
                        validNums.RemoveAt(0);
                    }

                    //Potentially add new match
                    string hash = GetSuperMd5Hash(md5Hash, $"{input}{index + 1000}");

                    if (fivePlus.IsMatch(hash))
                    {
                        MatchCollection matches = fivePlus.Matches(hash);
                        foreach (Match match in matches)
                        {
                            validNums.Add((index + 1000, match.Value[0]));
                        }
                    }

                    hash = GetSuperMd5Hash(md5Hash, $"{input}{index}");
                    if (threePlus.IsMatch(hash))
                    {
                        Match match = threePlus.Matches(hash).First();
                        if (validNums.Any(x => x.digit == match.Value[0]))
                        {
                            hitCount++;
                        }
                    }
                }

                Console.WriteLine($"Found {hitCount} pads at index {index}");
            }

            Console.WriteLine();
            Console.ReadKey();

        }

        /// <summary>
        /// Stolen from MS Docs on MD5
        /// </summary>
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static string GetSuperMd5Hash(MD5 md5Hash, string input)
        {
            for (int i = 0; i < 2017; i++)
            {
                input = GetMd5Hash(md5Hash, input);
            }

            return input;
        }
    }
}
