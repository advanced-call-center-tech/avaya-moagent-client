namespace AvayaDialerTestClient
{
  partial class SelectJob
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectJob));
      this.pnlBody = new System.Windows.Forms.Panel();
      this.btnRefresh = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
      this.btnBack = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.btnFinish = new System.Windows.Forms.Button();
      this.lblJobFilterDisabled = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // pnlBody
      // 
      this.pnlBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlBody.Location = new System.Drawing.Point(12, 12);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new System.Drawing.Size(413, 246);
      this.pnlBody.TabIndex = 7;
      // 
      // btnRefresh
      // 
      this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRefresh.Location = new System.Drawing.Point(110, 278);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(75, 23);
      this.btnRefresh.TabIndex = 9;
      this.btnRefresh.Text = "&Refresh";
      this.btnRefresh.UseVisualStyleBackColor = true;
      this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(12, 278);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // tmrRefresh
      // 
      this.tmrRefresh.Enabled = true;
      this.tmrRefresh.Interval = 1000;
      this.tmrRefresh.Tick += new System.EventHandler(this.Refresh_Tick);
      // 
      // btnBack
      // 
      this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBack.Enabled = false;
      this.btnBack.Location = new System.Drawing.Point(190, 278);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(75, 23);
      this.btnBack.TabIndex = 10;
      this.btnBack.Text = "< &Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.Back_Click);
      // 
      // btnNext
      // 
      this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNext.Location = new System.Drawing.Point(270, 278);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new System.Drawing.Size(75, 23);
      this.btnNext.TabIndex = 11;
      this.btnNext.Text = "&Next >";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new System.EventHandler(this.Next_Click);
      // 
      // btnFinish
      // 
      this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFinish.Location = new System.Drawing.Point(350, 278);
      this.btnFinish.Name = "btnFinish";
      this.btnFinish.Size = new System.Drawing.Size(75, 23);
      this.btnFinish.TabIndex = 12;
      this.btnFinish.Text = "&Finish";
      this.btnFinish.UseVisualStyleBackColor = true;
      this.btnFinish.Click += new System.EventHandler(this.Finish_Click);
      // 
      // lblJobFilterDisabled
      // 
      this.lblJobFilterDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblJobFilterDisabled.AutoSize = true;
      this.lblJobFilterDisabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.lblJobFilterDisabled.ForeColor = System.Drawing.Color.Red;
      this.lblJobFilterDisabled.Location = new System.Drawing.Point(307, 261);
      this.lblJobFilterDisabled.Name = "lblJobFilterDisabled";
      this.lblJobFilterDisabled.Size = new System.Drawing.Size(118, 14);
      this.lblJobFilterDisabled.TabIndex = 13;
      this.lblJobFilterDisabled.Text = "Job Filter Disabled";
      this.lblJobFilterDisabled.Visible = false;
      // 
      // SelectJob
      // 
      this.AcceptButton = this.btnFinish;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(437, 313);
      this.Controls.Add(this.pnlBody);
      this.Controls.Add(this.btnRefresh);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnFinish);
      this.Controls.Add(this.lblJobFilterDisabled);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SelectJob";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Select Job";
      this.Load += new System.EventHandler(this.SelectJob_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.Panel pnlBody;
    private System.Windows.Forms.Button btnRefresh;
    internal System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Timer tmrRefresh;
    internal System.Windows.Forms.Button btnBack;
    internal System.Windows.Forms.Button btnNext;
    internal System.Windows.Forms.Button btnFinish;
    private System.Windows.Forms.Label lblJobFilterDisabled;
  }
}