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
        }

        private void clickButtonLineDraw(object sender, EventArgs e)
        {
            this.Cursor = embellisher.SwitchCursor(this.Cursor, Cursors.Cross);
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e);
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            designer.DrawPoly(e);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            // my own implementation of Double Buffering
            using (Bitmap bitmap = new Bitmap(printer.Width, printer.Height))
            {
                using (Graphics gx = Graphics.FromImage(bitmap))
                {
                    // prepare a new canvas
                    gx.Clear(Color.White);

                    // swap canvas in printer
                    printer.SetGraphics(gx);

                    // reprint all objects
                    designer.Canvas.Reprint();
                    
                    // swap canvas on the screen
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    printer.UpdateGraphics();
                }
            }
        }
    }
}
