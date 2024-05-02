using System.Net.Sockets;

namespace ChatServer
{
    internal class Client
    {
        public string Username { get; set; } = "ClientUsernamePlaceholder";
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with username: {Username}");
        }
    }
}
