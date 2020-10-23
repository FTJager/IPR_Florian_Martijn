using JsonCommands;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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

        private static List<Film> films;
        private static GUI.App app;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client");
            Console.WriteLine("What is your username");
            username = Console.ReadLine();
            app = new GUI.App();
            app.Run();

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


            //TESTING
            GetMovies();
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
            System.Threading.Thread.Sleep(50);
            writer.Write(Encoding.ASCII.GetBytes(packet));
        }

        private static void handleData(string packetData)
        {
            string id = "";
            JsonElement jsonCommand = JsonDocument.Parse(packetData).RootElement;   //Converts packetData back into a JSON string
            id = jsonCommand.GetProperty("id").GetString().Substring(0, jsonCommand.GetProperty("id").GetString().IndexOf("/"));

            switch (id)
            {
                case "movies":
                    MoviesCommandHandling(jsonCommand);
                    break;
                default:
                    Console.WriteLine("Bad command");
                    break;
            }

        }

        private static void MoviesCommandHandling(JsonElement command)
        {
            string id = command.GetProperty("id").GetString().Substring(command.GetProperty("id").GetString().IndexOf("/") + 1);

            switch (id)
            {
                case "getResponse":
                    for(int i = 0; i < command.GetProperty("data").GetProperty("movies").GetArrayLength(); i++)
                    {
                        Film film = new Film("", 0, "", 0);     //Initialize with empty values for readability/space
                        film.Title = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Title").GetString();
                        for(int i2 = 0; i2 < command.GetProperty("data").GetProperty("movies")[i].GetProperty("Date").GetArrayLength(); i++)
                        {
                            film.Date.Add(command.GetProperty("data").GetProperty("movies")[i].GetProperty("Date")[i2].GetDateTime());
                        }
                        film.Length = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Length").GetInt32();
                        film.Description = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Description").GetString();
                        film.review = command.GetProperty("data").GetProperty("movies")[i].GetProperty("review").GetInt32();
                        film.TicketsLeft = command.GetProperty("data").GetProperty("movies")[i].GetProperty("TicketsLeft").GetInt32(); ;
                        films.Add(film);
                    }
                    break;
                case "orderResponse":
                    if (command.GetProperty("data").GetProperty("status").GetString() == "success")
                    {
                        //Movie successfully ordered
                    }
                    else
                    {
                        //Movie not successfully ordered (either movie doesn't exist or there were not enough tickets)
                    }
                    break;
                default:
                    break;
            }

        }

        public static void GetMovies()
        {
            write(Commands.GetMovies());
        }
    }
}
