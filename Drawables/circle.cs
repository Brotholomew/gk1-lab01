using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace lab01
{
    public class circle : drawable
    {
        private int radius;

        public int Radius { get => this.radius; }
        public circle(vertex _center, int _radius, List<Point> _pixels, Brush _brush) : base(_pixels, new List<vertex> { _center }, _brush)
        {
            this.radius = _radius;
        }

        public override void DeregisterDrawable()
        {
            this.Vertices[0].DeregisterDrawable();
            base.DeregisterDrawable();
        }

        public override void Print(Action<Action> how)
        {
            this.Vertices[0].Print(how);
            base.Print(how);
        }

        public override void Register()
        {
            base.Register();
            this.Vertices[0].Register();
        }

        public override void Move(MouseEventArgs e, Point distance, IRelation sanitizer, bool solo = true)
        {
            double d = Functors.RealDistance(e.Location, this.Vertices[0].Center);
            if (d >= this.Radius - 10)
            {
                // on circle rim
                sanitizer.Sanitize(this, ref distance, true);
                this.radius = (int)Math.Ceiling(d);
                circle temp = designer._DrawCircle(this.Vertices[0], this.radius, embellisher.DrawColor);
                this._Pixels = temp.Pixels;
            }
            else
            {
                sanitizer.Sanitize(this, ref distance);
                this._Vertices[0].Move(e, distance, sanitizer, false);
                Functors.MovePoints(this._Pixels, distance);
            }

            this.Print(designer.Canvas.PrintToPreview);
        }

        public override void Delete()
        {
            designer.Canvas.EraseVertices(this.Vertices);
            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }
    }
}
