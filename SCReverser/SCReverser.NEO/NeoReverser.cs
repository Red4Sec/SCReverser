using Neo;
using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
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
                // Detect bad property optimization
                case nameof(NeoOpCode.DROP):
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
                                        if (i.OpCode.Name != nameof(NeoOpCode.FROMALTSTACK))
                                        {
                                            // Detect Push / Drop
                                            if (i.OpCode.Name.StartsWith("PUSH"))
                                            {
                                                ins.Flags = InstructionFlag.UnusableCode;
                                                ins.ApplyColorForFlags();
                                                i.Flags = InstructionFlag.UnusableCode;
                                                i.ApplyColorForFlags();
                                            }
                                            return;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        if (i.OpCode.Name != nameof(NeoOpCode.NOP)) return;
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
                                        if (i.OpCode.Name != nameof(NeoOpCode.TOALTSTACK)) return;
                                        break;
                                    }
                                case 5:
                                    {
                                        if (i.OpCode.Name != nameof(NeoOpCode.NEWARRAY)) return;
                                        break;
                                    }
                                case 6:
                                    {
                                        if (i.OpCode.Name != nameof(NeoOpCode.PUSH0)) return;
                                        break;
                                    }
                            }
                        }

                        if (y == 7)
                        {
                            ins.Flags = InstructionFlag.UnusableCode;
                            ins.ApplyColorForFlags();

                            y = 1;
                            for (int x = (int)ins.Location.Index - 1, m = x - 6; x > m; x--, y++)
                            {
                                if (y == 3) continue;

                                Instruction i = bag[x];

                                i.Flags = InstructionFlag.UnusableCode;
                                i.ApplyColorForFlags();
                            }
                        }
                        break;
                    }
                // Detect NOP
                case nameof(NeoOpCode.NOP):
                    {
                        ins.Flags = InstructionFlag.UnusableCode;
                        ins.ApplyColorForFlags();
                        break;
                    }
                // Detect To/From ALTSTACK
                case nameof(NeoOpCode.FROMALTSTACK):
                    {
                        if (ins.Location.Index != 0)
                        {
                            Instruction prev = bag[ins.Location.Index - 1];
                            if (prev != null && prev.OpCode != null && prev.OpCode.Name == nameof(NeoOpCode.TOALTSTACK))
                            {
                                prev.Flags = InstructionFlag.UnusableCode;
                                ins.Flags = InstructionFlag.UnusableCode;

                                prev.ApplyColorForFlags();
                                ins.ApplyColorForFlags();
                            }
                        }
                        break;
                    }
                case nameof(NeoOpCode.RET):
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
                case nameof(NeoOpCode.CALL):
                case nameof(NeoOpCode.JMP):
                    {
                        if (!(ins.Argument is OpCodeShortArgument a)) return;

                        uint offset = (uint)a.Value;

                        if (offset == 3)
                        {
                            // Detect JMP to next line
                            ins.Flags = InstructionFlag.UnusableCode;
                            ins.ApplyColorForFlags();
                        }

                        offset = ins.Location.Offset + offset;

                        if (offsetToIndexCache.TryGetValue(offset, out uint ix, OffsetIndexRelation.OffsetToIndex))
                            ins.Jump = new Jump(offset, ix);
                        else
                            ins.Jump = new Jump(offset, uint.MaxValue);

                        if (string.IsNullOrEmpty(ins.Comment))
                            ins.Comment = ins.OpCode.Name.Substring(0, 1).ToUpper() +
                                ins.OpCode.Name.Substring(1).ToLower() + " to 0x" + offset.ToString("X4");
                        break;
                    }
                case nameof(NeoOpCode.JMPIF):
                case nameof(NeoOpCode.JMPIFNOT):
                    {
                        if (!(ins.Argument is OpCodeShortArgument a)) return;

                        bool check = ins.OpCode.Name == nameof(NeoOpCode.JMPIF);
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
                                        else
                                            i.Jump.Style = DashStyle.Dot;
                                    }
                                    return offset;
                                }
                                catch
                                {
                                    //i.Jump.Style = DashStyle.DashDot;
                                    //return offset;
                                }

                                return null;
                            })
                        );
                        break;
                    }
            }
        }
    }
}