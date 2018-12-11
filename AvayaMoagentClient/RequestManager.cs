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
using System.Threading;
using AvayaMoagentClient.Requests;

namespace AvayaMoagentClient
{
  /// <summary>
  /// RequestManager
  /// </summary>
  public class RequestManager
  {
    private const int _DEFAULT_RESPONSE_TIMEOUT_MS = 10000;
    private List<Request> _requests = new List<Request>();
    private Thread _cleanupThread;

    /// <summary>
    /// Default constructor
    /// </summary>
    public RequestManager()
    {
      _cleanupThread = new Thread(new ThreadStart(_Cleanup));
      _cleanupThread.IsBackground = true;
      _cleanupThread.Start();
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Request Create(Request request)
    {
      lock (_requests)
      {
        _requests.Add(request);
      }

      return request;
    }

    /// <summary>
    /// GetFirst
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetFirst<T>() where T : Request
    {
      var ret = _requests.FirstOrDefault(t => t as T != null);

      return (T)ret;
    }

    /// <summary>
    /// MarkComplete
    /// </summary>
    /// <param name="request"></param>
    public void MarkComplete(Request request)
    {
      request.MarkComplete();
    }

    /// <summary>
    /// WaitForCompletion
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Request WaitForCompletion(Request request)
    {
      return WaitForCompletion(request, _DEFAULT_RESPONSE_TIMEOUT_MS);
    }

    /// <summary>
    /// WaitForCompletion
    /// </summary>
    /// <param name="request"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public Request WaitForCompletion(Request request, int timeoutMs)
    {
      var noTimeout = request.WaitHandle.WaitOne(timeoutMs);

      lock (_requests)
      {
        if (!request.CompletionTime.HasValue)
        {
          request.CompletionTime = DateTime.Now;
        }

        if (!request.IsComplete.HasValue)
        {
          request.IsComplete = noTimeout;
        }

        if (_requests.Contains(request))
        {
          _requests.Remove(request);
        }
      }

      if (!noTimeout)
      {
        throw new TimeoutException(string.Format("No response for {0} after {1}ms", request.GetType().Name, timeoutMs));
      }

      return request;
    }

    private void _Cleanup()
    {
      while (true)
      {
        lock (_requests)
        {
          _requests.RemoveAll(t => t.CompletionTime.HasValue && t.CompletionTime.Value < DateTime.Now.AddMinutes(-10));
        }

        Thread.Sleep(10000);
      }
    }
  }
}
