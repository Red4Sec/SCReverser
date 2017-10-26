using Neo.SmartContract;

namespace SCReverser.NEO
{
    public class NeoDebuggerConfig
    {
        /// <summary>
        /// Trigger type
        /// </summary>
        public TriggerType TriggerType { get; set; } = TriggerType.Application;
        /// <summary>
        /// Blockchain Path
        /// </summary>
        public string BlockChainPath { get; set; } = null;
    }
}