using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Interfaces;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace SCReverser.NEO
{
    public class NeoReverser : ReverserBase<NeoOpCode>
    {
        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="result">Resut</param>
        public override void PrapareResultForOcurrences(ReverseResult result)
        {
            base.PrapareResultForOcurrences(result);

            if (result == null) return;

            // Append syscall
            if (!result.Ocurrences.ContainsKey("SysCalls"))
                result.Ocurrences["SysCalls"] = new OcurrenceCollection() { Checker = SysCallCheckOcurrence };
        }
        /// <summary>
        /// Check if Instruction are SysCall
        /// </summary>
        /// <param name="instruction">Instruction</param>
        /// <param name="name">Name</param>
        bool SysCallCheckOcurrence(Instruction instruction, out string name)
        {
            if (instruction.Argument != null &&
                instruction.OpCode != null &&
                instruction.OpCode.Name == "SYSCALL")
            {
                name = instruction.Argument.ASCIIValue;
                return !string.IsNullOrEmpty(name);
            }

            name = null;
            return false;
        }
        /// <summary>
        /// Fill jumps
        /// </summary>
        /// <param name="ins">Instruction</param>
        /// <param name="offsetToIndexCache">Cache</param>
        public override void ProcessInstruction(Instruction ins, Dictionary<uint, uint> offsetToIndexCache)
        {
            if (ins.OpCode == null) return;

            switch (ins.OpCode.Name)
            {
                case "RET":
                    {
                        ins.Jump = new Jump(new OnJumpDelegate(
                            (d, i) =>
                            {
                                if (d == null || d.CurrentInstructionIndex != i.Index || !(d is NeoDebugger neodebug))
                                    return null;

                                try
                                {
                                    return (uint)neodebug.Engine.InvocationStack.Peek().InstructionPointer;
                                }
                                catch { }

                                return null;
                            })
                        );
                        break;
                    }
                case "CALL":
                case "JMP":
                    {
                        if (ins.Argument is OpCodeShortArgument a)
                        {
                            uint offset = (uint)a.Value;
                            offset = ins.Offset + offset;

                            uint? index = null;
                            if (offsetToIndexCache.TryGetValue(offset, out uint ix))
                                index = ix;

                            ins.Jump = new Jump(offset, index);

                            ins.Comment = "J" + ins.OpCode.Name.Substring(1).ToLower() + " to 0x" + offset.ToString("X4");
                        }
                        break;
                    }
                case "JMPIF":
                case "JMPIFNOT":
                    {
                        if (!(ins.Argument is OpCodeShortArgument a)) return;

                        bool check = ins.OpCode.Name == "JMPIF";
                        uint offset = (uint)a.Value;

                        offset = ins.Offset + offset;

                        ins.Comment = "Jump [" +
                            ins.OpCode.Name.Substring(3).ToLower() + "] to 0x" + offset.ToString("X4");

                        ins.Jump = new Jump(new OnJumpDelegate(
                            (d, i) =>
                            {
                                if (d == null || !(d is NeoDebugger neodebug))
                                    return null;

                                try
                                {
                                    // If not is the current (dot)
                                    if (d.CurrentInstructionIndex != i.Index)
                                        i.Jump.Style = DashStyle.Dot;
                                    else
                                    {
                                        // If condition are ok (no dot)
                                        if (neodebug.Engine.EvaluationStack.Peek().GetBoolean() == check)
                                            i.Jump.Style = DashStyle.Solid;
                                    }
                                    return offset;
                                }
                                catch { }

                                return null;
                            })
                        );
                        break;
                    }
            }
        }
    }
}