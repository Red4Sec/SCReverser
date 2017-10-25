using System;
using System.Text;
using System.Windows.Forms;

namespace SCReverser
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string repeat =
@"
[OpCodeArgument(typeof(OpCodeByteArrayArgument), ConstructorArguments = new object[] { 0x01 })]
[Description(""0x01 The next opcode bytes is data to be pushed onto the stack."")]
PUSHBYTES#X# = 0x01,";

            StringBuilder sb = new StringBuilder();
            for (int x = 1; x <= 75; x++)
            {
                sb.Append(repeat.Replace("0x01", "0x" + x.ToString("x2").ToUpperInvariant()).Replace("#X#", x.ToString()));
            }

            Clipboard.SetText(sb.ToString());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}