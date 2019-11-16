using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


namespace Client
{
    class ClientCreator
    {
        private List<string> _serverAddresses;
        private List<CustomClient> _customClients;
        private CustomClient _customClient;

        public CustomClient CustomClient  => _customClient;

        public ClientCreator()
        {
            _customClients = new List<CustomClient>();
            _serverAddresses = new List<string>();
            GetLocalIpAdresses();
        }

        public void CreateClient()
        {
            while(true)
            {
                try
                {
                    Console.WriteLine("Choose number server :");
                    var numberAddres = Console.ReadLine();                    
                    _customClient = new CustomClient(_serverAddresses[Convert.ToInt32(numberAddres)], 2000, 2046);
                    Console.WriteLine("Clients create");
                    break;
                }
                catch
                {
                    Console.WriteLine("Error input");
                }
            }

        }

        public void CreateClients(bool custom)
        {
            foreach(string adress in _serverAddresses)
            {
                CustomClient customClient = new CustomClient(adress, 2000, 2046);
                _customClients.Add(customClient);
                customClient.SendMessageInNewThread(custom);
            }
            Console.WriteLine("Clients created");
        }

        private void GetLocalIpAdresses()
        {

            int i = -1;
            Console.WriteLine("List of available servers :");
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                Console.WriteLine("0) 127.0.0.1 ");
                _serverAddresses.Add("127.0.0.1");
                i = 0;
            }
           
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    i++;
                    Console.WriteLine("{0}) {1}", i, ip.ToString());
                    _serverAddresses.Add(ip.ToString());
                }
            }
        }

    }
}
