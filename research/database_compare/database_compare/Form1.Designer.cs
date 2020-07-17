namespace database_compare
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
            this.label5 = new System.Windows.Forms.Label();
            this.txtTargetPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTargetUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTargetDatabase = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTargetServer = new System.Windows.Forms.TextBox();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gvTarget = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSourcePassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSourceUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSourceDatabase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gvSource = new System.Windows.Forms.DataGridView();
            this.txtSourceServer = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenScript = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarget)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Password";
            // 
            // txtTargetPassword
            // 
            this.txtTargetPassword.Location = new System.Drawing.Point(263, 73);
            this.txtTargetPassword.Name = "txtTargetPassword";
            this.txtTargetPassword.PasswordChar = '*';
            this.txtTargetPassword.Size = new System.Drawing.Size(97, 20);
            this.txtTargetPassword.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "User";
            // 
            // txtTargetUser
            // 
            this.txtTargetUser.Location = new System.Drawing.Point(74, 70);
            this.txtTargetUser.Name = "txtTargetUser";
            this.txtTargetUser.Size = new System.Drawing.Size(97, 20);
            this.txtTargetUser.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Database";
            // 
            // txtTargetDatabase
            // 
            this.txtTargetDatabase.Location = new System.Drawing.Point(74, 44);
            this.txtTargetDatabase.Name = "txtTargetDatabase";
            this.txtTargetDatabase.Size = new System.Drawing.Size(286, 20);
            this.txtTargetDatabase.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Server";
            // 
            // txtTargetServer
            // 
            this.txtTargetServer.Location = new System.Drawing.Point(74, 18);
            this.txtTargetServer.Name = "txtTargetServer";
            this.txtTargetServer.Size = new System.Drawing.Size(286, 20);
            this.txtTargetServer.TabIndex = 0;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "Foreign";
            this.Column10.HeaderText = "Foreign";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "Columns";
            this.Column9.HeaderText = "Columns";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 60;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "Note";
            this.Column8.HeaderText = "Note";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "TableName";
            this.Column7.HeaderText = "Name";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "IsSuccess";
            this.Column6.HeaderText = "Success";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column6.Width = 60;
            // 
            // gvTarget
            // 
            this.gvTarget.AllowUserToAddRows = false;
            this.gvTarget.AllowUserToDeleteRows = false;
            this.gvTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.gvTarget.Location = new System.Drawing.Point(17, 109);
            this.gvTarget.Name = "gvTarget";
            this.gvTarget.ReadOnly = true;
            this.gvTarget.Size = new System.Drawing.Size(343, 284);
            this.gvTarget.TabIndex = 8;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(10, 17);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvTarget);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTargetPassword);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtTargetUser);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtTargetDatabase);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtTargetServer);
            this.groupBox2.Location = new System.Drawing.Point(399, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 408);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password";
            // 
            // txtSourcePassword
            // 
            this.txtSourcePassword.Location = new System.Drawing.Point(263, 73);
            this.txtSourcePassword.Name = "txtSourcePassword";
            this.txtSourcePassword.PasswordChar = '*';
            this.txtSourcePassword.Size = new System.Drawing.Size(97, 20);
            this.txtSourcePassword.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User";
            // 
            // txtSourceUser
            // 
            this.txtSourceUser.Location = new System.Drawing.Point(74, 70);
            this.txtSourceUser.Name = "txtSourceUser";
            this.txtSourceUser.Size = new System.Drawing.Size(97, 20);
            this.txtSourceUser.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database";
            // 
            // txtSourceDatabase
            // 
            this.txtSourceDatabase.Location = new System.Drawing.Point(74, 44);
            this.txtSourceDatabase.Name = "txtSourceDatabase";
            this.txtSourceDatabase.Size = new System.Drawing.Size(286, 20);
            this.txtSourceDatabase.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Foreign";
            this.Column5.HeaderText = "Foreign";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Columns";
            this.Column4.HeaderText = "Columns";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 60;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Note";
            this.Column3.HeaderText = "Note";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "TableName";
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IsSuccess";
            this.Column1.HeaderText = "Success";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 60;
            // 
            // gvSource
            // 
            this.gvSource.AllowUserToAddRows = false;
            this.gvSource.AllowUserToDeleteRows = false;
            this.gvSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.gvSource.Location = new System.Drawing.Point(17, 109);
            this.gvSource.Name = "gvSource";
            this.gvSource.ReadOnly = true;
            this.gvSource.Size = new System.Drawing.Size(343, 284);
            this.gvSource.TabIndex = 8;
            // 
            // txtSourceServer
            // 
            this.txtSourceServer.Location = new System.Drawing.Point(74, 18);
            this.txtSourceServer.Name = "txtSourceServer";
            this.txtSourceServer.Size = new System.Drawing.Size(286, 20);
            this.txtSourceServer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvSource);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSourcePassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSourceUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSourceDatabase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSourceServer);
            this.groupBox1.Location = new System.Drawing.Point(10, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 408);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // btnGenScript
            // 
            this.btnGenScript.Location = new System.Drawing.Point(91, 17);
            this.btnGenScript.Name = "btnGenScript";
            this.btnGenScript.Size = new System.Drawing.Size(75, 23);
            this.btnGenScript.TabIndex = 8;
            this.btnGenScript.Text = "Gen script";
            this.btnGenScript.UseVisualStyleBackColor = true;
            this.btnGenScript.Click += new System.EventHandler(this.btnGenScript_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 473);
            this.Controls.Add(this.btnGenScript);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvTarget)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTargetPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTargetUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTargetDatabase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTargetServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridView gvTarget;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSourcePassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSourceUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSourceDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridView gvSource;
        private System.Windows.Forms.TextBox txtSourceServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenScript;
    }
}

