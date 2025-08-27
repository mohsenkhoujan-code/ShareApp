using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace TeamWorkOPC
{
    internal class Connection
    {
        
        public bool is_internetOn()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 1000); // Google's public DNS
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
            
        }
        public bool is_serverOn()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("http://127.0.0.1:8000/TeamWorkWebSite/ClientR0/conncetions/checkServerConnection10108000/", 1000); // Google's public DNS
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
