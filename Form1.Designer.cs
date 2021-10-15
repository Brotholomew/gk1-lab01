
namespace lab01
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.canvas = new System.Windows.Forms.PictureBox();
            this.ButtonPolyDraw = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonCircleDraw = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas.Location = new System.Drawing.Point(12, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(688, 539);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.onMouseClickCanvas);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMoveCanvas);
            // 
            // ButtonPolyDraw
            // 
            this.ButtonPolyDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ButtonPolyDraw.Location = new System.Drawing.Point(12, 22);
            this.ButtonPolyDraw.Name = "ButtonPolyDraw";
            this.ButtonPolyDraw.Size = new System.Drawing.Size(182, 30);
            this.ButtonPolyDraw.TabIndex = 1;
            this.ButtonPolyDraw.Text = "Poly / Line";
            this.ButtonPolyDraw.UseVisualStyleBackColor = true;
            this.ButtonPolyDraw.Click += new System.EventHandler(this.clickButtonLineDraw);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonCircleDraw);
            this.groupBox1.Controls.Add(this.ButtonPolyDraw);
            this.groupBox1.Location = new System.Drawing.Point(706, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 101);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Draw";
            // 
            // ButtonCircleDraw
            // 
            this.ButtonCircleDraw.Location = new System.Drawing.Point(12, 58);
            this.ButtonCircleDraw.Name = "ButtonCircleDraw";
            this.ButtonCircleDraw.Size = new System.Drawing.Size(182, 33);
            this.ButtonCircleDraw.TabIndex = 2;
            this.ButtonCircleDraw.Text = "Circle";
            this.ButtonCircleDraw.UseVisualStyleBackColor = true;
            this.ButtonCircleDraw.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickButtonCircleDraw);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 563);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.canvas);
            this.Name = "mainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button ButtonPolyDraw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonCircleDraw;
    }
}

