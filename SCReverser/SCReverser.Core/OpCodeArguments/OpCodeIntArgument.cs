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
            Value = stream.ReadInt32();
            return 4;
        }
    }
}