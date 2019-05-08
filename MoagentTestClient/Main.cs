using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvayaTestClient
{
  /// <summary>
  /// Main
  /// </summary>
  public partial class Main : Form
  {
    private AvayaMoagentClient.MoagentClient _client = null;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Main()
    {
      InitializeComponent();
    }

    private delegate void _MessageDelegate(string direction, AvayaMoagentClient.Messages.Message message);

    private void Connect_Click(object sender, EventArgs e)
    {
      int port;

      if (int.TryParse(txtPort.Text, out port))
      {
        try
        {
          _client = new AvayaMoagentClient.MoagentClient(txtIPAddress.Text, port, chkUseSSL.Checked);
          _client.MessageReceived += _dialer_MessageReceived;
          _client.MessageSent += _dialer_MessageSent;
          _client.StartConnect();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString(), "Error");
        }
      }
    }

    private void _dialer_MessageSent(object sender, AvayaMoagentClient.Messages.MessageSentEventArgs e)
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

    private void _dialer_MessageReceived(object sender, AvayaMoagentClient.Messages.MessageReceivedEventArgs e)
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

    private void _AddMessage(string direction, AvayaMoagentClient.Messages.Message msg)
    {
      var item = new ListViewItem();
      item.Text = direction;
      item.SubItems.Add(msg.Type.ToString());
      item.SubItems.Add(msg.RawMessage);
      lstMessages.Items.Insert(0, item);
    }

    private void Disconnect_Click(object sender, EventArgs e)
    {
      if (_client != null && _client.IsConnected)
      {
        _client.Disconnect();
        _client.MessageReceived -= _dialer_MessageReceived;
        _client.MessageSent -= _dialer_MessageSent;
      }

      _client = null;
    }

    private void Logon_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.Logon(txtUserName.Text, txtPassword.Text));
      }
    }

    private void Logoff_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.Logoff.Default);
      }
    }

    private void ReserveHeadset_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.ReserveHeadset(txtExtension.Text));
      }
    }

    private void DisconnectHeadset_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.DisconnectHeadset.Default);
      }
    }

    private void FreeHeadset_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.FreeHeadset.Default);
      }
    }

    private void ListState_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.ListState.Default);
      }
    }

    private void ListJobs_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.ListJobs.All);
      }
    }

    private void DetachJob_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);
      }
    }

    private void GoAvailable_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.AvailableWork.Default);
      }
    }

    private void ReadyNextItem_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.ReadyNextItem.Default);
      }
    }

    private void Release_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.ReleaseLine.Default);
      }
    }

    private void NoFurtherWork_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(AvayaMoagentClient.Commands.NoFurtherWork.Default);
      }
    }

    private void AttachJob_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.AttachJob(txtJobName.Text));
      }
    }

    private void SetDataField_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.SetDataField(chkOutbound.Checked ?
          AvayaMoagentClient.Enumerations.FieldListType.Outbound : AvayaMoagentClient.Enumerations.FieldListType.Inbound,
          txtFieldName.Text));
      }
    }

    private void SetNotifyKeyField_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.SetNotifyKeyField(chkOutbound.Checked ?
          AvayaMoagentClient.Enumerations.FieldListType.Outbound : AvayaMoagentClient.Enumerations.FieldListType.Inbound,
          txtFieldName.Text));
      }
    }

    private void FinishItem_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.FinishedItem(txtCompletionCode.Text));
      }
    }

    private void SetWorkClass_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        var workClass = (AvayaMoagentClient.Enumerations.WorkClass)Enum.Parse(
          typeof(AvayaMoagentClient.Enumerations.WorkClass), cboWorkClass.SelectedText, true);

        _client.Send(new AvayaMoagentClient.Commands.SetWorkClass(workClass));
      }
    }

    private void SetPassword_Click(object sender, EventArgs e)
    {
      if (_client != null)
      {
        _client.Send(new AvayaMoagentClient.Commands.SetPassword(txtSetPassUser.Text, txtOldPass.Text, txtNewPass.Text));
      }
    }

    private void btnConnectHeadset_Click(object sender, EventArgs e)
    {
      _client.Send(AvayaMoagentClient.Commands.ConnectHeadset.Default);
    }
  }
}
