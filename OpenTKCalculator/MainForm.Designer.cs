
namespace OpenTKCalculator
{
    partial class Form1
    {
        //this.glControl1 = new OpenTK.GLControl(OpenTK.Graphics.GraphicsMode.Default, 4, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
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
            this.glControl1 = new OpenTK.GLControl(OpenTK.Graphics.GraphicsMode.Default, 4, 0, OpenTK.Graphics.GraphicsContextFlags.Default);
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glControl1.Location = new System.Drawing.Point(13, 14);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(902, 645);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = true;
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            // 
            // expressionTextBox
            // 
            this.expressionTextBox.Location = new System.Drawing.Point(928, 12);
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Size = new System.Drawing.Size(322, 27);
            this.expressionTextBox.TabIndex = 1;
            this.expressionTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.expressionTextBox_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.expressionTextBox);
            this.Controls.Add(this.glControl1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "3D Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TextBox expressionTextBox;
    }
}

