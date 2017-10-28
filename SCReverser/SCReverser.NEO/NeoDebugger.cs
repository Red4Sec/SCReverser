using Neo.Core;
using Neo.SmartContract;
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
        bool HaveBlockChain;
        ApplicationEngine Engine;

        /// <summary>
        /// Gas consumed
        /// </summary>
        [Category("NEO")]
        public string GasConsumed { get; private set; }

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

            Blockchain bc;
            Engine = config.CreateEngine(out bc);

            // Register blockchain
            if (bc != null)
            {
                HaveBlockChain = true;
                
                // Prevent double dispose errors
                Blockchain.RegisterBlockchain(new NullBlockChain());
                Blockchain.RegisterBlockchain(bc);
            }
            // Load script
            Engine.LoadScript(script, false);

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
                    Engine.StepInto();

                    // Copy state
                    if (Engine.State.HasFlag(VMState.HALT))
                        State |= DebuggerState.Halt;
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
            if (Engine != null)
            {
                Engine.Dispose();
                Engine = null;
            }

            // Free blockchain
            if (HaveBlockChain)
            {
                Blockchain.RegisterBlockchain(new NullBlockChain());
            }
        }
    }
}
