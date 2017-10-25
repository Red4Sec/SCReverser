using SCReverser.Core.Types;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeValueArgument<T> : OpCodeArgument where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Size
        /// </summary>
        public int Size { get; private set; }
        /// <summary>
        /// Value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected OpCodeValueArgument()
        {
            Size = Marshal.SizeOf<T>();
        }
        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public override uint Read(Stream stream)
        {
            throw (new NotImplementedException());
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}