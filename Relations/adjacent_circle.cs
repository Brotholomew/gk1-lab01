using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace lab01
{
    class AdjacentCircle
    {
        private circle _Circle;
        private line _Line;

        public AdjacentCircle(circle c, line l)
        {
            this._Circle = c;
            this._Line = l;
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

        //public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        //{
        //    if (mo.Stop) return;
        //    if (d is circle && !mo.CircleOpts) distance = new Point(0, 0);

        //    d.RespondToRelation(this);
        //}

        //public override bool IsBoundWith(drawable d) => d == this._Drawable;
    }
}
