using JsonCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    class ClientHandling
    {
        private TcpClient client;
        private NetworkStream stream;
        private Byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        private int messageLength;
        private List<Film> films;

        public string username { get; set; }

        public ClientHandling(TcpClient client, List<Film> films)
        {
            this.client = client;
            stream = client.GetStream();
            this.films = films;

            stream.BeginRead(buffer, 0, buffer.Length, OnRead, null);
        }

        private void DataHandling(string packetData)
        {
            string id = "";
            JsonElement jsonCommand = JsonDocument.Parse(packetData).RootElement;   //Converts packetData back into a JSON string
            id = jsonCommand.GetProperty("id").GetString().Substring(0, jsonCommand.GetProperty("id").GetString().IndexOf("/"));

            switch (id)
            {
                case "login":
                    LoginCommandHandling(jsonCommand);
                    break;
                case "movies":
                    MoviesCommandHandling(jsonCommand);
                    break;
                default:
                    Console.WriteLine("Bad command");
                    break;
            }
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

        //COMMAND HANDLING

        internal void LoginCommandHandling(JsonElement command)
        {
            username = command.GetProperty("data").GetProperty("username").GetString();
        }

        internal void MoviesCommandHandling(JsonElement command)
        {
            string id = command.GetProperty("id").GetString().Substring(command.GetProperty("id").GetString().IndexOf("/") + 1);    //Only gets the second part of the id

            switch (id)
            {
                case "get":     //Retrieve the list of movies
                    Console.WriteLine("movies/get command received");
                    Write(JsonCommands.Commands.GetMoviesResponse(films));
                    break;
                case "orderticket":     //Reduce the tickets left for a movie by a certain amount
                    bool success = false;
                    foreach(Film film in films)
                    {
                        if (film.Title == command.GetProperty("data").GetProperty("title").GetString()) //Check movie availabiliyu
                        {
                            if(command.GetProperty("data").GetProperty("amount").GetInt32() >= film.TicketsLeft)    //Check amount of tickets left
                            {
                                film.TicketsLeft -= command.GetProperty("data").GetProperty("amount").GetInt32();   //Remove tickets
                                Server.updateFilms(films);
                                success = true;  
                            }
                            else
                            {
                                success = false;
                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    Write(Commands.OrderResponse(success));
                    break;
                default:
                    Console.WriteLine("invalid movie command");
                    break;
            }
        }
    }
}
