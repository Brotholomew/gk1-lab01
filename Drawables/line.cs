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

        public Point Start { get => this.Vertices[0].Center; }
        public Point End { get => this.Vertices[1].Center; }

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

            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }

        public override void Move(MouseEventArgs e, Point distance, IRelation sanitizer, bool solo = true)
        {
            sanitizer.Sanitize(this, ref distance);
            Functors.MovePoints(this._Pixels, distance);

            if (solo)
            {
                foreach (var v in this._Vertices)
                {
                    v.Move(e, distance, sanitizer, false);
                    v.GetNext(this).Reprint();

                    this.Print(designer.Canvas.PrintToPreview);
                }
            }
        }

        public override void PostMove()
        {
            foreach (var v in this._Vertices)
            {
                v.GetNext(this).Register();
                v.Register();
            }

            base.PostMove();
        }

        public override void PreMove()
        {
            foreach (var v in this._Vertices)
            {
                v.DeregisterDrawable();
                v.GetNext(this).DeregisterDrawable();
            }

            base.PreMove();
        }

        public void Reprint()
        {
            this.UpdatePoints();

            line temp = null;
            designer.Canvas.PrintToPreview(() => temp = designer.DrawLine(this.Vertices[0].Center, this.Vertices[1].Center, embellisher.DrawColor));
            this._Pixels = temp._Pixels;
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

            this.Poly.Vertices.Add(v);

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
    }

}
