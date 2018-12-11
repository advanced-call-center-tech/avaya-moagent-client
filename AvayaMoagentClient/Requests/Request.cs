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
using AvayaMoagentClient.Enumerations;

namespace AvayaMoagentClient.Requests
{
  /// <summary>
  /// Request
  /// </summary>
  public class Request
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public Request()
    {
      StartTime = DateTime.Now;
      WaitHandle = new ManualResetEvent(false);
    }

    /// <summary>
    /// StartTime
    /// </summary>
    public DateTime StartTime { get; private set; }

    /// <summary>
    /// WaitHandle
    /// </summary>
    public ManualResetEvent WaitHandle { get; private set; }

    /// <summary>
    /// CompletionTime
    /// </summary>
    public DateTime? CompletionTime { get; set; }

    /// <summary>
    /// IsComplete
    /// </summary>
    public bool? IsComplete { get; set; }

    /// <summary>
    /// ErrorCode
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// RequestError
    /// </summary>
    public RequestError Error { get; set; }

    /// <summary>
    /// IsError
    /// </summary>
    public bool IsError
    {
      get
      {
        return (Error != RequestError.None) || !string.IsNullOrEmpty(ErrorCode);
      }
    }

    /// <summary>
    /// MarkComplete
    /// </summary>
    public void MarkComplete()
    {
      CompletionTime = DateTime.Now;
      IsComplete = true;

      WaitHandle.Set();
    }
  }
}
