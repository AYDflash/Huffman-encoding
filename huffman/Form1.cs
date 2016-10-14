using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace huffman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputText = richTextBox1.Text;
            if (inputText != "")
            {
                //Построить дерево Хаффмана
                HuffmanTree huffman = new HuffmanTree();
                huffman.Build(inputText);

                //Закодировать
                BitArray encoded = huffman.Encode(inputText);

                foreach (bool bit in encoded)
                {
                    if (bit) richTextBox2.Text += "1";
                    else richTextBox2.Text += "0";
                }

                string decoded = huffman.Decode(encoded);

                richTextBox2.Text += decoded;
            }
            else 
            {
                MessageBox.Show("Поле ввода пустое. Пожалуйста, введите текст.");
                richTextBox1.Focus();
            }
        }
    }
}
