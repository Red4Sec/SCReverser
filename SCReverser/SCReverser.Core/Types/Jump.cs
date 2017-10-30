using Newtonsoft.Json;
using SCReverser.Core.Delegates;
using SCReverser.Core.Helpers;
using SCReverser.Core.Interfaces;
using System.Drawing.Drawing2D;

namespace SCReverser.Core.Types
{
    public class Jump
    {
        OnJumpDelegate _Checker;

        /// <summary>
        /// Offset
        /// </summary>
        public IndexOffset To { get; private set; }

        /// <summary>
        /// Checker
        /// </summary>
        [JsonIgnore]
        public OnJumpDelegate Checker
        {
            get { return _Checker; }
        }
        /// <summary>
        /// Is Dynamic
        /// </summary>
        [JsonIgnore]
        public bool IsDynamic
        {
            get { return _Checker != null; }
        }
        /// <summary>
        /// Style
        /// </summary>
        public DashStyle Style { get; set; } = DashStyle.Solid;

        /// <summary>
        /// Check
        /// </summary>
        /// <param name="debug">Debugger</param>
        public bool Check(IDebugger debug, Instruction ins)
        {
            if (_Checker == null)
            {
                To = null;
                return false;
            }

            uint? offset = _Checker.Invoke(debug, ins);
            if (offset.HasValue)
            {
                if (debug.OffsetToIndex(offset.Value, out uint ix))
                {
                    To = new IndexOffset() { Index = ix, Offset = offset.Value };
                    return true;
                }
            }

            To = null;
            return false;
        }

        /// <summary>
        /// Jump
        /// </summary>
        public Jump(uint offset, uint index)
        {
            To = new IndexOffset() { Offset = offset, Index = index };
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public Jump(OnJumpDelegate checker)
        {
            _Checker = checker;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return JsonHelper.Serialize(this);
        }
    }
}