using System;
using SCReverser.Core.Types;

namespace SCReverser.Core.Interfaces
{
    public class ReverseTemplate<ReverserT> : IReverseTemplate
        where ReverserT :IReverser
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

        public IDebugger CreateDebugger(params Instruction[] instructions)
        {
            throw new NotImplementedException();
        }
    }
}