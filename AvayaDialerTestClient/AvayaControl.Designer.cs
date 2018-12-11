namespace AvayaDialerTestClient
{
  partial class AvayaControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.Panel1 = new System.Windows.Forms.Panel();
      this.Panel2 = new System.Windows.Forms.Panel();
      this.lblMessage = new System.Windows.Forms.Label();
      this.Panel3 = new System.Windows.Forms.Panel();
      this.btnDial = new System.Windows.Forms.Button();
      this.btnNextCall = new System.Windows.Forms.Button();
      this.btnConference = new System.Windows.Forms.Button();
      this.btnTransfer = new System.Windows.Forms.Button();
      this.btnRelease = new System.Windows.Forms.Button();
      this.btnAvailable = new System.Windows.Forms.Button();
      this.btnLoginout = new System.Windows.Forms.Button();
      this.picStatus = new System.Windows.Forms.PictureBox();
      this.Panel1.SuspendLayout();
      this.Panel2.SuspendLayout();
      this.Panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
      this.SuspendLayout();
      // 
      // Panel1
      // 
      this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.Panel1.Controls.Add(this.Panel2);
      this.Panel1.Controls.Add(this.Panel3);
      this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Panel1.Location = new System.Drawing.Point(0, 0);
      this.Panel1.Name = "Panel1";
      this.Panel1.Size = new System.Drawing.Size(783, 61);
      this.Panel1.TabIndex = 17;
      // 
      // Panel2
      // 
      this.Panel2.Controls.Add(this.lblMessage);
      this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Panel2.Location = new System.Drawing.Point(0, 27);
      this.Panel2.Name = "Panel2";
      this.Panel2.Size = new System.Drawing.Size(779, 30);
      this.Panel2.TabIndex = 12;
      // 
      // lblMessage
      // 
      this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblMessage.Location = new System.Drawing.Point(0, 0);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new System.Drawing.Size(779, 30);
      this.lblMessage.TabIndex = 5;
      this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // Panel3
      // 
      this.Panel3.Controls.Add(this.btnDial);
      this.Panel3.Controls.Add(this.btnNextCall);
      this.Panel3.Controls.Add(this.btnConference);
      this.Panel3.Controls.Add(this.btnTransfer);
      this.Panel3.Controls.Add(this.btnRelease);
      this.Panel3.Controls.Add(this.btnAvailable);
      this.Panel3.Controls.Add(this.btnLoginout);
      this.Panel3.Controls.Add(this.picStatus);
      this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.Panel3.Location = new System.Drawing.Point(0, 0);
      this.Panel3.Name = "Panel3";
      this.Panel3.Size = new System.Drawing.Size(779, 27);
      this.Panel3.TabIndex = 13;
      // 
      // btnDial
      // 
      this.btnDial.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnDial.Enabled = false;
      this.btnDial.Location = new System.Drawing.Point(497, 0);
      this.btnDial.Name = "btnDial";
      this.btnDial.Size = new System.Drawing.Size(70, 27);
      this.btnDial.TabIndex = 16;
      this.btnDial.Text = "Dial";
      this.btnDial.Click += new System.EventHandler(this.Dial_Click);
      // 
      // btnNextCall
      // 
      this.btnNextCall.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnNextCall.Enabled = false;
      this.btnNextCall.Location = new System.Drawing.Point(423, 0);
      this.btnNextCall.Name = "btnNextCall";
      this.btnNextCall.Size = new System.Drawing.Size(74, 27);
      this.btnNextCall.TabIndex = 14;
      this.btnNextCall.Text = "Next Call";
      this.btnNextCall.Click += new System.EventHandler(this.NextCall_Click);
      // 
      // btnConference
      // 
      this.btnConference.AutoSize = true;
      this.btnConference.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnConference.Enabled = false;
      this.btnConference.Location = new System.Drawing.Point(336, 0);
      this.btnConference.Name = "btnConference";
      this.btnConference.Size = new System.Drawing.Size(87, 27);
      this.btnConference.TabIndex = 15;
      this.btnConference.Text = "Conference";
      this.btnConference.Click += new System.EventHandler(this.Conference_Click);
      // 
      // btnTransfer
      // 
      this.btnTransfer.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnTransfer.Enabled = false;
      this.btnTransfer.Location = new System.Drawing.Point(263, 0);
      this.btnTransfer.Name = "btnTransfer";
      this.btnTransfer.Size = new System.Drawing.Size(73, 27);
      this.btnTransfer.TabIndex = 9;
      this.btnTransfer.Text = "Transfer";
      this.btnTransfer.Click += new System.EventHandler(this.Transfer_Click);
      // 
      // btnRelease
      // 
      this.btnRelease.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnRelease.Enabled = false;
      this.btnRelease.Location = new System.Drawing.Point(198, 0);
      this.btnRelease.Name = "btnRelease";
      this.btnRelease.Size = new System.Drawing.Size(65, 27);
      this.btnRelease.TabIndex = 8;
      this.btnRelease.Text = "Release";
      this.btnRelease.Click += new System.EventHandler(this.Release_Click);
      // 
      // btnAvailable
      // 
      this.btnAvailable.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnAvailable.Enabled = false;
      this.btnAvailable.Location = new System.Drawing.Point(97, 0);
      this.btnAvailable.Name = "btnAvailable";
      this.btnAvailable.Size = new System.Drawing.Size(101, 27);
      this.btnAvailable.TabIndex = 7;
      this.btnAvailable.Text = "Go Available";
      this.btnAvailable.Click += new System.EventHandler(this.Available_Click);
      // 
      // btnLoginout
      // 
      this.btnLoginout.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnLoginout.Location = new System.Drawing.Point(32, 0);
      this.btnLoginout.Name = "btnLoginout";
      this.btnLoginout.Size = new System.Drawing.Size(65, 27);
      this.btnLoginout.TabIndex = 3;
      this.btnLoginout.Text = "Logon";
      this.btnLoginout.Click += new System.EventHandler(this.Loginout_Click);
      // 
      // picStatus
      // 
      this.picStatus.BackColor = System.Drawing.Color.Black;
      this.picStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picStatus.Dock = System.Windows.Forms.DockStyle.Left;
      this.picStatus.Location = new System.Drawing.Point(0, 0);
      this.picStatus.Name = "picStatus";
      this.picStatus.Size = new System.Drawing.Size(32, 27);
      this.picStatus.TabIndex = 1;
      this.picStatus.TabStop = false;
      this.picStatus.DoubleClick += new System.EventHandler(this.Status_Click);
      // 
      // AvayaControl
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.Panel1);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "AvayaControl";
      this.Size = new System.Drawing.Size(783, 61);
      this.Panel1.ResumeLayout(false);
      this.Panel2.ResumeLayout(false);
      this.Panel3.ResumeLayout(false);
      this.Panel3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Panel Panel1;
    internal System.Windows.Forms.Panel Panel2;
    internal System.Windows.Forms.Label lblMessage;
    internal System.Windows.Forms.Panel Panel3;
    internal System.Windows.Forms.Button btnDial;
    internal System.Windows.Forms.Button btnNextCall;
    internal System.Windows.Forms.Button btnConference;
    internal System.Windows.Forms.Button btnTransfer;
    internal System.Windows.Forms.Button btnRelease;
    internal System.Windows.Forms.Button btnAvailable;
    internal System.Windows.Forms.Button btnLoginout;
    internal System.Windows.Forms.PictureBox picStatus;
  }
}
