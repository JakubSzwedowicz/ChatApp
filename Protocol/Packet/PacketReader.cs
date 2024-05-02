﻿using System.Net.Sockets;
using System.Text;

namespace Protocol.Packet
{
    public class PacketReader : BinaryReader
    {
        private readonly NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns) 
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            var length = ReadInt32();
            byte[] msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
