using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 解析表达式
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ExpressionDecoder decoder = new ExpressionDecoder();
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                MessageBox.Show("表达式不能为空！");
                return;
            }
            try
            {
                double result = decoder.Decode(this.richTextBox1.Text);
                MessageBox.Show(result.ToString());
            }
            catch (Exception ee)
            {
                MessageBox.Show("错误" + ee.Message);
            }
        }
    }
}
