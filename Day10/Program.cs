using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        private const string inputFile = @"../../../../input10.txt";
        private static Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
        private static Dictionary<int, OutputContainer> outputs = new Dictionary<int, OutputContainer>();
        public static Bot q1target = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 10");
            Console.WriteLine("Star 1");
            Console.WriteLine();
            //Each bot only proceeds when it has 2 microchips
            //  when it does, it gives each one to a different bot
            //  or puts it in the marked "output" bin
            //Sometimes, bots take microchips from "input" bins

            //Each Microchip contains a single number
            //The bots decide what to do with each chip

            //What is the number of the bot that is responsible for comparing
            //value-61 chips with value-17 chips?

            string[] lines = File.ReadAllLines(inputFile);

            List<(int bot, int value)> injections = new List<(int, int)>();

            foreach (string line in lines)
            {
                if (line.StartsWith("bot"))
                {
                    string[] splitLine = line.Split(' ');
                    int id = int.Parse(splitLine[1]);
                    int lowId = int.Parse(splitLine[6]);
                    int highId = int.Parse(splitLine[11]);

                    Bot targetBot = GetBot(id);

                    ChipContainer lowReceiver;
                    ChipContainer highReceiver;
                    if (splitLine[5] == "bot")
                    {
                        lowReceiver = GetBot(lowId);
                    }
                    else
                    {
                        lowReceiver = GetOutput(lowId);
                    }

                    if (splitLine[10] == "bot")
                    {
                        highReceiver = GetBot(highId);
                    }
                    else
                    {
                        highReceiver = GetOutput(highId);
                    }

                    targetBot.SetReceivers(highReceiver, lowReceiver);
                }
                else if (line.StartsWith("value"))
                {
                    string[] splitLine = line.Split(' ');
                    injections.Add((int.Parse(splitLine[5]), int.Parse(splitLine[1])));
                }
                else
                {
                    throw new Exception($"Unexpected Line: {line}");
                }
            }

            foreach ((int bot, int value) in injections)
            {
                GetBot(bot).ReceiveChip(value);
            }

            if (q1target == null)
            {
                throw new Exception($"Target comparison never occurred");
            }

            Console.WriteLine($"Bot {q1target.id} compares values 61 and 17");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //Multiply together outputs in 0, 1, and 2

            //I expected the second part to involve a lot more simulation -
            //  obviously now I see I should have just built a tree.

            int product = 1;

            for (int i = 0; i < 3; i++)
            {
                OutputContainer container = GetOutput(i);
                if (container.output.Count != 1)
                {
                    throw new Exception($"Container {container.id} has {container.output.Count} outputs");
                }

                product *= container.output[0];
            }

            Console.WriteLine($"Product of containers 0, 1, and 2 is: {product}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private static Bot GetBot(int id)
        {
            if (bots.ContainsKey(id))
            {
                return bots[id];
            }

            Bot tempBot = new Bot(id);
            bots.Add(id, tempBot);

            return tempBot;
        }

        private static OutputContainer GetOutput(int id)
        {
            if (outputs.ContainsKey(id))
            {
                return outputs[id];
            }

            OutputContainer tempOutput = new OutputContainer(id);
            outputs.Add(id, tempOutput);

            return tempOutput;
        }
    }


    public abstract class ChipContainer
    {
        public abstract void ReceiveChip(int chipID);
    }

    public class Bot : ChipContainer
    {
        public readonly int id;

        public int chipA = 0;

        public ChipContainer highReceiver;
        public ChipContainer lowReceiver;

        public Bot(int id)
        {
            this.id = id;
        }

        public void SetReceivers(ChipContainer highReceiver, ChipContainer lowReceiver)
        {
            if (this.highReceiver != null)
            {
                throw new Exception($"Bot {id} already set with receivers");
            }

            this.highReceiver = highReceiver;
            this.lowReceiver = lowReceiver;
        }

        public override void ReceiveChip(int chipID)
        {
            if (chipA == 0)
            {
                chipA = chipID;
            }
            else
            {
                int lowChip = Math.Min(chipA, chipID);
                int highChip = Math.Max(chipA, chipID);

                if (lowChip == 17 && highChip == 61 && Program.q1target == null)
                {
                    Program.q1target = this;
                }

                chipA = 0;

                if (highReceiver == null)
                {
                    throw new Exception($"Bot {id} receiving chip never initialized");
                }

                highReceiver.ReceiveChip(highChip);
                lowReceiver.ReceiveChip(lowChip);
            }
        }
    }

    public class OutputContainer : ChipContainer
    {
        public readonly List<int> output = new List<int>();

        public readonly int id;

        public OutputContainer(int id)
        {
            this.id = id;
        }

        public override void ReceiveChip(int chipID)
        {
            output.Add(chipID);
        }
    }
}
