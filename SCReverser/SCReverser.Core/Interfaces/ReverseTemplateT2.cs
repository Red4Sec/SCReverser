using SCReverser.Core.Types;
using System;

namespace SCReverser.Core.Interfaces
{
    public class ReverseTemplate<ReverserT, DebuggerT> : IReverseTemplate
        where ReverserT : IReverser
        where DebuggerT : IDebugger
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
        /// <summary>
        /// Create debugger
        /// </summary>
        /// <param name="instructions">Instructions</param>
        public DebuggerT CreateDebugger(Instruction[] instructions)
        {
            return (DebuggerT)Activator.CreateInstance(typeof(DebuggerT), new object[] { instructions });
        }

        IReverser IReverseTemplate.CreateReverser()
        {
            return Activator.CreateInstance<ReverserT>();
        }
        IDebugger IReverseTemplate.CreateDebugger(params Instruction[] instructions)
        {
            return (DebuggerT)Activator.CreateInstance(typeof(DebuggerT), new object[] { instructions });
        }
    }
}