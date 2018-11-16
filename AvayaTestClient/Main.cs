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
    private AvayaMoagentClient.AvayaDialer _dialer = null;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Main()
    {
      InitializeComponent();
    }

    private delegate void _MessageDelegate(string direction, AvayaMoagentClient.Messages.Message message);

    private void btnConnect_Click(object sender, EventArgs e)
    {
      int port;

      if (int.TryParse(txtPort.Text, out port))
      {
        _dialer = new AvayaMoagentClient.AvayaDialer(txtIPAddress.Text, port, chkUseSSL.Checked);
        _dialer.MessageReceived += _dialer_MessageReceived;
        _dialer.MessageSent += _dialer_MessageSent;
        _dialer.Connect();
      }
    }

    private void _dialer_MessageSent(object sender, AvayaMoagentClient.Messages.MessageSentEventArgs e)
    {
      var direction = "Client > Dialer";

      if (this.InvokeRequired)
      {
        this.Invoke(new _MessageDelegate(_AddMessage), direction, e.Message);
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
        this.Invoke(new _MessageDelegate(_AddMessage), direction, e.Message);
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

    private void btnDisconnect_Click(object sender, EventArgs e)
    {
      if (_dialer != null && _dialer.IsConnected)
      {
        _dialer.Disconnect();
        _dialer.MessageReceived -= _dialer_MessageReceived;
        _dialer.MessageSent -= _dialer_MessageSent;
      }

      _dialer = null;
    }

    private void btnLogon_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.Login(txtUserName.Text, txtPassword.Text);
      }
    }

    private void btnLogoff_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.Logoff();
      }
    }

    private void btnReserveHeadset_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.ReserveHeadset(txtExtension.Text);
      }
    }

    private void btnDisconnectHeadset_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.DisconnectHeadset();
      }
    }

    private void btnFreeHeadset_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.FreeHeadset();
      }
    }

    private void btnListState_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.ListState();
      }
    }

    private void btnListJobs_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.ListJobs();
      }
    }

    private void btnDetachJob_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.DetachJob();
      }
    }

    private void btnGoAvailable_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.AvailableWork();
      }
    }

    private void btnReadyNextItem_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.ReadyNextItem();
      }
    }

    private void btnRelease_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.ReleaseLine();
      }
    }

    private void btnNoFurtherWork_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.NoFurtherWork();
      }
    }

    private void btnAttachJob_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.AttachJob(txtJobName.Text);
      }
    }

    private void btnSetDataField_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.SetDataField(chkOutbound.Checked ? AvayaMoagentClient.Enumerations.FieldListType.Outbound :
          AvayaMoagentClient.Enumerations.FieldListType.Inbound, txtFieldName.Text);
      }
    }

    private void btnSetNotifyKeyField_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.SetNotifyKeyField(chkOutbound.Checked ? AvayaMoagentClient.Enumerations.FieldListType.Outbound :
          AvayaMoagentClient.Enumerations.FieldListType.Inbound, txtFieldName.Text);
      }
    }

    private void btnFinishItem_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.FinishItem(txtCompletionCode.Text);
      }
    }

    private void btnSetWorkClass_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        var workClass = (AvayaMoagentClient.Enumerations.WorkClass)Enum.Parse(
          typeof(AvayaMoagentClient.Enumerations.WorkClass), cboWorkClass.SelectedText, true);

        _dialer.SetWorkClass(workClass);
      }
    }

    private void btnSetPassword_Click(object sender, EventArgs e)
    {
      if (_dialer != null)
      {
        _dialer.SetPassword(txtSetPassUser.Text, txtOldPass.Text, txtNewPass.Text);
      }
    }
  }
}
