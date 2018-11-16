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
using System.Net;
using System.Net.Sockets;
using System.Text;
using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;
using OpenSSL;
using OpenSSL.Core;
using OpenSSL.X509;

namespace AvayaMoagentClient
{
  /// <summary>
  /// MoagentClient
  /// </summary>
  [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
  public class MoagentClient
  {
    private const string _DEFAULT_AVAYA_CLIENT_CERT_PATH = @".\agentClientCert.p12";
    private const string _DEFAULT_AVAYA_CERT_PATH = @".\ProactiveContactCA.cer";

    private readonly string _server;
    private readonly int _port;
    private readonly bool _useSsl;
    private readonly Socket _client;

    private int _nextInvokeId = 1;

    private SslStream _secureClient;

    /// <summary>
    /// Creates a non-SSL MoagentClient object for the specified host and port.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public MoagentClient(string host, int port)
      : this(host, port, false)
    {
    }

    /// <summary>
    /// Creates a MoagentClient object for the specified host, port, and SSL setting.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="useSsl"></param>
    public MoagentClient(string host, int port, bool useSsl)
    {
      X509Certificate clientCert;
      X509Certificate serverCert;

      _server = host;
      _port = port;
      _useSsl = useSsl;
      AvayaCertificatePath = _DEFAULT_AVAYA_CERT_PATH;
      AvayaClientCertificatePath = _DEFAULT_AVAYA_CLIENT_CERT_PATH;

      try
      {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        if (_useSsl)
        {
          try
          {
            var certBio = BIO.File(AvayaClientCertificatePath, "r");
            clientCert = X509Certificate.FromPKCS12(certBio, string.Empty);
          }
          catch (Exception ex)
          {
            throw new FileLoadException("Unable to load Avaya client certificate file", AvayaClientCertificatePath, ex);
          }

          try
          {
            var serverBio = BIO.File(AvayaCertificatePath, "r");
            serverCert = X509Certificate.FromDER(serverBio);
          }
          catch (Exception ex)
          {
            throw new FileLoadException("Unable to load Avaya certificate file", AvayaCertificatePath, ex);
          }

          CertificateList = new X509List { clientCert };
          CertificateChain = new X509Chain { serverCert };
        }
      }
      catch (TypeInitializationException tex)
      {
        throw tex.InnerException;
      }
    }

    /// <summary>
    /// MessageSentHandler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageSentHandler(object sender, MessageSentEventArgs e);

    /// <summary>
    /// MessageReceivedHandler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageReceivedHandler(object sender, MessageReceivedEventArgs e);

    /// <summary>
    /// DisconnectedHandler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DisconnectedHandler(object sender, EventArgs e);

    /// <summary>
    /// Connected
    /// </summary>
    public event EventHandler Connected;

    /// <summary>
    /// MessageSent
    /// </summary>
    public event MessageSentHandler MessageSent;

    /// <summary>
    /// MessageReceived
    /// </summary>
    public event MessageReceivedHandler MessageReceived;

    /// <summary>
    /// Disconnected
    /// </summary>
    public event DisconnectedHandler Disconnected;

    /// <summary>
    /// CertificateChain
    /// </summary>
    [CLSCompliant(false)]
    public X509Chain CertificateChain { get; private set; }

    /// <summary>
    /// CertificateList
    /// </summary>
    [CLSCompliant(false)]
    public X509List CertificateList { get; private set; }

    /// <summary>
    /// AvayaClientCertificatePath
    /// </summary>
    public string AvayaClientCertificatePath { get; set; }

    /// <summary>
    /// AvayaCertificatePath
    /// </summary>
    public string AvayaCertificatePath { get; set; }

    /// <summary>
    /// IsConnected
    /// </summary>
    public bool IsConnected
    {
      get
      {
        var ret = false;

        if (_useSsl)
        {
          ret = (_secureClient != null);
        }
        else
        {
          ret = (_client != null && _client.Connected);
        }

        return ret;
      }
    }

    /// <summary>
    /// StartConnect
    /// </summary>
    public void StartConnect()
    {
      var ip = IPAddress.Parse(_server);
      var remoteEp = new IPEndPoint(ip, _port);

      _client.BeginConnect(remoteEp, _ConnectCallback, _client);
    }

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="cmd"></param>
    public void Send(Commands.Command cmd)
    {
      var msg = Message.FromCommand(cmd);
      msg.InvokeId = (_nextInvokeId++).ToString();

      Send(msg);
    }

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="msg"></param>
    public void Send(Message msg)
    {
      byte[] byteData = Encoding.ASCII.GetBytes(msg.RawMessage);

      // Begin sending the data to the remote device.
      if (_useSsl)
      {
        _secureClient.BeginWrite(byteData, 0, byteData.Length, _SecureSendCallback, msg);
      }
      else
      {
        _client.BeginSend(byteData, 0, byteData.Length, 0, _SendCallback, msg);
      }
    }

    /// <summary>
    /// Disconnect
    /// </summary>
    public void Disconnect()
    {
      if (_secureClient != null)
      {
        _secureClient.Close();
        _secureClient.Dispose();
        _secureClient = null;
      }

      if (_client.Connected)
      {
        _client.Close();
      }

      if (Disconnected != null)
      {
        Disconnected(this, EventArgs.Empty);
      }
    }

    private void _ConnectCallback(IAsyncResult ar)
    {
      var client = (Socket)ar.AsyncState;
      client.EndConnect(ar);

      if (_useSsl)
      {
        var stream = new NetworkStream(_client, FileAccess.ReadWrite, true);
        _secureClient = new SslStream(stream, false, _ValidateRemoteCert, _ClientCertificateSelectionCallback);

        _secureClient.AuthenticateAsClient(_server, CertificateList, CertificateChain, SslProtocols.Default, SslStrength.All, false);

        _SecureReceive(_secureClient);
      }
      else
      {
        _Receive(client);
      }

      if (Connected != null)
      {
        Connected(this, EventArgs.Empty);
      }
    }

    private void _Receive(Socket client)
    {
      try
      {
        var state = new StateObject { Stream = client };
        client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, _ReceiveCallback, state);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    private void _SecureReceive(SslStream client)
    {
      try
      {
        var state = new StateObject { SecureStream = client };

        client.BeginRead(state.Buffer, 0, StateObject.BufferSize, _SecureReceiveCallback, state);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    private void _ReceiveCallback(IAsyncResult ar)
    {
      string content;

      try
      {
        // Retrieve the state object and the client socket 
        // from the asynchronous state object.
        var state = (StateObject)ar.AsyncState;
        var handler = state.Stream;
        Message lastMsg = null;

        if (handler.Connected)
        {
          // Read data from the remote device.
          int bytesRead = handler.EndReceive(ar);

          if (bytesRead > 0)
          {
            // There  might be more data, so store the data received so far.
            state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read more data.
            content = state.StringBuilder.ToString();
            if (content.IndexOf((char)3) > -1)
            {
              var msgs = new List<string>();
              var msg = new StringBuilder();

              foreach (var ch in content)
              {
                if (ch != (char)3)
                {
                  msg.Append(ch);
                }
                else
                {
                  msg.Append(ch);
                  msgs.Add(msg.ToString());
                  msg.Length = 0;
                }
              }

              state.StringBuilder.Length = 0;
              state.StringBuilder.Append(msg.ToString());

              lastMsg = _LogMessagesReceived(msgs);
            }
          }

          if (!(lastMsg != null &&
                lastMsg.Type == MessageType.Response &&
                lastMsg.Command.Trim() == Commands.Logoff.Default.Cmd))
          {
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, _ReceiveCallback, state);
          }
        }
      }
      catch (IOException)
      {
        //something in the transport leyer has failed, such as the network connection died
        //TODO: log the exception details?
        Disconnect();
      }
      catch (ObjectDisposedException)
      {
        //we've been disconnected
        //TODO: log the exception details?
        Disconnect();
      }
      catch (Exception)
      {
        Debugger.Break();
      }
    }

    private void _SecureReceiveCallback(IAsyncResult ar)
    {
      string content;

      try
      {
        // Retrieve the state object and the client socket 
        // from the asynchronous state object.
        var state = (StateObject)ar.AsyncState;
        var handler = state.SecureStream;
        Message lastMsg = null;

        if (handler.CanRead)
        {
          // Read data from the remote device.
          int bytesRead = handler.EndRead(ar);
          ////int bytesRead = handler.EndReceive(ar);

          if (bytesRead > 0)
          {
            // There might be more data, so store the data received so far.
            state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read more data.
            content = state.StringBuilder.ToString();
            if (content.IndexOf((char)3) > -1)
            {
              var msgs = new List<string>();
              var msg = new StringBuilder();

              foreach (var ch in content)
              {
                if (ch != (char)3)
                {
                  msg.Append(ch);
                }
                else
                {
                  msg.Append(ch);
                  msgs.Add(msg.ToString());
                  msg.Length = 0;
                }
              }

              state.StringBuilder.Length = 0;
              state.StringBuilder.Append(msg.ToString());

              lastMsg = _LogMessagesReceived(msgs);
            }
          }

          if (!(lastMsg != null &&
                lastMsg.Type == MessageType.Response &&
                lastMsg.Command.Trim() == Commands.Logoff.Default.Cmd))
          {
            handler.BeginRead(state.Buffer, 0, StateObject.BufferSize, _SecureReceiveCallback, state);
          }
        }
      }
      catch (IOException)
      {
        //something in the transport leyer has failed, such as the network connection died
        //TODO: log the exception details?
        Disconnect();
      }
      catch (ObjectDisposedException)
      {
        //we've been disconnected
        //TODO: log the exception details?
        Disconnect();
      }
      catch (Exception)
      {
        Debugger.Break();
      }
    }

    private Message _LogMessagesReceived(IEnumerable<string> msgs)
    {
      Message lastMsg = null;

      foreach (var msg in msgs)
      {
        lastMsg = Message.ParseMessage(msg);
        if (MessageReceived != null)
        {
          MessageReceived(this, new MessageReceivedEventArgs { Message = lastMsg });
        }
      }

      return lastMsg;
    }

    private void _SendCallback(IAsyncResult ar)
    {
      try
      {
        var message = (Message)ar.AsyncState;
        _client.EndSend(ar);

        if (MessageSent != null)
        {
          MessageSent(this, new MessageSentEventArgs { Message = message });
        }
      }
      catch (IOException)
      {
        //something in the transport leyer has failed, such as the network connection died
        //TODO: log the exception details?
        Disconnect();
      }
      catch (ObjectDisposedException)
      {
        //we've been disconnected
        //TODO: log the exception details?
        Disconnect();
      }
      catch (Exception)
      {
        Debugger.Break();
      }
    }

    private void _SecureSendCallback(IAsyncResult ar)
    {
      try
      {
        var message = (Message)ar.AsyncState;
        _secureClient.EndWrite(ar);

        if (MessageSent != null)
        {
          MessageSent(this, new MessageSentEventArgs { Message = message });
        }
      }
      catch (IOException)
      {
        //something in the transport layer has failed, such as the network connection died
        //TODO: log the exception details?
        Disconnect();
      }
      catch (ObjectDisposedException)
      {
        //we've been disconnected
        //TODO: log the exception details?
        Disconnect();
      }
      catch (Exception)
      {
        Debugger.Break();
      }
    }

    private bool _ValidateRemoteCert(object obj, X509Certificate cert, X509Chain chain, int depth, VerifyResult result)
    {
      bool ret = false;

      switch (result)
      {
        case VerifyResult.X509_V_ERR_CERT_UNTRUSTED:
        case VerifyResult.X509_V_ERR_UNABLE_TO_GET_ISSUER_CERT:
        case VerifyResult.X509_V_ERR_UNABLE_TO_GET_ISSUER_CERT_LOCALLY:
        case VerifyResult.X509_V_ERR_UNABLE_TO_VERIFY_LEAF_SIGNATURE:
          {
            // Check the chain to see if there is a match for the cert
            ret = _CheckCert(cert, chain);
            if (!ret && depth != 0)
            {
              ret = true;  // We want to keep checking until we get to depth 0
            }
          }

          break;
        case VerifyResult.X509_V_ERR_ERROR_IN_CERT_NOT_BEFORE_FIELD:
        case VerifyResult.X509_V_ERR_CERT_NOT_YET_VALID:
          {
            Console.WriteLine("Certificate is not valid yet");
            ret = false;
          }

          break;
        case VerifyResult.X509_V_ERR_CERT_HAS_EXPIRED:
        case VerifyResult.X509_V_ERR_ERROR_IN_CERT_NOT_AFTER_FIELD:
          {
            Console.WriteLine("Certificate is expired");
            ret = false;
          }

          break;
        case VerifyResult.X509_V_ERR_DEPTH_ZERO_SELF_SIGNED_CERT:
          {
            // we received a self signed cert - check to see if it's in our store
            ret = _CheckCert(cert, chain);
          }

          break;
        case VerifyResult.X509_V_ERR_SELF_SIGNED_CERT_IN_CHAIN:
          {
            // A self signed certificate was encountered in the chain
            // Check the chain to see if there is a match for the cert
            ret = _CheckCert(cert, chain);
            if (!ret && depth != 0)
            {
              ret = true;  // We want to keep checking until we get to depth 0
            }
          }
          
          break;
        case VerifyResult.X509_V_OK:
          {
            ret = true;
          }

          break;
      }

      return ret;
    }

    private X509Certificate _ClientCertificateSelectionCallback(object sender, string targetHost, X509List localCerts, X509Certificate remoteCert, string[] acceptableIssuers)
    {
      X509Certificate retCert = null;

      //// check target host?

      for (var i = 0; i < acceptableIssuers.GetLength(0); i++)
      {
        var name = new X509Name(acceptableIssuers[i]);

        foreach (X509Certificate cert in localCerts)
        {
          if (cert.Issuer.CompareTo(name) == 0)
          {
            retCert = cert;
            break;
          }

          cert.Dispose();
        }

        name.Dispose();
      }

      return retCert;
    }

    private bool _CheckCert(X509Certificate cert, X509Chain chain)
    {
      if (cert == null || chain == null)
      {
        return false;
      }

      foreach (X509Certificate certificate in chain)
      {
        if (cert == certificate)
        {
          return true;
        }
      }

      return false;
    }
  }
}
