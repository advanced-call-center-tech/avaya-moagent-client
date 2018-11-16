namespace AvayaPDSEmulator
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
      this.tbcMain = new System.Windows.Forms.TabControl();
      this.tbpServer = new System.Windows.Forms.TabPage();
      this.btnStop = new System.Windows.Forms.Button();
      this.btnStart = new System.Windows.Forms.Button();
      this.tbpConfigureServer = new System.Windows.Forms.TabPage();
      this.grpSendCall = new System.Windows.Forms.GroupBox();
      this.lblSeconds = new System.Windows.Forms.Label();
      this.txtSeconds = new System.Windows.Forms.TextBox();
      this.chkRepeat = new System.Windows.Forms.CheckBox();
      this.btnSendCall = new System.Windows.Forms.Button();
      this.lblFields = new System.Windows.Forms.Label();
      this.txtMessage = new System.Windows.Forms.TextBox();
      this.lblMessage = new System.Windows.Forms.Label();
      this.lblCallType = new System.Windows.Forms.Label();
      this.cboCallType = new System.Windows.Forms.ComboBox();
      this.txtData = new System.Windows.Forms.TextBox();
      this.grpJobs = new System.Windows.Forms.GroupBox();
      this.lblJobInstructions = new System.Windows.Forms.Label();
      this.btnRemove = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.txtJobName = new System.Windows.Forms.TextBox();
      this.lstJobs = new System.Windows.Forms.ListBox();
      this.btnDisconnect = new System.Windows.Forms.Button();
      this.btnConnect = new System.Windows.Forms.Button();
      this.txtIP = new System.Windows.Forms.TextBox();
      this.lblIP = new System.Windows.Forms.Label();
      this.tbcMain.SuspendLayout();
      this.tbpServer.SuspendLayout();
      this.tbpConfigureServer.SuspendLayout();
      this.grpSendCall.SuspendLayout();
      this.grpJobs.SuspendLayout();
      this.SuspendLayout();
      // 
      // tbcMain
      // 
      this.tbcMain.Controls.Add(this.tbpServer);
      this.tbcMain.Controls.Add(this.tbpConfigureServer);
      this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbcMain.Location = new System.Drawing.Point(0, 0);
      this.tbcMain.Name = "tbcMain";
      this.tbcMain.SelectedIndex = 0;
      this.tbcMain.Size = new System.Drawing.Size(541, 487);
      this.tbcMain.TabIndex = 0;
      // 
      // tbpServer
      // 
      this.tbpServer.Controls.Add(this.btnStop);
      this.tbpServer.Controls.Add(this.btnStart);
      this.tbpServer.Location = new System.Drawing.Point(4, 22);
      this.tbpServer.Name = "tbpServer";
      this.tbpServer.Padding = new System.Windows.Forms.Padding(3);
      this.tbpServer.Size = new System.Drawing.Size(533, 461);
      this.tbpServer.TabIndex = 0;
      this.tbpServer.Text = "Start/Stop Server";
      this.tbpServer.UseVisualStyleBackColor = true;
      // 
      // btnStop
      // 
      this.btnStop.Dock = System.Windows.Forms.DockStyle.Right;
      this.btnStop.Location = new System.Drawing.Point(269, 3);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(261, 455);
      this.btnStop.TabIndex = 1;
      this.btnStop.Text = "Stop Server";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.Stop_Click);
      // 
      // btnStart
      // 
      this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnStart.Location = new System.Drawing.Point(3, 3);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(260, 455);
      this.btnStart.TabIndex = 0;
      this.btnStart.Text = "Start Server";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.Start_Click);
      // 
      // tbpConfigureServer
      // 
      this.tbpConfigureServer.Controls.Add(this.grpSendCall);
      this.tbpConfigureServer.Controls.Add(this.grpJobs);
      this.tbpConfigureServer.Controls.Add(this.btnDisconnect);
      this.tbpConfigureServer.Controls.Add(this.btnConnect);
      this.tbpConfigureServer.Controls.Add(this.txtIP);
      this.tbpConfigureServer.Controls.Add(this.lblIP);
      this.tbpConfigureServer.Location = new System.Drawing.Point(4, 22);
      this.tbpConfigureServer.Name = "tbpConfigureServer";
      this.tbpConfigureServer.Padding = new System.Windows.Forms.Padding(3);
      this.tbpConfigureServer.Size = new System.Drawing.Size(533, 461);
      this.tbpConfigureServer.TabIndex = 1;
      this.tbpConfigureServer.Text = "Configure Server";
      this.tbpConfigureServer.UseVisualStyleBackColor = true;
      // 
      // grpSendCall
      // 
      this.grpSendCall.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpSendCall.Controls.Add(this.lblSeconds);
      this.grpSendCall.Controls.Add(this.txtSeconds);
      this.grpSendCall.Controls.Add(this.chkRepeat);
      this.grpSendCall.Controls.Add(this.btnSendCall);
      this.grpSendCall.Controls.Add(this.lblFields);
      this.grpSendCall.Controls.Add(this.txtMessage);
      this.grpSendCall.Controls.Add(this.lblMessage);
      this.grpSendCall.Controls.Add(this.lblCallType);
      this.grpSendCall.Controls.Add(this.cboCallType);
      this.grpSendCall.Controls.Add(this.txtData);
      this.grpSendCall.Location = new System.Drawing.Point(246, 31);
      this.grpSendCall.Name = "grpSendCall";
      this.grpSendCall.Size = new System.Drawing.Size(281, 427);
      this.grpSendCall.TabIndex = 5;
      this.grpSendCall.TabStop = false;
      this.grpSendCall.Text = "Send Calls";
      // 
      // lblSeconds
      // 
      this.lblSeconds.AutoSize = true;
      this.lblSeconds.Location = new System.Drawing.Point(240, 105);
      this.lblSeconds.Name = "lblSeconds";
      this.lblSeconds.Size = new System.Drawing.Size(28, 13);
      this.lblSeconds.TabIndex = 7;
      this.lblSeconds.Text = "secs";
      // 
      // txtSeconds
      // 
      this.txtSeconds.Location = new System.Drawing.Point(200, 101);
      this.txtSeconds.Name = "txtSeconds";
      this.txtSeconds.Size = new System.Drawing.Size(33, 21);
      this.txtSeconds.TabIndex = 6;
      this.txtSeconds.Text = "10";
      this.txtSeconds.TextChanged += new System.EventHandler(this.Seconds_TextChanged);
      // 
      // chkRepeat
      // 
      this.chkRepeat.AutoSize = true;
      this.chkRepeat.Location = new System.Drawing.Point(11, 103);
      this.chkRepeat.Name = "chkRepeat";
      this.chkRepeat.Size = new System.Drawing.Size(186, 17);
      this.chkRepeat.TabIndex = 5;
      this.chkRepeat.Text = "Repeat to available agents every";
      this.chkRepeat.UseVisualStyleBackColor = true;
      this.chkRepeat.CheckedChanged += new System.EventHandler(this.Repeat_CheckedChanged);
      // 
      // btnSendCall
      // 
      this.btnSendCall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSendCall.Location = new System.Drawing.Point(9, 73);
      this.btnSendCall.Name = "btnSendCall";
      this.btnSendCall.Size = new System.Drawing.Size(266, 21);
      this.btnSendCall.TabIndex = 4;
      this.btnSendCall.Text = "Send Call";
      this.btnSendCall.UseVisualStyleBackColor = true;
      this.btnSendCall.Click += new System.EventHandler(this.SendCall_Click);
      // 
      // lblFields
      // 
      this.lblFields.AutoSize = true;
      this.lblFields.Location = new System.Drawing.Point(6, 129);
      this.lblFields.Name = "lblFields";
      this.lblFields.Size = new System.Drawing.Size(181, 13);
      this.lblFields.TabIndex = 8;
      this.lblFields.Text = "Fields (comma-separated, key first):";
      // 
      // txtMessage
      // 
      this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMessage.Location = new System.Drawing.Point(76, 21);
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.Size = new System.Drawing.Size(200, 21);
      this.txtMessage.TabIndex = 1;
      this.txtMessage.Text = "Home Phone - 479-273-7762";
      // 
      // lblMessage
      // 
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new System.Drawing.Point(6, 24);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new System.Drawing.Size(53, 13);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "Message:";
      // 
      // lblCallType
      // 
      this.lblCallType.AutoSize = true;
      this.lblCallType.Location = new System.Drawing.Point(6, 50);
      this.lblCallType.Name = "lblCallType";
      this.lblCallType.Size = new System.Drawing.Size(55, 13);
      this.lblCallType.TabIndex = 2;
      this.lblCallType.Text = "Call Type:";
      // 
      // cboCallType
      // 
      this.cboCallType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cboCallType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboCallType.FormattingEnabled = true;
      this.cboCallType.Items.AddRange(new object[] {
            "OUTBOUND",
            "INBOUND",
            "MANAGED"});
      this.cboCallType.Location = new System.Drawing.Point(76, 46);
      this.cboCallType.Name = "cboCallType";
      this.cboCallType.Size = new System.Drawing.Size(200, 21);
      this.cboCallType.TabIndex = 3;
      // 
      // txtData
      // 
      this.txtData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtData.Location = new System.Drawing.Point(6, 149);
      this.txtData.Multiline = true;
      this.txtData.Name = "txtData";
      this.txtData.Size = new System.Drawing.Size(269, 272);
      this.txtData.TabIndex = 9;
      this.txtData.Text = "CUSTID,100\r\nPHONE1,4235555555\r\nPHONE2,4235555555\r\nCOAPPSIG,599\r\nPhone3,4235555555" +
    "\r\nPhone4,4235555555\r\nPhone5,4235555555\r\nCURPHONE,01";
      // 
      // grpJobs
      // 
      this.grpJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.grpJobs.Controls.Add(this.lblJobInstructions);
      this.grpJobs.Controls.Add(this.btnRemove);
      this.grpJobs.Controls.Add(this.btnAdd);
      this.grpJobs.Controls.Add(this.txtJobName);
      this.grpJobs.Controls.Add(this.lstJobs);
      this.grpJobs.Location = new System.Drawing.Point(6, 31);
      this.grpJobs.Name = "grpJobs";
      this.grpJobs.Size = new System.Drawing.Size(232, 427);
      this.grpJobs.TabIndex = 4;
      this.grpJobs.TabStop = false;
      this.grpJobs.Text = "Jobs";
      // 
      // lblJobInstructions
      // 
      this.lblJobInstructions.AutoSize = true;
      this.lblJobInstructions.Location = new System.Drawing.Point(6, 17);
      this.lblJobInstructions.Name = "lblJobInstructions";
      this.lblJobInstructions.Size = new System.Drawing.Size(199, 13);
      this.lblJobInstructions.TabIndex = 0;
      this.lblJobInstructions.Text = "(Prefix job name with type and comma.)";
      // 
      // btnRemove
      // 
      this.btnRemove.Location = new System.Drawing.Point(156, 73);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(70, 21);
      this.btnRemove.TabIndex = 4;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.Remove_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(156, 47);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(70, 21);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.Add_Click);
      // 
      // txtJobName
      // 
      this.txtJobName.Location = new System.Drawing.Point(6, 47);
      this.txtJobName.Name = "txtJobName";
      this.txtJobName.Size = new System.Drawing.Size(144, 21);
      this.txtJobName.TabIndex = 1;
      this.txtJobName.TextChanged += new System.EventHandler(this.JobName_TextChanged);
      // 
      // lstJobs
      // 
      this.lstJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.lstJobs.FormattingEnabled = true;
      this.lstJobs.Items.AddRange(new object[] {
            "O,TEST1",
            "O,TEST2",
            "I,TEST3",
            "I,TEST4",
            "M,TEST5",
            "M,TEST6",
            "B,TEST7",
            "B,TEST8"});
      this.lstJobs.Location = new System.Drawing.Point(6, 73);
      this.lstJobs.Name = "lstJobs";
      this.lstJobs.Size = new System.Drawing.Size(144, 342);
      this.lstJobs.TabIndex = 3;
      this.lstJobs.SelectedIndexChanged += new System.EventHandler(this.Jobs_SelectedIndexChanged);
      // 
      // btnDisconnect
      // 
      this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDisconnect.Location = new System.Drawing.Point(412, 4);
      this.btnDisconnect.Name = "btnDisconnect";
      this.btnDisconnect.Size = new System.Drawing.Size(84, 21);
      this.btnDisconnect.TabIndex = 3;
      this.btnDisconnect.Text = "Disconnect";
      this.btnDisconnect.UseVisualStyleBackColor = true;
      this.btnDisconnect.Click += new System.EventHandler(this.Disconnect_Click);
      // 
      // btnConnect
      // 
      this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConnect.Location = new System.Drawing.Point(322, 4);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new System.Drawing.Size(84, 21);
      this.btnConnect.TabIndex = 2;
      this.btnConnect.Text = "Connect";
      this.btnConnect.UseVisualStyleBackColor = true;
      this.btnConnect.Click += new System.EventHandler(this.Connect_Click);
      // 
      // txtIP
      // 
      this.txtIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtIP.Location = new System.Drawing.Point(146, 4);
      this.txtIP.Name = "txtIP";
      this.txtIP.Size = new System.Drawing.Size(169, 21);
      this.txtIP.TabIndex = 1;
      this.txtIP.Text = "127.0.0.1";
      // 
      // lblIP
      // 
      this.lblIP.AutoSize = true;
      this.lblIP.Location = new System.Drawing.Point(6, 8);
      this.lblIP.Name = "lblIP";
      this.lblIP.Size = new System.Drawing.Size(132, 13);
      this.lblIP.TabIndex = 0;
      this.lblIP.Text = "IP of Server to Configure:";
      // 
      // Main
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(541, 487);
      this.Controls.Add(this.tbcMain);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Avaya PDS Emulator";
      this.Load += new System.EventHandler(this.Main_Load);
      this.tbcMain.ResumeLayout(false);
      this.tbpServer.ResumeLayout(false);
      this.tbpConfigureServer.ResumeLayout(false);
      this.tbpConfigureServer.PerformLayout();
      this.grpSendCall.ResumeLayout(false);
      this.grpSendCall.PerformLayout();
      this.grpJobs.ResumeLayout(false);
      this.grpJobs.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tbcMain;
    private System.Windows.Forms.TabPage tbpServer;
    private System.Windows.Forms.TabPage tbpConfigureServer;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.TextBox txtIP;
    private System.Windows.Forms.Label lblIP;
    private System.Windows.Forms.Button btnDisconnect;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.GroupBox grpJobs;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.TextBox txtJobName;
    private System.Windows.Forms.ListBox lstJobs;
    private System.Windows.Forms.GroupBox grpSendCall;
    private System.Windows.Forms.TextBox txtData;
    private System.Windows.Forms.ComboBox cboCallType;
    private System.Windows.Forms.Label lblCallType;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.Label lblMessage;
    private System.Windows.Forms.Label lblFields;
    private System.Windows.Forms.Button btnSendCall;
    private System.Windows.Forms.CheckBox chkRepeat;
    private System.Windows.Forms.Label lblSeconds;
    private System.Windows.Forms.TextBox txtSeconds;
    private System.Windows.Forms.Label lblJobInstructions;
  }
}