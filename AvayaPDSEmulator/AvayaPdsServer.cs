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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using AvayaMoagentClient;

namespace AvayaPDSEmulator
{
  //State object for reading client data asynchronously
  public class StateObject
  {
    public const int READ_BUFFER_SIZE = 1024;

    public Guid Id = Guid.NewGuid();
    public Socket WorkSocket;
    public byte[] Buffer = new byte[READ_BUFFER_SIZE];
    public StringBuilder Message = new StringBuilder();
    public string CurrentState = "S70004"; //logged on, idle, not attached to job
    public string CurrentJob = string.Empty;
    public bool IsLeavingJob;
    public bool IsDisconnecting;
  }

  public class Job
  {
    public enum JobType
    {
      Inbound = 'I',
      Outbound = 'O',
      Managed = 'M',
      Blend = 'B'
    }

    public JobType Type { get; set; }
    public string JobName { get; set; }

    public Job(JobType type, string name)
    {
      Type = type;
      JobName = name;
    }
  }

  public class AvayaPdsServer
  {
    private const int _PORT_NUMBER = 22700;
    private const int _BACKLOG = 100;
    private ManualResetEvent _allDone = new ManualResetEvent(false);
    private Thread _listenThread = null;
    private List<Job> _jobs = new List<Job>();
    
    public Dictionary<Guid, StateObject> States = new Dictionary<Guid, StateObject>();

    public AvayaPdsServer()
    {
      _jobs.Add(new Job(Job.JobType.Outbound, "GEO_HM1"));
      _jobs.Add(new Job(Job.JobType.Outbound, "GEO_HM2"));
    }

    public void StartListening()
    {
      if (_listenThread == null)
      {
        _listenThread = new Thread(new ThreadStart(_Listen));
        _listenThread.IsBackground = true;
        _listenThread.Start();
      }
    }

    public void StopListening()
    {
      if (_listenThread != null)
      {
        _listenThread.Abort();
        _listenThread = null;
      }
    }

    private void _Listen()
    {
      //Create TCP/IP socket
      var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        //Bind the socket to the local endpoint and listen for incoming connections
        listener.Bind(new IPEndPoint(IPAddress.Any, _PORT_NUMBER));
        listener.Listen(_BACKLOG);

        while (true)
        {
          _allDone.Reset();

          Logging.LogEvent("Waiting for a connection...");
          listener.BeginAccept(new AsyncCallback(_AcceptClientConnection), listener);

          //Wait until a connection is made before continuing
          _allDone.WaitOne();
        }

      }
      catch (ThreadAbortException)
      {
        //Ignore it
      }
      catch (Exception e)
      {
        Logging.LogEvent("Error listening for incoming connection: " + e.ToString());
      }
    }

    private void _AcceptClientConnection(IAsyncResult ar)
    {
      //Signal the main thread to continue
      _allDone.Set();

      //Get the socket for the client request
      var listener = (Socket)ar.AsyncState;
      var handler = listener.EndAccept(ar);

      //Create the state object
      var state = new StateObject { WorkSocket = handler };
      States.Add(state.Id, state);
      Logging.LogEvent(string.Format("New connection accepted, state ID '{0}'", state.Id));

      //Send the initial message
      _SendMessageToClient(handler,
                    new Message
                    {
                      Command = "AGTSTART",
                      Type = Message.MessageType.Notification,
                      OrigId = "Agent server",
                      ProcessId = "26621",
                      InvokeId = "0",
                      Contents = new List<string> { "AGENT_STARTUP" }
                    });

      //Start receiving from the client
      _BeginReceiveFromClient(state);
    }

    private void _BeginReceiveFromClient(StateObject state)
    {
      if (!state.IsDisconnecting)
      {
        state.WorkSocket.BeginReceive(state.Buffer, 0, StateObject.READ_BUFFER_SIZE, 0,
          new AsyncCallback(_ReceiveFromClient), state);
      }
      else
      {
        _Disconnect(state);
      }
    }

