using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public static partial class designer
    {
        public static vertex FirstVertex = null;

        public static void DrawCircle(Point p, mainForm f, bool finish = false)
        {
            if (designer._State == PrintingStates.Off) 
            {
                designer.FirstVertex = designer.DrawVertex(p, Brushes.Black);
                designer._Canvas.RegisterVertex(designer.FirstVertex);

                designer._State = PrintingStates.FollowMouse;
            } 
            else
            {
                int radius = (int)designer.Distance(p, designer.FirstVertex.Pixels[0]);
                designer._Canvas.ErasePreview();

                if (!finish)
                {
                    // FollowMouse
                    designer._DrawCircle(designer.FirstVertex.Pixels[0], radius, Brushes.Black);
                }
                else
                {
                    circle c = null;
                    designer._Canvas.PrintToMain(() => c = designer._DrawCircle(designer.FirstVertex.Pixels[0], radius, Brushes.Black));
                    designer._Canvas.RegisterDrawable(c);

                    designer.FirstVertex = null;
                    designer._State = PrintingStates.Off;
                    f.DM = DesignModes.Off;
                }
            }

            printer.Erase();
        }

        private static circle _DrawCircle(Point center, int radius, Brush b)
        {
            int d = 1 - radius;
            int dE = 3;
            int dSE = 5 - 2 * radius;
            int dx, dy;

            Point p = new Point(center.X, center.Y + radius);

            printer.PutPixel(p, b);
            while (p.Y - center.Y > p.X - center.X)
            {
                if (d < 0)
                {
                    d += dE;
                    dE += 2;
                    dSE += 2;
                }
                else
                {
                    d += dSE;
                    dE += 2;
                    dSE += 4;
                    p.Y--;
                }
                p.X++;

                dx = Math.Abs(center.X - p.X);
                dy = Math.Abs(center.Y - p.Y);

                printer.PutPixel(center.X + dx, center.Y + dy, b);
                printer.PutPixel(center.X - dx, center.Y + dy, b);
                printer.PutPixel(center.X + dx, center.Y - dy, b);
                printer.PutPixel(center.X - dx, center.Y - dy, b);
                                                               
                // zamieniamy osie                             
                printer.PutPixel(center.X + dy, center.Y + dx, b);
                printer.PutPixel(center.X - dy, center.Y + dx, b);
                printer.PutPixel(center.X + dy, center.Y - dx, b);
                printer.PutPixel(center.X - dy, center.Y - dx, b);
            }

            circle ret = new circle(center, radius, printer.Buffer, b);
            printer.FlushBuffer();

            return ret;
        }
    }
}
