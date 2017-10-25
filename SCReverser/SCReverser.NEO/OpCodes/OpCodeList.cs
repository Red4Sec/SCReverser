using System.ComponentModel;

namespace SCReverser.NEO.OpCodes
{
    public enum OpCodeList : byte
    {
        #region Constants
        [Description("An empty array of bytes is pushed onto the stack.")]
        PUSH0 = 0x00,
        PUSHF = PUSH0,
        [Description("0x01-0x4B The next opcode bytes is data to be pushed onto the stack.")]
        PUSHBYTES1 = 0x01,
        PUSHBYTES75 = 0x4B,
        [Description("The next byte contains the number of bytes to be pushed onto the stack.")]
        PUSHDATA1 = 0x4C,
        [Description("The next two bytes contain the number of bytes to be pushed onto the stack.")]
        PUSHDATA2 = 0x4D,
        [Description("The next four bytes contain the number of bytes to be pushed onto the stack.")]
        PUSHDATA4 = 0x4E,
        [Description("The number -1 is pushed onto the stack.")]
        PUSHM1 = 0x4F,
        [Description("The number 1 is pushed onto the stack.")]
        PUSH1 = 0x51,
        PUSHT = PUSH1,
        [Description("The number 2 is pushed onto the stack.")]
        PUSH2 = 0x52,
        [Description("The number 3 is pushed onto the stack.")]
        PUSH3 = 0x53,
        [Description("The number 4 is pushed onto the stack.")]
        PUSH4 = 0x54,
        [Description("The number 5 is pushed onto the stack.")]
        PUSH5 = 0x55,
        [Description("The number 6 is pushed onto the stack.")]
        PUSH6 = 0x56,
        [Description("The number 7 is pushed onto the stack.")]
        PUSH7 = 0x57,
        [Description("The number 8 is pushed onto the stack.")]
        PUSH8 = 0x58,
        [Description("The number 9 is pushed onto the stack.")]
        PUSH9 = 0x59,
        [Description("The number 10 is pushed onto the stack.")]
        PUSH10 = 0x5A,
        [Description("The number 11 is pushed onto the stack.")]
        PUSH11 = 0x5B,
        [Description("The number 12 is pushed onto the stack.")]
        PUSH12 = 0x5C,
        [Description("The number 13 is pushed onto the stack.")]
        PUSH13 = 0x5D,
        [Description("The number 14 is pushed onto the stack.")]
        PUSH14 = 0x5E,
        [Description("The number 15 is pushed onto the stack.")]
        PUSH15 = 0x5F,
        [Description("The number 16 is pushed onto the stack.")]
        PUSH16 = 0x60,
        #endregion

        #region Flow control
        [Description("Does nothing.")]
        NOP = 0x61,
        JMP = 0x62,
        JMPIF = 0x63,
        JMPIFNOT = 0x64,
        CALL = 0x65,
        RET = 0x66,
        APPCALL = 0x67,
        SYSCALL = 0x68,
        TAILCALL = 0x69,
        #endregion

        #region Stack
        DUPFROMALTSTACK = 0x6A,
        [Description("Puts the input onto the top of the alt stack. Removes it from the main stack.")]
        TOALTSTACK = 0x6B,
        [Description("Puts the input onto the top of the main stack. Removes it from the alt stack.")]
        FROMALTSTACK = 0x6C,
        XDROP = 0x6D,
        XSWAP = 0x72,
        XTUCK = 0x73,
        [Description("Puts the number of stack items onto the stack.")]
        DEPTH = 0x74,
        [Description("Removes the top stack item.")]
        DROP = 0x75,
        [Description("Duplicates the top stack item.")]
        DUP = 0x76,
        [Description("Removes the second-to-top stack item.")]
        NIP = 0x77,
        [Description("Copies the second-to-top stack item to the top.")]
        OVER = 0x78,
        [Description("The item n back in the stack is copied to the top.")]
        PICK = 0x79,
        [Description("The item n back in the stack is moved to the top.")]
        ROLL = 0x7A,
        [Description("The top three items on the stack are rotated to the left.")]
        ROT = 0x7B,
        [Description("The top two items on the stack are swapped.")]
        SWAP = 0x7C,
        [Description("The item at the top of the stack is copied and inserted before the second-to-top item.")]
        TUCK = 0x7D,
        #endregion

