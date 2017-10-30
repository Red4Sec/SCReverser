using Newtonsoft.Json;
using SCReverser.Core.Collections;

namespace SCReverser.Core.Types
{
    public class ReverseResult
    {
        /// <summary>
        /// Bytes
        /// </summary>
        [JsonIgnore]
        public byte[] Bytes { get; set; }
        /// <summary>
        /// Modules
        /// </summary>
        public ModuleCollection Modules { get; private set; } = new ModuleCollection();
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