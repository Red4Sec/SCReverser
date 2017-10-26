using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace SCReverser.Core.Interfaces
{
    public class DebuggerBase : IDebugger
    {
        /// <summary>
        /// Cache offset - instruction index
        /// </summary>
        protected readonly Dictionary<uint, uint> Offsets = new Dictionary<uint, uint>();

        uint _CurrentInstructionIndex;

        /// <summary>
        /// Delegate for On instruction event
        /// </summary>
        /// <param name="sender">Debugger</param>
        /// <param name="instructionIndex">Instruction index</param>
        public delegate void delOnInstructionChanged(IDebugger sender, uint instructionIndex);
        /// <summary>
        /// On instruction changed event
        /// </summary>
        public event delOnInstructionChanged OnInstructionChanged;
        /// <summary>
        /// On breakpoint raised
        /// </summary>
        public event delOnInstructionChanged OnBreakPoint;

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
        /// Get instruction by index
        /// </summary>
        /// <param name="instructionIndex">Instruction index</param>
        public Instruction this[uint instructionIndex]
        {
            get
            {
                if (Instructions.Length <= instructionIndex) return null;
                return Instructions[instructionIndex];
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
        protected DebuggerBase(IEnumerable< Instruction> instructions)
        {
            Instructions = instructions.ToArray();
            BreakPoints = new ObservableCollection<uint>();
            State = DebuggerState.None;
            InvocationStackCount = 0;
            CurrentInstructionIndex = 0;

            // Cache offsets
            uint ix = 0;
            foreach (Instruction i in Instructions)
            {
                Offsets.Add(i.Offset, ix);
                ix++;
            }
        }
        /// <summary>
        /// Initialize debuger
        /// </summary>
        public virtual bool Initialize()
        {
            // Check Initialize method with one parameter like this
            //      public bool Initialize(NeoDebuggerConfig config)

            Type t = GetType();

            // Get method
            MethodInfo mi = t.GetMethods()
                .Where(u => u.Name == "Initialize")
                .Where(u => u.GetParameters().Count() == 1)
                .FirstOrDefault();

            if (mi != null)
            {
                // Configure object for Initialize debugger

                object par = Activator.CreateInstance(mi.GetParameters()[0].ParameterType);

                if (!FEditConfig.Configure(par))
                    return false;

                mi.Invoke(this, new object[] { par });
            }

            return true;
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public virtual void Dispose()
        {
            State |= DebuggerState.Disposed;
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