        #region Splice
        [Description("Concatenates two strings.")]
        CAT = 0x7E,
        [Description("Returns a section of a string.")]
        SUBSTR = 0x7F,
        [Description("Keeps only characters left of the specified point in a string.")]
        LEFT = 0x80,
        [Description("Keeps only characters right of the specified point in a string.")]
        RIGHT = 0x81,
        [Description("Returns the length of the input string.")]
        SIZE = 0x82,
        #endregion

        #region Bitwise logic
        [Description("Flips all of the bits in the input.")]
        INVERT = 0x83,
        [Description("Boolean and between each bit in the inputs.")]
        AND = 0x84,
        [Description("Boolean or between each bit in the inputs.")]
        OR = 0x85,
        [Description("Boolean exclusive or between each bit in the inputs.")]
        XOR = 0x86,
        [Description("Returns 1 if the inputs are exactly equal, 0 otherwise.")]
        EQUAL = 0x87,

        //[Description("Same as OP_EQUAL, but runs OP_VERIFY afterward.")]
        //OP_EQUALVERIFY = 0x88, 
        //[Description("Transaction is invalid unless occuring in an unexecuted OP_IF branch")]
        //OP_RESERVED1 = 0x89,
        //[Description("Transaction is invalid unless occuring in an unexecuted OP_IF branch")]
        //OP_RESERVED2 = 0x8A,
        #endregion

        #region Arithmetic
        // Note: Arithmetic inputs are limited to signed 32-bit integers, but may overflow their output.
        [Description("1 is added to the input.")]
        INC = 0x8B,
        [Description("1 is subtracted from the input.")]
        DEC = 0x8C,
        SIGN = 0x8D,
        [Description("The sign of the input is flipped.")]
        NEGATE = 0x8F,
        [Description("The input is made positive.")]
        ABS = 0x90,
        [Description("If the input is 0 or 1, it is flipped. Otherwise the output will be 0.")]
        NOT = 0x91,
        [Description("Returns 0 if the input is 0. 1 otherwise.")]
        NZ = 0x92,
        [Description("a is added to b.")]
        ADD = 0x93,
        [Description("b is subtracted from a.")]
        SUB = 0x94,
        [Description("a is multiplied by b.")]
        MUL = 0x95,
        [Description("a is divided by b.")]
        DIV = 0x96,
        [Description("Returns the remainder after dividing a by b.")]
        MOD = 0x97,
        [Description("Shifts a left b bits, preserving sign.")]
        SHL = 0x98,
        [Description("Shifts a right b bits, preserving sign.")]
        SHR = 0x99,
        [Description("If both a and b are not 0, the output is 1. Otherwise 0.")]
        BOOLAND = 0x9A,
        [Description("If a or b is not 0, the output is 1. Otherwise 0.")]
        BOOLOR = 0x9B,
        [Description("Returns 1 if the numbers are equal, 0 otherwise.")]
        NUMEQUAL = 0x9C,
        [Description("Returns 1 if the numbers are not equal, 0 otherwise.")]
        NUMNOTEQUAL = 0x9E,
        [Description("Returns 1 if a is less than b, 0 otherwise.")]
        LT = 0x9F,
        [Description("Returns 1 if a is greater than b, 0 otherwise.")]
        GT = 0xA0,
        [Description("Returns 1 if a is less than or equal to b, 0 otherwise.")]
        LTE = 0xA1,
        [Description("Returns 1 if a is greater than or equal to b, 0 otherwise.")]
        GTE = 0xA2,
        [Description("Returns the smaller of a and b.")]
        MIN = 0xA3,
        [Description("Returns the larger of a and b.")]
        MAX = 0xA4,
        [Description("Returns 1 if x is within the specified range (left-inclusive), 0 otherwise.")]
        WITHIN = 0xA5,
        #endregion

        #region Crypto
        //[Description("The input is hashed using RIPEMD-160.")]
        //RIPEMD160 = 0xA6,
        [Description("The input is hashed using SHA-1.")]
        SHA1 = 0xA7,
        [Description("The input is hashed using SHA-256.")]
        SHA256 = 0xA8,
        HASH160 = 0xA9,
        HASH256 = 0xAA,
        CHECKSIG = 0xAC,
        CHECKMULTISIG = 0xAE,
        #endregion

        #region Array
        ARRAYSIZE = 0xC0,
        PACK = 0xC1,
        UNPACK = 0xC2,
        PICKITEM = 0xC3,
        SETITEM = 0xC4,
        NEWARRAY = 0xC5,
        NEWSTRUCT = 0xC6,
        #endregion

        #region Exceptions
        THROW = 0xF0,
        THROWIFNOT = 0xF1
        #endregion
    }
}