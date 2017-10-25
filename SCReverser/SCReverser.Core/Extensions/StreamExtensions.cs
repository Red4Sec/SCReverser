using System.IO;

namespace SCReverser.Core.Extensions
{
    public static class StreamExtensions
    {
        public static short ReadInt16(this Stream reader)
        {
            byte[] m_buffer = new byte[2];

            if (reader.Read(m_buffer, 0, 2) != 2) throw (new EndOfStreamException());

            return (short)((int)m_buffer[0] | (int)m_buffer[1] << 8);
        }
        public static ushort ReadUInt16(this Stream reader)
        {
            byte[] m_buffer = new byte[2];

            if (reader.Read(m_buffer, 0, 2) != 2) throw (new EndOfStreamException());

            return (ushort)((int)m_buffer[0] | (int)m_buffer[1] << 8);
        }
        public static int ReadInt32(this Stream reader)
        {
            byte[] m_buffer = new byte[4];

            if (reader.Read(m_buffer, 0, 4) != 4) throw (new EndOfStreamException());

            return ((int)m_buffer[0] | (int)m_buffer[1] << 8 | (int)m_buffer[2] << 16 | (int)m_buffer[3] << 24);
        }
        public static uint ReadUInt32(this Stream reader)
        {
            byte[] m_buffer = new byte[4];

            if (reader.Read(m_buffer, 0, 4) != 4) throw (new EndOfStreamException());

            return (uint)((int)m_buffer[0] | (int)m_buffer[1] << 8 | (int)m_buffer[2] << 16 | (int)m_buffer[3] << 24);
        }
        public static ulong ReadUInt64(this Stream reader)
        {
            byte[] m_buffer = new byte[8];

            if (reader.Read(m_buffer, 0, 8) != 8) throw (new EndOfStreamException());

            uint num = (uint)((int)m_buffer[0] | (int)m_buffer[1] << 8 | (int)m_buffer[2] << 16 | (int)m_buffer[3] << 24);
            uint num2 = (uint)((int)m_buffer[4] | (int)m_buffer[5] << 8 | (int)m_buffer[6] << 16 | (int)m_buffer[7] << 24);
            return (ulong)num2 << 32 | (ulong)num;
        }

        public static long ReadInt64(this Stream reader)
        {
            byte[] m_buffer = new byte[8];

            if (reader.Read(m_buffer, 0, 8) != 8) throw (new EndOfStreamException());

            uint num = (uint)((int)m_buffer[0] | (int)m_buffer[1] << 8 | (int)m_buffer[2] << 16 | (int)m_buffer[3] << 24);
            uint num2 = (uint)((int)m_buffer[4] | (int)m_buffer[5] << 8 | (int)m_buffer[6] << 16 | (int)m_buffer[7] << 24);
            return (long)num2 << 32 | (long)num;
        }
    }
}