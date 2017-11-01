using SCReverser.Core.Collections;
using SCReverser.Core.Helpers;

namespace SCReverser.Core.Types
{
    public class Ocurrence
    {
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get { return Instructions.Count; } }
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Instructions
        /// </summary>
        public InstructionCollection Instructions { get; private set; } = new InstructionCollection();

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return JsonHelper.Serialize(this);
        }
    }
}