using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using SCReverser.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace SCReverser.NEO.Internals
{
    public class NeoEngine : ApplicationEngine
    {
        #region Limits
        /// <summary>
        /// Set the max size allowed size for BigInteger
        /// </summary>
        private const int MaxSizeForBigInteger = 32;
        /// <summary>
        /// Set the max Stack Size
        /// </summary>
        private const uint MaxStackSize = 2 * 1024;
        /// <summary>
        /// Set Max Item Size
        /// </summary>
        private const uint MaxItemSize = 1024 * 1024;
        /// <summary>
        /// Set Max Invocation Stack Size
        /// </summary>
        private const uint MaxInvocationStackSize = 1024;
        /// <summary>
        /// Set Max Array Size
        /// </summary>
        private const uint MaxArraySize = 1024;
        #endregion

        #region Variables
        /// <summary>
        /// Fake
        /// </summary>
        private readonly EFake Fake;

        static Type StorageContextType;
        static Type InteropInterfaceType;
        static MethodInfo StorageToArrayMethod;
        static FieldInfo InteropInterfaceFiled;

        Dictionary<string, byte[]> Storage = new Dictionary<string, byte[]>();

        static NeoEngine()
        {
            Assembly asm = typeof(ApplicationEngine).Assembly;

            StorageContextType = asm.GetType("Neo.SmartContract.StorageContext");
            StorageToArrayMethod = StorageContextType.GetMethod("ToArray");

            asm = typeof(StackItem).Assembly;
            InteropInterfaceType = asm.GetType("Neo.VM.Types.InteropInterface");
            InteropInterfaceFiled = InteropInterfaceType.GetField("_object", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        #endregion

        /// <summary>
        /// Load Storage
        /// </summary>
        void LoadStorage()
        {
            if (!Fake.HasFlag(EFake.Storage)) return;

            if (!File.Exists("storage.json"))
            {
                return;
            }

            string save = File.ReadAllText("storage.json", Encoding.UTF8);

            Storage = JsonHelper.Deserialize<Dictionary<string, byte[]>>(save);
            if (Storage == null) Storage = new Dictionary<string, byte[]>();
        }
        /// <summary>
        /// Save storage
        /// </summary>
        void SaveStorage()
        {
            string json = JsonHelper.Serialize(Storage);
            File.WriteAllText("storage.json", json, Encoding.UTF8);
        }
        /// <summary>
        /// Get StorageKey
        /// </summary>
        /// <param name="it">Item</param>
        string GetStorageKey(object it)
        {
            if (it.GetType() == InteropInterfaceType)
            {
                it = InteropInterfaceFiled.GetValue(it);
            }

            if (it.GetType() == StorageContextType)
            {
                object or = StorageToArrayMethod.Invoke(it, new object[] { });
                if (or == null) return "";

                return ((byte[])or).ToHexString();
            }
            return "";
        }

        #region Checks
        private bool CheckArraySize(OpCode nextInstruction)
        {
            switch (nextInstruction)
            {
                case OpCode.PACK:
                case OpCode.NEWARRAY:
                case OpCode.NEWSTRUCT:
                    {
                        if (EvaluationStack.Count == 0) return false;
                        int size = (int)EvaluationStack.Peek().GetBigInteger();
                        if (size > MaxArraySize) return false;
                        return true;
                    }
                default:
                    return true;
            }
        }

        private bool CheckInvocationStack(OpCode nextInstruction)
        {
            switch (nextInstruction)
            {
                case OpCode.CALL:
                case OpCode.APPCALL:
                    if (InvocationStack.Count >= MaxInvocationStackSize) return false;
                    return true;
                default:
                    return true;
            }
        }

        private bool CheckItemSize(OpCode nextInstruction)
        {
            switch (nextInstruction)
            {
                case OpCode.PUSHDATA4:
                    {
                        if (CurrentContext.InstructionPointer + 4 >= CurrentContext.Script.Length)
                            return false;
                        uint length = ToUInt32(CurrentContext.Script, CurrentContext.InstructionPointer + 1);
                        if (length > MaxItemSize) return false;
                        return true;
                    }
                case OpCode.CAT:
                    {
                        if (EvaluationStack.Count < 2) return false;

                        int length = EvaluationStack.Peek(0).GetByteArray().Length + EvaluationStack.Peek(1).GetByteArray().Length;
                        if (length > MaxItemSize) return false;
                        return true;
                    }
                default:
                    return true;
            }
        }

        /// <summary>
        /// Check if the BigInteger is allowed for numeric operations
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Return True if are allowed, otherwise False</returns>
        private bool CheckBigInteger(BigInteger value)
        {
            return value == null ? false :
                value.ToByteArray().Length <= MaxSizeForBigInteger;
        }

        /// <summary>
        /// Check if the BigInteger is allowed for numeric operations
        /// </summary> 
        private bool CheckBigIntegers(OpCode nextInstruction)
        {
            switch (nextInstruction)
            {
                case OpCode.INC:
                    {
                        BigInteger x = EvaluationStack.Peek().GetBigInteger();

                        if (!CheckBigInteger(x) || !CheckBigInteger(x + 1))
                            return false;

                        break;
                    }
                case OpCode.DEC:
                    {
                        BigInteger x = EvaluationStack.Peek().GetBigInteger();

                        if (!CheckBigInteger(x) || (x.Sign <= 0 && !CheckBigInteger(x - 1)))
                            return false;

                        break;
                    }
                case OpCode.ADD:
                    {
                        BigInteger x2 = EvaluationStack.Peek().GetBigInteger();
                        BigInteger x1 = EvaluationStack.Peek(1).GetBigInteger();

                        if (!CheckBigInteger(x2) || !CheckBigInteger(x1) || !CheckBigInteger(x1 + x2))
                            return false;

                        break;
                    }
                case OpCode.SUB:
                    {
                        BigInteger x2 = EvaluationStack.Peek().GetBigInteger();
                        BigInteger x1 = EvaluationStack.Peek(1).GetBigInteger();

                        if (!CheckBigInteger(x2) || !CheckBigInteger(x1) || !CheckBigInteger(x1 - x2))
                            return false;

                        break;
                    }
                case OpCode.MUL:
                    {
                        BigInteger x2 = EvaluationStack.Peek().GetBigInteger();
                        BigInteger x1 = EvaluationStack.Peek(1).GetBigInteger();

                        int lx1 = x1 == null ? 0 : x1.ToByteArray().Length;

                        if (lx1 > MaxSizeForBigInteger)
                            return false;

                        int lx2 = x2 == null ? 0 : x2.ToByteArray().Length;

                        if ((lx1 + lx2) > MaxSizeForBigInteger)
                            return false;

                        break;
                    }
                case OpCode.DIV:
                    {
                        BigInteger x2 = EvaluationStack.Peek().GetBigInteger();
                        BigInteger x1 = EvaluationStack.Peek(1).GetBigInteger();

                        if (!CheckBigInteger(x2) || !CheckBigInteger(x1))
                            return false;

                        break;
                    }
                case OpCode.MOD:
                    {
                        BigInteger x2 = EvaluationStack.Peek().GetBigInteger();
                        BigInteger x1 = EvaluationStack.Peek(1).GetBigInteger();

                        if (!CheckBigInteger(x2) || !CheckBigInteger(x1))
                            return false;

                        break;
                    }
            }

            return true;
        }

        private bool CheckStackSize(OpCode nextInstruction)
        {
            int size = 0;
            if (nextInstruction <= OpCode.PUSH16)
                size = 1;
            else
                switch (nextInstruction)
                {
                    case OpCode.DEPTH:
                    case OpCode.DUP:
                    case OpCode.OVER:
                    case OpCode.TUCK:
                        {
                            size = 1;
                            break;
                        }
                    case OpCode.UNPACK:
                        {
                            StackItem item = EvaluationStack.Peek();
                            if (!item.IsArray) return false;
                            size = item.GetArray().Length;
                            break;
                        }
                }

            if (size == 0) return true;
            size += EvaluationStack.Count + AltStack.Count;
            if (size > MaxStackSize) return false;

            return true;
        }
        #endregion

        //long gas_consumed = 0;
        const Decimal Ratio = 100000;

        public NeoEngine(TriggerType trigger, IScriptContainer container, IScriptTable table, InteropService service, EFake fake, Fixed8 gas, bool testMode = false)
            : base(trigger, container, table, service, gas, testMode)
        {
            Fake = fake;
            LoadStorage();
        }

        public new void StepInto()
        {
            if (CurrentContext.InstructionPointer < CurrentContext.Script.Length)
            {
                OpCode nextOpcode = CurrentContext.NextInstruction;

                switch (nextOpcode)
                {
                    case OpCode.SYSCALL:
                        {
                            if (Fake != EFake.None)
                            {
                                // Read api name
                                byte length = CurrentContext.Script[CurrentContext.InstructionPointer + 1];
                                string api_name = Encoding.ASCII.GetString(CurrentContext.Script, CurrentContext.InstructionPointer + 2, length);

                                switch (api_name)
                                {
                                    case "Neo.Transaction.GetReferences":
                                        {
                                            EvaluationStack.Push(new StackItem[] { });

                                            // Read OpCode + Length + String
                                            CurrentContext.InstructionPointer += 2 + length;

                                            return;
                                        }

                                    #region Fake Witness
                                    case "Neo.Runtime.CheckWitness":
                                    case "AntShares.Runtime.CheckWitness":
                                        {
                                            if (Fake.HasFlag(EFake.Witness))
                                            {
                                                // Fake witness
                                                EvaluationStack.Pop();
                                                EvaluationStack.Push(true);

                                                // Read OpCode
                                                CurrentContext.InstructionPointer += 2 + length;
                                                return;
                                            }
                                            break;
                                        }
                                    #endregion

                                    #region Fake Storage
                                    case "Neo.Storage.Get":
                                    case "AntShares.Storage.Get":
                                        {
                                            if (Fake.HasFlag(EFake.Storage))
                                            {
                                                //object sk =
                                                string key = GetStorageKey(EvaluationStack.Pop());
                                                byte[] data = EvaluationStack.Pop().GetByteArray();
                                                if (data == null) data = new byte[] { };

                                                if (!Storage.TryGetValue(key + data.ToHexString(), out data))
                                                {
                                                    EvaluationStack.Push(new byte[0]);
                                                }
                                                else
                                                {
                                                    EvaluationStack.Push(data);
                                                }

                                                // Read OpCode + Length + String
                                                CurrentContext.InstructionPointer += 2 + length;
                                                return;
                                            }
                                            break;
                                        }
                                    case "Neo.Storage.Delete":
                                    case "AntShares.Storage.Delete":
                                        {
                                            if (Fake.HasFlag(EFake.Storage))
                                            {
                                                //object sk =
                                                string key = GetStorageKey(EvaluationStack.Pop());
                                                byte[] data = EvaluationStack.Pop().GetByteArray();
                                                if (data == null) data = new byte[] { };

                                                Storage.Remove(key + data.ToHexString());

                                                // Read OpCode + Length + String
                                                CurrentContext.InstructionPointer += 2 + length;

                                                SaveStorage();
                                                return;
                                            }
                                            break;
                                        }
                                    case "Neo.Storage.Put":
                                    case "AntShares.Storage.Put":
                                        {
                                            if (Fake.HasFlag(EFake.Storage))
                                            {
                                                string key = GetStorageKey(EvaluationStack.Pop());
                                                byte[] data = EvaluationStack.Pop().GetByteArray();
                                                byte[] data2 = EvaluationStack.Pop().GetByteArray();
                                                if (data == null) data = new byte[] { };

                                                Storage[key + data.ToHexString()] = data2;

                                                // Read OpCode + Length + String
                                                CurrentContext.InstructionPointer += 2 + length;

                                                SaveStorage();
                                                return;
                                            }
                                            break;
                                        }
                                        #endregion
                                }
                            }

                            break;
                        }
                    case OpCode.CHECKSIG:
                        {
                            if (Fake.HasFlag(EFake.Signature))
                            {
                                // Read OpCode
                                CurrentContext.InstructionPointer++;

                                // Fake signature
                                byte[] pubkey = EvaluationStack.Pop().GetByteArray();
                                byte[] signature = EvaluationStack.Pop().GetByteArray();
                                EvaluationStack.Push(true);

                                return;
                            }

                            break;
                        }
                    case OpCode.CHECKMULTISIG:
                        {
                            if (Fake.HasFlag(EFake.Signature))
                            {

                            }

                            break;
                        }
                }

                //gas_consumed = checked(gas_consumed + GetPrice(nextOpcode) * ratio);
                //if (!testMode && gas_consumed > gas_amount) return false;

                if (!CheckItemSize(nextOpcode))
                    throw (new Exception("VM-Limits Raised [CheckItemSize]"));

                if (!CheckStackSize(nextOpcode))
                    throw (new Exception("VM-Limits Raised [CheckStackSize]"));

                if (!CheckArraySize(nextOpcode))
                    throw (new Exception("VM-Limits Raised [CheckArraySize]"));

                if (!CheckInvocationStack(nextOpcode))
                    throw (new Exception("VM-Limits Raised [CheckInvocationStack]"));

                if (!CheckBigIntegers(nextOpcode))
                    throw (new Exception("VM-Limits Raised [CheckBigIntegers]"));
            }

            base.StepInto();
        }

        unsafe static uint ToUInt32(byte[] value, int startIndex)
        {
            fixed (byte* pbyte = &value[startIndex])
            {
                return *((uint*)pbyte);
            }
        }

        public Decimal GetPricePublic()
        {
            return (base.GetPrice() * Ratio) / (Decimal)Math.Pow(10, Blockchain.UtilityToken.Precision);
        }
    }
}