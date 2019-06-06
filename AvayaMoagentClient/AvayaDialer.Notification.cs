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

namespace AvayaMoagentClient
{
  /// <summary>
  /// AvayaDialer
  /// </summary>
  public partial class AvayaDialer
  {
    private void _HandleNotificationMessage(Message msg)
    {
      switch (msg.Command)
      {
        case Notification.Start:
          {
            AgentState = Enumerations.AgentState.LoggingOn;

            _client.Send(new AvayaMoagentClient.Commands.Logon(_userName, Utilities.ToUnsecureString(_password)));

            break;
          }

        case Notification.CallNotify:
          {
            switch (msg.Code)
            {
              case AvayaDialer.CODE_ADDITIONAL_DATA:
                {
                  if (CurrentCallData == null)
                  {
                    CurrentCallData = new CallData();
                    _ProcessCallNotify(msg.Contents[2]);
                  }
                  else
                  {
                    _ProcessCallData(msg.Contents);
                  }

                  break;
                }

              case AvayaDialer.CODE_COMPLETE:
                {
                  //Officially on the call now
                  JobStage = Enumerations.JobStage.OnCall;
                  AgentState = Enumerations.AgentState.OnCall;

                  break;
                }

              default:
                {
                  //Uh...ignore, I guess...
                  break;
                }
            }

            break;
          }

        case Notification.PreviewRecord:
          {
            JobStage = Enumerations.JobStage.PreviewingRecord;
            AgentState = Enumerations.AgentState.Preview;

            switch (msg.Code)
            {
              case CODE_ADDITIONAL_DATA:
                {
                  if (CurrentCallData == null)
                  {
                    CurrentCallData = new CallData();
                    _ProcessCallNotify(msg.Contents[2]);
                  }
                  else
                  {
                    _ProcessCallData(msg.Contents);
                  }

                  break;
                }

              case CODE_COMPLETE:
                {
                  //Nothing to do
                  break;
                }

              default:
                {
                  //Uh...
                  break;
                }
            }

            break;
          }

        case Notification.AutoReleaseLine:
        case Notification.TransferCustHangup:
          {
            JobStage = Enumerations.JobStage.Wrap;
            AgentState = Enumerations.AgentState.OnCall;

            break;
          }

        case Notification.JobTransferRequest:
        case Notification.JobTransferLink:
          {
            switch (msg.Code)
            {
              case CODE_ADDITIONAL_DATA:
                {
                  _nextJobName = msg.Contents[2];

                  break;
                }

              case CODE_COMPLETE:
                {
                  AgentState = Enumerations.AgentState.JobTransfer;
                  _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);

                  break;
                }

              default:
                {
                  //Uh...I don't know...
                  break;
                }
            }

            break;
          }

        case Notification.JobEnd:
          {
            _jobEnded = true;
            AgentState = Enumerations.AgentState.GoUnavailable;

            _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);

            break;
          }

        default:
          {
            //Uh...I don't know...
            break;
          }
      }
    }

    private void _ProcessCallNotify(string input)
    {
      if (input.IndexOf("INBOUND") >= 0)
      {
        CurrentCallData.DialerMode = DialerMode.Inbound;
      }
      else
      {
        if (input.IndexOf("Recall") > 0)
        {
          CurrentCallData.DialerMode = DialerMode.OutboundRecall;
        }
        else
        {
          CurrentCallData.DialerMode = DialerMode.Outbound;
        }

        var fields = input.Split(' ');

        //If multiple phone numbers are possible, the phone number will get parsed in the other event
        if (!AllowMultiplePhoneNumbers)
        {
          //Get the first one
          foreach (var field in fields)
          {
            var tmp = field.Replace("-", string.Empty).Trim();

            if (tmp.Length == 10)
            {
              CurrentCallData.NumberDialed = tmp;
              break;
            }
          }
        }
      }

      if (CurrentCallData.DialerMode == DialerMode.Inbound && !UseCallFieldsForInbound)
      {
        //No fields defined for inbound, so go ahead and raise the event
        if (CallConnected != null)
        {
          CallConnected(this, EventArgs.Empty);
        }
      }
    }

    private void _ProcessCallData(List<string> fields)
    {
      var data = new Dictionary<string, string>();

      for (int i = 2; i < fields.Count; i++)
      {
        var field = fields[i].Split(',');
        data.Add(field[0], field[1]);
      }

      //Try to get the phone number from the dialed field, or last ditch effort for cases like recall
      if (AllowMultiplePhoneNumbers || string.IsNullOrEmpty(CurrentCallData.NumberDialed))
      {
        if (data.ContainsKey(_FIELD_CURPHONE))
        {
          //NOTE: This will choke on PHONE10
          var phoneField = "PHONE" + data[_FIELD_CURPHONE].Replace("0", string.Empty);

          if (data.ContainsKey(phoneField))
          {
            CurrentCallData.NumberDialed = Utilities.DigitsOnly(data[phoneField]);
          }
          else
          {
            if (data.ContainsKey(_FIELD_DEFAULT_PHONE))
            {
              CurrentCallData.NumberDialed = Utilities.DigitsOnly(data[_FIELD_DEFAULT_PHONE]);
            }
          }
        }
      }

      CurrentCallData.Data = data;
      if (CallConnected != null)
      {
        CallConnected(this, EventArgs.Empty);
      }
    }
  }
}
