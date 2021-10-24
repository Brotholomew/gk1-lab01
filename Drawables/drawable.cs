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

        public drawable(List<Point> _pixels, List<vertex> _vertices, Brush _brush)
        {
            this._Pixels = _pixels;
            this._Vertices = _vertices;
            this.Brush = _brush;
        }

        #region Moving

        public abstract void Move(MouseEventArgs e, Point distance, Relation sanitizer, MovingOpts mo);

        public virtual void PreMove(MovingOpts mo)
        {
            this.DeregisterDrawable();
            designer.Canvas.Reprint();
            designer.Canvas.ErasePreview();
        }

        public virtual void PostMove(MovingOpts mo)
        {
            this.Register();
            designer.Canvas.Reprint();
            designer.Canvas.ErasePreview();

            printer.Erase();
        }

        #endregion

        #region Canvas Register and Deregister

        public virtual void Register() => designer.Canvas.RegisterDrawable(this);

        public virtual void DeregisterDrawable()
        {
            designer.Canvas.EraseDrawable(this);
        }

        #endregion

        #region Reprints and Highlights

        public virtual void Reprint(int nx = 0, int ny = 0) { }
        
        public virtual void Print(Action<Action> how, Brush brush)
        {
            how(() => printer.PutPixels(this.Pixels, brush));
            printer.FlushBuffer();
        }

        public virtual void Highlight(Action<Action> how, Brush brush) => this.Print(how, brush);

        #endregion

        public virtual bool OnCircleRim(MouseEventArgs e) => false;
        public abstract void Delete();
        public abstract void RespondToRelation(Relation rel, int nx = 0, int ny = 0);
    }
}