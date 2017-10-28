using SCReverser.Core.Helpers;

namespace SCReverser.Core.Types
{
    public class StackItem
    {
        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Json Value
        /// </summary>
        public virtual string PrintableValue
        {
            get
            {
                if (Value == null) return "";
                return JsonHelper.Serialize(Value);
            }
        }
        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="obj">Object</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is StackItem i)) return false;
            return i.Value == Value;
        }
        /// <summary>
        /// Dummy hashcode
        /// </summary>
        public override int GetHashCode()
        {
            if (Value == null) return 0;
            return 1;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return PrintableValue;
        }
    }
}