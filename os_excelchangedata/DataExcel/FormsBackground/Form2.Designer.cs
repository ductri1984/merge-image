namespace FormsBackground
{
    partial class Form2
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
            this.btnFolderUpload = new System.Windows.Forms.Button();
            this.txtFolderUpload = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkIsFile = new System.Windows.Forms.CheckBox();
            this.txtHandlerLink = new System.Windows.Forms.TextBox();
            this.txtHandlerKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLinkPush = new System.Windows.Forms.TextBox();
            this.txtLinkData = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRangeValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCellTitle = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFolderUpload
            // 
            this.btnFolderUpload.Location = new System.Drawing.Point(311, 22);
            this.btnFolderUpload.Name = "btnFolderUpload";
            this.btnFolderUpload.Size = new System.Drawing.Size(109, 23);
            this.btnFolderUpload.TabIndex = 4;
            this.btnFolderUpload.Text = "Folder upload";
            this.btnFolderUpload.UseVisualStyleBackColor = true;
            this.btnFolderUpload.Click += new System.EventHandler(this.btnFolderUpload_Click);
            // 
            // txtFolderUpload
            // 
            this.txtFolderUpload.Location = new System.Drawing.Point(22, 24);
            this.txtFolderUpload.Name = "txtFolderUpload";
            this.txtFolderUpload.Size = new System.Drawing.Size(283, 20);
            this.txtFolderUpload.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(345, 377);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(264, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(183, 377);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkIsFile
            // 
            this.chkIsFile.AutoSize = true;
            this.chkIsFile.Location = new System.Drawing.Point(22, 60);
            this.chkIsFile.Name = "chkIsFile";
            this.chkIsFile.Size = new System.Drawing.Size(50, 17);
            this.chkIsFile.TabIndex = 7;
            this.chkIsFile.Text = "IsFile";
            this.chkIsFile.UseVisualStyleBackColor = true;
            // 
            // txtHandlerLink
            // 
            this.txtHandlerLink.Location = new System.Drawing.Point(6, 50);
            this.txtHandlerLink.Name = "txtHandlerLink";
            this.txtHandlerLink.Size = new System.Drawing.Size(283, 20);
            this.txtHandlerLink.TabIndex = 5;
            // 
            // txtHandlerKey
            // 
            this.txtHandlerKey.Location = new System.Drawing.Point(6, 81);
            this.txtHandlerKey.Name = "txtHandlerKey";
            this.txtHandlerKey.Size = new System.Drawing.Size(283, 20);
            this.txtHandlerKey.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Handler link";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(6, 19);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(283, 20);
            this.txtFileName.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtHandlerKey);
            this.groupBox1.Controls.Add(this.txtHandlerLink);
            this.groupBox1.Location = new System.Drawing.Point(22, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 120);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Handler";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(295, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Handler key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "File name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCellTitle);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtLinkPush);
            this.groupBox2.Controls.Add(this.txtLinkData);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtRangeValue);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(22, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(398, 143);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(295, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Link push";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(295, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Link data";
            // 
            // txtLinkPush
            // 
            this.txtLinkPush.Location = new System.Drawing.Point(6, 112);
            this.txtLinkPush.Name = "txtLinkPush";
            this.txtLinkPush.Size = new System.Drawing.Size(283, 20);
            this.txtLinkPush.TabIndex = 28;
            // 
            // txtLinkData
            // 
            this.txtLinkData.Location = new System.Drawing.Point(6, 81);
            this.txtLinkData.Name = "txtLinkData";
            this.txtLinkData.Size = new System.Drawing.Size(283, 20);
            this.txtLinkData.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Range value";
            // 
            // txtRangeValue
            // 
            this.txtRangeValue.Location = new System.Drawing.Point(6, 19);
            this.txtRangeValue.Name = "txtRangeValue";
            this.txtRangeValue.Size = new System.Drawing.Size(283, 20);
            this.txtRangeValue.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(295, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Cell title";
            // 
            // txtCellTitle
            // 
            this.txtCellTitle.Location = new System.Drawing.Point(6, 50);
            this.txtCellTitle.Name = "txtCellTitle";
            this.txtCellTitle.Size = new System.Drawing.Size(283, 20);
            this.txtCellTitle.TabIndex = 31;
            // 
            // Form2
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(438, 413);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkIsFile);
            this.Controls.Add(this.btnFolderUpload);
            this.Controls.Add(this.txtFolderUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFolderUpload;
        private System.Windows.Forms.TextBox txtFolderUpload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkIsFile;
        private System.Windows.Forms.TextBox txtHandlerLink;
        private System.Windows.Forms.TextBox txtHandlerKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRangeValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtLinkPush;
        private System.Windows.Forms.TextBox txtLinkData;
        private System.Windows.Forms.TextBox txtCellTitle;
    }
}