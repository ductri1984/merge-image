namespace FormsBackground
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
            this.txtChooseJson = new System.Windows.Forms.TextBox();
            this.btnChooseJson = new System.Windows.Forms.Button();
            this.btnChooseLog = new System.Windows.Forms.Button();
            this.txtChooseLog = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkIsLog = new System.Windows.Forms.CheckBox();
            this.chkIsStop = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnLoadLog = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtChooseJson
            // 
            this.txtChooseJson.Location = new System.Drawing.Point(23, 20);
            this.txtChooseJson.Name = "txtChooseJson";
            this.txtChooseJson.Size = new System.Drawing.Size(403, 20);
            this.txtChooseJson.TabIndex = 1;
            // 
            // btnChooseJson
            // 
            this.btnChooseJson.Location = new System.Drawing.Point(432, 18);
            this.btnChooseJson.Name = "btnChooseJson";
            this.btnChooseJson.Size = new System.Drawing.Size(109, 23);
            this.btnChooseJson.TabIndex = 2;
            this.btnChooseJson.Text = "Choose json";
            this.btnChooseJson.UseVisualStyleBackColor = true;
            this.btnChooseJson.Click += new System.EventHandler(this.btnChooseJson_Click);
            // 
            // btnChooseLog
            // 
            this.btnChooseLog.Location = new System.Drawing.Point(432, 53);
            this.btnChooseLog.Name = "btnChooseLog";
            this.btnChooseLog.Size = new System.Drawing.Size(109, 23);
            this.btnChooseLog.TabIndex = 4;
            this.btnChooseLog.Text = "Choose log";
            this.btnChooseLog.UseVisualStyleBackColor = true;
            this.btnChooseLog.Click += new System.EventHandler(this.btnChooseLog_Click);
            // 
            // txtChooseLog
            // 
            this.txtChooseLog.Location = new System.Drawing.Point(23, 55);
            this.txtChooseLog.Name = "txtChooseLog";
            this.txtChooseLog.Size = new System.Drawing.Size(403, 20);
            this.txtChooseLog.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 90);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(518, 105);
            this.dataGridView1.TabIndex = 5;
            // 
            // chkIsLog
            // 
            this.chkIsLog.AutoSize = true;
            this.chkIsLog.Location = new System.Drawing.Point(23, 201);
            this.chkIsLog.Name = "chkIsLog";
            this.chkIsLog.Size = new System.Drawing.Size(52, 17);
            this.chkIsLog.TabIndex = 6;
            this.chkIsLog.Text = "IsLog";
            this.chkIsLog.UseVisualStyleBackColor = true;
            // 
            // chkIsStop
            // 
            this.chkIsStop.AutoSize = true;
            this.chkIsStop.Location = new System.Drawing.Point(81, 201);
            this.chkIsStop.Name = "chkIsStop";
            this.chkIsStop.Size = new System.Drawing.Size(56, 17);
            this.chkIsStop.TabIndex = 7;
            this.chkIsStop.Text = "IsStop";
            this.chkIsStop.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(403, 201);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(331, 201);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(66, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(187, 201);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(66, 23);
            this.btnLoad.TabIndex = 10;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(23, 230);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(518, 214);
            this.txtLog.TabIndex = 11;
            // 
            // btnLoadLog
            // 
            this.btnLoadLog.Location = new System.Drawing.Point(331, 450);
            this.btnLoadLog.Name = "btnLoadLog";
            this.btnLoadLog.Size = new System.Drawing.Size(66, 23);
            this.btnLoadLog.TabIndex = 13;
            this.btnLoadLog.Text = "Load";
            this.btnLoadLog.UseVisualStyleBackColor = true;
            this.btnLoadLog.Click += new System.EventHandler(this.btnLoadLog_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(403, 450);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(66, 23);
            this.btnClearLog.TabIndex = 12;
            this.btnClearLog.Text = "Clear";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(475, 201);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(66, 23);
            this.btnRun.TabIndex = 14;
            this.btnRun.Text = "Run test";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(259, 201);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(66, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(475, 450);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(563, 485);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnLoadLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkIsStop);
            this.Controls.Add(this.chkIsLog);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnChooseLog);
            this.Controls.Add(this.txtChooseLog);
            this.Controls.Add(this.btnChooseJson);
            this.Controls.Add(this.txtChooseJson);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChooseJson;
        private System.Windows.Forms.Button btnChooseJson;
        private System.Windows.Forms.Button btnChooseLog;
        private System.Windows.Forms.TextBox txtChooseLog;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkIsLog;
        private System.Windows.Forms.CheckBox chkIsStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnLoadLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
    }
}

