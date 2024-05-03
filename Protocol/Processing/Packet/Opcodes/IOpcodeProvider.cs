using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Processing.Packet.Opcodes
{
    public interface IOpcodeProvider
    {
        public static abstract Opcode Opcode { get;}
    }
}
