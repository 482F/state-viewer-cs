using System.IO.Pipes;
using System;
using System.IO;
using System.Text;
using System.Security.Principal;
using System.Threading.Tasks;

namespace stateViewer
{
  static class NamedPipe
  {
    async static public Task Listen(string pipeName, Action<string> action)
    {
      while (true)
      {
        using var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In, 1);
        await pipeServer.WaitForConnectionAsync();

        using var ms = new MemoryStream();
        pipeServer.CopyTo(ms);
        pipeServer.Disconnect();
        pipeServer.Close();
        action(Encoding.UTF8.GetString(ms.ToArray()));
      }
    }
    async static public Task Send(string pipeName, string message)
    {
      using var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.None, TokenImpersonationLevel.Impersonation);
      Console.WriteLine("connecting: " + message);
      await pipeClient.ConnectAsync(60000);
      Console.WriteLine("connected: " + message);
      var bs = Encoding.UTF8.GetBytes(message);
      pipeClient.Write(bs);
      pipeClient.Flush();
      pipeClient.Dispose();
    }
  }
}
