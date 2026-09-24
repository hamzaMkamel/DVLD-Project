namespace DVLD_Project.Applications.Local_Driving_Applications
{
    partial class ShowLocalDrivingLicenseApplicationInfoForm
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
            this.showDrivingLicenseApplicationInfoControl1 = new DVLD_Project.Tests.Controls.ShowDrivingLicenseApplicationInfoControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // showDrivingLicenseApplicationInfoControl1
            // 
            this.showDrivingLicenseApplicationInfoControl1.Location = new System.Drawing.Point(12, 12);
            this.showDrivingLicenseApplicationInfoControl1.Name = "showDrivingLicenseApplicationInfoControl1";
            this.showDrivingLicenseApplicationInfoControl1.ShowLicenseInfoEnabled = false;
            this.showDrivingLicenseApplicationInfoControl1.Size = new System.Drawing.Size(629, 297);
            this.showDrivingLicenseApplicationInfoControl1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::DVLD_Project.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(536, 315);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 44;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ShowLocalDrivingLicenseApplicationInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(648, 355);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.showDrivingLicenseApplicationInfoControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShowLocalDrivingLicenseApplicationInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Show Local Driving License Application Info :";
            this.ResumeLayout(false);

        }

        #endregion

        private Tests.Controls.ShowDrivingLicenseApplicationInfoControl showDrivingLicenseApplicationInfoControl1;
        private System.Windows.Forms.Button btnClose;
    }
}