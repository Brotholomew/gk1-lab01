using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace lab01
{
    public static class printer
    {
        private static PictureBox CanvasWrapper;
        private static Graphics Canvas;
        private static SolidBrush Brush;
        private static List<Point> _Buffer;

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static void FlushBuffer() => printer._Buffer = new List<Point>();
        public static List<Point> Buffer { get => printer._Buffer; }

        public static void Initialize(PictureBox _canvas)
        {
            printer.CanvasWrapper = _canvas;
            printer.Canvas = printer.CanvasWrapper.CreateGraphics();

            printer.Height = printer.CanvasWrapper.Height;
            printer.Width = printer.CanvasWrapper.Width;

            printer.Brush = new SolidBrush(Color.Black);

            printer._Buffer = new List<Point>();
        }
        public static void PutPixel(int x, int y, Color color)
        {
            printer.Brush.Color = color;
            Canvas.FillRectangle(printer.Brush, x, y, 1, 1);
            printer._Buffer.Add(new Point(x, y));
        }

        public static void ErasePixel(int x, int y)
        {
            printer.Brush.Color = Color.White;
            Canvas.FillRectangle(printer.Brush, x, y, 1, 1);
        }

        public static void ErasePixels(List<Point> pixels)
        {
            foreach (var pixel in pixels)
                printer.ErasePixel(pixel.X, pixel.Y);
        }
    }
}
