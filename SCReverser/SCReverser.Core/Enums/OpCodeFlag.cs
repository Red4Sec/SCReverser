namespace SCReverser.Core.Enums
{
    public enum OpCodeFlag : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Is call
        /// </summary>
        IsCall = 1,
        /// <summary>
        /// Is ret
        /// </summary>
        IsRet = 2,
        /// <summary>
        /// Is jump
        /// </summary>
        IsJump = 4,
    }
}