using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace lab01
{
    public class FixedCenter : Relation
    {
        private circle _Drawable;
        private vertex _Center;

        public FixedCenter(circle c)
        {
            this._Drawable = c;
            this._Center = c.Vertices[0];
        }

        #region Prints and Highlights

        public override List<(drawable, Brush)> GetHighlights()
        {
            return new List<(drawable, Brush)> { (this._Center, Embellisher.FixedLengthHighlightBrush) };
        }

        public override List<((string, Point), Brush)> GetStrings()
        {
            return new List<((string, Point), Brush)> { (($"C = ({this._Center.Center.X}, {this._Center.Center.Y})", new Point(this._Drawable.Vertices[0].Center.X + 10, this._Drawable.Vertices[0].Center.Y - 5)), Embellisher.VertexBrush) };
        }

        #endregion

        #region Prohibit other Relations

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

        #endregion

        #region Sanitize

        public override void Sanitize(drawable d, ref Point distance, MovingOpts mo)
        {
            if (mo.Stop) return;
            if (!this.IsBoundWith(d)) return;
            if (this._Break.Contains(d)) return;
            if (d is circle && !mo.CircleOpts) distance = new Point(0, 0);

            int nx = distance.X;
            int ny = distance.Y;

            this.StackOverflowControl(() => d.RespondToRelation(this, nx, ny), d);
        }

        public override void SanitizeVertex(vertex v, int nx = 0, int ny = 0)
        {
            Point reduce = new Point((-1) * nx, (-1) * ny);
            v.Move(null, reduce, designer.RelationSanitizer, new MovingOpts());
        }

        #endregion

        public override bool IsBoundWith(drawable d) => d == this._Drawable || d == this._Center;
    }
}
