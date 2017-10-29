using SCReverser.Core.Attributes;
using SCReverser.Core.OpCodeArguments;
using SCReverser.NEO.OpCodeArguments;
using System.ComponentModel;

namespace SCReverser.NEO
{
    public enum NeoOpCode : byte
    {
        #region Constants
        [OpCodeArgument]
        [Description("An empty array of bytes is pushed onto the stack.")]
        PUSH0 = 0x00,

        #region PUSHBYTES 0x01-0x4B
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x01 })]
        [Description("0x01 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES1 = 0x01,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x02 })]
        [Description("0x02 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES2 = 0x02,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x03 })]
        [Description("0x03 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES3 = 0x03,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x04 })]
        [Description("0x04 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES4 = 0x04,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x05 })]
        [Description("0x05 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES5 = 0x05,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x06 })]
        [Description("0x06 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES6 = 0x06,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x07 })]
        [Description("0x07 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES7 = 0x07,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x08 })]
        [Description("0x08 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES8 = 0x08,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x09 })]
        [Description("0x09 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES9 = 0x09,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0A })]
        [Description("0x0A The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES10 = 0x0A,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0B })]
        [Description("0x0B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES11 = 0x0B,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0C })]
        [Description("0x0C The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES12 = 0x0C,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0D })]
        [Description("0x0D The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES13 = 0x0D,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0E })]
        [Description("0x0E The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES14 = 0x0E,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x0F })]
        [Description("0x0F The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES15 = 0x0F,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x10 })]
        [Description("0x10 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES16 = 0x10,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x11 })]
        [Description("0x11 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES17 = 0x11,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x12 })]
        [Description("0x12 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES18 = 0x12,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x13 })]
        [Description("0x13 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES19 = 0x13,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x14 })]
        [Description("0x14 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES20 = 0x14,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x15 })]
        [Description("0x15 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES21 = 0x15,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x16 })]
        [Description("0x16 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES22 = 0x16,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x17 })]
        [Description("0x17 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES23 = 0x17,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x18 })]
        [Description("0x18 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES24 = 0x18,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x19 })]
        [Description("0x19 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES25 = 0x19,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1A })]
        [Description("0x1A The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES26 = 0x1A,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1B })]
        [Description("0x1B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES27 = 0x1B,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1C })]
        [Description("0x1C The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES28 = 0x1C,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1D })]
        [Description("0x1D The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES29 = 0x1D,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1E })]
        [Description("0x1E The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES30 = 0x1E,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x1F })]
        [Description("0x1F The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES31 = 0x1F,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x20 })]
        [Description("0x20 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES32 = 0x20,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x21 })]
        [Description("0x21 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES33 = 0x21,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x22 })]
        [Description("0x22 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES34 = 0x22,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x23 })]
        [Description("0x23 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES35 = 0x23,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x24 })]
        [Description("0x24 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES36 = 0x24,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x25 })]
        [Description("0x25 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES37 = 0x25,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x26 })]
        [Description("0x26 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES38 = 0x26,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x27 })]
        [Description("0x27 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES39 = 0x27,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x28 })]
        [Description("0x28 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES40 = 0x28,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x29 })]
        [Description("0x29 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES41 = 0x29,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2A })]
        [Description("0x2A The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES42 = 0x2A,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2B })]
        [Description("0x2B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES43 = 0x2B,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2C })]
        [Description("0x2C The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES44 = 0x2C,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2D })]
        [Description("0x2D The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES45 = 0x2D,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2E })]
        [Description("0x2E The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES46 = 0x2E,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x2F })]
        [Description("0x2F The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES47 = 0x2F,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x30 })]
        [Description("0x30 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES48 = 0x30,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x31 })]
        [Description("0x31 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES49 = 0x31,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x32 })]
        [Description("0x32 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES50 = 0x32,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x33 })]
        [Description("0x33 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES51 = 0x33,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x34 })]
        [Description("0x34 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES52 = 0x34,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x35 })]
        [Description("0x35 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES53 = 0x35,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x36 })]
        [Description("0x36 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES54 = 0x36,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x37 })]
        [Description("0x37 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES55 = 0x37,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x38 })]
        [Description("0x38 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES56 = 0x38,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x39 })]
        [Description("0x39 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES57 = 0x39,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3A })]
        [Description("0x3A The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES58 = 0x3A,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3B })]
        [Description("0x3B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES59 = 0x3B,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3C })]
        [Description("0x3C The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES60 = 0x3C,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3D })]
        [Description("0x3D The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES61 = 0x3D,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3E })]
        [Description("0x3E The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES62 = 0x3E,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x3F })]
        [Description("0x3F The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES63 = 0x3F,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x40 })]
        [Description("0x40 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES64 = 0x40,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x41 })]
        [Description("0x41 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES65 = 0x41,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x42 })]
        [Description("0x42 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES66 = 0x42,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x43 })]
        [Description("0x43 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES67 = 0x43,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x44 })]
        [Description("0x44 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES68 = 0x44,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x45 })]
        [Description("0x45 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES69 = 0x45,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x46 })]
        [Description("0x46 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES70 = 0x46,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x47 })]
        [Description("0x47 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES71 = 0x47,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x48 })]
        [Description("0x48 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES72 = 0x48,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x49 })]
        [Description("0x49 The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES73 = 0x49,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x4A })]
        [Description("0x4A The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES74 = 0x4A,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x4B })]
        [Description("0x4B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES75 = 0x4B,
        #endregion

        [OpCodeArgument(typeof(OpCodeByteArgument))]
        [Description("The next byte contains the number of bytes to be pushed onto the stack.")]
        PUSHDATA1 = 0x4C,
        [OpCodeArgument(typeof(OpCodeUShortArgument))]
        [Description("The next two bytes contain the number of bytes to be pushed onto the stack.")]
        PUSHDATA2 = 0x4D,
        [OpCodeArgument(typeof(OpCodeIntArgument))]
        [Description("The next four bytes contain the number of bytes to be pushed onto the stack.")]
        PUSHDATA4 = 0x4E,

        [OpCodeArgument]
        [Description("The number -1 is pushed onto the stack.")]
        PUSHM1 = 0x4F,
        [OpCodeArgument]
        [Description("The number 1 is pushed onto the stack.")]
        PUSH1 = 0x51,
        [OpCodeArgument]
        [Description("The number 2 is pushed onto the stack.")]
        PUSH2 = 0x52,
        [OpCodeArgument]
        [Description("The number 3 is pushed onto the stack.")]
        PUSH3 = 0x53,
        [OpCodeArgument]
        [Description("The number 4 is pushed onto the stack.")]
        PUSH4 = 0x54,
        [OpCodeArgument]
        [Description("The number 5 is pushed onto the stack.")]
        PUSH5 = 0x55,
        [OpCodeArgument]
        [Description("The number 6 is pushed onto the stack.")]
        PUSH6 = 0x56,
        [OpCodeArgument]
        [Description("The number 7 is pushed onto the stack.")]
        PUSH7 = 0x57,
        [OpCodeArgument]
        [Description("The number 8 is pushed onto the stack.")]
        PUSH8 = 0x58,
        [OpCodeArgument]
        [Description("The number 9 is pushed onto the stack.")]
        PUSH9 = 0x59,
        [OpCodeArgument]
        [Description("The number 10 is pushed onto the stack.")]
        PUSH10 = 0x5A,
        [OpCodeArgument]
        [Description("The number 11 is pushed onto the stack.")]
        PUSH11 = 0x5B,
        [OpCodeArgument]
        [Description("The number 12 is pushed onto the stack.")]
        PUSH12 = 0x5C,
        [OpCodeArgument]
        [Description("The number 13 is pushed onto the stack.")]
        PUSH13 = 0x5D,
        [OpCodeArgument]
        [Description("The number 14 is pushed onto the stack.")]
        PUSH14 = 0x5E,
        [OpCodeArgument]
        [Description("The number 15 is pushed onto the stack.")]
        PUSH15 = 0x5F,
        [OpCodeArgument]
        [Description("The number 16 is pushed onto the stack.")]
        PUSH16 = 0x60,
        #endregion

        #region Flow control
        [OpCodeArgument]
        [Description("Does nothing.")]
        NOP = 0x61,
        [OpCodeArgument(typeof(OpCodeShortArgument))]
        JMP = 0x62,
        [OpCodeArgument(typeof(OpCodeShortArgument))]
        JMPIF = 0x63,
        [OpCodeArgument(typeof(OpCodeShortArgument))]
        JMPIFNOT = 0x64,
        [OpCodeArgument(typeof(OpCodeShortArgument))]
        CALL = 0x65,
        [OpCodeArgument]
        RET = 0x66,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 20 })]
        APPCALL = 0x67,
        [OpCodeArgument(typeof(OpCodeVarByteArrayArgument), ConstructorArguments = new object[] { 252 })]
        SYSCALL = 0x68,
        [OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 20 })]
        TAILCALL = 0x69,
        #endregion

        #region Stack
        [OpCodeArgument]
        DUPFROMALTSTACK = 0x6A,
        [Description("Puts the input onto the top of the alt stack. Removes it from the main stack.")]
        [OpCodeArgument]
        TOALTSTACK = 0x6B,
        [Description("Puts the input onto the top of the main stack. Removes it from the alt stack.")]
        [OpCodeArgument]
        FROMALTSTACK = 0x6C,
        [OpCodeArgument]
        XDROP = 0x6D,
        [OpCodeArgument]
        XSWAP = 0x72,
        [OpCodeArgument]
        XTUCK = 0x73,
        [Description("Puts the number of stack items onto the stack.")]
        [OpCodeArgument]
        DEPTH = 0x74,
        [Description("Removes the top stack item.")]
        [OpCodeArgument]
        DROP = 0x75,
        [Description("Duplicates the top stack item.")]
        [OpCodeArgument]
        DUP = 0x76,
        [Description("Removes the second-to-top stack item.")]
        [OpCodeArgument]
        NIP = 0x77,
        [Description("Copies the second-to-top stack item to the top.")]
        [OpCodeArgument]
        OVER = 0x78,
        [Description("The item n back in the stack is copied to the top.")]
        [OpCodeArgument]
        PICK = 0x79,
        [Description("The item n back in the stack is moved to the top.")]
        [OpCodeArgument]
        ROLL = 0x7A,
        [Description("The top three items on the stack are rotated to the left.")]
        [OpCodeArgument]
        ROT = 0x7B,
        [Description("The top two items on the stack are swapped.")]
        [OpCodeArgument]
        SWAP = 0x7C,
        [Description("The item at the top of the stack is copied and inserted before the second-to-top item.")]
        [OpCodeArgument]
        TUCK = 0x7D,
        #endregion

        #region Splice
        [Description("Concatenates two strings.")]
        [OpCodeArgument]
        CAT = 0x7E,
        [Description("Returns a section of a string.")]
        [OpCodeArgument]
        SUBSTR = 0x7F,
        [Description("Keeps only characters left of the specified point in a string.")]
        [OpCodeArgument]
        LEFT = 0x80,
        [Description("Keeps only characters right of the specified point in a string.")]
        [OpCodeArgument]
        RIGHT = 0x81,
        [Description("Returns the length of the input string.")]
        [OpCodeArgument]
        SIZE = 0x82,
        #endregion

        #region Bitwise logic
        [Description("Flips all of the bits in the input.")]
        [OpCodeArgument]
        INVERT = 0x83,
        [Description("Boolean and between each bit in the inputs.")]
        [OpCodeArgument]
        AND = 0x84,
        [Description("Boolean or between each bit in the inputs.")]
        [OpCodeArgument]
        OR = 0x85,
        [Description("Boolean exclusive or between each bit in the inputs.")]
        [OpCodeArgument]
        XOR = 0x86,
        [Description("Returns 1 if the inputs are exactly equal, 0 otherwise.")]
        [OpCodeArgument]
        EQUAL = 0x87,

        //[Description("Same as OP_EQUAL, but runs OP_VERIFY afterward.")]
        //[OpCodeArgument]
        //OP_EQUALVERIFY = 0x88, 
        //[Description("Transaction is invalid unless occuring in an unexecuted OP_IF branch")]
        //[OpCodeArgument]
        //OP_RESERVED1 = 0x89,
        //[Description("Transaction is invalid unless occuring in an unexecuted OP_IF branch")]
        //[OpCodeArgument]
        //OP_RESERVED2 = 0x8A,
        #endregion

        #region Arithmetic
        // Note: Arithmetic inputs are limited to signed 32-bit integers, but may overflow their output.
        [Description("1 is added to the input.")]
        [OpCodeArgument]
        INC = 0x8B,
        [Description("1 is subtracted from the input.")]
        [OpCodeArgument]
        DEC = 0x8C,
        [OpCodeArgument]
        SIGN = 0x8D,
        [Description("The sign of the input is flipped.")]
        [OpCodeArgument]
        NEGATE = 0x8F,
        [Description("The input is made positive.")]
        [OpCodeArgument]
        ABS = 0x90,
        [Description("If the input is 0 or 1, it is flipped. Otherwise the output will be 0.")]
        [OpCodeArgument]
        NOT = 0x91,
        [Description("Returns 0 if the input is 0. 1 otherwise.")]
        [OpCodeArgument]
        NZ = 0x92,
        [Description("a is added to b.")]
        [OpCodeArgument]
        ADD = 0x93,
        [Description("b is subtracted from a.")]
        [OpCodeArgument]
        SUB = 0x94,
        [Description("a is multiplied by b.")]
        [OpCodeArgument]
        MUL = 0x95,
        [Description("a is divided by b.")]
        [OpCodeArgument]
        DIV = 0x96,
        [Description("Returns the remainder after dividing a by b.")]
        [OpCodeArgument]
        MOD = 0x97,
        [Description("Shifts a left b bits, preserving sign.")]
        [OpCodeArgument]
        SHL = 0x98,
        [Description("Shifts a right b bits, preserving sign.")]
        [OpCodeArgument]
        SHR = 0x99,
        [Description("If both a and b are not 0, the output is 1. Otherwise 0.")]
        [OpCodeArgument]
        BOOLAND = 0x9A,
        [Description("If a or b is not 0, the output is 1. Otherwise 0.")]
        [OpCodeArgument]
        BOOLOR = 0x9B,
        [Description("Returns 1 if the numbers are equal, 0 otherwise.")]
        [OpCodeArgument]
        NUMEQUAL = 0x9C,
        [Description("Returns 1 if the numbers are not equal, 0 otherwise.")]
        [OpCodeArgument]
        NUMNOTEQUAL = 0x9E,
        [Description("Returns 1 if a is less than b, 0 otherwise.")]
        [OpCodeArgument]
        LT = 0x9F,
        [Description("Returns 1 if a is greater than b, 0 otherwise.")]
        [OpCodeArgument]
        GT = 0xA0,
        [Description("Returns 1 if a is less than or equal to b, 0 otherwise.")]
        [OpCodeArgument]
        LTE = 0xA1,
        [Description("Returns 1 if a is greater than or equal to b, 0 otherwise.")]
        [OpCodeArgument]
        GTE = 0xA2,
        [Description("Returns the smaller of a and b.")]
        [OpCodeArgument]
        MIN = 0xA3,
        [Description("Returns the larger of a and b.")]
        [OpCodeArgument]
        MAX = 0xA4,
        [Description("Returns 1 if x is within the specified range (left-inclusive), 0 otherwise.")]
        [OpCodeArgument]
        WITHIN = 0xA5,
        #endregion

        #region Crypto
        //[Description("The input is hashed using RIPEMD-160.")]
        //RIPEMD160 = 0xA6,
        [Description("The input is hashed using SHA-1.")]
        [OpCodeArgument]
        SHA1 = 0xA7,
        [Description("The input is hashed using SHA-256.")]
        [OpCodeArgument]
        SHA256 = 0xA8,
        [OpCodeArgument]
        HASH160 = 0xA9,
        [OpCodeArgument]
        HASH256 = 0xAA,
        [OpCodeArgument]
        CHECKSIG = 0xAC,
        [OpCodeArgument]
        CHECKMULTISIG = 0xAE,
        #endregion

        #region Array
        [OpCodeArgument]
        ARRAYSIZE = 0xC0,
        [OpCodeArgument]
        PACK = 0xC1,
        [OpCodeArgument]
        UNPACK = 0xC2,
        [OpCodeArgument]
        PICKITEM = 0xC3,
        [OpCodeArgument]
        SETITEM = 0xC4,
        [OpCodeArgument]
        NEWARRAY = 0xC5,
        [OpCodeArgument]
        NEWSTRUCT = 0xC6,
        #endregion

        #region Exceptions
        [OpCodeArgument]
        THROW = 0xF0,
        [OpCodeArgument]
        THROWIFNOT = 0xF1
        #endregion
    }
}