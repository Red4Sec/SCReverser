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
            Value = stream.ReadUInt16();
            return 2;
        }
    }
}