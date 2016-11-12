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
            richTextBox2.Clear();
            richTextBox3.Clear();
           try{
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

                        richTextBox2.Text += (bit ? 1 : 0);
                    }

                    richTextBox3.Text += huffman.GetNodes();
                    nLabel.Text = string.Format("n = {0:f3}", huffman.getNValue());
                    nAvgLabel.Text = string.Format("n_сред = {0:f3}", huffman.getNAvgValue());
                    mLabel.Text = string.Format("M = {0:f3}, {1}% сжатия", huffman.getMValue(), (int)((1 - huffman.getMValue()) * 100));
                    HALable.Text = string.Format("H(A) = {0:f3}", huffman.getEnthrophy());
                }
                else
                {
                    MessageBox.Show("Поле ввода пустое. Пожалуйста, введите текст.");
                    richTextBox1.Focus();
                }
            }
            catch 
            {
                MessageBox.Show("Нет смысла кодировать один символ! Введите нормальный текст.");
                richTextBox1.Focus();
            }
        }
    }
}
