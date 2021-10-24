using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class FixedRadius : Relation
    {
        private circle _Drawable;
        private int _Radius;

        public FixedRadius(circle c)
        {
            this._Drawable = c;

            popup p = new popup((int)c.Radius, "radius");
            p.ShowDialog();
            this._Radius = p.Length;

            c.PreMove(new MovingOpts());
            c.Rescale(this._Radius);
            c.PostMove(new MovingOpts());
        }

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> { (this._Drawable, embellisher.FixedLengthHighlightBrush) };
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            //Point midpoint = Functors.Midpoint(this._Drawable.Vertices[0].Center, new Point(this._Drawable.Vertices[0].Center.X + this._Radius, this._Drawable.Vertices[0].Center.Y));

            return new List<((string, Point), Brush)> { (($"r = {this._Radius}", new Point(this._Drawable.Vertices[0].Center.X + 10, this._Drawable.Vertices[0].Center.Y - 5)), embellisher.VertexBrush) };
        }

        public override bool FixedRadiusEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }
        public override bool FixedCenterEnabled(drawable d)
        {
            if (this.IsBoundWith(d))
                return false;

            return true;
        }

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (d is circle && mo.CircleOpts) distance = new Point(0, 0);
        }

        public override bool IsBoundWith(drawable d) => d == this._Drawable;
    }
}
