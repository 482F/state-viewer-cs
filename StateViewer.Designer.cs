using System;
using System.Configuration;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Utils482F;

namespace stateViewer
{
  partial class StateViewer
  {
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const UInt32 SWP_NOSIZE = 0x0001;
    private const UInt32 SWP_NOMOVE = 0x0002;
    private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int edgeWidth = 16;
    private const int headerWidth = 32;
    private const int buttonSize = 24;
    private Configuration config;
    private System.ComponentModel.IContainer components = null;
    private Dictionary<string, State> StateMap;
    private FlowLayoutPanel Flp;
    private Button MinimizeButton;
    private Button CloseButton;

    protected override void Dispose(bool disposing)
    {
      setSetting("x", this.Location.X);
      setSetting("y", this.Location.Y);
      setSetting("width", this.Size.Width);
      setSetting("height", this.Size.Height);
      this.config.Save();
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private int getSetting(string key)
    {
      return Int32.Parse(this.config.AppSettings.Settings[key].Value);
    }
    private void setSetting(string key, int value)
    {
      this.config.AppSettings.Settings[key].Value = value.ToString();
    }

    private void InitializeComponent()
    {
      this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

      this.StateMap = new Dictionary<string, State>();
      this.components = new Container();
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Text = "state viewer";
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MinimumSize = new Size(buttonSize * 3, buttonSize * 2);
      this.Padding = new Padding(edgeWidth, headerWidth, edgeWidth, edgeWidth);
      this.BackColor = Color.LightGray;

      this.CloseButton = new Button();
      this.CloseButton.Text = "x";
      this.CloseButton.Size = new Size(buttonSize, buttonSize);
      this.CloseButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
      this.CloseButton.Location = new Point(this.ClientSize.Width - buttonSize, 0);
      this.CloseButton.Click += this.Close;
      this.Controls.Add(this.CloseButton);

      this.MinimizeButton = new Button();
      this.MinimizeButton.Text = "_";
      this.MinimizeButton.Size = new Size(buttonSize, buttonSize);
      this.MinimizeButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
      this.MinimizeButton.Location = new Point(this.ClientSize.Width - buttonSize * 2, 0);
      this.MinimizeButton.Click += this.Minimize;
      this.Controls.Add(this.MinimizeButton);

      this.Flp = new FlowLayoutPanel();
      this.Flp.AutoSize = true;
      this.Flp.AutoSizeMode = AutoSizeMode.GrowOnly;
      this.Flp.WrapContents = false;
      this.Flp.BackColor = Color.White;
      this.Flp.FlowDirection = FlowDirection.TopDown;
      this.Flp.Dock = DockStyle.Fill;
      this.Controls.Add(this.Flp);

      this.Load += this.StateViewer_Load;
      this.Shown += this.StateViewer_Shown;

      var _ = NamedPipe.Listen("stateViewer", this.SetState);
    }
    private void StateViewer_Load(object sender, EventArgs e)
    {
      this.Location = new Point(getSetting("x"), getSetting("y"));
      this.Size = new Size(getSetting("width"), getSetting("height"));
    }
    private void StateViewer_Shown(object sender, EventArgs e)
    {
      SetWindowPos(this.Handle, HWND_TOPMOST, getSetting("x"), getSetting("y"), getSetting("width"), getSetting("height"), TOPMOST_FLAGS);
    }

    private void Close(object sender, EventArgs e)
    {
      this.Dispose();
    }

    private void Minimize(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
    }

    private int IsEdge(Point p)
    {
      var x = p.X;
      var y = p.Y;
      var top = y <= edgeWidth;
      var left = x <= edgeWidth;
      var bottom = this.ClientSize.Height - y <= edgeWidth;
      var right = this.ClientSize.Width - x <= edgeWidth;
      if (top && left)
      {
        return 13; // HTTOPLEFT
      }
      else if (top && right)
      {
        return 14; // HTTOPRIGHT
      }
      else if (bottom && left)
      {
        return 16; // HTBOTTOMLEFT
      }
      else if (bottom && right)
      {
        return 17; // HTBOTTOMRIGHT
      }
      else if (left)
      {
        return 10; // HTLEFT
      }
      else if (bottom)
      {
        return 15; // HTBOTTOM
      }
      else if (right)
      {
        return 11; // HTRIGHT
      }
      return 2;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 0x84)
      {
        Point pos = new Point(m.LParam.ToInt32());
        pos = this.PointToClient(pos);
        m.Result = (IntPtr)this.IsEdge(pos);
        return;
      }
      base.WndProc(ref m);
    }

  }
}

