using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;

namespace SCReverser.Core.Interfaces
{
    public interface IDebugger : IDisposable
    {
        /// <summary>
        /// On module changed event
        /// </summary>
        event OnModuleDelegate OnModuleChanged;
        /// <summary>
        /// On method changed event
        /// </summary>
        event OnMethodDelegate OnMethodChanged;
        /// <summary>
        /// On instruction changed event
        /// </summary>
        event OnInstructionDelegate OnInstructionChanged;
        /// <summary>
        /// On state changed
        /// </summary>
        event OnStateChangedDelegate OnStateChanged;
        /// <summary>
        /// On breakpoint raised
        /// </summary>
        event OnInstructionDelegate OnBreakPoint;

        #region State variables
        /// <summary>
        /// Return true if have Disposed State
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// Return true if have Halt State
        /// </summary>
        bool IsHalt { get; }
        /// <summary>
        /// Return true if have Error State
        /// </summary>
        bool IsError { get; }
        /// <summary>
        /// Return true if have BreakPoint State
        /// </summary>
        bool IsBreakPoint { get; }
        #endregion
        /// <summary>
        /// Stack
        /// </summary>
        StackCollection Stack { get; }
        /// <summary>
        /// AltStack
        /// </summary>
        StackCollection AltStack { get; }
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
        /// Current Instruction offset
        /// </summary>
        uint CurrentInstructionOffset { get; set; }
        /// <summary>
        /// Current Instruction
        /// </summary>
        Instruction CurrentInstruction { get; set; }
        /// <summary>
        /// Modules
        /// </summary>
        ModuleCollection Modules { get; }
        /// <summary>
        /// Instructions
        /// </summary>
        InstructionCollection Instructions { get; }
        /// <summary>
        /// Current Method
        /// </summary>
        Method CurrentMethod { get; }
        /// <summary>
        /// Current Module
        /// </summary>
        Module CurrentModule { get; }
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