using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public static partial class designer
    {
        public static line DrawLine(Point start, Point end, Brush brush)
        {
            if (end.X < start.X)
                Functors.Swap(ref start, ref end);

            int _dx = end.X - start.X;
            int _dy = end.Y - start.Y;

            int dx = Math.Abs(_dx);
            int dy = Math.Abs(_dy);

            Point p = new Point(start.X, start.Y);

            if (_dy > _dx && _dy > 0)
            {
                #region First Octant
                int incr1 = 2 * dx;
                int incr2 = 2 * dx - 2 * dy;
                int d = 2 * dy - dx;

                while (p.Y < end.Y)
                {
                    if (d <= 0)
                    {
                        p.Y++;
                        d += incr1;
                    }
                    else
                    {
                        p.X++; p.Y++;
                        d += incr2;
                    }

                    printer.PutPixel(p, brush);
                }
                #endregion
            }
            else if (_dy <= _dx && _dy > 0)
            {
                #region Second Octant
                int incr1 = 2 * dy;
                int incr2 = 2 * dy - 2 * dx;
                int d = 2 * dx - dy;

                while (p.X < end.X)
                {
                    if (d <= 0)
                    {
                        p.X++;
                        d += incr1;
                    }
                    else
                    {
                        p.X++; p.Y++;
                        d += incr2;
                    }

                    printer.PutPixel(p, brush);
                }
                #endregion
            }
            else if (_dy > -_dx)
            {
                #region Third Octant
                int incr1 = 2 * dy;
                int incr2 = 2 * dy - 2 * dx;
                int d = 2 * dy - dx;

                while (p.X < end.X)
                {
                    if (d <= 0)
                    {
                        p.X++;
                        d += incr1;
                    }
                    else
                    {
                        p.X++; p.Y--;
                        d += incr2;
                    }

                    printer.PutPixel(p, brush);
                }
                #endregion
            }
            else if (_dy <= -_dx)
            {
                #region Fourth Octant
                int incr1 = 2 * dx;
                int incr2 = 2 * dx - 2 * dy;
                int d = 2 * dy - dx;

                while (p.Y > end.Y)
                {
                    if (d <= 0)
                    {
                        p.Y--;
                        d += incr1;
                    }
                    else
                    {
                        p.X++; p.Y--;
                        d += incr2;
                    }

                    printer.PutPixel(p, brush);
                }
                #endregion
            }

            line ret = new line(start, end, printer.Buffer, brush);
            printer.FlushBuffer();

            return ret;
        }
    }
}
