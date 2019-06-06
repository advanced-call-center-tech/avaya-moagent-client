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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaPDSEmulator
{
  /// <summary>
  /// AvayaPdsServer
  /// </summary>
  [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
  public abstract class DialerServer
  {
    public const int DEFAULT_PORT_NUMBER = 22700;
    private const int _BACKLOG = 100;
    private int _portNumber = DEFAULT_PORT_NUMBER;
    private ManualResetEvent _allDone = new ManualResetEvent(false);
    private Thread _listenThread = null;

    /// <summary>
    /// Default constructor
    /// </summary>
    public DialerServer()
    {
      Connections = new Dictionary<Guid, DialerConnection>();
    }

    /// <summary>
    /// PortNumber
    /// </summary>
    public int PortNumber
    {
      get
      {
        return _portNumber;
      }

      set
      {
        if (_listenThread != null)
        {
          throw new InvalidOperationException("Cannot change port number while listening.");
        }

        _portNumber = value;
      }
    }

    /// <summary>
    /// Connections
    /// </summary>
    public Dictionary<Guid, DialerConnection> Connections { get; private set; }

    /// <summary>
    /// StartListening
    /// </summary>
    public void StartListening()
    {
      if (_listenThread == null)
      {
        _listenThread = new Thread(new ThreadStart(_Listen));
        _listenThread.IsBackground = true;
        _listenThread.Start();
      }
    }

    /// <summary>
    /// StopListening
    /// </summary>
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
        listener.Bind(new IPEndPoint(IPAddress.Any, PortNumber));
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
      finally
      {
        listener.Close();
      }
    }

    private void _AcceptClientConnection(IAsyncResult ar)
    {
      try
      {
        //Signal the main thread to continue
        _allDone.Set();

        //Get the socket for the client request
        var listener = (Socket)ar.AsyncState;
        var handler = listener.EndAccept(ar);

        //Create the state object
        var conn = new DialerConnection { WorkSocket = handler };
        Connections.Add(conn.Id, conn);
        Logging.LogEvent(string.Format("New connection accepted, connection ID '{0}'", conn.Id));

        NewClientConnected(conn);

        //Start receiving from the client
        _BeginReceiveFromClient(conn);
      }
      catch
      {
        //Uh...
      }
    }

    protected abstract void NewClientConnected(DialerConnection conn);

    private void _BeginReceiveFromClient(DialerConnection conn)
    {
      if (!conn.IsDisconnecting)
      {
        conn.WorkSocket.BeginReceive(conn.Buffer, 0, DialerConnection.ReadBufferSize, 0,
          new AsyncCallback(_ReceiveFromClient), conn);
      }
      else
      {
        _Disconnect(conn);
      }
    }

    private void _ReceiveFromClient(IAsyncResult ar)
    {
      try
      {
        //Retrieve the state object and the handler socket
        //  from the asynchronous state object
        var conn = (DialerConnection)ar.AsyncState;
        var socket = conn.WorkSocket;

        //Read data from the client socket
        var bytesRead = socket.EndReceive(ar);
        if (bytesRead > 0)
        {
          // There might be more data, so store the data received so far
          conn.Message.Append(Encoding.ASCII.GetString(conn.Buffer, 0, bytesRead));

          //Check for end-of-file tag; if not there, read more data
          var content = conn.Message.ToString();
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

            conn.Message.Length = 0;
            if (message.ToString().IndexOf((char)3) > -1)
            {
              conn.Message.Append(message.ToString());
            }

            _HandleClientMessages(conn, messages);
          }
          else
          {
            //Not all data received, so get more
            socket.BeginReceive(conn.Buffer, 0, DialerConnection.ReadBufferSize, 0,
              new AsyncCallback(_ReceiveFromClient), conn);
          }
        }
      }
      catch (SocketException ex)
      {
        Logging.LogEvent("Socket error receiving data from client: " + ex.ToString());

        _Disconnect((DialerConnection)ar.AsyncState);
      }
      catch (IOException ex)
      {
        //Something in the transport layer has failed, such as the network connection died
        Logging.LogEvent("I/O error receiving data from client: " + ex.ToString());

        _Disconnect((DialerConnection)ar.AsyncState);
      }
      catch (ObjectDisposedException ex)
      {
        //We've been disconnected
        Logging.LogEvent("Error receiving data from client: " + ex.ToString());

        _Disconnect((DialerConnection)ar.AsyncState);
      }
      catch (Exception ex)
      {
        Logging.LogEvent("Unexpected error receiving data from client: " + ex.ToString());

        Debugger.Break();
      }
    }

    private void _HandleClientMessages(DialerConnection conn, IEnumerable<string> data)
    {
      foreach (var msg in data)
      {
        Logging.LogEvent(string.Format("Receiving message ({0}): {1}", DateTime.Now, msg));

        HandleClientMessage(conn, msg);
      }

      //Prepare to receive the next message
      _BeginReceiveFromClient(conn);
    }

    protected abstract void HandleClientMessage(DialerConnection conn, string data);

    private void _Disconnect(DialerConnection conn)
    {
      if (conn != null)
      {
        if (Connections.ContainsKey(conn.Id))
        {
          Connections.Remove(conn.Id);
        }

        if (conn.WorkSocket != null && conn.WorkSocket.Connected)
        {
          conn.WorkSocket.Close(2);
        }
      }
    }
  }
}
