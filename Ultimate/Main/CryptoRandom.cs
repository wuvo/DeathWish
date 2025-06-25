using System;
using System.Security.Cryptography;
namespace Ultimate.Main
{
    class CryptoRandom
    {
        private static RandomNumberGenerator r;
        public CryptoRandom()
        {
            r = RandomNumberGenerator.Create();
        }
        public double NextDouble()
        {
            byte[] b = new byte[4];
            r.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }
        public int Next(int minValue, int maxValue)
        {
            long range = (long)(maxValue - minValue);
            return (int)((long)Math.Floor(NextDouble() * range) + minValue);
        }
        public int Next()
        {
            return Next(0, Int32.MaxValue);
        }
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}
