using System;
using System.Net.Sockets;

namespace ChatClient.Net
{
    internal class Server
    {
        private readonly TcpClient _client = new();

        public void ConnecToServer()
        {
            if(!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
            }
        }
    }
}
