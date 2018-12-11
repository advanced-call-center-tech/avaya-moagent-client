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
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AvayaMoagentClient;
using AvayaMoagentClient.Commands;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;
using AvayaMoagentClient.Requests;

namespace AvayaMoagentClient
{
  /// <summary>
  /// AvayaDialer
  /// </summary>
  public partial class AvayaDialer
  {
    private const string _CODE_COMPLETE = "M00000";
    private const string _CODE_ADDITIONAL_DATA = "M00001";
    private const string _STATUS_ON_JOB_ON_CALL = "S70000";
    private const string _STATUS_ON_JOB_READY = "S70001";
    private const string _STATUS_ON_JOB_IDLE_NOT_READY = "S70002";
    private const string _STATUS_ON_JOB_UNAVAILABLE = "S70003";
    private const string _STATUS_NOT_ON_JOB = "S70004";
    private const string _FIELD_CURPHONE = "CURPHONE";
    private const string _FIELD_DEFAULT_PHONE = "PHONE1";

    private FileLogger _logger;
    private MoagentClient _client;
    private string _extension;
    private string _userName;
    private SecureString _password;
    private RequestManager _requestManager = new RequestManager();
    private JobStage _jobStage = JobStage.Undefined;
    private AgentState _agentState = AgentState.LoggedOff;
    private string _nextJobName;
    private bool _jobEnded;
    private bool _headsetConnectionBroken;

    /// <summary>
    /// Default constructor
    /// </summary>
    public AvayaDialer()
    {
      CallFields = new List<string>();

      AgentState = Enumerations.AgentState.LoggedOff;
    }

    /// <summary>
    /// MessageReceived
    /// </summary>
    public event EventHandler<AvayaMoagentClient.Messages.MessageReceivedEventArgs> MessageReceived;

    /// <summary>
    /// MessageSent
    /// </summary>
    public event EventHandler<AvayaMoagentClient.Messages.MessageSentEventArgs> MessageSent;

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
    /// JobStageChanged
    /// </summary>
    public event EventHandler<EventArgs> JobStageChanged;

    /// <summary>
    /// AgentStateChanged
    /// </summary>
    public event EventHandler<EventArgs> AgentStateChanged;

    /// <summary>
    /// Ring back not answered
    /// </summary>
    public event EventHandler<EventArgs> RingbackNotAnswered;

    /// <summary>
    /// DialerErrorReceived
    /// </summary>
    public event EventHandler<DialerErrorReceivedEventArgs> DialerErrorReceived;

    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Indicates whether to use SSL.
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// IsConnected
    /// </summary>
    public bool IsConnected
    {
      get
      {
        return _client != null && _client.IsConnected;
      }
    }

    /// <summary>
    /// CurrentCallData
    /// </summary>
    public CallData CurrentCallData { get; private set; }

    /// <summary>
    /// IsHeadsetConnected
    /// </summary>
    public bool IsHeadsetConnected { get; private set; }

    /// <summary>
    /// AllowMultiplePhoneNumbers
    /// </summary>
    public bool AllowMultiplePhoneNumbers { get; set; }

    /// <summary>
    /// UseCallFieldsForInbound
    /// </summary>
    public bool UseCallFieldsForInbound { get; set; }

    /// <summary>
    /// CallFields
    /// </summary>
    public List<string> CallFields { get; private set; }

    /// <summary>
    /// JobName
    /// </summary>
    public string JobName { get; private set; }

    /// <summary>
    /// IsJoinJobRequestPending
    /// </summary>
    public bool IsJoinJobRequestPending
    {
      get
      {
        return (_requestManager.GetFirst<JoinJobRequest>() != null);
      }
    }

    /// <summary>
    /// JobStage
    /// </summary>
    public JobStage JobStage
    {
      get
      {
        return _jobStage;
      }

      set
      {
        var old = _jobStage;

        _jobStage = value;

        if (old != value)
        {
          if (JobStageChanged != null)
          {
            JobStageChanged(this, EventArgs.Empty);
          }
        }
      }
    }

    /// <summary>
    /// AgentState
    /// </summary>
    public AgentState AgentState
    {
      get
      {
        return _agentState;
      }

      set
      {
        var old = _agentState;

        _agentState = value;
        if (_agentState == Enumerations.AgentState.OnCall)
        {
          switch (JobStage)
          {
            case Enumerations.JobStage.PreviewingRecord:
              {
                _agentState = Enumerations.AgentState.Preview;
                break;
              }

            case Enumerations.JobStage.Dialing:
              {
                _agentState = Enumerations.AgentState.Dial;
                break;
              }

            case Enumerations.JobStage.Wrap:
              {
                _agentState = Enumerations.AgentState.Update;
                break;
              }

            default:
              {
                //Do nothing
                break;
              }
          }
        }

        if (old != _agentState)
        {
          if (AgentStateChanged != null)
          {
            AgentStateChanged(this, EventArgs.Empty);
          }
        }
      }
    }

