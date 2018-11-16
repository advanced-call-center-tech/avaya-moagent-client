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

using AvayaMoagentClient.Enumerations;
using AvayaMoagentClient.Messages;

namespace AvayaMoagentClient.Commands
{
  /// <summary>
  /// ListJobs
  /// </summary>
  public class ListJobs : Command
  {
    private const string _COMMAND = "AGTListJobs";

    static ListJobs()
    {
      All = new ListJobs(JobListingType.All);
    }

    /// <summary>
    /// Creates a ListJob command of the specified type.
    /// </summary>
    /// <param name="type"></param>
    public ListJobs(JobListingType type)
      : base(_COMMAND, ((char)type).ToString())
    {
    }

    /// <summary>
    /// Creates a ListJob command with the specified type and status.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="status"></param>
    public ListJobs(JobListingType type, JobStatus status)
      : base(_COMMAND, ((char)type).ToString(), ((char)status).ToString())
    {
    }

    /// <summary>
    /// All
    /// </summary>
    public static ListJobs All { get; private set; }
  }
}
