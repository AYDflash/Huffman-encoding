using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman
{
    class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

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
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
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
    }
}
