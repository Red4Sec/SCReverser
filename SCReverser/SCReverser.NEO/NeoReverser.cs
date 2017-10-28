using SCReverser.Core.Collections;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;

namespace SCReverser.NEO
{
    public class NeoReverser : ReverserBase<NeoOpCode>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NeoReverser() : base() { }

        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="result">Resut</param>
        public override void PrapareResultForOcurrences(ReverseResult result)
        {
            base.PrapareResultForOcurrences(result);

            if (result == null) return;

            // Append syscall
            if (!result.Ocurrences.ContainsKey("SysCalls"))
                result.Ocurrences["SysCalls"] = new OcurrenceCollection() { Checker = SysCallCheckOcurrence };
        }
        /// <summary>
        /// Check if Instruction are SysCall
        /// </summary>
        /// <param name="instruction">Instruction</param>
        /// <param name="name">Name</param>
        bool SysCallCheckOcurrence(Instruction instruction, out string name)
        {
            if (instruction.Argument != null &&
                instruction.OpCode != null && 
                instruction.OpCode.Name == "SYSCALL")
            {
                name = instruction.Argument.ASCIIValue;
                return !string.IsNullOrEmpty(name);
            }

            name = null;
            return false;
        }
    }
}