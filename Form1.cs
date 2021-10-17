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
    }
}
