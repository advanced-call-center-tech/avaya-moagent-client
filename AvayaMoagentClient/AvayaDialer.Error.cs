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
    private void _HandleErrorMessage(Message msg)
    {
      const string SYSTEM_ERROR = "AGTSystemError";
      const string HEADSET_CONN_BROKEN = "AGTHeadsetConnBroken";

      switch (msg.Command)
      {
        case SYSTEM_ERROR:
          {
            switch (msg.Code)
            {
              case ErrorCode.RingbackTimeout:
                {
                  if (!IsHeadsetConnected)
                  {
                    var e = new RingbackNotAnsweredEventArgs();
                    if (RingbackNotAnswered != null)
                    {
                      RingbackNotAnswered(this, e);
                    }

                    if (e.TryAgain)
                    {
                      _StartConnectHeadset();
                    }
                    else
                    {
                      if (_headsetConnectionBroken)
                      {
                        GoUnavailable();
                      }
                      else
                      {
                        _client.Send(AvayaMoagentClient.Commands.FreeHeadset.Default);
                      }
                    }
                  }

                  break;
                }

              case ErrorCode.KickedFromDialer:
                {
                  _client.Send(AvayaMoagentClient.Commands.DisconnectHeadset.Default);

                  break;
                }

              default:
                {
                  _RaiseError(msg.Code);

                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.Logon.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.InvalidCredentials:
                {
                  var request = _requestManager.GetFirst<LogonRequest>();
                  request.ErrorCode = msg.Code;
                  request.MarkComplete();

                  break;
                }

              case ErrorCode.AlreadyLoggedIn:
              case ErrorCode.AlreadySignedOn:
              case ErrorCode.ExceededNumberOfAgents:
              case ErrorCode.PasswordExpired:
                {
                  var request = _requestManager.GetFirst<LogonRequest>();
                  request.ErrorCode = msg.Code;
                  request.MarkComplete();

                  _client.Send(AvayaMoagentClient.Commands.Logoff.Default);

                  break;
                }

              default:
                {
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.ReserveHeadset.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.CannotOpenChannelToOpMonProcess:
              case ErrorCode.InvalidHeadsetId:
              case ErrorCode.NoReserveHeadsetRequestPending:
              case ErrorCode.HeadsetIdAlreadyReserved:
              case ErrorCode.NoMoreHeadsetsPermitted:
              case ErrorCode.FailureToAccessHeadsetTable:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.Logoff.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.ConnectHeadset.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.CannotOpenChannelToOpMonProcess:
              case ErrorCode.HeadsetIdNotReservedNorValidated:
              case ErrorCode.NoHeadsetConnectRequestPending:
              case ErrorCode.HeadsetNotConnected:
              case ErrorCode.HeadsetIdNotFoundInResevedList:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.FreeHeadset.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            var request = _requestManager.GetFirst<ConnectHeadsetRequest>();
            if (request != null)
            {
              request.ErrorCode = msg.Code;
              request.MarkComplete();
            }

            break;
          }

        case HEADSET_CONN_BROKEN:
          {
            switch (msg.Code)
            {
              case ErrorCode.HeadsetConnectionBroken:
                {
                  IsHeadsetConnected = false;
                  _headsetConnectionBroken = true;
                  AgentState = Enumerations.AgentState.Idle;

                  break;
                }

              case ErrorCode.HeadsetConnectionReconnected:
                {
                  _headsetConnectionBroken = false;

                  break;
                }

              case ErrorCode.WrongMessageIdReceived:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.SetWorkClass.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.AlreadyAvailableCannotChangeClass:
              case ErrorCode.InvalidWorkClass:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }
                
              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.AttachJob.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.JobNotRunning:
              case ErrorCode.AttachedToJobAlreadyMustDetach:
              case ErrorCode.FailureToOpenJobResourceFile:
              case ErrorCode.FailedToAttachUnitSegment:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.SetNotifyKeyField.Name:
        case AvayaMoagentClient.Commands.SetDataField.Name:
          {
            if (msg.Code == ErrorCode.FieldNameNotFound)
            {
              _RaiseError(msg.Code);

              _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);
            }

            break;
          }

        case AvayaMoagentClient.Commands.AvailableWork.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.JobNotReadyLogOnLater:
              case ErrorCode.MaximumAgentLimitReached:
              case ErrorCode.ManagedAgentsNotPermittedOnJob:
              case ErrorCode.SalesVerificationWithUnitWorkListsNotPermitted:
              case ErrorCode.InboundAgentsOnlyPermitted:
              case ErrorCode.OutboundAgentsOnlyPermitted:
              case ErrorCode.OutboundOrManagedAgentsOnlyPermitted:
              case ErrorCode.OnlyOutboundAgentsPermittedBySalesVerification:
              case ErrorCode.NotAttachedToJob:
              case ErrorCode.HeadsetMustBeActive:
              case ErrorCode.JobNotAvailable:
              case ErrorCode.NoAvailableForWorkRequestPending:
              case ErrorCode.AgentNotAcquiredYet:
              case ErrorCode.ManagedDialingCapableJobsOnly:
              case ErrorCode.MustSelectUnit:
              case ErrorCode.FailedToJoinJob:
                {
                  _RaiseError(msg.Code);

                  _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.TransferCall.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.TransferFailedTryAgainLater:
              case ErrorCode.TelephoneLineNotAvailable:
              case ErrorCode.TelephoneLineNotOffhook:
              case ErrorCode.FeatureNotAvailableInSoftdialerMode:
                {
                  var request = _requestManager.GetFirst<TransferCallRequest>();
                  if (request != null)
                  {
                    request.ErrorCode = msg.Code;
                    request.Result = TransferCallRequest.TransferResult.Error;
                    request.MarkComplete();
                  }

                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.HangUpCall.Name:
          {
            if (msg.Code == ErrorCode.TelephoneLineNotAvailable)
            {
              _client.Send(AvayaMoagentClient.Commands.ListState.Default);

              var request = _requestManager.GetFirst<TransferCallRequest>();
              if (request != null)
              {
                request.ErrorCode = msg.Code;
                request.Result = TransferCallRequest.TransferResult.Error;
                request.MarkComplete();
              }
            }

            break;
          }

        case AvayaMoagentClient.Commands.NoFurtherWork.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.NoJobAttached:
              case ErrorCode.NotAvailableForWorkOnJob:
              case ErrorCode.AgentNotAllowedToLogoff:
                {
                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.ReleaseLine.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.CallCancelNotPermitted:
                {
                  var request = _requestManager.GetFirst<ReleaseLineRequest>();
                  if (request != null)
                  {
                    request.ErrorCode = msg.Code;
                    request.MarkComplete();
                  }

                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              case ErrorCode.TelephoneLineNotAvailable:
              case ErrorCode.NotAttachedToJob:
                {
                  _client.Send(AvayaMoagentClient.Commands.ListState.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.DisconnectHeadset.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.HeadsetIdNotReservedNorValidated:
              case ErrorCode.HeadsetNotConnected:
              case ErrorCode.WrongMessageIdReceived:
                {
                  _client.Send(AvayaMoagentClient.Commands.FreeHeadset.Default);
                  
                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.FreeHeadset.Name:
          {
            switch (msg.Code)
            {
              case ErrorCode.HeadsetIdNotReservedNorValidated:
              case ErrorCode.HeadsetNotDisconnected:
                {
                  _client.Send(AvayaMoagentClient.Commands.Logoff.Default);

                  break;
                }

              default:
                {
                  //Do nothing
                  break;
                }
            }

            break;
          }

        case AvayaMoagentClient.Commands.Logoff.Name:
          {
            if (msg.Code == ErrorCode.JobAttachedMustDetachFirst)
            {
              var request = _requestManager.GetFirst<LogoffRequest>();
              if (request != null)
              {
                request.ErrorCode = msg.Code;
                request.MarkComplete();
              }
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

    private void _RaiseError(string errorCode)
    {
      if (DialerErrorReceived != null)
      {
        DialerErrorReceived(this, new DialerErrorReceivedEventArgs(errorCode));
      }
    }
  }
}