    /// <summary>
    /// Logon
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    public void Logon(string extension, string userName, string password)
    {
      if (!IsConnected)
      {
        _extension = extension;
        _userName = userName;
        _password = Utilities.ToSecureString(password);

        IsHeadsetConnected = false;
        AgentState = Enumerations.AgentState.LoggingOn;
        var request = _requestManager.Create(new LogonRequest());
        _Connect();

        _requestManager.WaitForCompletion(request);
        if (request.IsError)
        {
          AgentState = Enumerations.AgentState.LoggedOff;
          throw new Exception(string.Format("Logon request failed ({0})", request.ErrorCode));
        }
      }
    }

    /// <summary>
    /// Logoff
    /// </summary>
    public void Logoff()
    {
      if (IsConnected)
      {
        var request = _requestManager.Create(new LogoffRequest());
        AgentState = Enumerations.AgentState.LoggingOff;

        if (!string.IsNullOrEmpty(_extension))
        {
          //Start the logoff process by attempting to disconnect the headset
          _client.Send(Commands.DisconnectHeadset.Default);
        }
        else
        {
          _client.Send(Commands.Logoff.Default);
        }

        _requestManager.WaitForCompletion(request);
        IsHeadsetConnected = false;
      }
    }

    /// <summary>
    /// GoAvailable
    /// </summary>
    /// <param name="job"></param>
    /// <param name="blendMode"></param>
    public void GoAvailable(Job job, BlendMode blendMode)
    {
      if (!IsHeadsetConnected)
      {
        _ConnectHeadset();
      }

      if (_jobEnded)
      {
        _jobEnded = false;
        AgentState = Enumerations.AgentState.Idle;
      }

      JoinJob(job, blendMode, true);
    }

