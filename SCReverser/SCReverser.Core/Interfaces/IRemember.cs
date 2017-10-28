using System.Windows.Forms;

namespace SCReverser.Core.Interfaces
{
    public interface IRemember
    {
        void SaveValues(Form f);
        void GetValues(Form f);
    }
}