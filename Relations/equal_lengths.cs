using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class EqualLenghts : Relation
    {
        private List<line> _Drawables;

        public EqualLenghts(List<line> _Drawables)
        {
            this._Drawables = _Drawables;

            line ln = this._Drawables[1];

            if (ln.Length != this._Drawables[0].Length)
            {
                ln.PreMove(new MovingOpts());
                this.StackOverflowControl((() => ln.Rescale(this._Drawables[0].Length)), ln.Vertices.ConvertAll(((vertex v) => (drawable)v)));
                ln.PostMove(new MovingOpts());
            }
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> (this._Drawables.ConvertAll((line l) =>  ((drawable)l, embellisher.DrawColor)));
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            //Point midpoint = Functors.Midpoint(this._Drawable.Vertices[0].Center, new Point(this._Drawable.Vertices[0].Center.X + this._Drawable.Radius, this._Drawable.Vertices[0].Center.Y));

            //return new List<((string, Point), Brush)> { (("fixed radius", midpoint), embellisher.FixedLengthHighlightBrush) };
            return new List<((string, Point), Brush)>();
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (this._Break.Contains(d)) return;

            foreach (var l in this._Drawables)
                if (l.MovingSimultaneously) return;

            this.StackOverflowControl(() => d.RespondToRelation(this), d);
        }

        public override void SanitizeLine(line l)
        {
            //if ()
        }

        public override void SanitizeVertex(vertex v)
        {
            line ln = this.GetAffectedLine(v);
            if (ln == null) return;

            this.StackOverflowControl((() => ln.Rescale(this.GetNext(ln).Length)), ln.Vertices.ConvertAll(((vertex v) => (drawable)v)));
        }

        public override bool IsBoundWith(drawable d) => d is line && this._Drawables.Contains((line)d);

        private line GetNext(line l)
        {
            foreach (var line in this._Drawables)
                if (line != l) return line;

            return null;
        }

        private line GetAffectedLine(vertex v)
        {
            line l = null;

            foreach (var ln in v.AdjacentLines)
                if (this.IsBoundWith(ln)) l = (line)ln;

            if (l == null) return l;

            line boundLine = this.GetNext(l);
            return boundLine;
        }

        public override void PreMove(vertex v, MovingOpts mo) 
        {
            line l = this.GetAffectedLine(v);
            if (l == null) return;

            if (this.GetNext(l).MovingSimultaneously) return;
            mo.Solo = false;
            l.PreMove(mo);
            //foreach (var d in this.GetAffectedLine(v)) /* d.PreMove(mo);*/
            //{
            //    if (this.) break;
            //    mo.Solo = false;
            //    d.PreMove(mo);
            //    /*mo.Solo = false;
            //    d.PreMove(mo);
            //    mo.Solo = false;
            //    d.Vertices[1].PreMove(mo);*/
            //}
        }
        public override void PostMove(vertex v, MovingOpts mo) 
        {
            line l = this.GetAffectedLine(v);
            if (l == null) return;

            if (this.GetNext(l).MovingSimultaneously) return;
            l.PostMove(mo);
            //foreach (var d in this.GetAffectedDrawables(v))
            //{
            //    if (d.MovingSimultaneously) break;
            //    d.PostMove(mo);
            //    /*mo.Solo = false;
            //    d.PostMove(mo);
            //    mo.Solo = false;
            //    d.Vertices[1].PostMove(mo);*/
            //}
        }
    }
}