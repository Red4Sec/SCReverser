using SCReverser.Core.OpCodeArguments;
using System;

namespace SCReverser.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class OpCodeArgumentAttribute : Attribute
    {
        /// <summary>
        /// OpCode
        /// </summary>
        public string OpCode { get; set; }
        /// <summary>
        /// Argument Type
        /// </summary>
        public Type ArgumentType { get; private set; }
        /// <summary>
        /// Constructor arguments
        /// </summary>
        public object[] ConstructorArguments { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Is sys call
        /// </summary>
        public bool IsSysCall { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="argumentType">Argument Type</param>
        public OpCodeArgumentAttribute(Type argumentType)
        {
            if (!typeof(OpCodeEmptyArgument).IsAssignableFrom(argumentType))
                throw (new ArgumentException("argumentType"));

            ArgumentType = argumentType;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeArgumentAttribute()
        {
            // Empty argument
            ArgumentType = typeof(OpCodeEmptyArgument);
        }
        /// <summary>
        /// Create OpCodeArgument
        /// </summary>
        public OpCodeEmptyArgument Create()
        {
            return (OpCodeEmptyArgument)Activator.CreateInstance(ArgumentType, ConstructorArguments);
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description)) return OpCode;
            return OpCode + " - " + Description.ToString();
        }
    }
}