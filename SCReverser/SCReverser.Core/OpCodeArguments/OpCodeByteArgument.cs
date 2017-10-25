using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeByteArgument : OpCodeValueArgument<byte>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeByteArgument() : base() { }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            int read = stream.ReadByte();
            if (read < 0) throw (new EndOfStreamException());

            Value = (byte)read;
            return 1;
        }
    }
}