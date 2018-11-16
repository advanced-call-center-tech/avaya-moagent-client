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
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvayaMoagentClient;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaTestClient.ViewModels
{
  /// <summary>
  /// MainViewModel
  /// </summary>
  public class MainViewModel : INotifyPropertyChanged
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public MainViewModel()
    {
      //Avaya = new AvayaDialer("192.168.16.13", 22700);
      Avaya = new AvayaDialer("192.168.80.78", 22700, true);
      Avaya.MessageReceived += _avaya_MessageReceived;
      Avaya.MessageSent += Avaya_MessageSent;
      Messages = new ObservableCollection<Message>();

      UIAction = ((uiAction) => uiAction());
    }

    /// <summary>
    /// PropertyChanged
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Avaya
    /// </summary>
    public AvayaDialer Avaya { get; protected set; }

    /// <summary>
    /// Messages
    /// </summary>
    public ObservableCollection<Message> Messages { get; set; }

    /// <summary>
    /// UIAction
    /// </summary>
    public Action<Action> UIAction { get; set; }

    private void Avaya_MessageSent(object sender, MessageSentEventArgs e)
    {
      UIAction(() => Messages.Insert(0, e.Message));
    }

    private void _avaya_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      UIAction(() => Messages.Insert(0, e.Message));

      switch (e.Message.Type)
      {
        case MessageType.Command:
          break;
        case MessageType.Pending:
          break;
        case MessageType.Data:
          break;
        case MessageType.Response:
          switch (e.Message.Command.Trim())
          {
            case "AGTLogon":
              //Avaya.ReserveHeadset("1");
              break;
            case "AGTReserveHeadset":
              //Avaya.ConnectHeadset();
              break;
            case "AGTConnHeadset":
              //Avaya.ListState();
              break;
            ////case "AGTListState":
            ////  Avaya.DisconnectHeadset();
            ////  break;
            ////case "AGTDisconnHeadset":
            ////  Avaya.SendCommand(new Message("AGTFreeHeadset", Message.MessageType.Command));
            ////  break;
            ////case "AGTFreeHeadset":
            ////  Avaya.Logoff();
            ////  break;
            ////case "AGTLogoff":
            ////  Avaya.Disconnect();
            ////  break;
           }

          break;
        case MessageType.Busy:
          break;
        case MessageType.Notification:
          switch (e.Message.Command.Trim())
          {
            case "AGTSTART":
              //Avaya.Login("m9057","mlitt001");
              break;
          }

          break;
        case MessageType.Undefined:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
