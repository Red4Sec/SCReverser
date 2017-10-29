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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

            bool leaveOpen;
            Stream stream = ics.GetStream(out leaveOpen);

            OffsetRelationCache offsetCache = new OffsetRelationCache();

            try
            {
                #region load file if are json FileStream
                if (stream is FileStream fi && Path.GetExtension(fi.Name).ToLowerInvariant() == ".json")
                {
                    long originalPos = stream.Position;
                    byte[] all = new byte[stream.Length - originalPos];
                    stream.Read(all, 0, all.Length);

                    string json = Encoding.UTF8.GetString(all, 0, all.Length);

                    result = JsonHelper.Deserialize<ReverseResult>(json, true);
                    if (result != null)
                    {
                        // Fill cache
                        offsetCache.FillWith(result.Instructions);

                        // Process instructions (Jumps)
                        using (MemoryStream ms = new MemoryStream())
                        {
                            foreach (Instruction i in result.Instructions)
                            {
                                ProcessInstruction(i, offsetCache);
                                i.Write(ms);
                            }
                            result.Bytes = ms.ToArray();
                        }

                        return true;
                    }

                    stream.Seek(originalPos, SeekOrigin.Begin);
                }
                #endregion

                PrapareResultForOcurrences(result);

                long max = stream.Length;
                int percent = 0, newPercent = 0;

                List<Instruction> recallJump = new List<Instruction>();

                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        byte[] opCode = new byte[OpCodeSize];

                        if (stream.Read(opCode, 0, OpCodeSize) != OpCodeSize)
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
                        uint rBytes = arg.Read(stream);

                        Instruction ins = new Instruction()
                        {
                            Index = insNumber,
                            Offset = offset,
                            OpCode = new OpCode()
                            {
                                RawValue = opCode,
                                Name = read.OpCode,
                                Description = read.Description,
                                Flags = read.Flags
                            },
                            Argument = arg,
                            Comment = arg.ASCIIValue,
                        };

                        offsetCache.Add(ins);

                        ProcessInstruction(ins, offsetCache);

                        // Recall jumps
                        if (ins.Jump != null && !ins.Jump.IsDynamic && ins.Jump.Offset.HasValue && !ins.Jump.Index.HasValue)
                            recallJump.Add(ins);

                        result.Instructions.Add(ins);

                        #region Fill ocurrences
                        foreach (OcurrenceCollection ocur in result.Ocurrences.Values)
                        {
                            if (ocur.Checker != null && ocur.Checker(ins, out string val))
                                ocur.Append(val, 1);
                        }
                        #endregion

                        offset += (uint)(rBytes + OpCodeSize);
                        insNumber++;

                        newPercent = (int)((offset * 100) / max);
                        if (percent != newPercent)
                        {
                            percent = newPercent;
                            OnParseProgress?.Invoke(this, percent);
                        }

                        ins.Write(ms);
                    }

                    result.Bytes = ms.ToArray();
                }

                foreach (Instruction j in recallJump)
                {
                    if (offsetCache.TryGetValue(j.Jump.Offset.Value, out uint index, OffsetIndexRelation.OffsetToIndex))
                    {
                        j.Jump = new Jump(j.Jump.Offset.Value, index);
                    }
                    else
                    {
                        // If enter here, there will be an error
                        j.Jump = null;
                    }
                }

                // Remove empty ocurrences
                foreach (string key in result.Ocurrences.Keys.ToArray())
                {
                    if (result.Ocurrences[key].Count <= 0)
                        result.Ocurrences.Remove(key);
                }

                return result.Instructions.Count > 0;
            }
            catch (Exception er)
            {
                throw (er);
            }
            finally
            {
                if (!leaveOpen && stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }
        /// <summary>
        /// Process instruction
        /// </summary>
        /// <param name="ins">Instruction</param>
        /// <param name="offsetToIndexCache">Cache</param>
        public virtual void ProcessInstruction(Instruction ins, OffsetRelationCache offsetToIndexCache) { }
    }
}
