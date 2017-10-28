using System;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Drawing;
using SCReverser.Core.Enums;

namespace SCReverser.Core.Interfaces
{
    public interface IReverseTemplate
    {
        /// <summary>
        /// Have debugger
        /// </summary>
        bool HaveDebugger { get; }
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
        /// <summary>
        /// Config Type
        /// </summary>
        Type ConfigType { get; }
        /// <summary>
        /// Flags
        /// </summary>
        ETemplateFlags Flags { get; }

        /// <summary>
        /// Create reverser
        /// </summary>
        IReverser CreateReverser();
        /// <summary>
        /// Create debugger
        /// </summary>
        /// <param name="instructions">Instructions</param>
        /// <param name="debugConfig">Config</param>
        IDebugger CreateDebugger(IEnumerable<Instruction> instructions, object debugConfig);
        /// <summary>
        /// Get Logo
        /// </summary>
        Image GetLogo();

        /// <summary>
        /// Get new config type
        /// </summary>
        object CreateNewConfig();
    }
}