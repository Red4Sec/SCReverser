using SCReverser.Core.Attributes;
using SCReverser.Core.Collections;
using SCReverser.Core.Delegates;
using SCReverser.Core.Enums;
using SCReverser.Core.Exceptions;
using SCReverser.Core.Extensions;
using SCReverser.Core.Helpers;
using SCReverser.Core.OpCodeArguments;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SCReverser.Core.Interfaces
{
    public class ReverserBase<T> : IReverser
        where T : struct, IConvertible
    {
        /// <summary>
        /// OnProgress
        /// </summary>
        public event OnProgressDelegate OnParseProgress;
        /// <summary>
        /// OpCode Size
        /// </summary>
        static public readonly int OpCodeSize;
        /// <summary>
        /// OpCache
        /// </summary>
        static Dictionary<string, OpCodeArgumentAttribute> OpCodeCache;
        /// <summary>
        /// Constructor
        /// </summary>
        static ReverserBase()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum) throw (new ArgumentException("T"));

            // Get type and size from underlying type
            Type enumTypeBase = Enum.GetUnderlyingType(enumType);
            OpCodeSize = Marshal.SizeOf(enumTypeBase);
            OpCodeCache = new Dictionary<string, OpCodeArgumentAttribute>();

            foreach (object t in Enum.GetValues(enumType))
            {
                // Get enumn member
                MemberInfo[] memInfo = enumType.GetMember(t.ToString());
                if (memInfo == null || memInfo.Length != 1)
                    throw (new FormatException());

                DescriptionAttribute desc = memInfo[0].GetCustomAttribute<DescriptionAttribute>();
                OpCodeArgumentAttribute opa = memInfo[0].GetCustomAttribute<OpCodeArgumentAttribute>();

                if (opa == null)
                    throw (new FormatException());

                if (desc != null && string.IsNullOrEmpty(opa.Description))
                    opa.Description = desc.Description;

                opa.OpCode = t.ToString();

                //  Get byte array from underlying type type
                byte[] d;
                object value = Convert.ChangeType(t, enumTypeBase);

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Byte: d = new byte[] { (byte)value }; break;
                    case TypeCode.Int16: d = ((short)value).ToByteArray(); break;
                    case TypeCode.UInt16: d = ((ushort)value).ToByteArray(); break;
                    case TypeCode.Int32: d = ((int)value).ToByteArray(); break;
                    case TypeCode.UInt32: d = ((uint)value).ToByteArray(); break;
                    case TypeCode.Int64: d = ((long)value).ToByteArray(); break;
                    case TypeCode.UInt64: d = ((ulong)value).ToByteArray(); break;

                    default: throw (new ArgumentException("T"));
                }

                // Append to cache
                OpCodeCache.Add(d.ToHexString(), opa);
            }
        }
        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="result">Result</param>
        public virtual void PrapareResultForOcurrences(ReverseResult result)
        {
            if (result == null) return;

            if (!result.Ocurrences.ContainsKey("Strings"))
                result.Ocurrences["Strings"] = new OcurrenceCollection() { Checker = StringCheckOcurrence };
            if (!result.Ocurrences.ContainsKey("OpCodes"))
                result.Ocurrences["OpCodes"] = new OcurrenceCollection() { Checker = OpCodesCheckOcurrence };
        }
        /// <summary>
        /// Check if instruction have OpCode
        /// </summary>
        /// <param name="i">Instruction</param>
        /// <param name="name">OpCode name</param>
        bool OpCodesCheckOcurrence(Instruction i, out string name)
        {
            name = i.OpCode.Name;
            return !string.IsNullOrEmpty(name);
        }
        /// <summary>
        /// Check if instruction have ASCII value
        /// </summary>
        /// <param name="i">Instruction</param>
        /// <param name="name">ASCII value</param>
        bool StringCheckOcurrence(Instruction i, out string name)
        {
            name = i.Argument == null ? "" : i.Argument.ASCIIValue;
            return !string.IsNullOrEmpty(name);
        }
        /// <summary>
        /// Get instructions from stream
        /// </summary>
        /// <param name="initClass">Init class</param>
        /// <param name="result">Result</param>
        public virtual bool TryParse(object initClass, ref ReverseResult result)
        {
            uint insNumber = 0;
            uint offset = 0;

            if (result == null) result = new ReverseResult() { };

            if (initClass == null || !(initClass is IInitClassStream ics))
                return false;

            byte[] all;
            OffsetRelationCache offsetCache = new OffsetRelationCache();
            List<Instruction> recallJump = new List<Instruction>();
            List<Instruction> calls = new List<Instruction>();

            PrapareResultForOcurrences(result);

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (StreamModule module in ics.GetStream())
                {
                    Types.Module mAdd = new Types.Module()
                    {
                        Name = module.Name,
                        Start = new IndexOffset()
                        {
                            Offset = (uint)ms.Position,
                            Index = (uint)result.Instructions.Count
                        },
                        Color = module.Color
                    };

                    Method mEntryPoint = new Method()
                    {
                        Start = mAdd.Start,
                        Name = "EntryPoint of " + module.Name,
                    };

                    Instruction lastIns = null;

                    try
                    {
                        if (module.Stream is FileStream fi && Path.GetExtension(fi.Name).ToLowerInvariant() == ".json")
                        {
                            #region load file if are json FileStream
                            long originalPos = module.Stream.Position;
                            all = new byte[module.Stream.Length - originalPos];
                            module.Stream.Read(all, 0, all.Length);

                            string json = Encoding.UTF8.GetString(all, 0, all.Length);

                            result = JsonHelper.Deserialize<ReverseResult>(json, true);
                            if (result != null)
                            {
                                // Fill cache
                                offsetCache.FillWith(result.Instructions);

                                // Process instructions (Jumps)
                                using (MemoryStream msX = new MemoryStream())
                                {
                                    foreach (Instruction i in result.Instructions)
                                    {
                                        ProcessInstruction(i, offsetCache);
                                        i.Write(msX);
                                    }
                                    result.Bytes = msX.ToArray();
                                }

                                return true;
                            }

                            module.Stream.Seek(originalPos, SeekOrigin.Begin);
                            #endregion
                        }

                        long max = module.Stream.Length;
                        int percent = 0, newPercent = 0;

                        while (true)
                        {
                            byte[] opCode = new byte[OpCodeSize];

                            if (module.Stream.Read(opCode, 0, OpCodeSize) != OpCodeSize)
                                break;

                            string key = opCode.ToHexString();

                            OpCodeArgumentAttribute read;
                            if (!OpCodeCache.TryGetValue(key, out read) || read == null)
                                throw (new OpCodeNotFoundException()
                                {
                                    Offset = offset,
                                    OpCode = opCode,
                                });

                            OpCodeEmptyArgument arg = read.Create();
                            uint rBytes = arg.Read(module.Stream);

                            lastIns = new Instruction()
                            {
                                OpCode = new OpCode()
                                {
                                    RawValue = opCode,
                                    Name = read.OpCode,
                                    Description = read.Description,
                                    Flags = read.Flags
                                },
                                Argument = arg.GetType() == typeof(OpCodeEmptyArgument) ? null : arg,
                                Comment = arg.ASCIIValue,
                                Color = module.Color,
                            };

                            lastIns.Location.Index = insNumber;
                            lastIns.Location.Offset = offset;

                            offsetCache.Add(lastIns.Location);

                            if (lastIns.OpCode.Flags.HasFlag(OpCodeFlag.IsCall))
                                calls.Add(lastIns);
                            else
                            {
                                if (mEntryPoint != null && lastIns.OpCode.Flags.HasFlag(OpCodeFlag.IsRet))
                                {
                                    mEntryPoint.End = lastIns.Location;

                                    mAdd.Methods.Add(mEntryPoint);
                                    mEntryPoint = null;
                                }
                            }
                            ProcessInstruction(lastIns, offsetCache);

                            // Recall jumps
                            if (lastIns.Jump != null && !lastIns.Jump.IsDynamic && lastIns.Jump.To.Index == uint.MaxValue)
                                recallJump.Add(lastIns);

                            result.Instructions.Add(lastIns);

                            #region Fill ocurrences
                            foreach (OcurrenceCollection ocur in result.Ocurrences.Values)
                            {
                                if (ocur.Checker != null && ocur.Checker(lastIns, out string val))
                                    ocur.Append(val, 1);
                            }
                            #endregion

                            offset += (uint)(rBytes + OpCodeSize);
                            insNumber++;

                            newPercent = (int)((module.Stream.Position * 100) / max);
                            if (percent != newPercent)
                            {
                                percent = newPercent;
                                OnParseProgress?.Invoke(this, percent);
                            }

                            lastIns.Write(ms);
                        }

                        if (mEntryPoint != null)
                        {
                            mEntryPoint.End = lastIns.Location;

                            mAdd.Methods.Add(mEntryPoint);
                            mEntryPoint = null;
                        }
                    }
                    catch (Exception er)
                    {
                        result = null;
                        throw (er);
                    }
                    finally
                    {
                        if (module != null)
                            module.Dispose();
                    }

                    long pos = ms.Position;
                    mAdd.End = lastIns.Location;
                    mAdd.Size = (uint)(pos - mAdd.Start.Offset);

                    ms.Seek(mAdd.Start.Offset, SeekOrigin.Begin);

                    all = new byte[mAdd.Size];
                    ms.Read(all, 0, all.Length);

                    using (SHA1 sha = SHA1.Create())
                        mAdd.Hash = sha.ComputeHash(all, 0, all.Length).ToHexString();

                    result.Modules.Add(mAdd);
                }

                result.Bytes = ms.ToArray();
            }

            // Recall jumps
            foreach (Instruction j in recallJump)
            {
                if (offsetCache.TryGetValue(j.Jump.To.Offset, out uint index, OffsetIndexRelation.OffsetToIndex))
                {
                    j.Jump.To.Index = index;
                }
                else
                {
                    // If enter here, there will be an error
                    j.Jump = null;
                }
            }

            // Calculate methods
            foreach (Instruction j in calls)
            {
                if (j.Jump == null || j.Jump.IsDynamic) continue;

                Instruction to = result.Instructions[j.Jump.To.Index];
                if (to == null) continue;

                int asciiCount = 0;
                string ascii = "";

                foreach (Instruction ins in result.Instructions.Take(to.Location))
                {
                    if (ins.OpCode.Flags.HasFlag(OpCodeFlag.IsRet))
                    {
                        Types.Module m = result.Modules.GetModuleOf(to.Location);
                        if (m != null)
                        {
                            m.Methods.Add(new Method()
                            {
                                Start = to.Location,
                                End = ins.Location,
                                Name = "Method " + to.OffsetHex + (string.IsNullOrEmpty(ascii) || asciiCount != 1 ? "" : " [" + ascii + "]")
                            });
                        }
                        break;
                    }
                    else
                    {
                        // Find only one ascii value for name
                        if (ins.Argument != null)
                        {
                            string r = ins.Argument.ASCIIValue;

                            if (!string.IsNullOrEmpty(r))
                            {
                                ascii = r;
                                asciiCount++;
                            }
                        }
                    }
                }
            }

            // Style methods
            foreach (Types.Module mod in result.Modules)
            {
                foreach (Method met in mod.Methods)
                {
                    met.Size = 0;
                    Instruction first = null;
                    foreach (Instruction i in result.Instructions.Take(met.Start, met.End))
                    {
                        if (first == null)
                        {
                            first = i;
                            i.Comment = met.Name;
                            first.BorderStyle = RowBorderStyle.EmptyBottom;
                        }
                        else
                        {
                            first = i;
                            first.BorderStyle = RowBorderStyle.OnlyLeftAndRight;
                        }
                        met.Size += i.Size;
                    }
                    if (first != null)
                    {
                        first.BorderStyle = first.BorderStyle == RowBorderStyle.EmptyBottom ? RowBorderStyle.All : RowBorderStyle.EmptyTop;
                    }
                }

                mod.Methods.Sort();
            }

            result.Modules.Sort();

            // Remove empty ocurrences
            foreach (string key in result.Ocurrences.Keys.ToArray())
            {
                if (result.Ocurrences[key].Count <= 0)
                    result.Ocurrences.Remove(key);
            }

            return result.Instructions.Count > 0;
        }
        /// <summary>
        /// Process instruction
        /// </summary>
        /// <param name="ins">Instruction</param>
        /// <param name="offsetToIndexCache">Cache</param>
        public virtual void ProcessInstruction(Instruction ins, OffsetRelationCache offsetToIndexCache) { }
    }
}
