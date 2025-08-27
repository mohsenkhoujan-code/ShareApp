using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamWorkOPC
{
    public partial class Form2 : Form
    {
        public bool keymatch = false;
        public string username = "",password = "";
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { 
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '•';

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "mahdi_azk" && textBox2.Text == "12345678qwerty")||(textBox1.Text
                == "yasin" && textBox2.Text == "12345678qwerty")||(textBox1.Text=="mohsen"
                && textBox2.Text == "12345678qwerty"))
            {
                keymatch = true;
                username = textBox1.Text;
                password = textBox2.Text;
                this.Close();

            }
            else
            {
                this.Close();
            }
        }
    }
}
