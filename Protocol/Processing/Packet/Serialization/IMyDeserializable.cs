using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Processing.Packet.Serialization
{
    public interface IMyDeserializable<T>
    {

        // It's important to notice that by the time this function is called, the opcode must have already been read.
        public static abstract T Deserialize(BinaryReader br);

        protected static string ReadString(BinaryReader br)
        {
            var length = br.ReadInt32();
            byte[] msgBuffer = new byte[length];
            br.Read(msgBuffer, 0, length);


            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }

    }
}
