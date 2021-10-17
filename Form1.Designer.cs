
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
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddVertexButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FixedRadiusButton = new System.Windows.Forms.Button();
            this.FixedLengthButton = new System.Windows.Forms.Button();
            this.FixedCenterButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCanvas);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMoveCanvas);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCanvas);
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
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(11, 23);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(182, 29);
            this.DeleteButton.TabIndex = 0;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickDeleteButton);
            // 
            // AddVertexButton
            // 
            this.AddVertexButton.Location = new System.Drawing.Point(11, 58);
            this.AddVertexButton.Name = "AddVertexButton";
            this.AddVertexButton.Size = new System.Drawing.Size(182, 31);
            this.AddVertexButton.TabIndex = 2;
            this.AddVertexButton.Text = "Add Vertex";
            this.AddVertexButton.UseVisualStyleBackColor = true;
            this.AddVertexButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickAddVertexButton);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AddVertexButton);
            this.groupBox2.Controls.Add(this.DeleteButton);
            this.groupBox2.Location = new System.Drawing.Point(707, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Edit";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.FixedCenterButton);
            this.groupBox3.Controls.Add(this.FixedRadiusButton);
            this.groupBox3.Controls.Add(this.FixedLengthButton);
            this.groupBox3.Location = new System.Drawing.Point(707, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 153);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Relations";
            // 
            // FixedRadiusButton
            // 
            this.FixedRadiusButton.Location = new System.Drawing.Point(7, 61);
            this.FixedRadiusButton.Name = "FixedRadiusButton";
            this.FixedRadiusButton.Size = new System.Drawing.Size(186, 35);
            this.FixedRadiusButton.TabIndex = 1;
            this.FixedRadiusButton.Text = "fixed radius";
            this.FixedRadiusButton.UseVisualStyleBackColor = true;
            this.FixedRadiusButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickFixedRadiusButton);
            // 
            // FixedLengthButton
            // 
            this.FixedLengthButton.Location = new System.Drawing.Point(7, 23);
            this.FixedLengthButton.Name = "FixedLengthButton";
            this.FixedLengthButton.Size = new System.Drawing.Size(186, 32);
            this.FixedLengthButton.TabIndex = 0;
            this.FixedLengthButton.Text = "fixed length";
            this.FixedLengthButton.UseVisualStyleBackColor = true;
            this.FixedLengthButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickFixedLengthButton);
            // 
            // FixedCenterButton
            // 
            this.FixedCenterButton.Location = new System.Drawing.Point(7, 103);
            this.FixedCenterButton.Name = "FixedCenterButton";
            this.FixedCenterButton.Size = new System.Drawing.Size(75, 23);
            this.FixedCenterButton.TabIndex = 2;
            this.FixedCenterButton.Text = "fixed center";
            this.FixedCenterButton.UseVisualStyleBackColor = true;
            this.FixedCenterButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickFixedCenterButton);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 563);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.canvas);
            this.Name = "mainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button ButtonPolyDraw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonCircleDraw;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddVertexButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button FixedLengthButton;
        private System.Windows.Forms.Button FixedRadiusButton;
        private System.Windows.Forms.Button FixedCenterButton;
    }
}

