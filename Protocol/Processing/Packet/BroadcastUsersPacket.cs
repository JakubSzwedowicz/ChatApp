using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Protocol.Processing.Packet.Opcodes;

namespace Protocol.Processing.Packet
{
    public class BroadcastUsersPacket : IOpcodeProvider, IMySerializable<BroadcastUsersPacket>, IMyDeserializable<BroadcastUsersPacket>
    {
        public static Opcode Opcode { get; } = Opcode.BroadcastUsers;

        public record User(string username, string guid)
        {
            public string username = username;
            public string guid = guid;
        }

        public List<User> Users { get; set; } = new();

        public static void Serialize(MemoryStream ms, BroadcastUsersPacket packet)
        {
            ms.WriteByte((byte)Opcode);
            foreach (var user in packet.Users)
            {
                IMySerializable<BroadcastUsersPacket>.WriteString(ms, user.username);
                IMySerializable<BroadcastUsersPacket>.WriteString(ms, user.guid);
            }
        }

        public static BroadcastUsersPacket Deserialize(BinaryReader br)
        {
            var broadcastedUsers = new BroadcastUsersPacket();
            while (((NetworkStream)br.BaseStream).DataAvailable)
            {
                var username = IMyDeserializable<NewClientPacket>.ReadString(br);
                var guid = IMyDeserializable<NewClientPacket>.ReadString(br);
                var user = new User(username, guid);
                broadcastedUsers.Users.Add(user);
            }
            return broadcastedUsers;
        }
    }
}
