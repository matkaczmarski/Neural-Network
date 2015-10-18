namespace MLPonline
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonGetInputFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxBias = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxActivationFunction = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownMomentum = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLearningRate = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownIterationsCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownNeuronsCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLayersCount = new System.Windows.Forms.NumericUpDown();
            this.buttonTrain = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.chartErrors = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMomentum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearningRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterationsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNeuronsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLayersCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGetInputFile
            // 
            this.buttonGetInputFile.Location = new System.Drawing.Point(21, 26);
            this.buttonGetInputFile.Name = "buttonGetInputFile";
            this.buttonGetInputFile.Size = new System.Drawing.Size(137, 35);
            this.buttonGetInputFile.TabIndex = 0;
            this.buttonGetInputFile.Text = "Dane treningowe";
            this.buttonGetInputFile.UseVisualStyleBackColor = true;
            this.buttonGetInputFile.Click += new System.EventHandler(this.buttonGetInputFile_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Liczba warstw:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Liczba neuronów:";
            this.toolTip.SetToolTip(this.label2, "Liczba neuronów w każdej warstwie.");
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Liczba iteracji:";
            this.toolTip.SetToolTip(this.label3, "Liczba iteracji algorytmu uczenia.");
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 34);
            this.label4.TabIndex = 9;
            this.label4.Text = "Współczynnik bezwładności:";
            this.toolTip.SetToolTip(this.label4, "Wartość współczynnika bezwładności (momentum).");
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Współczynnik nauki:";
            this.toolTip.SetToolTip(this.label5, "Wartość współczynnika nauki (learning rate).");
            // 
            // checkBoxBias
            // 
            this.checkBoxBias.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxBias.AutoSize = true;
            this.checkBoxBias.Location = new System.Drawing.Point(219, 277);
            this.checkBoxBias.Name = "checkBoxBias";
            this.checkBoxBias.Size = new System.Drawing.Size(18, 17);
            this.checkBoxBias.TabIndex = 11;
            this.checkBoxBias.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 277);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Bias:";
            // 
            // comboBoxActivationFunction
            // 
            this.comboBoxActivationFunction.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxActivationFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxActivationFunction.FormattingEnabled = true;
            this.comboBoxActivationFunction.Items.AddRange(new object[] {
            "unipolarna",
            "bipolarna"});
            this.comboBoxActivationFunction.Location = new System.Drawing.Point(168, 328);
            this.comboBoxActivationFunction.Name = "comboBoxActivationFunction";
            this.comboBoxActivationFunction.Size = new System.Drawing.Size(121, 24);
            this.comboBoxActivationFunction.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 332);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Funkcja aktywacji:";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.numericUpDownMomentum, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownLearningRate, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownIterationsCount, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownNeuronsCount, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownLayersCount, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxActivationFunction, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.checkBoxBias, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel.Location = new System.Drawing.Point(21, 85);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(305, 370);
            this.tableLayoutPanel.TabIndex = 15;
            // 
            // numericUpDownMomentum
            // 
            this.numericUpDownMomentum.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownMomentum.DecimalPlaces = 2;
            this.numericUpDownMomentum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownMomentum.Location = new System.Drawing.Point(178, 223);
            this.numericUpDownMomentum.Name = "numericUpDownMomentum";
            this.numericUpDownMomentum.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownMomentum.TabIndex = 21;
            // 
            // numericUpDownLearningRate
            // 
            this.numericUpDownLearningRate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownLearningRate.DecimalPlaces = 2;
            this.numericUpDownLearningRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownLearningRate.Location = new System.Drawing.Point(178, 171);
            this.numericUpDownLearningRate.Name = "numericUpDownLearningRate";
            this.numericUpDownLearningRate.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownLearningRate.TabIndex = 20;
            // 
            // numericUpDownIterationsCount
            // 
            this.numericUpDownIterationsCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownIterationsCount.Location = new System.Drawing.Point(178, 119);
            this.numericUpDownIterationsCount.Name = "numericUpDownIterationsCount";
            this.numericUpDownIterationsCount.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownIterationsCount.TabIndex = 19;
            // 
            // numericUpDownNeuronsCount
            // 
            this.numericUpDownNeuronsCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownNeuronsCount.Location = new System.Drawing.Point(178, 67);
            this.numericUpDownNeuronsCount.Name = "numericUpDownNeuronsCount";
            this.numericUpDownNeuronsCount.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownNeuronsCount.TabIndex = 18;
            // 
            // numericUpDownLayersCount
            // 
            this.numericUpDownLayersCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownLayersCount.Location = new System.Drawing.Point(178, 15);
            this.numericUpDownLayersCount.Name = "numericUpDownLayersCount";
            this.numericUpDownLayersCount.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownLayersCount.TabIndex = 17;
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(21, 481);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(137, 35);
            this.buttonTrain.TabIndex = 16;
            this.buttonTrain.Text = "Ucz";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(189, 481);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonTest.Size = new System.Drawing.Size(137, 35);
            this.buttonTest.TabIndex = 17;
            this.buttonTest.Text = "Testuj";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // chartErrors
            // 
            chartArea1.AxisX.Title = "Iteracje";
            chartArea1.AxisY.Title = "Wartość błędu";
            chartArea1.Name = "ChartArea1";
            this.chartErrors.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartErrors.Legends.Add(legend1);
            this.chartErrors.Location = new System.Drawing.Point(389, 85);
            this.chartErrors.Name = "chartErrors";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Błędy";
            this.chartErrors.Series.Add(series1);
            this.chartErrors.Size = new System.Drawing.Size(477, 300);
            this.chartErrors.TabIndex = 18;
            this.chartErrors.Text = "Błędy";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 537);
            this.Controls.Add(this.chartErrors);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonTrain);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.buttonGetInputFile);
            this.Name = "mainForm";
            this.Text = "MLP";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMomentum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearningRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterationsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNeuronsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLayersCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGetInputFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxBias;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxActivationFunction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.NumericUpDown numericUpDownLearningRate;
        private System.Windows.Forms.NumericUpDown numericUpDownIterationsCount;
        private System.Windows.Forms.NumericUpDown numericUpDownNeuronsCount;
        private System.Windows.Forms.NumericUpDown numericUpDownLayersCount;
        private System.Windows.Forms.NumericUpDown numericUpDownMomentum;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartErrors;
    }
}

