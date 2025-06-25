using System;
using System.Collections.Generic;
using System.Text;

namespace Ultimate
{
    public class LegacyCipher
    {
        class Counter
        {
            ushort KeyCounter;
            public byte FirstKey
            {
                get { return (byte)(KeyCounter & 0xFF); }
            }
            public byte SecondKey
            {
                get { return (byte)(KeyCounter >> 8); }
            }
            public ushort Increment(ushort val)
            {
                this.KeyCounter += val;
                return (this.KeyCounter);
            }
        }
        private readonly Counter DecryptCounter;
        private readonly Counter EncryptCounter;
        private readonly byte[] FirstKey;
        private readonly byte[] SecondKey;
        public LegacyCipher()
        {
            this.DecryptCounter = new Counter();
            this.EncryptCounter = new Counter();
            this.FirstKey = new byte[0x100];
            this.SecondKey = new byte[0x100];
            byte FirstByte = 0x9D;
            byte SecondByte = 0x62;

            for (int i = 0; i < 0x100; i++)
            {
                this.FirstKey[i] = FirstByte;
                this.SecondKey[i] = SecondByte;
                FirstByte = (byte)((0x0F + (byte)(FirstByte * 0xFA)) * FirstByte + 0x13);
                SecondByte = (byte)((0x79 - (byte)(SecondByte * 0x5C)) * SecondByte + 0x6D);
            }
        }
        public void Increment(ushort va)
        {
            this.EncryptCounter.Increment(va);
        }
        public void Encode(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)0xAB;
                buffer[i] = (byte)(buffer[i] >> 4 | buffer[i] << 4);
                buffer[i] ^= (byte)(this.FirstKey[this.EncryptCounter.FirstKey] ^ this.SecondKey[this.EncryptCounter.SecondKey]);
                this.EncryptCounter.Increment(1);
            }
        }
        public void Decode(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)0xAB;
                buffer[i] = (byte)(buffer[i] >> 4 | buffer[i] << 4);
                buffer[i] ^= (byte)(this.FirstKey[this.DecryptCounter.FirstKey] ^ this.SecondKey[this.DecryptCounter.SecondKey]);
                this.DecryptCounter.Increment(1);
            }
        }
    }
}
