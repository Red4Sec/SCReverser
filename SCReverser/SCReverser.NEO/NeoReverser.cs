using Neo;
using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
using System.Drawing;
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

            // Append UInt160
            if (!result.Ocurrences.ContainsKey("UInt160-Addresses"))
                result.Ocurrences["UInt160-Addresses"] = new OcurrenceCollection() { Checker = UInt160AddressesCheckOcurrence };

            // Append UInt256
            if (!result.Ocurrences.ContainsKey("UInt256-Addresses"))
                result.Ocurrences["UInt256-Addresses"] = new OcurrenceCollection() { Checker = UInt256AddressesCheckOcurrence };
        }
        /// <summary>
        /// Check if Instruction have any UInt256 Addresses
        /// </summary>
        /// <param name="instruction">Instruction</param>
        /// <param name="name">Name</param>
        bool UInt256AddressesCheckOcurrence(Instruction instruction, out string name)
        {
            if (instruction == null || instruction.Argument == null)
            {
                name = null;
                return false;
            }

            byte[] d = instruction.Argument.RawValue;

            if (d != null && d.Length == 32)
            {
                UInt256 r = new UInt256(d);
                name = r.ToString();
                return true;
            }

            name = null;
            return false;
        }
        /// <summary>
        /// Check if Instruction have any UInt160 Addresses
        /// </summary>
        /// <param name="instruction">Instruction</param>
        /// <param name="name">Name</param>
        bool UInt160AddressesCheckOcurrence(Instruction instruction, out string name)
        {
            if (instruction == null || instruction.Argument == null)
            {
                name = null;
                return false;
            }

            byte[] d = instruction.Argument.RawValue;

            if (d != null && d.Length == 20)
            {
                UInt160 r = new UInt160(d);
                name = r.ToString();
                return true;
            }

            name = null;
            return false;
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
        /// <param name="bag">Bag</param>
        /// <param name="ins">Instruction</param>
        /// <param name="offsetToIndexCache">Cache</param>
        public override void ProcessInstruction(InstructionCollection bag, Instruction ins, OffsetRelationCache offsetToIndexCache)
        {
            if (ins.OpCode == null) return;

            switch (ins.OpCode.Name)
            {
                case "DROP":
                    {
                        /*
                         0x03C5	PUSH0		Method 0x03C5 [R4S]
                         0x03C6	NEWARRAY		
                         0x03C7	TOALTSTACK		
That is the only valid > 0x03C8	PUSHBYTES3	0x523453	R4S
                         0x03CC	NOP		
                         0x03CD	FROMALTSTACK		
                       > 0x03CE	DROP		
                        */

                        int y = 1;
                        for (int x = (int)ins.Location.Index - 1; x >= 0 && y <= 6; x--, y++)
                        {
                            Instruction i = bag[x];
                            if (i == null || i.OpCode == null) return;

                            switch (y)
                            {
                                case 1:
                                    {
                                        if (i.OpCode.Name != "FROMALTSTACK") return;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (i.OpCode.Name != "NOP") return;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (!i.OpCode.Name.StartsWith("PUSH"))
                                            return;

                                        break;
                                    }
                                case 4:
                                    {
                                        if (i.OpCode.Name != "TOALTSTACK") return;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (i.OpCode.Name != "NEWARRAY") return;
                                        break;
                                    }
                                case 6:
                                    {
                                        if (i.OpCode.Name != "PUSH0") return;
                                        break;
                                    }
                            }
                        }

                        if (y == 7)
                        {
                            ins.Flags = InstructionFlag.DeadCode;
                            ins.Color = Color.FromArgb(10, Color.Black);

                            y = 1;
                            for (int x = (int)ins.Location.Index - 1, m = x - 6; x > m; x--, y++)
                            {
                                if (y == 3) continue;

                                Instruction i = bag[x];

                                i.Flags = InstructionFlag.DeadCode;
                                i.Color = Color.FromArgb(10, Color.Black);
                            }
                        }
                        break;
                    }
                case "NOP":
                    {
                        ins.Flags = InstructionFlag.DeadCode;
                        ins.Color = Color.FromArgb(10, Color.Black);
                        break;
                    }
                case "FROMALTSTACK":
                    {
                        if (ins.Location.Index != 0)
                        {
                            Instruction prev = bag[ins.Location.Index - 1];
                            if (prev != null && prev.OpCode != null && prev.OpCode.Name == "TOALTSTACK")
                            {
                                prev.Flags = InstructionFlag.DeadCode;
                                prev.Color = Color.FromArgb(10, Color.Black);

                                ins.Flags = InstructionFlag.DeadCode;
                                ins.Color = Color.FromArgb(10, Color.Black);
                            }
                        }
                        break;
                    }
                case "RET":
                    {
                        ins.Jump = new Jump(new OnJumpDelegate(
                            (d, i) =>
                            {
                                if (d == null || d.CurrentInstructionIndex != i.Location.Index || !(d is NeoDebugger neodebug))
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
                        if (!(ins.Argument is OpCodeShortArgument a)) return;

                        uint offset = (uint)a.Value;

                        if (offset == 3)
                        {
                            ins.Flags = InstructionFlag.DeadCode;
                            ins.Color = Color.FromArgb(10, Color.Black);
                        }

                        offset = ins.Location.Offset + offset;

                        uint? index = null;
                        if (offsetToIndexCache.TryGetValue(offset, out uint ix, OffsetIndexRelation.OffsetToIndex))
                            index = ix;

                        ins.Jump = new Jump(offset, uint.MaxValue);

                        if (string.IsNullOrEmpty(ins.Comment))
                            ins.Comment = ins.OpCode.Name.Substring(0, 1).ToUpper() + ins.OpCode.Name.Substring(1).ToLower() + " to 0x" + offset.ToString("X4");
                        break;
                    }
                case "JMPIF":
                case "JMPIFNOT":
                    {
                        if (!(ins.Argument is OpCodeShortArgument a)) return;

                        bool check = ins.OpCode.Name == "JMPIF";
                        uint offset = (uint)a.Value;

                        offset = ins.Location.Offset + offset;

                        if (string.IsNullOrEmpty(ins.Comment))
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
                                    if (d.CurrentInstructionIndex != i.Location.Index)
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