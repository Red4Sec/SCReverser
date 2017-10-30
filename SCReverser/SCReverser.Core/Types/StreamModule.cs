using System;
using System.Drawing;
using System.IO;

namespace SCReverser.Core.Types
{
    public class StreamModule : IDisposable
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Stream
        /// </summary>
        public Stream Stream { get; private set; }
        /// <summary>
        /// Leave stream open
        /// </summary>
        public bool LeaveStreamOpen { get; private set; }
        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="stream">Stream</param>
        /// <param name="leaveOpen">LeaveOpen</param>
        public StreamModule(string name,Stream stream,bool leaveOpen)
        {
            LeaveStreamOpen = leaveOpen;
            Stream = stream;
            Name = name;
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            if (LeaveStreamOpen) return;
            if (Stream!=null)
            {
                Stream.Close();
                Stream.Dispose();
            }
        }
    }
}