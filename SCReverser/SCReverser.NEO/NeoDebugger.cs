using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace SCReverser.NEO
{
    public class NeoDebugger : DebuggerBase
    {
        Blockchain BlockChain;
        NeoDebuggerConfig Config;
        ApplicationEngine Engine;

        /// <summary>
        /// Gas consumed
        /// </summary>
        public string GasConsumed { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public NeoDebugger(IEnumerable<Instruction> instructions) : base(instructions) { }
        /// <summary>
        /// Initialize
        /// </summary>
        public bool Initialize(NeoDebuggerConfig config)
        {
            // Set config
            if (config == null) config = new NeoDebuggerConfig();

            Config = config;

            // Create script
            byte[] script;
            using (MemoryStream ms = new MemoryStream())
            {
                //uint offset = 0;
                foreach (Instruction i in Instructions)
                {
                    //i.Offset = offset;
                    /*offset +=*/ i.Write(ms);
                }
                script = ms.ToArray();
            }

            // Prepare engine

            Engine = config.CreateEngine(out BlockChain);

            // Load script
            Engine.LoadScript(script, false);

            if (Config != null && Engine != null)
            {
                // Start
                State |= DebuggerState.Initialized;
                return true;
            }
            else
            {
                State |= DebuggerState.Error;
            }
            return false;
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
                    Engine.StepInto();

                    // Copy state
                    if (Engine.State.HasFlag(VMState.HALT))
                        State |= DebuggerState.Ended;
                    if (Engine.State.HasFlag(VMState.FAULT))
                        State |= DebuggerState.Error;

                    // Copy registers
                    GasConsumed = Engine.GasConsumed.ToString();
                    InvocationStackCount = (uint)Engine.InvocationStack.Count;
                    CurrentInstructionIndex = Offsets[(uint)Engine.CurrentContext.InstructionPointer];
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
            Engine.Dispose();
            Engine = null;

            // Free blockchain
            if (BlockChain != null)
            {
                BlockChain.Dispose();
                BlockChain = null;
            }
        }
    }
}