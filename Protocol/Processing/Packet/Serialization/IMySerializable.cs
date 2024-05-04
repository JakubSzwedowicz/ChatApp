using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Processing.Packet.Serialization
{
    public interface IMySerializable<T>
    {

        public static abstract void Serialize(MemoryStream ms, T serializable);

        protected static void WriteString(MemoryStream ms, string msg)
        {
            var msgLength = msg.Length;
            ms.Write(BitConverter.GetBytes(msgLength));
            ms.Write(Encoding.ASCII.GetBytes(msg));
        }

    }
}
