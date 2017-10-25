using SCReverser.Core.Extensions;
using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeUShortArgument : OpCodeValueArgument<ushort>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeUShortArgument() : base() { }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            RawValue = new byte[2];
            if (stream.Read(RawValue, 0, 2) != 2)
                throw (new EndOfStreamException());

            Value = RawValue.ToUInt16(0);
            return 2;
        }
    }
}