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
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AvayaMoagentClient;
using AvayaMoagentClient.Messages;

namespace AvayaPDSEmulator
{
  /// <summary>
  /// StateObject
  /// </summary>
  public class DialerConnection
  {
    public const int ReadBufferSize = 1024;

    /// <summary>
    /// Default constructor
    /// </summary>
    public DialerConnection()
    {
      Id = Guid.NewGuid();
      Buffer = new byte[ReadBufferSize];
      Message = new StringBuilder();
      CurrentState = AvayaDialer.STATUS_NOT_ON_JOB;
      CurrentJob = string.Empty;
    }

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// WorkSocket
    /// </summary>
    public Socket WorkSocket { get; set; }

    /// <summary>
    /// Buffer
    /// </summary>
    public byte[] Buffer { get; private set; }

    /// <summary>
    /// Message
    /// </summary>
    public StringBuilder Message { get; private set; }

    /// <summary>
    /// CurrentState
    /// </summary>
    public string CurrentState { get; set; }

    /// <summary>
    /// CurrentJob
    /// </summary>
    public string CurrentJob { get; set; }

    /// <summary>
    /// IsLeavingJob
    /// </summary>
    public bool IsLeavingJob { get; set; }

    /// <summary>
    /// IsDisconnecting
    /// </summary>
    public bool IsDisconnecting { get; set; }

    public void SendToClient(Message msg)
    {
      var writer = new StreamWriter(new NetworkStream(WorkSocket, true));
      var raw = msg.RawMessage;

      Logging.LogEvent(string.Format("Sending message ({0}): {1}", DateTime.Now, raw));
      writer.Write(raw);
      writer.Flush();
      Thread.Sleep(50);
    }
  }
}
