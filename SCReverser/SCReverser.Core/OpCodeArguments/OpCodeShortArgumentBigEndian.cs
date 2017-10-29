using SCReverser.Core.Extensions;
using System;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeShortArgumentBigEndian : OpCodeShortArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        public override short Value
        {
            get { return RawValue.ToInt16BigEndian(); }
            set
            {
                RawValue = value.ToByteArray();
                Array.Reverse(RawValue);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeShortArgumentBigEndian() { }
    }
}