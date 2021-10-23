using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    class AdjacentCircle : Relation
    {
        private circle _Circle;
        private line _Line;

        public AdjacentCircle(circle c, line l)
        {
            this._Circle = c;
            this._Line = l;

            this._Circle.PreMove(new MovingOpts());
            this._Line.Poly.PreMove(new MovingOpts());
            this.StackOverflowControl(() => this.MoveDrawables(new MovingOpts()), new List<drawable> { this._Circle, this._Line });
            this._Line.Poly.PostMove(new MovingOpts());
            this._Circle.PostMove(new MovingOpts());
        }

        //public override List<(drawable, Brush)> GetHighlights()
        //{
        //    return new List<(drawable, Brush)> { (this._Circle, embellisher.FixedLengthHighlightBrush) };
        //}

        //public override List<((string, Point), Brush)> GetStrings()
        //{
        //    // Point midpoint = Functors.Midpoint(this._Drawable.Vertices[0].Center, new Point(this._Drawable.Vertices[0].Center.X + this._Drawable.Radius, this._Drawable.Vertices[0].Center.Y));

        //    return new List<((string, Point), Brush)> { (("fixed radius", midpoint), embellisher.FixedLengthHighlightBrush) };
        //}

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (!this.IsBoundWith(d)) return;
            if (this._Break.Contains(d)) return;
            this.StackOverflowControl(() => this.MoveDrawables(mo), new List<drawable> { d });

            // d.RespondToRelation(this);
        }

        //public override void SanitizePoly(poly p) => this.StackOverflowControl(() => this.MoveDrawables(), new List<drawable> { p, this._Line, this._Circle, this._Circle.Vertices[0] });
        //public override void SanitizeCricle(circle c) => this.StackOverflowControl(() => this.MoveDrawables(), new List<drawable> { c, this._Line, this._Circle.Vertices[0] });
        //public override void SanitizeLine(line l) => this.StackOverflowControl(() => this.MoveDrawables(), new List<drawable> { l, this._Circle, this._Circle.Vertices[0] });
        //public override void SanitizeVertex(vertex v) => this.StackOverflowControl(() => this.MoveDrawables(), new List<drawable> { v, this._Circle, this._Line });

        private void PreMove(drawable d)
        {
            if (!this.IsBoundWith(d))
                return;

            this._Circle.PreMove(new MovingOpts());
            this._Line.Poly.PreMove(new MovingOpts());
            this._Line.PreMove(new MovingOpts());
        }

        private void PostMove(drawable d)
        {
            if (!this.IsBoundWith(d))
                return;

            this._Circle.PostMove(new MovingOpts());
            this._Line.Poly.PostMove(new MovingOpts());
            this._Line.PostMove(new MovingOpts());
        }

        public override void PreMove(poly p, MovingOpts mo) { if (this._Break.Contains(p)) return; this.StackOverflowControl(() => this.PreMove(p), p); }
        public override void PreMove(circle c, MovingOpts mo) { if (this._Break.Contains(c)) return; this.StackOverflowControl(() => this.PreMove(c), c); }
        public override void PreMove(line l, MovingOpts mo) { if (this._Break.Contains(l)) return; this.StackOverflowControl(() => this.PreMove(l), l); }
        public override void PreMove(vertex v, MovingOpts mo) { if (this._Break.Contains(v)) return; this.StackOverflowControl(() => this.PreMove(v), v); }

        public override void PostMove(poly p, MovingOpts mo) { if (this._Break.Contains(p)) return; this.StackOverflowControl(() => this.PostMove(p), p); }
        public override void PostMove(circle c, MovingOpts mo) { if (this._Break.Contains(c)) return; this.StackOverflowControl(() => this.PostMove(c), c); }
        public override void PostMove(line l, MovingOpts mo) { if (this._Break.Contains(l)) return; this.StackOverflowControl(() => this.PostMove(l), l); }
        public override void PostMove(vertex v, MovingOpts mo) { if (this._Break.Contains(v)) return; this.StackOverflowControl(() => this.PostMove(v), v); }

        public override bool IsBoundWith(drawable d) => d == this._Circle || d == this._Line || d == this._Line.Poly  || d == this._Circle.Vertices[0];

        private void MoveDrawables(MovingOpts mo)
        {
            // case when a = infty
            int x0 = this._Line.Vertices[0].Center.X;
            int x1 = this._Line.Vertices[1].Center.X;
            int y0 = this._Line.Vertices[0].Center.Y;
            int y1 = this._Line.Vertices[1].Center.Y;

            //LineVariables temp = Functors.GetLineVariables(this._Line);
            LineNormalEquation temp = new LineNormalEquation(this._Line);
            double a = temp.GetPerpendicularSlope();

            LineNormalEquation boundary0 = new LineNormalEquation(new LineVariables(a, y0 - a * x0), this._Line.Vertices[0].Center);
            LineNormalEquation boundary1 = new LineNormalEquation(new LineVariables(a, y1 - a * x1), this._Line.Vertices[1].Center);
            LineNormalEquation circleLine = null;

            Point center = this._Circle.Vertices[0].Center;

            double check = boundary0.GetC(center);

            double lower = boundary0.rC <= boundary1.rC ? boundary0.rC : boundary1.rC;
            double upper = boundary0.rC <= boundary1.rC ? boundary1.rC : boundary0.rC;

            if (check > upper || check < lower)
            {
                // move circle or poly 
                double d1 = boundary0.DistanceFrom(center);
                double d2 = boundary1.DistanceFrom(center);

                if (d1 >= d2) Functors.Swap<LineNormalEquation>(ref boundary0, ref boundary1);
                Point target = new Point(0, 0);

                circleLine = new LineNormalEquation(new LineVariables(temp.A, center.Y - temp.A * center.X), center);
                target = LineNormalEquation.Intersection(boundary0, circleLine);

                // try to move circle
                if (!mo.CircleVMoving && !mo.CircleOpts)
                    this.StackOverflowControl(() => this._Circle.Vertices[0].Move(null, Functors.Distance(target, center), designer.RelationSanitizer, new MovingOpts(_circleOpts: false)), this._Circle.Vertices[0]);

                // if circle has not moved, move the line
                if (this._Circle.Vertices[0].Center == center)
                {
                    circleLine = new LineNormalEquation(new LineVariables(a, center.Y - a * center.X), center);
                    target = LineNormalEquation.Intersection(circleLine, temp);
                    Point closerV = Functors.RealDistance(this._Line.Start, target) <= Functors.RealDistance(target, this._Line.End) ? this._Line.Start : this._Line.End;

                    this.StackOverflowControl(() => this._Line.Move(null, Functors.Distance(target, closerV), designer.RelationSanitizer, new MovingOpts(_solo : true)), this._Line);
                }
            }

            // resize radius if needed
            center = this._Circle.Vertices[0].Center;
            double newRadius = temp.DistanceFrom(center);
            circleLine = new LineNormalEquation(new LineVariables(temp.A, center.Y - temp.A * center.X), center);

            if (!mo.CircleOpts && !mo.CircleVMoving)
                this._Circle.Rescale(newRadius);

            if (this._Circle.Radius != Math.Ceiling(newRadius))
            {
                // move circle
                (boundary0, boundary1) = LineNormalEquation.GetMovedLine(this._Circle.Radius, temp.A, temp.B, temp.C);

                a = temp.GetPerpendicularSlope();
                double d0 = boundary0.DistanceFrom(center);
                double d1 = boundary1.DistanceFrom(center);

                if (d0 >= d1) Functors.Swap<LineNormalEquation>(ref boundary0, ref boundary1);

                Point target = LineNormalEquation.Intersection(boundary0, circleLine);

                if (!mo.CircleVMoving && !mo.CircleOpts)
                    this.StackOverflowControl(() => this._Circle.Vertices[0].Move(null, Functors.Distance(target, center), designer.RelationSanitizer, new MovingOpts(_circleOpts: false)), this._Circle.Vertices[0]);

                // if circle has not moved, move the line
                if (this._Circle.Vertices[0].Center == center)
                {
                    circleLine = new LineNormalEquation(new LineVariables(temp.A, center.Y - temp.A * center.X), center);
                    (boundary0, boundary1) = LineNormalEquation.GetMovedLine(this._Circle.Radius, circleLine.A, circleLine.B, circleLine.C);
                    d0 = boundary0.DistanceFrom(this._Line.Vertices[0].Center);
                    d1 = boundary1.DistanceFrom(this._Line.Vertices[0].Center);

                    if (d0 >= d1) Functors.Swap<LineNormalEquation>(ref boundary0, ref boundary1);
                    
                    a = temp.GetPerpendicularSlope();
                    circleLine = new LineNormalEquation(new LineVariables(a, center.Y - a * center.X), center);

                    target = LineNormalEquation.Intersection(circleLine, boundary0);
                    Point beginning = LineNormalEquation.Intersection(circleLine, temp);

                    this.StackOverflowControl(() => this._Line.Move(null, Functors.Distance(target, beginning), designer.RelationSanitizer, new MovingOpts(_solo : true)), this._Line);
                }
            }

            // this._Line.Poly.Reprint();
        }
    }
}
