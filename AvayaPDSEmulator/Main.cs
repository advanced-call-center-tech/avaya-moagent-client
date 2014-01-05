using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AvayaPDSEmulator
{
  public partial class Main : Form
  {
    private delegate void _MessageReceivedDelegate(object sender, AvayaMoagentClient.MessageReceivedEventArgs e);
    private Thread _repeatThread = null;
    private AvayaPdsServer _server = null;
    private AvayaMoagentClient.MoagentClient _client = null;

    public Main()
    {
      InitializeComponent();

      cboCallType.SelectedIndex = 0;
    }

    private void Main_Load(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void txtJobName_TextChanged(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(txtJobName.Text))
      {
        lstJobs.Items.Add(txtJobName.Text);

        txtJobName.Text = string.Empty;
        _SetControls();
        _SetJobList();
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (lstJobs.SelectedIndex != -1)
      {
        lstJobs.Items.RemoveAt(lstJobs.SelectedIndex);

        _SetControls();
        _SetJobList();
      }
    }

    private void btnConnect_Click(object sender, EventArgs e)
    {
      _client = new AvayaMoagentClient.MoagentClient(txtIP.Text, 22700);
      _client.MessageReceived += _client_MessageReceived;
      _client.ConnectComplete += _client_ConnectComplete;
      _client.StartConnectAsync();

      _SetControls();
    }

    private void _client_ConnectComplete(object sender, EventArgs e)
    {
      _SetControls();

      _client.Send(new AvayaMoagentClient.Commands.ListJobs(AvayaMoagentClient.Commands.ListJobs.JobListingType.All, true));
    }

    private void _client_MessageReceived(object sender, AvayaMoagentClient.MessageReceivedEventArgs e)
    {
      if (e.Message.Command == "AGTListJobs" && e.Message.Contents[1] == "M00001")
      {
        _SetJobs(e.Message.Contents);
      }
    }

    private void btnDisconnect_Click(object sender, EventArgs e)
    {
      _client.ConnectComplete -= _client_ConnectComplete;
      _client.MessageReceived -= _client_MessageReceived;
      _client.Disconnect();
      _client = null;

      _SetControls();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      _server = new AvayaPdsServer();
      _server.StartListening();

      _SetControls();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      _server.StopListening();
      _server = null;

      _SetControls();
    }

    private void btnSendCall_Click(object sender, EventArgs e)
    {
      _SendCall();
    }

    private void lstJobs_SelectedIndexChanged(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void txtSeconds_TextChanged(object sender, EventArgs e)
    {
      int.TryParse(txtSeconds.Text, out _seconds);
    }

    private void chkRepeat_CheckedChanged(object sender, EventArgs e)
    {
      if (chkRepeat.Checked)
      {
        _EndRepeatThread();

        if (int.TryParse(txtSeconds.Text, out _seconds))
        {
          _repeatThread = new Thread(new ThreadStart(_RepeatThread));
          _repeatThread.IsBackground = true;
          _repeatThread.Start();
        }
      }
      else
      {
        _EndRepeatThread();
      }
    }

    private delegate void _SetJobsDelegate(List<string> jobs);
    private void _SetJobs(List<string> jobs)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new _SetJobsDelegate(_SetJobs), jobs);
      }
      else
      {
        lstJobs.Items.Clear();
        for (int i = 2; i < jobs.Count; i++)
        {
          var parts = jobs[i].Split(new char[] { ',' });

          lstJobs.Items.Add(parts[0] + "," + parts[1]);
        }
      }
    }

    private void _SetControls()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new MethodInvoker(_SetControls));
      }
      else
      {
        btnStart.Enabled = (_server == null);
        btnStop.Enabled = (_server != null);

        if (_client != null && _client.Connected)
        {
          btnConnect.Enabled = false;
          txtIP.Enabled = false;
          btnDisconnect.Enabled = true;
          grpJobs.Enabled = true;
          grpSendCall.Enabled = true;

          btnAdd.Enabled = !string.IsNullOrEmpty(txtJobName.Text);
          btnRemove.Enabled = (lstJobs.SelectedIndex != -1);
        }
        else
        {
          btnConnect.Enabled = true;
          txtIP.Enabled = true;
          btnDisconnect.Enabled = false;
          grpJobs.Enabled = false;
          grpSendCall.Enabled = false;
        }
      }
    }

    private void _SetJobList()
    {
      var jobs = new List<string>();
      foreach (var item in lstJobs.Items)
      {
        jobs.Add(Convert.ToString(item));
      }

      _client.Send(new AvayaMoagentClient.Message("SETJOBS",
        AvayaMoagentClient.Message.MessageType.Command, true, jobs.ToArray()));
    }

    private void _EndRepeatThread()
    {
      if (_repeatThread != null)
      {
        _repeatThread.Abort();
        _repeatThread = null;
      }
    }

    private int _seconds;
    private void _RepeatThread()
    {
      do
      {
        Thread.Sleep(_seconds * 1000);

        _SendCall();
      } while (true);
    }

    private void _SendCall()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new MethodInvoker(_SendCall));
      }
      else
      {
        var data = new List<string>();
        var type = Convert.ToString(cboCallType.SelectedItem);

        data.Add("M00001");
        data.Add(txtMessage.Text);
        data.Add(type);
        data.AddRange(txtData.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

        _client.Send(new AvayaMoagentClient.Message(type,
          AvayaMoagentClient.Message.MessageType.Command, true, data.ToArray()));
      }
    }
  }
}
