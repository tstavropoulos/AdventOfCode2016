using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Day17
{
    class Program
    {
        private const string input = "edjrjqaa";
        //private const string input = "ulqzkmiv";
        private static MD5 md5Hash;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 17");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            md5Hash = MD5.Create();
            TreeNode rootNode = new TreeNode(0, 0, null);

            //Hash encodes U,D,L,R
            //0-a: Locked
            //b-f: Unlocked

            bool hitVault = false;
            int layer = 0;

            TreeNode pathToVault = null;
            bool[] passable = new bool[4];

            while (!hitVault)
            {
                foreach (TreeNode node in rootNode.GetLayer(layer))
                {
                    GetHashPassable(passable, node.GetPassCode());
                    UpdatePassable(passable, node.x, node.y);

                    if (passable[0])
                    {
                        node.up = new TreeNode(node.x, node.y - 1, node);
                    }

                    if (passable[1])
                    {
                        node.dn = new TreeNode(node.x, node.y + 1, node);

                        if (node.x == 3 && node.y == 2)
                        {
                            hitVault = true;
                            pathToVault = node.dn;
                        }
                    }

                    if (passable[2])
                    {
                        node.le = new TreeNode(node.x - 1, node.y, node);
                    }

                    if (passable[3])
                    {
                        node.ri = new TreeNode(node.x + 1, node.y, node);

                        if (node.x == 2 && node.y == 3)
                        {
                            hitVault = true;
                            pathToVault = node.ri;
                        }
                    }
                }

                layer++;
            }

            Console.WriteLine($"Shortest path to Vault: {pathToVault.GetPath()}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int distanceToVault = 0;
            bool hasNodes = true;

            while (hasNodes)
            {
                hasNodes = false;
                foreach (TreeNode node in rootNode.GetLayer(layer))
                {
                    hasNodes = true;

                    GetHashPassable(passable, node.GetPassCode());
                    UpdatePassable(passable, node.x, node.y);

                    if (passable[0])
                    {
                        node.up = new TreeNode(node.x, node.y - 1, node);
                    }

                    if (passable[1])
                    {
                        if (node.x == 3 && node.y == 2)
                        {
                            distanceToVault = layer;
                        }
                        else
                        {
                            node.dn = new TreeNode(node.x, node.y + 1, node);

                        }
                    }

                    if (passable[2])
                    {
                        node.le = new TreeNode(node.x - 1, node.y, node);
                    }

                    if (passable[3])
                    {
                        if (node.x == 2 && node.y == 3)
                        {
                            distanceToVault = layer;
                        }
                        else
                        {
                            node.ri = new TreeNode(node.x + 1, node.y, node);
                        }
                    }
                }

                layer++;
            }

            Console.WriteLine($"Longest path to Vault: {distanceToVault + 1}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public class TreeNode
        {
            public readonly int x;
            public readonly int y;

            public readonly TreeNode parent;
            public readonly string lastStep;

            public TreeNode up = null;
            public TreeNode dn = null;
            public TreeNode le = null;
            public TreeNode ri = null;

            public string passcode = null;

            public TreeNode(int x, int y, TreeNode parent)
            {
                this.x = x;
                this.y = y;
                this.parent = parent;

                if (parent == null)
                {
                    lastStep = "";
                }
                else
                {
                    if (y == parent.y)
                    {
                        if (x > parent.x)
                        {
                            lastStep = "R";
                        }
                        else
                        {
                            lastStep = "L";
                        }
                    }
                    else
                    {
                        if (y > parent.y)
                        {
                            lastStep = "D";
                        }
                        else
                        {
                            lastStep = "U";
                        }
                    }
                }
            }

            public string GetPath()
            {
                StringBuilder stringBuilder = new StringBuilder();
                FillPath(stringBuilder);
                return stringBuilder.ToString();
            }

            public string GetPassCode()
            {
                StringBuilder stringBuilder = new StringBuilder();
                FillPassCode(stringBuilder);
                return stringBuilder.ToString();
            }

            private void FillPath(StringBuilder stringBuilder)
            {
                parent?.FillPath(stringBuilder);
                stringBuilder.Append(lastStep);
            }

            private void FillPassCode(StringBuilder stringBuilder)
            {
                if (passcode == null)
                {
                    if (parent == null)
                    {
                        stringBuilder.Append(input);
                    }
                    else
                    {
                        parent.FillPassCode(stringBuilder);
                    }

                    stringBuilder.Append(lastStep);

                    passcode = stringBuilder.ToString();
                }
                else
                {
                    stringBuilder.Append(passcode);
                }
            }

            public IEnumerable<TreeNode> GetChildren()
            {
                if (up != null)
                {
                    yield return up;
                }

                if (dn != null)
                {
                    yield return dn;
                }

                if (le != null)
                {
                    yield return le;
                }

                if (ri != null)
                {
                    yield return ri;
                }
            }

            public IEnumerable<TreeNode> GetLayer(int layersDeeper)
            {
                if (layersDeeper == 0)
                {
                    yield return this;
                }
                else if (layersDeeper == 1)
                {
                    foreach (TreeNode child in GetChildren())
                    {
                        yield return child;
                    }
                }
                else
                {
                    foreach (TreeNode child in GetChildren())
                    {
                        foreach (TreeNode targetNode in child.GetLayer(layersDeeper - 1))
                        {
                            yield return targetNode;
                        }
                    }
                }
            }
        }

        private static void GetHashPassable(bool[] passable, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (int i = 0; i < 2; i++)
            {
                string hex = data[i].ToString("x2");

                passable[2 * i] = TranslateChar(hex[0]);
                passable[2 * i + 1] = TranslateChar(hex[1]);
            }
        }

        private static bool TranslateChar(char c)
        {
            switch (c)
            {
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                    return true;

                default:
                    return false;
            }
        }

        private static void UpdatePassable(bool[] passable, int x, int y)
        {
            if (x == 0)
            {
                passable[2] = false;
            }
            else if (x == 3)
            {
                passable[3] = false;
            }

            if (y == 0)
            {
                passable[0] = false;
            }
            else if (y == 3)
            {
                passable[1] = false;
            }
        }
    }
}
