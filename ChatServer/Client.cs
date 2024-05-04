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

                Task.Run(() => Process());
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

        public void Process()
        {
            while (true)
            {
                try
                {
                    _packetReader.WaitForPacket();
                    var opcode = _packetReader.Opcode;
                    switch (opcode)
                    {
                        case Opcode.SendMessage:
                            var sendMessagePacket = _packetReader.Deserialize<SendMessagePacket>();
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {sendMessagePacket.message}");
                            Console.WriteLine($"[{DateTime.Now}]: User: {this} has sent the message: {sendMessagePacket.message}");
                            break;
                    }

                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"[{DateTime.Now}]: User: {this} disconnected!");
                    Program.BroadcastDisconnect(Guid);
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
