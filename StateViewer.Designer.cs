using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace stateViewer
{
  partial class StateViewer
  {
    private const int edgeWidth = 16;
    private const int headerWidth = 32;
    private System.ComponentModel.IContainer components = null;
    private Dictionary<string, State> StateMap;
    private FlowLayoutPanel Flp;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.StateMap = new Dictionary<string, State>();
      this.components = new Container();
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Text = "state viewer";
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

      this.Flp = new FlowLayoutPanel();
      this.Padding = new Padding(edgeWidth, headerWidth, edgeWidth, edgeWidth);
      this.Flp.AutoSize = true;
      this.Flp.AutoSizeMode = AutoSizeMode.GrowOnly;
      this.Flp.WrapContents = false;

      this.Flp.FlowDirection = FlowDirection.TopDown;
      this.Flp.Dock = DockStyle.Top;
      this.Controls.Add(this.Flp);

      NamedPipe.Listen("stateViewer", this.SetState);
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

