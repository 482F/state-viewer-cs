using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using Utils482F;

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

      if (args[0] == "set")
      {
        NamedPipe.Send("stateViewer", args[1]).Wait();
      }
      else if (args[0] == "by-url")
      {
        var gArgs = HttpUtility.UrlDecode(args[1].Replace("stateviewer://", "")).Split('/');
        Main(gArgs);
      }
      else
      {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new StateViewer());
      }
    }
  }
}
