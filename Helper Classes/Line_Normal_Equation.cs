using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
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

        private void Init(double _A, double _B, double _C)
        {
            this._A = _A;
            this._B = _B;
            this._C = _C;
        }

        private void Init(LineVariables l) => this.Init(l.a, -1, l.b);

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
                return p.X;

            if (this._A == 0)
                return this.C;

            return (-1) * (this._A * p.X + this._B * p.Y);
        }

        public double GetPerpendicularSlope()
        {
            if (double.IsInfinity(this._A))
                return 0;

            return (-1) * 1 / this._A;
        }

        public static (LineNormalEquation, LineNormalEquation) GetMovedLine(double r, double A, double B, double C)
        {
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

        public LineVariables GetSlopeEquation() => new LineVariables((-1) * this._A / this._B, (-1) * this._C / this._B);

        public static Point Intersection(LineNormalEquation L1, LineNormalEquation L2)
        {
            if (double.IsInfinity(L1.A) && double.IsInfinity(L2.A) || L1.A == L2.A)
                return new Point(int.MaxValue, int.MaxValue);

            LineVariables l1 = L1.GetSlopeEquation();
            LineVariables l2 = L2.GetSlopeEquation();

            int x = (int)((l2.b - l1.b) / (l1.a - l2.a));
            int y = (int)(l1.a * x + l1.b);

            if (double.IsInfinity(L1.A))
                x = (int)L1.C * (-1);

            if (double.IsInfinity(L2.A))
                x = (int)L2.C * (-1);

            if (L1.A == 0)
                y = (int)L1.C;

            if (L2.A == 0)
                y = (int)L2.C;

            return new Point(x, y);
        }
    }
}
