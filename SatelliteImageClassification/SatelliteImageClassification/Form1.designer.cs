﻿namespace SatelliteImageClassification
{
    partial class mainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonGetInputFile = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonTrain = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.chartErrors = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonLoadNetwork = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_pictureBox = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_pictureBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonGetInputFile
            // 
            this.buttonGetInputFile.Location = new System.Drawing.Point(16, 21);
            this.buttonGetInputFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonGetInputFile.Name = "buttonGetInputFile";
            this.buttonGetInputFile.Size = new System.Drawing.Size(103, 28);
            this.buttonGetInputFile.TabIndex = 0;
            this.buttonGetInputFile.Text = "Dane treningowe";
            this.buttonGetInputFile.UseVisualStyleBackColor = true;
            this.buttonGetInputFile.Click += new System.EventHandler(this.buttonGetInputFile_Click);
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(256, 21);
            this.buttonTrain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(103, 28);
            this.buttonTrain.TabIndex = 16;
            this.buttonTrain.Text = "Ucz";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(381, 21);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonTest.Size = new System.Drawing.Size(103, 28);
            this.buttonTest.TabIndex = 17;
            this.buttonTest.Text = "Testuj";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // chartErrors
            // 
            this.chartErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            chartArea2.AxisX.Title = "Iteracje";
            chartArea2.AxisY.Title = "Wartość błędu";
            chartArea2.Name = "ChartArea1";
            this.chartErrors.ChartAreas.Add(chartArea2);
            this.chartErrors.Cursor = System.Windows.Forms.Cursors.Default;
            legend2.Name = "Legend1";
            this.chartErrors.Legends.Add(legend2);
            this.chartErrors.Location = new System.Drawing.Point(16, 76);
            this.chartErrors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chartErrors.Name = "chartErrors";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series5.Color = System.Drawing.Color.Blue;
            series5.Legend = "Legend1";
            series5.Name = "Layer1";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series6.Color = System.Drawing.Color.Red;
            series6.Legend = "Legend1";
            series6.Name = "Layer2";
            series7.BorderWidth = 2;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series7.Color = System.Drawing.Color.Black;
            series7.Legend = "Legend1";
            series7.Name = "Layer3";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series8.Color = System.Drawing.Color.Green;
            series8.Legend = "Legend1";
            series8.Name = "Layer4";
            this.chartErrors.Series.Add(series5);
            this.chartErrors.Series.Add(series6);
            this.chartErrors.Series.Add(series7);
            this.chartErrors.Series.Add(series8);
            this.chartErrors.Size = new System.Drawing.Size(404, 284);
            this.chartErrors.TabIndex = 18;
            this.chartErrors.Text = "Błędy";
            // 
            // buttonLoadNetwork
            // 
            this.buttonLoadNetwork.Location = new System.Drawing.Point(135, 21);
            this.buttonLoadNetwork.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonLoadNetwork.Name = "buttonLoadNetwork";
            this.buttonLoadNetwork.Size = new System.Drawing.Size(103, 28);
            this.buttonLoadNetwork.TabIndex = 19;
            this.buttonLoadNetwork.Text = "Wczytaj sieć";
            this.buttonLoadNetwork.UseVisualStyleBackColor = true;
            this.buttonLoadNetwork.Click += new System.EventHandler(this.buttonLoadNetwork_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(557, 346);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // panel_pictureBox
            // 
            this.panel_pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_pictureBox.AutoScroll = true;
            this.panel_pictureBox.Controls.Add(this.pictureBox1);
            this.panel_pictureBox.Location = new System.Drawing.Point(440, 76);
            this.panel_pictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel_pictureBox.Name = "panel_pictureBox";
            this.panel_pictureBox.Size = new System.Drawing.Size(424, 284);
            this.panel_pictureBox.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(505, 21);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(103, 28);
            this.button1.TabIndex = 22;
            this.button1.Text = "Zapisz - 0/9";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 369);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel_pictureBox);
            this.Controls.Add(this.buttonLoadNetwork);
            this.Controls.Add(this.chartErrors);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonTrain);
            this.Controls.Add(this.buttonGetInputFile);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "mainForm";
            this.Text = "Autoencoder";
            ((System.ComponentModel.ISupportInitialize)(this.chartErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_pictureBox.ResumeLayout(false);
            this.panel_pictureBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGetInputFile;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartErrors;
        private System.Windows.Forms.Button buttonLoadNetwork;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel_pictureBox;
        private System.Windows.Forms.Button button1;
    }
}

