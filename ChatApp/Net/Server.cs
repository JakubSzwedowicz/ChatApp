using Protocol.Packet;
using System;
using System.Net.Sockets;

namespace ChatClient.Net
{
    internal class Server
    {
        private readonly TcpClient _client = new();

        public void ConnectToServer(string username)
        {
            if(!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
        }
    }
}
