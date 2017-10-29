using System.IO;

namespace SCReverser.Core.Interfaces
{
    public interface IInitClassStream
    {
        Stream GetStream(out bool leaveOpen);
    }
}