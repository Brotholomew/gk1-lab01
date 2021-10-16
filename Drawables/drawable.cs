using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public abstract class drawable
    {
        protected List<Point> _Pixels;
        protected List<vertex> _Vertices;

        public virtual List<Point> Pixels { get => this._Pixels; }
        public virtual List<vertex> Vertices { get => this._Vertices; }
        public virtual Brush Brush { get; set; }
        public virtual bool IsDummy => false;

        public abstract void Delete();

        public abstract void Move(MouseEventArgs e, Point distance, IRelation sanitizer, bool solo = true);
        public virtual void PreMove()
        {
            this.DeregisterDrawable();
            designer.Canvas.Reprint();
            designer.Canvas.ErasePreview();
        }
        public virtual void PostMove()
        {
            this.Register();
            designer.Canvas.Reprint();
            designer.Canvas.ErasePreview();

            printer.Erase();
        }

        public virtual void Register() => designer.Canvas.RegisterDrawable(this);

        public virtual void DeregisterDrawable()
        {
            designer.Canvas.EraseDrawable(this);
        }

        public virtual void Print(Action<Action> how)
        {
            how(() => printer.PutPixels(this.Pixels, embellisher.DrawColor));
            printer.FlushBuffer();
        }

        public drawable(List<Point> _pixels, List<vertex> _vertices, Brush _brush)
        {
            this._Pixels = _pixels;
            this._Vertices = _vertices;
            this.Brush = _brush;
        }
    }
}