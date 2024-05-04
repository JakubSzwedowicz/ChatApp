using System.Net.Sockets;
using System.Text;
using Protocol.Processing.Packet.Serialization;

namespace Protocol.Processing
{
    public class PacketWriter
    {
        private readonly MemoryStream _ms = new();

        public void Serialize<T>(T packet) where T : IMySerializable<T>
        {
            // TOOD: Move appending opcode here
            T.Serialize(_ms, packet);
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
