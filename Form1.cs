using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab01
{
    public enum DesignModes { Off, Poly, Circle, Moving };

    public partial class mainForm : Form
    {
        private DesignModes _DesignMode = DesignModes.Off;
        public DesignModes DM { get => this._DesignMode;
            set
            {
                if (value == DesignModes.Off)
                    this.Cursor = embellisher.NormalCursor;

                this._DesignMode = value;
            }
        }
        public mainForm()
        {
            InitializeComponent();
            printer.Initialize(canvas);
            designer.Initialize();
            printer.SetGraphics(designer.Canvas.PreviewGraphics);
            this.UpdateButtons();
            this.PutUpPredefinedScene();
        }

        private void clickButtonLineDraw(object sender, EventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Poly ? DesignModes.Off : DesignModes.Poly;

            if (this._DesignMode == DesignModes.Poly)
                this.Cursor = embellisher.CircleDrawCursor;
        }

        private void MouseClickButtonCircleDraw(object sender, MouseEventArgs e)
        {
            this.DM = this._DesignMode == DesignModes.Circle ? DesignModes.Off : DesignModes.Circle;

            if (this._DesignMode == DesignModes.Circle)
                this.Cursor = embellisher.CircleDrawCursor;
        }

        private void mouseMoveCanvas(object sender, MouseEventArgs e)
        {
            designer.FollowMouse(e, this);

            
        }

        private void onMouseClickCanvas(object sender, MouseEventArgs e)
        {
            if (this._DesignMode == DesignModes.Poly)
            {
                designer.DrawPoly(e, this);
            }
            else if (this._DesignMode == DesignModes.Circle)
            {
                designer.DrawCircle(e.Location, this, designer.State == PrintingStates.FollowMouse);
            }

            if (e.Button == MouseButtons.Right)
            {
                designer.Tracked.Clear();
                designer.Canvas.Highlights.Clear();
                designer.Canvas.Reprint();
                printer.Erase();
            }

            if (designer.LastTracked != null)
            {
                this._DesignMode = DesignModes.Off;
                drawable d = designer.LastTracked;

                if (designer.Tracked.Contains(d))
                {
                    designer.Tracked.Remove(d);
                }
                else
                {
                    designer.Tracked.Add(d);
                }

                designer.Canvas.Reprint();
                printer.Erase();
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(designer.Canvas.MainBitmap, 0, 0);
            e.Graphics.DrawImage(designer.Canvas.Preview, 0, 0);
        }

        private void MouseUpCanvas(object sender, MouseEventArgs e)
        {
            if (designer._Moving != null)
            {
                drawable d = designer._Moving;
                this._DesignMode = DesignModes.Off;
                designer._LastPoint = Point.Empty;
                d.PostMove(new MovingOpts(_stop: false));
            }

            designer.LastTracked = null;
            designer._Moving = null;
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            DeleteButton.Enabled = false;
            AddVertexButton.Enabled = false;
            FixedLengthButton.Enabled = false;
            FixedRadiusButton.Enabled = false;
            FixedCenterButton.Enabled = false;
            EqualLengthsButton.Enabled = false;
            ParallelButton.Enabled = false;
            AdjacentCircleButton.Enabled = false;

            if (designer.Tracked.Count == 1)
            {
                DeleteButton.Enabled = true;

                if (designer.Tracked[0] is line)
                {
                    AddVertexButton.Enabled = true;
                    FixedLengthButton.Enabled = true;
                }

                if (designer.Tracked[0] is circle)
                {
                    FixedRadiusButton.Enabled = true;
                    FixedCenterButton.Enabled = true;
                }
            } 

            if (designer.Tracked.Count == 2)
            {
                if (designer.Tracked[0] is line && designer.Tracked[1] is line)
                {
                    EqualLengthsButton.Enabled = true;
                    ParallelButton.Enabled = true;
                }

                if ((designer.Tracked[0] is line && designer.Tracked[1] is circle) ||
                    (designer.Tracked[0] is circle && designer.Tracked[1] is line))
                    AdjacentCircleButton.Enabled = true;
            }
        }

        private void MouseClickDeleteButton(object sender, MouseEventArgs e)
        {
            drawable d = designer.Tracked[0];
            designer.Tracked.Clear();
            designer.Canvas.Highlights.Remove(d);
            d.Delete();
            this.UpdateButtons();
        }

        private void MouseClickAddVertexButton(object sender, MouseEventArgs e)
        {
            designer.Canvas.Highlights.Remove(designer.Tracked[0]);
            ((line)designer.Tracked[0]).AddVertex();
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseDownCanvas(object sender, MouseEventArgs e)
        {
            if (designer.LastTracked != null)
            {
                designer._Moving = designer.LastTracked;
                designer._LastPoint = e.Location;
                designer._Moving.PreMove(new MovingOpts(_stop : false));
                // moving
                this._DesignMode = DesignModes.Moving;
            }
        }

        private void MouseClickFixedLengthButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedLength((line)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickFixedRadiusButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedRadius((circle)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickFixedCenterButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new FixedCenter((circle)designer.Tracked[0]));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickEqualLengthsButton(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new EqualLenghts(designer.Tracked.ConvertAll((drawable d) => (line)d)));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MoouseClickParallelEdges(object sender, MouseEventArgs e)
        {
            designer.RelationSanitizer.AddRelation(new ParallelEdges(designer.Tracked.ConvertAll((drawable d) => (line)d)));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void MouseClickAdjacentCircleButton(object sender, MouseEventArgs e)
        {
            circle c = null;
            line l = null;
            foreach (var d in designer.Tracked)
            {
                if (d is line) l = (line)d;
                if (d is circle) c = (circle)d;
            }

            designer.RelationSanitizer.AddRelation(new AdjacentCircle(c, l));
            designer.Tracked.Clear();
            this.UpdateButtons();
        }

        private void PutUpPredefinedScene()
        {
            Point pc = new Point(450, 100);
            Point pp1 = new Point(450, 300);
            Point pp2 = new Point(500, 400);
            Point pp3 = new Point(300, 450);

            vertex cv = new vertex(pc, new List<Point> { pc }, embellisher.VertexBrush);
            vertex p1 = new vertex(pp1, new List<Point> { pp1 }, embellisher.VertexBrush);
            vertex p2 = new vertex(pp2, new List<Point> { pp2 }, embellisher.VertexBrush);
            vertex p3 = new vertex(pp3, new List<Point> { pp3 }, embellisher.VertexBrush);

            line l1 = designer.DrawLine(pp1, pp2, embellisher.DrawColor);
            line l2 = designer.DrawLine(pp2, pp3, embellisher.DrawColor);
            line l3 = designer.DrawLine(pp3, pp1, embellisher.DrawColor);

            l1.AddVertex(p1); l1.AddVertex(p2);
            l2.AddVertex(p2); l2.AddVertex(p3);
            l3.AddVertex(p3); l3.AddVertex(p1);

            p1.AddLine(l1); p1.AddLine(l3);
            p2.AddLine(l1); p2.AddLine(l2);
            p3.AddLine(l2); p3.AddLine(l3);

            circle c = designer._DrawCircle(cv, 75, embellisher.DrawColor);
            poly p = new poly(new List<drawable> { l1, l2, l3 }, new List<vertex> { p1, p2, p3 });

            l1.Poly = p; l2.Poly = p; l3.Poly = p;
            cv.AdjacentLines.Add(c);

            c.Register();
            p.Register();

            designer.Canvas.Reprint();
        }
    }
}
