using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.VisualStyles;

namespace TeamWorkOPC
{
    public partial class Form4 : Form
    {
        bool movement = false;
        int Xt = 0, Yt = 0,update=0,max_update=100,reload=0;
        public string username = "",password="";

        public Form4()
        {
            InitializeComponent();
            
            timer1.Start();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (update++ >=max_update) {
                explore_file(username, password, flowLayoutPanel1,cursor:this.Cursor,form:this);
                
            }
            if (movement)
            {
                this.Left = Cursor.Position.X - Xt;
                this.Top = Cursor.Position.Y - Yt;
                //MessageBox.Show(Cursor.Position.X.ToString() + " , " + Cursor.Position.Y.ToString());
                //movement = false;
            }

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            movement = true;
            Xt = Cursor.Position.X - this.Left;
            Yt = Cursor.Position.Y - this.Top;

            timer1.Interval = 1;
            max_update = 1000;

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label1.Text = "Files management - "+username;
            explore_file(username, password, flowLayoutPanel1, this.Cursor, this, true);

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            Annocent.isError= false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
        }
        
        private void DeleteFileEvents(object sender, EventArgs e, int id)
        {

        }

        private static async void explore_file(string username, string password, FlowLayoutPanel fl, Cursor cursor,Form4 form, bool free=false)
        {
            try
            {
                cursor = Cursors.WaitCursor;
                bool result = await DJ_URis.CheckDataBaseEvent(username, password, free);

                if (result)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            String[][] args =
                            {
                    new string[] {"username" ,username},
                    new string[] {"password" ,password},
                    };
                            DJ_URis djuri = new DJ_URis();

                            HttpResponseMessage res = await client.GetAsync(djuri.GetExploreUri(args));
                            if (res.IsSuccessStatusCode)
                            {
                                string StringContent = await res.Content.ReadAsStringAsync();
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true,
                                };
                                var json = JsonSerializer.Deserialize<ListF>(StringContent);
                                fl.Controls.Clear();
                                FontFamily family = new FontFamily("Consolas");

                                Font labelFont = new Font(family, (float)12.0, FontStyle.Bold);
                                Font labelFont2 = new Font(family, (float)10.0, FontStyle.Regular);
                                Font labelFont3 = new Font(family, (float)8.0, FontStyle.Regular);
                                Font labelFont4 = new Font(family, (float)8.0, FontStyle.Regular);
                                foreach (var items in json.content)
                                {
                                    FlowLayoutPanel flowLayoutPanel2 = new FlowLayoutPanel
                                    {
                                        Width = fl.Width - 24,
                                        Height = 100,
                                        BackColor = Color.MediumSlateBlue,

                                    };
                                    Label caption_label = new Label
                                    {
                                        Text = items.caption,
                                        Width = 180,
                                        Height = flowLayoutPanel2.Height,
                                        Font = labelFont,
                                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                                        RightToLeft = RightToLeft.Yes,



                                    };
                                    caption_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    Label username_label = new Label
                                    {
                                        Text = items.username,
                                        Width = 180,
                                        Height = flowLayoutPanel2.Height,
                                        Font = labelFont2,
                                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft

                                    };
                                    Label datetime_label = new Label
                                    {
                                        Text = items.datetime,
                                        Width = 200,
                                        Height = flowLayoutPanel2.Height,
                                        Font = labelFont4,
                                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft

                                    };
                                    Padding btnpad = new Padding(0, (flowLayoutPanel2.Height / 2) - 21, 0, 0);
                                    Button button_delete = new Button
                                    {
                                        Text = "Delete",
                                        Margin = btnpad,
                                        Width = 100,
                                        Height = 43,
                                        Font = labelFont3,

                                    };
                                    button_delete.Click += (sender, e) =>
                                    {
                                        int id = items.id;
                                        DeleteFile.FileId(id, username, password);
                                    };
                                    Button button_download = new Button
                                    {
                                        Text = "Download",
                                        Margin = btnpad,
                                        Width = 100,
                                        Height = 43,
                                        Font = labelFont3,

                                    };
                                    button_download.Click += (sender, e) =>
                                    {
                                        int id = items.id;
                                        Process process = new Process();
                                        process.StartInfo.FileName = "cmd.exe"; // Specify CMD
                                        process.StartInfo.Arguments = $"/c py Download.cps\\download.py {username} {password} {id}"; // CMD command
                                        process.StartInfo.RedirectStandardOutput = true; // Capture output
                                        process.StartInfo.UseShellExecute = false; // Required for redirection
                                        process.StartInfo.CreateNoWindow = true; // Run without CMD window

                                        // Start the process
                                        process.Start();

                                        // Read the output
                                        string output = process.StandardOutput.ReadToEnd();
                                        process.WaitForExit();

                                    };
                                    flowLayoutPanel2.Controls.Add(caption_label);
                                    flowLayoutPanel2.Controls.Add(username_label);
                                    flowLayoutPanel2.Controls.Add(datetime_label);
                                    flowLayoutPanel2.Controls.Add(button_delete);
                                    flowLayoutPanel2.Controls.Add(button_download);

                                    fl.Controls.Add(flowLayoutPanel2);


                                }



                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        form.Close();
                    }
                }
                cursor = Cursors.Default;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.Close();
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Upload upload = new Upload();
            upload.username = username;
            upload.password = password;
            upload.ShowDialog();
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            movement = false;
            timer1.Interval = 100;
            max_update = 10;
        }
    }
}
