using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Server
    {
        private static TcpListener listener;
        private static List<ClientHandling> clients = new List<ClientHandling>();       //List of connected clients
        private static List<Film> films;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Server started");
            Console.WriteLine("Loading movies...");
            //films = MakeFilmList();
            films = StreamReadWrite.Read();
            Console.WriteLine("Movies loaded");

            listener = new TcpListener(IPAddress.Any, 14653);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);     //Open the connection, server can now receive clients

            Console.ReadLine();     //Can be used to implement server-side commands if needed
        }

        /// <summary>
        /// Method handles connecting clients
        /// </summary>
        /// <param name="ar">connected TCP client</param>
        private static void OnConnect(IAsyncResult ar)
        {
            var client = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {client.Client.RemoteEndPoint}");
            clients.Add(new ClientHandling(client, films));
            listener.BeginAcceptTcpClient(OnConnect, null);
        }

        //Creates some test movies, only gets called when file has to be remade
        internal static List<Film> MakeFilmList()
        {
            List<Film> films = new List<Film>();    //Use FileIO to make this list
            films.Add(new Film("Tester", 120, "Fuckin top tier movie right there", 100));
            films.Add(new Film("Tester2", 80, "Shit movie but at least it short lmao", 300));
            films[0].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            films[1].Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            StreamReadWrite.Write(films);
            //films = StreamReadWrite.Read();

            Console.WriteLine("movies saved");

            return films;
        }

        /// <summary>
        /// Sends a message to all connected clients
        /// </summary>
        /// <param name="packet"></param>
        internal static void Broadcast(string packet)
        {
            foreach (var client in clients)
            {
                client.Write(packet);
            }
        }

        /// <summary>
        /// Sends a message to a specific user, indicated by their username
        /// </summary>
        /// <param name="user">User's username or identifier</param>
        /// <param name="packet">Data to send to user</param>
        internal static void SendToUser(string user, string packet) //Send data to a specific user. user would be the bike name
        {
            foreach (var client in clients)
            {
                if (client.username == user) client.Write(packet);  //Send the packet if usernames match. Regex may be useful
            }
        }

        /// <summary>
        /// Method for when a client disconnects. Removes the client from the list of connected clients.
        /// </summary>
        /// <param name="client">disconnected client/clienthandler</param>
        internal static void Disconnect(ClientHandling client)
        {
            clients.Remove(client);
            Console.WriteLine($"Client {client.username} has been disconnected");
        }

        /// <summary>
        /// Updates the movies and saves the changes in the files
        /// </summary>
        /// <param name="newfilms">Updated list of movies</param>
        public static void updateFilms(List<Film> newfilms)
        {
            films = newfilms;
            StreamReadWrite.Write(films);
        }
    }
}
