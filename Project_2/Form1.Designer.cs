namespace Project_2
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
            this.button_LoadStock = new System.Windows.Forms.Button();
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.label_StartingDate = new System.Windows.Forms.Label();
            this.label_EndingDate = new System.Windows.Forms.Label();
            this.openFileDialog_LoadTicker = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button_LoadStock
            // 
            this.button_LoadStock.Location = new System.Drawing.Point(486, 90);
            this.button_LoadStock.Name = "button_LoadStock";
            this.button_LoadStock.Size = new System.Drawing.Size(103, 23);
            this.button_LoadStock.TabIndex = 0;
            this.button_LoadStock.Text = "Load Stock";
            this.button_LoadStock.UseVisualStyleBackColor = true;
            this.button_LoadStock.Click += new System.EventHandler(this.button_LoadStock_Click);
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(159, 90);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(269, 22);
            this.dateTimePicker_StartDate.TabIndex = 2;
            this.dateTimePicker_StartDate.Value = new System.DateTime(2021, 2, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(634, 91);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(271, 22);
            this.dateTimePicker_EndDate.TabIndex = 3;
            this.dateTimePicker_EndDate.Value = new System.DateTime(2021, 2, 28, 0, 0, 0, 0);
            // 
            // label_StartingDate
            // 
            this.label_StartingDate.AutoSize = true;
            this.label_StartingDate.Location = new System.Drawing.Point(252, 52);
            this.label_StartingDate.Name = "label_StartingDate";
            this.label_StartingDate.Size = new System.Drawing.Size(69, 16);
            this.label_StartingDate.TabIndex = 4;
            this.label_StartingDate.Text = "Start Date:";
            // 
            // label_EndingDate
            // 
            this.label_EndingDate.AutoSize = true;
            this.label_EndingDate.Location = new System.Drawing.Point(694, 52);
            this.label_EndingDate.Name = "label_EndingDate";
            this.label_EndingDate.Size = new System.Drawing.Size(66, 16);
            this.label_EndingDate.TabIndex = 5;
            this.label_EndingDate.Text = "End Date:";
            // 
            // openFileDialog_LoadTicker
            // 
            this.openFileDialog_LoadTicker.DefaultExt = "CSV";
            this.openFileDialog_LoadTicker.FileName = "ABBV-Day.csv";
            this.openFileDialog_LoadTicker.Filter = "All|*.csv|Month|*-Month.csv|Week|*-Week.csv|Day|*-Day.csv|Start with A|A*.csv";
            this.openFileDialog_LoadTicker.FilterIndex = 5;
            this.openFileDialog_LoadTicker.InitialDirectory = "C:\\Users\\solom\\OneDrive\\Desktop\\Project 1\\Stock Data";
            this.openFileDialog_LoadTicker.Multiselect = true;
            this.openFileDialog_LoadTicker.ReadOnlyChecked = true;
            this.openFileDialog_LoadTicker.ShowReadOnly = true;
            this.openFileDialog_LoadTicker.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 185);
            this.Controls.Add(this.label_EndingDate);
            this.Controls.Add(this.label_StartingDate);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Controls.Add(this.button_LoadStock);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LoadStock;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.Label label_StartingDate;
        private System.Windows.Forms.Label label_EndingDate;
        private System.Windows.Forms.OpenFileDialog openFileDialog_LoadTicker;
    }
}

