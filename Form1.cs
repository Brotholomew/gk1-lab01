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
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            printer.Initialize(canvas);
            designer.Initialize();

            for (int i = printer.Width / 4; i <= printer.Width / 4 * 3; i++)
                printer.PutPixel(printer.Width / 2, i, Color.Black);
        }

        private void clickButtonLineDraw(object sender, EventArgs e)
        {
            this.Cursor = embellisher.SwitchCursor(this.Cursor, Cursors.Cross);
            //designer.DrawCircle(new Point(printer.Width / 2, printer.Height / 2), 50);

            //designer.DrawLine(new Point(100, 100), new Point(140, 140)); // First quadrant
            //designer.DrawLine(new Point(100, 100), new Point(100, 140)); // Y-Axis
            //designer.DrawLine(new Point(100, 100), new Point(140, 100)); // X-Axis

            //designer.DrawLine(new Point(100, 100), new Point(140, 60)); // Second quadrant
            //designer.DrawLine(new Point(100, 100), new Point(100, 60)); // Y-Axis

            //designer.DrawLine(new Point(100, 100), new Point(60, 60)); // Third quadrant
            //designer.DrawLine(new Point(100, 100), new Point(60, 100)); // X-Axis

            //designer.DrawLine(new Point(100, 100), new Point(60, 140)); // Fourth quadrant

            /*for (int i = printer.Width / 4; i <= printer.Width / 4 * 3; i++)
                printer.PutPixel(i, printer.Height / 2, Color.Black);

            for (int i = printer.Height / 4; i <= printer.Height / 4 * 3; i++)
                printer.PutPixel(printer.Width / 2, i, Color.Black);

            for (int i = printer.Width / 8 * 3; i <= printer.Width / 8 * 5; i++)
                printer.PutPixel(i, 319, Color.Black);*/
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e);
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            designer.DrawPoly(e);
        }
    }
}
