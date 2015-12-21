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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonGetInputFile = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonTrain = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.chartErrors = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonLoadNetwork = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_pictureBox = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chartErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_pictureBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonGetInputFile
            // 
            this.buttonGetInputFile.Location = new System.Drawing.Point(21, 26);
            this.buttonGetInputFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetInputFile.Name = "buttonGetInputFile";
            this.buttonGetInputFile.Size = new System.Drawing.Size(137, 34);
            this.buttonGetInputFile.TabIndex = 0;
            this.buttonGetInputFile.Text = "Dane treningowe";
            this.buttonGetInputFile.UseVisualStyleBackColor = true;
            this.buttonGetInputFile.Click += new System.EventHandler(this.buttonGetInputFile_Click);
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(341, 26);
            this.buttonTrain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(137, 34);
            this.buttonTrain.TabIndex = 16;
            this.buttonTrain.Text = "Ucz";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(508, 26);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonTest.Size = new System.Drawing.Size(137, 34);
            this.buttonTest.TabIndex = 17;
            this.buttonTest.Text = "Testuj";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // chartErrors
            // 
            this.chartErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            chartArea5.AxisX.Title = "Iteracje";
            chartArea5.AxisY.Title = "Wartość błędu";
            chartArea5.Name = "ChartArea1";
            this.chartErrors.ChartAreas.Add(chartArea5);
            this.chartErrors.Cursor = System.Windows.Forms.Cursors.Default;
            legend5.Name = "Legend1";
            this.chartErrors.Legends.Add(legend5);
            this.chartErrors.Location = new System.Drawing.Point(21, 94);
            this.chartErrors.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartErrors.Name = "chartErrors";
            series17.ChartArea = "ChartArea1";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series17.Color = System.Drawing.Color.Black;
            series17.Legend = "Legend1";
            series17.Name = "Layer1";
            series18.ChartArea = "ChartArea1";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series18.Color = System.Drawing.Color.Red;
            series18.Legend = "Legend1";
            series18.Name = "Layer2";
            series19.ChartArea = "ChartArea1";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series19.Color = System.Drawing.Color.Aqua;
            series19.Legend = "Legend1";
            series19.Name = "Layer3";
            series20.ChartArea = "ChartArea1";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series20.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series20.Legend = "Legend1";
            series20.Name = "Layer4";
            this.chartErrors.Series.Add(series17);
            this.chartErrors.Series.Add(series18);
            this.chartErrors.Series.Add(series19);
            this.chartErrors.Series.Add(series20);
            this.chartErrors.Size = new System.Drawing.Size(539, 350);
            this.chartErrors.TabIndex = 18;
            this.chartErrors.Text = "Błędy";
            // 
            // buttonLoadNetwork
            // 
            this.buttonLoadNetwork.Location = new System.Drawing.Point(180, 26);
            this.buttonLoadNetwork.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLoadNetwork.Name = "buttonLoadNetwork";
            this.buttonLoadNetwork.Size = new System.Drawing.Size(137, 34);
            this.buttonLoadNetwork.TabIndex = 19;
            this.buttonLoadNetwork.Text = "Wczytaj sieć";
            this.buttonLoadNetwork.UseVisualStyleBackColor = true;
            this.buttonLoadNetwork.Click += new System.EventHandler(this.buttonLoadNetwork_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
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
            this.panel_pictureBox.Location = new System.Drawing.Point(586, 94);
            this.panel_pictureBox.Name = "panel_pictureBox";
            this.panel_pictureBox.Size = new System.Drawing.Size(565, 350);
            this.panel_pictureBox.TabIndex = 21;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 454);
            this.Controls.Add(this.panel_pictureBox);
            this.Controls.Add(this.buttonLoadNetwork);
            this.Controls.Add(this.chartErrors);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonTrain);
            this.Controls.Add(this.buttonGetInputFile);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
    }
}

