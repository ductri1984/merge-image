namespace WinForm
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
            this.btnTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.txtTemplateNew = new System.Windows.Forms.TextBox();
            this.btnTemplateNew = new System.Windows.Forms.Button();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.btnJson = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMovieFolder = new System.Windows.Forms.TextBox();
            this.btnMovieFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(363, 23);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(116, 23);
            this.btnTemplate.TabIndex = 0;
            this.btnTemplate.Text = "Chọn mẫu (*)";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(27, 25);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(330, 20);
            this.txtTemplate.TabIndex = 1;
            // 
            // txtTemplateNew
            // 
            this.txtTemplateNew.Location = new System.Drawing.Point(27, 61);
            this.txtTemplateNew.Name = "txtTemplateNew";
            this.txtTemplateNew.Size = new System.Drawing.Size(330, 20);
            this.txtTemplateNew.TabIndex = 3;
            // 
            // btnTemplateNew
            // 
            this.btnTemplateNew.Location = new System.Drawing.Point(363, 59);
            this.btnTemplateNew.Name = "btnTemplateNew";
            this.btnTemplateNew.Size = new System.Drawing.Size(116, 23);
            this.btnTemplateNew.TabIndex = 2;
            this.btnTemplateNew.Text = "Lưu mẫu mới";
            this.btnTemplateNew.UseVisualStyleBackColor = true;
            this.btnTemplateNew.Click += new System.EventHandler(this.btnTemplateNew_Click);
            // 
            // txtJson
            // 
            this.txtJson.Location = new System.Drawing.Point(27, 135);
            this.txtJson.Name = "txtJson";
            this.txtJson.Size = new System.Drawing.Size(330, 20);
            this.txtJson.TabIndex = 5;
            // 
            // btnJson
            // 
            this.btnJson.Location = new System.Drawing.Point(363, 133);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(116, 23);
            this.btnJson.TabIndex = 4;
            this.btnJson.Text = "Lưu file json";
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(27, 183);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(116, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Đồng ý";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(149, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMovieFolder
            // 
            this.txtMovieFolder.Location = new System.Drawing.Point(27, 97);
            this.txtMovieFolder.Name = "txtMovieFolder";
            this.txtMovieFolder.Size = new System.Drawing.Size(330, 20);
            this.txtMovieFolder.TabIndex = 9;
            // 
            // btnMovieFolder
            // 
            this.btnMovieFolder.Location = new System.Drawing.Point(363, 95);
            this.btnMovieFolder.Name = "btnMovieFolder";
            this.btnMovieFolder.Size = new System.Drawing.Size(116, 23);
            this.btnMovieFolder.TabIndex = 8;
            this.btnMovieFolder.Text = "Thư mục phim";
            this.btnMovieFolder.UseVisualStyleBackColor = true;
            this.btnMovieFolder.Click += new System.EventHandler(this.btnMovieFolder_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(519, 232);
            this.Controls.Add(this.txtMovieFolder);
            this.Controls.Add(this.btnMovieFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtJson);
            this.Controls.Add(this.btnJson);
            this.Controls.Add(this.txtTemplateNew);
            this.Controls.Add(this.btnTemplateNew);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.btnTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.TextBox txtTemplateNew;
        private System.Windows.Forms.Button btnTemplateNew;
        private System.Windows.Forms.TextBox txtJson;
        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMovieFolder;
        private System.Windows.Forms.Button btnMovieFolder;
    }
}