    private void _ReceiveFromClient(IAsyncResult ar)
    {
      string content;

      try
      {
        //Retrieve the state object and the handler socket
        //  from the asynchronous state object
        var state = (StateObject)ar.AsyncState;
        var handler = state.WorkSocket;

        //Read data from the client socket
        var bytesRead = handler.EndReceive(ar);
        if (bytesRead > 0)
        {
          // There might be more data, so store the data received so far
          state.Message.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

          //Check for end-of-file tag; if not there, read more data
          content = state.Message.ToString();
          if (content.IndexOf((char)3) > -1)
          {
            var messages = new List<string>();
            var message = new StringBuilder();

            foreach (var ch in content)
            {
              if (ch == (char)3)
              {
                message.Append(ch);
                messages.Add(message.ToString());
                message.Length = 0;
              }
              else
              {
                message.Append(ch);
              }
            }

            state.Message.Length = 0;
            if (message.ToString().IndexOf((char)3) > -1)
            {
              state.Message.Append(message.ToString());
            }

            _HandleClientMessages(state, messages);
          }
          else
          {
            //Not all data received, so get more
            handler.BeginReceive(state.Buffer, 0, StateObject.READ_BUFFER_SIZE, 0,
              new AsyncCallback(_ReceiveFromClient), state);
          }
        }
      }
      catch (SocketException ex)
      {
        Logging.LogEvent("Socket error receiving data from client: " + ex.ToString());

        _Disconnect((StateObject)ar.AsyncState);
      }
      catch (IOException ex)
      {
        //Something in the transport layer has failed, such as the network connection died
        Logging.LogEvent("I/O error receiving data from client: " + ex.ToString());

        _Disconnect((StateObject)ar.AsyncState);
      }
      catch (ObjectDisposedException ex)
      {
        //We've been disconnected
        Logging.LogEvent("Error receiving data from client: " + ex.ToString());

        _Disconnect((StateObject)ar.AsyncState);
      }
      catch (Exception ex)
      {
        Logging.LogEvent("Unexpected error receiving data from client: " + ex.ToString());

        Debugger.Break();
      }
    }

    private void _HandleClientMessages(StateObject state, IEnumerable<string> data)
    {
      foreach (var msg in data)
      {
        if (msg.StartsWith("INBOUND") || msg.StartsWith("OUTBOUND") || msg.StartsWith("MANAGED") ||
          msg.StartsWith("TRANS"))
        {
          //Blast the handling of this command as though it came from everyone
          foreach (var conn in States.Values)
          {
            if (conn.Id != state.Id && (conn.CurrentState == "S70001"))
            {
              _HandleMessageFromClient(conn, msg);
            }
          }
        }
        else
        {
          //Not a special case, so just handle it
          _HandleMessageFromClient(state, msg);
        }
      }

      //Prepare to receive the next message
      _BeginReceiveFromClient(state);
    }

