﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace UnitTests
{
    [TestClass]
    class ServerTest
    {
        private NetworkStream stream;
        private TcpClient tcpClient;
        private Client.Client client;

        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        private int CompletedState = 0;
        private string sendCommand = "";
        private int messageLength;
        private List<Film> films;
        private bool requestDone;
        private bool orderSuccess;

        [TestMethod]
        public void testGetFilms()
        {
            Login();
            client.GetMovies();
        }

        public void Login()
        {
            try
            {
                this.tcpClient = new TcpClient("localhost", 14653);
                client = new Client.Client();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }

            this.stream = tcpClient.GetStream();

            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(OnRead), null);

            this.stream.Close();
            this.tcpClient.Close();
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
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            if (messageReady) handleData(messageData);
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);

        }

        private void handleData(string packetData)
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

        private void MoviesCommandHandling(JsonElement command)
        {
            string id = command.GetProperty("id").GetString().Substring(command.GetProperty("id").GetString().IndexOf("/") + 1);

            switch (id)
            {
                case "getResponse":
                    List<Film> newFilms = new List<Film>();
                    for (int i = 0; i < command.GetProperty("data").GetProperty("movies").GetArrayLength(); i++)
                    {
                        Film film = new Film("", 0, "", 0);     //Initialize with empty values for readability/space
                        film.Title = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Title").GetString();
                        film.Date = (command.GetProperty("data").GetProperty("movies")[i].GetProperty("Date").GetDateTime());
                        film.Length = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Length").GetInt32();
                        film.Description = command.GetProperty("data").GetProperty("movies")[i].GetProperty("Description").GetString();
                        film.review = command.GetProperty("data").GetProperty("movies")[i].GetProperty("review").GetInt32();
                        film.TicketsLeft = command.GetProperty("data").GetProperty("movies")[i].GetProperty("TicketsLeft").GetInt32(); ;
                        newFilms.Add(film);
                    }
                    films = newFilms;
                    requestDone = true;
                    break;
                case "orderResponse":
                    if (command.GetProperty("data").GetProperty("status").GetString() == "success")
                    {
                        orderSuccess = true;
                    }
                    else
                    {
                        orderSuccess = false;
                        requestDone = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
