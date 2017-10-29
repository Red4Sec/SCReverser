using SCReverser.Core.Enums;
using SCReverser.Core.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SCReverser.Core.Interfaces
{
    public class ReverseTemplate<ReverserT, DebuggerT, CfgType> : IReverseTemplate
        where ReverserT : IReverser
        where DebuggerT : IDebugger
        where CfgType : class, new()
    {
        /// <summary>
        /// Template
        /// </summary>
        public virtual string Template => throw new NotImplementedException();

        /// <summary>
        /// Reverser
        /// </summary>
        public Type ReverserType => typeof(ReverserT);
        /// <summary>
        /// Debugger
        /// </summary>
        public Type DebuggerType => typeof(DebuggerT);
        /// <summary>
        /// Have debugger
        /// </summary>
        public bool HaveDebugger { get { return true; } }
        /// <summary>
        /// Config Type
        /// </summary>
        public Type ConfigType { get { return typeof(CfgType); } }
        /// <summary>
        /// Flags
        /// </summary>
        public virtual TemplateFlags Flags { get { return TemplateFlags.None; } }
        /// <summary>
        /// Constructor
        /// </summary>
        protected ReverseTemplate() { }

        /// <summary>
        /// Create reverser
        /// </summary>
        public virtual ReverserT CreateReverser()
        {
            return Activator.CreateInstance<ReverserT>();
        }
        /// <summary>
        /// Create debugger
        /// </summary>
        /// <param name="instructions">Instructions</param>
        /// <param name="debugConfig">Debugger config</param>
        public virtual DebuggerT CreateDebugger(IEnumerable<Instruction> instructions, object debugConfig)
        {
            return (DebuggerT)Activator.CreateInstance(typeof(DebuggerT), new object[] { instructions, debugConfig });
        }
        IReverser IReverseTemplate.CreateReverser()
        {
            ReverserT t = CreateReverser();
            return t;
        }
        IDebugger IReverseTemplate.CreateDebugger(IEnumerable<Instruction> instructions, object debugConfig)
        {
            DebuggerT t = CreateDebugger(instructions, debugConfig);
            return t;
        }
        /// <summary>
        /// Get new config type
        /// </summary>
        public virtual object CreateNewConfig()
        {
            return Activator.CreateInstance<CfgType>();
        }
        /// <summary>
        /// Get logo
        /// </summary>
        public virtual Image GetLogo() { return null; }
    }
}