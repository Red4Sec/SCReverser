using SCReverser.Core.Types;
using System;
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
        public OpCodeValueArgument()
        {
            Size = Marshal.SizeOf<T>();
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