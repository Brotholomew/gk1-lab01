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
        private static PrintingStates State = PrintingStates.Off;

        private static Point NullPoint = new Point(-1, -1);

        private static Point LastPoint = designer.NullPoint;
        private static Point FirstPoint = designer.NullPoint;

        private static Image LastImage = null;
        private static line LastLine = null;
        private static List<drawable> _Lines = new List<drawable>();
        private static List<vertex> _Vertices = new List<vertex>();

        public static Canvas Canvas = new Canvas();

        public static void Initialize()
        {
        }

        public static void FollowMouse(MouseEventArgs e)
        {
            if (designer.State == PrintingStates.FollowMouse)
            {
                if (designer.LastLine != null) designer.Canvas.ErasePreview();

                designer.LastLine = designer.DrawLine(LastPoint, e.Location, Brushes.Black);
            }
        } 

        private static double Distance(Point start, Point end) => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
    }
}
