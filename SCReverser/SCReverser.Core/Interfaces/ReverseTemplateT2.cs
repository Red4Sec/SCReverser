using System;
using SCReverser.Core.Types;
using System.Collections.Generic;
using System.Drawing;
using SCReverser.Core.Enums;

namespace SCReverser.Core.Interfaces
{
    public class ReverseTemplate<ReverserT, CfgType> : IReverseTemplate
        where ReverserT : IReverser
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
        public Type DebuggerType => null;
        /// <summary>
        /// Have debugger
        /// </summary>
        public bool HaveDebugger { get { return false; } }
        /// <summary>
        /// Config Type
        /// </summary>
        public Type ConfigType { get { return typeof(CfgType); } }
        /// <summary>
        /// Flags
        /// </summary>
        public virtual ETemplateFlags Flags { get { return ETemplateFlags.None; } }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ReverseTemplate() { }

        /// <summary>
        /// Create reverser
        /// </summary>
        public ReverserT CreateReverser()
        {
            return Activator.CreateInstance<ReverserT>();
        }

        IReverser IReverseTemplate.CreateReverser()
        {
            return Activator.CreateInstance<ReverserT>();
        }

        public IDebugger CreateDebugger(IEnumerable<Instruction> instructions, object debugConfig)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get new config Type
        /// </summary>
        public object CreateNewConfig()
        {
            return Activator.CreateInstance<CfgType>();
        }
        /// <summary>
        /// Get logo
        /// </summary>
        public virtual Image GetLogo() { return null; }
    }
}