using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace stateViewer
{
  static class Program
  {
    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(int dwProcessId);

    [STAThread]
    static void Main(string[] args)
    {
      AttachConsole(-1);

      if (args[0] == "main")
      {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new StateViewer());
      }
      else if (args[0] == "send")
      {
        NamedPipe.Send("stateViewer", args[1]);
      }
      else if (args[0] == "set-by-url")
      {
        var message = args[1].Replace("stateviewer://", "").Replace("/", ",");
        NamedPipe.Send("stateViewer", message);
      }
    }
  }
}
