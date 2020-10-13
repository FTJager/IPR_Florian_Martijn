using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    class Program
    {

        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
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
            int receivedBytes = stream.EndRead(ar);
            string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            totalBuffer += receivedText;

            while (totalBuffer.Contains("\r\n\r\n"))
            {
                string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
                totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
                string[] packetData = Regex.Split(packet, "\r\n");
                handleData(packetData);
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private static void write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }

        private static void handleData(string[] packetData)
        {
            Console.WriteLine($"Packet ontvangen: {packetData[0]}");

            switch (packetData[0])
            {
                case "login":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Logged in!");
                        loggedIn = true;
                    }
                    else
                        Console.WriteLine(packetData[1]);
                    break;
                case "chat":
                    Console.WriteLine($"Chat ontvangen: '{packetData[1]}'");
                    break;
            }

        }
    }
}
