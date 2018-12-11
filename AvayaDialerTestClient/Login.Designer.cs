namespace AvayaDialerTestClient
{
  partial class Login
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
      this.lblLine = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.txtUserId = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblUserId = new System.Windows.Forms.Label();
      this.lblPassword = new System.Windows.Forms.Label();
      this.lblExtension = new System.Windows.Forms.Label();
      this.txtExtension = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // lblLine
      // 
      this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblLine.Location = new System.Drawing.Point(6, 95);
      this.lblLine.Name = "lblLine";
      this.lblLine.Size = new System.Drawing.Size(180, 2);
      this.lblLine.TabIndex = 6;
      this.lblLine.Text = "Label4";
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(109, 106);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(29, 106);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new System.EventHandler(this.OK_Click);
      // 
      // txtUserId
      // 
      this.txtUserId.Location = new System.Drawing.Point(83, 8);
      this.txtUserId.MaxLength = 255;
      this.txtUserId.Name = "txtUserId";
      this.txtUserId.Size = new System.Drawing.Size(100, 21);
      this.txtUserId.TabIndex = 1;
      this.txtUserId.TextChanged += new System.EventHandler(this.UserId_TextChanged);
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(83, 36);
      this.txtPassword.MaxLength = 255;
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new System.Drawing.Size(100, 21);
      this.txtPassword.TabIndex = 3;
      this.txtPassword.TextChanged += new System.EventHandler(this.Password_TextChanged);
      // 
      // lblUserId
      // 
      this.lblUserId.Location = new System.Drawing.Point(5, 7);
      this.lblUserId.Name = "lblUserId";
      this.lblUserId.Size = new System.Drawing.Size(72, 20);
      this.lblUserId.TabIndex = 0;
      this.lblUserId.Text = "User ID:";
      this.lblUserId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblPassword
      // 
      this.lblPassword.Location = new System.Drawing.Point(5, 35);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(72, 20);
      this.lblPassword.TabIndex = 2;
      this.lblPassword.Text = "Password:";
      this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblExtension
      // 
      this.lblExtension.Location = new System.Drawing.Point(5, 63);
      this.lblExtension.Name = "lblExtension";
      this.lblExtension.Size = new System.Drawing.Size(72, 20);
      this.lblExtension.TabIndex = 4;
      this.lblExtension.Text = "Extension:";
      this.lblExtension.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtExtension
      // 
      this.txtExtension.Location = new System.Drawing.Point(83, 65);
      this.txtExtension.MaxLength = 255;
      this.txtExtension.Name = "txtExtension";
      this.txtExtension.Size = new System.Drawing.Size(100, 21);
      this.txtExtension.TabIndex = 5;
      this.txtExtension.TextChanged += new System.EventHandler(this.Extension_TextChanged);
      // 
      // Login
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(197, 139);
      this.Controls.Add(this.txtExtension);
      this.Controls.Add(this.lblLine);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.txtUserId);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.lblUserId);
      this.Controls.Add(this.lblPassword);
      this.Controls.Add(this.lblExtension);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Login";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Login";
      this.Activated += new System.EventHandler(this.Login_Activated);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblLine;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.TextBox txtUserId;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblUserId;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblExtension;
    private System.Windows.Forms.TextBox txtExtension;
  }
}