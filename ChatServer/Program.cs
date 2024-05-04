using Protocol.Processing;
using Protocol.Processing.Packet;
using Protocol.Utils;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    internal class Program
    {
        private static List<Client> _users = new();
        private static TcpListener _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);

        static void Main(string[] args)
        {
            _listener.Start();

            while (true)
            {
                try
                {
                    var client = new Client(_listener.AcceptTcpClient());
                    _users.Add(client);
                    Console.WriteLine("Client has connected!");
                    BroadcastConnection();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        static void BroadcastConnection()
        {
            // TODO: Server should distinguish between first-time connected user and the ones who only need information about a single new user.


            // These should be DTOs and they should be crated in mappers but whatever. The purpose is to learn WPF
            var broadcastUsersPacket = new BroadcastUsersPacket();
            _users.ForEach(x => { broadcastUsersPacket.Users.Add(new BroadcastUsersPacket.User(x.Username, x.Guid.ToString())); });

            var packetWriter = new PacketWriter();
            packetWriter.Serialize(broadcastUsersPacket);
            foreach (var user in _users)
            {
                Console.WriteLine($"[{DateTime.Now}]: Sending {broadcastUsersPacket.Users.Count} users to user: {user}");
                //Console.WriteLine($"[DEBUG] [{DateTime.Now}]: packet as binary array: {Printing.PrintByteArray(packetWriter.GetPacketBytes())}");
                user.ClientSocket.Client.Send(packetWriter.GetPacketBytes());
            }
        }

        public static void BroadcastMessage(string message)
        {
            var sendMessagePacket = new SendMessagePacket(message);
            BroadcastMessage(sendMessagePacket);
        }

        public static void BroadcastMessage(SendMessagePacket message)
        {
            foreach (var user in _users)
            {
                var packetWriter = new PacketWriter();
                packetWriter.Serialize(message);
                user.ClientSocket.Client.Send(packetWriter.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(Guid guid)
        {
            var disconnectedUser = _users.Find(user => user.Guid == guid);
            
            if (disconnectedUser != null && _users.Remove(disconnectedUser))
            {
                var packetWriter = new PacketWriter();
                var userDisconnectedPacket = new UserDisconnectedPacket(guid.ToString());
                packetWriter.Serialize(userDisconnectedPacket);

                foreach (var user in _users)
                {
                    user.ClientSocket.Client.Send(packetWriter.GetPacketBytes());
                }

                BroadcastMessage($"[{DateTime.Now}]: User: {disconnectedUser} disconnected!");
            }
        }
    }
}