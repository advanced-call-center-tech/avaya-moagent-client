namespace AvayaDialerTestClient
{
  partial class Mode
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
      this.lstBlendMode = new System.Windows.Forms.ListBox();
      this.grpBlendMode = new System.Windows.Forms.GroupBox();
      this.grpBlendMode.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstBlendMode
      // 
      this.lstBlendMode.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstBlendMode.FormattingEnabled = true;
      this.lstBlendMode.Location = new System.Drawing.Point(3, 16);
      this.lstBlendMode.Name = "lstBlendMode";
      this.lstBlendMode.Size = new System.Drawing.Size(311, 229);
      this.lstBlendMode.TabIndex = 0;
      this.lstBlendMode.SelectedIndexChanged += new System.EventHandler(this.BlendMode_SelectedIndexChanged);
      // 
      // grpBlendMode
      // 
      this.grpBlendMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpBlendMode.Controls.Add(this.lstBlendMode);
      this.grpBlendMode.Location = new System.Drawing.Point(0, 2);
      this.grpBlendMode.Name = "grpBlendMode";
      this.grpBlendMode.Size = new System.Drawing.Size(317, 248);
      this.grpBlendMode.TabIndex = 3;
      this.grpBlendMode.TabStop = false;
      this.grpBlendMode.Text = "Select Blend Mode:";
      // 
      // Mode
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grpBlendMode);
      this.Name = "Mode";
      this.Size = new System.Drawing.Size(317, 248);
      this.grpBlendMode.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.ListBox lstBlendMode;
    internal System.Windows.Forms.GroupBox grpBlendMode;
  }
}
