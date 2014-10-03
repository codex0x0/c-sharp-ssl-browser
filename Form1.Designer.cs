namespace testSSLPrj
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
            this.components = new System.ComponentModel.Container();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtboxURL = new System.Windows.Forms.TextBox();
            this.btnSSLVisit = new System.Windows.Forms.Button();
            this.btnRegHTTP = new System.Windows.Forms.Button();
            this.chkBoxWriteResults = new System.Windows.Forms.CheckBox();
            this.chkMsgBox = new System.Windows.Forms.CheckBox();
            this.toolURLTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblTip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(38, 152);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(66, 13);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "Target URL:";
            // 
            // txtboxURL
            // 
            this.txtboxURL.Location = new System.Drawing.Point(110, 149);
            this.txtboxURL.Name = "txtboxURL";
            this.txtboxURL.Size = new System.Drawing.Size(136, 20);
            this.txtboxURL.TabIndex = 1;
            this.txtboxURL.Text = "www.meetme.com";
            // 
            // btnSSLVisit
            // 
            this.btnSSLVisit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSSLVisit.Location = new System.Drawing.Point(110, 175);
            this.btnSSLVisit.Name = "btnSSLVisit";
            this.btnSSLVisit.Size = new System.Drawing.Size(136, 21);
            this.btnSSLVisit.TabIndex = 2;
            this.btnSSLVisit.Text = "Visit via SSL";
            this.btnSSLVisit.UseVisualStyleBackColor = true;
            this.btnSSLVisit.Click += new System.EventHandler(this.btnSSLVisit_Click);
            // 
            // btnRegHTTP
            // 
            this.btnRegHTTP.Location = new System.Drawing.Point(110, 202);
            this.btnRegHTTP.Name = "btnRegHTTP";
            this.btnRegHTTP.Size = new System.Drawing.Size(136, 23);
            this.btnRegHTTP.TabIndex = 3;
            this.btnRegHTTP.Text = "visit via Regular HTTP";
            this.btnRegHTTP.UseVisualStyleBackColor = true;
            this.btnRegHTTP.Click += new System.EventHandler(this.btnRegHTTP_Click);
            // 
            // chkBoxWriteResults
            // 
            this.chkBoxWriteResults.AutoSize = true;
            this.chkBoxWriteResults.Location = new System.Drawing.Point(110, 39);
            this.chkBoxWriteResults.Name = "chkBoxWriteResults";
            this.chkBoxWriteResults.Size = new System.Drawing.Size(157, 17);
            this.chkBoxWriteResults.TabIndex = 4;
            this.chkBoxWriteResults.Text = "Write Results to HTML FIle.";
            this.chkBoxWriteResults.UseVisualStyleBackColor = true;
            // 
            // chkMsgBox
            // 
            this.chkMsgBox.AutoSize = true;
            this.chkMsgBox.Location = new System.Drawing.Point(110, 62);
            this.chkMsgBox.Name = "chkMsgBox";
            this.chkMsgBox.Size = new System.Drawing.Size(141, 17);
            this.chkMsgBox.TabIndex = 5;
            this.chkMsgBox.Text = "MessageBox The Data?";
            this.chkMsgBox.UseVisualStyleBackColor = true;
            // 
            // toolURLTip
            // 
            this.toolURLTip.Tag = "URL must be domain.com or www.domain.com only, no http:// is needed";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(107, 9);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(290, 13);
            this.lblTip.TabIndex = 6;
            this.lblTip.Text = "This will write results to a html file in the application directory.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 243);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.chkMsgBox);
            this.Controls.Add(this.chkBoxWriteResults);
            this.Controls.Add(this.btnRegHTTP);
            this.Controls.Add(this.btnSSLVisit);
            this.Controls.Add(this.txtboxURL);
            this.Controls.Add(this.lblURL);
            this.Name = "Form1";
            this.Text = "SSLDebugging";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtboxURL;
        private System.Windows.Forms.Button btnSSLVisit;
        private System.Windows.Forms.Button btnRegHTTP;
        private System.Windows.Forms.CheckBox chkBoxWriteResults;
        private System.Windows.Forms.CheckBox chkMsgBox;
        private System.Windows.Forms.ToolTip toolURLTip;
        private System.Windows.Forms.Label lblTip;
    }
}

