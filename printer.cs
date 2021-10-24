using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab01
{
    public static class printer
    {
        private static readonly int DEBUG = 0;
        private static PictureBox CanvasWrapper;
        private static Graphics Canvas;
        private static List<Point> _Buffer;

        public static readonly int PixelSize = 1;
        public static readonly int VertexRadius = 3;

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static void FlushBuffer() => printer._Buffer = new List<Point>();
        public static List<Point> Buffer { get => printer._Buffer; }

        public static void Initialize(PictureBox _canvas)
        {
            printer.CanvasWrapper = _canvas;
            // printer.Canvas = printer.CanvasWrapper.CreateGraphics();

            printer.Height = printer.CanvasWrapper.Height;
            printer.Width = printer.CanvasWrapper.Width;

            printer._Buffer = new List<Point>();
        }

        public static void SetGraphics(Graphics gx) => printer.Canvas = gx;

        public static void PutPixel(Point p, Brush brush)
        {
            printer.Canvas.FillRectangle(brush, p.X, p.Y, printer.PixelSize, printer.PixelSize);
            printer._Buffer.Add(new Point(p.X, p.Y));
        }

        public static void PutPixel(int x, int y, Brush brush) => printer.PutPixel(new Point(x, y), brush);

        public static void PutPixels(List<Point> pixels, Brush brush)
        {
            foreach (var pixel in pixels)
               printer.PutPixel(pixel, brush);
        }

        public static void PutVertex(Point p, Brush brush)
        {
            if (printer.DEBUG == 1) printer.Canvas.DrawString($"({p.X},{p.Y})", SystemFonts.DefaultFont, embellisher.DrawColor, new Point(p.X, p.Y + 10));
            printer.PutCircle(p, printer.VertexRadius, brush);
        }

        private static void PutCircle(Point p, int radius, Brush brush) => printer.Canvas.FillEllipse(brush, p.X - radius, p.Y - radius, 2 * radius + 1, 2 * radius + 1);

        public static void PutFigure(drawable d, Brush brush)
        {
            if (d is circle)
            {
                circle temp = (circle)d;
                printer.PutCircle(temp.Vertices[0].Pixels[0], temp.Radius, brush);
                printer.PutVertex(temp.Vertices[0].Pixels[0], temp.Vertices[0].Brush);
                //printer.PutPixels(temp.Pixels, );
            }
            else if (d is poly)
            {
                poly temp = (poly)d;
                printer.Canvas.FillPolygon(brush, temp.Vertices.ConvertAll((vertex v) => v.Pixels[0]).ToArray());
            }
        }

        public static void Erase()
        {
            printer.CanvasWrapper.Invalidate();
            printer.CanvasWrapper.Update();
        }

        public static void PrintString(string str, Point p, Brush brush) => printer.Canvas.DrawString(str, SystemFonts.DefaultFont, brush, p);

        #region debug

        public static void ShowDebugMsg(string str) => printer.Canvas.DrawString(str, SystemFonts.DefaultFont, Brushes.Black, new Point(15, 15));

        #endregion
    }
}
