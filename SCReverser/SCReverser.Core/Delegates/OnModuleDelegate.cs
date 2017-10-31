using SCReverser.Core.Types;

namespace SCReverser.Core.Delegates
{
    /// <summary>
    /// Delegate for on module event
    /// </summary>
    /// <param name="sender">Debugger</param>
    /// <param name="module">Module</param>
    public delegate void OnModuleDelegate(object sender, Module module);
}
