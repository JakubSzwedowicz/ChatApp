using Protocol.Processing;
using Protocol.Processing.Packet;
using Protocol.Processing.Packet.Opcodes;
using System;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Client
    {
        public string Username { get; set; }
        public Guid Guid { get; set; }
        public TcpClient ClientSocket { get; set; }
        private readonly PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            _packetReader = new PacketReader(ClientSocket.GetStream());

            _packetReader.WaitForPacket();
            if (_packetReader.Opcode == Opcode.NewClient)
            {
                var newClientPacket = _packetReader.Deserialize<NewClientPacket>();
                Username = newClientPacket.Username;
                Guid = Guid.NewGuid();
                Console.WriteLine($"[{DateTime.Now}]: Client has connected with username: {Username}");
            }
            else
            {
                var time = DateTime.Now;
                throw new ArgumentException($"[{time}]: Client creation with illegal opcode: {_packetReader.Opcode}");
            }
        }

        public override string ToString()
        {
            return $"(Username: {Username}, Guid: {Guid})";
        }
    }
}
