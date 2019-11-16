using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

namespace Client
{
    class CustomClient
    {
        private TcpClient _tcpClient;
        private int _startPort;
        private int _endPort;
        private string _activeIp;
        private int _activePort;

        public CustomClient(string ipAdress, int startPort, int endPort)
        {
            _startPort = startPort;
            _endPort =  endPort;
            _activeIp = ipAdress;
            StartClient();
        }

        private void StartClient()
        {
            try
            {
                Console.WriteLine("Trying to connect to the server....");
                for (int port = _startPort; port <= _endPort; port++)
                {
                    try
                    {
                        _tcpClient = new TcpClient(_activeIp, port); //169.254.205.140
                        _activePort = port;
                        Console.WriteLine("Successfully!");
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Server not found :(");
            }
        }

        public void SendMessage()
        {
            try
            {
                while(true)
                {
                    Console.WriteLine("Enter your message :");
                    SendMessageFromServer(Console.ReadLine());
                    GetMessageFromServer();
                }
            }
            catch
            {
                Console.WriteLine("Message not send");
            }
        }

        public void SendMessageInNewThread(bool custom)
        {
            if(!custom)
            {
                Thread sendMes = new Thread(SendMessageThread);
                sendMes.IsBackground = true;
                sendMes.Start();
            }
            else
            {
                Thread sendMes = new Thread(SendMessageThreadCustom);
                sendMes.IsBackground = true;
                sendMes.Start();
            }
        }

        private void SendMessageThread()
        {
            try
            {
                SendMessageFromServer();
                while (true)
                {
                    lock (this)
                    {
                        GetMessageFromServer();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Message not send");
            }
        }


        private void SendMessageThreadCustom()
        {
            try
            {
                PhraseCollector phraseCollector = new PhraseCollector();               
                while (true)
                {
                    lock (this)
                    {
                        try
                        {
                            SendMessageFromServer(phraseCollector.GetPhrase());
                            GetMessageFromServer();
                        }
                        catch
                        {
                            return;
                        }                                             
                    }
                }
            }
            catch
            {
                Console.WriteLine("Message not send");
            }
        }


        private void GetMessageFromServer()
        {
            NetworkStream stream = _tcpClient.GetStream();
            byte[] bytes = new byte[256];
            var length = stream.Read(bytes, 0, bytes.Length);
            var request = Encoding.UTF8.GetString(bytes, 0, length);
            Console.WriteLine("Response from server IP : {0} port : {1} : {2} : {3}", _activeIp, _activePort, GetComputerName(), request);
        }

        private void SendMessageFromServer(string message = "Do you understand me?")
        {
            NetworkStream stream = _tcpClient.GetStream();
            var data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            Console.WriteLine("Sent a message :{0}", message);
        }

        private string GetComputerName()
        {
            var host = Dns.GetHostEntry(IPAddress.Parse(_activeIp));
            return host.HostName;
        }
    }
}
