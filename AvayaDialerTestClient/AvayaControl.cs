using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvayaMoagentClient;

namespace AvayaDialerTestClient
{
  /// <summary>
  /// Avaya dialer control
  /// </summary>
  public partial class AvayaControl : UserControl
  {
    private const string _TEXT_GO_AVAILABLE = "Go Available";
    private const string _TEXT_GO_UNAVAILABLE = "Go Unavailable";
    private const string _TEXT_LOGOFF = "Logoff";
    private const string _TEXT_LOGON = "Logon";
    private const string _TEXT_TRANSFER = "Transfer";
    private const string _TEXT_TRANSFER_CANCEL = "Cancel";
    private AvayaDialer _dialer;
    private AvayaMoagentClient.Requests.TransferCallRequest _transferRequest;

    /// <summary>
    /// Default constructor
    /// </summary>
    public AvayaControl()
    {
      InitializeComponent();

      _dialer = new AvayaDialer();
      _dialer.MessageSent += _dialer_MessageSent;
      _dialer.MessageReceived += _dialer_MessageReceived;
      _dialer.AgentLoggedIn += _dialer_AgentLoggedIn;
      _dialer.AgentLoggedOut += _dialer_AgentLoggedOut;
      _dialer.CallConnected += _dialer_CallConnected;
      _dialer.AgentStateChanged += _dialer_AgentStateChanged;

      AllowTransfers = true;
      AllowCancelPreview = true;
      AllowPreviewManualDial = true;
    }

    private delegate void _EventDelegate(object sender, EventArgs e);

    /// <summary>
    /// MessageSent
    /// </summary>
    public event EventHandler<AvayaMoagentClient.Messages.MessageSentEventArgs> MessageSent;

    /// <summary>
    /// MessageReceived
    /// </summary>
    public event EventHandler<AvayaMoagentClient.Messages.MessageReceivedEventArgs> MessageReceived;

    /// <summary>
    /// AgentLoggedIn
    /// </summary>
    public event EventHandler<EventArgs> AgentLoggedIn;

    /// <summary>
    /// AgentLoggedOut
    /// </summary>
    public event EventHandler<EventArgs> AgentLoggedOut;

    /// <summary>
    /// CallConnected
    /// </summary>
    public event EventHandler<EventArgs> CallConnected;

    /// <summary>
    /// CallTransferring
    /// </summary>
    public event EventHandler<CallTransferringEventArgs> CallTransferring;

    /// <summary>
    /// CallTransferCanceled
    /// </summary>
    public event EventHandler<EventArgs> CallTransferCanceled;

    /// <summary>
    /// CallTransferred
    /// </summary>
    public event EventHandler<EventArgs> CallTransferred;

    /// <summary>
    /// CallConferenced
    /// </summary>
    public event EventHandler<EventArgs> CallConferenced;

    /// <summary>
    /// Host
    /// </summary>
    public string Host
    {
      get
      {
        return _dialer.Host;
      }

      set
      {
        _dialer.Host = value;
      }
    }

    /// <summary>
    /// Port
    /// </summary>
    public int Port
    {
      get
      {
        return _dialer.Port;
      }

      set
      {
        _dialer.Port = value;
      }
    }

    /// <summary>
    /// Indicates whether to connect using SSL.
    /// </summary>
    public bool UseSsl
    {
      get
      {
        return _dialer.UseSsl;
      }

      set
      {
        _dialer.UseSsl = value;
      }
    }

    /// <summary>
    /// Extension
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// AllowCancelPreview
    /// </summary>
    public bool AllowCancelPreview { get; set; }

    /// <summary>
    /// AllowPreviewManualDial
    /// </summary>
    public bool AllowPreviewManualDial { get; set; }

    /// <summary>
    /// AllowTransfers
    /// </summary>
    public bool AllowTransfers { get; set; }

    /// <summary>
    /// CallFields
    /// </summary>
    public List<string> CallFields
    {
      get
      {
        return _dialer.CallFields;
      }
    }

    /// <summary>
    /// CurrentCallData
    /// </summary>
    public CallData CurrentCallData
    {
      get
      {
        return _dialer.CurrentCallData;
      }
    }

    /// <summary>
    /// TerminateCall
    /// </summary>
    /// <param name="status"></param>
    public void TerminateCall(string status)
    {
      _dialer.TerminateCall(status);
    }

    /// <summary>
    /// GetNextCall
    /// </summary>
    public void GetNextCall()
    {
      _dialer.ReadyNextItem();
    }

