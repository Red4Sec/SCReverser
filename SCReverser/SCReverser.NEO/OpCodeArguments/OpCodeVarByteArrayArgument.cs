using SCReverser.Core.Extensions;
using SCReverser.Core.OpCodeArguments;
using System;
using System.IO;

namespace SCReverser.NEO.OpCodeArguments
{
    public class OpCodeVarByteArrayArgument : OpCodeByteArrayArgument
    {
        /// <summary>
        /// Max Length
        /// </summary>
        public ulong MaxLength { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLength">Max Length</param>
        public OpCodeVarByteArrayArgument(ulong maxLength) : base(0)
        {
            MaxLength = maxLength;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLength">Max Length</param>
        public OpCodeVarByteArrayArgument(int maxLength) : base(0)
        {
            MaxLength = (ulong)maxLength;
        }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            int read;
            ulong l = ReadVarInt(stream, out read, MaxLength);

            // TODO: This Raw value are wrong!
            RawValue = new byte[l];
            int lee = stream.Read(RawValue, 0, RawValue.Length);
            if (lee != RawValue.Length)
                throw (new EndOfStreamException());

            read += lee;
            return (uint)read;
        }
        /// <summary>
        /// Read Var Int logic from NEO
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="max">Max</param>
        public static ulong ReadVarInt(Stream reader, out int read, ulong max = ulong.MaxValue)
        {
            int fb = reader.ReadByte();
            if (fb < 0) throw (new EndOfStreamException());

            read = 1;

            ulong value;

            if (fb == 0xFD)
            {
                byte[] data = new byte[2];
                if (reader.Read(data, 0, 2) != 2) throw (new EndOfStreamException());

                value = data.ToUInt16(0);
                read += 2;
            }
            else if (fb == 0xFE)
            {
                byte[] data = new byte[4];
                if (reader.Read(data, 0, 4) != 4) throw (new EndOfStreamException());

                value = data.ToUInt32(0);
                read += 4;
            }
            else if (fb == 0xFF)
            {
                byte[] data = new byte[8];
                if (reader.Read(data, 0, 8) != 8) throw (new EndOfStreamException());

                value = data.ToUInt64(0);
                read += 8;
            }
            else value = (ulong)fb;

            if (value > max) throw new FormatException();
            return value;
        }
    }
}