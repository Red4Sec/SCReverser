using SCReverser.Core.Collections;
using SCReverser.Core.Converters;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.ComponentModel;

namespace SCReverser.Core.Interfaces
{
    public class DebuggerBase<T> : IDebugger
    {
        DebuggerState _State;
        Method _CurrentMethod;
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
        /// On module changed event
        /// </summary>
        public event OnModuleDelegate OnModuleChanged;
        /// <summary>
        /// On method changed event
        /// </summary>
        public event OnMethodDelegate OnMethodChanged;
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

                // Search Module and Method
                Module m = Modules.GetModuleOf(ins.Location);
                if (m != null) CurrentMethod = m.Methods.GetMethodOf(ins.Location);
                else CurrentMethod = null;

                // Raise event
                OnInstructionChanged?.Invoke(this, ins);

                if (ins.BreakPoint)
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
                if (Instructions.OffsetToIndex(value, out uint v))
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
                for (uint x = 0, m = (uint)(Instructions == null ? 0 : Instructions.Count); x < m; x++)
                    if (Instructions[x] == value)
                    {
                        CurrentInstructionIndex = x;
                        break;
                    }
            }
        }
        /// <summary>
        /// Current Module
        /// </summary>
        [Category("Debug")]
        public Module CurrentModule
        {
            get { return _CurrentMethod == null ? null : _CurrentMethod.Parent; }
        }
        /// <summary>
        /// Current Method
        /// </summary>
        [Category("Debug")]
        public Method CurrentMethod
        {
            get { return _CurrentMethod; }
            set
            {
                if (_CurrentMethod == value) return;

                Module mbefore = _CurrentMethod == null ? null : _CurrentMethod.Parent;
                _CurrentMethod = value;

                Module mafter = _CurrentMethod == null ? null : _CurrentMethod.Parent;

                if (mbefore != mafter)
                    OnModuleChanged?.Invoke(this, mafter);

                OnMethodChanged?.Invoke(this, value);
            }
        }
        /// <summary>
        /// Invocation stack count
        /// </summary>
        [Category("Debug")]
        public virtual uint InvocationStackCount { get; protected set; }
        #endregion
        /// <summary>
        /// Modules
        /// </summary>
        [Browsable(false)]
        public ModuleCollection Modules { get; private set; } = new ModuleCollection();
        /// <summary>
        /// Instructions
        /// </summary>
        [Browsable(false)]
        public InstructionCollection Instructions { get; private set; } = new InstructionCollection();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Reverse Result</param>
        /// <param name="debugConfig">Debugger config</param>
        protected DebuggerBase(ReverseResult result, T debugConfig)
        {
            if (result != null)
            {
                Modules.CopyFrom(result.Modules);
                Instructions.CopyFrom(result.Instructions);
            }

            Config = debugConfig;
            State = DebuggerState.None;
            InvocationStackCount = 0;
            CurrentInstructionIndex = 0;

            Instruction ins = Instructions[CurrentInstructionIndex];
            if (ins != null)
            {
                Module m = Modules.GetModuleOf(ins.Location);
                if (m != null)
                    _CurrentMethod = m.Methods.GetMethodOf(ins.Location);
            }
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
            if ((DateTime.UtcNow - startTime).TotalSeconds > 30)
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
