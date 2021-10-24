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
            this.Vertices[0].Highlight(how, embellisher.VertexBrush);
            base.Print(how, embellisher.DrawColor);
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
                //mo.Stop = true;
                designer.RelationSanitizer.PostMove(this, mo);
            }

            base.PostMove(mo);
        }

        public override void PreMove(MovingOpts mo)
        {
            if (!mo.Stop)
            {
                //mo.Stop = true;
                designer.RelationSanitizer.PreMove(this, mo);
            }

            base.PreMove(mo);
        }

        public override void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo)
        {
            if (mo.CircleOpts)
            {
                // move radius
                sanitizer.Sanitize(this, ref distance, mo);
                if (distance.X != 0 || distance.Y != 0)
                    this.radius = (int)Math.Ceiling(Functors.RealDistance(this.Vertices[0].Center, Functors.MovePoint(this._Pixels[0], distance)));
                circle temp = designer._DrawCircle(this.Vertices[0], this.radius, embellisher.DrawColor);
                this._Pixels = temp.Pixels;
            }
            else
            {
                mo.CircleVMoving = true;
                sanitizer.Sanitize(this, ref distance, mo);
                //this._Vertices[0].Move(e, distance, sanitizer, mo);
                Functors.MovePoints(this._Pixels, distance);
            }

            this.Print(designer.Canvas.PrintToPreview, embellisher.DrawColor);
        }

        public void Rescale(double newRadius)
        {
            Point p = new Point(-1, -1);
            designer.RelationSanitizer.Sanitize(this, ref p, new MovingOpts(_circleOpts: true));
            if (p.X == 0 && p.Y == 0) return;

            this.radius = (int)Math.Ceiling(newRadius);
            circle temp = designer._DrawCircle(this.Vertices[0], this.radius, embellisher.DrawColor);

            this._Pixels = temp.Pixels;
        }

        public override bool OnCircleRim(MouseEventArgs e)
        {
            double d = Functors.RealDistance(e.Location, this.Vertices[0].Center);
            return d >= this.Radius - 10;
        }

        public override void Reprint(int nx = 0, int ny = 0)
        {
            this.Move(null, new Point(nx, ny), designer.RelationSanitizer, new MovingOpts(_circleOpts: false));
            //Functors.MovePoints(this._Pixels, new Point(nx, ny));
            //this.Print(designer.Canvas.PrintToPreview, embellisher.DrawColor);
        }

        public override void Delete()
        {
            designer.RelationSanitizer.Delete(this);
            designer.Canvas.EraseVertices(this.Vertices);
            designer.Canvas.EraseDrawable(this);
            designer.Canvas.Reprint();
            printer.Erase();
        }

        public override void RespondToRelation(Relation rel, int nx = 0, int ny = 0)
        {
            rel.SanitizeCricle(this);
        }
    }
}
