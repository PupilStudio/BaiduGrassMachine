using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrassMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cnt;
            if (!int.TryParse(textBox2.Text, out cnt) || cnt < 2 || cnt > 500)
            {
                MessageBox.Show("翻译次数无效。翻译次数应当是一个整数，并且介于2至500之间。");
                return;
            }
            //textBox3.Text = TranslateHelper.Grass(textBox1.Text, cnt);
            textBox3.Text = "生草中, 请稍等...";
            textBox3.Text = TranslateHelper.Grass(textBox1.Text, cnt, textBox3);
        }
    }
}
