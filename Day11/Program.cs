using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 11");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            //Microchips are only safe on the same floor as any *other* RTG if it's connected
            //  to its own RTG.

            //Input
            //  Floor
            //  11  3  X
            //  10  2  CoM CuM RuM PlM
            //  01  1  CoG CuG RuG PlG
            //  00  0  PrG PrM

            //Elevator can only take 2 things at a time, and stops on each floor
            //The elevator will only function if it contains at least one Component.
            //Start on the 0th floor
            //Get everything to the 3rd floor

            //What is the minimum number of steps required to bring all items to the 4th floor?

            //Breadth-first tree search?  My options are always:
            //  Bring any one item up or down
            //  Bring any two items up or down

            //There are 10 items plus an elevator, with 4 floors each - 4M states
            //At 2 bits per item, I can encode the whole state in 24 bits,
            //  which fits in a 32-bit integer

            //But the states are Degenerate, so I will sort them by Chip floor then Gen Floor to
            //  maximize collisions

            {
                EquipmentState initialState = new EquipmentState(
                    elevator: 0,
                    pairs: new (int, int)[] { (2, 1), (2, 1), (2, 1), (0, 0), (2, 1) });

                EquipmentState targetState = new EquipmentState(
                    elevator: 3,
                    pairs: new (int, int)[] { (3, 3), (3, 3), (3, 3), (3, 3), (3, 3) });

                Dictionary<EquipmentState, int> movesDictionary = new Dictionary<EquipmentState, int>();
                HashSet<EquipmentState> invalidStates = new HashSet<EquipmentState>();
                Queue<EquipmentState> pendingEvaluation = new Queue<EquipmentState>();

                movesDictionary[initialState] = 0;
                movesDictionary[targetState] = int.MaxValue;
                pendingEvaluation.Enqueue(initialState);

                while (pendingEvaluation.Count > 0)
                {
                    EquipmentState currentState = pendingEvaluation.Dequeue();

                    int moveCount = movesDictionary[currentState];

                    if (moveCount >= movesDictionary[targetState])
                    {
                        //Too slow to matter
                        continue;
                    }

                    int nextMoveCount = moveCount + 1;
                    foreach (EquipmentState nextState in currentState.GetAdjacentStates(5))
                    {
                        if (invalidStates.Contains(nextState))
                        {
                            //Previously known invalid
                            continue;
                        }

                        if (!nextState.IsValid(5))
                        {
                            //Newly identified invalid
                            invalidStates.Add(nextState);
                            continue;
                        }

                        if (!movesDictionary.ContainsKey(nextState) || movesDictionary[nextState] > nextMoveCount)
                        {
                            //Update or add
                            movesDictionary[nextState] = nextMoveCount;
                            if (!pendingEvaluation.Contains(nextState) && nextState != targetState)
                            {
                                pendingEvaluation.Enqueue(nextState);
                            }
                        }
                    }
                }

                Console.WriteLine($"Minimum number of steps: {movesDictionary[targetState]}");
            }


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            {
                EquipmentState initialState = new EquipmentState(
                    elevator: 0,
                    pairs: new (int, int)[] { (2, 1), (2, 1), (2, 1), (0, 0), (2, 1), (0, 0), (0, 0) });

                EquipmentState targetState = new EquipmentState(
                    elevator: 3,
                    pairs: new (int, int)[] { (3, 3), (3, 3), (3, 3), (3, 3), (3, 3), (3, 3), (3, 3) });

                Dictionary<EquipmentState, int> movesDictionary = new Dictionary<EquipmentState, int>();
                HashSet<EquipmentState> invalidStates = new HashSet<EquipmentState>();
                Queue<EquipmentState> pendingEvaluation = new Queue<EquipmentState>();

                movesDictionary[initialState] = 0;
                movesDictionary[targetState] = int.MaxValue;
                pendingEvaluation.Enqueue(initialState);

                while (pendingEvaluation.Count > 0)
                {
                    EquipmentState currentState = pendingEvaluation.Dequeue();

                    int moveCount = movesDictionary[currentState];

                    if (moveCount >= movesDictionary[targetState])
                    {
                        //Too slow to matter
                        continue;
                    }

                    int nextMoveCount = moveCount + 1;
                    foreach (EquipmentState nextState in currentState.GetAdjacentStates(7))
                    {
                        if (invalidStates.Contains(nextState))
                        {
                            //Previously known invalid
                            continue;
                        }

                        if (!nextState.IsValid(7))
                        {
                            //Newly identified invalid
                            invalidStates.Add(nextState);
                            continue;
                        }

                        if (!movesDictionary.ContainsKey(nextState) || movesDictionary[nextState] > nextMoveCount)
                        {
                            //Update or add
                            movesDictionary[nextState] = nextMoveCount;
                            if (!pendingEvaluation.Contains(nextState) && nextState != targetState)
                            {
                                pendingEvaluation.Enqueue(nextState);
                            }
                        }
                    }
                }

                Console.WriteLine($"Minimum number of steps: {movesDictionary[targetState]}");
            }


            Console.WriteLine();
            Console.ReadKey();
        }

        public readonly struct EquipmentState
        {
            public readonly int state;

            public EquipmentState(
                int elevator,
                (int chip, int gen)[] pairs)
            {
                if (pairs.Length > 7)
                {
                    throw new ArgumentException($"Expected pairs to be length 7 or less.  Instead received: {pairs.Length}");
                }

                int state = elevator;

                for (int i = 0; i < pairs.Length; i++)
                {
                    state |= (pairs[i].chip | (pairs[i].gen << 2)) << (2 + 4 * i);
                }

                this.state = SortState(state);
            }

            public EquipmentState(int state)
            {
                //Should be already sorted
                this.state = state;
            }

            private static int SortState(int state)
            {
                //I know, bubble sort sucks, but this is fast to write, can operate inplace, and
                //  the sort should be pretty stable.
                bool changed;
                do
                {
                    changed = false;
                    int highChip = GetChip(state, 0);
                    for (int i = 0; i < 6; i++)
                    {
                        int lowChip = GetChip(state, i + 1);
                        if (highChip < lowChip)
                        {
                            state = SwapDown(state, i);
                            changed = true;
                        }
                        else if (highChip == lowChip && GetGen(state, i) < GetGen(state, i + 1))
                        {
                            state = SwapDown(state, i);
                            changed = true;
                        }

                        highChip = lowChip;
                    }

                }
                while (changed);

                return state;
            }

            private static int GetChip(int state, int chip) => (state >> (2 + 4 * chip)) & 0b11;
            private static int GetGen(int state, int gen) => (state >> (4 + 4 * gen)) & 0b11;
            private static int SwapDown(int state, int layer)
            {
                int maskA = 0b1111 << (2 + 4 * layer);
                int maskB = 0b1111 << (6 + 4 * layer);
                int compositeMask = maskA | maskB;

                int valueA = (state & maskA) << 4;
                int valueB = (state & maskB) >> 4;

                return (state & ~compositeMask) | valueA | valueB;
            }

            public int Elevator => state & 0b11;

            public int Chip(int chip) => (state >> (2 + 4 * chip)) & 0b11;
            public int Generator(int generator) => (state >> (4 + 4 * generator)) & 0b11;

            private static int MoveChip(int state, int chip, int newFloor)
            {
                int mask = 0b11 << (2 + 4 * chip);
                int value = newFloor << (2 + 4 * chip);

                return (state & ~mask) | value;
            }

            private static int MoveGenerator(int state, int generator, int newFloor)
            {
                int mask = 0b11 << (4 + 4 * generator);
                int value = newFloor << (4 + 4 * generator);

                return (state & ~mask) | value;
            }

            private static int MoveElevator(int state, int newFloor)
            {
                return (state & ~0b11) | newFloor;
            }

            public bool IsValid(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    //Check for chip irradiation
                    if (Chip(i) != Generator(i))
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }

                            if (Chip(i) == Generator(j))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            public static bool operator ==(in EquipmentState lhs, in EquipmentState rhs) =>
                lhs.state == rhs.state;
            public static bool operator !=(in EquipmentState lhs, in EquipmentState rhs) =>
                lhs.state != rhs.state;

            public override int GetHashCode() => state.GetHashCode();
            public override bool Equals(object obj) => (obj is EquipmentState) && Equals((EquipmentState)obj);
            public bool Equals(in EquipmentState other) => state == other.state;

            public IEnumerable<EquipmentState> GetAdjacentStates(int count)
            {
                int elevatorFloor = Elevator;

                bool up = elevatorFloor < 3;
                int upFloor = elevatorFloor + 1;

                bool down = elevatorFloor > 0;
                int downFloor = elevatorFloor - 1;

                List<int> chips = new List<int>();
                List<int> generators = new List<int>();
                HashSet<int> newStates = new HashSet<int>();


                for (int i = 0; i < count; i++)
                {
                    //Test and yield single moves and gather valid pieces
                    if (Chip(i) == elevatorFloor)
                    {
                        chips.Add(i);

                        if (up)
                        {
                            newStates.Add(SortState(
                                MoveElevator(MoveChip(state, i, upFloor),
                                    upFloor)));
                        }

                        if (down)
                        {
                            newStates.Add(SortState(
                                MoveElevator(MoveChip(state, i, downFloor),
                                    downFloor)));
                        }
                    }

                    if (Generator(i) == elevatorFloor)
                    {
                        generators.Add(i);

                        if (up)
                        {
                            newStates.Add(SortState(
                                MoveElevator(MoveGenerator(state, i, upFloor),
                                    upFloor)));
                        }

                        if (down)
                        {
                            newStates.Add(SortState(
                                MoveElevator(MoveGenerator(state, i, downFloor),
                                    downFloor)));
                        }
                    }
                }

                //Double Microchip moves
                if (chips.Count > 1)
                {
                    for (int i = 0; i < chips.Count - 1; i++)
                    {
                        for (int j = i + 1; j < chips.Count; j++)
                        {
                            if (up)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveChip(MoveChip(state, chips[j], upFloor),
                                            chips[i], upFloor),
                                        upFloor)));
                            }

                            if (down)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveChip(MoveChip(state, chips[j], downFloor),
                                            chips[i], downFloor),
                                        downFloor)));
                            }
                        }
                    }
                }

                //Double Generator moves
                if (generators.Count > 1)
                {
                    for (int i = 0; i < generators.Count - 1; i++)
                    {
                        for (int j = i + 1; j < generators.Count; j++)
                        {
                            if (up)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveGenerator(MoveGenerator(state, generators[j], upFloor),
                                            generators[i], upFloor),
                                        upFloor)));
                            }

                            if (down)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveGenerator(MoveGenerator(state, generators[j], downFloor),
                                            generators[i], downFloor),
                                        downFloor)));
                            }
                        }
                    }
                }

                //Chip And Generator Moves
                if (chips.Count > 0 && generators.Count > 0)
                {
                    for (int i = 0; i < chips.Count; i++)
                    {
                        if (generators.Contains(chips[i]))
                        {
                            if (up)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveChip(MoveGenerator(state, chips[i], upFloor),
                                            chips[i], upFloor),
                                        upFloor)));
                            }

                            if (down)
                            {
                                newStates.Add(SortState(
                                    MoveElevator(MoveChip(MoveGenerator(state, chips[i], downFloor),
                                            chips[i], downFloor),
                                        downFloor)));
                            }
                        }
                    }
                }

                foreach (int state in newStates)
                {
                    yield return new EquipmentState(state);
                }
            }
        }
    }
}