    /// <summary>
    /// ReleaseLine
    /// </summary>
    public void ReleaseLine()
    {
      _dialer.ReleaseLine();
    }

    /// <summary>
    /// GetAllJobs
    /// </summary>
    /// <returns></returns>
    public List<AvayaMoagentClient.Job> GetAllJobs()
    {
      return _dialer.GetAllJobs();
    }

    private void _dialer_MessageSent(object sender, AvayaMoagentClient.Messages.MessageSentEventArgs e)
    {
      if (MessageSent != null)
      {
        MessageSent(sender, e);
      }
    }

    private void _dialer_MessageReceived(object sender, AvayaMoagentClient.Messages.MessageReceivedEventArgs e)
    {
      if (MessageReceived != null)
      {
        MessageReceived(sender, e);
      }
    }

    private void _dialer_AgentLoggedIn(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new _EventDelegate(_dialer_AgentLoggedIn), sender, e);
      }
      else
      {
        if (AgentLoggedIn != null)
        {
          AgentLoggedIn(this, EventArgs.Empty);
        }
      }
    }

    private void _dialer_AgentLoggedOut(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new _EventDelegate(_dialer_AgentLoggedOut), sender, e);
      }
      else
      {
        if (AgentLoggedOut != null)
        {
          AgentLoggedOut(this, EventArgs.Empty);
        }
      }
    }

    private void _dialer_CallConnected(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new _EventDelegate(_dialer_CallConnected), sender, e);
      }
      else
      {
        if (CallConnected != null)
        {
          CallConnected(this, EventArgs.Empty);
        }
      }
    }

    private void _dialer_AgentStateChanged(object sender, EventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new _EventDelegate(_dialer_AgentStateChanged), sender, e);
      }
      else
      {
        switch (_dialer.AgentState)
        {
          case AvayaMoagentClient.Enumerations.AgentState.LoggingOn:
            {
              picStatus.BackColor = Color.DarkGray;
              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.RingBack:
            {
              picStatus.BackColor = Color.DarkGray;
              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.OnCall:
            {
              switch (_dialer.CurrentCallData.DialerMode)
              {
                case AvayaMoagentClient.Enumerations.DialerMode.Inbound:
                  {
                    picStatus.BackColor = Color.LemonChiffon;
                    break;
                  }

                case AvayaMoagentClient.Enumerations.DialerMode.Outbound:
                  {
                    picStatus.BackColor = Color.Yellow;
                    break;
                  }

                case AvayaMoagentClient.Enumerations.DialerMode.OutboundRecall:
                  {
                    picStatus.BackColor = Color.Orange;
                    break;
                  }

                default:
                  {
                    //Do nothing
                    break;
                  }
              }

              if (_transferRequest != null)
              {
                switch (_transferRequest.Result)
                {
                  case AvayaMoagentClient.Requests.TransferCallRequest.TransferResult.Started:
                    {
                      btnTransfer.Enabled = false;
                      btnTransfer.Text = _TEXT_TRANSFER;
                      btnConference.Visible = false;
                      btnConference.Enabled = false;
                      break;
                    }

                  case AvayaMoagentClient.Requests.TransferCallRequest.TransferResult.InProgress:
                    {
                      btnTransfer.Text = _TEXT_TRANSFER_CANCEL;
                      btnTransfer.Enabled = true;
                      btnConference.Visible = true;
                      btnConference.Enabled = true;

                      if (_transferRequest.IsConference)
                      {
                        btnTransfer.Enabled = false;
                        btnConference.Enabled = false;
                      }

                      break;
                    }

                  default:
                    {
                      //Do nothing
                      break;
                    }
                }
              }
              else
              {
                btnTransfer.Enabled = AllowTransfers;
                btnTransfer.Text = _TEXT_TRANSFER;
                btnConference.Visible = false;
                btnConference.Enabled = false;
              }

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = (_dialer.JobStage == AvayaMoagentClient.Enumerations.JobStage.OnCall);

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Attached:
            {
              picStatus.BackColor = Color.Red;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = true;
              btnLoginout.Enabled = true;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Available:
            {
              picStatus.BackColor = Color.Magenta;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = true;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Enabled = _dialer.IsJoinJobRequestPending;
              btnNextCall.Visible = _dialer.IsJoinJobRequestPending;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Idle:
            {
              picStatus.BackColor = Color.Blue;

              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnAvailable.Enabled = _dialer != null && _dialer.IsHeadsetConnected;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnLoginout.Enabled = true;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Preview:
            {
              picStatus.BackColor = Color.Cyan;

              btnRelease.Enabled = AllowCancelPreview;
              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;
              btnDial.Enabled = AllowPreviewManualDial;
              btnDial.Visible = AllowPreviewManualDial;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Dial:
            {
              picStatus.BackColor = Color.White;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Ready:
            {
              picStatus.BackColor = Color.Lime;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = true;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.Update:
            {
              picStatus.BackColor = Color.Orange;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = true;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.LoggedOn:
            {
              picStatus.BackColor = Color.DarkGray;

              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnLoginout.Text = _TEXT_LOGOFF;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.LoggedOff:
            {
              picStatus.BackColor = Color.Black;

              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnLoginout.Text = _TEXT_LOGON;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = true;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.JobTransfer:
            {
              picStatus.BackColor = Color.Brown;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.GoUnavailable:
            {
              picStatus.BackColor = Color.Red;

              btnAvailable.Text = _TEXT_GO_UNAVAILABLE;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          case AvayaMoagentClient.Enumerations.AgentState.LoggingOff:
            {
              picStatus.BackColor = Color.DarkGray;

              btnAvailable.Text = _TEXT_GO_AVAILABLE;
              btnAvailable.Enabled = false;
              btnLoginout.Enabled = false;
              btnRelease.Enabled = false;
              btnTransfer.Enabled = false;
              btnTransfer.Text = _TEXT_TRANSFER;
              btnConference.Enabled = false;
              btnConference.Visible = false;
              btnDial.Enabled = false;
              btnDial.Visible = false;
              btnNextCall.Visible = false;
              btnNextCall.Enabled = false;

              break;
            }

          default:
            {
              picStatus.BackColor = Color.Black;
              break;
            }
        }
      }
    }

    private void Loginout_Click(object sender, EventArgs e)
    {
      if (!_dialer.IsConnected)
      {
        var frm = new Login(Extension, UserId, Password);
        if (frm.ShowDialog() == DialogResult.OK)
        {
          Extension = frm.Extension;
          UserId = frm.UserId;
          Password = Utilities.ToUnsecureString(frm.Password);

          _dialer.Logon(Extension, UserId, Password);
        }
      }
      else
      {
        _dialer.Logoff();
      }
    }

    private void Available_Click(object sender, EventArgs e)
    {
      if (btnAvailable.Text == _TEXT_GO_AVAILABLE)
      {
        var frm = new SelectJob(this);
        if (frm.ShowDialog() == DialogResult.OK)
        {
          _dialer.GoAvailable(frm.SelectedJob, frm.SelectedBlendMode);
        }
      }
      else
      {
        _dialer.GoUnavailable();
      }
    }

    private void Release_Click(object sender, EventArgs e)
    {
      btnRelease.Enabled = false;
      _dialer.ReleaseLine();
    }

    private void Status_Click(object sender, EventArgs e)
    {
      _dialer.RefreshState();
    }

    private void Transfer_Click(object sender, EventArgs e)
    {
      if (btnTransfer.Text == _TEXT_TRANSFER)
      {
        var arg = new CallTransferringEventArgs();
        if (CallTransferring != null)
        {
          CallTransferring(this, arg);
        }

        if (arg.TransferNumber != null)
        {
          _transferRequest = _dialer.StartTransfer(arg.TransferNumber, (request) =>
          {
            _transferRequest = null;

            if (request.Result == AvayaMoagentClient.Requests.TransferCallRequest.TransferResult.Complete)
            {
              if (CallTransferred != null)
              {
                CallTransferred(this, EventArgs.Empty);
              }
            }
            else if (request.Result == AvayaMoagentClient.Requests.TransferCallRequest.TransferResult.Canceled)
            {
              if (CallTransferCanceled != null)
              {
                CallTransferCanceled(this, EventArgs.Empty);
              }
            }
          });
        }
      }
      else if (btnTransfer.Text == _TEXT_TRANSFER_CANCEL)
      {
        btnConference.Enabled = false;
      }
    }

    private void Conference_Click(object sender, EventArgs e)
    {
      btnTransfer.Enabled = false;
      btnConference.Enabled = false;
      _dialer.Conference();

      if (CallConferenced != null)
      {
        CallConferenced(this, EventArgs.Empty);
      }
    }

    private void NextCall_Click(object sender, EventArgs e)
    {
      _dialer.ReadyNextItem();
    }

    private void Dial_Click(object sender, EventArgs e)
    {
      _dialer.ManagedDial(null);
    }
  }
}
