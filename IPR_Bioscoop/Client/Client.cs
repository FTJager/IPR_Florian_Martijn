using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    class Client
    {

        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static int messageLength;
        private static string username;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client");
            Console.WriteLine("What is your username");
            username = Console.ReadLine();

            client = new TcpClient();
            client.BeginConnect("localhost", 14653, new AsyncCallback(OnConnect), null);

            Console.ReadLine();
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            Console.WriteLine("Verbonden");
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            write(JsonCommands.Commands.Login(username));
        }

        private static void OnRead(IAsyncResult ar)
        {
            string messageData = "";
            Boolean messageReady = false;
            try
            {
                int receivedBytes = stream.EndRead(ar);
                if (receivedBytes == 4)
                {
                    //Do something with length byte
                    messageLength = BitConverter.ToInt32(buffer, 0);
                    Console.WriteLine("message length by length byte: {0}", messageLength);
                }
                else
                {
                    string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                    totalBuffer += receivedText;
                    Console.WriteLine("--New message. Length: {0} bytes. Text: {1}", receivedBytes, receivedText);

                    messageData = receivedText;

                    if (totalBuffer.Length == messageLength)
                    {
                        messageData = totalBuffer;
                        messageReady = true;
                        totalBuffer = totalBuffer.Remove(0);
                        Console.WriteLine("Message complete: {0}", messageData);
                        messageLength = 0;
                    }
                }
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }

            if (messageReady) handleData(messageData);
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);

        }

        private static void write(string packet)
        {
            var writer = new BinaryWriter(stream);
            writer.Write(BitConverter.GetBytes(Encoding.ASCII.GetByteCount(packet))); //Make length packet
            writer.Write(Encoding.ASCII.GetBytes(packet));
        }

        private static void handleData(string packetData)
        {
            

        }
    }
}
