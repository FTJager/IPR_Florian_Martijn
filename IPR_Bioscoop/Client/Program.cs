using System;
using System.IO;
using System.Net.Sockets;

namespace Client
{
    class Program
    {

        private static TcpClient client;
        private static NetworkStream stream;
        private static string username;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client");
            Console.WriteLine("What is your username");
            username = Console.ReadLine();

            client = new TcpClient();
            client.BeginConnect("localhost", 84573, new AsyncCallback(OnConnect), null);
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            Console.WriteLine("Verbonden");
        }
    }
}
