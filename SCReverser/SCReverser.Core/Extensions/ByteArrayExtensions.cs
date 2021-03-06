﻿using System;
using System.Text;

namespace SCReverser.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Byte array to Hex string
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public static string ToHexString(this byte[] buffer)
        {
            StringBuilder hex = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer) hex.Append(b.ToString("X2"));

            return hex.ToString();
        }
        /// <summary>
        /// Hex string to byte array
        /// </summary>
        /// <param name="input">Input</param>
        public static byte[] HexToByteArray(this string input)
        {
            if (input == null) return null;

            // Remove 0x
            if (input.StartsWith("0X", StringComparison.InvariantCultureIgnoreCase))
                input = input.Substring(2);

            int chars = input.Length;

            byte[] bytes = new byte[chars / 2];
            for (int i = 0; i < chars; i += 2)
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);

            return bytes;
        }

        public static byte[] ToByteArray(this int value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24)
            };
        }

        public static byte[] ToByteArray(this uint value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24)
            };
        }

        public static byte[] ToByteArray(this long value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56),
            };
        }

        public static byte[] ToByteArray(this ulong value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56),
            };
        }

        public static byte[] ToByteArray(this short value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
            };
        }

        public static byte[] ToByteArray(this ushort value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
            };
        }

        public static short ToInt16(this byte[] buffer, int index = 0)
        {
            return (short)((int)buffer[index] | (int)buffer[index + 1] << 8);
        }
        public static ushort ToUInt16(this byte[] buffer, int index = 0)
        {
            return (ushort)((int)buffer[index] | (int)buffer[index + 1] << 8);
        }
        public static short ToInt16BigEndian(this byte[] buffer, int index = 0)
        {
            return (short)((int)buffer[index + 1] | (int)buffer[index] << 8);
        }
        public static ushort ToUInt16BigEndian(this byte[] buffer, int index = 0)
        {
            return (ushort)((int)buffer[index + 1] | (int)buffer[index] << 8);
        }

        public static int ToInt32(this byte[] buffer, int index = 0)
        {
            return ((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
        }
        public static uint ToUInt32(this byte[] buffer, int index = 0)
        {
            return (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
        }

        public static long ToInt64(this byte[] buffer, int index = 0)
        {
            uint num = (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
            uint num2 = (uint)((int)buffer[index + 4] | (int)buffer[index + 5] << 8 | (int)buffer[index + 6] << 16 | (int)buffer[index + 7] << 24);
            return (long)num2 << 32 | (long)num;
        }
        public static ulong ToUInt64(this byte[] buffer, int index = 0)
        {
            uint num = (uint)((int)buffer[index + 0] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24);
            uint num2 = (uint)((int)buffer[index + 4] | (int)buffer[index + 5] << 8 | (int)buffer[index + 6] << 16 | (int)buffer[index + 7] << 24);
            return (ulong)num2 << 32 | (ulong)num;
        }
    }
}