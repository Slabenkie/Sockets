using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class ServerCollector
    {
        private List<CustomServer> _customServers;
        public ServerCollector()
        {
            _customServers = new List<CustomServer>();
            CreateCustomServers();
        }

        private void CreateCustomServers()
        {
            CustomLogger customLogger = new CustomLogger();
            // запросить у DNS-сервера IP-адрес, связанный с именем узла
            var host = Dns.GetHostEntry(Dns.GetHostName());
            // Пройдем по списку IP-адресов, связанных с узлом
            foreach (var ip in host.AddressList)
            {
                // если текущий IP-адрес версии IPv4, то выведем его 
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    CustomServer customServer = new CustomServer(ip.ToString(),2000, 2040, customLogger);
                    _customServers.Add(customServer);
                }
            }

            // доступно ли сетевое подключение
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return;
            }
            else
            {
                CustomServer localCustomServer = new CustomServer("127.0.0.1", 2000, 2040, customLogger);
                _customServers.Add(localCustomServer);
            }

        }
    }
}
