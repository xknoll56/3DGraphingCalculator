
namespace OpenTKCalculator
{
    partial class MainForm
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
            this.glControl1 = new OpenTK.GLControl();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hScrollScale = new System.Windows.Forms.HScrollBar();
            this.numericUpDownRotX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownRotY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRotZ = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonEvaluate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownCentroidZ = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCentroidX = new System.Windows.Forms.NumericUpDown();
            this.buttonResetCamera = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotZ)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCentroidZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCentroidX)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glControl1.Location = new System.Drawing.Point(259, 14);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(656, 645);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = true;
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            // 
            // expressionTextBox
            // 
            this.expressionTextBox.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.expressionTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.expressionTextBox.Location = new System.Drawing.Point(3, 23);
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Size = new System.Drawing.Size(314, 27);
            this.expressionTextBox.TabIndex = 1;
            this.expressionTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.expressionTextBox_KeyDown);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.listBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(12, 36);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(240, 244);
            this.listBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Expression";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Meshes";
            // 
            // hScrollScale
            // 
            this.hScrollScale.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.hScrollScale.LargeChange = 1;
            this.hScrollScale.Location = new System.Drawing.Point(3, 543);
            this.hScrollScale.Minimum = 10;
            this.hScrollScale.Name = "hScrollScale";
            this.hScrollScale.Size = new System.Drawing.Size(311, 26);
            this.hScrollScale.TabIndex = 5;
            this.hScrollScale.Value = 10;
            this.hScrollScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // numericUpDownRotX
            // 
            this.numericUpDownRotX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRotX.DecimalPlaces = 2;
            this.numericUpDownRotX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownRotX.Location = new System.Drawing.Point(35, 619);
            this.numericUpDownRotX.Maximum = new decimal(new int[] {
            1253305502,
            146,
            0,
            720896});
            this.numericUpDownRotX.Name = "numericUpDownRotX";
            this.numericUpDownRotX.Size = new System.Drawing.Size(55, 27);
            this.numericUpDownRotX.TabIndex = 6;
            this.numericUpDownRotX.ValueChanged += new System.EventHandler(this.numericUpDownRotX_ValueChanged);
            this.numericUpDownRotX.DragOver += new System.Windows.Forms.DragEventHandler(this.numericUpDownRotX_DragOver);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 585);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Euler Angles(x,y,z)";
            // 
            // numericUpDownRotY
            // 
            this.numericUpDownRotY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRotY.DecimalPlaces = 2;
            this.numericUpDownRotY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownRotY.Location = new System.Drawing.Point(138, 619);
            this.numericUpDownRotY.Maximum = new decimal(new int[] {
            1253305502,
            146,
            0,
            720896});
            this.numericUpDownRotY.Name = "numericUpDownRotY";
            this.numericUpDownRotY.Size = new System.Drawing.Size(55, 27);
            this.numericUpDownRotY.TabIndex = 11;
            this.numericUpDownRotY.ValueChanged += new System.EventHandler(this.numericUpDownRotY_ValueChanged);
            // 
            // numericUpDownRotZ
            // 
            this.numericUpDownRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRotZ.DecimalPlaces = 2;
            this.numericUpDownRotZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownRotZ.Location = new System.Drawing.Point(237, 619);
            this.numericUpDownRotZ.Maximum = new decimal(new int[] {
            1253305502,
            146,
            0,
            720896});
            this.numericUpDownRotZ.Name = "numericUpDownRotZ";
            this.numericUpDownRotZ.Size = new System.Drawing.Size(55, 27);
            this.numericUpDownRotZ.TabIndex = 12;
            this.numericUpDownRotZ.ValueChanged += new System.EventHandler(this.numericUpDownRotZ_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonResetCamera);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.buttonEvaluate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numericUpDownCentroidZ);
            this.panel1.Controls.Add(this.numericUpDownCentroidX);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.hScrollScale);
            this.panel1.Controls.Add(this.numericUpDownRotZ);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.expressionTextBox);
            this.panel1.Controls.Add(this.numericUpDownRotY);
            this.panel1.Controls.Add(this.numericUpDownRotX);
            this.panel1.Location = new System.Drawing.Point(922, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 649);
            this.panel1.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 509);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Scale";
            // 
            // buttonEvaluate
            // 
            this.buttonEvaluate.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonEvaluate.Location = new System.Drawing.Point(11, 152);
            this.buttonEvaluate.Name = "buttonEvaluate";
            this.buttonEvaluate.Size = new System.Drawing.Size(306, 29);
            this.buttonEvaluate.TabIndex = 16;
            this.buttonEvaluate.Text = "Evaluate";
            this.buttonEvaluate.UseVisualStyleBackColor = false;
            this.buttonEvaluate.Click += new System.EventHandler(this.buttonEvaluate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Centroid(x, z)";
            // 
            // numericUpDownCentroidZ
            // 
            this.numericUpDownCentroidZ.DecimalPlaces = 3;
            this.numericUpDownCentroidZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownCentroidZ.Location = new System.Drawing.Point(192, 99);
            this.numericUpDownCentroidZ.Name = "numericUpDownCentroidZ";
            this.numericUpDownCentroidZ.Size = new System.Drawing.Size(100, 27);
            this.numericUpDownCentroidZ.TabIndex = 14;
            // 
            // numericUpDownCentroidX
            // 
            this.numericUpDownCentroidX.DecimalPlaces = 3;
            this.numericUpDownCentroidX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownCentroidX.Location = new System.Drawing.Point(35, 99);
            this.numericUpDownCentroidX.Name = "numericUpDownCentroidX";
            this.numericUpDownCentroidX.Size = new System.Drawing.Size(100, 27);
            this.numericUpDownCentroidX.TabIndex = 13;
            // 
            // buttonResetCamera
            // 
            this.buttonResetCamera.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonResetCamera.Location = new System.Drawing.Point(3, 471);
            this.buttonResetCamera.Name = "buttonResetCamera";
            this.buttonResetCamera.Size = new System.Drawing.Size(324, 29);
            this.buttonResetCamera.TabIndex = 18;
            this.buttonResetCamera.Text = "Reset Camera";
            this.buttonResetCamera.UseVisualStyleBackColor = false;
            this.buttonResetCamera.Click += new System.EventHandler(this.buttonResetCamera_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.glControl1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "3D Calculator";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotZ)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCentroidZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCentroidX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TextBox expressionTextBox;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.HScrollBar hScrollScale;
        private System.Windows.Forms.NumericUpDown numericUpDownRotX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownRotY;
        private System.Windows.Forms.NumericUpDown numericUpDownRotZ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonEvaluate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownCentroidZ;
        private System.Windows.Forms.NumericUpDown numericUpDownCentroidX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonResetCamera;
    }
}

