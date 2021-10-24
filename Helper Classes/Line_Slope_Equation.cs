using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
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
}
