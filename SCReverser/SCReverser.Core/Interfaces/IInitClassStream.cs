using SCReverser.Core.Types;
using System.Collections.Generic;

namespace SCReverser.Core.Interfaces
{
    public interface IInitClassStream
    {
        IEnumerable<StreamModule> GetStream();
    }
}