using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab01
{
    public enum DesignModes { Off, Poly, Circle };

    public partial class mainForm : Form
    {
        private DesignModes _DesignMode = DesignModes.Off;
        public mainForm()
        {
            InitializeComponent();
            printer.Initialize(canvas);
            designer.Initialize();
            printer.SetGraphics(designer.Canvas.PreviewGraphics);
        }

        private void clickButtonLineDraw(object sender, EventArgs e)
        {
            this._DesignMode = this._DesignMode == DesignModes.Poly ? DesignModes.Off : DesignModes.Poly;
            this.Cursor = embellisher.SwitchCursor(this.Cursor, Cursors.Cross);
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e);
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
                designer.DrawPoly(e);
            else if (this._DesignMode == DesignModes.Circle)
                designer.DrawCircle(e.Location);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);
        }
    }
}
