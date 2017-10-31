using Newtonsoft.Json;
using SCReverser.Core.Helpers;

namespace SCReverser.Core.Types
{
    public class IndexOffset
    {
        /// <summary>
        /// Index
        /// </summary>
        public uint Index { get; set; }
        /// <summary>
        /// Offset
        /// </summary>
        public uint Offset { get; set; }
        /// <summary>
        /// OffsetHex
        /// </summary>
        [JsonIgnore]
        public string OffsetHex { get { return "0x" + Offset.ToString("X4"); } }

        /// <summary>
        /// Index between
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        public bool IndexBetween(uint from, uint to)
        {
            return Index >= from && Index <= to;
        }
        /// <summary>
        /// Offset between
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        public bool OffsetBetween(uint from, uint to)
        {
            return Offset >= from && Offset <= to;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return JsonHelper.Serialize(this);
        }
    }
}