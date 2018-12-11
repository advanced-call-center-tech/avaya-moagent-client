using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AvayaMoagentClient;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// Main
  /// </summary>
  public partial class Main : Form
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public Main()
    {
      InitializeComponent();

      avaDialer.MessageSent += Dialer_MessageSent;
      avaDialer.MessageReceived += Dialer_MessageReceived;
      avaDialer.AgentLoggedIn += Dialer_AgentLoggedIn;
      avaDialer.AgentLoggedOut += Dialer_AgentLoggedOut;
      avaDialer.CallConnected += Dialer_CallConnected;
      avaDialer.CallTransferring += Dialer_CallTransferring;
    }

    private delegate void _MessageDelegate(string direction, AvayaMoagentClient.Messages.Message message);

    private void Main_Load(object sender, EventArgs e)
    {
      avaDialer.Extension = "46423";
      avaDialer.UserId = "agent2";
      avaDialer.Password = "agent2";

      avaDialer.CallFields.Add("CUSTID");
    }

    private void Dialer_MessageSent(object sender, AvayaMoagentClient.Messages.MessageSentEventArgs e)
    {
      var direction = "Client > Dialer";

      if (this.InvokeRequired)
      {
        this.BeginInvoke(new _MessageDelegate(_AddMessage), direction, e.Message);
      }
      else
      {
        _AddMessage(direction, e.Message);
      }
    }

    private void Dialer_MessageReceived(object sender, AvayaMoagentClient.Messages.MessageReceivedEventArgs e)
    {
      var direction = "Dialer > Client";

      if (this.InvokeRequired)
      {
        this.BeginInvoke(new _MessageDelegate(_AddMessage), direction, e.Message);
      }
      else
      {
        _AddMessage(direction, e.Message);
      }
    }

    private void Dialer_CallTransferring(object sender, CallTransferringEventArgs e)
    {
      //Just always return the same transfer number
      e.TransferNumber = new TransferNumber("5551234567", "Test");
    }

    private void Dialer_AgentLoggedIn(object sender, EventArgs e)
    {
      btnInitialize.Enabled = false;
      btnFinish.Enabled = true;
    }

    private void Dialer_AgentLoggedOut(object sender, EventArgs e)
    {
      btnInitialize.Enabled = true;
      btnFinish.Enabled = false;
    }

    private void Dialer_CallConnected(object sender, EventArgs e)
    {
      var builder = new StringBuilder();

      builder.AppendLine("Call connected:\n");
      builder.AppendLine("Number dialed: \t" + avaDialer.CurrentCallData.NumberDialed);
      builder.AppendLine("Dialer mode: \t" + avaDialer.CurrentCallData.DialerMode);
      builder.AppendLine("Call data:");
      foreach (var d in avaDialer.CurrentCallData.Data)
      {
        builder.AppendLine(string.Format("\t{0}: \t{1}", d.Key, d.Value));
      }

      Task.Factory.StartNew(() =>
      {
        MessageBox.Show(builder.ToString(), "Call Connected", MessageBoxButtons.OK);
      });
    }

    private void _AddMessage(string direction, AvayaMoagentClient.Messages.Message msg)
    {
      var item = new ListViewItem();
      item.Text = direction;
      item.SubItems.Add(msg.Type.ToString());
      item.SubItems.Add(msg.RawMessage);
      lstMessages.Items.Insert(0, item);
    }

    private void Initialize_Click(object sender, EventArgs e)
    {
      avaDialer.Host = txtIPAddress.Text;
      avaDialer.Port = Convert.ToInt32(txtPort.Text);
      avaDialer.UseSsl = chkUseSSL.Checked;
    }

    private void Finish_Click(object sender, EventArgs e)
    {
      avaDialer.TerminateCall("20");
      avaDialer.GetNextCall();
    }
  }
}
