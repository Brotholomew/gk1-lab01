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

            //Bitmap bottom = new Bitmap(printer.Width, printer.Height);
            //Bitmap top = new Bitmap(printer.Width, printer.Height);

            //Graphics gx1 = Graphics.FromImage(bottom);
            //Graphics gx2 = Graphics.FromImage(top);

            //gx1.Clear(Color.White);
            //gx2.Clear(Color.Transparent);

            //gx1.DrawLine(Pens.Black, new Point(100, 100), new Point(100, 200));
            //gx2.DrawLine(Pens.Black, new Point(50, 150), new Point(150, 150));

            //Graphics g = canvas.CreateGraphics();
            //g.DrawImage(bottom, 0, 0);
            //g.DrawImage(top, 0, 0);
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e);
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
                designer.DrawPoly(e);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);

            //// my own implementation of Double Buffering
            //using (Bitmap bitmap = new Bitmap(printer.Width, printer.Height))
            //{
            //    using (Graphics gx = Graphics.FromImage(bitmap))
            //    {
            //        // prepare a new canvas
            //        gx.Clear(Color.White);

            //        // swap canvas in printer
            //        printer.SetGraphics(gx);

            //        // reprint all objects
            //        designer.Canvas.Reprint();
                    
            //        // swap canvas on the screen
            //        e.Graphics.DrawImage(bitmap, 0, 0);
            //        printer.UpdateGraphics();
            //    }
            //}
        }
    }
}
