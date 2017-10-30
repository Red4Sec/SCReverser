using Neo.VM;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SCReverser.NEO
{
    public class NeoDebugger : DebuggerBase<NeoConfig>
    {
        Decimal _GasConsumed;
        internal NeoEngine Engine;

        /// <summary>
        /// Gas consumed
        /// </summary>
        [Category("NEO")]
        public string GasConsumed
        {
            get
            {
                return _GasConsumed.ToString("#0.0#######");
            }
        }
        /// <summary>
        /// Current Instruction Index
        /// </summary>
        public override uint CurrentInstructionIndex
        {
            get { return base.CurrentInstructionIndex; }
            set
            {
                if (Engine == null || Engine.State.HasFlag(VMState.HALT) || Engine.State.HasFlag(VMState.FAULT)) return;

                // Set the engine instruction pointer

                uint val;
                if (IndexToOffset(value, out val))
                {
                    if (Engine.CurrentContext.InstructionPointer != val)
                        Engine.CurrentContext.InstructionPointer = (int)val;

                    base.CurrentInstructionIndex = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instructions">Instructions</param>
        /// <param name="config">Configuration</param>
        public NeoDebugger(IEnumerable<Instruction> instructions, NeoConfig config) : base(instructions, config)
        {
            // Set config
            if (config == null) config = new NeoConfig();

            // Create script
            byte[] script;
            using (MemoryStream ms = new MemoryStream())
            {
                //uint offset = 0;
                foreach (Instruction i in Instructions)
                {
                    //i.Offset = offset;
                    /*offset +=*/
                    i.Write(ms);
                }
                script = ms.ToArray();
            }

            // Prepare engine

            Engine = config.CreateEngine();

            // Load script
            Engine.LoadScript(script, false);
            //Engine.LoadScript(verifiable.Scripts[i].InvocationScript, true); // VerifyScripts

            if (Config == null || Engine == null)
            {
                State |= DebuggerState.Error;
            }
        }
        /// <summary>
        /// Step into
        /// </summary>
        public override void StepInto()
        {
            if (AreEnded()) return;

            // Remove breakpoint
            State &= ~DebuggerState.BreakPoint;

            if (Engine != null)
            {
                try
                {
                    _GasConsumed += Engine.GetPricePublic();

                    Engine.StepInto();

                    // Copy registers
                    InvocationStackCount = (uint)Engine.InvocationStack.Count;

                    // Copy stack
                    int evc = Engine.EvaluationStack.Count;
                    NeoStackItem[] it = new NeoStackItem[evc];

                    for (int x = 0, m = evc; x < m; x++)
                        it[x] = new NeoStackItem(Engine.EvaluationStack.Peek(x));

                    Stack.CopyFrom(it);

                    evc = Engine.AltStack.Count;
                    it = new NeoStackItem[evc];

                    for (int x = 0, m = evc; x < m; x++)
                        it[x] = new NeoStackItem(Engine.AltStack.Peek(x));

                    AltStack.CopyFrom(it);

                    // Copy state
                    if (Engine.State.HasFlag(VMState.HALT))
                    {
                        State |= DebuggerState.Halt;
                    }
                    else
                    {
                        // Only Copy when not halt
                        if (OffsetToIndex((uint)Engine.CurrentContext.InstructionPointer, out uint index))
                            CurrentInstructionIndex = index;
                    }

                    if (Engine.State.HasFlag(VMState.FAULT))
                        State |= DebuggerState.Error;
                }
                catch (Exception e)
                {
                    // Error
                    State |= DebuggerState.Error;
                    throw (e);
                }
            }
            else
            {
                State |= DebuggerState.Disposed;
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            // Clean engine
            if (Engine != null)
            {
                Engine.Dispose();
                Engine = null;
            }
        }
    }
}
