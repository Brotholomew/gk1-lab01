using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace lab01
{
    public static partial class designer
    {
        private static PrintingStates _State = PrintingStates.Off;
        public static PrintingStates State { get => designer._State; }

        private static Point NullPoint = new Point(-1, -1);

        private static Point LastPoint = designer.NullPoint;
        private static Point FirstPoint = designer.NullPoint;

        private static line LastLine = null;
        private static List<drawable> _Lines = new List<drawable>();
        private static List<vertex> _Vertices = new List<vertex>();

        public static List<drawable> Tracked = new List<drawable>();
        public static drawable LastTracked = null;

        private static Canvas _Canvas = null;

        public static void Initialize()
        {
            designer._Canvas = new Canvas();
        }

        public static void FollowMouse(MouseEventArgs e, mainForm f)
        {
            if (designer._State == PrintingStates.FollowMouse)
            {
                designer._Canvas.ErasePreview();

                if (f.DM == DesignModes.Poly)
                    designer.LastLine = designer.DrawLine(LastPoint, e.Location, Brushes.Black);
                else if (f.DM == DesignModes.Circle)
                    designer.DrawCircle(e.Location, f);
                else
                {
                    
                }

                
            } 
            else
            {
                designer._Canvas.ErasePreview();
                designer.LastTracked = designer.Track(e.Location, embellisher.TrackColor);
            }

            printer.Erase();
        }

        public static Canvas Canvas { 
            get 
            {
                if (designer._Canvas == null) return null;
                return designer._Canvas;
            } 
        } 

        private static double Distance(Point start, Point end) => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));

        private static bool PointOnLine(double a, double b, Point p) => a * p.X + b == p.Y;
        private static bool LineCrossesY(line l, Point p) => (p.X >= l.Start.X || p.X >= l.End.X) && (p.Y > l.Start.Y && p.Y < l.End.Y) || (p.Y < l.Start.Y && p.Y > l.End.Y); 
    }
}
