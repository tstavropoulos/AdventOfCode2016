using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Day05
{
    class Program
    {
        public const string INPUT = "ojvtpuvg";
        static void Main(string[] args)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                Console.WriteLine("Day 5");
                Console.WriteLine("Star 1");
                Console.WriteLine();

                long index = 0;

                string password = "";

                while (password.Length < 8)
                {
                    byte[] hashData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes($"{INPUT}{index++}"));

                    if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 0x10)
                    {
                        password += hashData[2].ToString("X2")[1];
                    }
                }

                Console.WriteLine($"The password is: {password.ToLowerInvariant()}");

                Console.WriteLine();
                Console.WriteLine("Star 2");
                Console.WriteLine();

                index = 0;
                char[] password2 = new char[8];
                HashSet<int> filledSlots = new HashSet<int>();

                do
                {
                    byte[] hashData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes($"{INPUT}{index++}"));

                    if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 0x10)
                    {
                        int position = hashData[2] % 0x10;
                        if (position < 8 && filledSlots.Add(position))
                        {
                            char character = hashData[3].ToString("X2")[0];
                            password2[position] = character;
                            filledSlots.Add(position);
                        }
                    }
                }
                while (filledSlots.Count < 8);

                Console.WriteLine($"The password is: {new string(password2).ToLowerInvariant()}");

                Console.WriteLine();
                Console.ReadKey();
            }
        }
    }
}
