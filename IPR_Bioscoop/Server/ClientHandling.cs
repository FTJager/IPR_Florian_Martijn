using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class ClientHandling
    {
        private TcpClient client;
        private NetworkStream stream;
        private Byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        private int messageLength;

        public string username { get; set; }

        public ClientHandling(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, OnRead, null);
        }

        private void DataHandling(string packetData)
        {

        }

        private void OnRead(IAsyncResult ar)
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
            catch (IOException)
            {
                Server.Disconnect(this);
                return;
            }

            if (messageReady) DataHandling(messageData);

            Console.WriteLine("Begin reading again");
            stream.BeginRead(buffer, 0, buffer.Length, OnRead, null);
        }

        internal void Write(string packet)
        {
            var writer = new BinaryWriter(stream);
            writer.Write(BitConverter.GetBytes(Encoding.ASCII.GetByteCount(packet)));       //Make length packet
            writer.Write(Encoding.ASCII.GetBytes(packet));
        }
    }
}
