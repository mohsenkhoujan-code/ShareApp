using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Windows;
using System.Threading;
using System.Windows.Forms;

namespace TeamWorkOPC
{
    public class Json_stat_none
    {
        public string item{get; set; }
    }
    public class Json_stat
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string datetime { get; set; }
        public string content { get; set; }
    }
    public class List_root
    {
        public List<Json_stat> json_stat { get; set; }
        public List<Json_stat_none> json_stat_none { get; set; }

    }
    internal class Chats
    {

        public string Chats_html()
        {
            return @"K:\.venv_OurAppStore\TeamWork OPC\TeamWorkOPC\TeamWorkOPC\Chat.html";
        }
        public async void Send_message(string username,string password,string content, Form3 thiz,System.Windows.Forms.Timer timer)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string[][] args = {
                        new string[]
                        {
                            "username",username
                        },
                        new string[]
                        {
                            "password",password
                        },
                        new string[]
                        {
                            "content",content
                        }

                    };
                        DJ_URis djuri = new DJ_URis();
                        HttpResponseMessage response = await client.GetAsync(djuri.GetRecieveUri(args));
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();

                        }

                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Connection is lost", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        timer.Stop();
                        thiz.Close();
                    }
                }
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            
        }
        public async Task<bool> Refresh(string username,string password,Form3 thiz, System.Windows.Forms.Timer timer,bool free,Mut mut)
        {
            try
            {
                bool result = await DJ_URis.CheckDataBaseEvent(username, password, free, true);
                if (result)
                {
                    //System.Windows.Forms.MessageBox.Show(result.ToString());
                    DJ_URis djuri = new DJ_URis();
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            string[][] args = {
                        new string[]
                        {
                            "username",username
                        },
                        new string[]
                        {
                            "password",password
                        }
                    };
                            HttpResponseMessage response = await client.GetAsync(djuri.GetRefreshUri(args));
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true,
                                };
                                var data = JsonSerializer.Deserialize<List_root>(responseBody);

                                if (data?.json_stat_none?[0].item == null || data.json_stat_none[0].item != "None")

                                {
                                    if (data?.json_stat_none?[0].item == null || data.json_stat_none[0].item != "UnR")
                                    {
                                        string xaml = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n   <link rel=\"stylesheet\" href=\"rss.css\"> <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Document</title>\r\n</head>\r\n<body>";
                                        foreach (var item in data.json_stat)
                                        {
                                            xaml += "<div class=\"card\">";
                                            xaml += "<div class=\"header\">";
                                            xaml += $"<span class=\"username\">{item.username}</span><span class=\"datetime\">{item.datetime}</span>";
                                            xaml += $"</div>";
                                            xaml += "<div class=\"body\">";
                                            xaml += $"<p><pre class=\"content\">{item.content}</pre></p>";
                                            xaml += "</div></div>";
                                        }

                                        File.WriteAllText(@"K:\.venv_OurAppStore\TeamWork OPC\TeamWorkOPC\TeamWorkOPC\Chat.html", xaml);
                                        return true;
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("The authentication has problem", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    File.WriteAllText(@"K:\.venv_OurAppStore\TeamWork OPC\TeamWorkOPC\TeamWorkOPC\Chat.html", "");

                                }



                            }

                        }
                        catch (Exception ex)
                        {
                            timer.Stop();
                            System.Windows.MessageBox.Show("Connection is lost", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            thiz.Close();
                        }

                    }
                }
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;

        }
    }
}
