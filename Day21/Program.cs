using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        private const string inputFile = @"../../../../input21.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 21");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            const string seedString = "abcdefgh";
            char[] scrambledString = seedString.ToArray();

            string[] lines = File.ReadAllLines(inputFile);

            List<Scrambler> scramblers = new List<Scrambler>();

            foreach (string line in lines)
            {
                if (line[0] == 'm')
                {
                    //*m*ove
                    scramblers.Add(new Mover(line));
                }
                else if (line[1] == 'e')
                {
                    //r*e*verse
                    scramblers.Add(new Reverser(line));
                }
                else if (line[5] == 'p')
                {
                    //swap *p*osition
                    scramblers.Add(new PositionSwapper(line));
                }
                else if (line[5] == 'l')
                {
                    //swap *l*etter
                    scramblers.Add(new LetterSwapper(line));
                }
                else if (line[7] == 'b')
                {
                    //rotate *b*ased on
                    scramblers.Add(new LetterRotator(line));
                }
                else
                {
                    //rotate Left/Right
                    scramblers.Add(new DirectionRotator(line));
                }
            }

            foreach (Scrambler scrambler in scramblers)
            {
                scrambler.Scramble(scrambledString);
            }

            Console.WriteLine($"Scrambled String: {new string(scrambledString)}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            const string inputPassword = "fbgdceah";
            scrambledString = inputPassword.ToArray();


            foreach (Scrambler scrambler in (scramblers as IEnumerable<Scrambler>).Reverse())
            {
                scrambler.Unscramble(scrambledString);
            }

            Console.WriteLine($"Unscrambled String: {new string(scrambledString)}");

            Console.WriteLine();
            Console.ReadKey();
        }

        abstract class Scrambler
        {
            public abstract void Scramble(char[] input);
            public abstract void Unscramble(char[] input);
        }

        class LetterSwapper : Scrambler
        {
            readonly char letter1;
            readonly char letter2;

            public LetterSwapper(string line)
            {
                letter1 = line[12];
                letter2 = line[26];
            }

            public override void Scramble(char[] input)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == letter1)
                    {
                        input[i] = letter2;
                    }
                    else if (input[i] == letter2)
                    {
                        input[i] = letter1;
                    }
                }
            }

            //Unscramble is the same
            public override void Unscramble(char[] input) => Scramble(input);
        }

        class PositionSwapper : Scrambler
        {
            readonly int position1;
            readonly int position2;

            public PositionSwapper(string line)
            {
                string[] splitLine = line.Split(' ');

                position1 = int.Parse(splitLine[2]);
                position2 = int.Parse(splitLine[5]);

                if (position1 > position2)
                {
                    (position1, position2) = (position2, position1);
                }
            }

            public override void Scramble(char[] input) =>
                (input[position1], input[position2]) = (input[position2], input[position1]);

            //Unscramble is the same
            public override void Unscramble(char[] input) => Scramble(input);
        }

        class DirectionRotator : Scrambler
        {
            readonly bool right;
            readonly int steps;

            public DirectionRotator(string line)
            {
                string[] splitLine = line.Split(' ');

                right = splitLine[1] == "right";
                steps = int.Parse(splitLine[2]);
            }

            public override void Scramble(char[] input)
            {
                int rotation = steps;
                if (!right)
                {
                    rotation = input.Length - steps;
                }

                char[] inputCopy = (char[])input.Clone();

                for (int i = 0; i < input.Length; i++)
                {
                    input[(i + rotation) % input.Length] = inputCopy[i];
                }
            }

            public override void Unscramble(char[] input)
            {
                int rotation = steps;
                if (right)
                {
                    rotation = input.Length - steps;
                }

                char[] inputCopy = (char[])input.Clone();

                for (int i = 0; i < input.Length; i++)
                {
                    input[(i + rotation) % input.Length] = inputCopy[i];
                }
            }
        }

        class LetterRotator : Scrambler
        {
            readonly char letter;

            private static readonly int[] reversalIndices = new int[]
            {
                7, 7, 2, 6, 1, 5, 0, 4
            };

            public LetterRotator(string line)
            {
                letter = line[35];
            }

            private void Rotate(char[] input, int rotation)
            {
                char[] inputCopy = (char[])input.Clone();

                for (int i = 0; i < input.Length; i++)
                {
                    input[(i + rotation) % input.Length] = inputCopy[i];
                }
            }

            public override void Scramble(char[] input)
            {
                int rotation;

                for (rotation = 0; rotation < input.Length; rotation++)
                {
                    if (input[rotation] == letter)
                    {
                        break;
                    }
                }

                if (rotation >= 4)
                {
                    rotation++;
                }

                rotation++;

                Rotate(input, rotation);
            }

            public override void Unscramble(char[] input)
            {
                int index;
                for (index = 0; index < input.Length; index++)
                {
                    if (input[index] == letter)
                    {
                        break;
                    }
                }

                int rotation = reversalIndices[index];

                Rotate(input, rotation);
            }
        }

        class Reverser : Scrambler
        {
            readonly int index1;
            readonly int index2;

            public Reverser(string line)
            {
                string[] splitLine = line.Split(' ');

                index1 = int.Parse(splitLine[2]);
                index2 = int.Parse(splitLine[4]);

                if (index1 > index2)
                {
                    (index1, index2) = (index2, index1);
                }
            }

            public override void Scramble(char[] input)
            {
                Stack<char> reverser = new Stack<char>();

                for (int i = index1; i <= index2; i++)
                {
                    reverser.Push(input[i]);
                }

                for (int i = index1; i <= index2; i++)
                {
                    input[i] = reverser.Pop();
                }
            }

            //Unscramble is the same
            public override void Unscramble(char[] input) => Scramble(input);
        }

        class Mover : Scrambler
        {
            readonly int index1;
            readonly int index2;

            public Mover(string line)
            {
                string[] splitLine = line.Split(' ');

                index1 = int.Parse(splitLine[2]);
                index2 = int.Parse(splitLine[5]);
            }

            private void Move(char[] input, int index1, int index2)
            {
                if (index1 == index2)
                {
                    return;
                }

                char c = input[index1];

                if (index1 < index2)
                {
                    for (int i = index1; i < index2; i++)
                    {
                        input[i] = input[i + 1];
                    }
                    input[index2] = c;
                }
                else
                {
                    for (int i = index1; i > index2; i--)
                    {
                        input[i] = input[i - 1];
                    }
                    input[index2] = c;
                }
            }

            public override void Scramble(char[] input) => Move(input, index1, index2);
            public override void Unscramble(char[] input) => Move(input, index2, index1);
        }
    }
}
