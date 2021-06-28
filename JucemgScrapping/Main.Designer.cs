
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.currentCompanies = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.totalCompanies = new System.Windows.Forms.Label();
            this.currentOperation = new System.Windows.Forms.Label();
            this.exportBtn = new System.Windows.Forms.Button();
            this.exportPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // currentDate
            // 
            this.currentDate.Location = new System.Drawing.Point(12, 29);
            this.currentDate.Name = "currentDate";
            this.currentDate.Size = new System.Drawing.Size(231, 20);
            this.currentDate.TabIndex = 0;
            this.currentDate.Value = new System.DateTime(2021, 6, 26, 0, 0, 0, 0);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Extrair";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 178);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(182, 23);
            this.progressBar.TabIndex = 2;
            // 
            // currentCompanies
            // 
            this.currentCompanies.AutoSize = true;
            this.currentCompanies.Location = new System.Drawing.Point(202, 183);
            this.currentCompanies.Name = "currentCompanies";
            this.currentCompanies.Size = new System.Drawing.Size(13, 13);
            this.currentCompanies.TabIndex = 3;
            this.currentCompanies.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "/";
            // 
            // totalCompanies
            // 
            this.totalCompanies.AutoSize = true;
            this.totalCompanies.Location = new System.Drawing.Point(243, 183);
            this.totalCompanies.Name = "totalCompanies";
            this.totalCompanies.Size = new System.Drawing.Size(13, 13);
            this.totalCompanies.TabIndex = 5;
            this.totalCompanies.Text = "0";
            // 
            // currentOperation
            // 
            this.currentOperation.AutoSize = true;
            this.currentOperation.Location = new System.Drawing.Point(12, 73);
            this.currentOperation.Name = "currentOperation";
            this.currentOperation.Size = new System.Drawing.Size(0, 13);
            this.currentOperation.TabIndex = 6;
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(12, 210);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(104, 30);
            this.exportBtn.TabIndex = 7;
            this.exportBtn.Text = "Alterar Caminho";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // exportPath
            // 
            this.exportPath.AutoSize = true;
            this.exportPath.Location = new System.Drawing.Point(12, 265);
            this.exportPath.Name = "exportPath";
            this.exportPath.Size = new System.Drawing.Size(78, 13);
            this.exportPath.TabIndex = 8;
            this.exportPath.Text = "C:\\Users\\2233";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 290);
            this.Controls.Add(this.exportPath);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.currentOperation);
            this.Controls.Add(this.totalCompanies);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentCompanies);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.currentDate);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Extração Dados JUCEMG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker currentDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label currentCompanies;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label totalCompanies;
        private System.Windows.Forms.Label currentOperation;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.Label exportPath;
    }
}

