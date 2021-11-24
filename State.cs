using System;
using System.Drawing;
using System.Windows.Forms;

namespace stateViewer
{
  public class State : FlowLayoutPanel
  {
    private Label StateHandle;
    private Label StateName;
    private Label StateLabel;
    public State(string name)
    {
      InitializeComponent(name);
    }
    public void InitializeComponent(string name)
    {
      this.AutoSize = true;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowOnly;
      this.Dock = DockStyle.Top;

      this.StateHandle = new Label();
      this.StateHandle.Text = "■";
      this.StateHandle.AutoSize = true;
      this.StateHandle.Cursor = Cursors.SizeAll;
      this.Controls.Add(this.StateHandle);

      this.StateName = new Label();
      this.StateName.Text = name;
      this.StateName.AutoSize = true;
      this.Controls.Add(this.StateName);

      this.StateLabel = new Label();
      this.StateLabel.Text = "";
      this.StateLabel.AutoSize = true;
      this.Controls.Add(this.StateLabel);
    }
    public void ChangeText(string text)
    {
      this.StateLabel.Text = text;
    }
    public void SetState(Color color, string text)
    {
      this.StateHandle.ForeColor = color;
      this.StateName.ForeColor = color;
      this.StateLabel.ForeColor = color;
      this.StateLabel.Text = text;
    }
  }
}