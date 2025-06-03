namespace DeathWish.Game.MsgLoader
{
    public unsafe class ProtectCipher
    {
        private byte[] BufIV;
        private byte[] BufKey;

        public ProtectCipher()
        {
            BufIV = new byte[32] { 89, 80, 40, 35, 33, 90, 44, 15, 60, 91, 23, 46, 40, 54, 99, 33, 142, 4, 19, 113, 106, 59, 215, 210, 169, 232, 121, 79, 241, 95, 193, 149 };
            BufKey = new byte[32] { 70, 28, 28, 59, 10, 18, 18, 13, 154, 52, 11, 31, 84, 99, 83, 190, 57, 26, 89, 218, 121, 217, 232, 36, 55, 224, 48, 169, 254, 163, 166, 43 };
        }

        public void Encrypt(ServerSockets.Packet msg)
        {
            int index = 4;
            for (int x = index; x < msg.SizePacket - index; x++)
            {
                *(byte*)(msg.Memory + x) = (byte)(*(byte*)(msg.Memory + x) ^ BufIV[x * 44 % 32]);
                *(byte*)(msg.Memory + x) = (byte)(*(byte*)(msg.Memory + x) ^ BufKey[x * 99 % 32]);
            }
        }

        public void Decrypt(ServerSockets.Packet msg)
        {
            int index = 4;
            for (int x = index; x < msg.SizePacket - index; x++)
            {
                *(byte*)(msg.Memory + x) = (byte)(BufIV[x * 44 % 32] ^ (byte)(*(byte*)(msg.Memory + x)));
                *(byte*)(msg.Memory + x) = (byte)(BufKey[x * 99 % 32] ^ (byte)(*(byte*)(msg.Memory + x)));
            }
        }
    }
}
