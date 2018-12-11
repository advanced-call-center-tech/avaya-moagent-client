namespace AvayaDialerTestClient
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.btnInitialize = new System.Windows.Forms.Button();
      this.chkUseSSL = new System.Windows.Forms.CheckBox();
      this.txtPort = new System.Windows.Forms.TextBox();
      this.lblPort = new System.Windows.Forms.Label();
      this.txtIPAddress = new System.Windows.Forms.TextBox();
      this.lblIPAddress = new System.Windows.Forms.Label();
      this.lstMessages = new System.Windows.Forms.ListView();
      this.clmDirection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.clmMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnFinish = new System.Windows.Forms.Button();
      this.avaDialer = new AvayaDialerTestClient.AvayaControl();
      this.SuspendLayout();
      // 
      // btnInitialize
      // 
      this.btnInitialize.Location = new System.Drawing.Point(257, 5);
      this.btnInitialize.Name = "btnInitialize";
      this.btnInitialize.Size = new System.Drawing.Size(109, 71);
      this.btnInitialize.TabIndex = 16;
      this.btnInitialize.Text = "Initialize";
      this.btnInitialize.UseVisualStyleBackColor = true;
      this.btnInitialize.Click += new System.EventHandler(this.Initialize_Click);
      // 
      // chkUseSSL
      // 
      this.chkUseSSL.AutoSize = true;
      this.chkUseSSL.Location = new System.Drawing.Point(81, 59);
      this.chkUseSSL.Name = "chkUseSSL";
      this.chkUseSSL.Size = new System.Drawing.Size(69, 17);
      this.chkUseSSL.TabIndex = 15;
      this.chkUseSSL.Text = "Use SSL?";
      this.chkUseSSL.UseVisualStyleBackColor = true;
      // 
      // txtPort
      // 
      this.txtPort.Location = new System.Drawing.Point(81, 32);
      this.txtPort.Name = "txtPort";
      this.txtPort.Size = new System.Drawing.Size(170, 21);
      this.txtPort.TabIndex = 14;
      this.txtPort.Text = "22700";
      // 
      // lblPort
      // 
      this.lblPort.AutoSize = true;
      this.lblPort.Location = new System.Drawing.Point(44, 36);
      this.lblPort.Name = "lblPort";
      this.lblPort.Size = new System.Drawing.Size(31, 13);
      this.lblPort.TabIndex = 13;
      this.lblPort.Text = "Port:";
      // 
      // txtIPAddress
      // 
      this.txtIPAddress.Location = new System.Drawing.Point(81, 5);
      this.txtIPAddress.Name = "txtIPAddress";
      this.txtIPAddress.Size = new System.Drawing.Size(170, 21);
      this.txtIPAddress.TabIndex = 12;
      this.txtIPAddress.Text = "127.0.0.1";
      // 
      // lblIPAddress
      // 
      this.lblIPAddress.AutoSize = true;
      this.lblIPAddress.Location = new System.Drawing.Point(12, 9);
      this.lblIPAddress.Name = "lblIPAddress";
      this.lblIPAddress.Size = new System.Drawing.Size(63, 13);
      this.lblIPAddress.TabIndex = 11;
      this.lblIPAddress.Text = "IP Address:";
      // 
      // lstMessages
      // 
      this.lstMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmDirection,
            this.clmType,
            this.clmMessage});
      this.lstMessages.FullRowSelect = true;
      this.lstMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.lstMessages.Location = new System.Drawing.Point(4, 82);
      this.lstMessages.Name = "lstMessages";
      this.lstMessages.Size = new System.Drawing.Size(630, 173);
      this.lstMessages.TabIndex = 19;
      this.lstMessages.UseCompatibleStateImageBehavior = false;
      this.lstMessages.View = System.Windows.Forms.View.Details;
      // 
      // clmDirection
      // 
      this.clmDirection.Text = "Direction";
      this.clmDirection.Width = 120;
      // 
      // clmType
      // 
      this.clmType.Text = "Type";
      this.clmType.Width = 116;
      // 
      // clmMessage
      // 
      this.clmMessage.Text = "Message";
      this.clmMessage.Width = 350;
      // 
      // btnFinish
      // 
      this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFinish.Enabled = false;
      this.btnFinish.Location = new System.Drawing.Point(525, 6);
      this.btnFinish.Name = "btnFinish";
      this.btnFinish.Size = new System.Drawing.Size(109, 71);
      this.btnFinish.TabIndex = 21;
      this.btnFinish.Text = "Finish Call";
      this.btnFinish.UseVisualStyleBackColor = true;
      this.btnFinish.Click += new System.EventHandler(this.Finish_Click);
      // 
      // avaDialer
      // 
      this.avaDialer.AllowCancelPreview = false;
      this.avaDialer.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.avaDialer.Extension = null;
      this.avaDialer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.avaDialer.Host = null;
      this.avaDialer.Location = new System.Drawing.Point(0, 261);
      this.avaDialer.Name = "avaDialer";
      this.avaDialer.Password = null;
      this.avaDialer.Port = 0;
      this.avaDialer.Size = new System.Drawing.Size(638, 61);
      this.avaDialer.TabIndex = 20;
      this.avaDialer.UserId = null;
      this.avaDialer.UseSsl = false;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(638, 322);
      this.Controls.Add(this.btnFinish);
      this.Controls.Add(this.avaDialer);
      this.Controls.Add(this.lstMessages);
      this.Controls.Add(this.btnInitialize);
      this.Controls.Add(this.chkUseSSL);
      this.Controls.Add(this.txtPort);
      this.Controls.Add(this.lblPort);
      this.Controls.Add(this.txtIPAddress);
      this.Controls.Add(this.lblIPAddress);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Avaya Dialer Test Client";
      this.Load += new System.EventHandler(this.Main_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnInitialize;
    private System.Windows.Forms.CheckBox chkUseSSL;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtIPAddress;
    private System.Windows.Forms.Label lblIPAddress;
    private System.Windows.Forms.ListView lstMessages;
    private System.Windows.Forms.ColumnHeader clmDirection;
    private System.Windows.Forms.ColumnHeader clmType;
    private System.Windows.Forms.ColumnHeader clmMessage;
    private AvayaControl avaDialer;
    private System.Windows.Forms.Button btnFinish;
  }
}

