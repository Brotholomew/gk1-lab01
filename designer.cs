using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace lab01
{
    public static class designer
    {
        private static PrintingStates State = PrintingStates.Off;

        private static Point NullPoint = new Point(-1, -1);

        private static Point LastPoint = designer.NullPoint;
        private static Point FirstPoint = designer.NullPoint;

        private static line LastLine = null;
        private static List<line> _Lines = new List<line>();
        private static List<vertex> _Vertices = new List<vertex>();

        private static CanvasPixel[,] Canvas;

        public static void Initialize()
        {
            designer.Canvas = new CanvasPixel[printer.Width, printer.Height];
        }

        public static void FollowMouse(MouseEventArgs e)
        {
            if (designer.State == PrintingStates.FollowMouse)
            {
                if (designer.LastLine != null) designer.EraseDrawable(LastLine);
                
                line l = designer.DrawLine(LastPoint, e.Location, Color.Black);
                designer.LastLine = l;
            }
        }

        public static void InitializePixel(Point pixel)
        {
            if (!designer.Canvas[pixel.X, pixel.Y].Set)
            {
                designer.Canvas[pixel.X, pixel.Y].Set = true;
                designer.Canvas[pixel.X, pixel.Y].Drawables = new List<drawable>();
            }
        }

        public static void RegisterVertex(vertex v)
        {
            foreach(var pixel in v.Pixels)
            {
                designer.InitializePixel(pixel);
                if (designer.Canvas[pixel.X, pixel.Y].V == null)
                {
                    designer.Canvas[pixel.X, pixel.Y].V = v;
                }
                else
                {
                    throw new Exception("Cannot set two vertices in the same place!");
                } 
            }
        }

        public static void RegisterDrawable(drawable d)
        {
            foreach (var pixel in d.Pixels)
            {
                designer.InitializePixel(pixel);
                designer.Canvas[pixel.X, pixel.Y].Drawables.Add(d);
            }
        }

        public static void EraseDrawable(drawable d)
        {
            Color c;
            
            foreach (var pixel in d.Pixels)
            {
                c = Color.White;

                if (designer.Canvas[pixel.X, pixel.Y].Set)
                {
                    int idx = designer.Canvas[pixel.X, pixel.Y].Drawables.IndexOf(d);
                    designer.Canvas[pixel.X, pixel.Y].Drawables.RemoveAt(idx);
                    
                    if (designer.Canvas[pixel.X, pixel.Y].Drawables.Count > 0)
                    {
                        c = designer.Canvas[pixel.X, pixel.Y].Drawables[designer.Canvas[pixel.X, pixel.Y].Drawables.Count - 1].Color;
                    }
                }

                if (designer.Canvas[pixel.X, pixel.Y].V != null)
                {
                    c = designer.Canvas[pixel.X, pixel.Y].V.Color;
                }

                printer.PutPixel(pixel.X, pixel.Y, c);
            }
            
        }

        public static void DrawPoly(MouseEventArgs e)
        {
            designer.LastLine = null;

            // rysowanie linii, łamanych oraz wielokątów
            if (designer.State == PrintingStates.Off)
            {
                // pierwszy punkt
                vertex v = designer.DrawVertex(e.Location, Color.Red);
                designer.RegisterVertex(v);
                designer._Vertices.Add(v);

                designer.LastPoint = new Point(e.Location.X, e.Location.Y);
                designer.FirstPoint = new Point(e.Location.X, e.Location.Y);

                designer.State = PrintingStates.FollowMouse;
            }
            else
            {
                designer.LastPoint = e.Location;

                // ostatni punkt
                if (designer.Canvas[e.Location.X, e.Location.Y].V != null &&
                    designer._Vertices.Count > 0 &&
                    designer.Canvas[e.Location.X, e.Location.Y].V == designer._Vertices[0])
                {
                    designer.EraseDrawable(designer.LastLine);
                    designer.State = PrintingStates.Off;

                    line l = designer.DrawLine(designer.LastPoint, designer.Canvas[e.Location.X, e.Location.Y].V.Center, Color.Black);
                    designer._Lines.Add(l);
                    poly p = new poly(designer._Lines, designer._Vertices);
                    
                    designer._Lines.Clear();
                    designer._Vertices.Clear();
                    designer.LastPoint = designer.NullPoint;
                    designer.FirstPoint = designer.NullPoint;

                    designer.RegisterDrawable(p);
                } 
                else
                {
                    // kolejny punkt
                    vertex v = designer.DrawVertex(e.Location, Color.Blue);
                    designer.RegisterVertex(v);
                    designer._Vertices.Add(v);

                    line l = designer.DrawLine(designer.LastPoint, e.Location, Color.Black);
                    designer.RegisterDrawable(l);
                    designer._Lines.Add(l);
                }        
            }
        }

        public static vertex DrawVertex(Point pos, Color c)
        {
            // TODO
            if (designer.Canvas[pos.X, pos.Y].V != null)
                return designer.DrawVertex(new Point(pos.X - 1, pos.Y - 1), c); 

            for (int i = 3; i >=1; i--)
                designer._DrawCircle(pos, i, c);
            vertex ret = new vertex(pos, printer.Buffer, c);
            printer.FlushBuffer();
            return ret;
        }

        public static void DrawCircle(Point _center, int radius)
        {
            vertex center = designer.DrawVertex(_center, Color.Black);
            designer.RegisterVertex(center);

            circle c = designer._DrawCircle(_center, radius, Color.Black);
            designer.RegisterDrawable(c);
        }

        private static circle _DrawCircle(Point center, int radius, Color c)
        {
            int d = 1 - radius;
            int dE = 3;
            int dSE = 5 - 2 * radius;
            int dx, dy;

            Point p = new Point(center.X, center.Y + radius);

            printer.PutPixel(p.X, p.Y, c);
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

                printer.PutPixel(center.X + dx, center.Y + dy, c);
                printer.PutPixel(center.X - dx, center.Y + dy, c);
                printer.PutPixel(center.X + dx, center.Y - dy, c);
                printer.PutPixel(center.X - dx, center.Y - dy, c);
                                                               
                // zamieniamy osie                             
                printer.PutPixel(center.X + dy, center.Y + dx, c);
                printer.PutPixel(center.X - dy, center.Y + dx, c);
                printer.PutPixel(center.X + dy, center.Y - dx, c);
                printer.PutPixel(center.X - dy, center.Y - dx, c);
            }

            circle ret = new circle(center, radius, printer.Buffer, c);
            printer.FlushBuffer();
            return ret;
        }

        public static line DrawLine(Point start, Point end, Color c)
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

                    printer.PutPixel(p.X, p.Y, Color.Black);
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

                    printer.PutPixel(p.X, p.Y, Color.Black);
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

                    printer.PutPixel(p.X, p.Y, Color.Black);
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

                    printer.PutPixel(p.X, p.Y, Color.Black);
                }
                #endregion
            }

            line ret = new line(start, end, printer.Buffer, c);
            printer.FlushBuffer();

            return ret;
        }
    }
}
