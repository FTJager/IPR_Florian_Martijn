using Server.Film;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server.Film;

namespace Server
{
    class Server
    {
        private static TcpListener listener;
        private static List<ClientHandling> clients = new List<ClientHandling>();       //List of connected clients
        static void Main(string[] args)
        {
            Console.WriteLine("Server started");

            listener = new TcpListener(IPAddress.Any, 84573);
            listener.Start();
            listener.BeginAcceptTcpClient(OnConnect, null);     //Open the connection, server can now receive clients

            Console.ReadLine();     //Can be used to implement server-side commands if needed
        }

        private static void OnConnect(IAsyncResult ar)
        {
            var client = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {client.Client.RemoteEndPoint}");
            clients.Add(new ClientHandling(client));
            listener.BeginAcceptTcpClient(OnConnect, null);
        }

        internal static List<> MakeFilmList()
        {
            List<Film> films = new List<Film>;
            
            return films;
        }

        internal static void Broadcast(string packet)
        {
            foreach (var client in clients)
            {
                client.Write(packet);
            }
        }

        internal static void SendToUser(string user, string packet) //Send data to a specific user. user would be the bike name
        {
            foreach (var client in clients)
            {
                if (client.username == user) client.Write(packet);  //Send the packet if usernames match. Regex may be useful
            }
        }

        internal static void Disconnect(ClientHandling client)
        {
            clients.Remove(client);
            Console.WriteLine($"Client {client.username} has been disconnected");
        }
    }
}
