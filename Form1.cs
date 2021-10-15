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

            if (this._DesignMode == DesignModes.Poly)
                this.Cursor = embellisher.CircleDrawCursor;
            else
                this.Cursor = embellisher.NormalCursor;
        }

        private void MouseClickButtonCircleDraw(object sender, MouseEventArgs e)
        {
            this._DesignMode = this._DesignMode == DesignModes.Circle ? DesignModes.Off : DesignModes.Circle;

            if (this._DesignMode == DesignModes.Circle)
                this.Cursor = embellisher.CircleDrawCursor;
            else
                this.Cursor = embellisher.NormalCursor;
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e, this._DesignMode);
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
            {
                designer.DrawPoly(e, ref this._DesignMode);
            }
            else if (this._DesignMode == DesignModes.Circle)
            {
                designer.DrawCircle(e.Location, ref this._DesignMode, designer.State == PrintingStates.FollowMouse);

            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);
        }
    }
}
