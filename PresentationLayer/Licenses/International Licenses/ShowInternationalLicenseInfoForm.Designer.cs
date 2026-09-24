namespace DVLD_Project.Licenses.International_Licenses
{
    partial class ShowInternationalLicenseInfoForm
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
            this.pictureBoxPhoto = new System.Windows.Forms.PictureBox();
            this.labelMode = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.showInternationalDriverLicenseInfoControl1 = new DVLD_Project.Licenses.International_Licenses.Controls.ShowInternationalDriverLicenseInfoControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxPhoto
            // 
            this.pictureBoxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPhoto.Image = global::DVLD_Project.Properties.Resources.LicenseView_400;
            this.pictureBoxPhoto.InitialImage = null;
            this.pictureBoxPhoto.Location = new System.Drawing.Point(316, 12);
            this.pictureBoxPhoto.Name = "pictureBoxPhoto";
            this.pictureBoxPhoto.Size = new System.Drawing.Size(96, 83);
            this.pictureBoxPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPhoto.TabIndex = 39;
            this.pictureBoxPhoto.TabStop = false;
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMode.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelMode.Location = new System.Drawing.Point(209, 98);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(320, 30);
            this.labelMode.TabIndex = 41;
            this.labelMode.Text = "Driver International License Info :";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::DVLD_Project.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(638, 435);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 29);
            this.btnClose.TabIndex = 40;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // showInternationalDriverLicenseInfoControl1
            // 
            this.showInternationalDriverLicenseInfoControl1.Location = new System.Drawing.Point(12, 129);
            this.showInternationalDriverLicenseInfoControl1.Name = "showInternationalDriverLicenseInfoControl1";
            this.showInternationalDriverLicenseInfoControl1.Size = new System.Drawing.Size(741, 300);
            this.showInternationalDriverLicenseInfoControl1.TabIndex = 42;
            // 
            // ShowInternationalLicenseInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(751, 476);
            this.Controls.Add(this.pictureBoxPhoto);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.showInternationalDriverLicenseInfoControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShowInternationalLicenseInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show International License Info :";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxPhoto;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.Button btnClose;
        private Controls.ShowInternationalDriverLicenseInfoControl showInternationalDriverLicenseInfoControl1;
    }
}