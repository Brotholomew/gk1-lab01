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
            this._Length = Functors.RealDistance(d.Start, d.End);
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> { (this._Drawable, embellisher.FixedLengthHighlightBrush) };
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            Point midpoint = Functors.Midpoint(this._Drawable.Start, this._Drawable.End);
            midpoint = new Point(midpoint.X, midpoint.Y + 5);

            return new List<((string, Point), Brush)> { (("fixed length", midpoint), embellisher.FixedLengthHighlightBrush) };
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;

            d.RespondToRelation(this);
        }

        public override void SanitizeVertex(vertex v)
        {
            if (!v.IsAdjacent(this._Drawable)) return;

            vertex vx = this._Drawable.GetNext(v);
            double newLength = Functors.RealDistance(vx.Center, v.Center);

            Point d = Functors.Distance(vx.Center, v.Center);
            double scale = newLength / this._Length;

            Point np = new Point((int)(v.Center.X + d.X * scale), (int)(v.Center.Y + d.Y * scale));
            Point distance = Functors.Distance(vx.Center, np);

            if (distance.X != 0 || distance.Y != 0)
            {
                // move the other vertex to match the fixed length
                vx.Move(null, distance, designer.RelationSanitizer, new MovingOpts(_solo: true, _stop: true));
            }
        }

        private List<vertex> GetAffectedVertices(vertex v)
        {
            List<vertex> ret = new List<vertex>();

            if (v.IsAdjacent(this._Drawable))
                ret.Add(this._Drawable.GetNext(v));

            return ret;
        }

        public override void PreMove(vertex v, MovingOpts mo) { foreach (var vx in this.GetAffectedVertices(v)) vx.PreMove(mo); }
        public override void PostMove(vertex v, MovingOpts mo) { foreach (var vx in this.GetAffectedVertices(v)) vx.PostMove(mo); }
        public override bool IsBoundWith(drawable d) => d == this._Drawable;
    }
}
