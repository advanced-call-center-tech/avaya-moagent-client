//Copyright (c) 2010 - 2012, Matthew J Little and contributors.
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are permitted
//provided that the following conditions are met:
//
//  Redistributions of source code must retain the above copyright notice, this list of conditions
//  and the following disclaimer.
//
//  Redistributions in binary form must reproduce the above copyright notice, this list of
//  conditions and the following disclaimer in the documentation and/or other materials provided
//  with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
//IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AvayaMoagentClient;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaPDSEmulator
{
  /// <summary>
  /// Main
  /// </summary>
  public partial class Main : Form
  {
    private Thread _repeatThread = null;
    private AvayaPdsServer _server = null;
    private AvayaMoagentClient.MoagentClient _client = null;
    private int _seconds;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Main()
    {
      InitializeComponent();

      cboCallType.SelectedIndex = 0;
    }

    private delegate void _MessageReceivedDelegate(object sender, MessageReceivedEventArgs e);
    private delegate void _SetJobsDelegate(List<string> jobs);

    private void Main_Load(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void JobName_TextChanged(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void Add_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(txtJobName.Text))
      {
        lstJobs.Items.Add(txtJobName.Text);

        txtJobName.Text = string.Empty;
        _SetControls();
        _SetJobList();
      }
    }

    private void Remove_Click(object sender, EventArgs e)
    {
      if (lstJobs.SelectedIndex != -1)
      {
        lstJobs.Items.RemoveAt(lstJobs.SelectedIndex);

        _SetControls();
        _SetJobList();
      }
    }

    private void Connect_Click(object sender, EventArgs e)
    {
      _client = new AvayaMoagentClient.MoagentClient(txtIP.Text, AvayaPdsServer.DEFAULT_PORT_NUMBER);
      _client.MessageReceived += _client_MessageReceived;
      _client.Connected += _client_ConnectComplete;
      _client.StartConnect();

      _SetControls();
    }

    private void _client_ConnectComplete(object sender, EventArgs e)
    {
      _SetControls();

      _client.Send(new AvayaMoagentClient.Commands.ListJobs(JobListingType.All));
    }

    private void _client_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      if (e.Message.Command == AvayaMoagentClient.Commands.ListJobs.All.Cmd && e.Message.Contents[1] == AvayaDialer.CODE_ADDITIONAL_DATA)
      {
        _SetJobs(e.Message.Contents);
      }
    }

    private void Disconnect_Click(object sender, EventArgs e)
    {
      _client.Connected -= _client_ConnectComplete;
      _client.MessageReceived -= _client_MessageReceived;
      _client.Disconnect();
      _client = null;

      _SetControls();
    }

    private void Start_Click(object sender, EventArgs e)
    {
      _server = new AvayaPdsServer();
      _server.StartListening();

      _SetControls();
    }

    private void Stop_Click(object sender, EventArgs e)
    {
      _server.StopListening();
      _server = null;

      _SetControls();
    }

    private void SendCall_Click(object sender, EventArgs e)
    {
      _SendCall();
    }

    private void Jobs_SelectedIndexChanged(object sender, EventArgs e)
    {
      _SetControls();
    }

    private void Seconds_TextChanged(object sender, EventArgs e)
    {
      int.TryParse(txtSeconds.Text, out _seconds);
    }

    private void Repeat_CheckedChanged(object sender, EventArgs e)
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

        if (_client != null && _client.IsConnected)
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

      _client.Send(new AvayaMoagentClient.Messages.Message("SETJOBS",
        AvayaMoagentClient.Enumerations.MessageType.Command, jobs.ToArray()));
    }

    private void _EndRepeatThread()
    {
      if (_repeatThread != null)
      {
        _repeatThread.Abort();
        _repeatThread = null;
      }
    }

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

        data.Add(AvayaDialer.CODE_ADDITIONAL_DATA);
        data.Add(txtMessage.Text);
        data.Add(type);
        data.AddRange(txtData.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

        _client.Send(new AvayaMoagentClient.Commands.Command(type, data.ToArray()));
      }
    }
  }
}
