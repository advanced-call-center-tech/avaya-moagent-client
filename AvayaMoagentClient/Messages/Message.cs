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
using System.Text;
using AvayaMoagentClient.Enumerations;

namespace AvayaMoagentClient.Messages
{
  /// <summary>
  /// Message
  /// </summary>
  public class Message
  {
    private const char _RECORD_SEPERATOR = (char)30;
    private const char _END_OF_LINE = (char)3;

    private const string _ORIG_ID = "OrigID";
    private const string _PROCESS_ID = "PrID";
    private const string _INVOKE_ID = "InID";

    /// <summary>
    /// Default constructor
    /// </summary>
    public Message()
    {
      Type = MessageType.Undefined;
      OrigId = _ORIG_ID;
      ProcessId = _PROCESS_ID;
      InvokeId = _INVOKE_ID;
      Contents = new List<string>();
    }

    /// <summary>
    /// Creates a Message using the specified criteria.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="type"></param>
    /// <param name="messageContents"></param>
    public Message(string command, MessageType type, params string[] messageContents)
      : this()
    {
      Command = command;
      Type = type;
      Contents = new List<string>();
      Contents.AddRange(messageContents);
    }

    /// <summary>
    /// Command.
    /// </summary>
    public string Command { get; set; }

    /// <summary>
    /// Message type.
    /// </summary>
    public MessageType Type { get; set; }

    /// <summary>
    /// Originator ID.
    /// </summary>
    public string OrigId { get; set; }

    /// <summary>
    /// Process ID.
    /// </summary>
    public string ProcessId { get; set; }

    /// <summary>
    /// Invoke ID.
    /// </summary>
    public string InvokeId { get; set; }

    /// <summary>
    /// Optional contents/parameters.
    /// </summary>
    public List<string> Contents { get; set; }

    /// <summary>
    /// Raw message string representing this Message to be sent/received.
    /// </summary>
    public string RawMessage
    {
      get
      {
        return _ConstructMessage();
      }

      set
      {
        ParseMessage(value);
      }
    }

    /// <summary>
    /// Indicates whether this message indicates an error.
    /// </summary>
    public bool IsError
    {
      get
      {
        return (Contents.Count > 0 && Contents[0] == "1");
      }
    }

    /// <summary>
    /// Error code, if available.
    /// </summary>
    public string Code
    {
      get
      {
        var ret = string.Empty;

        if (Contents.Count > 1)
        {
          //Codes should only be 6 chars; except in some odd cases where the dialer tacks a message to the end
          //Ex: AGTSystemError      NAgent server        12708 0   2   1E70002,AGTConnHeadset_RESP(MAKECONN)
          ret = Contents[1].Substring(0, Math.Min(Contents[1].Length, 6));
        }

        return ret;
      }
    }

    /// <summary>
    /// Creates the start of a Message from a Command.
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public static Message FromCommand(Commands.Command cmd)
    {
      var ret = new Message();

      ret.Command = cmd.Cmd;
      ret.Type = MessageType.Command;
      ret.Contents.Clear();
      ret.Contents.AddRange(cmd.Contents);

      return ret;
    }

    /// <summary>
    /// Creates a Message from a raw message string.
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static Message ParseMessage(string raw)
    {
      var ret = new Message();

      ret._ParseMessage(raw);

      return ret;
    }

    private string _ConstructMessage()
    {
      var msg = new StringBuilder();
      var msgContents = new StringBuilder();
      var msgContentsSize = 0;

      //Server has an additional flag that indicates if the message is an Error
      //Client does not add this flag
      if (Type != MessageType.Command)
      {
        msgContents.Append(_RECORD_SEPERATOR);
        msgContents.Append(IsError ? "1" : "0");
        msgContentsSize++;
      }

      foreach (var content in Contents)
      {
        msgContents.Append(_RECORD_SEPERATOR);
        msgContents.Append(content);
        msgContentsSize++;
      }

      msg.Append(Command.PadRight(20, ' '));
      msg.Append(((char)Type).ToString().PadRight(1, ' '));
      msg.Append(OrigId.PadRight(20, ' '));
      msg.Append(ProcessId.PadRight(6, ' '));
      msg.Append(InvokeId.PadRight(4, ' '));
      msg.Append(msgContentsSize.ToString().PadRight(4, ' '));
      msg.Append(msgContents);

      msg.Append(_END_OF_LINE);

      return msg.ToString();
    }

    private void _ParseMessage(string raw)
    {
      Command = raw.Substring(0, 20).Trim();
      Type = (MessageType)char.Parse(raw.Substring(20, 1));
      OrigId = raw.Substring(21, 20).Trim();
      ProcessId = raw.Substring(41, 6).Trim();
      InvokeId = raw.Substring(47, 4).Trim();

      Contents.Clear();
      foreach (var data in raw.Substring(55).Replace(_END_OF_LINE.ToString(), string.Empty).Split(_RECORD_SEPERATOR))
      {
        if (!string.IsNullOrEmpty(data))
        {
          Contents.Add(data);
        }
      }
    }
  }
}