    private void _HandleMessageFromClient(StateObject state, string data)
    {
      var handler = state.WorkSocket;
      Logging.LogEvent(string.Format("Receiving message ({0}): {1}", DateTime.Now, data));

      var m = Message.ParseMessage(data);
      switch (m.Command.Trim())
      {
        case "OUTBOUND":
          {
            state.CurrentState = "S70000";

            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTCallNotify",
                              Type = Message.MessageType.Notification,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = m.Contents.Take(4).ToList()
                            });

            var contents = m.Contents.Take(1).ToList();
            contents.AddRange(m.Contents.Skip(3));
            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTCallNotify",
                              Type = Message.MessageType.Notification,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = contents.ToList()
                            });

            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTCallNotify",
                              Type = Message.MessageType.Notification,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = new List<string> { "M00000" }
                            });
            break;
          }

        case "MANAGED":
          {
            state.CurrentState = "S70000";

            _SendMessageToClient(handler,
                          new Message
                          {
                            Command = "AGTPreviewRecord",
                            Type = Message.MessageType.Notification,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = m.Contents.Take(4).ToList()
                          });

            var contents = m.Contents.Take(1).ToList();
            contents.AddRange(m.Contents.Skip(3));
            _SendMessageToClient(handler,
                          new Message
                          {
                            Command = "AGTPreviewRecord",
                            Type = Message.MessageType.Notification,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = contents
                          });
            _SendMessageToClient(handler,
                          new Message
                          {
                            Command = "AGTPreviewRecord",
                            Type = Message.MessageType.Notification,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });

            Thread.Sleep(5000);

            _SendMessageToClient(handler,
                          new Message
                          {
                            Command = "AGTManagedCall",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });

            Thread.Sleep(7000);

            _SendMessageToClient(handler,
                          new Message
                          {
                            Command = "AGTManagedCall",
                            Type = Message.MessageType.Data,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00001", "(CONNECT)" }
                          });
            _SendMessageToClient(handler,
                new Message
                {
                  Command = "AGTManagedCall",
                  Type = Message.MessageType.Response,
                  OrigId = "Agent server",
                  ProcessId = "26621",
                  InvokeId = "0",
                  Contents = new List<string> { "M00000" }
                });
            break;
          }

        case "INBOUND":
          state.CurrentState = "S70000";

          _SendMessageToClient(handler,
                        new Message
                        {
                          Command = "AGTCallNotify",
                          Type = Message.MessageType.Notification,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          Contents = new List<string> { "M00001", "INBOUND CALL * 11-20 SECS. WAITING", "INBOUND" }
                        });
          _SendMessageToClient(handler,
                        new Message
                        {
                          Command = "AGTCallNotify",
                          Type = Message.MessageType.Notification,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          Contents = new List<string> { "M00000" }
                        });
          break;

        case "TRANS":
          _SendMessageToClient(handler,
            new Message
            {
              Command = "AGTJobTransLink",
              Type = Message.MessageType.Notification,
              OrigId = "Agent server",
              ProcessId = "26621",
              InvokeId = "0",
              Contents = new List<string> { "M00001", m.Contents[0] }
            });
          _SendMessageToClient(handler,
            new Message
            {
              Command = "AGTJobTransLink",
              Type = Message.MessageType.Notification,
              OrigId = "Agent server",
              ProcessId = "26621",
              InvokeId = "0",
              Contents = new List<string> { "M00000" }
            });
          break;

        case "SETJOBS":
          _jobs.Clear();
          m.Contents.ForEach(t =>
            {
              var parts = t.Split(new char[] { ',' });

              _jobs.Add(new Job((Job.JobType)parts[0][0], parts[1]));
            });
          break;

        case "AGTLogon":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTLogon",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTLogon",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTReserveHeadset":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTReserveHeadset",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });

          Thread.Sleep(500);

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTReserveHeadset",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTConnHeadset":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTConnHeadset",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });

          Thread.Sleep(500);

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTConnHeadset",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTListState":
          string content;

          //If on a job, add the job name to the message
          if (state.CurrentState != "S70004")
            content = state.CurrentState + "," + state.CurrentJob;
          else
            content = state.CurrentState;

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTListState",
                            Type = Message.MessageType.Data,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { content }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTListState",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTSetWorkClass":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTSetWorkClass",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTAttachJob":
          state.CurrentState = "S70003";
          state.CurrentJob = m.Contents[0];

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTAttachJob",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTSetNotifyKeyField":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTSetNotifyKeyField",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTSetDataField":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTSetDataField",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTAvailWork":
          state.CurrentState = "S70002";

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTAvailWork",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTAvailWork",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTTransferCall":
          _SendMessageToClient(handler,
                        new Message
                        {
                          Command = "AGTTransferCall",
                          Type = Message.MessageType.Response,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          Contents = new List<string> { "M00000" }
                        });
          break;

        case "AGTReleaseLine":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTReleaseLine",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTReleaseLine",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTReadyNextItem":
          if (state.CurrentState == "S70004")
          {
            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTReadyNextItem",
                              Type = Message.MessageType.Response,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              //IsError = true,
                              Contents = new List<string> { "E28885" }
                            });
          }
          else
          {
            state.CurrentState = "S70001";

            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTReadyNextItem",
                              Type = Message.MessageType.Response,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = new List<string> { "M00000" }
                            });
          }
          break;

        case "AGTFinishedItem":
          state.CurrentState = "S70002";

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTFinishedItem",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTFinishedItem",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });

          if (state.IsLeavingJob)
          {
            state.CurrentState = "S70003";

            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTNoFurtherWork",
                              Type = Message.MessageType.Response,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = new List<string> { "M00000" }
                            });
          }
          break;

        case "AGTNoFurtherWork":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTNoFurtherWork",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });

          if (state.CurrentState != "S70000")
          {
            state.CurrentState = "S70003";

            _SendMessageToClient(handler,
                          new Message
                            {
                              Command = "AGTNoFurtherWork",
                              Type = Message.MessageType.Response,
                              OrigId = "Agent server",
                              ProcessId = "26621",
                              InvokeId = "0",
                              Contents = new List<string> { "M00000" }
                            });
          }
          else
          {
            state.IsLeavingJob = true;
          }
          break;

        case "AGTDetachJob":
          state.CurrentState = "S70004";
          state.CurrentJob = string.Empty;
          state.IsLeavingJob = false;

          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTDetachJob",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });

          break;
        case "AGTDisconnHeadset":
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTDisconnHeadset",
                            Type = Message.MessageType.Pending,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "S28833" }
                          });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTDisconnHeadset",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTFreeHeadset":
          _SendMessageToClient(handler,
                        new Message
                        {
                          Command = "AGTFreeHeadset",
                          Type = Message.MessageType.Response,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          Contents = new List<string> { "M00000" }
                        });
          break;

        case "AGTListJobs":
          var jobs = _jobs.Select(t => (char)t.Type + "," + t.JobName + ",A").ToList();
          jobs.Insert(0, "M00001");

          _SendMessageToClient(handler,
                        new Message
                          {
                          Command = "AGTListJobs",
                          Type = Message.MessageType.Data,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          //Contents = new List<string> { "M00001", "O,30DHOP1,I", "O,30DHOP2,I", "O,30HOHiP1,I", "O,30HOHiP2,I", "O,5BIHOP1,I", "O,5BIHOP2,I", "B,ACT_blend,I", "O,ACT_outbnd,I", "O,ALW_C1T3SL,I", "O,ALW_C7S1SL,A", "O,ATTE_C1S1,I", "O,ATTE_C1S2,I", "O,ATTE_C1S3,I", "O,ATTE_C1S5,I", "O,ATTE_C1SP,I", "O,ATTE_C1W1,I", "O,AutoTest,I", "B,BLENDCOPY,I", "B,BlendTst,I", "B,GE_JCALLP5,I", "I,InbClosed,I", "O,Matttest,I", "O,NS_OB,I", "O,SX_MSPP1,I", "O,SX_MSPP2,I", "O,SX_MSPWCP1,I", "O,SX_MSPWCP2,I", "O,SX_Mod1,I", "O,SX_Mod1_2,I", "O,SX_ModSkp,I", "I,SallieINB,I", "B,SallieLO,A", "B,SallieSLM,I", "O,Sallie_AM,I", "B,Sallie_Dev,I", "O,SaxCol2LN1,I", "M,SaxCol2LN2,I", "O,SaxColLPS1,I", "O,SaxColLPS2,I", "M,SaxColLST1,I", "O,SaxColLST2,I", "O,SaxColMSPW,I", "O,SaxColPEP1,I", "O,SaxColPEP2,I", "M,SaxColPR31,I", "M,SaxColPR32,I", "M,SaxColPR61,I", "M,SaxColPR62,I", "O,SaxCol_121,I", "O,SaxCol_122,I", "O,SaxCol_31,I", "O,SaxCol_32,I", "O,SaxCol_61,I", "O,SaxCol_62,I", "O,SaxCol_91,I", "O,SaxCol_92,I", "O,SaxCol_FC1,I", "O,SaxCol_FC2,I", "M,SaxCol_L31,I", "M,SaxCol_L32,I", "O,SaxCol_L61,I", "O,SaxCol_L62,I", "M,SaxDev,I", "M,SaxonArm,I", "M,SaxonEsc,I", "O,SaxonII_1,I", "O,SaxonII_2,I", "O,SxonII_Dev,I", "O,UVRE_C7S1,I", "O,UVRSE_C7S1,I", "O,UVRSW_C7S1,A", "O,UVRW_C7S1,A", "O,Uvrs_Dev,I", "B,blend,I", "I,inbnd1,I", "M,managed,I", "O,outbnd,I" }
                          Contents = jobs
                          //Contents = new List<string> { "M00001", "O,SallieLO,A", "O,JOB2,A" }
                        });
          _SendMessageToClient(handler,
                        new Message
                          {
                            Command = "AGTListJobs",
                            Type = Message.MessageType.Response,
                            OrigId = "Agent server",
                            ProcessId = "26621",
                            InvokeId = "0",
                            Contents = new List<string> { "M00000" }
                          });
          break;

        case "AGTLogoff":
          state.IsDisconnecting = true;

          _SendMessageToClient(handler,
                        new Message
                        {
                          Command = "AGTLogoff",
                          Type = Message.MessageType.Response,
                          OrigId = "Agent server",
                          ProcessId = "26621",
                          InvokeId = "0",
                          Contents = new List<string> { "M00000" }
                        });
          break;
      }
    }

    private void _SendMessageToClient(Socket sock, Message msg)
    {
      var writer = new StreamWriter(new NetworkStream(sock, true));
      var raw = msg.RawMessage;

      Logging.LogEvent(string.Format("Sending message ({0}): {1}", DateTime.Now, raw));
      writer.Write(raw);
      writer.Flush();
      Thread.Sleep(50);
    }

    private void _Disconnect(StateObject state)
    {
      if (state != null)
      {
        if (States.ContainsKey(state.Id))
        {
          States.Remove(state.Id);
        }

        if (state.WorkSocket != null && state.WorkSocket.Connected)
        {
          state.WorkSocket.Close(2);
        }
      }
    }
  }
}
