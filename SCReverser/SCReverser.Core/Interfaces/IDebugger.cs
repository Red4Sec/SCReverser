using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.ObjectModel;

namespace SCReverser.Core.Interfaces
{
    public interface IDebugger : IDisposable
    {
        /// <summary>
        /// On instruction changed event
        /// </summary>
        event DebuggerBase.delOnInstructionChanged OnInstructionChanged;
        /// <summary>
        /// On breakpoint raised
        /// </summary>
        event DebuggerBase.delOnInstructionChanged OnBreakPoint;
        /// <summary>
        /// BreakPoints
        /// </summary>
        ObservableCollection<uint> BreakPoints { get; }
        /// <summary>
        /// Debugger state
        /// </summary>
        DebuggerState State { get; }
        /// <summary>
        /// Invocation stack count
        /// </summary>
        uint InvocationStackCount { get; }
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
        /// Get instruction by index
        /// </summary>
        /// <param name="instructionIndex">Instruction index</param>
        Instruction this[uint instructionIndex] { get; }
        /// <summary>
        /// Have any breakpoint ?
        /// </summary>
        bool HaveBreakPoints { get; }
        /// <summary>
        /// Initialize debuger
        /// </summary>
        bool Initialize();
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