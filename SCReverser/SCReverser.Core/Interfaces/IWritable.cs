using System.IO;

namespace SCReverser.Core.Interfaces
{
    public interface IWritable
    {
        /// <summary>
        /// Write
        /// </summary>
        /// <param name="stream">Stream</param>
        uint Write(Stream stream);
    }
}