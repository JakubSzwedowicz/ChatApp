using Protocol.Processing.Packet.Opcodes;
using Protocol.Processing.Packet.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Processing.Packet
{
    public class UserDisconnectedPacket(string guid) : IOpcodeProvider, IMySerializable<UserDisconnectedPacket>, IMyDeserializable<UserDisconnectedPacket>
    {
        public static Opcode Opcode { get; } = Opcode.UserDisconnected;
        public readonly string guid = guid;

        public static UserDisconnectedPacket Deserialize(BinaryReader br)
        {
            var guid = IMyDeserializable<UserDisconnectedPacket>.ReadString(br);
            return new UserDisconnectedPacket(guid);
        }

        public static void Serialize(MemoryStream ms, UserDisconnectedPacket serializable)
        {
            ms.WriteByte((byte)Opcode);
            IMySerializable<UserDisconnectedPacket>.WriteString(ms, serializable.guid);
        }
    }
}
