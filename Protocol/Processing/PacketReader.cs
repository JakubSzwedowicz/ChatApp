using System.Net.Sockets;
using System.Text;
using Protocol.Processing.Packet.Opcodes;
using Protocol.Processing.Packet.Serialization;

namespace Protocol.Processing
{
    public class PacketReader(NetworkStream ns)
    {
        private readonly BinaryReader _reader = new BinaryReader(ns);

        public Opcode Opcode { get; private set; }

        public void WaitForPacket()
        {
            // Keep looping until data is available to read
            Opcode = (Opcode)_reader.ReadByte();
        }

        public T Deserialize<T>() where T : IMyDeserializable<T>, IOpcodeProvider
        {
            CheckOpcode<T>();
            T packet = T.Deserialize(_reader);
            return packet;
        }

        private void CheckOpcode<T>() where T : IOpcodeProvider
        {
            if (Opcode != T.Opcode)
            {
                throw new InvalidOperationException($"Cannot create {typeof(T).FullName} from opcode: {Opcode}");
            }
        }
    }
}
