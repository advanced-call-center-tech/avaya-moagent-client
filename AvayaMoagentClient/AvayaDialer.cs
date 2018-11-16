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
using AvayaMoagentClient.Commands;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaMoagentClient
{
  /// <summary>
  /// AvayaDialer
  /// </summary>
  public class AvayaDialer
  {
    private MoagentClient _client;
    private string _host;
    private int _port;
    private bool _useSsl;

    /// <summary>
    /// Creates an AvayaDialer object with the specified criteria.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="useSsl"></param>
    public AvayaDialer(string host, int port, bool useSsl)
    {
      _host = host;
      _port = port;
      _useSsl = useSsl;
    }

    /// <summary>
    /// MessageSent
    /// </summary>
    public event MoagentClient.MessageSentHandler MessageSent;

    /// <summary>
    /// MessageReceived
    /// </summary>
    public event MoagentClient.MessageReceivedHandler MessageReceived;

    /// <summary>
    /// Disconnected
    /// </summary>
    public event MoagentClient.DisconnectedHandler Disconnected;

    /// <summary>
    /// IsConnected
    /// </summary>
    public bool IsConnected
    {
      get
      {
        return _client.IsConnected;
      }
    }

    /// <summary>
    /// Connect
    /// </summary>
    public void Connect()
    {
      _client = new MoagentClient(_host, _port, _useSsl);
      _client.MessageSent += _client_MessageSent;
      _client.MessageReceived += _client_MessageReceived;
      _client.Disconnected += _client_Disconnected;
      _client.StartConnect();
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    public void Login(string userName, string password)
    {
     _client.Send(new Logon(userName, password)); 
    }

    /// <summary>
    /// ReserveHeadset
    /// </summary>
    /// <param name="extension"></param>
    public void ReserveHeadset(string extension)
    {
      _client.Send(new ReserveHeadset(extension));
    }

    /// <summary>
    /// ConnectHeadset
    /// </summary>
    public void ConnectHeadset()
    {
      _client.Send(Commands.ConnectHeadset.Default);
    }

    /// <summary>
    /// ListState
    /// </summary>
    public void ListState()
    {
      _client.Send(Commands.ListState.Default);
    }

    /// <summary>
    /// ListJobs
    /// </summary>
    public void ListJobs()
    {
      _client.Send(Commands.ListJobs.All);
    }

    /// <summary>
    /// AttachJob
    /// </summary>
    /// <param name="jobName"></param>
    public void AttachJob(string jobName)
    {
      _client.Send(new AttachJob(jobName));
    }

    /// <summary>
    /// SetWorkClass
    /// </summary>
    /// <param name="workClass"></param>
    public void SetWorkClass(WorkClass workClass)
    {
      _client.Send(new SetWorkClass(workClass));
    }

    /// <summary>
    /// SetNotifyKeyField
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fieldName"></param>
    public void SetNotifyKeyField(FieldListType type, string fieldName)
    {
      _client.Send(new SetNotifyKeyField(type, fieldName)); 
    }

    /// <summary>
    /// SetDataField
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fieldName"></param>
    public void SetDataField(FieldListType type, string fieldName)
    {
      _client.Send(new SetDataField(type, fieldName));
    }

    /// <summary>
    /// SetPassword
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="presentPassword"></param>
    /// <param name="newPassword"></param>
    public void SetPassword(string userId, string presentPassword, string newPassword)
    {
      _client.Send(new SetPassword(userId, presentPassword, newPassword));
    }

    /// <summary>
    /// AvailableWork
    /// </summary>
    public void AvailableWork()
    {
      _client.Send(Commands.AvailableWork.Default);
    }

    /// <summary>
    /// ReadyNextItem
    /// </summary>
    public void ReadyNextItem()
    {
      _client.Send(Commands.ReadyNextItem.Default);
    }

    /// <summary>
    /// FinishItem
    /// </summary>
    /// <param name="completionCode"></param>
    public void FinishItem(string completionCode)
    {
      _client.Send(new FinishedItem(completionCode));
    }

    /// <summary>
    /// HangUpCall
    /// </summary>
    public void HangUpCall()
    {
      _client.Send(Commands.HangUpCall.Default);
    }

    /// <summary>
    /// ReleaseLine
    /// </summary>
    public void ReleaseLine()
    {
      _client.Send(Commands.ReleaseLine.Default);
    }

    /// <summary>
    /// NoFurtherWork
    /// </summary>
    public void NoFurtherWork()
    {
      _client.Send(Commands.NoFurtherWork.Default);
    }

    /// <summary>
    /// DetachJob
    /// </summary>
    public void DetachJob()
    {
      _client.Send(Commands.DetachJob.Default);
    }
    
    /// <summary>
    /// ListActiveJobs
    /// </summary>
    public void ListActiveJobs()
    {
      _client.Send(new ListJobs(JobListingType.All, JobStatus.Active));
    }

    /// <summary>
    /// DisconnectHeadset
    /// </summary>
    public void DisconnectHeadset()
    {
      _client.Send(Commands.DisconnectHeadset.Default);
    }

    /// <summary>
    /// Logoff
    /// </summary>
    public void Logoff()
    {
      _client.Send(Commands.Logoff.Default);
    }

    /// <summary>
    /// Disconnect
    /// </summary>
    public void Disconnect()
    {
      _client.Disconnect();
      _client.MessageSent -= _client_MessageSent;
      _client.MessageReceived -= _client_MessageReceived;
      _client.Disconnected -= _client_Disconnected;
      _client = null;
    }

    /// <summary>
    /// FreeHeadset
    /// </summary>
    public void FreeHeadset()
    {
      _client.Send(Commands.FreeHeadset.Default);
    }

    /// <summary>
    /// TransferCall
    /// </summary>
    public void TransferCall()
    {
      _client.Send(Commands.TransferCall.Default);
    }

    /// <summary>
    /// TransferCall
    /// </summary>
    /// <param name="transferNumber"></param>
    public void TransferCall(string transferNumber)
    {
      _client.Send(new TransferCall(transferNumber));
    }

    /// <summary>
    /// ManagedCall
    /// </summary>
    public void ManagedCall()
    {
      _client.Send(Commands.ManagedCall.Default);
    }

    /// <summary>
    /// ManualCall
    /// </summary>
    public void ManualCall()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// DialDigit
    /// </summary>
    /// <param name="digit"></param>
    public void DialDigit(string digit)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// SetCallback
    /// </summary>
    /// <param name="callbackDate"></param>
    /// <param name="callbackTime"></param>
    /// <param name="phoneIndex"></param>
    /// <param name="recallName"></param>
    /// <param name="recallNumber"></param>
    public void SetCallback(string callbackDate, string callbackTime, string phoneIndex, string recallName, 
        string recallNumber)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// SendCommand
    /// </summary>
    /// <param name="command"></param>
    public void SendCommand(Command command)
    {
      _client.Send(command);
    }

    /// <summary>
    /// SendMessage
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(Message message)
    {
      _client.Send(message);
    }

    private void _client_MessageSent(object sender, MessageSentEventArgs e)
    {
      if (MessageSent != null)
      {
        MessageSent(this, e);
      }
    }

    private void _client_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      if (MessageReceived != null)
      {
        MessageReceived(this, e);
      }
    }

    private void _client_Disconnected(object sender, EventArgs e)
    {
      if (Disconnected != null)
      {
        Disconnected(this, e);
      }
    }
  }
}
