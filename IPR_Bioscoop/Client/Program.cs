using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {

        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
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
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            write($"Login\r\n {username}");
        }

        private static void OnRead(IAsyncResult ar)
        {
            StringBuilder message = new StringBuilder();
            int numberOfBytesRead = 0;
            byte[] receiveBuffer = new byte[1024];

            do
            {
                numberOfBytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                message.AppendFormat("{0}", Encoding.UTF8.GetString(receiveBuffer, 0, numberOfBytesRead));
            } while (stream.DataAvailable);

            string response = message.ToString();
            Console.WriteLine(response);
        }

        private static void write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
    }
}
