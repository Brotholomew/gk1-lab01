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
        #region Private Fields
        private static Point NullPoint = new Point(-1, -1);
        private static Point LastPoint = designer.NullPoint;
        private static Point FirstPoint = designer.NullPoint;

        private static line LastLine = null;
        private static List<drawable> _Lines = new List<drawable>();
        private static List<vertex> _Vertices = new List<vertex>();
        
        private static PrintingStates _State = PrintingStates.Off;
        
        private static Canvas _Canvas = null;
        private static RelationSanitizer _RelationSanitizer = new RelationSanitizer();
        #endregion

        #region Public Fields and Properties
        public static PrintingStates State { get => designer._State; }
        public static Canvas Canvas
        {
            get
            {
                if (designer._Canvas == null) return null;
                return designer._Canvas;
            }
        }
        public static RelationSanitizer RelationSanitizer { get => designer._RelationSanitizer; }

        public static List<drawable> Tracked = new List<drawable>();
        public static drawable LastTracked = null;
        public static drawable _Moving = null;
        public static Point _LastPoint = Point.Empty;
        public static Highlighter Highlighter = new Highlighter();
        #endregion

        public static void Initialize() => designer._Canvas = new Canvas();

        public static void FollowMouse(MouseEventArgs e, mainForm f)
        {
            if (designer._State == PrintingStates.FollowMouse)
            {
                designer._Canvas.ErasePreview();

                if (f.DM == DesignModes.Poly)
                    designer.LastLine = designer.DrawLine(LastPoint, e.Location, Brushes.Black);
                else if (f.DM == DesignModes.Circle)
                    designer.DrawCircle(e.Location, f);
            }
            else if (f.DM == DesignModes.Moving)
            {
                designer.Canvas.ErasePreview();
                drawable d = designer._Moving;
                d.Move(e, Functors.Distance(e.Location, designer._LastPoint), designer.RelationSanitizer, new MovingOpts(_solo : true, _circleOpts : true));
                designer._LastPoint = e.Location;
            }
            else
            {
                designer._Canvas.ErasePreview();
                designer.LastTracked = designer.Track(e.Location, Embellisher.TrackColor);
            }

            printer.Erase();
        }
    }
}
