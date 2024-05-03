using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Protocol.Processing.Packet.Opcodes;

namespace Protocol.Processing.Packet
{
    public class NewClientPacket(string username) : IOpcodeProvider, IMySerializable<NewClientPacket>, IMyDeserializable<NewClientPacket>
    {
        public static Opcode Opcode { get; } = Opcode.NewClient;
        public string Username { get; private set; } = username;

        public static void Serialize(MemoryStream ms, NewClientPacket packet)
        {
            ms.WriteByte((byte)Opcode);
            IMySerializable<NewClientPacket>.WriteString(ms, packet.Username);
        }

        public static NewClientPacket Deserialize(BinaryReader br)
        {
            var username = IMyDeserializable<NewClientPacket>.ReadString(br);
            return new NewClientPacket(username);
        }
    }
}
