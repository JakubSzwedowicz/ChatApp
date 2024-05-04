using Protocol.Processing;
using Protocol.Processing.Packet.Opcodes;
using Protocol.Processing.Packet;
using System;
using System.Net.Sockets;
using System.Net;
using Protocol.Processing.Packet.Serialization;

namespace ChatClient.Net
{
    internal class Server
    {
        private readonly TcpClient _client = new();
        public PacketReader? PacketReader { get; set; }

        public event Action? connectedEvent;
        public event Action? messageReceivedEvent;
        public event Action? userDisconnectedEvent;

        private string Username { get; set; } = string.Empty;


        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());
                SendUsernameToServer(username);
                ReadPackets();
            }
        }

        public void SendMessageToServer(string message)
        {
            var newMessage = new SendMessagePacket(message);
            SendPacket(newMessage);
        }

        private void SendUsernameToServer(string username)
        {
            var newClientPacket = new NewClientPacket(username);
            SendPacket(newClientPacket);
            Username = username;
        }

        private void SendPacket<T>(T packet) where T : IMySerializable<T>
        {
            var packetWriter = new PacketWriter();
            packetWriter.Serialize(packet);
            _client.Client.Send(packetWriter.GetPacketBytes());
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    PacketReader.WaitForPacket();
                    switch (PacketReader.Opcode)
                    {
                        case Opcode.BroadcastUsers:
                            connectedEvent?.Invoke();
                            break;
                        case Opcode.SendMessage:
                            messageReceivedEvent?.Invoke();
                            break;
                        case Opcode.UserDisconnected:
                            userDisconnectedEvent?.Invoke();
                            break;
                        case Opcode.NewClient:
                        default:
                            Console.WriteLine($"[{DateTime.Now}]: Unhandled opcode value {PacketReader.Opcode} for user {Username}");
                            break;
                    }
                }
            });
        }
    }
}
