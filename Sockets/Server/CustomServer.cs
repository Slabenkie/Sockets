using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class CustomServer
    {
        private int _startPort;
        private int _endPort;
        private TcpListener _tcpServer;
        private int _activePort;
        private string _address;
        private CustomLogger _customLogger;


        public CustomServer(string address, int startPort, int endPort, CustomLogger customLogger)
        {
            _customLogger = customLogger;
            _address = address;
            _startPort = startPort;
            _endPort = endPort;
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                for (int port = _startPort; port <= _endPort; port++)
                {
                    try
                    {
                        _tcpServer = new TcpListener(IPAddress.Parse(_address), port);
                        _tcpServer.Start();              
                        
                        Console.WriteLine("Ip {0} port {1} ready", _address, port);
                        _customLogger.WriteMessage($"Ip {_address} port {port} ready");
                        _activePort = port;

                        break;
                    }
                    catch
                    {
                        continue;
                    }

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка порта");
            }

            try
            {
                BeginListening();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void BeginListening()
        {
            Thread listenThread = new Thread(Listening);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void Listening()
        {
            TcpClient serverRequest = _tcpServer.AcceptTcpClient();
            Console.WriteLine("Listener ready!");
            NetworkStream stream = null;

            while (true)
            {
                lock(this)
                {
                    stream = serverRequest.GetStream();
                    var request = GetMessage(stream);
                    if (request == "Do you understand me?")
                        SendMessage(stream, "Yes, I do");
                    else
                        SendMessage(stream, "I'm, not understand");
                }
            }
        }

        private string GetMessage(NetworkStream stream)
        {
            
            byte[] data = new byte[256];
            var length = stream.Read(data, 0, data.Length);
            var request = Encoding.UTF8.GetString(data, 0, length);
            Console.WriteLine("Response from client : {0} : {1}", _address, request);
            _customLogger.WriteMessage($"Response from client : {_address} : {request}");
            return request;
        }

        private void SendMessage(NetworkStream stream,string answer)
        {
            var data = Encoding.UTF8.GetBytes(answer);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            _customLogger.WriteMessage($"Answer to client : {_address} : {answer}");
        }
    }

}
