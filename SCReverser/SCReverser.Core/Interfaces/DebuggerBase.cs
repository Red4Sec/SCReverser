using SCReverser.Core.Collections;
using SCReverser.Core.Converters;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SCReverser.Core.Interfaces
{
    public class DebuggerBase<T> : IDebugger
    {
        /// <summary>
        /// Cache offset - instruction index
        /// </summary>
        readonly OffsetRelationCache OffsetCache = new OffsetRelationCache();

        DebuggerState _State;
        uint _CurrentInstructionIndex;

        #region State variables
        /// <summary>
        /// Return true if have Disposed State
        /// </summary>
        [Category("State")]
        public bool IsDisposed { get { return State.HasFlag(DebuggerState.Disposed); } }
        /// <summary>
        /// Return true if have Halt State
        /// </summary>
        [Category("State")]
        public bool IsHalt { get { return State.HasFlag(DebuggerState.Halt); } }
        /// <summary>
        /// Return true if have Error State
        /// </summary>
        [Category("State")]
        public bool IsError { get { return State.HasFlag(DebuggerState.Error); } }
        /// <summary>
        /// Return true if have BreakPoint State
        /// </summary>
        [Category("State")]
        public bool IsBreakPoint { get { return State.HasFlag(DebuggerState.BreakPoint); } }
        /// <summary>
        /// Debugger state
        /// </summary>
        [Category("State")]
        public DebuggerState State
        {
            get { return _State; }
            protected set
            {
                if (_State == value) return;

                DebuggerState old = _State;
                _State = value;

                // Raise event
                OnStateChanged?.Invoke(this, old, _State);
            }
        }
        #endregion

        /// <summary>
        /// On instruction changed event
        /// </summary>
        public event OnInstructionDelegate OnInstructionChanged;
        /// <summary>
        /// On breakpoint raised
        /// </summary>
        public event OnInstructionDelegate OnBreakPoint;
        /// <summary>
        /// On state changed
        /// </summary>
        public event OnStateChangedDelegate OnStateChanged;
        /// <summary>
        /// Configuration
        /// </summary>
        [Browsable(false)]
        public T Config { get; private set; }
        /// <summary>
        /// Get Type of ConfigType
        /// </summary>
        [Browsable(false)]
        public Type InitializeConfigType
        {
            get { return typeof(T); }
        }
        #region
        /// <summary>
        /// Stack
        /// </summary>
        [Browsable(false)]
        public StackCollection Stack { get; private set; } = new StackCollection();
        /// <summary>
        /// AltStack
        /// </summary>
        [Browsable(false)]
        public StackCollection AltStack { get; private set; } = new StackCollection();
        /// <summary>
        /// Current Instruction
        /// </summary>
        [Category("Debug")]
        public virtual uint CurrentInstructionIndex
        {
            get { return _CurrentInstructionIndex; }
            set
            {
                if (_CurrentInstructionIndex == value)
                    return;

                _CurrentInstructionIndex = value;

                Instruction ins = Instructions[CurrentInstructionIndex];

                OnInstructionChanged?.Invoke(this, ins);

                if (ins.HaveBreakPoint)
                {
                    // Raise breakpoint
                    State |= DebuggerState.BreakPoint;
                    OnBreakPoint?.Invoke(this, ins);
                }
            }
        }
        /// <summary>
        /// Current instruction offset
        /// </summary>
        [Category("Debug"), TypeConverter(typeof(UInt32HexTypeConverter))]
        public uint CurrentInstructionOffset
        {
            get { return CurrentInstruction.Location.Offset; }
            set
            {
                if (OffsetToIndex(value, out uint v))
                    CurrentInstructionIndex = v;
            }
        }
        /// <summary>
        /// Current Instruction
        /// </summary>
        [Category("Debug")]
        public Instruction CurrentInstruction
        {
            get { return Instructions[CurrentInstructionIndex]; }
            set
            {
                // Search index of this instruction
                for (uint x = 0, m = (uint)(Instructions == null ? 0 : Instructions.Length); x < m; x++)
                    if (Instructions[x] == value)
                    {
                        CurrentInstructionIndex = x;
                        break;
                    }
            }
        }
        /// <summary>
        /// Invocation stack count
        /// </summary>
        [Category("Debug")]
        public virtual uint InvocationStackCount { get; protected set; }
        #endregion
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
        /// Instructions
        /// </summary>
        [Browsable(false)]
        public Instruction[] Instructions { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instructions">Instructions</param>
        /// <param name="debugConfig">Debugger config</param>
        protected DebuggerBase(IEnumerable<Instruction> instructions, T debugConfig)
        {
            Config = debugConfig;
            Instructions = instructions.ToArray();
            State = DebuggerState.None;
            InvocationStackCount = 0;
            CurrentInstructionIndex = 0;

            // Cache offsets
            OffsetCache.FillWith(Instructions);
        }
        /// <summary>
        /// Index to Offset
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="offset">Offset</param>
        public bool IndexToOffset(uint index, out uint offset)
        {
            return OffsetCache.TryGetValue(index, out offset, OffsetIndexRelation.IndexToOffset);
        }
        /// <summary>
        /// Offset to Index
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="index">Index</param>
        public bool OffsetToIndex(uint offset, out uint index)
        {
            return OffsetCache.TryGetValue(offset, out index, OffsetIndexRelation.OffsetToIndex);
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
        protected bool AreEnded()
        {
            return
                (
                State.HasFlag(DebuggerState.Disposed) || State.HasFlag(DebuggerState.Error) || State.HasFlag(DebuggerState.Halt)
                );
        }
        bool ShouldStep(DateTime startTime, uint ins)
        {
            if ((DateTime.UtcNow - startTime).TotalSeconds > 10)
                throw (new Exception("[" + ins.ToString() + " instructions processed] Possible infinite loop detected!"));

            return !AreEnded() && !State.HasFlag(DebuggerState.BreakPoint);
        }
        /// <summary>
        /// Resume
        /// </summary>
        public void Execute()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            uint ins = 0;
            DateTime now = DateTime.UtcNow;

            while (ShouldStep(now, ins))
            {
                StepInto();
                ins++;
            }
        }
        /// <summary>
        /// Step out
        /// </summary>
        public void StepOut()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            uint ins = 0;
            DateTime now = DateTime.UtcNow;
            uint c = InvocationStackCount;

            while (ShouldStep(now, ins) && InvocationStackCount >= c)
            {
                StepInto();
                ins++;
            }
        }
        /// <summary>
        /// Step over
        /// </summary>
        public void StepOver()
        {
            if (AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            uint ins = 0;
            DateTime now = DateTime.UtcNow;
            uint c = InvocationStackCount;

            do
            {
                StepInto();
                ins++;
            }
            while (ShouldStep(now, ins) && InvocationStackCount > c);
        }
    }
}
