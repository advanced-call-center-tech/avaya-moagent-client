using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using AvayaMoagentClient;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// Login
  /// </summary>
  public partial class Login : Form
  {
    /// <summary>
    /// Creates a Login form with the specified extension, user ID, and password pre-filled.
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    public Login(string extension, string userId, string password)
    {
      InitializeComponent();

      Extension = extension;
      UserId = userId;
      Password = Utilities.ToSecureString(password);

      txtExtension.Text = extension;
      txtUserId.Text = userId;
      txtPassword.Text = password;
    }

    /// <summary>
    /// Extension.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// UserId.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    public SecureString Password { get; set; }

    private void OK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void Extension_TextChanged(object sender, EventArgs e)
    {
      _SetButtons();
    }

    private void UserId_TextChanged(object sender, EventArgs e)
    {
      _SetButtons();
    }

    private void Password_TextChanged(object sender, EventArgs e)
    {
      _SetButtons();
    }
    
    private void _SetButtons()
    {
      btnOK.Enabled = !string.IsNullOrEmpty(txtExtension.Text) &&
        !string.IsNullOrEmpty(txtUserId.Text) && !string.IsNullOrEmpty(txtPassword.Text);
    }

    private void Login_Activated(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtExtension.Text))
      {
        txtExtension.Focus();
      }
      else if (string.IsNullOrEmpty(txtUserId.Text))
      {
        txtUserId.Focus();
      }
      else if (string.IsNullOrEmpty(txtPassword.Text))
      {
        txtPassword.Focus();
      }
      else
      {
        btnOK.Focus();
      }
    }
  }
}
