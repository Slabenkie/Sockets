using System;
using System.IO;


namespace Server
{
    class CustomLogger
    {
        private StreamWriter _writer;
        private string _path;

        public CustomLogger()
        {
            _path = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".txt";
            Create();
        }

        private void Create()
        {
            try
            {
                if (!File.Exists(_path))
                {
                    _writer = File.CreateText(_path);
                    _writer.Close();
                }
                Console.WriteLine(_path);
            }
            catch
            {
                Console.WriteLine("Can't create file");
            }
        }

        public void WriteMessage(string message)
        {
            string date = DateTime.Now.ToString("HH:mm:ss");
            lock (this)
            {
                try
                {
                    _writer = File.AppendText(_path);
                    _writer.WriteLine("{0} {1}", date, message);
                    _writer.Close();
                }
                catch
                {
                    Console.WriteLine("Not write");
                }
            }
        }
    }
}
