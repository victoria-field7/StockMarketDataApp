namespace Project_2
{
    partial class Form_ChartDisplay
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
            this.chart_OHLCV = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.label_StartDate = new System.Windows.Forms.Label();
            this.label_EndDate = new System.Windows.Forms.Label();
            this.hScrollBar_CandlestickMargin = new System.Windows.Forms.HScrollBar();
            this.comboBox_UpWaves = new System.Windows.Forms.ComboBox();
            this.comboBox_DownWaves = new System.Windows.Forms.ComboBox();
            this.label_UpWave = new System.Windows.Forms.Label();
            this.label_DownWaves = new System.Windows.Forms.Label();
            this.label_selectMargin = new System.Windows.Forms.Label();
            this.Timer_Simulate = new System.Windows.Forms.Timer(this.components);
            this.label_ConfirmationCount = new System.Windows.Forms.Label();
            this.button_Simulate = new System.Windows.Forms.Button();
            this.button_Plus = new System.Windows.Forms.Button();
            this.button_Minus = new System.Windows.Forms.Button();
            //this.trackBar_MaxPrice = new System.Windows.Forms.TrackBar();
            //this.numericUpDown_stepSize = new System.Windows.Forms.NumericUpDown();
            //this.trackBar_MinPrice = new System.Windows.Forms.TrackBar();
            this.candlestickBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart_OHLCV)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.trackBar_MaxPrice)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.numericUpDown_stepSize)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.trackBar_MinPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_OHLCV
            // 
            chartArea1.Name = "ChartArea_OHLC";
            this.chart_OHLCV.ChartAreas.Add(chartArea1);
            this.chart_OHLCV.Dock = System.Windows.Forms.DockStyle.Top;
            legend1.Name = "Legend1";
            this.chart_OHLCV.Legends.Add(legend1);
            this.chart_OHLCV.Location = new System.Drawing.Point(0, 0);
            this.chart_OHLCV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_OHLCV.Name = "chart_OHLCV";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=0\\, 192\\, 0";
            series1.IsVisibleInLegend = false;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Series_OHLC";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series1.YValueMembers = "High,Low,Open,Close";
            series1.YValuesPerPoint = 4;
            this.chart_OHLCV.Series.Add(series1);
            this.chart_OHLCV.Size = new System.Drawing.Size(1550, 525);
            this.chart_OHLCV.TabIndex = 2;
            this.chart_OHLCV.Text = "chart_Display";
            // 
            // button_Refresh
            // 
            this.button_Refresh.Location = new System.Drawing.Point(408, 581);
            this.button_Refresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(75, 28);
            this.button_Refresh.TabIndex = 1;
            this.button_Refresh.Text = "Refresh";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(107, 582);
            this.dateTimePicker_StartDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(265, 22);
            this.dateTimePicker_StartDate.TabIndex = 2;
            this.dateTimePicker_StartDate.Value = new System.DateTime(2021, 2, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(592, 586);
            this.dateTimePicker_EndDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(265, 22);
            this.dateTimePicker_EndDate.TabIndex = 3;
            this.dateTimePicker_EndDate.Value = new System.DateTime(2021, 2, 28, 0, 0, 0, 0);
            // 
            // label_StartDate
            // 
            this.label_StartDate.AutoSize = true;
            this.label_StartDate.Location = new System.Drawing.Point(20, 586);
            this.label_StartDate.Name = "label_StartDate";
            this.label_StartDate.Size = new System.Drawing.Size(69, 16);
            this.label_StartDate.TabIndex = 4;
            this.label_StartDate.Text = "Start Date:";
            // 
            // label_EndDate
            // 
            this.label_EndDate.AutoSize = true;
            this.label_EndDate.Location = new System.Drawing.Point(500, 588);
            this.label_EndDate.Name = "label_EndDate";
            this.label_EndDate.Size = new System.Drawing.Size(66, 16);
            this.label_EndDate.TabIndex = 5;
            this.label_EndDate.Text = "End Date:";
            // 
            // hScrollBar_CandlestickMargin
            // 
            this.hScrollBar_CandlestickMargin.LargeChange = 8;
            this.hScrollBar_CandlestickMargin.Location = new System.Drawing.Point(200, 538);
            this.hScrollBar_CandlestickMargin.Minimum = 1;
            this.hScrollBar_CandlestickMargin.Name = "hScrollBar_CandlestickMargin";
            this.hScrollBar_CandlestickMargin.Size = new System.Drawing.Size(539, 27);
            this.hScrollBar_CandlestickMargin.TabIndex = 7;
            this.hScrollBar_CandlestickMargin.Value = 1;
            this.hScrollBar_CandlestickMargin.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_CandlestickMargin_Scroll);
            // 
            // comboBox_UpWaves
            // 
            this.comboBox_UpWaves.FormattingEnabled = true;
            this.comboBox_UpWaves.Location = new System.Drawing.Point(1048, 586);
            this.comboBox_UpWaves.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_UpWaves.Name = "comboBox_UpWaves";
            this.comboBox_UpWaves.Size = new System.Drawing.Size(189, 24);
            this.comboBox_UpWaves.TabIndex = 8;
            // 
            // comboBox_DownWaves
            // 
            this.comboBox_DownWaves.FormattingEnabled = true;
            this.comboBox_DownWaves.Location = new System.Drawing.Point(1360, 586);
            this.comboBox_DownWaves.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_DownWaves.Name = "comboBox_DownWaves";
            this.comboBox_DownWaves.Size = new System.Drawing.Size(180, 24);
            this.comboBox_DownWaves.TabIndex = 9;
            // 
            // label_UpWave
            // 
            this.label_UpWave.AutoSize = true;
            this.label_UpWave.Location = new System.Drawing.Point(962, 589);
            this.label_UpWave.Name = "label_UpWave";
            this.label_UpWave.Size = new System.Drawing.Size(70, 16);
            this.label_UpWave.TabIndex = 10;
            this.label_UpWave.Text = "Up waves:";
            // 
            // label_DownWaves
            // 
            this.label_DownWaves.AutoSize = true;
            this.label_DownWaves.Location = new System.Drawing.Point(1272, 589);
            this.label_DownWaves.Name = "label_DownWaves";
            this.label_DownWaves.Size = new System.Drawing.Size(82, 16);
            this.label_DownWaves.TabIndex = 11;
            this.label_DownWaves.Text = "Down wave: ";
            // 
            // label_selectMargin
            // 
            this.label_selectMargin.AutoSize = true;
            this.label_selectMargin.Location = new System.Drawing.Point(23, 549);
            this.label_selectMargin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_selectMargin.Name = "label_selectMargin";
            this.label_selectMargin.Size = new System.Drawing.Size(92, 16);
            this.label_selectMargin.TabIndex = 12;
            this.label_selectMargin.Text = "Select Margin:";
            // 
            // Timer_Simulate
            // 
            this.Timer_Simulate.Tick += new System.EventHandler(this.Timer_Simulate_Tick);
            // 
            // label_ConfirmationCount
            // 
            this.label_ConfirmationCount.AutoSize = true;
            this.label_ConfirmationCount.Location = new System.Drawing.Point(601, 660);
            this.label_ConfirmationCount.Name = "label_ConfirmationCount";
            this.label_ConfirmationCount.Size = new System.Drawing.Size(91, 16);
            this.label_ConfirmationCount.TabIndex = 13;
            this.label_ConfirmationCount.Text = "Confirmations:";
            // 
            // button_Simulate
            // 
            this.button_Simulate.Location = new System.Drawing.Point(1206, 660);
            this.button_Simulate.Name = "button_Simulate";
            this.button_Simulate.Size = new System.Drawing.Size(75, 23);
            this.button_Simulate.TabIndex = 14;
            this.button_Simulate.Text = "Start";
            this.button_Simulate.UseVisualStyleBackColor = true;
            this.button_Simulate.Click += new System.EventHandler(this.button_Simulate_Click);
            // 
            // button_Plus
            // 
            this.button_Plus.Location = new System.Drawing.Point(1075, 659);
            this.button_Plus.Name = "button_Plus";
            this.button_Plus.Size = new System.Drawing.Size(75, 23);
            this.button_Plus.TabIndex = 15;
            this.button_Plus.Text = "+";
            this.button_Plus.UseVisualStyleBackColor = true;
            this.button_Plus.Click += new System.EventHandler(this.button_Plus_Click);
            // 
            // button_Minus
            // 
            this.button_Minus.Location = new System.Drawing.Point(1357, 660);
            this.button_Minus.Name = "button_Minus";
            this.button_Minus.Size = new System.Drawing.Size(75, 23);
            this.button_Minus.TabIndex = 16;
            this.button_Minus.Text = "-";
            this.button_Minus.UseVisualStyleBackColor = true;
            this.button_Minus.Click += new System.EventHandler(this.button_Minus_Click);
            // 
            //// trackBar_MaxPrice
            //// 
            //this.trackBar_MaxPrice.Location = new System.Drawing.Point(0, 0);
            //this.trackBar_MaxPrice.Name = "trackBar_MaxPrice";
            //this.trackBar_MaxPrice.Size = new System.Drawing.Size(104, 56);
            //this.trackBar_MaxPrice.TabIndex = 2;
            //// 
            //// numericUpDown_stepSize
            //// 
            //this.numericUpDown_stepSize.Location = new System.Drawing.Point(0, 0);
            //this.numericUpDown_stepSize.Name = "numericUpDown_stepSize";
            //this.numericUpDown_stepSize.Size = new System.Drawing.Size(120, 22);
            //this.numericUpDown_stepSize.TabIndex = 1;
            //// 
            //// trackBar_MinPrice
            //// 
            //this.trackBar_MinPrice.Location = new System.Drawing.Point(0, 37);
            //this.trackBar_MinPrice.Name = "trackBar_MinPrice";
            //this.trackBar_MinPrice.Size = new System.Drawing.Size(104, 56);
            //this.trackBar_MinPrice.TabIndex = 0;
            // 
            // candlestickBindingSource
            // 
            this.candlestickBindingSource.DataSource = typeof(Project_2.Candlestick);
            // 
            // Form_ChartDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1550, 753);
            //this.Controls.Add(this.trackBar_MinPrice);
            //this.Controls.Add(this.numericUpDown_stepSize);
            //this.Controls.Add(this.trackBar_MaxPrice);
            this.Controls.Add(this.button_Minus);
            this.Controls.Add(this.button_Plus);
            this.Controls.Add(this.button_Simulate);
            this.Controls.Add(this.label_ConfirmationCount);
            this.Controls.Add(this.label_selectMargin);
            this.Controls.Add(this.label_DownWaves);
            this.Controls.Add(this.label_UpWave);
            this.Controls.Add(this.comboBox_DownWaves);
            this.Controls.Add(this.comboBox_UpWaves);
            this.Controls.Add(this.hScrollBar_CandlestickMargin);
            this.Controls.Add(this.label_EndDate);
            this.Controls.Add(this.label_StartDate);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Controls.Add(this.button_Refresh);
            this.Controls.Add(this.chart_OHLCV);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form_ChartDisplay";
            this.Text = "Form_ChartDisplay";
            ((System.ComponentModel.ISupportInitialize)(this.chart_OHLCV)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.trackBar_MaxPrice)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.numericUpDown_stepSize)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.trackBar_MinPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_OHLCV;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.Label label_StartDate;
        private System.Windows.Forms.Label label_EndDate;
        private System.Windows.Forms.BindingSource candlestickBindingSource;
        private System.Windows.Forms.HScrollBar hScrollBar_CandlestickMargin;
        private System.Windows.Forms.ComboBox comboBox_UpWaves;
        private System.Windows.Forms.ComboBox comboBox_DownWaves;
        private System.Windows.Forms.Label label_UpWave;
        private System.Windows.Forms.Label label_DownWaves;
        private System.Windows.Forms.Label label_selectMargin;
        private System.Windows.Forms.Timer Timer_Simulate;
        private System.Windows.Forms.Label label_ConfirmationCount;
        private System.Windows.Forms.Button button_Simulate;
        private System.Windows.Forms.Button button_Plus;
        private System.Windows.Forms.Button button_Minus;
        //private System.Windows.Forms.TrackBar trackBar_MaxPrice;
       //private System.Windows.Forms.NumericUpDown numericUpDown_stepSize;
        //private System.Windows.Forms.TrackBar trackBar_MinPrice;
    }
}