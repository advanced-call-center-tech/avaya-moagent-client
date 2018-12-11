namespace AvayaDialerTestClient
{
  partial class Job
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
      this.grpWorkClass = new System.Windows.Forms.GroupBox();
      this.lstWorkClasses = new System.Windows.Forms.ListBox();
      this.lstJobs = new System.Windows.Forms.ListBox();
      this.grpJobs = new System.Windows.Forms.GroupBox();
      this.grpWorkClass.SuspendLayout();
      this.grpJobs.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpWorkClass
      // 
      this.grpWorkClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.grpWorkClass.Controls.Add(this.lstWorkClasses);
      this.grpWorkClass.Location = new System.Drawing.Point(0, 0);
      this.grpWorkClass.Name = "grpWorkClass";
      this.grpWorkClass.Size = new System.Drawing.Size(142, 207);
      this.grpWorkClass.TabIndex = 5;
      this.grpWorkClass.TabStop = false;
      this.grpWorkClass.Text = "Job Types:";
      // 
      // lstWorkClasses
      // 
      this.lstWorkClasses.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstWorkClasses.FormattingEnabled = true;
      this.lstWorkClasses.Location = new System.Drawing.Point(3, 16);
      this.lstWorkClasses.Name = "lstWorkClasses";
      this.lstWorkClasses.Size = new System.Drawing.Size(136, 188);
      this.lstWorkClasses.TabIndex = 0;
      this.lstWorkClasses.SelectedIndexChanged += new System.EventHandler(this.WorkClasses_SelectedIndexChanged);
      // 
      // lstJobs
      // 
      this.lstJobs.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstJobs.FormattingEnabled = true;
      this.lstJobs.Location = new System.Drawing.Point(3, 16);
      this.lstJobs.Name = "lstJobs";
      this.lstJobs.Size = new System.Drawing.Size(265, 188);
      this.lstJobs.TabIndex = 0;
      this.lstJobs.SelectedIndexChanged += new System.EventHandler(this.Jobs_SelectedIndexChanged);
      // 
      // grpJobs
      // 
      this.grpJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpJobs.Controls.Add(this.lstJobs);
      this.grpJobs.Location = new System.Drawing.Point(145, 0);
      this.grpJobs.Name = "grpJobs";
      this.grpJobs.Size = new System.Drawing.Size(271, 207);
      this.grpJobs.TabIndex = 4;
      this.grpJobs.TabStop = false;
      this.grpJobs.Text = "Jobs:";
      // 
      // Job
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grpWorkClass);
      this.Controls.Add(this.grpJobs);
      this.Name = "Job";
      this.Size = new System.Drawing.Size(416, 207);
      this.grpWorkClass.ResumeLayout(false);
      this.grpJobs.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.GroupBox grpWorkClass;
    internal System.Windows.Forms.ListBox lstWorkClasses;
    internal System.Windows.Forms.ListBox lstJobs;
    internal System.Windows.Forms.GroupBox grpJobs;
  }
}
