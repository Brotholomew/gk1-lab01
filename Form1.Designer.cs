
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.canvas = new System.Windows.Forms.PictureBox();
            this.buttonLineDraw = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
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
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.onMouseClickCanvas);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMoveCanvas);
            // 
            // buttonLineDraw
            // 
            this.buttonLineDraw.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonLineDraw.BackgroundImage")));
            this.buttonLineDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonLineDraw.Location = new System.Drawing.Point(707, 13);
            this.buttonLineDraw.Name = "buttonLineDraw";
            this.buttonLineDraw.Size = new System.Drawing.Size(71, 72);
            this.buttonLineDraw.TabIndex = 1;
            this.buttonLineDraw.UseVisualStyleBackColor = true;
            this.buttonLineDraw.Click += new System.EventHandler(this.clickButtonLineDraw);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 563);
            this.Controls.Add(this.buttonLineDraw);
            this.Controls.Add(this.canvas);
            this.Name = "mainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button buttonLineDraw;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

