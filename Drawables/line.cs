using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
    public class line : drawable
    {
        private Point _Start;
        private Point _End;

        private bool _MovingSimultaneously = false;
        public bool MovingSimultaneously
        {
            get
            {
                return this.Poly.MovingSimultaneously || this._MovingSimultaneously;
            }
        }

        public bool PolyMoving { get => this.Poly.MovingSimultaneously; }

        public Point Start { get => this.Vertices[0].Center; }
        public Point End { get => this.Vertices[1].Center; }
        public double Length { get => Functors.RealDistance(this.Start, this.End); }
        private double _OldLength;

        public poly Poly;
        public line(Point _start, Point _end, List<Point> _pixels, Brush _brush) : base(_pixels, new List<vertex>(), _brush)
        {
            this._Start = _start;
            this._End = _end;
        }

        public void AddVertex(vertex v) => this._Vertices.Add(v);

        public vertex GetNext(vertex v)
        {
            foreach (var vertex in this.Vertices)
                if (vertex != v) return vertex;

            return null;
        }

        public void Clear() => this._Vertices.Clear();

        public override void Delete()
        {
            poly p = this.Poly;

            if (this.Vertices.Count == 2)
            {
                if (this.Poly.Lines.Count <= 3)
                {
                    this.Poly.Delete();
                    return;
                }

                foreach (var v in this.Vertices)
                    v.AdjacentLines.Remove(this);

                vertex nv = vertex.Merge(this.Vertices[0], this.Vertices[1]);
                p.Vertices.Add(nv);
            }

            if (p != null)
            {
                p.Lines.Remove(this);
            }

            designer.RelationSanitizer.Delete(this);
            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }

        public override void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo)
        {
            sanitizer.Sanitize(this, ref distance);
            Functors.MovePoints(this._Pixels, distance);

            if (mo.Solo)
            {
                mo.Solo = false;
                foreach (var v in this._Vertices)
                {
                    v.Move(e, distance, sanitizer, mo);
                    v.GetNext(this).Reprint();

                    this.Print(designer.Canvas.PrintToPreview, embellisher.DrawColor);
                }
            }
        }

        public override void PostMove(MovingOpts mo)
        {
            foreach (var v in this._Vertices)
                v.PostMove(mo);

            //mo.Stop = true;
            designer.RelationSanitizer.PostMove(this, mo);

            this._MovingSimultaneously = false;
            base.PostMove(mo);
        }

        public override void PreMove(MovingOpts mo)
        {
            this._OldLength = this.Length;

            if (mo.Solo) this._MovingSimultaneously = true;
            foreach (var v in this._Vertices)
                v.PreMove(mo);

            //mo.Stop = true;
            designer.RelationSanitizer.PreMove(this, mo);

            base.PreMove(mo);
        }

        public override void Reprint(int nx = 0, int ny = 0)
        {
            this.UpdatePoints();

            line temp = null;
            designer.Canvas.PrintToPreview(() => temp = designer.DrawLine(this.Vertices[0].Center, this.Vertices[1].Center, embellisher.DrawColor));
            this._Pixels = temp._Pixels;

            Point dummy = new Point(0, 0);
            designer.RelationSanitizer.Sanitize(this, ref dummy, new MovingOpts());
        }

        private void UpdatePoints()
        {
            this._Start = this._Vertices[0].Center;
            this._End = this.Vertices[1].Center;
        }

        public void AddVertex()
        {
            Point midpoint = Functors.Midpoint(this.Start, this.End);
            vertex v = designer.DrawVertex(midpoint, embellisher.VertexBrush);

            this.Poly.AddVertex(this.Vertices[0], v, this.Vertices[1]);

            List<vertex> temp = new List<vertex>();
            foreach (var vx in this.Vertices)
            {
                temp.Add(vx);
                vx.AdjacentLines.Remove(this);

                line l = null;
                designer.Canvas.PrintToMain(() => l = designer.DrawLine(v.Center, vx.Center, embellisher.DrawColor));
                designer.Canvas.RegisterDrawable(l);
                l.Poly = this.Poly;
                l.Vertices.Add(vx);
                l.Vertices.Add(v);

                vx.AdjacentLines.Add(l);
                v.AdjacentLines.Add(l);
                this.Poly.Lines.Add(l);
            }

            this.Vertices.Clear();
            this.Delete();
        }

        public override void RespondToRelation(Relation rel, int nx = 0, int ny = 0)
        {
            rel.SanitizeLine(this);
        }

        public bool IsAdjacent(line l)
        {
            foreach (var v in this._Vertices)
                if (l._Vertices.Contains(v)) return true;

            return false;
        }

        public vertex CommonVertex(line l)
        {
            foreach (var v in this._Vertices)
                if (l._Vertices.Contains(v)) return v;

            return null;
        }

        public void Rescale(double length, vertex v = null)
        {
            if (v == null)
                v = this.Vertices[0];

            vertex vx = this.GetNext(v);
            double newLength = Functors.RealDistance(vx.Center, v.Center);

            Point d = Functors.Distance(vx.Center, v.Center);
            //double scale = newLength / length;
            double scale = length / newLength;

            if (newLength == 0)
                scale = length;

            Point np = new Point((int)(v.Center.X + d.X * scale), (int)(v.Center.Y + d.Y * scale));
            Point distance = Functors.Distance(np, vx.Center);

            vx.Move(null, distance, designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: false));
            v.Move(null, new Point(0,0), designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: false));
        }

        public void MakeParallel(line l, double oldLength)
        {
            double prevL = this.Length;
            double prevR = l.Length;

            LineVariables pattern = Functors.GetLineVariables(l);
            LineVariables L = Functors.GetLineVariables(this);

            vertex oldv0 = this.Start.Y <= this.End.Y ? this.Vertices[0] : this.Vertices[1];
            vertex oldv1 = this.Start.Y <= this.End.Y ? this.Vertices[1] : this.Vertices[0];
            Point oldStart = oldv0.Center;
            Point oldEnd = oldv1.Center;
            //double a = (oldEnd.Y - oldStart.y)

            int nXE = oldEnd.X, nYE = oldEnd.Y;
            int nXS = oldStart.X, nYS = oldStart.Y;
            double b = 0;
            Action improve = (() => { });

            if (L.a != pattern.a || !(double.IsInfinity(L.a) && double.IsInfinity(pattern.a)))
            {
                double a = pattern.a;
                if (double.IsInfinity(L.a))
                {
                    nXE = oldStart.X + (l.End.X - l.Start.X);
                    nXS = oldEnd.X + (l.End.X - l.Start.X);
                    a = pattern.a;
                }
                else if (double.IsInfinity(pattern.a))
                {
                    nXE = oldStart.X;
                    nXS = oldEnd.X;
                    a = L.a;
                }
                else
                {
                    nXE = (int) (this.Length * Math.Cos(Math.Atan(a)) + oldStart.X);
                    nXS = (int) (this.Length * Math.Cos(Math.Atan(a)) + oldEnd.X);
                }

                b = oldStart.Y - a * oldStart.X;
                nYE = (int) (a * nXE + b);
            
                b = oldEnd.Y - a * oldEnd.X;
                nYS = (int) (a * nXS + b);

                Point mE = Functors.Midpoint(oldStart, new Point(nXE, nYE));
                Point mS = Functors.Midpoint(new Point(nXS, nYS), oldEnd);
                Point m = Functors.Midpoint(l.Start, l.End);

                if (Functors.RealDistance(mS, m) <= Functors.RealDistance(mE, m))
                { nXE = oldEnd.X; nYE = oldEnd.Y; improve = () => this.Rescale(this._OldLength, oldv0); }
                else 
                { nXS = oldStart.X; nYS = oldStart.Y; improve = () => this.Rescale(this._OldLength, oldv1); }
            }

            oldv1.Move(null, Functors.Distance(new Point(nXE, nYE), oldEnd), designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: false));
            oldv0.Move(null, Functors.Distance(new Point(nXS, nYS), oldStart), designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: false));
            improve.Invoke();
        }

    }

}
