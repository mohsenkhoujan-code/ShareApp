using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TeamWorkOPC
{
    class Annocent
    {
        public static int biom = 0;
        public static bool isError = false;
    }
    class Mut
    {
        public bool result;
        public static int reload = 0;
    }
    class DB_files_x
    {
        public static int round = 0;
        public int id { get; set; }
        public string username { get; set; }
        public string caption { get; set; }
        public string datetime { get; set; }
        public int Y = 0;
    }
    class DB_files
    {
        public int id { get; set; }
        public string username { get; set; }
        public string caption { get; set; }
        public string datetime { get; set; }
        
    }

    class ListF
    {
        public List<DB_files> content { get; set; }
    }
    class Responsecode
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    
    class DeleteFile
    {
        public async static void FileId(int id,string username,string password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string[][] args =
                    {
                    new string[] {
                        "username",username
                    },
                    new string[] {
                        "password",password
                    },
                    new string[] {
                        "id",id.ToString()
                    }
                };
                    DJ_URis djuri = new DJ_URis();
                    HttpResponseMessage res = await client.GetAsync(djuri.GetDeleteFileUri(args));
                    if (res.IsSuccessStatusCode)
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        switch (content)
                        {
                            case "1929231":
                                MessageBox.Show("Successfully deleted", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            default:
                                MessageBox.Show("Authenticade is fail", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }

    internal class DJ_URis
    {
        string protocol = "http",
               domain = "127.0.0.1",
               port = "8000",
               app_tw = "TeamWorkWebSite";
        bool isError = false;
        public static async Task<bool> CheckDataBaseEvent(string username, string password, bool free = false,bool chat=false)
        {
            try
            {
                if (free)
                {
                    return true;
                }
                using (HttpClient client = new HttpClient())
                {
                    DJ_URis djuri = new DJ_URis();
                    string[][] args = {
                    new string[] {"username", username },
                    new string[] {"password", password}
                };
                    if (chat)
                    {
                        HttpResponseMessage res = await client.GetAsync(djuri.GetDatabaseEventsUri(chat, args));
                        if (res.IsSuccessStatusCode)
                        {
                            String resString = await res.Content.ReadAsStringAsync();
                            if (resString == "1929231")
                            {
                                return true;
                            }
                            else if (resString != "123323")
                            {
                                System.Windows.Forms.MessageBox.Show("Authenticate is fail");
                            }
                        }
                    }
                    else
                    {
                        HttpResponseMessage res = await client.GetAsync(djuri.GetDatabaseEventsUri(args));
                        if (res.IsSuccessStatusCode)
                        {
                            String resString = await res.Content.ReadAsStringAsync();
                            if (resString == "1929231")
                            {
                                return true;
                            }
                            else if (resString != "123323")
                            {
                                System.Windows.Forms.MessageBox.Show("Authenticate is fail");
                            }
                        }
                    }

                }
            }
            catch (Exception ex) {
                if (!Annocent.isError)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Annocent.isError = true;
                }
            }
            return false;
            
        }
        public string GetRecieveUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/pa/recieve";
        }

        public string GetExploreUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/fl/Explore";
        }

        public string GetRefreshUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/pa/refresh";
        }
        public string GetCSRF_ViewUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/CSRF_PROTECTION/x";
        }

        public string GetUploadUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/fl/Upload";
        }

        public string GetDatabaseEventsUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/fl/DATABASE_EVENTS";
        }
        public string GetDatabaseEventsUri(bool chat)
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/fl/DATABASE_EVENTS_CHAT";
        }
        public string GetDeleteFileUri()
        {
            return $"{protocol}://{domain}:{port}/{app_tw}/ClientR0/Actions/fl/Delete";
        }


        public string GetRecieveUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs) {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length-1;
            getString.Remove(len, 1);
            return this.GetRecieveUri()+"?"+getString;
        }
        public string GetRefreshUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetRefreshUri() + "?" + getString;
        }
        public string GetCSRF_ViewUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetCSRF_ViewUri() + "?" + getString;
        }

        public string GetUploadUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetUploadUri() + "?" + getString;
        }
        public string GetExploreUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len,1);
            return this.GetExploreUri() + "?" + getString;
        }
        public string GetDatabaseEventsUri(string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetDatabaseEventsUri() + "?" + getString;
        }
        public string GetDatabaseEventsUri(bool chat,string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetDatabaseEventsUri(chat) + "?" + getString;
        }
        public string GetDeleteFileUri( string[][] GetMethodArgs)
        {
            string getString = "";
            foreach (string[] methodArgs in GetMethodArgs)
            {
                getString = getString + $"{methodArgs[0]}={methodArgs[1]}&";
            }
            int len = getString.Length - 1;
            getString.Remove(len, 1);
            return this.GetDeleteFileUri() + "?" + getString;
        }
    }
}
