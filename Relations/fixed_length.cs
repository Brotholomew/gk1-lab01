using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class FixedLength : Relation
    {
        private line _Drawable;
        private double _Length;

        public FixedLength(line d)
        {
            this._Drawable = d;
            popup p = new popup((int)d.Length, "length");
            p.ShowDialog();
            this._Length = p.Length;

            d.PreMove(new MovingOpts());
            d.Vertices[0].PreMove(new MovingOpts());
            d.Vertices[0].RespondToRelation(this);
            d.Vertices[0].PostMove(new MovingOpts());
            d.PostMove(new MovingOpts());
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> { (this._Drawable, embellisher.FixedLengthHighlightBrush) };
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            Point midpoint = Functors.Midpoint(this._Drawable.Start, this._Drawable.End);
            midpoint = new Point(midpoint.X + 5, midpoint.Y);

            return new List<((string, Point), Brush)> { (($"l = {this._Length}", midpoint), embellisher.StringBrush) };
        }

        public override bool ParallelEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }
        public override bool EqualEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }
        public override bool FixedLengthEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            // if (mo.Stop) return;
            //if (this._Break.Contains(d)) return;

            d.RespondToRelation(this);
        }

        public override void SanitizeVertex(vertex v, int nx = 0, int ny = 0)
        {
            if (this._Break.Contains(v)) return;
            if (!v.IsAdjacent(this._Drawable)) return;
            if (this._Drawable.MovingSimultaneously) return;
            this.StackOverflowControl(() => this._Drawable.Rescale(this._Length, v), new List<drawable> { v, this._Drawable.GetNext(v) });

            //vertex vx = this._Drawable.GetNext(v);
            //double newLength = Functors.RealDistance(vx.Center, v.Center);

            //Point d = Functors.Distance(vx.Center, v.Center);
            //double scale = newLength / this._Length;

            //Point np = new Point((int)(v.Center.X + d.X * scale), (int)(v.Center.Y + d.Y * scale));
            //Point distance = Functors.Distance(vx.Center, np);

            //if (distance.X != 0 || distance.Y != 0)
            //{
            //    // move the other vertex to match the fixed length
            //    vx.Move(null, distance, designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: true));
            //}
        }

        //public override void SanitizeLine(line l) 
        //{
        //    if (l == this._Drawable) return;
        //    if (!l.IsAdjacent(this._Drawable)) return;

        //    vertex v = l.CommonVertex(this._Drawable);
        //    vertex vx = this._Drawable.GetNext(v);
        //    v.RespondToRelation(this);
        //    vx.RespondToRelation(this);
        //}

        private List<vertex> GetAffectedVertices(vertex v)
        {
            List<vertex> ret = new List<vertex>();

            if (v.IsAdjacent(this._Drawable))
                ret.Add(this._Drawable.GetNext(v));

            return ret;
        }

        public override void PreMove(vertex v, MovingOpts mo) 
        {
            if (this._Break.Contains(v))
                return;

            this.StackOverflowControl(() =>
            {
                foreach (var vx in this.GetAffectedVertices(v))
                    vx.PreMove(mo);
            }, v);
        }
        public override void PostMove(vertex v, MovingOpts mo)
        {
            if (this._Break.Contains(v))
                return;

            this.StackOverflowControl(() =>
            {
                foreach (var vx in this.GetAffectedVertices(v))
                    vx.PostMove(mo);
            }, v);
        }

        public override bool IsBoundWith(drawable d) => d == this._Drawable;
    }
}
