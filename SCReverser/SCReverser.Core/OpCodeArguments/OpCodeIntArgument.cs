using SCReverser.Core.Extensions;
using System.IO;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeIntArgument : OpCodeValueArgument<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeIntArgument() : base() { }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            RawValue = new byte[4];
            if (stream.Read(RawValue, 0, 4) != 4)
                throw (new EndOfStreamException());

            Value = RawValue.ToInt32(0);
            return 4;
        }
    }
}