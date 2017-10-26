using SCReverser.Core.Types;
using System;

namespace SCReverser.Core.Interfaces
{
    public interface IDebugger : IDisposable
    {
        /// <summary>
        /// Invocation stack count
        /// </summary>
        uint InvocationStackCount { get;  }
        /// <summary>
        /// Current Instruction Index
        /// </summary>
        uint CurrentInstructionIndex { get; set; }
        /// <summary>
        /// Current Instruction
        /// </summary>
        Instruction CurrentInstruction { get; set; }
        /// <summary>
        /// Instructions
        /// </summary>
        Instruction[] Instructions { get; }
        /// <summary>
        /// Resume
        /// </summary>
        void Execute();
        /// <summary>
        /// Step into
        /// </summary>
        void StepInto();
        /// <summary>
        /// Step over
        /// </summary>
        void StepOver();
        /// <summary>
        /// Step out
        /// </summary>
        void StepOut();
    }
}