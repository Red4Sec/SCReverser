using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

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
        TemplateFlags Flags { get; }

        /// <summary>
        /// Create reverser
        /// </summary>
        IReverser CreateReverser();
        /// <summary>
        /// Create debugger
        /// </summary>
        /// <param name="result">Reverse result</param>
        /// <param name="debugConfig">Config</param>
        IDebugger CreateDebugger(ReverseResult result, object debugConfig);
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