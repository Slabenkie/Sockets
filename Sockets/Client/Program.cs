using System;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose task");
            var task = Console.ReadLine();
            ClientCreator clientCreator = new ClientCreator();
            switch(task)
            {
                case "1":
                    clientCreator.CreateClient();
                    clientCreator.CustomClient.SendMessage();
                    break;
                case "2":
                    clientCreator.CreateClients(false);
                    break;
                case "3":
                    clientCreator.CreateClients(true);
                    break;
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
