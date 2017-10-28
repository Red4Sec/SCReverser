using SCReverser.Core.Helpers;

namespace SCReverser.Core.Types
{
    public class Ocurrence
    {
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return JsonHelper.Serialize(this);
        }
    }
}