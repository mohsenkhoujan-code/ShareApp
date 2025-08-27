using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
namespace TeamWorkOPC
{
    
    public partial class Form3 : Form
    {
        Chats chats = new Chats();
        public string username,password;
        int timer_int = 0,Xrs=15,Xt=0,Yt=0;
        bool movement = false;
        public Form3()
        {
            InitializeComponent();
            Cursor = Cursors.WaitCursor;
            
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void Pong() {
            Mut mut = new Mut();
            bool res = await chats.Refresh(username, password, this, timer1, false, mut);
            if (res) {
     
                webBrowser1.Url = new Uri(chats.Chats_html());
                //Task.Delay(500);
                //webBrowser1.Document.Body.ScrollIntoView(false);
                Annocent.biom = 0;




            }
            timer_int = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (movement)
            {
                this.Left = Cursor.Position.X - Xt;
                this.Top = Cursor.Position.Y - Yt;
                //MessageBox.Show(Cursor.Position.X.ToString() + " , " + Cursor.Position.Y.ToString());
                //movement = false;
            }
            if (timer_int++ >= Xrs)
            {
                //WebBrowserScrollHelper.ScrollDown(webBrowser1); // Scroll down

                Pong();
                if (Annocent.biom++ <= 4) 
                { 
                    webBrowser1.Document.Window.ScrollTo(0, webBrowser1.Document.Body.ScrollRectangle.Height);
                }
                


            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            movement = false;
            timer1.Interval = 100;
            Xrs = 15;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            movement = true;
            Xt = Cursor.Position.X - this.Left;
            Yt = Cursor.Position.Y - this.Top;
            Xrs = 1500;
            timer1.Interval = 1;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            Annocent.isError = false;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                Mut mut = new Mut();
                label1.Text = "Chat Room - " + username;
                chats.Refresh(username, password, this, timer1, true, mut);
                Task.Delay(1000);
                webBrowser1.Url = new Uri(chats.Chats_html());
                Task.Delay(500);
                WebBrowserScrollHelper.ScrollDown(webBrowser1); // Scroll down
                //webBrowser1.Document.Window.ScrollTo(0, webBrowser1.Document.Body.ScrollRectangle.Height);
                //webBrowser1.Document.Body.ScrollIntoView(false); // Scroll down
                Cursor = Cursors.Default;
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string content=textBox1.Text;
            chats.Send_message(username,password,content,this, timer1);
            timer_int = 5;
            Cursor = Cursors.Default;
            textBox1.Text = "";

        }
    }
    public class WebBrowserScrollHelper
    {
        private const int WM_VSCROLL = 0x0115;
        private const int SB_PAGEDOWN = 3;
        private const int SB_PAGEUP = 2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public static void ScrollDown(WebBrowser webBrowser)
        {
            SendMessage(webBrowser.Handle, WM_VSCROLL, SB_PAGEDOWN, 0);
        }

        public static void ScrollUp(WebBrowser webBrowser)
        {
            SendMessage(webBrowser.Handle, WM_VSCROLL, SB_PAGEUP, 0);
        }
    }
}
