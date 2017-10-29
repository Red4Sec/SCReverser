using Newtonsoft.Json;
using SCReverser.Core.Extensions;
using System;

namespace SCReverser.Core.OpCodeArguments
{
    public class OpCodeUShortArgumentBigEndian : OpCodeUShortArgument
    {
        /// <summary>
        /// Value
        /// </summary>
        [JsonIgnore]
        public override ushort Value
        {
            get { return RawValue.ToUInt16BigEndian(); }
            set
            {
                RawValue = value.ToByteArray();
                Array.Reverse(RawValue);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeUShortArgumentBigEndian() { }
    }
}