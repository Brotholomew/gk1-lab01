using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public enum PrintingStates { StartPoly, FollowMouse, Off };

    public static class embellisher
    {
        // cursors
        public static readonly Cursor NormalCursor = Cursors.Default;
        public static readonly Cursor PolyDrawCursor = Cursors.Cross;
        public static readonly Cursor CircleDrawCursor = Cursors.Cross;

        // colors
        public static readonly Brush DrawColor = Brushes.Black;
        public static readonly Brush TrackColor = new SolidBrush(Color.FromArgb(150, Color.Magenta));
        public static readonly Brush SelectedColor = new SolidBrush(Color.FromArgb(150, Color.DarkOrange));
        public static readonly Brush VertexBrush = Brushes.Blue;
        public static readonly Brush FirstVertexBrush = Brushes.Red;
        public static readonly Brush FixedLengthHighlightBrush = Brushes.LightCoral;
        public static readonly Brush FixedCenterBrush = Brushes.Red;
        public static readonly Brush EqualLengthBrush = Brushes.LawnGreen;
        public static readonly Brush AdjacentBrush = Brushes.CornflowerBlue;
        public static readonly Brush ParallelBrush = Brushes.Red;

        public static Cursor SwitchCursor(Cursor current, Cursor switched = null) {
            if (current == embellisher.NormalCursor)
                return switched;
            else
                return embellisher.NormalCursor;
        }
    }

    public static class Functors
    {
        public delegate void refAction<T>(ref T arg);
        public static void Swap<T>(ref T obj1, ref T obj2)
        {
            T buf;
            buf = obj1;
            obj1 = obj2;
            obj2 = buf;
        }
        public static Point Midpoint(Point p1, Point p2) => new Point((int)Math.Ceiling((p1.X + p2.X) / 2.0), (int)Math.Ceiling((p1.Y + p2.Y) / 2.0));
        public static Point MovePoint(Point p, Point distance) => new Point(p.X + distance.X, p.Y + distance.Y);
        public static void MovePoints(List<Point> l, Point distance)
        {
            for (int i = 0; i < l.Count; i++)
                l[i] = Functors.MovePoint(l[i], distance);
        }
        public static Point Distance(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);
        public static double RealDistance(Point start, Point end) => Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
        public static LineVariables GetLineVariables(line l)
        {
            Point start = l.Start;
            Point end = l.End;

            double a = double.PositiveInfinity;
            if (Math.Abs(l.Start.X - l.End.X) > 5) 
                a = (double)(end.Y - start.Y) / (double)(end.X - start.X);
            
            double b = start.Y - a * start.X;

            return new LineVariables(a, b);
        }
    }

    public struct MovingOpts
    {
        public bool Solo;
        public bool Stop;
        public bool CircleOpts;
        public bool CircleVMoving;

        public MovingOpts(bool _solo = true, bool _stop = false, bool _circleOpts = false, bool _circleVMoving = false)
        {
            this.Solo = _solo;
            this.Stop = _stop;
            this.CircleOpts = _circleOpts;
            this.CircleVMoving = _circleVMoving;
        }
    }

    public struct LineVariables
    {
        public double a;
        public double b;

        public LineVariables(double _a, double _b)
        {
            this.a = _a;
            this.b = _b;
        }
    }

    public class LineSlopeEquation
    {
        private double _a;
        private double _b;

        public double a { get => this._a; }
        public double b { get => this._b; }

        public LineSlopeEquation(line l) => this.Init(LineSlopeEquation.GetLineSlopeEquation(l));

        public LineSlopeEquation(LineVariables l) => this.Init(l);

        private void Init(LineVariables l)
        {
            this._a = l.a;
            this._b = l.b;
        }

        public void Shift(double db) => this._b += db;

        public LineVariables GetLineShifted(double db) => new LineVariables(this._a, this._b + db);
        
        public static LineVariables GetLineSlopeEquation(line l)
        {
            Point start = l.Start;
            Point end = l.End;

            double a = double.PositiveInfinity;
            if (Math.Abs(l.Start.X - l.End.X) > 5)
                a = (double)(end.Y - start.Y) / (double)(end.X - start.X);

            double b = start.Y - a * start.X;

            return new LineVariables(a, b);
        }
    }

    public class LineNormalEquation
    {
        private double _A;
        private double _B;
        private double _C;

        public double A { get => this._A; }
        public double B { get => this._B; }
        public double C { get => this._C; }
        public double rC 
        { 
            get 
            { 
                if (double.IsInfinity(this._A)) 
                    return (-1) * this._C; 
                else 
                    return this._C; 
            } 
        }

        public LineNormalEquation(double _A, double _B, double _C) => this.Init(_A, _B, _C);

        public LineNormalEquation(LineVariables l, Point p)
        {
            this.Init(l);

            if (double.IsInfinity(l.a))
                this._C = (-1) * p.X;
        }

        public LineNormalEquation(line l)
        {
            this.Init(LineSlopeEquation.GetLineSlopeEquation(l));

            if (double.IsInfinity(this._A))
                this._C = (-1) * l.Vertices[0].Center.X;
        }
        public double DistanceFrom(Point p)
        {
            double d = 0;
            double denominator = Math.Sqrt(Math.Pow(this._A, 2) + Math.Pow(this._B, 2));

            if (double.IsInfinity(this._A))
                return Math.Abs((-1) * this._C - p.X);

            if (denominator != 0)
                d = Math.Abs(this._A * p.X + this._B * p.Y + this._C) / denominator;

            return d;
        }

        public double GetC(Point p)
        {
            if (double.IsInfinity(this._A))
                return (-1) * p.X;

            return (-1) * (this._A * p.X + this._B * p.Y);
        }

        public double GetPerpendicularSlope()
        {
            if (double.IsInfinity(this._A))
                return 0;

            return (-1) * this._A;
        }

        public static (LineNormalEquation, LineNormalEquation) GetMovedLine(double r, double A, double B, double C)
        {;
            double c0 = C - r * Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));
            double c1 = C - (-1) * r * Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));

            if (double.IsInfinity(A))
            {
                c0 = C + r;
                c1 = C - r;
            }

            LineNormalEquation l0 = new LineNormalEquation(A, B, c0);
            LineNormalEquation l1 = new LineNormalEquation(A, B, c1);

            return (l0, l1);
        }

        private void Init(double _A, double _B, double _C)
        {
            this._A = _A;
            this._B = _B;
            this._C = _C;
        }

        private void Init(LineVariables l) => this.Init(l.a, -1, l.b);
        public LineVariables GetSlopeEquation() => new LineVariables((-1) * this._A / this._B, (-1) * this._C / this._B);

        public static Point Intersection(LineNormalEquation L1, LineNormalEquation L2)
        {
            LineVariables l1 = L1.GetSlopeEquation();
            LineVariables l2 = L2.GetSlopeEquation();

            int x = (int)((l2.b - l1.b) / (l1.a - l2.a));
            int y = (int)(l1.a * x + l1.b);
            
            return new Point(x, y);
        }
    }
}
