using SCReverser.Core.Types;
using System;

namespace SCReverser.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class OpCodeArgumentAttribute : Attribute
    {
        /// <summary>
        /// Argument Type
        /// </summary>
        public Type ArgumentType { get; private set; }
        /// <summary>
        /// Constructor arguments
        /// </summary>
        public object[] ConstructorArguments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="argumentType">Argument Type</param>
        public OpCodeArgumentAttribute(Type argumentType)
        {
            if (!typeof(OpCodeArgument).IsAssignableFrom(argumentType))
                throw (new ArgumentException("argumentType"));

            ArgumentType = argumentType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpCodeArgumentAttribute()
        {
            // Empty argument
            ArgumentType = typeof(OpCodeArgument);
        }

        /// <summary>
        /// Create OpCodeArgument
        /// </summary>
        public OpCodeArgument Create()
        {
            return (OpCodeArgument)Activator.CreateInstance(ArgumentType, ConstructorArguments);
        }
    }
}