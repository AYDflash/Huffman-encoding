﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace huffman
{
    class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        private int countSource; //кол-во символов в тексте
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();
        public Dictionary<char, string> Alphabet = new Dictionary<char, string>();
        public void Build(string source) 
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i])) 
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }
            // Заполняем список узлов
            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new Node { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1) 
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

                if (orderedNodes.Count >= 2) 
                {
                    //Берем 2 первых символа
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    //Создаем родительский узел c суммированием вероятностей
                    Node parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(string source)
        {
            countSource = source.Length;
            List<bool> encodedSource = new List<bool>();
                for (int i = 0; i < source.Length; i++)
                {
                    List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                    encodedSource.AddRange(encodedSymbol);
                    bool[] array = encodedSymbol.ToArray();
                    string _temp = "";
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (array[j] == true) _temp += 1 + "";
                        else _temp += 0 + "";
                    }
                    if (Alphabet.ContainsKey(source[i]) == false)
                        Alphabet.Add(source[i], _temp);
                }
                
            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            Node current = this.Root;
            string decoder = "";
            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (isLeaf(current)) 
                {
                    decoder += current.Symbol;
                    current = this.Root;
                }
            }

            return decoder;
        }

        public bool isLeaf(Node node) 
        {
            return (node.Left == null && node.Right == null);
        }

        public string GetNodes() 
        {
            string result = "";
            foreach (var sym in Alphabet) 
            {
                result += sym + " ";
            }

            return result;
        }

        public double getNValue() 
        {
            double result = Math.Ceiling(Math.Log(Frequencies.Count, 2));
            return result;
        }

        public double getNAvgValue() 
        {
            int[] countSymb = new int[Alphabet.Count];
            int j = 0;
            foreach (var sym in Alphabet)
            {
                countSymb[j] = sym.Value.Length;
                j++;
            }
 
            double result = 0;
            int i = 0;
            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                result += (1.0*symbol.Value / countSource) * countSymb[i];
                i++;
            }

            return result;
        }

        public double getMValue() 
        {
            return getNAvgValue() / getNValue();
        }

        public double getEnthrophy() 
        {
            double result = 0;

            foreach (KeyValuePair<char, int> symbol in Frequencies) 
            {
                double pi = (1.0 * symbol.Value / countSource);
                result -= (1.0 * symbol.Value / countSource) * Math.Ceiling(Math.Log(pi, 2));
            }

            return +result;
        }
    }
}
