
namespace JucemgScrapping
{
    partial class Main
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
            this.currentDate = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.currentCompanies = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.totalCompanies = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // currentDate
            // 
            this.currentDate.Location = new System.Drawing.Point(107, 62);
            this.currentDate.Name = "currentDate";
            this.currentDate.Size = new System.Drawing.Size(204, 20);
            this.currentDate.TabIndex = 0;
            this.currentDate.Value = new System.DateTime(2021, 6, 26, 0, 0, 0, 0);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(107, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Extrair";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(107, 201);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(182, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // currentCompanies
            // 
            this.currentCompanies.AutoSize = true;
            this.currentCompanies.Location = new System.Drawing.Point(296, 211);
            this.currentCompanies.Name = "currentCompanies";
            this.currentCompanies.Size = new System.Drawing.Size(13, 13);
            this.currentCompanies.TabIndex = 3;
            this.currentCompanies.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "/";
            // 
            // totalCompanies
            // 
            this.totalCompanies.AutoSize = true;
            this.totalCompanies.Location = new System.Drawing.Point(317, 211);
            this.totalCompanies.Name = "totalCompanies";
            this.totalCompanies.Size = new System.Drawing.Size(13, 13);
            this.totalCompanies.TabIndex = 5;
            this.totalCompanies.Text = "0";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 250);
            this.Controls.Add(this.totalCompanies);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentCompanies);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.currentDate);
            this.Name = "Main";
            this.Text = "Extração Dados JUCEMG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker currentDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label currentCompanies;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label totalCompanies;
    }
}

