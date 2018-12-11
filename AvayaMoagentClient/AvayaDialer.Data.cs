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
using AvayaMoagentClient.Messages;
using AvayaMoagentClient.Requests;

namespace AvayaMoagentClient
{
  /// <summary>
  /// AvayaDialer
  /// </summary>
  public partial class AvayaDialer
  {
    private void _HandleDataMessage(Message msg)
    {
      switch (msg.Command)
      {
        case AvayaMoagentClient.Commands.ListState.Name:
          {
            if (msg.Contents.Count > 1)
            {
              var state = msg.Contents[1];
              if (state.Contains(","))
              {
                var parts = state.Split(',');
                JobName = parts[1];
                state = parts[0];
              }

              switch (state)
              {
                case _STATUS_ON_JOB_ON_CALL:
                  {
                    AgentState = Enumerations.AgentState.OnCall;
                    break;
                  }

                case _STATUS_ON_JOB_READY:
                  {
                    AgentState = Enumerations.AgentState.Ready;
                    break;
                  }

                case _STATUS_ON_JOB_IDLE_NOT_READY:
                  {
                    AgentState = Enumerations.AgentState.Available;
                    break;
                  }

                case _STATUS_ON_JOB_UNAVAILABLE:
                  {
                    AgentState = Enumerations.AgentState.Attached;
                    break;
                  }

                case _STATUS_NOT_ON_JOB:
                  {
                    AgentState = Enumerations.AgentState.Idle;
                    break;
                  }

                default:
                  {
                    //Uh...do nothing...
                    break;
                  }
              }
            }

            var joinJobRequestPending = (_requestManager.GetFirst<JoinJobRequest>() != null);
            if (joinJobRequestPending)
            {
              switch (msg.Code)
              {
                case _STATUS_ON_JOB_ON_CALL:
                case _STATUS_ON_JOB_READY:
                  {
                    var request = _requestManager.GetFirst<JoinJobRequest>();
                    if (request != null)
                    {
                      //At this point we'll consider the request complete
                      request.MarkComplete();
                    }

                    _client.Send(AvayaMoagentClient.Commands.NoFurtherWork.Default);

                    break;
                  }

                case _STATUS_ON_JOB_IDLE_NOT_READY:
                  {
                    _client.Send(AvayaMoagentClient.Commands.NoFurtherWork.Default);

                    break;
                  }

                case _STATUS_ON_JOB_UNAVAILABLE:
                  {
                    _client.Send(AvayaMoagentClient.Commands.DetachJob.Default);

                    break;
                  }

                case _STATUS_NOT_ON_JOB:
                  {
                    if (_requestManager.GetFirst<LogoffRequest>() != null)
                    {
                      _client.Send(AvayaMoagentClient.Commands.DisconnectHeadset.Default);
                    }

                    _client.Send(AvayaMoagentClient.Commands.ListJobs.All);

                    break;
                  }

                default:
                  {
                    //Do nothing
                    break;
                  }
              }
            }

            break;
          }

        case AvayaMoagentClient.Commands.ListJobs.Name:
          {
            if (msg.Contents.Count >= 3)
            {
              var jobs = _ParseJobs(msg.Contents);
              var request = _requestManager.GetFirst<GetJobsRequest>();
              if (request != null)
              {
                request.Jobs.AddRange(jobs);
                request.MarkComplete();
              }

              if (!string.IsNullOrEmpty(_nextJobName))
              {
                var job = jobs.FirstOrDefault(t => t.JobName == _nextJobName);

                if (job != null)
                {
                  _requestManager.Create(new JoinJobRequest(job, BlendMode.Blend, CallFields, true));
                }
              }
              else
              {
                var joinJobRequest = _requestManager.GetFirst<JoinJobRequest>();
                if (joinJobRequest != null)
                {
                  if (joinJobRequest.Job.JobStatus == JobStatus.Active)
                  {
                    JobName = joinJobRequest.Job.JobName;
                    _client.Send(new AvayaMoagentClient.Commands.SetWorkClass(_GetWorkClass(joinJobRequest)));
                  }
                  else
                  {
                    AgentState = Enumerations.AgentState.Idle;

                    joinJobRequest.Error = RequestError.JobNotActive;
                    joinJobRequest.MarkComplete();
                  }
                }
              }
            }

            break;
          }

        case AvayaMoagentClient.Commands.ManagedCall.Name:
          {
            var request = _requestManager.GetFirst<ManagedDialRequest>();
            if (request != null)
            {
              AgentState = Enumerations.AgentState.OnCall;

              var result = msg.Contents[2];
              if (result.Contains("CONNECT"))
              {
                request.Result = DialResult.Connected;
              }
              else
              {
                JobStage = Enumerations.JobStage.Wrap;
              }
            }

            break;
          }

        default:
          {
            //Uh...I don't know...
            break;
          }
      }
    }

    private WorkClass _GetWorkClass(JoinJobRequest request)
    {
      var ret = request.Job.WorkClass;

      if (request.Job.WorkClass == WorkClass.Blend)
      {
        switch (request.BlendMode)
        {
          case BlendMode.Blend:
            {
              ret = WorkClass.Blend;
              break;
            }

          case BlendMode.Inbound:
            {
              ret = WorkClass.Inbound;
              break;
            }

          case BlendMode.Outbound:
            {
              ret = WorkClass.Outbound;
              break;
            }

          default:
            {
              ret = WorkClass.Blend;
              break;
            }
        }
      }

      return ret;
    }

    private List<Job> _ParseJobs(List<string> contents)
    {
      var ret = new List<Job>();

      for (int i = 2; i < contents.Count; i++)
      {
        var workClass = WorkClass.Undefined;
        JobStatus jobStatus;
        var raw = contents[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        switch (raw[0])
        {
          case "I":
            {
              workClass = WorkClass.Inbound;
              break;
            }

          case "O":
            {
              workClass = WorkClass.Outbound;
              break;
            }

          case "B":
            {
              workClass = WorkClass.Blend;
              break;
            }

          case "M":
            {
              workClass = WorkClass.Managed;
              break;
            }

          default:
            {
              break;
            }
        }

        switch (raw[2])
        {
          case "I":
            {
              jobStatus = JobStatus.Inactive;
              break;
            }

          case "A":
            {
              jobStatus = JobStatus.Active;
              break;
            }

          default:
            {
              //Uh...I guess it's not active...
              jobStatus = JobStatus.Inactive;
              break;
            }
        }

        ret.Add(new Job(raw[1], workClass, jobStatus));
      }

      return ret;
    }
  }
}
