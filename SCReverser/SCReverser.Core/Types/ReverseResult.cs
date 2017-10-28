using SCReverser.Core.Collections;

namespace SCReverser.Core.Types
{
    public class ReverseResult
    {
        /// <summary>
        /// Instructions
        /// </summary>
        public InstructionCollection Instructions { get; private set; } = new InstructionCollection();
        /// <summary>
        /// Ocurrences
        /// </summary>
        public KeyValueCollection<string, OcurrenceCollection> Ocurrences { get; private set; } = new KeyValueCollection<string, OcurrenceCollection>();
    }
}