    /// <summary>
    /// GoUnavailable
    /// </summary>
    public void GoUnavailable()
    {
      if (_jobEnded)
      {
        AgentState = Enumerations.AgentState.GoUnavailable;

        _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);
      }
      else
      {
        if (IsHeadsetConnected)
        {
          if (AgentState == Enumerations.AgentState.Attached)
          {
            _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);
          }
          else
          {
            _client.Send(AvayaMoagentClient.Commands.NoFurtherWork.Default);
          }
        }
        else
        {
          _client.Send(AvayaMoagentClient.Commands.NoFurtherWork.Default);
        }
      }
    }

    /// <summary>
    /// GetAllJobs
    /// </summary>
    /// <returns></returns>
    public List<Job> GetAllJobs()
    {
      return GetJobs(JobListingType.All);
    }

    /// <summary>
    /// GetJobs
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<Job> GetJobs(JobListingType type)
    {
      List<Job> ret = null;

      if (IsConnected)
      {
        var request = (GetJobsRequest)_requestManager.Create(new GetJobsRequest());
        _client.Send(new Commands.ListJobs(type));

        _requestManager.WaitForCompletion(request);

        ret = request.Jobs;
      }

      return ret;
    }

    /// <summary>
    /// JoinJob
    /// </summary>
    /// <param name="job"></param>
    /// <param name="blendMode"></param>
    /// <param name="goReady"></param>
    public void JoinJob(Job job, BlendMode blendMode, bool goReady)
    {
      if (IsConnected)
      {
        var request = _requestManager.Create(new JoinJobRequest(job, blendMode, CallFields, goReady));
        _client.Send(AvayaMoagentClient.Commands.ListState.Default);

        _requestManager.WaitForCompletion(request);
      }
    }

    /// <summary>
    /// TerminateCall
    /// </summary>
    /// <param name="status"></param>
    public void TerminateCall(string status)
    {
      if (IsConnected)
      {
        var request = _requestManager.Create(new TerminateCallRequest(status));

        _client.Send(new AvayaMoagentClient.Commands.FinishedItem(status));

        _requestManager.WaitForCompletion(request);
      }
    }

    /// <summary>
    /// ReadyNextItem
    /// </summary>
    public void ReadyNextItem()
    {
      if (IsConnected)
      {
        var request = _requestManager.Create(new GetNextCallRequest());

        _client.Send(AvayaMoagentClient.Commands.ReadyNextItem.Default);

        _requestManager.WaitForCompletion(request);
      }
    }

    /// <summary>
    /// ReleaseLine
    /// </summary>
    public void ReleaseLine()
    {
      if (IsConnected)
      {
        var request = _requestManager.Create(new ReleaseLineRequest());

        _client.Send(AvayaMoagentClient.Commands.ReleaseLine.Default);

        _requestManager.WaitForCompletion(request);
      }
    }

    /// <summary>
    /// ManagedDial
    /// </summary>
    /// <param name="action"></param>
    public void ManagedDial(Action<ManagedDialRequest> action)
    {
      var request = (ManagedDialRequest)_requestManager.Create(new ManagedDialRequest());

      _client.Send(AvayaMoagentClient.Commands.ManagedCall.Default);

      Task.Factory.StartNew(() =>
        {
          _requestManager.WaitForCompletion(request);

          if (action != null)
          {
            action(request);
          }
        });
    }

    /// <summary>
    /// StartTransfer
    /// </summary>
    /// <param name="transferNumber"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public TransferCallRequest StartTransfer(TransferNumber transferNumber, Action<TransferCallRequest> action)
    {
      var request = (TransferCallRequest)_requestManager.Create(new TransferCallRequest(transferNumber));

      _client.Send(new AvayaMoagentClient.Commands.TransferCall(transferNumber.PhoneNumber));

      Task.Factory.StartNew(() =>
      {
        _requestManager.WaitForCompletion(request, 60000 * 60);

        if (action != null)
        {
          action(request);
        }
      });

      return request;
    }

    /// <summary>
    /// Conference
    /// </summary>
    public void Conference()
    {
      _client.Send(AvayaMoagentClient.Commands.TransferCall.Default);
    }

    /// <summary>
    /// CancelTransfer
    /// </summary>
    public void CancelTransfer()
    {
      _client.Send(AvayaMoagentClient.Commands.HangUpCall.Default);
    }

    /// <summary>
    /// RefreshState
    /// </summary>
    public void RefreshState()
    {
      if (IsConnected)
      {
        _client.Send(AvayaMoagentClient.Commands.ListState.Default);
      }
    }

    private void _ConnectHeadset()
    {
      var request = _requestManager.Create(new ConnectHeadsetRequest());

      _StartConnectHeadset();

      _requestManager.WaitForCompletion(request);
    }

    private void _StartConnectHeadset()
    {
      AgentState = Enumerations.AgentState.RingBack;
      _client.Send(AvayaMoagentClient.Commands.ConnectHeadset.Default);
    }

    private void _client_MessageSent(object sender, MessageSentEventArgs e)
    {
      if (_logger != null)
      {
        _logger.Write(string.Format("Client > {0:T} {1}", DateTime.Now, e.Message.RawMessage));
      }

      if (MessageSent != null)
      {
        MessageSent(sender, e);
      }
    }

    private void _client_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      if (_logger != null)
      {
        _logger.Write(string.Format("Client < {0:T} {1}", DateTime.Now, e.Message.RawMessage));
      }

      if (MessageReceived != null)
      {
        MessageReceived(sender, e);
      }

      try
      {
        if (e.Message.IsError)
        {
          _HandleErrorMessage(e.Message);
        }
        else
        {
          switch (e.Message.Type)
          {
            case MessageType.Pending:
              {
                _HandlePendingMessage(e.Message);
                break;
              }

            case MessageType.Data:
              {
                _HandleDataMessage(e.Message);
                break;
              }

            case MessageType.Response:
              {
                _HandleResponseMessage(e.Message);
                break;
              }

            case MessageType.Notification:
              {
                _HandleNotificationMessage(e.Message);
                break;
              }

            default:
              {
                //Uh...ignore it, I guess...
                break;
              }
          }
        }
      }
      catch
      {
        //Well...we tried...
      }
    }

    private void _SetupLogger()
    {
      _TeardownLogger();

      _logger = new FileLogger();
      _logger.Write(string.Format("<=========== Avaya PC Agent API log file for {0:F} ===========>", DateTime.Now));
      _logger.Write(string.Empty);
      _logger.Write("The 55 contiguous bytes starting with the A of AGT... form the Mosaix header.");
      _logger.Write("The header is composed of these parts in the order given:");
      _logger.Write("20 bytes - Mosaix command with prefix AGT (agent)");
      _logger.Write(" 1 byte - Message type");
      _logger.Write("20 bytes - Originator ID (reserved for future use)");
      _logger.Write(" 6 bytes - Process ID (reserved for future use)");
      _logger.Write(" 4 bytes - Invoke ID (reserved for future use)");
      _logger.Write(" 4 bytes - Number of data segments appended to header");
      _logger.Write(string.Empty);
    }

    private void _TeardownLogger()
    {
      if (_logger != null)
      {
        _logger.FlushBuffer();
        _logger = null;
      }
    }

    private void _Connect()
    {
      _SetupLogger();

      _client = new MoagentClient(Host, Port, UseSsl);
      _client.MessageSent += _client_MessageSent;
      _client.MessageReceived += _client_MessageReceived;
      _client.StartConnect();
    }

    private void _Disconnect()
    {
      _client.Disconnect();
      _client.MessageSent -= _client_MessageSent;
      _client.MessageReceived -= _client_MessageReceived;
      _client = null;

      _TeardownLogger();
    }

    private void _HandlePendingMessage(Message msg)
    {
      switch (msg.Command)
      {
        case AvayaMoagentClient.Commands.ManagedCall.Name:
          {
            AgentState = Enumerations.AgentState.Dial;
            JobStage = Enumerations.JobStage.Dialing;

            break;
          }

        case AvayaMoagentClient.Commands.NoFurtherWork.Name:
          {
            break;
          }

        case AvayaMoagentClient.Commands.TransferCall.Name:
          {
            break;
          }

        default:
          {
            //Uh...I don't know...
            break;
          }
      }
    }
  }
}
