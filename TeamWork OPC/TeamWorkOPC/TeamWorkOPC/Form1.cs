using Microsoft.Win32;
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
    public partial class Form1 : Form
    {
        Form2 form2 = new Form2();
        Connection connection = new Connection();
        bool movement = false;
        int Xt = 0, Yt = 0;
        public string username = "",password = "";
        private int interval_int = 0;
        public Form1()
        {
            InitializeComponent();
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                  "TeamWorkOPC.exe", 11001, RegistryValueKind.DWord);

            form2.ShowDialog();
            label2.Text = form2.username;
            username = form2.username;
            password = form2.password;
            timer1.Start();

        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
           
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            movement = true;
            Xt = Cursor.Position.X - this.Left;
            Yt = Cursor.Position.Y - this.Top;

            timer1.Interval = 1;

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebSite website = new WebSite();
            website.WindowState = FormWindowState.Maximized;
            website.username = username;
            website.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form3 = new Form3();
            form3.username = username;
            form3.password = password;
            form3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 form4 = new Form4();
            form4.username = username;
            form4.password = password;
            form4.ShowDialog();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            movement = false;
            timer1.Interval = 100;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (movement)
            {
                this.Left = Cursor.Position.X-Xt;
                this.Top = Cursor.Position.Y-Yt;
                //MessageBox.Show(Cursor.Position.X.ToString() + " , " + Cursor.Position.Y.ToString());
                //movement = false;
            }
            if (!form2.keymatch)
            {
                this.Close();
            }
            if (interval_int++ >= 10)
            {
                if (!connection.is_internetOn())
                {
                    linkLabel1.Enabled = false;
                    linkLabel2.Enabled = false;
                    linkLabel3.Enabled = false;
                    label3.Text = "Offline";
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    linkLabel1.Enabled = true;
                    linkLabel2.Enabled = true;
                    linkLabel3.Enabled = true;
                    label3.Text = "Online";
                    label3.ForeColor = Color.ForestGreen;
                }/*
                if (!connection.is_serverOn())
                {
                    linkLabel1.Enabled = false;
                    linkLabel2.Enabled = false;
                    linkLabel3.Enabled = false;
                    label3.Text = "Server is off";
                    label3.ForeColor = Color.Red;
                }*/
                interval_int = 0;
            }
            
        }
    }
}
