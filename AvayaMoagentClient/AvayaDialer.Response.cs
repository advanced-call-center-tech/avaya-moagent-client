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
using System.Text;
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
    private void _HandleResponseMessage(Message msg)
    {
      switch (msg.Command)
      {
        case AvayaMoagentClient.Commands.Logon.Name:
          {
            var request = _requestManager.GetFirst<LogonRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            if (AgentLoggedIn != null)
            {
              AgentLoggedIn(this, EventArgs.Empty);
            }

            if (!string.IsNullOrEmpty(_extension))
            {
              _client.Send(new AvayaMoagentClient.Commands.ReserveHeadset(_extension));
            }

            break;
          }

        case AvayaMoagentClient.Commands.ReserveHeadset.Name:
          {
            _StartConnectHeadset();

            break;
          }

        case AvayaMoagentClient.Commands.ConnectHeadset.Name:
          {
            IsHeadsetConnected = true;
            _headsetConnectionBroken = false;

            AgentState = Enumerations.AgentState.Idle;

            var request = _requestManager.GetFirst<ConnectHeadsetRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            break;
          }

        case AvayaMoagentClient.Commands.SetWorkClass.Name:
          {
            var request = _requestManager.GetFirst<JoinJobRequest>();
            _client.Send(new AvayaMoagentClient.Commands.AttachJob(request.Job.JobName));

            break;
          }

        case AvayaMoagentClient.Commands.AttachJob.Name:
          {
            _nextJobName = string.Empty;
            _jobEnded = false;

            var request = _requestManager.GetFirst<JoinJobRequest>();
            if (request != null)
            {
              _SetupDataFields(request, request.Job.WorkClass);
              _SendNextField(request);
            }

            break;
          }

        case AvayaMoagentClient.Commands.SetNotifyKeyField.Name:
          {
            var request = _requestManager.GetFirst<JoinJobRequest>();
            if (request != null)
            {
              _SendNextField(request);
            }

            break;
          }

        case AvayaMoagentClient.Commands.SetDataField.Name:
          {
            var request = _requestManager.GetFirst<JoinJobRequest>();
            if (request != null)
            {
              _SendNextField(request);
            }

            break;
          }

        case AvayaMoagentClient.Commands.AvailableWork.Name:
          {
            var request = _requestManager.GetFirst<JoinJobRequest>();
            if (request != null)
            {
              AgentState = Enumerations.AgentState.Available;
              request.MarkComplete();
            }

            _client.Send(AvayaMoagentClient.Commands.ReadyNextItem.Default);

            break;
          }

        case AvayaMoagentClient.Commands.ReadyNextItem.Name:
          {
            var tcr = _requestManager.GetFirst<TransferCallRequest>();
            if (tcr != null)
            {
              tcr.Result = TransferCallRequest.TransferResult.Unknown;
              tcr.MarkComplete();
            }

            JobStage = Enumerations.JobStage.WaitingOnCall;
            AgentState = Enumerations.AgentState.Ready;

            CurrentCallData = null;
            var request = _requestManager.GetFirst<GetNextCallRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            break;
          }

        case AvayaMoagentClient.Commands.TransferCall.Name:
          {
            var request = _requestManager.GetFirst<TransferCallRequest>();
            if (request != null)
            {
              if (request.Result == TransferCallRequest.TransferResult.InProgress && !request.IsConference)
              {
                request.IsConference = true;
              }

              request.Result = TransferCallRequest.TransferResult.InProgress;
            }

            _client.Send(AvayaMoagentClient.Commands.ListState.Default);

            break;
          }

        case AvayaMoagentClient.Commands.ReleaseLine.Name:
          {
            var request = _requestManager.GetFirst<ReleaseLineRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            if (JobStage == Enumerations.JobStage.PreviewingRecord)
            {
              var req = _requestManager.GetFirst<ManagedDialRequest>();
              if (req != null)
              {
                req.Result = DialResult.Canceled;
              }
            }

            var tcr = _requestManager.GetFirst<TransferCallRequest>();
            if (tcr != null)
            {
              tcr.Result = TransferCallRequest.TransferResult.Complete;
              tcr.MarkComplete();
            }

            JobStage = Enumerations.JobStage.Wrap;
            AgentState = Enumerations.AgentState.OnCall;

            break;
          }

        case AvayaMoagentClient.Commands.FinishedItem.Name:
          {
            var request = _requestManager.GetFirst<TerminateCallRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            AgentState = Enumerations.AgentState.Available;

            break;
          }

        case AvayaMoagentClient.Commands.HangUpCall.Name:
          {
            var request = _requestManager.GetFirst<TransferCallRequest>();
            if (request != null)
            {
              request.Result = TransferCallRequest.TransferResult.Canceled;
              request.MarkComplete();
            }

            break;
          }

        case AvayaMoagentClient.Commands.NoFurtherWork.Name:
          {
            if (!string.IsNullOrEmpty(_nextJobName))
            {
              AgentState = Enumerations.AgentState.JobTransfer;
            }
            else
            {
              AgentState = Enumerations.AgentState.GoUnavailable;
            }

            if (!_jobEnded)
            {
              _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);
            }

            break;
          }

        case AvayaMoagentClient.Commands.DetachJob.Name:
          {
            if (IsHeadsetConnected)
            {
              if (!string.IsNullOrEmpty(_nextJobName))
              {
                AgentState = Enumerations.AgentState.JobTransfer;
                _client.Send(AvayaMoagentClient.Commands.ListJobs.All);
              }
              else
              {
                if (_requestManager.GetFirst<LogoffRequest>() != null)
                {
                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);
                }
                else
                {
                  AgentState = Enumerations.AgentState.Idle;
                }
              }
            }
            else
            {
              _nextJobName = string.Empty;

              //If the headset is disconnected, get off the dialer
              _client.Send(AvayaMoagentClient.Commands.DisconnectHeadset.Default);
            }

            break;
          }

        case AvayaMoagentClient.Commands.DisconnectHeadset.Name:
          {
            IsHeadsetConnected = false;
            _client.Send(AvayaMoagentClient.Commands.FreeHeadset.Default);

            break;
          }

        case AvayaMoagentClient.Commands.FreeHeadset.Name:
          {
            _client.Send(AvayaMoagentClient.Commands.Logoff.Default);

            break;
          }

        case AvayaMoagentClient.Commands.Logoff.Name:
          {
            _Disconnect();

            var request = _requestManager.GetFirst<LogoffRequest>();
            if (request != null)
            {
              request.MarkComplete();
            }

            if (AgentLoggedOut != null)
            {
              AgentLoggedOut(this, EventArgs.Empty);
            }

            AgentState = Enumerations.AgentState.LoggedOff;

            break;
          }
      }
    }

    private void _SetupDataFields(JoinJobRequest request, WorkClass workClass)
    {
      if (workClass == WorkClass.Outbound || workClass == WorkClass.Managed)
      {
        //Set the primary key for the data fields first
        request.NotifyFields.Enqueue(new KeyValuePair<FieldListType, string>(FieldListType.Outbound, request.CallFields[0].ToUpper()));

        //The others are what we'll receive when we get a call
        foreach (var field in request.CallFields)
        {
          request.DataFields.Enqueue(new KeyValuePair<FieldListType, string>(FieldListType.Outbound, field.ToUpper()));
        }

        if (AllowMultiplePhoneNumbers)
        {
          request.DataFields.Enqueue(new KeyValuePair<FieldListType, string>(FieldListType.Outbound, _FIELD_CURPHONE));
        }
      }
      else if (workClass == WorkClass.Inbound && UseCallFieldsForInbound)
      {
        //Set the primary key for the data fields first
        request.NotifyFields.Enqueue(new KeyValuePair<FieldListType, string>(FieldListType.Inbound, request.CallFields[0].ToUpper()));

        //The others are what we'll receive when we get a call
        foreach (var field in request.CallFields)
        {
          request.DataFields.Enqueue(new KeyValuePair<FieldListType, string>(FieldListType.Inbound, field.ToUpper()));
        }
      }
      else if (workClass == WorkClass.Blend)
      {
        _SetupDataFields(request, WorkClass.Outbound);
        _SetupDataFields(request, WorkClass.Inbound);
      }
    }

    private void _SendNextField(JoinJobRequest request)
    {
      if (request.NotifyFields.Count > 0)
      {
        _SendNextNotifyField(request);
      }
      else if (request.DataFields.Count > 0)
      {
        _SendNextDataField(request);
      }
      else
      {
        _client.Send(AvayaMoagentClient.Commands.AvailableWork.Default);
      }
    }

    private void _SendNextNotifyField(JoinJobRequest request)
    {
      var field = request.NotifyFields.Dequeue();
      _client.Send(new Commands.SetNotifyKeyField(field.Key, field.Value));
    }

    private void _SendNextDataField(JoinJobRequest request)
    {
      var field = request.DataFields.Dequeue();
      _client.Send(new Commands.SetDataField(field.Key, field.Value));
    }
  }
}
