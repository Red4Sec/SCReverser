using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.ObjectModel;

namespace SCReverser.Core.Interfaces
{
    public class DebuggerBase : IDebugger
    {
        /// <summary>
        /// Delegate for On instruction event
        /// </summary>
        /// <param name="sender">Debugger</param>
        /// <param name="currentInstruction">Current instruction</param>
        public delegate void delOnInstructionChanged(IDebugger sender, uint currentInstruction);
        /// <summary>
        /// On instruction changed event
        /// </summary>
        public event delOnInstructionChanged OnInstructionChanged;
        /// <summary>
        /// On breakpoint raised
        /// </summary>
        public event delOnInstructionChanged OnBreakPoint;

        uint _CurrentInstructionIndex;
        /// <summary>
        /// Current Instruction
        /// </summary>
        public uint CurrentInstructionIndex
        {
            get { return _CurrentInstructionIndex; }
            set
            {
                if (_CurrentInstructionIndex == value)
                    return;

                _CurrentInstructionIndex = value;
                OnInstructionChanged?.Invoke(this, value);

                if (BreakPoints.Contains(value))
                {
                    // Raise breakpoint
                    State |= DebuggerState.BreakPoint;
                    OnBreakPoint?.Invoke(this, value);
                }
            }
        }
        /// <summary>
        /// Current Instruction
        /// </summary>
        public Instruction CurrentInstruction
        {
            get { return Instructions[CurrentInstructionIndex]; }
            set
            {
                // Search index of this instruction
                for (uint x = 0, m = (uint)(Instructions == null ? 0 : Instructions.Length); x < m; x++)
                    if (Instructions[x] == value)
                        CurrentInstructionIndex = x;
            }
        }
        /// <summary>
        /// Invocation stack count
        /// </summary>
        public virtual uint InvocationStackCount { get; protected set; }
        /// <summary>
        /// BreakPoints
        /// </summary>
        public ObservableCollection<uint> BreakPoints { get; private set; }
        /// <summary>
        /// Have any breakpoint ?
        /// </summary>
        public bool HaveBreakPoints { get { return BreakPoints.Count > 0; } }
        /// <summary>
        /// Instructions
        /// </summary>
        public Instruction[] Instructions { get; private set; }
        /// <summary>
        /// Debugger state
        /// </summary>
        public DebuggerState State { get; protected set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instructions">Instructions</param>
        protected DebuggerBase(params Instruction[] instructions)
        {
            Instructions = instructions;
            BreakPoints = new ObservableCollection<uint>();
            CurrentInstructionIndex = 0;
            State = DebuggerState.None;
        }
        /// <summary>
        /// Run
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public virtual void Run(params Instruction[] instructions)
        {
            State = DebuggerState.Run;
            Instructions = instructions;
            CurrentInstructionIndex = 0;
            InvocationStackCount = 0;
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public virtual void Dispose()
        {
            State = DebuggerState.Disposed;
        }
        /// <summary>
        /// Step into
        /// </summary>
        public virtual void StepInto()
        {
            throw new NotImplementedException();
        }
        protected bool ShouldStep()
        {
            return !AreEnded() && !State.HasFlag(DebuggerState.BreakPoint);
        }
        protected bool AreEnded()
        {
            return
                (
                State.HasFlag(DebuggerState.Disposed) || State.HasFlag(DebuggerState.Error) || State.HasFlag(DebuggerState.Ended)
                );
        }
        /// <summary>
        /// Resume
        /// </summary>
        public void Execute()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            while (ShouldStep())
                StepInto();
        }
        /// <summary>
        /// Step out
        /// </summary>
        public void StepOut()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            uint c = InvocationStackCount;
            while (ShouldStep() && InvocationStackCount >= c)
                StepInto();
        }
        /// <summary>
        /// Step over
        /// </summary>
        public void StepOver()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            uint c = InvocationStackCount;
            do
            {
                StepInto();
            }
            while (ShouldStep() && InvocationStackCount > c);
        }
    }
}