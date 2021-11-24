using System;
using System.Drawing;
using System.Windows.Forms;

namespace stateViewer
{
  public partial class StateViewer : Form
  {
    public StateViewer()
    {
      InitializeComponent();
    }
    private void SetState(string message)
    {
      try
      {
        Console.WriteLine(message);
        if (!message.Contains(","))
        {
          return;
        }
        var messages = message.Split(",");
        var key = messages[0];
        Console.WriteLine(key);
        var color = ColorTranslator.FromHtml(messages[1]);
        Console.WriteLine(color.ToString());
        var value = messages[2];
        if (!this.StateMap.ContainsKey(key))
        {
          this.AddState(key);
        }
        this.StateMap[key].SetState(color, value);
      }
      catch { }
    }
    private void AddState(string key)
    {
      var state = new State(key);
      this.StateMap.Add(key, state);
      this.Flp.Controls.Add(state);
    }
  }
}
