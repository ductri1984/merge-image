namespace winform
{
    partial class frmSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileData = new System.Windows.Forms.TextBox();
            this.btnFileData = new System.Windows.Forms.Button();
            this.btnFolderLibrary = new System.Windows.Forms.Button();
            this.txtFolderBackground = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFolderPointer = new System.Windows.Forms.Button();
            this.txtFolderPointer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File data";
            // 
            // txtFileData
            // 
            this.txtFileData.Location = new System.Drawing.Point(131, 22);
            this.txtFileData.Name = "txtFileData";
            this.txtFileData.Size = new System.Drawing.Size(304, 20);
            this.txtFileData.TabIndex = 1;
            // 
            // btnFileData
            // 
            this.btnFileData.Location = new System.Drawing.Point(441, 21);
            this.btnFileData.Name = "btnFileData";
            this.btnFileData.Size = new System.Drawing.Size(39, 23);
            this.btnFileData.TabIndex = 2;
            this.btnFileData.Text = "...";
            this.btnFileData.UseVisualStyleBackColor = true;
            this.btnFileData.Click += new System.EventHandler(this.btnFileData_Click);
            // 
            // btnFolderLibrary
            // 
            this.btnFolderLibrary.Location = new System.Drawing.Point(441, 62);
            this.btnFolderLibrary.Name = "btnFolderLibrary";
            this.btnFolderLibrary.Size = new System.Drawing.Size(39, 23);
            this.btnFolderLibrary.TabIndex = 5;
            this.btnFolderLibrary.Text = "...";
            this.btnFolderLibrary.UseVisualStyleBackColor = true;
            this.btnFolderLibrary.Click += new System.EventHandler(this.btnFolderLibrary_Click);
            // 
            // txtFolderBackground
            // 
            this.txtFolderBackground.Location = new System.Drawing.Point(131, 63);
            this.txtFolderBackground.Name = "txtFolderBackground";
            this.txtFolderBackground.Size = new System.Drawing.Size(304, 20);
            this.txtFolderBackground.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Folder background";
            // 
            // btnFolderPointer
            // 
            this.btnFolderPointer.Location = new System.Drawing.Point(441, 104);
            this.btnFolderPointer.Name = "btnFolderPointer";
            this.btnFolderPointer.Size = new System.Drawing.Size(39, 23);
            this.btnFolderPointer.TabIndex = 8;
            this.btnFolderPointer.Text = "...";
            this.btnFolderPointer.UseVisualStyleBackColor = true;
            this.btnFolderPointer.Click += new System.EventHandler(this.btnFolderPointer_Click);
            // 
            // txtFolderPointer
            // 
            this.txtFolderPointer.Location = new System.Drawing.Point(131, 105);
            this.txtFolderPointer.Name = "txtFolderPointer";
            this.txtFolderPointer.Size = new System.Drawing.Size(304, 20);
            this.txtFolderPointer.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Folder pointer";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(131, 145);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save changes";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 190);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFolderPointer);
            this.Controls.Add(this.txtFolderPointer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFolderLibrary);
            this.Controls.Add(this.txtFolderBackground);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFileData);
            this.Controls.Add(this.txtFileData);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileData;
        private System.Windows.Forms.Button btnFileData;
        private System.Windows.Forms.Button btnFolderLibrary;
        private System.Windows.Forms.TextBox txtFolderBackground;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFolderPointer;
        private System.Windows.Forms.TextBox txtFolderPointer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
    }
}