namespace AvayaTestClient
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
      this.splMain = new System.Windows.Forms.SplitContainer();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tbpSetup = new System.Windows.Forms.TabPage();
      this.btnFreeHeadset = new System.Windows.Forms.Button();
      this.btnDisconnectHeadset = new System.Windows.Forms.Button();
      this.btnReserveHeadset = new System.Windows.Forms.Button();
      this.txtExtension = new System.Windows.Forms.TextBox();
      this.lblExtension = new System.Windows.Forms.Label();
      this.btnLogoff = new System.Windows.Forms.Button();
      this.btnLogon = new System.Windows.Forms.Button();
      this.btnDisconnect = new System.Windows.Forms.Button();
      this.btnConnect = new System.Windows.Forms.Button();
      this.chkUseSSL = new System.Windows.Forms.CheckBox();
      this.txtPort = new System.Windows.Forms.TextBox();
      this.lblPort = new System.Windows.Forms.Label();
      this.txtIPAddress = new System.Windows.Forms.TextBox();
      this.lblIPAddress = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblPassword = new System.Windows.Forms.Label();
      this.txtUserName = new System.Windows.Forms.TextBox();
      this.lblUserName = new System.Windows.Forms.Label();
      this.tbpBasic = new System.Windows.Forms.TabPage();
      this.btnConnectHeadset = new System.Windows.Forms.Button();
      this.btnDetachJob = new System.Windows.Forms.Button();
      this.btnNoFurtherWork = new System.Windows.Forms.Button();
      this.btnRelease = new System.Windows.Forms.Button();
      this.btnReadyNextItem = new System.Windows.Forms.Button();
      this.btnGoAvailable = new System.Windows.Forms.Button();
      this.btnListJobs = new System.Windows.Forms.Button();
      this.btnListState = new System.Windows.Forms.Button();
      this.tbpParameterized = new System.Windows.Forms.TabPage();
      this.txtNewPass = new System.Windows.Forms.TextBox();
      this.lblNewPassword = new System.Windows.Forms.Label();
      this.btnSetPassword = new System.Windows.Forms.Button();
      this.txtOldPass = new System.Windows.Forms.TextBox();
      this.lblOldPassword = new System.Windows.Forms.Label();
      this.txtSetPassUser = new System.Windows.Forms.TextBox();
      this.lblSetPassUserName = new System.Windows.Forms.Label();
      this.btnSetWorkClass = new System.Windows.Forms.Button();
      this.cboWorkClass = new System.Windows.Forms.ComboBox();
      this.lblWorkClass = new System.Windows.Forms.Label();
      this.btnFinishItem = new System.Windows.Forms.Button();
      this.txtCompletionCode = new System.Windows.Forms.TextBox();
      this.lblCompletionCode = new System.Windows.Forms.Label();
      this.chkOutbound = new System.Windows.Forms.CheckBox();
      this.btnSetNotifyKeyField = new System.Windows.Forms.Button();
      this.btnSetDataField = new System.Windows.Forms.Button();
      this.txtFieldName = new System.Windows.Forms.TextBox();
      this.lblFieldName = new System.Windows.Forms.Label();
      this.btnAttachJob = new System.Windows.Forms.Button();
      this.txtJobName = new System.Windows.Forms.TextBox();
      this.lblJobName = new System.Windows.Forms.Label();
      this.lstMessages = new System.Windows.Forms.ListView();
      this.clmDirection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.clmMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
      this.splMain.Panel1.SuspendLayout();
      this.splMain.Panel2.SuspendLayout();
      this.splMain.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tbpSetup.SuspendLayout();
      this.tbpBasic.SuspendLayout();
      this.tbpParameterized.SuspendLayout();
      this.SuspendLayout();
      // 
      // splMain
      // 
      this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splMain.Location = new System.Drawing.Point(0, 0);
      this.splMain.Name = "splMain";
      this.splMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splMain.Panel1
      // 
      this.splMain.Panel1.Controls.Add(this.tabControl1);
      // 
      // splMain.Panel2
      // 
      this.splMain.Panel2.Controls.Add(this.lstMessages);
      this.splMain.Size = new System.Drawing.Size(618, 381);
      this.splMain.SplitterDistance = 235;
      this.splMain.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tbpSetup);
      this.tabControl1.Controls.Add(this.tbpBasic);
      this.tabControl1.Controls.Add(this.tbpParameterized);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(618, 235);
      this.tabControl1.TabIndex = 0;
      // 
      // tbpSetup
      // 
      this.tbpSetup.Controls.Add(this.btnFreeHeadset);
      this.tbpSetup.Controls.Add(this.btnDisconnectHeadset);
      this.tbpSetup.Controls.Add(this.btnReserveHeadset);
      this.tbpSetup.Controls.Add(this.txtExtension);
      this.tbpSetup.Controls.Add(this.lblExtension);
      this.tbpSetup.Controls.Add(this.btnLogoff);
      this.tbpSetup.Controls.Add(this.btnLogon);
      this.tbpSetup.Controls.Add(this.btnDisconnect);
      this.tbpSetup.Controls.Add(this.btnConnect);
      this.tbpSetup.Controls.Add(this.chkUseSSL);
      this.tbpSetup.Controls.Add(this.txtPort);
      this.tbpSetup.Controls.Add(this.lblPort);
      this.tbpSetup.Controls.Add(this.txtIPAddress);
      this.tbpSetup.Controls.Add(this.lblIPAddress);
      this.tbpSetup.Controls.Add(this.txtPassword);
      this.tbpSetup.Controls.Add(this.lblPassword);
      this.tbpSetup.Controls.Add(this.txtUserName);
      this.tbpSetup.Controls.Add(this.lblUserName);
      this.tbpSetup.Location = new System.Drawing.Point(4, 22);
      this.tbpSetup.Name = "tbpSetup";
      this.tbpSetup.Padding = new System.Windows.Forms.Padding(3);
      this.tbpSetup.Size = new System.Drawing.Size(610, 209);
      this.tbpSetup.TabIndex = 0;
      this.tbpSetup.Text = "Setup";
      this.tbpSetup.UseVisualStyleBackColor = true;
      // 
      // btnFreeHeadset
      // 
      this.btnFreeHeadset.Location = new System.Drawing.Point(494, 176);
      this.btnFreeHeadset.Name = "btnFreeHeadset";
      this.btnFreeHeadset.Size = new System.Drawing.Size(108, 25);
      this.btnFreeHeadset.TabIndex = 17;
      this.btnFreeHeadset.Text = "Free Headset";
      this.btnFreeHeadset.UseVisualStyleBackColor = true;
      this.btnFreeHeadset.Click += new System.EventHandler(this.FreeHeadset_Click);
      // 
      // btnDisconnectHeadset
      // 
      this.btnDisconnectHeadset.Location = new System.Drawing.Point(370, 176);
      this.btnDisconnectHeadset.Name = "btnDisconnectHeadset";
      this.btnDisconnectHeadset.Size = new System.Drawing.Size(118, 25);
      this.btnDisconnectHeadset.TabIndex = 16;
      this.btnDisconnectHeadset.Text = "Disconnect Headset";
      this.btnDisconnectHeadset.UseVisualStyleBackColor = true;
      this.btnDisconnectHeadset.Click += new System.EventHandler(this.DisconnectHeadset_Click);
      // 
      // btnReserveHeadset
      // 
      this.btnReserveHeadset.Location = new System.Drawing.Point(255, 176);
      this.btnReserveHeadset.Name = "btnReserveHeadset";
      this.btnReserveHeadset.Size = new System.Drawing.Size(109, 25);
      this.btnReserveHeadset.TabIndex = 15;
      this.btnReserveHeadset.Text = "Reserve Headset";
      this.btnReserveHeadset.UseVisualStyleBackColor = true;
      this.btnReserveHeadset.Click += new System.EventHandler(this.ReserveHeadset_Click);
      // 
      // txtExtension
      // 
      this.txtExtension.Location = new System.Drawing.Point(79, 176);
      this.txtExtension.Name = "txtExtension";
      this.txtExtension.Size = new System.Drawing.Size(170, 21);
      this.txtExtension.TabIndex = 14;
      // 
      // lblExtension
      // 
      this.lblExtension.AutoSize = true;
      this.lblExtension.Location = new System.Drawing.Point(16, 180);
      this.lblExtension.Name = "lblExtension";
      this.lblExtension.Size = new System.Drawing.Size(58, 13);
      this.lblExtension.TabIndex = 13;
      this.lblExtension.Text = "Extension:";
      // 
      // btnLogoff
      // 
      this.btnLogoff.Location = new System.Drawing.Point(370, 99);
      this.btnLogoff.Name = "btnLogoff";
      this.btnLogoff.Size = new System.Drawing.Size(109, 47);
      this.btnLogoff.TabIndex = 12;
      this.btnLogoff.Text = "Logoff";
      this.btnLogoff.UseVisualStyleBackColor = true;
      this.btnLogoff.Click += new System.EventHandler(this.Logoff_Click);
      // 
      // btnLogon
      // 
      this.btnLogon.Location = new System.Drawing.Point(255, 99);
      this.btnLogon.Name = "btnLogon";
      this.btnLogon.Size = new System.Drawing.Size(109, 47);
      this.btnLogon.TabIndex = 11;
      this.btnLogon.Text = "Logon";
      this.btnLogon.UseVisualStyleBackColor = true;
      this.btnLogon.Click += new System.EventHandler(this.Logon_Click);
      // 
      // btnDisconnect
      // 
      this.btnDisconnect.Location = new System.Drawing.Point(370, 6);
      this.btnDisconnect.Name = "btnDisconnect";
      this.btnDisconnect.Size = new System.Drawing.Size(109, 71);
      this.btnDisconnect.TabIndex = 10;
      this.btnDisconnect.Text = "Disconnect";
      this.btnDisconnect.UseVisualStyleBackColor = true;
      this.btnDisconnect.Click += new System.EventHandler(this.Disconnect_Click);
      // 
      // btnConnect
      // 
      this.btnConnect.Location = new System.Drawing.Point(255, 6);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new System.Drawing.Size(109, 71);
      this.btnConnect.TabIndex = 9;
      this.btnConnect.Text = "Connect";
      this.btnConnect.UseVisualStyleBackColor = true;
      this.btnConnect.Click += new System.EventHandler(this.Connect_Click);
      // 
      // chkUseSSL
      // 
      this.chkUseSSL.AutoSize = true;
      this.chkUseSSL.Checked = true;
      this.chkUseSSL.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkUseSSL.Location = new System.Drawing.Point(79, 60);
      this.chkUseSSL.Name = "chkUseSSL";
      this.chkUseSSL.Size = new System.Drawing.Size(69, 17);
      this.chkUseSSL.TabIndex = 8;
      this.chkUseSSL.Text = "Use SSL?";
      this.chkUseSSL.UseVisualStyleBackColor = true;
      // 
      // txtPort
      // 
      this.txtPort.Location = new System.Drawing.Point(79, 33);
      this.txtPort.Name = "txtPort";
      this.txtPort.Size = new System.Drawing.Size(170, 21);
      this.txtPort.TabIndex = 7;
      this.txtPort.Text = "22700";
      // 
      // lblPort
      // 
      this.lblPort.AutoSize = true;
      this.lblPort.Location = new System.Drawing.Point(42, 37);
      this.lblPort.Name = "lblPort";
      this.lblPort.Size = new System.Drawing.Size(31, 13);
      this.lblPort.TabIndex = 6;
      this.lblPort.Text = "Port:";
      // 
      // txtIPAddress
      // 
      this.txtIPAddress.Location = new System.Drawing.Point(79, 6);
      this.txtIPAddress.Name = "txtIPAddress";
      this.txtIPAddress.Size = new System.Drawing.Size(170, 21);
      this.txtIPAddress.TabIndex = 5;
      // 
      // lblIPAddress
      // 
      this.lblIPAddress.AutoSize = true;
      this.lblIPAddress.Location = new System.Drawing.Point(10, 10);
      this.lblIPAddress.Name = "lblIPAddress";
      this.lblIPAddress.Size = new System.Drawing.Size(63, 13);
      this.lblIPAddress.TabIndex = 4;
      this.lblIPAddress.Text = "IP Address:";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(79, 125);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(170, 21);
      this.txtPassword.TabIndex = 3;
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new System.Drawing.Point(16, 129);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(57, 13);
      this.lblPassword.TabIndex = 2;
      this.lblPassword.Text = "Password:";
      // 
      // txtUserName
      // 
      this.txtUserName.Location = new System.Drawing.Point(79, 99);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.Size = new System.Drawing.Size(170, 21);
      this.txtUserName.TabIndex = 1;
      // 
      // lblUserName
      // 
      this.lblUserName.AutoSize = true;
      this.lblUserName.Location = new System.Drawing.Point(10, 103);
      this.lblUserName.Name = "lblUserName";
      this.lblUserName.Size = new System.Drawing.Size(63, 13);
      this.lblUserName.TabIndex = 0;
      this.lblUserName.Text = "User Name:";
      // 
      // tbpBasic
      // 
      this.tbpBasic.Controls.Add(this.btnConnectHeadset);
      this.tbpBasic.Controls.Add(this.btnDetachJob);
      this.tbpBasic.Controls.Add(this.btnNoFurtherWork);
      this.tbpBasic.Controls.Add(this.btnRelease);
      this.tbpBasic.Controls.Add(this.btnReadyNextItem);
      this.tbpBasic.Controls.Add(this.btnGoAvailable);
      this.tbpBasic.Controls.Add(this.btnListJobs);
      this.tbpBasic.Controls.Add(this.btnListState);
      this.tbpBasic.Location = new System.Drawing.Point(4, 22);
      this.tbpBasic.Name = "tbpBasic";
      this.tbpBasic.Padding = new System.Windows.Forms.Padding(3);
      this.tbpBasic.Size = new System.Drawing.Size(610, 209);
      this.tbpBasic.TabIndex = 1;
      this.tbpBasic.Text = "Basic Commands";
      this.tbpBasic.UseVisualStyleBackColor = true;
      // 
      // btnConnectHeadset
      // 
      this.btnConnectHeadset.Location = new System.Drawing.Point(240, 59);
      this.btnConnectHeadset.Name = "btnConnectHeadset";
      this.btnConnectHeadset.Size = new System.Drawing.Size(109, 47);
      this.btnConnectHeadset.TabIndex = 20;
      this.btnConnectHeadset.Text = "Connect Headset";
      this.btnConnectHeadset.UseVisualStyleBackColor = true;
      this.btnConnectHeadset.Click += new System.EventHandler(this.btnConnectHeadset_Click);
      // 
      // btnDetachJob
      // 
      this.btnDetachJob.Location = new System.Drawing.Point(10, 112);
      this.btnDetachJob.Name = "btnDetachJob";
      this.btnDetachJob.Size = new System.Drawing.Size(109, 47);
      this.btnDetachJob.TabIndex = 19;
      this.btnDetachJob.Text = "Detach Job";
      this.btnDetachJob.UseVisualStyleBackColor = true;
      this.btnDetachJob.Click += new System.EventHandler(this.DetachJob_Click);
      // 
      // btnNoFurtherWork
      // 
      this.btnNoFurtherWork.Location = new System.Drawing.Point(240, 6);
      this.btnNoFurtherWork.Name = "btnNoFurtherWork";
      this.btnNoFurtherWork.Size = new System.Drawing.Size(109, 47);
      this.btnNoFurtherWork.TabIndex = 18;
      this.btnNoFurtherWork.Text = "No Further Work";
      this.btnNoFurtherWork.UseVisualStyleBackColor = true;
      this.btnNoFurtherWork.Click += new System.EventHandler(this.NoFurtherWork_Click);
      // 
      // btnRelease
      // 
      this.btnRelease.Location = new System.Drawing.Point(125, 112);
      this.btnRelease.Name = "btnRelease";
      this.btnRelease.Size = new System.Drawing.Size(109, 47);
      this.btnRelease.TabIndex = 17;
      this.btnRelease.Text = "Release";
      this.btnRelease.UseVisualStyleBackColor = true;
      this.btnRelease.Click += new System.EventHandler(this.Release_Click);
      // 
      // btnReadyNextItem
      // 
      this.btnReadyNextItem.Location = new System.Drawing.Point(125, 59);
      this.btnReadyNextItem.Name = "btnReadyNextItem";
      this.btnReadyNextItem.Size = new System.Drawing.Size(109, 47);
      this.btnReadyNextItem.TabIndex = 16;
      this.btnReadyNextItem.Text = "Ready Next Item";
      this.btnReadyNextItem.UseVisualStyleBackColor = true;
      this.btnReadyNextItem.Click += new System.EventHandler(this.ReadyNextItem_Click);
      // 
      // btnGoAvailable
      // 
      this.btnGoAvailable.Location = new System.Drawing.Point(125, 6);
      this.btnGoAvailable.Name = "btnGoAvailable";
      this.btnGoAvailable.Size = new System.Drawing.Size(109, 47);
      this.btnGoAvailable.TabIndex = 15;
      this.btnGoAvailable.Text = "Go Available";
      this.btnGoAvailable.UseVisualStyleBackColor = true;
      this.btnGoAvailable.Click += new System.EventHandler(this.GoAvailable_Click);
      // 
      // btnListJobs
      // 
      this.btnListJobs.Location = new System.Drawing.Point(10, 59);
      this.btnListJobs.Name = "btnListJobs";
      this.btnListJobs.Size = new System.Drawing.Size(109, 47);
      this.btnListJobs.TabIndex = 13;
      this.btnListJobs.Text = "List Jobs";
      this.btnListJobs.UseVisualStyleBackColor = true;
      this.btnListJobs.Click += new System.EventHandler(this.ListJobs_Click);
      // 
      // btnListState
      // 
      this.btnListState.Location = new System.Drawing.Point(10, 6);
      this.btnListState.Name = "btnListState";
      this.btnListState.Size = new System.Drawing.Size(109, 47);
      this.btnListState.TabIndex = 12;
      this.btnListState.Text = "List State";
      this.btnListState.UseVisualStyleBackColor = true;
      this.btnListState.Click += new System.EventHandler(this.ListState_Click);
      // 
      // tbpParameterized
      // 
      this.tbpParameterized.Controls.Add(this.txtNewPass);
      this.tbpParameterized.Controls.Add(this.lblNewPassword);
      this.tbpParameterized.Controls.Add(this.btnSetPassword);
      this.tbpParameterized.Controls.Add(this.txtOldPass);
      this.tbpParameterized.Controls.Add(this.lblOldPassword);
      this.tbpParameterized.Controls.Add(this.txtSetPassUser);
      this.tbpParameterized.Controls.Add(this.lblSetPassUserName);
      this.tbpParameterized.Controls.Add(this.btnSetWorkClass);
      this.tbpParameterized.Controls.Add(this.cboWorkClass);
      this.tbpParameterized.Controls.Add(this.lblWorkClass);
      this.tbpParameterized.Controls.Add(this.btnFinishItem);
      this.tbpParameterized.Controls.Add(this.txtCompletionCode);
      this.tbpParameterized.Controls.Add(this.lblCompletionCode);
      this.tbpParameterized.Controls.Add(this.chkOutbound);
      this.tbpParameterized.Controls.Add(this.btnSetNotifyKeyField);
      this.tbpParameterized.Controls.Add(this.btnSetDataField);
      this.tbpParameterized.Controls.Add(this.txtFieldName);
      this.tbpParameterized.Controls.Add(this.lblFieldName);
      this.tbpParameterized.Controls.Add(this.btnAttachJob);
      this.tbpParameterized.Controls.Add(this.txtJobName);
      this.tbpParameterized.Controls.Add(this.lblJobName);
      this.tbpParameterized.Location = new System.Drawing.Point(4, 22);
      this.tbpParameterized.Name = "tbpParameterized";
      this.tbpParameterized.Size = new System.Drawing.Size(610, 209);
      this.tbpParameterized.TabIndex = 2;
      this.tbpParameterized.Text = "Parameterized Commands";
      this.tbpParameterized.UseVisualStyleBackColor = true;
      // 
      // txtNewPass
      // 
      this.txtNewPass.Location = new System.Drawing.Point(104, 176);
      this.txtNewPass.Name = "txtNewPass";
      this.txtNewPass.Size = new System.Drawing.Size(170, 21);
      this.txtNewPass.TabIndex = 36;
      // 
      // lblNewPassword
      // 
      this.lblNewPassword.AutoSize = true;
      this.lblNewPassword.Location = new System.Drawing.Point(17, 180);
      this.lblNewPassword.Name = "lblNewPassword";
      this.lblNewPassword.Size = new System.Drawing.Size(81, 13);
      this.lblNewPassword.TabIndex = 35;
      this.lblNewPassword.Text = "New Password:";
      // 
      // btnSetPassword
      // 
      this.btnSetPassword.Location = new System.Drawing.Point(280, 123);
      this.btnSetPassword.Name = "btnSetPassword";
      this.btnSetPassword.Size = new System.Drawing.Size(109, 74);
      this.btnSetPassword.TabIndex = 34;
      this.btnSetPassword.Text = "Set Password";
      this.btnSetPassword.UseVisualStyleBackColor = true;
      this.btnSetPassword.Click += new System.EventHandler(this.SetPassword_Click);
      // 
      // txtOldPass
      // 
      this.txtOldPass.Location = new System.Drawing.Point(104, 149);
      this.txtOldPass.Name = "txtOldPass";
      this.txtOldPass.Size = new System.Drawing.Size(170, 21);
      this.txtOldPass.TabIndex = 33;
      // 
      // lblOldPassword
      // 
      this.lblOldPassword.AutoSize = true;
      this.lblOldPassword.Location = new System.Drawing.Point(22, 153);
      this.lblOldPassword.Name = "lblOldPassword";
      this.lblOldPassword.Size = new System.Drawing.Size(76, 13);
      this.lblOldPassword.TabIndex = 32;
      this.lblOldPassword.Text = "Old Password:";
      // 
      // txtSetPassUser
      // 
      this.txtSetPassUser.Location = new System.Drawing.Point(104, 123);
      this.txtSetPassUser.Name = "txtSetPassUser";
      this.txtSetPassUser.Size = new System.Drawing.Size(170, 21);
      this.txtSetPassUser.TabIndex = 31;
      // 
      // lblSetPassUserName
      // 
      this.lblSetPassUserName.AutoSize = true;
      this.lblSetPassUserName.Location = new System.Drawing.Point(35, 127);
      this.lblSetPassUserName.Name = "lblSetPassUserName";
      this.lblSetPassUserName.Size = new System.Drawing.Size(63, 13);
      this.lblSetPassUserName.TabIndex = 30;
      this.lblSetPassUserName.Text = "User Name:";
      // 
      // btnSetWorkClass
      // 
      this.btnSetWorkClass.Location = new System.Drawing.Point(251, 92);
      this.btnSetWorkClass.Name = "btnSetWorkClass";
      this.btnSetWorkClass.Size = new System.Drawing.Size(109, 25);
      this.btnSetWorkClass.TabIndex = 29;
      this.btnSetWorkClass.Text = "Set Work Class";
      this.btnSetWorkClass.UseVisualStyleBackColor = true;
      this.btnSetWorkClass.Click += new System.EventHandler(this.SetWorkClass_Click);
      // 
      // cboWorkClass
      // 
      this.cboWorkClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWorkClass.FormattingEnabled = true;
      this.cboWorkClass.Items.AddRange(new object[] {
            "Undefined",
            "Inbound",
            "Outbound",
            "Blend",
            "PersonToPerson",
            "Managed"});
      this.cboWorkClass.Location = new System.Drawing.Point(104, 92);
      this.cboWorkClass.Name = "cboWorkClass";
      this.cboWorkClass.Size = new System.Drawing.Size(141, 21);
      this.cboWorkClass.TabIndex = 28;
      // 
      // lblWorkClass
      // 
      this.lblWorkClass.AutoSize = true;
      this.lblWorkClass.Location = new System.Drawing.Point(35, 95);
      this.lblWorkClass.Name = "lblWorkClass";
      this.lblWorkClass.Size = new System.Drawing.Size(64, 13);
      this.lblWorkClass.TabIndex = 27;
      this.lblWorkClass.Text = "Work Class:";
      // 
      // btnFinishItem
      // 
      this.btnFinishItem.Location = new System.Drawing.Point(251, 63);
      this.btnFinishItem.Name = "btnFinishItem";
      this.btnFinishItem.Size = new System.Drawing.Size(109, 25);
      this.btnFinishItem.TabIndex = 26;
      this.btnFinishItem.Text = "Finish Item";
      this.btnFinishItem.UseVisualStyleBackColor = true;
      this.btnFinishItem.Click += new System.EventHandler(this.FinishItem_Click);
      // 
      // txtCompletionCode
      // 
      this.txtCompletionCode.Location = new System.Drawing.Point(104, 63);
      this.txtCompletionCode.Name = "txtCompletionCode";
      this.txtCompletionCode.Size = new System.Drawing.Size(141, 21);
      this.txtCompletionCode.TabIndex = 25;
      this.txtCompletionCode.Text = "20";
      // 
      // lblCompletionCode
      // 
      this.lblCompletionCode.AutoSize = true;
      this.lblCompletionCode.Location = new System.Drawing.Point(7, 67);
      this.lblCompletionCode.Name = "lblCompletionCode";
      this.lblCompletionCode.Size = new System.Drawing.Size(92, 13);
      this.lblCompletionCode.TabIndex = 24;
      this.lblCompletionCode.Text = "Completion Code:";
      // 
      // chkOutbound
      // 
      this.chkOutbound.AutoSize = true;
      this.chkOutbound.Checked = true;
      this.chkOutbound.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkOutbound.Location = new System.Drawing.Point(254, 40);
      this.chkOutbound.Name = "chkOutbound";
      this.chkOutbound.Size = new System.Drawing.Size(74, 17);
      this.chkOutbound.TabIndex = 23;
      this.chkOutbound.Text = "Outbound";
      this.chkOutbound.UseVisualStyleBackColor = true;
      // 
      // btnSetNotifyKeyField
      // 
      this.btnSetNotifyKeyField.Location = new System.Drawing.Point(479, 36);
      this.btnSetNotifyKeyField.Name = "btnSetNotifyKeyField";
      this.btnSetNotifyKeyField.Size = new System.Drawing.Size(123, 25);
      this.btnSetNotifyKeyField.TabIndex = 22;
      this.btnSetNotifyKeyField.Text = "Set Notify Key Field";
      this.btnSetNotifyKeyField.UseVisualStyleBackColor = true;
      this.btnSetNotifyKeyField.Click += new System.EventHandler(this.SetNotifyKeyField_Click);
      // 
      // btnSetDataField
      // 
      this.btnSetDataField.Location = new System.Drawing.Point(364, 36);
      this.btnSetDataField.Name = "btnSetDataField";
      this.btnSetDataField.Size = new System.Drawing.Size(109, 25);
      this.btnSetDataField.TabIndex = 21;
      this.btnSetDataField.Text = "Set Data Field";
      this.btnSetDataField.UseVisualStyleBackColor = true;
      this.btnSetDataField.Click += new System.EventHandler(this.SetDataField_Click);
      // 
      // txtFieldName
      // 
      this.txtFieldName.Location = new System.Drawing.Point(104, 36);
      this.txtFieldName.Name = "txtFieldName";
      this.txtFieldName.Size = new System.Drawing.Size(141, 21);
      this.txtFieldName.TabIndex = 20;
      this.txtFieldName.Text = "ACTID";
      // 
      // lblFieldName
      // 
      this.lblFieldName.AutoSize = true;
      this.lblFieldName.Location = new System.Drawing.Point(36, 40);
      this.lblFieldName.Name = "lblFieldName";
      this.lblFieldName.Size = new System.Drawing.Size(63, 13);
      this.lblFieldName.TabIndex = 19;
      this.lblFieldName.Text = "Field Name:";
      // 
      // btnAttachJob
      // 
      this.btnAttachJob.Location = new System.Drawing.Point(251, 6);
      this.btnAttachJob.Name = "btnAttachJob";
      this.btnAttachJob.Size = new System.Drawing.Size(109, 25);
      this.btnAttachJob.TabIndex = 18;
      this.btnAttachJob.Text = "Attach Job";
      this.btnAttachJob.UseVisualStyleBackColor = true;
      this.btnAttachJob.Click += new System.EventHandler(this.AttachJob_Click);
      // 
      // txtJobName
      // 
      this.txtJobName.Location = new System.Drawing.Point(104, 6);
      this.txtJobName.Name = "txtJobName";
      this.txtJobName.Size = new System.Drawing.Size(141, 21);
      this.txtJobName.TabIndex = 17;
      // 
      // lblJobName
      // 
      this.lblJobName.AutoSize = true;
      this.lblJobName.Location = new System.Drawing.Point(41, 10);
      this.lblJobName.Name = "lblJobName";
      this.lblJobName.Size = new System.Drawing.Size(58, 13);
      this.lblJobName.TabIndex = 16;
      this.lblJobName.Text = "Job Name:";
      // 
      // lstMessages
      // 
      this.lstMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmDirection,
            this.clmType,
            this.clmMessage});
      this.lstMessages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstMessages.FullRowSelect = true;
      this.lstMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.lstMessages.Location = new System.Drawing.Point(0, 0);
      this.lstMessages.Name = "lstMessages";
      this.lstMessages.Size = new System.Drawing.Size(618, 142);
      this.lstMessages.TabIndex = 0;
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
      // Main
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(618, 381);
      this.Controls.Add(this.splMain);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(626, 408);
      this.Name = "Main";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Moagent Test Client";
      this.splMain.Panel1.ResumeLayout(false);
      this.splMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
      this.splMain.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tbpSetup.ResumeLayout(false);
      this.tbpSetup.PerformLayout();
      this.tbpBasic.ResumeLayout(false);
      this.tbpParameterized.ResumeLayout(false);
      this.tbpParameterized.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splMain;
    private System.Windows.Forms.ListView lstMessages;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tbpSetup;
    private System.Windows.Forms.TabPage tbpBasic;
    private System.Windows.Forms.TabPage tbpParameterized;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtIPAddress;
    private System.Windows.Forms.Label lblIPAddress;
    private System.Windows.Forms.CheckBox chkUseSSL;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnDisconnect;
    private System.Windows.Forms.Button btnLogoff;
    private System.Windows.Forms.Button btnLogon;
    private System.Windows.Forms.Button btnFreeHeadset;
    private System.Windows.Forms.Button btnDisconnectHeadset;
    private System.Windows.Forms.Button btnReserveHeadset;
    private System.Windows.Forms.TextBox txtExtension;
    private System.Windows.Forms.Label lblExtension;
    private System.Windows.Forms.ColumnHeader clmDirection;
    private System.Windows.Forms.ColumnHeader clmMessage;
    private System.Windows.Forms.Button btnDetachJob;
    private System.Windows.Forms.Button btnNoFurtherWork;
    private System.Windows.Forms.Button btnRelease;
    private System.Windows.Forms.Button btnReadyNextItem;
    private System.Windows.Forms.Button btnGoAvailable;
    private System.Windows.Forms.Button btnListJobs;
    private System.Windows.Forms.Button btnListState;
    private System.Windows.Forms.Button btnAttachJob;
    private System.Windows.Forms.TextBox txtJobName;
    private System.Windows.Forms.Label lblJobName;
    private System.Windows.Forms.Button btnSetDataField;
    private System.Windows.Forms.TextBox txtFieldName;
    private System.Windows.Forms.Label lblFieldName;
    private System.Windows.Forms.Button btnSetNotifyKeyField;
    private System.Windows.Forms.CheckBox chkOutbound;
    private System.Windows.Forms.Button btnFinishItem;
    private System.Windows.Forms.TextBox txtCompletionCode;
    private System.Windows.Forms.Label lblCompletionCode;
    private System.Windows.Forms.Label lblWorkClass;
    private System.Windows.Forms.ComboBox cboWorkClass;
    private System.Windows.Forms.Button btnSetWorkClass;
    private System.Windows.Forms.TextBox txtNewPass;
    private System.Windows.Forms.Label lblNewPassword;
    private System.Windows.Forms.Button btnSetPassword;
    private System.Windows.Forms.TextBox txtOldPass;
    private System.Windows.Forms.Label lblOldPassword;
    private System.Windows.Forms.TextBox txtSetPassUser;
    private System.Windows.Forms.Label lblSetPassUserName;
    private System.Windows.Forms.ColumnHeader clmType;
    private System.Windows.Forms.Button btnConnectHeadset;
  }
}