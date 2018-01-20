using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Numerics;

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
                        size = 1;
                        break;
                    case OpCode.UNPACK:
                        StackItem item = EvaluationStack.Peek();
                        if (!item.IsArray) return false;
                        size = item.GetArray().Length;
                        break;
                }
            if (size == 0) return true;
            size += EvaluationStack.Count + AltStack.Count;
            if (size > MaxStackSize) return false;
            return true;
        }
        #endregion

        //long gas_consumed = 0;
        const Decimal Ratio = 100000;

        public NeoEngine(TriggerType trigger, IScriptContainer container, IScriptTable table, InteropService service, Fixed8 gas, bool testMode = false)
            : base(trigger, container, table, service, gas, testMode)
        {
        }

        public new void StepInto()
        {
            Check();
            base.StepInto();
        }

        void Check()
        {
            if (CurrentContext.InstructionPointer < CurrentContext.Script.Length)
            {
                OpCode nextOpcode = CurrentContext.NextInstruction;

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