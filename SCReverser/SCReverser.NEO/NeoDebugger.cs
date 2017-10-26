using Neo.Cryptography;
using Neo.VM;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using System.IO;

namespace SCReverser.NEO
{
    public class NeoDebugger : DebuggerBase
    {
        class NeoScript : IScriptContainer
        {
            byte[] _Script;
            public NeoScript(byte[] script) { _Script = script; }

            public byte[] GetMessage() { return _Script; }
            public byte[] GetScript(byte[] script_hash) { return _Script; }
        }
        ExecutionEngine Engine;
        /// <summary>
        /// Run
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public NeoDebugger(Instruction[] instructions) : base(instructions)
        {
            byte[] script;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (Instruction i in instructions) i.Write(ms);
                script = ms.ToArray();
            }

            // Prepare engine

            IScriptContainer scriptContainer = new NeoScript(script);
            IScriptTable scriptTable = null;
            InteropService interop = null;

            Engine = new ExecutionEngine
                (
                scriptContainer,
                Crypto.Default,
                scriptTable,
                interop
                );
        }
        /// <summary>
        /// Step into
        /// </summary>
        public override void StepInto()
        {
            if (!AreEnded()) return;

            State &= ~DebuggerState.BreakPoint;

            if (Engine != null)
            {
                Engine.StepInto();

                InvocationStackCount = (uint)Engine.InvocationStack.Count;
                CurrentInstructionIndex = (uint)Engine.CurrentContext.InstructionPointer;
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

            Engine.Dispose();
            Engine = null;
        }
    }
}