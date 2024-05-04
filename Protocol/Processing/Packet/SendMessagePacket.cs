using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Protocol.Processing.Packet.Opcodes;
using Protocol.Processing.Packet.Serialization;

namespace Protocol.Processing.Packet
{
    public class SendMessagePacket(string message) : IOpcodeProvider, IMySerializable<SendMessagePacket>, IMyDeserializable<SendMessagePacket>
    {
        public static Opcode Opcode { get; } = Opcode.SendMessage;
        public readonly string message = message;

        public static void Serialize(MemoryStream ms, SendMessagePacket packet)
        {
            ms.WriteByte((byte)Opcode);
            IMySerializable<SendMessagePacket>.WriteString(ms, packet.message);
        }

        public static SendMessagePacket Deserialize(BinaryReader br)
        {
            var message = IMyDeserializable<SendMessagePacket>.ReadString(br);
            return new SendMessagePacket(message);
        }
    }
}
