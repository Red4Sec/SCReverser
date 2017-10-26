using System;

namespace SCReverser.Core.Interfaces
{
    public interface IReverseTemplate
    {
        /// <summary>
        /// Have debugger
        /// </summary>
        bool HaveDebugger{get;}
        /// <summary>
        /// Template
        /// </summary>
        string Template { get; }
        /// <summary>
        /// Reverse type
        /// </summary>
        Type ReverserType { get; }
        /// <summary>
        /// Debugger type
        /// </summary>
        Type DebuggerType { get; }
    }
}
