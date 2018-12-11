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

namespace AvayaMoagentClient.Requests
{
  /// <summary>
  /// JoinJobRequest
  /// </summary>
  public class JoinJobRequest : Request
  {
    /// <summary>
    /// Creates a JoinJobRequest object with the specified job and call fields.
    /// </summary>
    /// <param name="job"></param>
    /// <param name="blendMode"></param>
    /// <param name="callFields"></param>
    /// <param name="goReady"></param>
    public JoinJobRequest(Job job, BlendMode blendMode, List<string> callFields, bool goReady)
    {
      Job = job;
      BlendMode = blendMode;
      CallFields = callFields;
      GoReady = goReady;
      
      NotifyFields = new Queue<KeyValuePair<FieldListType, string>>();
      DataFields = new Queue<KeyValuePair<FieldListType, string>>();
    }

    /// <summary>
    /// Job
    /// </summary>
    public Job Job { get; private set; }

    /// <summary>
    /// BlendMode
    /// </summary>
    public BlendMode BlendMode { get; private set; }

    /// <summary>
    /// CallFields
    /// </summary>
    public List<string> CallFields { get; private set; }

    /// <summary>
    /// Indicates whether to go ready immediately after attaching to the job.
    /// </summary>
    public bool GoReady { get; private set; }

    internal Queue<KeyValuePair<FieldListType, string>> NotifyFields { get; private set; }

    internal Queue<KeyValuePair<FieldListType, string>> DataFields { get; private set; }
  }
}
