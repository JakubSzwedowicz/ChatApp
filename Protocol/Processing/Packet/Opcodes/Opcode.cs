namespace Protocol.Processing.Packet.Opcodes
{
    public enum Opcode : byte
    {
        NewClient,
        UserDisconnected,
        BroadcastUsers,
        SendMessage
    }
}
