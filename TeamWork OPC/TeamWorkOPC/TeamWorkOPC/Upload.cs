using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamWorkOPC
{
    public partial class Upload : Form
    {
        string file_dir = ""; public string username = "", password="",format;
        int length=0;
        
        public Upload()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Zip file | *.zip |Winrar file | *.rar";
            openFileDialog1.Title = "Open file for upload";
            openFileDialog1.FileName = "";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK) {
                file_dir = openFileDialog1.FileName;
                button2.Enabled = true;
                textBox1.Text = openFileDialog1.FileName;
                length = File.ReadAllBytes(openFileDialog1.FileName).Length / 1024 / 1024;
                label2.Text = "Length: "+(length).ToString() + "MB";
                label3.Text = "Uploaded by: "+username;
                format = openFileDialog1.SafeFileName;

            }
        }
        
        private static async void upload_file(string file_dir,ProgressBar pb,string username,string caption,string password,string format)
        {
            pb.Value = 10;
            using (HttpClient client = new HttpClient()) {
                pb.Value = 20;
                using (var form = new MultipartFormDataContent()) 
                {
                    pb.Value = 30;
                    var username_c = new StringContent(username);
                    pb.Value = 35;
                    var password_c = new StringContent(password);
                    pb.Value = 40;
                    var caption_c = new StringContent(caption, System.Text.Encoding.UTF8);
                    pb.Value = 45;
                    byte[] file = File.ReadAllBytes(file_dir);
                    pb.Value = 50;
                    var fileContent = new ByteArrayContent(file);
                    pb.Value = 55;
                    var formatc = new StringContent(Path.GetExtension(file_dir));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");
                    pb.Value = 60;
                    form.Add(username_c, "username");
                    form.Add(password_c, "password");
                    form.Add(caption_c, "caption");
                    form.Add(fileContent, "file",Path.GetFileName(file_dir));
                    form.Add(formatc, "format");
                    pb.Value = 70;
                    DJ_URis djuri = new DJ_URis();
                    HttpResponseMessage res = await client.PostAsync(djuri.GetUploadUri(),form);
                    pb.Value = 80;
                    if (res.IsSuccessStatusCode)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var json = JsonSerializer.Deserialize<Responsecode>(await res.Content.ReadAsStringAsync());
                        pb.Value = 90;
                        pb.Value = 100;


                        if (json.code == "200")
                        {
                            MessageBox.Show("Successfully file uploaded","Info",MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
                        else if(json.code == "401")
                        {
                            MessageBox.Show($"401 authentication failed. {json.message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else if(json.code == "700")
                        {
                            MessageBox.Show("not responding");
                        }

                    }
                }
            
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(length < 29)
            {
                upload_file(file_dir, progressBar1, username, textBox2.Text, password,format);
                this.Close();
            }
            else
            {
                MessageBox.Show("The file size must be less than 30 megabytes.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
    }
}
