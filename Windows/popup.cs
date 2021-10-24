using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lab01
{
    public partial class popup : Form
    {
        private int _Length;

        private void SetName(string name) => this.Text = $"Please provide {name}";
        public int Length { private set => this._Length = value; get => this._Length; }
        public popup(int value, string name)
        {
            InitializeComponent();
            this.SetName(name);
            this.Length = value;
            this.lengthTextBox.Text = this.Length.ToString();
        }

        private void OKButtonClick(object sender, EventArgs e)
        {
            this.Length = Int32.Parse(this.lengthTextBox.Text);
            this.Close();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
