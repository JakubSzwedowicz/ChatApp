using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Program
    {
        private static List<Client> _users;
        private static TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                 Console.WriteLine("Client has connected!");

                // Broadcast the connection to everyone on the server
            }
        }
    }
}