namespace SN_segmentacja
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.seedDistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxColorDistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.crossRadioButton = new System.Windows.Forms.RadioButton();
            this.startRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.seedDistanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColorDistanceNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(72, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(44, 201);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Wczytaj obraz";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // seedDistanceNumeric
            // 
            this.seedDistanceNumeric.Location = new System.Drawing.Point(49, 44);
            this.seedDistanceNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.seedDistanceNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.seedDistanceNumeric.Name = "seedDistanceNumeric";
            this.seedDistanceNumeric.Size = new System.Drawing.Size(120, 20);
            this.seedDistanceNumeric.TabIndex = 2;
            this.seedDistanceNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Seed Distance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max Color Distance";
            // 
            // maxColorDistanceNumeric
            // 
            this.maxColorDistanceNumeric.DecimalPlaces = 3;
            this.maxColorDistanceNumeric.Location = new System.Drawing.Point(49, 108);
            this.maxColorDistanceNumeric.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maxColorDistanceNumeric.Name = "maxColorDistanceNumeric";
            this.maxColorDistanceNumeric.Size = new System.Drawing.Size(120, 20);
            this.maxColorDistanceNumeric.TabIndex = 5;
            this.maxColorDistanceNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // crossRadioButton
            // 
            this.crossRadioButton.AutoSize = true;
            this.crossRadioButton.Checked = true;
            this.crossRadioButton.Location = new System.Drawing.Point(49, 153);
            this.crossRadioButton.Name = "crossRadioButton";
            this.crossRadioButton.Size = new System.Drawing.Size(51, 17);
            this.crossRadioButton.TabIndex = 6;
            this.crossRadioButton.TabStop = true;
            this.crossRadioButton.Text = "Cross";
            this.crossRadioButton.UseVisualStyleBackColor = true;
            this.crossRadioButton.CheckedChanged += new System.EventHandler(this.crossRadioButton_CheckedChanged);
            // 
            // startRadioButton
            // 
            this.startRadioButton.AutoSize = true;
            this.startRadioButton.Location = new System.Drawing.Point(125, 153);
            this.startRadioButton.Name = "startRadioButton";
            this.startRadioButton.Size = new System.Drawing.Size(44, 17);
            this.startRadioButton.TabIndex = 7;
            this.startRadioButton.Text = "Star";
            this.startRadioButton.UseVisualStyleBackColor = true;
            this.startRadioButton.CheckedChanged += new System.EventHandler(this.startRadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 287);
            this.Controls.Add(this.startRadioButton);
            this.Controls.Add(this.crossRadioButton);
            this.Controls.Add(this.maxColorDistanceNumeric);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seedDistanceNumeric);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.seedDistanceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColorDistanceNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown seedDistanceNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown maxColorDistanceNumeric;
        private System.Windows.Forms.RadioButton crossRadioButton;
        private System.Windows.Forms.RadioButton startRadioButton;
    }
}

