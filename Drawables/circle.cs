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

        public override void Print(Action<Action> how, Brush brush)
        {
            this.Vertices[0].Print(how, brush);
            base.Print(how, brush);
        }

        public override void Highlight(Action<Action> how, Brush brush)
        {
            how(() => printer.PutFigure(this, brush));
            this.Vertices[0].Highlight(how, brush);
            base.Highlight(how, brush);
        }

        public override void Register()
        {
            base.Register();
            this.Vertices[0].Register();
        }

        public override void PostMove(MovingOpts mo)
        {
            if (!mo.Stop)
            {
                mo.Stop = true;
                designer.RelationSanitizer.PostMove(this, mo);
            }

            base.PostMove(mo);
        }

        public override void PreMove(MovingOpts mo)
        {
            if (!mo.Stop)
            {
                mo.Stop = true;
                designer.RelationSanitizer.PreMove(this, mo);
            }

            base.PreMove(mo);
        }

        public override void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo)
        {
            double d = Functors.RealDistance(e.Location, this.Vertices[0].Center);
            mo.Solo = false;

            if (d >= this.Radius - 10)
            {
                // on circle rim
                mo.CircleOpts = true;
                sanitizer.Sanitize(this, ref distance, mo);
                this.radius = (int)Math.Ceiling(d);
                circle temp = designer._DrawCircle(this.Vertices[0], this.radius, embellisher.DrawColor);
                this._Pixels = temp.Pixels;
            }
            else
            {
                sanitizer.Sanitize(this, ref distance, mo);
                this._Vertices[0].Move(e, distance, sanitizer, mo);
                Functors.MovePoints(this._Pixels, distance);
            }

            this.Print(designer.Canvas.PrintToPreview, embellisher.DrawColor);
        }

        public override void Delete()
        {
            designer.RelationSanitizer.Delete(this);
            designer.Canvas.EraseVertices(this.Vertices);
            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }

        public override void RespondToRelation(Relation rel)
        {
            rel.SanitizeCricle(this);
        }
    }
}
