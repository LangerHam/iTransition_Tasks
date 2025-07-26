using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using NSec.Cryptography;

namespace Task3
{
    class Crypt
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static byte[] SecureKey()
        {
            byte[] key = new byte[32]; // 256 bits
            Rng.GetBytes(key);
            return key;
        }

        public static int SecureRandom(int max)
        {
            byte[] bytes = new byte[4];
            uint threshold = uint.MaxValue - (uint.MaxValue % (uint)max);
            uint value;
            do
            {
                Rng.GetBytes(bytes);
                value = BitConverter.ToUInt32(bytes, 0);
            }
            while (value >= threshold);
            return (int)(value % (uint)max);
        }

        public static string HMAC(byte[] key, string message) =>
            Convert.ToHexString(HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(message)));
    }
}
